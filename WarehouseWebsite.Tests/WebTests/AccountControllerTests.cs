using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System.Security.Claims;
using WarehouseWebsite.Domain.Interfaces.Repositories;
using WarehouseWebsite.Domain.Models;
using WarehouseWebsite.Infrastructure.Data;
using WarehouseWebsite.Infrastructure.Models;
using WarehouseWebsite.Web.Controllers;
using WarehouseWebsite.Web.Identity;
using WarehouseWebsite.Web.Models;
using static WarehouseWebsite.Tests.UnitTestHelper;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WarehouseWebsite.Tests.WebTests
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Mock<UserManager<AppUser>> _mockUserManager;
        private Mock<SignInManager<AppUser>> _mockSignInManager;
        private Mock<IOptions<JwtSettings>> _mockJwtSettings;
        private AccountController _controller;
        private JwtSettings _jwtSettings;
        private DataContext _dataContext;
        private IRefreshTokenRepository _refreshTokenRepository;
        private JwtTokenService _jwtTokenService;
        private IMemoryCache _cache;
        private List<Claim> _claims;
        private List<string> _roles;

        [SetUp]
        public void SetUp()
        {
            _mockUserManager = new Mock<UserManager<AppUser>>(
                new Mock<IUserStore<AppUser>>().Object, null, null, null, null, null, null, null, null);

            _mockSignInManager = new Mock<SignInManager<AppUser>>(
                _mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object, null, null, null, null);

            _jwtSettings = new JwtSettings
            {
                Key = "cYJjDqsfYRLrtwlkAgWNrmDjZZcPDTIs",
                AccessTokenExpirationMinutes = 30,
                RefreshTokenExpirationDays = 30
            };
            _mockJwtSettings = new Mock<IOptions<JwtSettings>>();
            _mockJwtSettings.Setup(o => o.Value).Returns(_jwtSettings);

            _dataContext = new DataContext(GetUnitTestDbOptions());
            _refreshTokenRepository = new RefreshTokenRepository(_dataContext);
            _cache = new MemoryCache(new MemoryCacheOptions());
            _jwtTokenService = new JwtTokenService(_mockJwtSettings.Object, _refreshTokenRepository, _cache);

            _controller = new AccountController(
                _mockUserManager.Object,
                _mockSignInManager.Object,
                _jwtTokenService,
                _mockJwtSettings.Object
            );

            _claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "username"),
                new Claim(ClaimTypes.NameIdentifier, "userId")
            };

            _roles = new List<string> { "User" };
        }

        [TearDown]
        public void TearDown()
        {
            _dataContext.Dispose();
            _cache.Dispose();
        }

        #region Login

        [Test]
        public async Task AccountControllerLoginReturnUnauthorizedWhenUserNotFound()
        {
            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password"
            };
            _mockUserManager.Setup(u => u.FindByEmailAsync(request.Email))
                .ReturnsAsync((AppUser)null!);

            var result = await _controller.Login(request);
            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        public async Task AccountControllerLoginReturnUnauthorizedWhenEmailOrPasswordAreNotValid()
        {
            var user = new AppUser { Email = "test@example.com" };
            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password"
            };
            _mockUserManager.Setup(u => u.FindByEmailAsync(request.Email)).ReturnsAsync(user);
            _mockSignInManager.Setup(s => s.CheckPasswordSignInAsync(user, request.Password, false))
                .ReturnsAsync(SignInResult.Failed);

            var result = await _controller.Login(request);
            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        public async Task AccountControllerLoginReturnOkWithValidTokenWhenLoginIsSuccessful()
        {
            var user = new AppUser { Email = "test@example.com" };
            var request = new LoginRequest { Email = "test@example.com", Password = "password" };

            _mockUserManager.Setup(u => u.FindByEmailAsync(request.Email)).ReturnsAsync(user);
            _mockUserManager.Setup(u => u.GetClaimsAsync(user)).ReturnsAsync(_claims);
            _mockUserManager.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(_roles);
            _mockUserManager.Setup(u => u.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            _mockSignInManager.Setup(s => s.CheckPasswordSignInAsync(user, request.Password, false))
                .ReturnsAsync(SignInResult.Success);

            var controllerResult = await _controller.Login(request);

            Assert.That(controllerResult, Is.TypeOf<OkObjectResult>());

            string resultJson = JsonConvert.SerializeObject((controllerResult as OkObjectResult)!.Value!);
            var result = JsonConvert.DeserializeAnonymousType(resultJson, new { AccessToken = "", RefreshToken = "" })!;

            Assert.That(result.AccessToken, Is.Not.Empty);
            Assert.That(result.RefreshToken, Is.Not.Empty);

            ClaimsPrincipal principal = _jwtTokenService.GetPrincipalFromExpiredToken(result.AccessToken)!;
            Assert.That(principal.Claims.First(c => c.Type == ClaimTypes.Email).Value, Is.EqualTo(user.Email));
            Assert.That(principal.Claims.First(c => c.Type == ClaimTypes.Role).Value, Is.EqualTo("User"));
        }

        #endregion

        #region Register

        [Test]
        public async Task AccountControllerRegisterReturnsOkWhenRegistrationSuccessful()
        {
            var request = new RegisterRequest { Email = "test@example.com", Password = "password", Name = "John", Surname = "Doe" };
            var user = new AppUser { Email = request.Email };

            _mockUserManager.Setup(u => u.CreateAsync(It.IsAny<AppUser>(), request.Password)).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(u => u.AddToRoleAsync(It.IsAny<AppUser>(), nameof(Roles.User))).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(u => u.AddClaimAsync(It.IsAny<AppUser>(), It.IsAny<Claim>())).ReturnsAsync(IdentityResult.Success);

            var result = await _controller.Register(request);
            Assert.That(result, Is.TypeOf<OkResult>());
        }

        #endregion

        #region Refresh

        [Test]
        public async Task AccountControllerRefreshReturnBadRequestWhenInvalidAccessToken()
        {
            var request = new TokenRequest { AccessToken = "invalid-token", RefreshToken = "refresh-token" };
            var result = await _controller.Refresh(request);
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AccountControllerRefreshReturnUnauthorizedWhenInvalidAccessToken()
        {
            var accessToken = _jwtTokenService.GenerateAccessToken(_claims);
            var request = new TokenRequest { AccessToken = accessToken, RefreshToken = "refresh-token" };

            var result = await _controller.Refresh(request);
            Assert.That(result, Is.TypeOf<UnauthorizedObjectResult>());
        }

        [Test]
        public async Task AccountControllerRefreshReturnOkWhenTokensAreValid()
        {
            var accessTokenStr = _jwtTokenService.GenerateAccessToken(_claims);
            var refreshTokenStr = _jwtTokenService.GenerateRefreshToken();
            var refreshToken = new RefreshToken
            {
                Token = refreshTokenStr,
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddYears(1)
            };
            var request = new TokenRequest { AccessToken = accessTokenStr, RefreshToken = refreshTokenStr };
            await _dataContext.RefreshTokens.AddAsync(refreshToken);
            await _dataContext.SaveChangesAsync();

            var controllerResult = await _controller.Refresh(request);
            Assert.That(controllerResult, Is.TypeOf<OkObjectResult>());

            string resultJson = JsonConvert.SerializeObject((controllerResult as OkObjectResult)!.Value!);
            var result = JsonConvert.DeserializeAnonymousType(resultJson, new { AccessToken = "", RefreshToken = "" })!;

            Assert.That(result.AccessToken, Is.Not.Empty);
            Assert.That(result.RefreshToken, Is.Not.Empty);

            ClaimsPrincipal principal = _jwtTokenService.GetPrincipalFromExpiredToken(result.AccessToken)!;
            Assert.That(principal.Claims.First(c => c.Type == ClaimTypes.Name).Value, Is.EqualTo("username"));
            Assert.That(principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value, Is.EqualTo("userId"));
        }

        #endregion

        #region Logout

        [Test]
        public async Task AccountControllerLogoutReturnsOkAndRevokesTokens()
        {
            string accessToken = _jwtTokenService.GenerateAccessToken(_claims);
            var refreshToken = new RefreshToken
            {
                Token = "RefreshToken",
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddYears(1)
            };
            await _dataContext.RefreshTokens.AddAsync(refreshToken);
            await _dataContext.SaveChangesAsync();

            var user = new AppUser() { Id = "userId", RefreshTokenId = refreshToken.Id };

            var headers = new HeaderDictionary { { "Authorization", $"Bearer {accessToken}" } };
            var contextMock = new Mock<HttpContext>();

            contextMock.Setup(c => c.User).Returns(_jwtTokenService.GetPrincipalFromExpiredToken(accessToken)!);
            contextMock.Setup(c => c.Request.Headers).Returns(headers);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = contextMock.Object
            };
            _mockUserManager.Setup(u => u.FindByIdAsync(user.Id)).ReturnsAsync(user);

            var result = await _controller.Logout();

            Assert.That(result, Is.TypeOf<OkResult>());

            var storedToken = (await _dataContext.RefreshTokens.FindAsync(refreshToken.Id))!;
            Assert.That(storedToken.IsRevoked);
            Assert.That(_cache.TryGetValue(accessToken, out _));
        }

        #endregion
    }
}
