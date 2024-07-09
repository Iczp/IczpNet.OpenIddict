﻿using Volo.Abp;
using Volo.Abp.MongoDB;

namespace IczpNet.OpenIddict.MongoDB;

public static class OpenIddictMongoDbContextExtensions
{
    public static void ConfigureOpenIddict(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
