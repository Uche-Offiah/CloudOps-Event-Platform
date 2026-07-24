using System.Threading;
using System.Threading.Tasks;

namespace CloudOps.Application.Features.Events.ProcessEvent;

public interface IEventMessageProcessor
{
    Task<ProcessingResult> ProcessAsync(string messageBody, CancellationToken cancellationToken);
}