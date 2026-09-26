using Microsoft.EntityFrameworkCore;
using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Adicionar(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        _context.SaveChanges();
    }

    public Categoria? BuscarPorId(int id)
    {
        return _context.Categorias.Find(id);
    }

    public List<Categoria> ListarTodas()
    {
        return _context.Categorias.ToList();
    }

    public void Atualizar(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
        _context.SaveChanges();
    }

    public void Deletar(Categoria categoria)
    {
        _context.Categorias.Remove(categoria);
        _context.SaveChanges();
    }

}