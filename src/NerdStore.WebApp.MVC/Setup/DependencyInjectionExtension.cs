using MediatR;
using NerdStore.Catalogo.Application.Services;
using Nerdstore.Catalogo.Data;
using Nerdstore.Catalogo.Data.Repository;
using NerdStore.Catalogo.Domain;
using NerdStore.Catalogo.Domain.Events;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Data;
using NerdStore.Vendas.Domain;

namespace NerdStore.WebApp.MVC.Setup;

public static class DependencyInjectionExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        RegisterMediatr(services);
        
        RegisterServicesCatalogoContext(services);
        
        RegisterServicesVendasContext(services);
        
        RegisterServicesDomainEvents(services);

        RegisterCommandsAndCommandHandlers(services);

        RegisterNotificationAndNotificationHandlers(services);

        return services;
    }

    private static void RegisterMediatr(IServiceCollection services)
    {
        services.AddScoped<IMediatorHandler, MediatorHandler>();
    }

    private static void RegisterServicesCatalogoContext(IServiceCollection services)
    {
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IProdutoAppService, ProdutoAppService>();
        services.AddScoped<IEstoqueService, EstoqueService>();
        services.AddScoped<CatalogoContext>();
    }

    private static void RegisterServicesVendasContext(IServiceCollection services)
    {
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        // services.AddScoped<IPedidoQueries, PedidoQueries>();
        services.AddScoped<VendasContext>();
    }

    private static void RegisterServicesDomainEvents(IServiceCollection services)
    {
        services.AddScoped<INotificationHandler<ProdutoAbaixoEstoqueEvent>, ProdutoEventHandler>();
    }

    private static void RegisterCommandsAndCommandHandlers(IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
    }

    private static void RegisterNotificationAndNotificationHandlers(IServiceCollection services)
    {
        services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
    }
}