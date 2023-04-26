//#define USE_ACTORMODEL

/* 
This controller contains 2 implementations of the TrafficControl functionality: a basic 
implementation and an actor-model based implementation.

The code for the basic implementation is in this controller. The actor-model implementation 
resides in the Vehicle actor (./Actors/VehicleActor.cs).

To switch between the two implementations, you need to use the USE_ACTORMODEL symbol at
the top of this file. If you comment the #define USE_ACTORMODEL statement, the basic 
implementation is used. Uncomment this statement to use the Actormodel implementation.
*/

using Insurance.Models;
using Microsoft.Extensions.Logging;

namespace Insurance.Controllers;

[ApiController]
[Route("")]
public class InsuranceController : ControllerBase
{
    private readonly ILogger<InsuranceController> _logger;

    public InsuranceController(
        ILogger<InsuranceController> logger)
    {
        _logger = logger;
    }

    [HttpPost("verify")]
    public async Task<ActionResult> VehicleEntryAsync(VerificationRequest msg)
    {
        try
        {
            _logger.LogInformation($"Verifying car {msg.VIN}.");

            await Task.Delay(TimeSpan.FromSeconds(1));

            return Ok(new VerificationResponse(ValidUntil: DateTime.Now));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing msg");
            return StatusCode(500);
        }
    }
}
