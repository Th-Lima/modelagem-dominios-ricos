using NerdStore.Core.DomainObjects.Dto;

namespace NerdStore.Pagamentos.Business;

public interface IPagamentoService
{
    Task<Transacao> RealizarPagamentoPedido(PagamentoPedidoDto pagamentoPedido);
}