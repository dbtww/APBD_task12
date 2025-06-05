using APBD_tutorial12.Data;
using APBD_tutorial12.Services;
using Microsoft.EntityFrameworkCore;

public class ClientService : IClientService
{
    private readonly SbdContext _context;

    public ClientService(SbdContext context)
    {
        _context = context;
    }

    public async Task DeleteClientAsync(int idClient)
    {
        var hasTrips = await _context.ClientTrips.AnyAsync(ct => ct.IdClient == idClient);
        if (hasTrips) throw new Exception("Client is assigned to a trip");

        var client = await _context.Clients.FindAsync(idClient);
        if (client == null) throw new Exception("Client not found");

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
    }
}