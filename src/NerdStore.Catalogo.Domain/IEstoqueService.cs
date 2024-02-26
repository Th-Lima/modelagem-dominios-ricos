using NerdStore.Core.DomainObjects.Dto;

namespace NerdStore.Catalogo.Domain;

public interface IEstoqueService : IDisposable
{
    Task<bool> DebitarEstoque(Guid produtoId, int quantidade);
    Task<bool> DebitarListaProdutosPedido(ListaProdutosPedidoDto lista);
    Task<bool> ReporEstoque(Guid produtoId, int quantidade);
    Task<bool> ReporListaProdutosPedido(ListaProdutosPedidoDto lista);
}