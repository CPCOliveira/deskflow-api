using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repositories;

public interface IChamadoRepository
{
    void Adicionar(Chamado chamado);
    Chamado? BuscarPorId(int id);
    List<Chamado> ListarTodos(StatusChamado? status, Prioridade? prioridade, int? categoriaId);
    void Atualizar(Chamado chamado);
    bool CategoriaPossuiChamado(int categoriaId);
}