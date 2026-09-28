using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Models.Requests;

public class ChamadoRequest
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string SolicitanteNome { get; set; } = string.Empty;
    public Prioridade Prioridade { get; set; }
    public int CategoriaId { get; set; }
}