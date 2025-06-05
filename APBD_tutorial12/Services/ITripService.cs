using APBD_tutorial12.DTO;

namespace APBD_tutorial12.Services;

public interface ITripService
{
    Task<List<TripDTO>> GetTripsAsync(int page, int pageSize);
    Task AddClientToTripAsync(int idTrip, CreateClientDTO dto);
}
