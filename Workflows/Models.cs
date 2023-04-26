namespace Workflows.Models;

public record Car(string VIN);

public record CarInfo(DateTime? TechnicalInspectionValidUntil, DateTime? RoVignetteValidUntil, DateTime? InsuranceValidUntil);
record VerifyInsuranceRequest(string RequestId, string VIN);
record VerifyInsuranceResult(DateTime? ValidUntil);
record VerifyRoVignetteRequest(string RequestId, string VIN);
record VerifyRoVignetteResult(DateTime? ValidUntil);
record VerifyTechnicalInspectionRequest(string RequestId, string VIN);
record VerifyTechnicalInspectionResult(DateTime? ValidUntil);
