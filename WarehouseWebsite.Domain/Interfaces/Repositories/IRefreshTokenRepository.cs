using WarehouseWebsite.Domain.Models;

namespace WarehouseWebsite.Domain.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetRefreshTokenAsync(string token);

        Task<RefreshToken?> GetRefreshTokenByIdAsync(Guid tokenId);

        Task UpdateRefreshTokenAsync(RefreshToken refreshToken);

        Task RevokeRefreshTokenASync(RefreshToken refreshToken);
    }
}
