using MediatR;
using NerdStore.Catalogo.Application.Services;
using Nerdstore.Catalogo.Data;
using Nerdstore.Catalogo.Data.Repository;
using NerdStore.Catalogo.Domain;
using NerdStore.Catalogo.Domain.Events;
using NerdStore.Core.BusMemory;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Data;
using NerdStore.Vendas.Domain;

namespace NerdStore.WebApp.MVC.Setup;

public static class DependencyInjectionExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        #region MediatR
        
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        
        #endregion

        #region Catalogo Context

        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IProdutoAppService, ProdutoAppService>();
        services.AddScoped<IEstoqueService, EstoqueService>();
        services.AddScoped<CatalogoContext>();

        #endregion
        
        #region Vendas Context

        services.AddScoped<IPedidoRepository, PedidoRepository>();
        // services.AddScoped<IPedidoQueries, PedidoQueries>();
        services.AddScoped<VendasContext>();

        #endregion

        #region MediatR Bus - Events

        services.AddScoped<INotificationHandler<ProdutoAbaixoEstoqueEvent>, ProdutoEventHandler>();

        #endregion

        #region Commands & CommandHandler

        services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
        
        #endregion

        return services;
    }
}