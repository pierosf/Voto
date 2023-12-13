namespace voto.Entities;

public class Resultado
{
    public int? CandidatoId { get; set; }
    public required string Candidato { get; set; }
    public int Votos { get; set; }
}
