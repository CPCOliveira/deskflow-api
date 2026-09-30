using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Requests;
using DeskFlow.API.Services;

namespace DeskFlow.API.Controllers;

[ApiController]
[Route("api/chamados")]
[Authorize]
public class ChamadoController : ControllerBase
{
    private readonly IChamadoService _chamadoService;

    public ChamadoController(IChamadoService chamadoService)
    {
        _chamadoService = chamadoService;
    }

    [HttpPost]
    public IActionResult AbrirChamado([FromBody] ChamadoRequest request)
    {
        var chamado = _chamadoService.AbrirChamado(request);

        if (chamado == null)
        {
            return BadRequest("A categoria informada não existe.");
        }

        return CreatedAtAction(nameof(BuscarPorId), new { id = chamado.Id }, chamado);
    }

    [HttpGet]
    public IActionResult ListarTodos([FromQuery] StatusChamado? status, [FromQuery] Prioridade? prioridade, [FromQuery] int? categoriaId)
    {
        var chamados = _chamadoService.ListarTodos(status, prioridade, categoriaId);
        return Ok(chamados);
    }

    [HttpGet("{id}")]
    public IActionResult BuscarPorId(int id)
    {
        var chamado = _chamadoService.BuscarPorId(id);

        if (chamado == null)
        {
            return NotFound();
        }

        return Ok(chamado);
    }

    [HttpPost("{id}/iniciar")]
    public IActionResult IniciarAtendimento(int id)
    {
        var resultado = _chamadoService.IniciarAtendimento(id);

        return resultado switch
        {
            ResultadoChamado.Validado => NoContent(),
            ResultadoChamado.NaoLocalizado => NotFound(),
            ResultadoChamado.TransicaoInvalida => BadRequest("O chamado não está com status 'Aberto'; não é possível iniciar o atendimento."),
            _ => BadRequest()
        };
    }

    [HttpPost("{id}/encerrar")]
    public IActionResult Encerrar(int id, [FromBody] EncerrarChamadoRequest request)
    {
        var resultado = _chamadoService.Encerrar(id, request.Solucao);

        return resultado switch
        {
            ResultadoChamado.Validado => NoContent(),
            ResultadoChamado.NaoLocalizado => NotFound(),
            ResultadoChamado.TransicaoInvalida => BadRequest("Não é possível encerrar este chamado (já está fechado ou a solução não foi informada)."),
            _ => BadRequest()
        };
    }
}