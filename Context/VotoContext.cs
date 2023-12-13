using Microsoft.EntityFrameworkCore;

namespace voto.Context;

public class VotoDb : DbContext
{
    public VotoDb(DbContextOptions<VotoDb> options) : base(options) { 
        Database.EnsureCreated();
    }

    public DbSet<Voto> Votos => Set<Voto>();
    public DbSet<Log> Logs => Set<Log>();
}
