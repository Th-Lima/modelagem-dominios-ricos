using NerdStore.Catalogo.Domain.Events;
using NerdStore.Core.BusMemory;

namespace NerdStore.Catalogo.Domain;

public class EstoqueService : IEstoqueService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IMediatorHandler _bus;

    public EstoqueService(IProdutoRepository produtoRepository, IMediatorHandler bus)
    {
        _produtoRepository = produtoRepository;
        _bus = bus;
    }

    public async Task<bool> DebitarEstoque(Guid produtoId, int quantidade)
    {
        var produto = await _produtoRepository.ObterPorId((produtoId));

        if (produto == null || !produto.PossuiEstoque(quantidade))
            return false;
        
        produto.DebitarEstoque(quantidade);
        
        // TODO: Parametrizar a quantidade de estoque baixo
        //Publicando Evento de domínio, sem criar dependência com a classe que vai tratar o evento
        if (produto.QuantidadeEstoque < 10)
            await _bus.PublicarEvento(new ProdutoAbaixoEstoqueEvent(produto.Id, produto.QuantidadeEstoque));
        
        _produtoRepository.Atualizar(produto);

        return await _produtoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> ReporEstoque(Guid produtoId, int quantidade)
    {
        var produto = await _produtoRepository.ObterPorId((produtoId));

        if (produto == null)
            return false;
        
        produto.ReporEstoque(quantidade);
        
        _produtoRepository.Atualizar(produto);

        return await _produtoRepository.UnitOfWork.Commit();
    }

    public void Dispose()
    {
       _produtoRepository.Dispose();
    }
}