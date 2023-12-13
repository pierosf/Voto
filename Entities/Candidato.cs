using System.Text.Json.Serialization;

namespace voto.Entities;

public class Candidato
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("nombre")]
    public required string Nombre { get; set; }
    [JsonPropertyName("apellido")]
    public required string Apellido { get; set; }
}