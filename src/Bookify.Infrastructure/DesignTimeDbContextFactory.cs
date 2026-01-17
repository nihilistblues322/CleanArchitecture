using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Bookify.Infrastructure;

public class DesignTimeDbContextFactory
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder
            .UseNpgsql(
                "Host=localhost;Database=bookify;Username=postgres;Password=postgres")
            .UseSnakeCaseNamingConvention();

        return new ApplicationDbContext(optionsBuilder.Options, null!);
    }
}