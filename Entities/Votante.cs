using System.Globalization;
using System.Text.Json.Serialization;

namespace voto.Entities;

public class Votante
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("fechaNacimiento")]
    public required string FechaNacimiento { get; set; }

    public bool EsMayorDeEdad()
    {
        DateTime fechaNacimiento = DateTime.ParseExact(this.FechaNacimiento, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        DateTime hoy = DateTime.Now;
        int diferencia = hoy.Year - fechaNacimiento.Year;
        if (hoy.Month < fechaNacimiento.Month || (hoy.Month == fechaNacimiento.Month && hoy.Day < fechaNacimiento.Day)) diferencia--;
        return diferencia > 16;
    }
}

