namespace CloudOps.Application.Features.Events.ProcessEvent;

public sealed record ProcessingResult(bool Succeeded, string? FailureReason = null)
{
    public static ProcessingResult Success() => new(true);

    public static ProcessingResult Failure(string reason) => new(false, reason);
}