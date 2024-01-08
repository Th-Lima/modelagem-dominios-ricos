using MediatR;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Application.Commands;

public class PedidoCommandHandler : IRequestHandler<AdicionarItemPedidoCommand, bool>
{
    private readonly IPedidoRepository _pedidoRepository;

    public PedidoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    #region Adicionar Pedido - Handle - AdicionarItemPedidoCommando
    
    public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
    {
        if (!ValidarComando(message))
            return false;

        var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);
        var pedidoItem = new PedidoItem(message.ProdutoId, message.Nome, message.Quantidade, message.ValorUnitario);

        if (pedido == null)
        {
            pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.ClienteId);
            pedido.AdicionarItem(pedidoItem);

            _pedidoRepository.Adicionar(pedido);
            //pedido.AdicionarEvento(new PedidoRascunhoIniciadoEvent(message.ClienteId, pedido.Id));
        }
        else
        {
            var pedidoItemExistente = pedido.PedidoItemExistente(pedidoItem);
            pedido.AdicionarItem(pedidoItem); // Altera o pedido realizando update no banco

            if (pedidoItemExistente)
            {
                _pedidoRepository.AtualizarItem(pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId));
            }
            else
            {
                _pedidoRepository.AdicionarItem(pedidoItem);
            }
        }

        //pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id, message.ProdutoId, message.Nome, message.ValorUnitario, message.Quantidade));
        return await _pedidoRepository.UnitOfWork.Commit();
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