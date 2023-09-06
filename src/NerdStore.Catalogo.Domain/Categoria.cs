using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalogo.Domain;

public class Categoria : Entity
{
    public string Nome { get; private set; }
    
    public int Codigo { get; private set; }
    
    //EF Relation
    public ICollection<Produto> Produtos { get; private set; }

    protected Categoria() { }
    public Categoria(string nome, int codigo)
    {
        Nome = nome;
        Codigo = codigo;

        Validar();
    }

    public override string ToString()
    {
        return $"{Nome} - {Codigo}";
    }

    public void Validar()
    {
        AssertionConcern.ValidarSeVazio(Nome, $"O Campo {nameof(Nome)} da categoria não pode estar Vazio");
        AssertionConcern.ValidarSeIgual(Codigo, 0, $"O Campo {nameof(Codigo)} da categoria não pode ser igual a 0");
    }
}