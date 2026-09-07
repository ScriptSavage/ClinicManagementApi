using Infrastructure.Context;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Auth;

public class RefreshTokenRepository(DatabaseContext context) : IRefreshTokenRepository
{
    public Task<RefreshToken?> FindByHashAsync(string tokenHash) =>
        context.RefreshTokens.AsNoTracking().SingleOrDefaultAsync(x => x.TokenHash == tokenHash);

    public async Task AddAsync(RefreshToken token) => await context.RefreshTokens.AddAsync(token);

    public async Task<bool> TryRevokeAsync(Guid tokenId, string replacementHash, DateTime now)
    {
        var affectedRows = await context.RefreshTokens
            .Where(x => x.RefreshTokenId == tokenId && x.Revoked == null && x.Expires > now)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(x => x.Revoked, now)
                .SetProperty(x => x.ReplacedByTokenHash, replacementHash));

        return affectedRows == 1;
    }

    public async Task RevokeFamilyAsync(Guid familyId, DateTime now) =>
        await context.RefreshTokens
            .Where(x => x.FamilyId == familyId && x.Revoked == null)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Revoked, now));

    public async Task RevokeUserTokensAsync(Guid userId, DateTime now) =>
        await context.RefreshTokens
            .Where(x => x.UserId == userId && x.Revoked == null)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Revoked, now));
}
