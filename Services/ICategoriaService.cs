using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Services;

public interface ICategoriaService
{
    Categoria Cadastrar(string nome);
    List<Categoria> ListarTodas();
    Categoria? BuscarPorId(int id);
    bool AtualizarNome(int id, string nome);
    ResultadoExclusaoCategoria Deletar(int id);

}