using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalogo.Domain;

public class Dimensoes
{
    public decimal Altura { get; private set; }

    public decimal Largura { get; private set; }

    public decimal Profundidade { get; private set; }

    public Dimensoes(decimal altura, decimal largura, decimal profundidade)
    {
        AssertionConcern.ValidarSeMenorIgualAMinimo(altura, 1, $"O campo {nameof(Altura)} não pode ser menor ou igual a 1");
        AssertionConcern.ValidarSeMenorIgualAMinimo(largura, 1, $"O campo {nameof(Largura)} não pode ser menor ou igual a 1");
        AssertionConcern.ValidarSeMenorIgualAMinimo(profundidade, 1, $"O campo {nameof(Profundidade)} não pode ser menor ou igual a 1");
        
        Altura = altura;
        Largura = largura;
        Profundidade = profundidade;
    }

    public string DescricaoFormatada()
    {
        return $"LxAxP: {Largura} x {Altura} x {Profundidade}";
    }

    public override string ToString()
    {
        return DescricaoFormatada();
    }
}