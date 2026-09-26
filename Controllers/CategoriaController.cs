using Microsoft.AspNetCore.Mvc;
using DeskFlow.API.Services;
using DeskFlow.API.Models.Requests;

namespace DeskFlow.API.Controllers;

[ApiController]
[Route("api/categorias")]

public class CategoriaController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriaController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpPost]
    
    public IActionResult Cadastrar([FromBody] CategoriaRequest request)
    {
        var categoria = _categoriaService.Cadastrar(request.Nome);
        return CreatedAtAction(nameof(BuscarPorId), new { id = categoria.Id }, categoria);
    }

    [HttpGet]

    public IActionResult ListarTodas()
    {
        var categorias = _categoriaService.ListarTodas();
        return Ok(categorias);
    }

    [HttpGet("{id}")]
    
    public IActionResult BuscarPorId(int id)
    {
        var categoria = _categoriaService.BuscarPorId(id);
        if (categoria == null)
        {
            return NotFound();
        }
        return Ok(categoria);
    }

    [HttpPut("{id}")]

    public IActionResult Atualizar(int id, [FromBody] CategoriaRequest request)
    {
        var sucesso = _categoriaService.AtualizarNome(id, request.Nome);
        if (!sucesso)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]

    public IActionResult Deletar(int id)
    {
        var sucesso = _categoriaService.Deletar(id);
        if (!sucesso)
        {
            return NotFound();
        }
        return NoContent();
    }
}