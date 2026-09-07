using Infrastructure.Entities;

namespace Infrastructure.Repositories.Auth;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> FindByHashAsync(string tokenHash);
    Task AddAsync(RefreshToken token);
    Task<bool> TryRevokeAsync(Guid tokenId, string replacementHash, DateTime now);
    Task RevokeFamilyAsync(Guid familyId, DateTime now);
    Task RevokeUserTokensAsync(Guid userId, DateTime now);
}
