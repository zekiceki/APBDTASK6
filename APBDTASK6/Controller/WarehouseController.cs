using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService ?? throw new ArgumentNullException(nameof(warehouseService));
        }

        [HttpPost("main-scenario")]
        public async Task<IActionResult> MainScenario([FromBody] Request request)
        {
            try
            {
                var result = await _warehouseService.MainScenarioAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in main scenario: {ex.Message}");
            }
        }

        [HttpPost("second-scenario")]
        public async Task<IActionResult> SecondScenario([FromBody] Request request)
        {
            try
            {
                var result = await _warehouseService.SecondScenarioAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in second scenario: {ex.Message}");
            }
        }
    }
}