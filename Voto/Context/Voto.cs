using System.ComponentModel.DataAnnotations;

namespace voto.Context;

public class Voto
{
    public int Id { get; set; }
    [Required]
    public int VotanteId { get; set; }
    public int? CandidatoId { get; set; }
}