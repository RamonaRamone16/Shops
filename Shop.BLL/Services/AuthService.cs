using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shop.DAL;
using Shop.Models.Dtos;
using Shop.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shop.BLL.Services
{
    public class AuthService : BaseService
    {
        private readonly IConfiguration _configuration;
        private User _user;
        public AuthService(ApplicationDbContext context,
            IMapper mapper, UserManager<User> userManager,
            IConfiguration configuration) : base(context, mapper, userManager)
        {
            _configuration = configuration;
        }

        public async Task<IdentityResult> CreateUserAsync(UserRegistrationDto dto) 
        {
            var entity = _mapper.Map<User>(dto);

            var result = await _userManager.CreateAsync(entity, dto.Password);
            await _userManager.AddToRoleAsync(entity, dto.Role.ToString()); 

            return result;
        }

        public async Task<bool> ValidateUserAsync(UserLoginDto dto) 
        { 
            _user = await _userManager.FindByNameAsync(dto.UserName);
            var result = await _userManager.CheckPasswordAsync(_user, dto.Password);
            return result;
        }

        public async Task<string> CreateTokenAsync()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtConfig = _configuration.GetSection("jwtConfig");
            var key = Encoding.UTF8.GetBytes(jwtConfig["Secret"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtConfig");
            var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expiresIn"])),
                signingCredentials: signingCredentials
            );
            return tokenOptions;
        }

    }
}