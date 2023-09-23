using NerdStore.Core.Messages;

namespace NerdStore.Core.BusMemory;

public interface IMediatrHandler
{
    Task PublicarEvento<T>(T evento) where T : Event;
}