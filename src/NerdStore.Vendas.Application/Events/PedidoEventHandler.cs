using System.Security.Cryptography;
using MediatR;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;

namespace NerdStore.Vendas.Application.Events;

public class PedidoEventHandler : 
    INotificationHandler<PedidoRascunhoIniciadoEvent>,
    INotificationHandler<PedidoAtualizadoEvent>,
    INotificationHandler<PedidoItemAdicionadoEvent>,
    INotificationHandler<PedidoEstoqueRejeitadoEvent>
{
    public Task Handle(PedidoRascunhoIniciadoEvent notification, CancellationToken cancellationToken)
    {
        //Implementar lógica para este evento
        //Exemplo: Publicar mensagem em uma fila de mensageria informando que o pedido esta no carrinho como rascunho.
       return Task.CompletedTask;
    }

    public Task Handle(PedidoAtualizadoEvent notification, CancellationToken cancellationToken)
    {
        //Implementar lógica para este evento
        //Exemplo: Enviar email, fazer alguma outra integração e etc ...
        return Task.CompletedTask;
    }

    public Task Handle(PedidoItemAdicionadoEvent notification, CancellationToken cancellationToken)
    {
        //Implementar lógica para este evento
        return Task.CompletedTask;
    }

    public Task Handle(PedidoEstoqueRejeitadoEvent notification, CancellationToken cancellationToken)
    {
        //Cancelar o processamento do pedido- retornar erro para o cliente
        return Task.CompletedTask;
    }
}