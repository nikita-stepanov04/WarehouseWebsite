using Microsoft.EntityFrameworkCore;
using WarehouseWebsite.Domain.Interfaces.Repositories;
using WarehouseWebsite.Domain.Models;
using WarehouseWebsite.Infrastructure.Models;

namespace WarehouseWebsite.Infrastructure.Data
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private DataContext _dbContext;

        public RefreshTokenRepository(DataContext context) => _dbContext = context;

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return await _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task<RefreshToken?> GetRefreshTokenByIdAsync(Guid tokenId)
        {
            return await _dbContext.RefreshTokens.FindAsync(tokenId);
        }

        public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            _dbContext.Update(refreshToken);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RevokeRefreshTokenASync(RefreshToken refreshToken)
        {
            refreshToken.IsRevoked = true;
            await UpdateRefreshTokenAsync(refreshToken);
        }
    }
}
