using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services;

public class InteracaoService : IInteracaoService
{
    private readonly IInteracaoRepository _repository;
    private readonly IChamadoRepository _chamadoRepository;

    public InteracaoService(IInteracaoRepository repository, IChamadoRepository chamadoRepository)
    {
        _repository = repository;
        _chamadoRepository = chamadoRepository;
    }

    public Interacao? Adicionar(int chamadoId, string autor, string mensagem)
    {
        var chamado = _chamadoRepository.BuscarPorId(chamadoId);
        if (chamado == null)
        {
            return null;
        }

        var interacao = new Interacao
        {
            ChamadoId = chamadoId,
            Autor = autor,
            Mensagem = mensagem,
            DataRegistro = DateTime.Now
        };

        _repository.Adicionar(interacao);
        return interacao;
    }

    public List<Interacao>? ListarPorChamado(int chamadoId)
    {
        var chamado = _chamadoRepository.BuscarPorId(chamadoId);
        if (chamado == null)
        {
            return null;
        }

        return _repository.ListarPorChamado(chamadoId);
    }
}