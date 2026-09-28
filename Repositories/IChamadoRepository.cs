using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repositories;

public interface IChamadoRepository
{
    void Adicionar(Chamado chamado);
    Chamado? BuscarPorId(int id);
    List<Chamado> ListarTodos();
    void Atualizar(Chamado chamado);
    bool CategoriaPossuiChamado(int categoriaId);
}