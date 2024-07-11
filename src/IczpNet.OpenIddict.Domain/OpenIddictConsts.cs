using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace IczpNet.OpenIddict;

public static class OpenIddictConsts
{

    public static List<string> ClientTypes { get; set; } = [
        OpenIddictConstants.ClientTypes.Confidential,
        OpenIddictConstants.ClientTypes.Public,
    ];

    public static List<string> ConsentTypes { get; set; } = [
         OpenIddictConstants.ConsentTypes.Systematic,
        OpenIddictConstants.ConsentTypes.Explicit,
        OpenIddictConstants.ConsentTypes.External,
        OpenIddictConstants.ConsentTypes.Implicit,
    ];

}
