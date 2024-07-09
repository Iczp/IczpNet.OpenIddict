using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace IczpNet.OpenIddict.EntityFrameworkCore;

public class OpenIddictHttpApiHostMigrationsDbContext : AbpDbContext<OpenIddictHttpApiHostMigrationsDbContext>
{
    public OpenIddictHttpApiHostMigrationsDbContext(DbContextOptions<OpenIddictHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureOpenIddict();
    }
}
