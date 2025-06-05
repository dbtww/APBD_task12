using APBD_tutorial12.DTO;
using APBD_tutorial12.Models;

namespace APBD_tutorial12.Services;

public interface ITripService
{
    Task<List<Trip>> GetTripsAsync(int page, int pageSize);
    Task AddClientToTripAsync(int idTrip, CreateClientDTO dto);
}

