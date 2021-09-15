using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Common;
using Application.Common.Interfaces;
using Application.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Data;

namespace Api.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly JwtSettings _jwtSettings;
        
        public IdentityService(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _roleManager = roleManager;
        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string username, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthenticationResult()
                {
                    ErrorMessages = new[] {"User with this email address already exists."}
                };
            }

            var newUser = new ApplicationUser()
            {
                Email = email,
                UserName = username
            };
            
            var createdUser = await _userManager.CreateAsync(newUser, password);
            
            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult()
                {
                    ErrorMessages = createdUser.Errors.Select(x => x.Description)
                };
            }

            await _userManager.AddToRoleAsync(newUser, Roles.User);
            
            return await GenerateAuthenticationResult(newUser);
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthenticationResult()
                {
                    ErrorMessages = new[] {"User does not exist."}
                };
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
            {
                return new AuthenticationResult()
                {
                    ErrorMessages = new[] {"Email/password combination is wrong"}
                };
            }

            return await GenerateAuthenticationResult(user);
        }
        
        private async Task<AuthenticationResult> GenerateAuthenticationResult(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id.ToString())
            };
            
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role == null)
                {
                    continue;
                }

                var roleClaims = await _roleManager.GetClaimsAsync(role);
                foreach (var roleClaim in roleClaims)
                {
                    if (!claims.Contains(roleClaim))
                    {
                        claims.Add(roleClaim);
                    }
                }
            }
            
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult()
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }

       
    }
}