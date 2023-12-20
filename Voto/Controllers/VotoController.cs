using Microsoft.AspNetCore.Mvc;
using voto.Context;
using voto.Entities;
using voto.Services.Interfaces;

namespace voto.Controllers;


[ApiController]
[Route("votos")]
[Produces("application/json")]
public class VotoController : ControllerBase
{
    private readonly IVotoService _votoService;

    public VotoController(IVotoService votoService)
    {
        _votoService = votoService;
    }

    /// <summary>
    /// Crear un nuevo Voto
    /// </summary>
    /// <param name="_nuevoVoto"></param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /votos
    ///     {
    ///        "candidatoId": 1,
    ///        "votanteId": 3
    ///     }
    ///
    /// Autor
    ///
    ///     Piero Maritano
    ///
    /// </remarks>
    /// <response code="200">Devuelve el identificador del voto creado</response>
    /// <response code="404">Si no se encuentra el votante</response>
    /// <response code="500">Si el votante no esta habilitado</response>
    [HttpPost]
    [Route("")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorEntity), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorEntity), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CrearVoto([FromBody] Voto _nuevoVoto)
    {
        return Ok(await _votoService.Crear(_nuevoVoto));
    }

    /// <summary>
    /// Obtiene los resultados de la votación
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> ObtenerResultados()
    {
        return Ok(await _votoService.ObtenerResultados());
    }
}
