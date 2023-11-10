using MediatR;
using NerdStore.Catalogo.Application.Services;
using Nerdstore.Catalogo.Data;
using Nerdstore.Catalogo.Data.Repository;
using NerdStore.Catalogo.Domain;
using NerdStore.Catalogo.Domain.Events;
using NerdStore.Core.BusMemory;

namespace NerdStore.WebApp.MVC.Setup;

public static class DependencyInjectionExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        // Mediator
        services.AddScoped<IMediatrHandler, MediatrHandler>();
        
        // Catalogo
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IProdutoAppService, ProdutoAppService>();
        services.AddScoped<IEstoqueService, EstoqueService>();
        services.AddScoped<CatalogoContext>();
        
        //Mediatr bus - events
        services.AddScoped<INotificationHandler<ProdutoAbaixoEstoqueEvent>, ProdutoEventHandler>();
        
        return services;
    }
}