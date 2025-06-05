using APBD_tutorial12.DTO;
using APBD_tutorial12.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_tutorial12.Controllers;

[ApiController]
[Route("api/trips")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService)
    {
        _tripService = tripService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var trips = await _tripService.GetTripsAsync(page, pageSize);
        return Ok(trips);
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AddClientToTrip(int idTrip, [FromBody] CreateClientDTO dto)
    {
        try
        {
            await _tripService.AddClientToTripAsync(idTrip, dto);
            return Ok("Client added to trip.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}