using Microsoft.AspNetCore.Mvc;
using User.Application.Services;
using User.Domain.Models.Entities;
using User.Domain.Models.Requests;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost]
        public async Task<ActionResult<Client>> CreateClient([FromBody] CreateClientRequest request)
        {
            var client = await _clientService.CreateClientAsync(request);
            return CreatedAtAction(nameof(GetClientById), new { id = client.Id }, client);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetAllClients()
        {
            var clients = await _clientService.GetAllClientsAsync();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClientById(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
                return NotFound();
            return Ok(client);
        }

        [HttpGet("{id}/address")]
        public async Task<ActionResult<Address>> GetAddressForClient(int id)
        {
            var address = await _clientService.GetAddressForClientAsync(id);
            if (address == null)
                return NotFound();
            return Ok(address);
        }

        [HttpPut("{id}/address")]
        public async Task<ActionResult<Address>> UpdateAddressForClient(int id, [FromBody] Address address)
        {
            var updated = await _clientService.UpdateAddressForClientAsync(id, address);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var result = await _clientService.DeleteClientAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}/address")]
        public async Task<IActionResult> DeleteAddressForClient(int id)
        {
            var result = await _clientService.DeleteAddressForClientAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
} 