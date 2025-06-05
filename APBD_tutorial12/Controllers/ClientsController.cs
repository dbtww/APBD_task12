using APBD_tutorial12.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_tutorial12.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        try
        {
            await _clientService.DeleteClientAsync(idClient);
            return Ok("Client deleted.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}