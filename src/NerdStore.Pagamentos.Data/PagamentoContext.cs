using Microsoft.EntityFrameworkCore;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Data;
using NerdStore.Core.Messages;
using NerdStore.Pagamentos.Business;

namespace NerdStore.Pagamentos.Data;

public class PagamentoContext : DbContext, IUnitOfWork
{
    private readonly IMediatorHandler _mediatorHandler;

    public PagamentoContext(DbContextOptions<PagamentoContext> options, IMediatorHandler rebusHandler)
        : base(options)
    {
        _mediatorHandler = rebusHandler;
    }

    public DbSet<Pagamento> Pagamentos { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pagamento>()
            .Property(p => p.Valor)
            .HasConversion<double>();
        
        modelBuilder.Entity<Transacao>()
            .Property(p => p.Total)
            .HasConversion<double>();
        
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
        modelBuilder.Ignore<Event>();

        // Vai buscar todas as entidades e seus mappings via "reflection" apenas um vez
        // e irá configrar para que siga as configurações feitas nos mappings
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PagamentoContext).Assembly);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
        
        base.OnModelCreating(modelBuilder);
    }
    
    public async Task<bool> Commit()
    {
        foreach (var entry in ChangeTracker.Entries()
                     .Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
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
        if (sucesso) 
            await _mediatorHandler.PublicarEventosAsync(this);

        return sucesso;
    }
}