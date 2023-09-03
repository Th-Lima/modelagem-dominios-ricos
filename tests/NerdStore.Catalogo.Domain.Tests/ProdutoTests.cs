using System.Runtime.InteropServices;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalogo.Domain.Tests;

public class ProdutoTests
{
    [Fact]
    public void Produto_Validar_ValidacoesDevemRetornarExceptions()
    {
        //Arrange & Act & Assert
        
        //Nome do Produto
        var ex = Assert.Throws<DomainException>(() =>
        {
            new Produto(string.Empty, "Descrição", false, 100,  DateTime.Now, "Imagem", Guid.NewGuid(), new Dimensoes(1,1,1));
        });
        
        Assert.Equal("O Campo Nome do produto não pode estar Vazio", ex.Message);
        
        //Descrição do Produto
        ex = Assert.Throws<DomainException>(() =>
        {
            new Produto("Nome", string.Empty, false, 100,  DateTime.Now, "Imagem", Guid.NewGuid(), new Dimensoes(1,1,1));
        });
        
        Assert.Equal("O Campo Descrição do produto não pode estar Vazio", ex.Message);
        
        //Valor do Produto
        ex = Assert.Throws<DomainException>(() =>
        {
            new Produto("Nome", "Descrição", false, 0,  DateTime.Now, "Imagem", Guid.NewGuid(), new Dimensoes(1,1,1));
        });
        
        Assert.Equal("O Campo Valor do produto não pode ser menor igual a 0", ex.Message);
        
        //CategoriaId do Produto
        ex = Assert.Throws<DomainException>(() =>
        {
            new Produto("Nome", "Descrição", false, 100,  DateTime.Now, "Imagem", Guid.Empty, new Dimensoes(1,1,1));
        });
        
        Assert.Equal("O Campo CategoriaId do produto não pode estar Vazio", ex.Message);
        
        //Imagem do Produto
        ex = Assert.Throws<DomainException>(() =>
        {
            new Produto("Nome", "Descrição", false, 100,  DateTime.Now, string.Empty, Guid.NewGuid(), new Dimensoes(1,1,1));
        });
        
        Assert.Equal("O Campo Imagem do produto não pode estar Vazio", ex.Message);
        
        //Largura da dimensão do Produto
        ex = Assert.Throws<DomainException>(() =>
        {
            new Produto("Nome", "Descrição", false, 100,  DateTime.Now, "Imagem", Guid.NewGuid(), new Dimensoes(1,0,1));
        });
        
        Assert.Equal("O campo Largura não pode ser menor ou igual a 0", ex.Message);
    }
}