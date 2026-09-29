using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Requests;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services;

public class ChamadoService : IChamadoService
{
    private readonly IChamadoRepository _repository;
    private readonly ICategoriaRepository _categoriaRepository;

    public ChamadoService(IChamadoRepository repository, ICategoriaRepository categoriaRepository)
    {
        _repository = repository;
        _categoriaRepository = categoriaRepository;
    }

    public Chamado? AbrirChamado(ChamadoRequest request)
    {
        var categoria = _categoriaRepository.BuscarPorId(request.CategoriaId);

        if (categoria == null)
        {
            return null;
        }

        var chamado = new Chamado
        {
            Titulo = request.Titulo,
            Descricao = request.Descricao,
            SolicitanteNome = request.SolicitanteNome,
            Prioridade = request.Prioridade,
            CategoriaId = request.CategoriaId,
            Status = StatusChamado.Aberto,
            DataAbertura = DateTime.Now
        };

        _repository.Adicionar(chamado);
        return chamado;
    }

    public List<Chamado> ListarTodos(StatusChamado? status, Prioridade? prioridade, int? categoriaId)
    {
        return _repository.ListarTodos(status, prioridade, categoriaId);
    }

    public Chamado? BuscarPorId(int id)
    {
        return _repository.BuscarPorId(id);
    }

    public ResultadoChamado IniciarAtendimento(int id)
    {
        var chamado = _repository.BuscarPorId(id);

        if (chamado == null)
        {
            return ResultadoChamado.NaoLocalizado;
        }

        if (chamado.Status != StatusChamado.Aberto)
        {
            return ResultadoChamado.TransicaoInvalida;
        }

        chamado.Status = StatusChamado.EmAndamento;
        _repository.Atualizar(chamado);

        return ResultadoChamado.Validado;
    }

    public ResultadoChamado Encerrar(int id, string solucao)
    {
        var chamado = _repository.BuscarPorId(id);

        if (chamado == null)
        {
            return ResultadoChamado.NaoLocalizado;
        }

        if (chamado.Status == StatusChamado.Fechado)
        {
            return ResultadoChamado.TransicaoInvalida;
        }

        if (string.IsNullOrWhiteSpace(solucao))
        {
            return ResultadoChamado.TransicaoInvalida;
        }

        chamado.Solucao = solucao;
        chamado.DataFechamento = DateTime.Now;
        chamado.Status = StatusChamado.Fechado;
        _repository.Atualizar(chamado);

        return ResultadoChamado.Validado;
    }
}