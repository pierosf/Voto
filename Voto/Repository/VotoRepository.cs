using Microsoft.EntityFrameworkCore;
using voto.Context;
using voto.Repository.Interfaces;

namespace voto.Repository;

public class VotoRepository : IVotoRepository
{
    private readonly VotoDb _db;
    public VotoRepository(VotoDb db)
    {
        _db = db;
    }

    public async Task<int> Crear(Voto voto)
    {
        await _db.Votos.AddAsync(voto);
        await _db.SaveChangesAsync();
        return voto.Id;
    }

    public async Task<List<Voto>> Obtener()
    {
        return await _db.Votos.ToListAsync();
    }
}