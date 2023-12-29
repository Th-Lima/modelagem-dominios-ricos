using MediatR;
using NerdStore.Catalogo.Application.Services;
using Nerdstore.Catalogo.Data;
using Nerdstore.Catalogo.Data.Repository;
using NerdStore.Catalogo.Domain;
using NerdStore.Catalogo.Domain.Events;
using NerdStore.Core.BusMemory;
using NerdStore.Vendas.Application.Commands;

namespace NerdStore.WebApp.MVC.Setup;

public static class DependencyInjectionExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        #region MediatR
        
        services.AddScoped<IMediatrHandler, MediatrHandler>();
        
        #endregion

        #region Catalogo Context

        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IProdutoAppService, ProdutoAppService>();
        services.AddScoped<IEstoqueService, EstoqueService>();
        services.AddScoped<CatalogoContext>();

        #endregion

        #region MediatR Bus - Events

        services.AddScoped<INotificationHandler<ProdutoAbaixoEstoqueEvent>, ProdutoEventHandler>();

        #endregion

        #region Vendas - Command & CommandHandler

        services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
        
        #endregion
        
        return services;
    }
}