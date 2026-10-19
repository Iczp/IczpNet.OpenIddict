using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IczpNet.OpenIddict.Tokens;

public record RevokedTokenState(Guid Id, DateTime? ExpirationDate);

/// <summary>Bounded metadata queries used only by the authority's revocation recovery.</summary>
public interface ITokenRevocationRepository
{
    Task<List<RevokedTokenState>> ReadTokensAsync(Guid? after, int count, string subject = null, Guid? authorizationId = null, CancellationToken cancellationToken = default);
    Task<List<Guid>> ReadAuthorizationsAsync(Guid? after, int count, string subject = null, CancellationToken cancellationToken = default);
    Task<bool> HasUnexpiredTokensAsync(Guid? authorizationId = null, Guid? applicationId = null, CancellationToken cancellationToken = default);
}
