using System.Text.Json;
using voto.Entities;
using voto.Exceptions;
using voto.ExternalServices.Interfaces;

namespace voto.ExternalServices;

public class VotanteService : IVotanteService
{
    private readonly HttpClient _client;
    public VotanteService(HttpClient client)
    {
        _client = client;
    }

    public async Task<Votante> Obtener(int id)
    {
        try 
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"http://localhost:5000/votantes/{id}"));
            var response = await _client.SendAsync(request, CancellationToken.None);
            response.EnsureSuccessStatusCode();
            var votante = JsonSerializer.Deserialize<Votante>(await response.Content.ReadAsStringAsync());
            return votante;
        }
        catch(Exception ex)
        {
            throw new CustomException($"No se puede encontrar el votante - {ex.Message}",404);
        }

    }
}