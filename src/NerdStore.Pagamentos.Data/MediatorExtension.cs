using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Pagamentos.Data;

public static class MediatorExtension
{
    /// <summary>
    /// Extension Method para publicar uma lista de eventos
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="ctx"></param>
    public static async Task PublicarEventosAsync(this IMediatorHandler mediator, PagamentoContext ctx)
    {
        /// Irá pegar todas as entidades dentro do ChangeTracker do context onde forem do tipo Entity
        /// onde elas possuem alguma notificação
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.EventNotificacoes != null && x.Entity.EventNotificacoes.Any());

        /// Selecionando todos os enventos de domínio 
        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.EventNotificacoes)
            .ToList();

        /// Transformando em uma lista e limpando os eventos em seguida
        domainEntities.ToList()
            .ForEach(entity => entity.Entity.LimparEventos());

        /// Os eventos ao serem selecionados na lista, serão publicados um a um
        var tasks = domainEvents
            .Select(async (domainEvent) => { await mediator.PublicarEvento(domainEvent); });

        /// Somente irá retornar quando todos os eventos forem lançados
        await Task.WhenAll(tasks);
    }
}