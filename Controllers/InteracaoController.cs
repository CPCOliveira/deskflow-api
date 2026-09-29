using Microsoft.AspNetCore.Mvc;
using DeskFlow.API.Models.Requests;
using DeskFlow.API.Services;

namespace DeskFlow.API.Controllers;

[ApiController]
[Route("api/chamados/{chamadoId}/interacoes")]
public class InteracaoController : ControllerBase
{
    private readonly IInteracaoService _interacaoService;

    public InteracaoController(IInteracaoService interacaoService)
    {
        _interacaoService = interacaoService;
    }

    [HttpPost]
    public IActionResult Adicionar(int chamadoId, [FromBody] InteracaoRequest request)
    {
        var interacao = _interacaoService.Adicionar(chamadoId, request.Autor, request.Mensagem);

        if (interacao == null)
        {
            return NotFound();
        }

        return CreatedAtAction(nameof(ListarPorChamado), new { chamadoId }, interacao);
    }

    [HttpGet]
    public IActionResult ListarPorChamado(int chamadoId)
    {
        var interacoes = _interacaoService.ListarPorChamado(chamadoId);

        if (interacoes == null)
        {
            return NotFound();
        }

        return Ok(interacoes);
    }
}