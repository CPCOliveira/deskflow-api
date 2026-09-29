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
        return _context.Chamados
            .Include(c => c.Categoria)
            .Include(c => c.Interacoes)
            .FirstOrDefault(c => c.Id == id);

    }

    public List<Chamado> ListarTodos(StatusChamado? status, Prioridade? prioridade, int? categoriaId)
    {
        var query = _context.Chamados.AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        if (prioridade.HasValue)
        {
            query = query.Where(c => c.Prioridade == prioridade.Value);
        }

        if (categoriaId.HasValue)
        {
            query = query.Where(c => c.CategoriaId == categoriaId.Value);
        }

        return query.ToList();
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