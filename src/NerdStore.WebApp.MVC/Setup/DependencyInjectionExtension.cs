using MediatR;
using NerdStore.Catalogo.Application.Services;
using Nerdstore.Catalogo.Data;
using Nerdstore.Catalogo.Data.Repository;
using NerdStore.Catalogo.Domain;
using NerdStore.Catalogo.Domain.Events;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Data;
using NerdStore.Vendas.Domain;

namespace NerdStore.WebApp.MVC.Setup;

public static class DependencyInjectionExtension
{
    public static void RegisterServices(this IServiceCollection services)
    {
        //MediatR
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        
        //Notifications
        services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
        
        //Catalogo
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IProdutoAppService, ProdutoAppService>();
        services.AddScoped<IEstoqueService, EstoqueService>();
        services.AddScoped<CatalogoContext>();
        
        services.AddScoped<INotificationHandler<ProdutoAbaixoEstoqueEvent>, ProdutoEventHandler>();
        
        //Vendas
        services.AddScoped<VendasContext>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        
        services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
        
        services.AddScoped<INotificationHandler<PedidoRascunhoIniciadoEvent>, PedidoEventHandler>();
        services.AddScoped<INotificationHandler<PedidoAtualizadoEvent>, PedidoEventHandler>();
        services.AddScoped<INotificationHandler<PedidoItemAdicionadoEvent>, PedidoEventHandler>();
    }
}