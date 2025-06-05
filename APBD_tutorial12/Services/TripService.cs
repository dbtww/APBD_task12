using APBD_tutorial12.Data;
using APBD_tutorial12.DTO;
using APBD_tutorial12.Models;
using APBD_tutorial12.Services;
using Microsoft.EntityFrameworkCore;

public class TripService : ITripService
{
    private readonly SbdContext _context;

    public TripService(SbdContext context)
    {
        _context = context;
    }

    public async Task<List<Trip>> GetTripsAsync(int page, int pageSize)
    {
        return await _context.Trips
            .Include(t => t.IdCountries)
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task AddClientToTripAsync(int idTrip, CreateClientDTO dto)
    {
        var trip = await _context.Trips.FindAsync(idTrip);
        if (trip == null || trip.DateFrom < DateTime.Now)
            throw new Exception("Trip not found or already started");

        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == dto.Pesel);
        if (client == null)
        {
            client = new Client
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Telephone = dto.Telephone,
                Pesel = dto.Pesel
            };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        var exists = await _context.ClientTrips
            .AnyAsync(ct => ct.IdClient == client.IdClient && ct.IdTrip == idTrip);
        if (exists) throw new Exception("Client already registered");

        var newCT = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = dto.PaymentDate
        };
        _context.ClientTrips.Add(newCT);
        await _context.SaveChangesAsync();
    }
}