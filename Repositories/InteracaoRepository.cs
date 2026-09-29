using Microsoft.EntityFrameworkCore;
using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repositories;

public class InteracaoRepository : IInteracaoRepository
{
    private readonly AppDbContext _context;

    public InteracaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Adicionar(Interacao interacao)
    {
        _context.Interacoes.Add(interacao);
        _context.SaveChanges();
    }

    public List<Interacao> ListarPorChamado(int chamadoId)
    {
        return _context.Interacoes
            .Where(i => i.ChamadoId == chamadoId)
            .ToList();
    }
}