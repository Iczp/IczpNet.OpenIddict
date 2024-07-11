using System;
using System.Threading.Tasks;
using IczpNet.AbpCommons;
using IczpNet.OpenIddict.Applications.Dtos;
using IczpNet.OpenIddict.BaseAppServices;
using IczpNet.OpenIddict.BaseDtos;
using IczpNet.OpenIddict.Permissions;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.OpenIddict.Applications;
namespace IczpNet.OpenIddict.Applications;

public class ApplicationAppService : CrudOpenIddictAppService<OpenIddictApplication, OpenIddictApplicationDto, OpenIddictApplicationDto, Guid, GetListInput, ApplicationCreateInput, ApplicationUpdateInput>, IApplicationAppService
{

    protected override string GetListPolicyName { get; set; } = OpenIddictPermissions.ApplicationPermissions.GetList;
    protected override string GetPolicyName { get; set; } = OpenIddictPermissions.ApplicationPermissions.GetItem;
    protected override string CreatePolicyName { get; set; } = OpenIddictPermissions.ApplicationPermissions.Create;
    protected override string UpdatePolicyName { get; set; } = OpenIddictPermissions.ApplicationPermissions.Update;
    protected override string DeletePolicyName { get; set; } = OpenIddictPermissions.ApplicationPermissions.Delete;
    protected IApplicationManager ApplicationManager { get; set; }

    public ApplicationAppService(
        IRepository<OpenIddictApplication, Guid> repository,
        IApplicationManager applicationManager) : base(repository)
    {
        ApplicationManager = applicationManager;
    }

    [HttpPost]
    public override async Task<OpenIddictApplicationDto> CreateAsync(ApplicationCreateInput input)
    {
        await CheckCreatePolicyAsync(input);

        await CheckCreateAsync(input);

        var application = await ApplicationManager.CreateApplicationAsync(
             name: input.ClientId,
             type: input.Type,
             consentType: input.ConsentType,
             displayName: input.DisplayName,
             secret: input.ClientSecret,
             grantTypes: input.GrantTypes,
             scopes: input.Scopes,
             redirectUri: input.RedirectUri,
             postLogoutRedirectUri: input.PostLogoutRedirectUri,
             permissions: input.Permissions);

        var entity = await GetEntityByIdAsync(application.Id);

        return await MapToGetOutputDtoAsync(entity);
    }

    [HttpPost]
    public override async Task<OpenIddictApplicationDto> UpdateAsync(Guid id, ApplicationUpdateInput input)
    {

        await CheckUpdatePolicyAsync(id, input);

        var application = (await ApplicationManager.FindByIdAsync(id.ToString())).As<OpenIddictApplicationModel>();

        await ApplicationManager.UpdateApplicationAsync(application,
               type: input.Type,
               consentType: input.ConsentType,
              displayName: input.DisplayName,
              secret: input.ClientSecret,
              grantTypes: input.GrantTypes,
              scopes: input.Scopes,
              redirectUri: input.RedirectUri,
              postLogoutRedirectUri: input.PostLogoutRedirectUri
              );

        //Assert.If(!await ApplicationManager.ValidatePostLogoutRedirectUriAsync(application, input.PostLogoutRedirectUri), $"PostLogoutRedirectUri Failed:{input.PostLogoutRedirectUri}");

        //Assert.If(!await ApplicationManager.ValidateRedirectUriAsync(application, input.RedirectUri), $"RedirectUri Failed:{input.RedirectUri}");


        //var entity = await GetEntityByIdAsync(id);
        //await CheckUpdateAsync(id, entity, input);
        //await MapToEntityAsync(input, entity);
        //await SetUpdateEntityAsync(entity, input);

        //================


        //var applicationDescriptor = new AbpApplicationDescriptor
        //{
        //    ClientId = input.ClientId,
        //    ConsentType = input.ConsentType,
        //    DisplayName = input.DisplayName,
        //    //ClientSecret = input.ClientSecret,
        //    Type = input.Type,
        //    LogoUri = input.LogoUri,
        //    ClientUri = input.ClientUri,
        //};

        //await ApplicationManager.PopulateAsync(application, applicationDescriptor);

        //await ApplicationManager.SetClientSecretAsync(application, input.ClientSecret);

        //await ApplicationManager.UpdateAsync(application);

        var entity = await GetEntityByIdAsync(application.Id);

        return await MapToGetOutputDtoAsync(entity);

    }

    [HttpPost]
    public async Task<OpenIddictApplicationDto> Update1Async(Guid id, ApplicationUpdateInput input)
    {
        var application = await ApplicationManager.FindByIdAsync(id.ToString());

        Assert.If(application == null, $"EntityNotFound,id:{id}");

        var applicationDescriptor = new OpenIddictApplicationDescriptor
        {
            ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
            DisplayName = input.DisplayName,
            ClientSecret = input.ClientSecret,
            Type = input.Type,

            //Permissions = input.GrantTypes?.Select(gt => OpenIddictConstants.Permissions.Prefixes.GrantType + gt).Concat(
            //              input.Scopes.Select(s => OpenIddictConstants.Permissions.Prefixes.Scope + s)).ToList(),
            //RedirectUris = new List<Uri> { new Uri(input.RedirectUri) },
            //PostLogoutRedirectUris = [new Uri(input.PostLogoutRedirectUri)]
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
