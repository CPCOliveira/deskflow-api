using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DeskFlow.API.Models.Requests;
using DeskFlow.API.Services;

namespace DeskFlow.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthController(UserManager<IdentityUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] RegistrarUsuarioRequest request)
    {
        var usuario = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        var resultado = await _userManager.CreateAsync(usuario, request.Senha);

        if (!resultado.Succeeded)
        {
            return BadRequest(resultado.Errors.Select(e => e.Description));
        }

        return Ok("Usuário registrado com sucesso.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var usuario = await _userManager.FindByEmailAsync(request.Email);

        if (usuario == null)
        {
            return Unauthorized("Email ou senha inválidos.");
        }

        var senhaValida = await _userManager.CheckPasswordAsync(usuario, request.Senha);

        if (!senhaValida)
        {
            return Unauthorized("Email ou senha inválidos.");
        }

        var token = _tokenService.GerarToken(usuario);
        return Ok(new { token });
    }
}