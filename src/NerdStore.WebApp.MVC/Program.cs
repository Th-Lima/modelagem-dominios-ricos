using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NerdStore.Catalogo.Application.AutoMapper;
using NerdStore.WebApp.MVC.Data;
using MediatR;
using Nerdstore.Catalogo.Data;
using NerdStore.Pagamentos.Data;
using NerdStore.Vendas.Data;
using NerdStore.WebApp.MVC.Setup;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

#region Identity

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

#endregion

#region DbContext Catalogo, Vendas, Pagamentos

builder.Services
    .AddDbContext<CatalogoContext>(options => options.UseSqlite(connectionString));

builder.Services
    .AddDbContext<VendasContext>(options => options.UseSqlite(connectionString));

builder.Services
    .AddDbContext<PagamentoContext>(options => options.UseSqlite(connectionString));

#endregion

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

#region AutoMapper

builder.Services.AddAutoMapper(
    typeof(DomainToDtoMappingProfile),
    typeof(DtoToDomainMappingProfile));

#endregion

#region MediatR

builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

#endregion

#region Dependency Injection

builder.Services.RegisterServices();

#endregion


builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Vitrine}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();