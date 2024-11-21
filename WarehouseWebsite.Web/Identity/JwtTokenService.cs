using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WarehouseWebsite.Domain.Interfaces.Repositories;
using WarehouseWebsite.Domain.Models;

namespace WarehouseWebsite.Web.Identity
{
    public class JwtTokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IMemoryCache _cache;
        private readonly IRefreshTokenRepository _tokenRepository;

        public JwtTokenService(IOptions<JwtSettings> jwtSettings,
            IRefreshTokenRepository tokenRepository,
            IMemoryCache cache)
        {
            _jwtSettings = jwtSettings.Value;
            _tokenRepository = tokenRepository;
            _cache = cache;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<RefreshToken?> GetStoredRefreshTokenAsync(string token)
        {
            return await _tokenRepository.GetRefreshTokenAsync(token);
        }

        public async Task<RefreshToken?> GetRefreshTokenByIdAsync(Guid tokenId)
        {
            return await _tokenRepository.GetRefreshTokenByIdAsync(tokenId);
        }

        public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _tokenRepository.UpdateRefreshTokenAsync(refreshToken);
        }

        public async Task<bool> RevokeTokensAsync(string accessToken, Guid refreshTokenId)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);
            var expiration = jwtToken.ValidTo;

            _cache.Set(accessToken, true, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(expiration - DateTime.UtcNow));

            var refreshToken = await _tokenRepository.GetRefreshTokenByIdAsync(refreshTokenId);

            if (refreshToken == null) return false;

            await _tokenRepository.RevokeRefreshTokenASync(refreshToken);
            return true;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (!jwtSecurityToken!.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
