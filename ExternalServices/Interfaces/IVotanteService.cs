using voto.Entities;

namespace voto.ExternalServices.Interfaces;

public interface IVotanteService
{
    Task<Votante> Obtener (int id);
}