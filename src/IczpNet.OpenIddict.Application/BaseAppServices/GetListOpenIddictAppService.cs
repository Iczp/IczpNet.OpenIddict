using Volo.Abp.Domain.Repositories;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using System.Threading.Tasks;
using IczpNet.AbpCommons.Dtos;
using Volo.Abp;
using System;


namespace IczpNet.OpenIddict.BaseAppServices;

public abstract class GetListOpenIddictAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>
    : CrudOpenIddictAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, CreateInput, UpdateInput>
    where TEntity : class, IEntity<TKey>
    where TGetOutputDto : IEntityDto<TKey>
    where TGetListOutputDto : IEntityDto<TKey>
{
    protected GetListOpenIddictAppService(IRepository<TEntity, TKey> repository) : base(repository)
    {

    }

    [RemoteService(false)]
    public override Task<TGetOutputDto> UpdateAsync(TKey id, UpdateInput input)
    {
        throw new NotImplementedException();
    }

    [RemoteService(false)]
    public override Task<TGetOutputDto> CreateAsync(CreateInput input)
    {
        throw new NotImplementedException();
    }

}
