using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalogo.Domain;

public class Produto : Entity, IAggregateRoot
{
    #region Properties
    
    public string Nome { get; private set; }
    
    public string Descricao { get; private set; }
    
    public bool Ativo { get; private set; }
    
    public decimal Valor { get; private set; }
    
    public DateTime DataCadastro { get; private set; }
    
    public string Imagem { get; private set; }
    
    public int QuantidadeEstoque { get; private set; }

    public Dimensoes Dimensoes { get; private set; }
    
    //EF Core
    public Categoria Categoria { get; private set; }
    
    //Foreign Key
    public Guid CategoriaId { get; private set; }
    
    #endregion
    
    #region Constructor
    
    public Produto(
        string nome,
        string descricao,
        bool ativo,
        decimal valor,
        DateTime dataCadastro,
        string imagem,
        Guid categoriaId, 
        Dimensoes dimensoes)
    {
        Nome = nome;
        Descricao = descricao;
        Ativo = ativo;
        Valor = valor;
        CategoriaId = categoriaId;
        Dimensoes = dimensoes;
        DataCadastro = dataCadastro;
        Imagem = imagem;
        
        Validar();
    }
    
    #endregion

    #region Methods Ad Hock setters
    
    public void Ativar() => Ativo = true;
    public void Desativar() => Ativo = false;

    public void AlterarCategoria(Categoria categoria)
    {
        Categoria = categoria;
        CategoriaId = categoria.Id;
    }

    public void AlterarDescricao(string descricao)
    {
        AssertionConcern.ValidarSeVazio(descricao, $"O Campo Descrição do produto não pode estar vazio");
        Descricao = descricao;
    }

    public void DebitarEstoque(int quantidade)
    {
        if (quantidade < 0) 
            quantidade *= -1;

        if (!PossuiEstoque(quantidade))
            throw new DomainException("Estoque insuficiente");
        
        QuantidadeEstoque -= quantidade;
    }
    
    public void ReporEstoque(int quantidade)
    {
        QuantidadeEstoque += quantidade;
    }

    public bool PossuiEstoque(int quantidade) => QuantidadeEstoque >= quantidade;
    
    #endregion
    
    public void Validar()
    {
        AssertionConcern.ValidarSeVazio(Nome, $"O Campo Nome do produto não pode estar Vazio");
        AssertionConcern.ValidarSeVazio(Descricao, $"O Campo Descrição do produto não pode estar Vazio");
        AssertionConcern.ValidarSeIgual(CategoriaId, Guid.Empty, $"O Campo CategoriaId do produto não pode estar Vazio");
        AssertionConcern.ValidarSeMenorQue(Valor, 1, $"O Campo Valor do produto não pode ser menor igual a 0");
        AssertionConcern.ValidarSeVazio(Imagem, $"O Campo Imagem do produto não pode estar Vazio");
    }
}