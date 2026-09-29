using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Services;

public interface IInteracaoService
{
    Interacao? Adicionar(int chamadoId, string autor, string mensagem);
    List<Interacao>? ListarPorChamado(int chamadoId);
}