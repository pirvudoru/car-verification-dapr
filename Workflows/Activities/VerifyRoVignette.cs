using Dapr.Workflow;
using Microsoft.Extensions.Logging;
using Workflows.Models;

namespace Workflows.Activities;

class VerifyRoVignette : WorkflowActivity<VerifyRoVignetteRequest, VerifyRoVignetteResult>
{
    readonly ILogger logger;

    public VerifyRoVignette(ILoggerFactory loggerFactory)
    {
        this.logger = loggerFactory.CreateLogger<VerifyRoVignette>();
    }

    public override async Task<VerifyRoVignetteResult> RunAsync(WorkflowActivityContext context, VerifyRoVignetteRequest input)
    {
        this.logger.LogInformation(
            "Verify RoVignette: ReqId# {requestId} for {vin}",
            input.RequestId,
            input.VIN);

        await Task.Delay(TimeSpan.FromSeconds(1));

        return new VerifyRoVignetteResult(ValidUntil: DateTime.Today);
    }
}