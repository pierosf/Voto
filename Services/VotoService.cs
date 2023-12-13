using voto.Context;
using voto.Entities;
using voto.Exceptions;
using voto.ExternalServices.Interfaces;
using voto.Repository.Interfaces;
using voto.Services.Interfaces;

namespace voto.Services;

public class VotoService : IVotoService
{
    private readonly IVotanteService _votanteService;
    private readonly ICandidatoService _candidatoService;
    private readonly IVotoRepository _votoBD;
    public VotoService(IVotanteService votanteService, ICandidatoService candidatoService, IVotoRepository votoBD)
    {
        _votanteService = votanteService;
        _votoBD = votoBD;
        _candidatoService = candidatoService;
    }
    public async Task<int> Crear(Voto voto)
    {
        var votante = await _votanteService.Obtener(voto.VotanteId);
        if (votante.EsMayorDeEdad())
        {
            return await _votoBD.Crear(voto);
        }
        else 
        {
            throw new CustomException("El votante no tiene la edad para votar", 500);        
        }
    }

    public async Task<List<Resultado>> ObtenerResultados()
    {
        var resultadosVotaciones = new List<Resultado>();
        var votos = await _votoBD.Obtener();
        resultadosVotaciones = votos.GroupBy(v => v.CandidatoId).Select(v => new Resultado(){CandidatoId = v.Key, Votos = v.Count(), Candidato = string.Empty}).ToList();
        List<Task<Resultado>> candidatosDesdeApi = new();
        resultadosVotaciones.ForEach((r) => candidatosDesdeApi.Add(RecuperarCandidatoApi(r)));
        await Task.WhenAll(candidatosDesdeApi);
        return candidatosDesdeApi.Select(cda => cda.Result).ToList();
    }

    private async Task<Resultado> RecuperarCandidatoApi(Resultado resultado)
    {
        if (resultado.CandidatoId.HasValue)
        {
            var candidatoDesdeApi = await _candidatoService.Obtener(resultado.CandidatoId.Value);
            resultado.Candidato = $"{candidatoDesdeApi.Nombre} {candidatoDesdeApi.Apellido}";
        }
        else
        {
            resultado.Candidato = "Nulo";
        }
        return resultado;
    }
}