using System.Text.Json;
using voto.Entities;
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
        var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"http://localhost:5117/candidatos/{id}"));
        var response = await _client.SendAsync(request, CancellationToken.None);
        response.EnsureSuccessStatusCode();
        var candidato = JsonSerializer.Deserialize<Candidato>(await response.Content.ReadAsStringAsync());
        return candidato;
    }
}