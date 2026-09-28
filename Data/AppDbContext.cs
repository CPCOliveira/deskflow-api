using Microsoft.EntityFrameworkCore;
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Categoria> Categorias { get; set; }

    public DbSet<Chamado> Chamados { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chamado>()
            .HasOne(c => c.Categoria)
            .WithMany(cat => cat.Chamados)
            .HasForeignKey(c => c.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}