using System.Reactive;
using Dapr.Workflow;
using WorkflowConsoleApp.Activities;
using Workflows.Activities;

namespace Workflows.Models.Workflows;

public class AddCarWorkflow : Workflow<Car, CarInfo>
{
    public override async Task<CarInfo> RunAsync(WorkflowContext context, Car input)
    {
        var requestId = context.InstanceId;

        var verifyInsuranceResult =
            await context.CallActivityAsync<VerifyInsuranceResult>(
                nameof(VerifyInsurance),
                new VerifyInsuranceRequest(RequestId: requestId, VIN: input.VIN));

        var verifyTechnicalInspectionResult =
            await context.CallActivityAsync<VerifyTechnicalInspectionResult>(
                nameof(VerifyTechnicalInspection),
                new VerifyTechnicalInspectionRequest(RequestId: requestId, VIN: input.VIN));

        var verifyRoVignetteResult =
            await context.CallActivityAsync<VerifyRoVignetteResult>(
                nameof(VerifyRoVignette),
                new VerifyRoVignetteRequest(RequestId: requestId, VIN: input.VIN));

        return
            new CarInfo(
                TechnicalInspectionValidUntil: verifyTechnicalInspectionResult.ValidUntil,
                RoVignetteValidUntil: verifyRoVignetteResult.ValidUntil,
                InsuranceValidUntil: verifyInsuranceResult.ValidUntil);
    }
}