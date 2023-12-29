using MediatR;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Application.Commands;

namespace NerdStore.Vendas.Application.Commands;

public class PedidoCommandHandler : IRequestHandler<AdicionarItemPedidoCommand, bool>
{
    #region Adicionar Pedido - Handle - AdicionarItemPedidoCommando
    
    public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
    {
        if (!ValidarComando(message))
            return false;

        return true;
    }

    #endregion
    
    private bool ValidarComando(Command message)
    {
        if (message.EhValido()) 
            return true;

        foreach (var error in message.ValidationResult.Errors)
        {
            //Lançar evento de erro
            // _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, error.ErrorMessage));
        }

        return false;
    }
}