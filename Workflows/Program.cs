using Dapr.Client;
using Dapr.Workflow;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using WorkflowConsoleApp.Activities;
using Workflows.Activities;
using Workflows.Models;
using Workflows.Models.Workflows;

const string storeName = "statestore";

// The workflow host is a background service that connects to the sidecar over gRPC
var builder = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
{
    services.AddDaprWorkflow(options =>
    {
        // Note that it's also possible to register a lambda function as the workflow
        // or activity implementation instead of a class.
        options.RegisterWorkflow<AddCarWorkflow>();

        // These are the activities that get invoked by the workflow(s).
        options.RegisterActivity<VerifyInsurance>();
        options.RegisterActivity<VerifyRoVignette>();
        options.RegisterActivity<VerifyTechnicalInspection>();
    });
});

// Start the app - this is the point where we connect to the Dapr sidecar
using var host = builder.Build();
host.Start();

using var daprClient = new DaprClientBuilder().Build();

// NOTE: WorkflowEngineClient will be replaced with a richer version of DaprClient
//       in a subsequent SDK release. This is a temporary workaround.
WorkflowEngineClient workflowClient = host.Services.GetRequiredService<WorkflowEngineClient>();

// Generate a unique ID for the workflow
string carId = Guid.NewGuid().ToString()[..8];
var vin = "WBAKS123412345";
var car = new Car(vin);

// Start the workflow
Console.WriteLine("Starting workflow {0} verifying VIN: {1}", carId, vin);

await workflowClient.ScheduleNewWorkflowAsync(
    name: nameof(AddCarWorkflow),
    instanceId: carId,
    input: car);

// Wait a second to allow workflow to start
await Task.Delay(TimeSpan.FromSeconds(1));

WorkflowState state = await workflowClient.GetWorkflowStateAsync(
    instanceId: carId,
    getInputsAndOutputs: true);

Console.WriteLine("Your workflow has started. Here is the status of the workflow: {0}", state.RuntimeStatus);
while (!state.IsWorkflowCompleted)
{
    await Task.Delay(TimeSpan.FromSeconds(5));
    state = await workflowClient.GetWorkflowStateAsync(
        instanceId: carId,
        getInputsAndOutputs: true);
}

Console.WriteLine("Workflow Status: {0}", state.RuntimeStatus);
