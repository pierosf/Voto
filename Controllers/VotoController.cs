using Microsoft.AspNetCore.Mvc;
using voto.Context;
using voto.Services.Interfaces;

namespace voto.Controllers;


[ApiController]
[Route("votos")]
public class VotoController : ControllerBase
{
    private readonly IVotoService _votoService;

    public VotoController(IVotoService votoService)
    {
        _votoService = votoService;
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CrearVoto([FromBody] Voto _nuevoVoto)
    {
        return Ok(await _votoService.Crear(_nuevoVoto));
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> ObtenerResultados()
    {
        return Ok(await _votoService.ObtenerResultados());
    }
}
