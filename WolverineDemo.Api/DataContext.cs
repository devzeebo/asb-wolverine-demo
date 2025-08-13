using Microsoft.EntityFrameworkCore;
using Wolverine.EntityFrameworkCore;

namespace WolverineDemo.Api;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.MapWolverineEnvelopeStorage();
    }
}