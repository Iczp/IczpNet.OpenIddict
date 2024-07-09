using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace IczpNet.OpenIddict.EntityFrameworkCore;

[ConnectionStringName(OpenIddictDbProperties.ConnectionStringName)]
public interface IOpenIddictDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
