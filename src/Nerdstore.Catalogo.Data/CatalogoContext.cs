using Microsoft.EntityFrameworkCore;
using NerdStore.Catalogo.Domain;
using NerdStore.Core.Data;
using NerdStore.Core.Messages;

namespace Nerdstore.Catalogo.Data;

public class CatalogoContext : DbContext, IUnitOfWork
{
    public CatalogoContext(DbContextOptions<CatalogoContext> options) : base(options){ }

    public DbSet<Produto> Produtos { get; set; }
    
    public DbSet<Categoria> Categorias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produto>()
            .Property(p => p.Valor)
            .HasColumnType("decimal(18,4)");
        
        var modelBuilderProperties = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(x => x.GetProperties().Where(x => x.ClrType == typeof(string)));

        foreach (var property in modelBuilderProperties) 
            property.SetColumnType("varchar(100)");

        // Ignore é para ignorar o Event pois ele não deve ser persistido na base
        modelBuilder.Ignore<Event>();
        
        //Busca os mappings via reflection e configura para que seja seguida as configurações das entidades
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoContext).Assembly);
    }

    public async Task<bool> Commit()
    {
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
        {
            // Se DataCadastro existe e a entidade estiver sendo adicionada, 
            // o DataCadastro irá receber o valor da data do momento do commit
            if (entry.State == EntityState.Added)
            {
                entry.Property("DataCadastro").CurrentValue = DateTime.Now;
            }

            // Se a entidade estiver sendo atualizada, qualquer valor em DataCadastro será ignorado
            if (entry.State == EntityState.Modified)
            {
                entry.Property("DataCadastro").IsModified = false;
            }
        }
        
        // Se o número de linhas afetadas for maior que zero irá retornar true
        return await base.SaveChangesAsync() > 0;
    }
}