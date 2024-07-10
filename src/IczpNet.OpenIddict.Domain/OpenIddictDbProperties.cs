﻿namespace IczpNet.OpenIddict;

public static class OpenIddictDbProperties
{
    public static string DbTablePrefix { get; set; } = "OpenIddict";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "OpenIddict";
}
