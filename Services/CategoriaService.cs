using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _repository;

    public CategoriaService(ICategoriaRepository repository)
    {
        _repository = repository;
    }

    public Categoria Cadastrar(string nome)
    {
        var categoria = new Categoria {Nome = nome};
        _repository.Adicionar(categoria);
        return categoria;
    }

    public List<Categoria> ListarTodas()
    {
        return _repository.ListarTodas();
    }

    public Categoria? BuscarPorId(int id)
    {
        return _repository.BuscarPorId(id);
    }

    public bool AtualizarNome(int id, string novoNome)
    {
        var categoria = _repository.BuscarPorId(id);
        if (categoria == null)
        {
            return false;
        }

        categoria.Nome = novoNome;
        _repository.Atualizar(categoria);
        return true;
    }

    public bool Deletar(int id)
    {
        var categoria = _repository.BuscarPorId(id);
        if (categoria == null)
        {
            return false;
        }

        // TODO: quando a entidade Chamado existir, validar aqui (RF04)
        // se essa Categoria possui Chamados vinculados antes de deletar.
        // Por enquanto, deleta direto.

        _repository.Deletar(categoria);
        return true;
    }
}