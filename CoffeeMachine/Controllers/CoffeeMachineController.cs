using Microsoft.AspNetCore.Mvc;
using CoffeeMachine.Models;

namespace CoffeeMachine.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoffeeMachineController : ControllerBase
    {
        private readonly ILogger<CoffeeMachineController> _logger;
        private static int _counter = 0;

        public CoffeeMachineController(ILogger<CoffeeMachineController> logger)
        {
            _logger = logger;
        }

        [HttpGet("brew-coffee")]
        public IActionResult BrewCoffee()
        {
            try
            {
                _counter++;

                // Requirement #3
                if (SystemTime.Now().Month == 4 && SystemTime.Now().Day == 1) 
                {
                    _logger.LogInformation("April 1st, Teapot.");
                    return StatusCode(418);
                }

                // Requirement #2
                if (_counter % 5 ==  0)
                {
                    _logger.LogInformation("Coffee machine is out of coffee");
                    return StatusCode(503);
                }

                // Requirement #1
                var res = new Response
                {
                    Message = "Your piping hot coffee is ready",
                    Prepared = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                };
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred");
                return StatusCode(500);
            }
        }
    }

    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.Now;

        public static void Reset() => Now = () => DateTime.Now;
    }
}
