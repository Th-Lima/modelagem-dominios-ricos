using System.Security.Cryptography;
using MediatR;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;
using NerdStore.Vendas.Application.Commands;

namespace NerdStore.Vendas.Application.Events;

public class PedidoEventHandler : 
    INotificationHandler<PedidoRascunhoIniciadoEvent>,
    INotificationHandler<PedidoAtualizadoEvent>,
    INotificationHandler<PedidoItemAdicionadoEvent>,
    INotificationHandler<PedidoEstoqueRejeitadoEvent>,
    INotificationHandler<PagamentoRealizadoEvent>,
    INotificationHandler<PagamentoRecusadoEvent>,
    INotificationHandler<PedidoFinalizadoEvent>
    
{
    private IMediatorHandler _mediator;

    public PedidoEventHandler(IMediatorHandler mediator)
    {
        _mediator = mediator;
    }

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

    public async Task Handle(PedidoEstoqueRejeitadoEvent message, CancellationToken cancellationToken)
    {
        await _mediator.EnviarComando(new CancelarProcessamentoPedidoCommand(message.PedidoId, message.ClienteId));
    }

    public async Task Handle(PagamentoRealizadoEvent message, CancellationToken cancellationToken)
    {
        await _mediator.EnviarComando(new FinalizarPedidoCommand(message.PedidoId, message.ClienteId));
    }

    public async Task Handle(PagamentoRecusadoEvent message, CancellationToken cancellationToken)
    {
        await _mediator.EnviarComando(new CancelarProcessamentoPedidoEstornarEstoqueCommand(message.PedidoId, message.ClienteId));
    }

    public Task Handle(PedidoFinalizadoEvent notification, CancellationToken cancellationToken)
    {
        //Envia email de confirmação do pedido para usuário
        return Task.CompletedTask;
    }
}