using System.Threading.Tasks;
using Dapr.Client;
using Dapr.Workflow;
using Workflows.Models;
using Microsoft.Extensions.Logging;
using System;

namespace WorkflowConsoleApp.Activities;

class VerifyInsurance : WorkflowActivity<VerifyInsuranceRequest, VerifyInsuranceResult>
{
    static readonly string storeName = "statestore";
    readonly ILogger logger;
    readonly DaprClient client;

    public VerifyInsurance(ILoggerFactory loggerFactory, DaprClient client)
    {
        this.logger = loggerFactory.CreateLogger<VerifyInsurance>();
        this.client = client;
    }

    public override async Task<VerifyInsuranceResult> RunAsync(WorkflowActivityContext context, VerifyInsuranceRequest input)
    {
        this.logger.LogInformation(
            "Verify Insurance: ReqId# {requestId} for {vin}",
            input.RequestId,
            input.VIN);

        var result = client.CreateInvokeMethodRequest(HttpMethod.Post, "insuranceservice", "verify");
        
        var response = await client.InvokeMethodAsync<VerifyInsuranceResult>(result);

        this.logger.LogInformation(
            "Verify Insurance: ReqId# {requestId} for {vin}",
            input.RequestId,
            input.VIN);

        return new VerifyInsuranceResult(ValidUntil: response.ValidUntil);
    }
}