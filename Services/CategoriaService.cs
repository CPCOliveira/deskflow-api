using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _repository;
    private readonly IChamadoRepository _chamadoRepository;

    public CategoriaService(ICategoriaRepository repository, IChamadoRepository chamadoRepository)
    {
        _repository = repository;
        _chamadoRepository = chamadoRepository;
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

    public ResultadoExclusaoCategoria Deletar(int id)
    {
        var categoria = _repository.BuscarPorId(id);
        if (categoria == null)
        {
            return ResultadoExclusaoCategoria.CategoriaNaoLocalizada;
        }

        if (_chamadoRepository.CategoriaPossuiChamado(id))
        {
            return ResultadoExclusaoCategoria.CategoriaPossuiChamados;
        }

        _repository.Deletar(categoria);
        return ResultadoExclusaoCategoria.DeletadoComSucesso;
    }
}