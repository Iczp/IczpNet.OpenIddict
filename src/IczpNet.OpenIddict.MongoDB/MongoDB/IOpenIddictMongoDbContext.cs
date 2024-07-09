﻿using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace IczpNet.OpenIddict.MongoDB;

[ConnectionStringName(OpenIddictDbProperties.ConnectionStringName)]
public interface IOpenIddictMongoDbContext : IAbpMongoDbContext
{
    /* Define mongo collections here. Example:
     * IMongoCollection<Question> Questions { get; }
     */
}
