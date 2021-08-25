using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SpatulaApi.Data;
using SpatulaApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace SpatulaApi.Services
{
	public class AuthManager : IAuthManager
	{

		private readonly UserManager<ApiUser> _userManager;
		private readonly IConfiguration _configuration;
		private ApiUser _user;
		private readonly ILogger _logger;
		private readonly DatabaseContext _context;
		public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration, ILogger<AuthManager> logger,
			DatabaseContext context)
		{
			_userManager = userManager;
			_configuration = configuration;
			_logger = logger;
			_context = context;
		}

		public async Task<string> CreateToken()
		{
			var signingCredentials = GetSigningCredentials();
			var claims = await GetClaims();
			var token = GenerateTokenOptions(signingCredentials, claims);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
		{
			var jwtSettings = _configuration.GetSection("Jwt");

			var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("Lifetime").Value));
			var token = new JwtSecurityToken
			(
				issuer: jwtSettings.GetSection("issuer").Value,
				claims: claims,
				expires: expiration,
				signingCredentials: signingCredentials
			);
			return token;
		}

		private async Task<List<Claim>> GetClaims()
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, _user.UserName),
				new Claim(ClaimTypes.PrimarySid, _user.Id)
			};
			var roles = await _userManager.GetRolesAsync(_user);
			foreach(var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}
			return claims;
		}

		private SigningCredentials GetSigningCredentials()
		{
			var key = Environment.GetEnvironmentVariable("HOTELJWTKEY");
			var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
			return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256); 
		}

		public async Task<bool> ValidateUser(UserLoginDTO userDTO)
		{
			_user = await _userManager.FindByNameAsync(userDTO.Email);
			return (_user != null && await _userManager.CheckPasswordAsync(_user, userDTO.Password));
		}

		public async Task<bool> ValidateFacebookUser(UserLoginDTO userDTO)
		{
			_user = await _userManager.FindByEmailAsync(userDTO.Email);
			return (_user != null);
		}




	}
}
