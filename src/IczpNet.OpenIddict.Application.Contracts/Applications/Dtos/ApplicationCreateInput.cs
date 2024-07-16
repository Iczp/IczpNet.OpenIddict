using System.Collections.Generic;
using System.ComponentModel;

namespace IczpNet.OpenIddict.Applications.Dtos;

public class ApplicationCreateInput: ApplicationUpdateInput
{

    /// <summary>
    /// 
    /// </summary>
    [DefaultValue(null)]
    public virtual string ClientSecret { get; set; }
}
