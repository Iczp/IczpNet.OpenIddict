using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;

namespace IczpNet.OpenIddict.EntityFrameworkCore;

[ConnectionStringName(OpenIddictDbProperties.ConnectionStringName)]
public class OpenIddictDbContext : AbpDbContext<OpenIddictDbContext>, IOpenIddictDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public OpenIddictDbContext(DbContextOptions<OpenIddictDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureOpenIddict();
    }
}
