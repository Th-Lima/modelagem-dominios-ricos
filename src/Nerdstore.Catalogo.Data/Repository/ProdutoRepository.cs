using Microsoft.EntityFrameworkCore;
using NerdStore.Catalogo.Domain;
using NerdStore.Core.Data;

namespace Nerdstore.Catalogo.Data.Repository;

public class ProdutoRepository : IProdutoRepository
{
    private readonly CatalogoContext _catalogoContext;
    
    public IUnitOfWork UnitOfWork => _catalogoContext;

    public ProdutoRepository(CatalogoContext catalogoContext)
    {
        _catalogoContext = catalogoContext;
    }
    
    public async Task<IEnumerable<Produto>> ObterTodos()
    {
        return await _catalogoContext.Produtos.AsNoTracking().ToListAsync();
    }

    public async Task<Produto> ObterPorId(Guid id)
    {
        return await _catalogoContext.Produtos.FindAsync(id);
    }

    public async Task<IEnumerable<Produto>> ObterPorCategoria(int codigo)
    {
        return await _catalogoContext.Produtos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Where(c => c.Categoria.Codigo == codigo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Categoria>> ObterCategorias()
    {
        return await _catalogoContext.Categorias.AsNoTracking().ToListAsync();
    }

    public void Adicionar(Produto produto)
    {
        _catalogoContext.Produtos.Add(produto);
    }

    public void Atualizar(Produto produto)
    {
        _catalogoContext.Produtos.Update(produto);
    }

    public void Adicionar(Categoria categoria)
    {
        _catalogoContext.Categorias.Add(categoria);
    }

    public void Atualizar(Categoria categoria)
    {
        _catalogoContext.Categorias.Update(categoria);
    }

    public void Dispose()
    {
        _catalogoContext?.Dispose();
    }
}