using Microsoft.EntityFrameworkCore;
using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repositories;

public class ChamadoRepository : IChamadoRepository
{
    private readonly AppDbContext _context;

    public ChamadoRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Adicionar(Chamado chamado)
    {
        _context.Chamados.Add(chamado);
        _context.SaveChanges();
    }

    public Chamado? BuscarPorId(int id)
    {
        return _context.Chamados.Find(id);

    }

    public List<Chamado> ListarTodos()
    {
        return _context.Chamados.ToList();

    }

    public void Atualizar(Chamado chamado)
    {
        _context.Chamados.Update(chamado);
        _context.SaveChanges();
    }

    public bool CategoriaPossuiChamado(int categoriaId)
    {
        return _context.Chamados.Any(c => c.CategoriaId == categoriaId);
    }

}