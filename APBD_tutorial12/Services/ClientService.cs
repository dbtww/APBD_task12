using Microsoft.Data.SqlClient;

namespace APBD_tutorial12.Services;

public class ClientService : IClientService
{
    private readonly string _connectionString;

    public ClientService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default")!;
    }

    public async Task DeleteClientAsync(int idClient)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        var checkCmd = new SqlCommand("SELECT COUNT(1) FROM Client_Trip WHERE IdClient = @id", conn);
        checkCmd.Parameters.AddWithValue("@id", idClient);

        if ((int)await checkCmd.ExecuteScalarAsync() > 0)
            throw new Exception("Client is assigned to a trip and cannot be deleted");

        var deleteCmd = new SqlCommand("DELETE FROM Client WHERE IdClient = @id", conn);
        deleteCmd.Parameters.AddWithValue("@id", idClient);
        await deleteCmd.ExecuteNonQueryAsync();
    }
}