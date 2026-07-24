using System.Threading;
using System.Threading.Tasks;
using CloudOps.Application.Features.Events.ProcessEvent;

namespace CloudOps.Application.Interfaces.Messaging;

public interface IEventMessageProcessor
{
    Task<ProcessingResult> ProcessAsync(string messageBody, CancellationToken cancellationToken);
}