using voto.Entities;

namespace voto.ExternalServices.Interfaces;

public interface ICandidatoService
{
    Task<Candidato> Obtener(int id);
}