using Microsoft.EntityFrameworkCore;
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Categoria> Categorias { get; set; }
}