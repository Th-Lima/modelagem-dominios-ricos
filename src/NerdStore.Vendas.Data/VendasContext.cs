using Microsoft.EntityFrameworkCore;
using NerdStore.Core.Data;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Data;

public class VendasContext : DbContext, IUnitOfWork
{
    public VendasContext(DbContextOptions<VendasContext> options) : base(options) { }

    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<PedidoItem> PedidoItems { get; set; }
    public DbSet<Voucher> Vouchers { get; set; }
    
    public async Task<bool> Commit()
    {
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("DataCadastro").CurrentValue = DateTime.Now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property("DataCadastro").IsModified = false;
            }
        }
        
        var sucesso = await base.SaveChangesAsync() > 0;
            
        /// O this abaixo é o VendasContext
        //if(sucesso) await _mediatorHandler.PublicarEventos(this);

        return sucesso;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Decimal mapeamentos - Especifico para SQLite

        modelBuilder.Entity<PedidoItem>()
            .Property(p => p.ValorUnitario)
            .HasConversion<double>();
        
        modelBuilder.Entity<Pedido>()
            .Property(p => p.Desconto)
            .HasConversion<double>();
        
        modelBuilder.Entity<Pedido>()
            .Property(p => p.ValorTotal)
            .HasConversion<double>();
        
        modelBuilder.Entity<Voucher>()
            .Property(p => p.Percentual)
            .HasConversion<double>();
        
        modelBuilder.Entity<Voucher>()
            .Property(p => p.ValorDesconto)
            .HasConversion<double>();

        #endregion
        
        /// <summary>
        /// O foreach abaixo pega todas as entidades mapeadas, verifica quais as propriedades são
        /// do tipo "string" e mapear automaticamente o tipo da coluna como "varchar(100)" caso
        /// a coluna já não tenha uma especificação diferente. Isso por motivos de segurança, impedindo
        /// a criação de uma coluna como "NVARCHAR(MAX)"
        /// </summary>
        var modelBuilderProperties = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(x => x.GetProperties().Where(x => x.ClrType == typeof(string)));

        foreach (var property in modelBuilderProperties) 
            property.SetColumnType("varchar(100)");

        // Ignore é para ignorar o Event pois ele não deve ser persistido na base
        //modelBuilder.Ignore<Event>();

        // Vai buscar todas as entidades e seus mappings via "reflection" apenas um vez
        // e irá configrar para que siga as configurações feitas nos mappings
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VendasContext).Assembly);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) 
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        //modelBuilder.HasSequence<int>("MinhaSequencia").StartsAt(1000).IncrementsBy(1);
        base.OnModelCreating(modelBuilder);
    }
}