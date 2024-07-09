using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

using OpenIddict.Abstractions;
using Rctea.IM;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Json;
using Volo.Abp.OpenIddict.Applications;




public class CreateUpdateOpenIddictClientDto
{
    public string ClientId { get; set; }
    public string DisplayName { get; set; }
    public string ClientSecret { get; set; }
    public List<string> GrantTypes { get; set; }
    public List<string> Scopes { get; set; }
    public string RedirectUri { get; set; }
    public string PostLogoutRedirectUri { get; set; }
}


public class OpenIddictClientAppService : ApplicationService
{
    private readonly IAbpApplicationManager _applicationManager;
    private readonly IStringLocalizer<OpenIddictResponse> _localizer;

    protected IOpenIddictApplicationRepository OpenIddictApplicationRepository { get; set; }
    protected IOpenIddictApplicationStore<OpenIddictApplicationModel> AbpOpenIdApplicationStore => LazyServiceProvider.GetRequiredService<IOpenIddictApplicationStore<OpenIddictApplicationModel>>();
    protected IJsonSerializer JsonSerializer { get; set; }

    public OpenIddictClientAppService(
        IAbpApplicationManager applicationManager,
        IStringLocalizer<OpenIddictResponse> localizer,
        IJsonSerializer jsonSerializer,
        IOpenIddictApplicationRepository openIddictApplicationRepository)
    {
        _applicationManager = applicationManager;
        _localizer = localizer;
        JsonSerializer = jsonSerializer;
        OpenIddictApplicationRepository = openIddictApplicationRepository;
    }

    public async Task<ListResultDto<OpenIddictApplicationDto>> GetListAsync()
    {
        var clients = await OpenIddictApplicationRepository.GetPagedListAsync(0, 100, null, true);

        var clientDtos = MapToDto(clients);

        return new ListResultDto<OpenIddictApplicationDto>(clientDtos);
    }

    protected virtual OpenIddictApplicationDto MapToDto(OpenIddictApplication entitiy)
    {
        return ObjectMapper.Map<OpenIddictApplication, OpenIddictApplicationDto>(entitiy);
    }

    protected virtual List<OpenIddictApplicationDto> MapToDto(List<OpenIddictApplication> entitiies)
    {
        return entitiies.Select(MapToDto).ToList();
    }

    public async Task<OpenIddictApplicationDto> GetAsync(Guid id)
    {
        var client = await _applicationManager.FindByIdAsync(id.ToString());
        if (client == null)
        {
            throw new EntityNotFoundException(typeof(OpenIddictApplicationModel), id);
        }

        var application = client as OpenIddictApplicationModel;
        return new OpenIddictApplicationDto
        {
            ClientId = application.ClientId,
            DisplayName = application.DisplayName,
            //ClientType = application.Type,
            //ClientSecret = application.ClientSecret,
            //GrantTypes = JsonConvert.DeserializeObject<List<string>>(application.Permissions).Where(p => p.StartsWith(OpenIddictConstants.Permissions.Prefixes.GrantType)).ToList(),
            //Scopes = JsonConvert.DeserializeObject<List<string>>(application.Permissions).Where(p => p.StartsWith(OpenIddictConstants.Permissions.Prefixes.Scope)).ToList(),
            //RedirectUri = JsonConvert.DeserializeObject<List<string>>(application.RedirectUris).FirstOrDefault(),
            //PostLogoutRedirectUri = JsonConvert.DeserializeObject<List<string>>(application.PostLogoutRedirectUris).FirstOrDefault()
        };
    }

    public async Task<OpenIddictApplicationDto> CreateAsync(CreateUpdateOpenIddictClientDto input)
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
        await _applicationManager.PopulateAsync(application, applicationDescriptor);
        await _applicationManager.CreateAsync(application);

        return ObjectMapper.Map<OpenIddictApplicationModel, OpenIddictApplicationDto>(application);
    }

    public async Task<OpenIddictApplicationDto> UpdateAsync(Guid id, CreateUpdateOpenIddictClientDto input)
    {
        var application = await _applicationManager.FindByIdAsync(id.ToString());
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

        await _applicationManager.PopulateAsync(application, applicationDescriptor);
        await _applicationManager.UpdateAsync(application);

        return ObjectMapper.Map<OpenIddictApplicationModel, OpenIddictApplicationDto>(application as OpenIddictApplicationModel);
    }

    public async Task DeleteAsync(Guid id)
    {
        var application = await _applicationManager.FindByIdAsync(id.ToString());
        if (application == null)
        {
            throw new EntityNotFoundException(typeof(OpenIddictApplicationModel), id);
        }

        await _applicationManager.DeleteAsync(application);
    }
}
