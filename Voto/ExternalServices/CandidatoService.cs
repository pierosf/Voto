using System.Text.Json;
using voto.Entities;
using voto.Exceptions;
using voto.ExternalServices.Interfaces;

namespace voto.ExternalServices;

public class CandidatoService : ICandidatoService
{
    private readonly HttpClient _client;
    public CandidatoService(HttpClient client)
    {
        _client = client;
    }

    public async Task<Candidato> Obtener(int id)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"http://localhost:5117/candidatos/{id}"));
            var response = await _client.SendAsync(request, CancellationToken.None);
            response.EnsureSuccessStatusCode();
            var candidato = JsonSerializer.Deserialize<Candidato>(await response.Content.ReadAsStringAsync());
            return candidato ?? throw new CustomException($"No se puede encontrar el candidato",404);;
        }
        catch (Exception ex)
        {
            throw new CustomException($"No se puede encontrar el candidato - {ex.Message}",404);
        }
    }
}