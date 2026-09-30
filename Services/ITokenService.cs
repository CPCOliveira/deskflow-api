using Microsoft.AspNetCore.Identity;

namespace DeskFlow.API.Services;

public interface ITokenService
{
    string GerarToken(IdentityUser usuario);
}