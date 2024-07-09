using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace IczpNet.OpenIddict.EntityFrameworkCore;

public class OpenIddictHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<OpenIddictHttpApiHostMigrationsDbContext>
{
    public OpenIddictHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<OpenIddictHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("OpenIddict"));

        return new OpenIddictHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
