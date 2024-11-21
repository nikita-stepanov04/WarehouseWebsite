using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WarehouseWebsite.Domain.Interfaces.Repositories;
using WarehouseWebsite.Domain.Models;
using WarehouseWebsite.Infrastructure.Data;
using WarehouseWebsite.Infrastructure.Models;
using WarehouseWebsite.Web.Identity;
using static WarehouseWebsite.Tests.UnitTestHelper;

namespace WarehouseWebsite.Tests.WebTests
{
    [TestFixture]
    public class JwtTokenServiceTests
    {
        private Mock<IOptions<JwtSettings>> _mockOptions;
        private DataContext _dataContext;
        private IMemoryCache _cache;
        private IRefreshTokenRepository _refreshTokenRepository;
        private JwtTokenService _jwtTokenService;
        private JwtSettings _jwtSettings;
        private RefreshToken _refreshToken;
        private List<Claim> _claims;

        [SetUp]
        public void SetUp()
        {
            _jwtSettings = new JwtSettings 
            { 
                Key = "cYJjDqsfYRLrtwlkAgWNrmDjZZcPDTIs",
                AccessTokenExpirationMinutes = 30,
                RefreshTokenExpirationDays = 30
            };
            _mockOptions = new Mock<IOptions<JwtSettings>>();
            _mockOptions.Setup(o => o.Value).Returns(_jwtSettings);
            _dataContext = new DataContext(GetUnitTestDbOptions());

            _refreshToken = new RefreshToken
            {
                Token = "TestToken",
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
            };
            _dataContext.RefreshTokens.Add(_refreshToken);
            _dataContext.SaveChanges();

            _claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "username"),
                new Claim(ClaimTypes.Role, "role")
            };

            _refreshTokenRepository = new RefreshTokenRepository(_dataContext);
            _cache = new MemoryCache(new MemoryCacheOptions());
            _jwtTokenService = new JwtTokenService(_mockOptions.Object, _refreshTokenRepository, _cache);
        }

        [TearDown]
        public void TearDown()
        {
            _dataContext.Dispose();
            _cache.Dispose();
        }

        [Test]
        public void JwtTokenServiceGenerateAccessTokenGeneratesValidAccessToken()
        {
            var token = _jwtTokenService.GenerateAccessToken(_claims);

            Assert.That(token, Is.Not.Null);
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            Assert.That(jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Name).Value, Is.EqualTo("username"));
            Assert.That(jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Role).Value, Is.EqualTo("role"));
        }

        [Test]
        public async Task JwtTokenServiceGetStoredRefreshTokenAsyncReturnsStoredRefreshToken()
        {
            var storedToken = await _jwtTokenService.GetStoredRefreshTokenAsync("TestToken");
            Assert.That(storedToken, Is.Not.Null);
            Assert.That(storedToken, Is.EqualTo(_refreshToken).Using(new RefreshTokenEqualityComparer()));
        }
        
        [Test]
        public async Task JwtTokenServiceGetRefreshTokenByIdAsyncReturnsStoredRefreshToken()
        {
            var storedToken = await _jwtTokenService.GetRefreshTokenByIdAsync(_refreshToken.Id);
            Assert.That(storedToken, Is.Not.Null);
            Assert.That(storedToken, Is.EqualTo(_refreshToken).Using(new RefreshTokenEqualityComparer()));
        }
        
        [Test]
        public async Task JwtTokenServiceRevokeTokensAsyncRevokesStoredRefreshToken()
        {
            string token = _jwtTokenService.GenerateAccessToken(_claims);
            await _jwtTokenService.RevokeTokensAsync(token, _refreshToken.Id);

            var storedToken = await _jwtTokenService.GetRefreshTokenByIdAsync(_refreshToken.Id);

            Assert.That(storedToken, Is.Not.Null);
            Assert.That(storedToken!.IsRevoked, Is.EqualTo(true));
            Assert.That(_cache.TryGetValue(token, out _));
        }
        
        [Test]
        public void JwtTokenServiceGetPrincipalFromExpiredTokenGetsValidPrincipalFromToken()
        {
            string token = _jwtTokenService.GenerateAccessToken(_claims);
            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(token);

            Assert.That(principal, Is.Not.Null);
            Assert.That(principal!.Claims.First(c => c.Type == ClaimTypes.Name).Value, Is.EqualTo("username"));
            Assert.That(principal!.Claims.First(c => c.Type == ClaimTypes.Role).Value, Is.EqualTo("role"));
        }
        
        [Test]
        public async Task JwtTokenServiceUpdateRefreshTokenAsyncUpdatesStoredRefreshToken()
        {
            _refreshToken.Token = "NewTestToken";
            var created = DateTime.Parse("2023-11-12");
            var expires = DateTime.Parse("2023-11-12");

            _refreshToken.Created = created;
            _refreshToken.Expires = expires;

            await _jwtTokenService.UpdateRefreshTokenAsync(_refreshToken);

            var storedToken = await _jwtTokenService.GetRefreshTokenByIdAsync(_refreshToken.Id);

            Assert.That(storedToken, Is.Not.Null);
            Assert.That(storedToken!.Token, Is.EqualTo("NewTestToken"));
            Assert.That(storedToken!.Created, Is.EqualTo(created));
            Assert.That(storedToken!.Expires, Is.EqualTo(expires));
        }
    }
}
