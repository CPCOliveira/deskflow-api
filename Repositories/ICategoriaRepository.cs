using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repositories;



public interface ICategoriaRepository
{
    void Adicionar(Categoria categoria);
    Categoria? BuscarPorId(int id);
    List<Categoria> ListarTodas();
    void Atualizar(Categoria categoria);
    void Deletar(Categoria categoria);
}