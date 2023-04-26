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

    // public override async Task<Object> RunAsync(WorkflowActivityContext context, PaymentRequest req)
    // {
    //
    //     // Simulate slow processing
    //     await Task.Delay(TimeSpan.FromSeconds(5));
    //
    //     // Determine if there are enough Items for purchase
    //     var (original, originalETag) =
    //         await client.GetStateAndETagAsync<OrderPayload>(storeName, req.ItemBeingPruchased);
    //     int newQuantity = original.Quantity - req.Amount;
    //
    //     // Update the statestore with the new amount of paper clips
    //     await client.SaveStateAsync<OrderPayload>(storeName, req.ItemBeingPruchased,
    //         new OrderPayload(Name: req.ItemBeingPruchased, TotalCost: req.Currency, Quantity: newQuantity));
    //     this.logger.LogInformation($"There are now: {newQuantity} {original.Name} left in stock");
    //
    //     return null;
    // }

    public override async Task<VerifyInsuranceResult> RunAsync(WorkflowActivityContext context, VerifyInsuranceRequest input)
    {
        this.logger.LogInformation(
            "Verify Insurance: ReqId# {requestId} for {vin}",
            input.RequestId,
            input.VIN);

        await Task.Delay(TimeSpan.FromSeconds(1));

        return new VerifyInsuranceResult(ValidUntil: DateTime.Today);
    }
}