using APBD_tutorial12.DTO;
using Microsoft.Data.SqlClient;

namespace APBD_tutorial12.Services;

public class TripService : ITripService
{
    private readonly string _connectionString;

    public TripService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default")!;
    }

    public async Task<List<TripDTO>> GetTripsAsync(int page, int pageSize)
    {
        var trips = new List<TripDTO>();
        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        string tripQuery = @"
        SELECT IdTrip, Name, Description, DateFrom, DateTo, MaxPeople
        FROM Trip
        ORDER BY DateFrom DESC
        OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

        using var tripCmd = new SqlCommand(tripQuery, conn);
        tripCmd.Parameters.AddWithValue("@skip", (page - 1) * pageSize);
        tripCmd.Parameters.AddWithValue("@take", pageSize);

        using var reader = await tripCmd.ExecuteReaderAsync();
        var tripIds = new List<int>();

        while (await reader.ReadAsync())
        {
            var trip = new TripDTO
            {
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                DateFrom = reader.GetDateTime(3),
                DateTo = reader.GetDateTime(4),
                MaxPeople = reader.GetInt32(5),
                Countries = new List<CountryDTO>(),
                Clients = new List<ClientDTO>()
            };

            trips.Add(trip);
            tripIds.Add(reader.GetInt32(0));
        }

        await reader.CloseAsync();

        if (!tripIds.Any()) return trips;
        
        string countryQuery = @"
        SELECT ct.IdTrip, c.Name
        FROM Country_Trip ct
        JOIN Country c ON ct.IdCountry = c.IdCountry
        WHERE ct.IdTrip IN (" + string.Join(",", tripIds) + ")";

        using var countryCmd = new SqlCommand(countryQuery, conn);
        using var cReader = await countryCmd.ExecuteReaderAsync();

        while (await cReader.ReadAsync())
        {
            int tripId = cReader.GetInt32(0);
            string name = cReader.GetString(1);

            var trip = trips.First(t => t.Name == trips[tripIds.IndexOf(tripId)].Name);
            trip.Countries.Add(new CountryDTO { Name = name });
        }

        await cReader.CloseAsync();
        
        string clientQuery = @"
        SELECT ct.IdTrip, cl.FirstName, cl.LastName
        FROM Client_Trip ct
        JOIN Client cl ON ct.IdClient = cl.IdClient
        WHERE ct.IdTrip IN (" + string.Join(",", tripIds) + ")";

        using var clientCmd = new SqlCommand(clientQuery, conn);
        using var clReader = await clientCmd.ExecuteReaderAsync();

        while (await clReader.ReadAsync())
        {
            int tripId = clReader.GetInt32(0);
            string firstName = clReader.GetString(1);
            string lastName = clReader.GetString(2);

            var trip = trips.First(t => t.Name == trips[tripIds.IndexOf(tripId)].Name);
            trip.Clients.Add(new ClientDTO { FirstName = firstName, LastName = lastName });
        }

        return trips;
    }

    public async Task AddClientToTripAsync(int idTrip, CreateClientDTO dto)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        using var tran = conn.BeginTransaction();

        try
        {
            var checkTrip = new SqlCommand("SELECT DateFrom FROM Trip WHERE IdTrip = @id", conn, tran);
            checkTrip.Parameters.AddWithValue("@id", idTrip);
            var dateObj = await checkTrip.ExecuteScalarAsync();
            if (dateObj == null) throw new Exception("Trip not found");
            if ((DateTime)dateObj < DateTime.Now) throw new Exception("Trip already started");
            
            var checkClient = new SqlCommand("SELECT IdClient FROM Client WHERE Pesel = @pesel", conn, tran);
            checkClient.Parameters.AddWithValue("@pesel", dto.Pesel);
            var clientIdObj = await checkClient.ExecuteScalarAsync();

            int clientId;

            if (clientIdObj == null)
            {
                var insertClient = new SqlCommand(@"
                INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
                OUTPUT INSERTED.IdClient
                VALUES (@fn, @ln, @em, @tel, @pesel)", conn, tran);

                insertClient.Parameters.AddWithValue("@fn", dto.FirstName);
                insertClient.Parameters.AddWithValue("@ln", dto.LastName);
                insertClient.Parameters.AddWithValue("@em", dto.Email);
                insertClient.Parameters.AddWithValue("@tel", dto.Telephone);
                insertClient.Parameters.AddWithValue("@pesel", dto.Pesel);

                clientId = (int)(await insertClient.ExecuteScalarAsync())!;
            }
            else
            {
                clientId = (int)clientIdObj;
                
                var check = new SqlCommand("SELECT COUNT(1) FROM Client_Trip WHERE IdClient = @cid AND IdTrip = @tid", conn, tran);
                check.Parameters.AddWithValue("@cid", clientId);
                check.Parameters.AddWithValue("@tid", idTrip);
                if ((int)await check.ExecuteScalarAsync() > 0)
                    throw new Exception("Client already registered for this trip");
            }

            var insertLink = new SqlCommand(@"
            INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt, PaymentDate)
            VALUES (@cid, @tid, @now, @pay)", conn, tran);
            insertLink.Parameters.AddWithValue("@cid", clientId);
            insertLink.Parameters.AddWithValue("@tid", idTrip);
            insertLink.Parameters.AddWithValue("@now", DateTime.Now);
            insertLink.Parameters.AddWithValue("@pay", (object?)dto.PaymentDate ?? DBNull.Value);
            await insertLink.ExecuteNonQueryAsync();

            await tran.CommitAsync();
        }
        catch
        {
            await tran.RollbackAsync();
            throw;
        }
    }
}
