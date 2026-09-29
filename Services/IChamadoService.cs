using DeskFlow.API.Models.Entities;

using DeskFlow.API.Models.Requests;

namespace DeskFlow.API.Services;

public interface IChamadoService
{
    Chamado AbrirChamado(ChamadoRequest request);
    List<Chamado> ListarTodos(StatusChamado? status, Prioridade? prioridade, int? categoriaId);
    Chamado? BuscarPorId(int id);
    ResultadoChamado IniciarAtendimento(int id);
    ResultadoChamado Encerrar(int id, string solucao);
}