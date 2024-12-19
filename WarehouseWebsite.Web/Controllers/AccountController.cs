using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using WarehouseWebsite.Domain.Models;
using WarehouseWebsite.Domain.Models.Customers;
using WarehouseWebsite.Infrastructure.Models;
using WarehouseWebsite.Web.Identity;
using WarehouseWebsite.Web.Models;

namespace WarehouseWebsite.Web.Controllers
{
    [Route("api/account")]
    public class AccountController : WarehouseControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JwtTokenService _jwtTokenService;
        private readonly JwtSettings _jwtSettings;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            JwtTokenService jwtTokenService,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null) return Unauthorized(new { Message = "Invalid email" });

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            {
                var claims = (await _userManager.GetClaimsAsync(user)).ToList();
                var roles = await _userManager.GetRolesAsync(user);

                var additionalClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

                claims.AddRange(additionalClaims);
                claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

                var accessToken = _jwtTokenService.GenerateAccessToken(claims);
                var refreshToken = _jwtTokenService.GenerateRefreshToken();

                RefreshToken token = new();
                if (user.RefreshTokenId != null) // update token in db if user already has it
                {
                    token = (await _jwtTokenService.GetRefreshTokenByIdAsync((Guid)user.RefreshTokenId))!;                     
                }                
                token.Token = refreshToken;
                token.Created = DateTime.UtcNow;
                token.Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
                token.IsRevoked = false;

                user.RefreshToken = token;
                var updateResult = await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded) return BadRequest();

                return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken, Roles = roles });
            }
            return Unauthorized(new { Message = "Invalid password" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var findResult = await _userManager.FindByEmailAsync(request.Email);
            if (findResult != null) return BadRequest(new { Message = "User with specified email has already been registered" });

            var user = new AppUser
            {
                Email = request.Email,
                UserName = request.Email,
                Customer = new Customer()
                {
                    Name = request.Name,
                    Surname = request.Surname,
                    Address = "",
                    Email = request.Email
                }
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, nameof(Roles.User));

                if (!roleResult.Succeeded) return BadRequest();

                var customerIdClaim = new Claim("CustomerId", user.CustomerId.ToString());
                var claimResult = await _userManager.AddClaimAsync(user, customerIdClaim);

                if (!claimResult.Succeeded) return BadRequest();

                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequest request)
        {
            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(request.AccessToken);

            if (principal == null) return BadRequest(new { Message = "Invalid access token" });

            var storedToken = await _jwtTokenService.GetStoredRefreshTokenAsync(request.RefreshToken);

            if (storedToken == null || !storedToken.IsActive) return Unauthorized(new { Message = "Refresh token is invalid or expired" });

            var newAccessToken = _jwtTokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            storedToken.Token = newRefreshToken;
            storedToken.Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
            await _jwtTokenService.UpdateRefreshTokenAsync(storedToken);

            return Ok(new TokenRequest
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            string header = HttpContext.Request.Headers["Authorization"]!;
            string accessToken = header.Substring("Bearer ".Length).Trim();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return BadRequest(new { Message = "User was not found" });

            await _jwtTokenService.RevokeTokensAsync(accessToken, (Guid)user.RefreshTokenId!);
            return Ok();
        }
    }
}
