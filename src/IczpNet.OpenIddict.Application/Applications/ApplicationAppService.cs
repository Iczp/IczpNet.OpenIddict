using System;
using System.Threading.Tasks;
using IczpNet.OpenIddict.Applications.Dtos;
using IczpNet.OpenIddict.BaseAppServices;
using IczpNet.OpenIddict.BaseDtos;
using IczpNet.OpenIddict.Permissions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

using OpenIddict.Abstractions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Json;
using Volo.Abp.OpenIddict.Applications;
namespace IczpNet.OpenIddict.Applications;

public class ApplicationAppService : CrudOpenIddictAppService<OpenIddictApplication, OpenIddictApplicationDto, OpenIddictApplicationDto, Guid, GetListInput, CreateInput, UpdateInput>, IApplicationAppService
{


    protected override string GetListPolicyName { get; set; } = OpenIddictPermissions.ApplicationPermissions.GetList;
    protected override string GetPolicyName { get; set; } = OpenIddictPermissions.ApplicationPermissions.GetItem;
    protected override string CreatePolicyName { get; set; } = OpenIddictPermissions.ApplicationPermissions.Create;
    protected override string UpdatePolicyName { get; set; } = OpenIddictPermissions.ApplicationPermissions.Update;
    protected override string DeletePolicyName { get; set; } = OpenIddictPermissions.ApplicationPermissions.Delete;


    protected IAbpApplicationManager ApplicationManager { get; set; }

    private readonly IStringLocalizer<OpenIddictResponse> _localizer;

    protected IOpenIddictApplicationRepository OpenIddictApplicationRepository { get; set; }
    protected IOpenIddictApplicationStore<OpenIddictApplicationModel> AbpOpenIdApplicationStore => LazyServiceProvider.GetRequiredService<IOpenIddictApplicationStore<OpenIddictApplicationModel>>();
    protected IJsonSerializer JsonSerializer { get; set; }

    public ApplicationAppService(
        IRepository<OpenIddictApplication, Guid> repository,
        IAbpApplicationManager applicationManager,
        IStringLocalizer<OpenIddictResponse> localizer,
        IJsonSerializer jsonSerializer,
        IOpenIddictApplicationRepository openIddictApplicationRepository) : base(repository)
    {
        ApplicationManager = applicationManager;
        _localizer = localizer;
        JsonSerializer = jsonSerializer;
        OpenIddictApplicationRepository = openIddictApplicationRepository;
    }

    public async Task<OpenIddictApplicationDto> Create1Async(ApplicationCreateUpdateInput input)
    {
        var applicationDescriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = input.ClientId,
            ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
            DisplayName = input.DisplayName,
            ClientSecret = input.ClientSecret,
            //Permissions = input.GrantTypes.Select(gt => OpenIddictConstants.Permissions.Prefixes.GrantType + gt).Concat(
            //              input.Scopes.Select(s => OpenIddictConstants.Permissions.Prefixes.Scope + s)).ToList(),
            //RedirectUris = new List<Uri> { new Uri(input.RedirectUri) },
            //PostLogoutRedirectUris = new List<Uri> { new Uri(input.PostLogoutRedirectUri) }
        };

        var application = new OpenIddictApplicationModel();
        await ApplicationManager.PopulateAsync(application, applicationDescriptor);
        await ApplicationManager.CreateAsync(application);

        return ObjectMapper.Map<OpenIddictApplicationModel, OpenIddictApplicationDto>(application);
    }

    public async Task<OpenIddictApplicationDto> Update1Async(Guid id, ApplicationCreateUpdateInput input)
    {
        var application = await ApplicationManager.FindByIdAsync(id.ToString());
        if (application == null)
        {
            throw new EntityNotFoundException(typeof(OpenIddictApplicationModel), id);
        }

        var applicationDescriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = input.ClientId,
            ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
            DisplayName = input.DisplayName,
            ClientSecret = input.ClientSecret,
            //Permissions = input.GrantTypes.Select(gt => OpenIddictConstants.Permissions.Prefixes.GrantType + gt).Concat(
            //              input.Scopes.Select(s => OpenIddictConstants.Permissions.Prefixes.Scope + s)).ToList(),
            //RedirectUris = new List<Uri> { new Uri(input.RedirectUri) },
            //PostLogoutRedirectUris = new List<Uri> { new Uri(input.PostLogoutRedirectUri) }
        };

        await ApplicationManager.PopulateAsync(application, applicationDescriptor);
        await ApplicationManager.UpdateAsync(application);

        return ObjectMapper.Map<OpenIddictApplicationModel, OpenIddictApplicationDto>(application as OpenIddictApplicationModel);
    }




    

    public async Task Delete1Async(Guid id)
    {
        var application = await ApplicationManager.FindByIdAsync(id.ToString());
        if (application == null)
        {
            throw new EntityNotFoundException(typeof(OpenIddictApplicationModel), id);
        }

        await ApplicationManager.DeleteAsync(application);
    }
}
