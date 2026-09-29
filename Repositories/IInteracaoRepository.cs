using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repositories;

public interface IInteracaoRepository
{
    void Adicionar(Interacao interacao);
    List<Interacao> ListarPorChamado(int chamadoId);
}