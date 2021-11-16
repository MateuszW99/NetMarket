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
using Application.Identity.Responses;
using Application.Models.DTOs;
using Domain;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly JwtSettings _jwtSettings;

        public IdentityService(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _roleManager = roleManager;
        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string username, string password,
            string role = "User")
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

            await _userManager.AddToRoleAsync(newUser, role);

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

        public async Task<ResetPasswordResponse> ResetPassword(string email, string password, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new ResetPasswordResponse()
                {
                    ErrorMessages = new[] {"User does not exist."}
                };
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
            {
                return new ResetPasswordResponse()
                {
                    ErrorMessages = new[] {"Password is wrong"}
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!resetPasswordResult.Succeeded)
            {
                return new ResetPasswordResponse()
                {
                    ErrorMessages = resetPasswordResult.Errors.Select(x => x.Description)
                };
            }

            return new ResetPasswordResponse()
            {
                Success = true
            };
        }

        public async Task<List<SupervisorObject>> GetSupervisorsAsync()
        {
            var supervisorUsers = await _userManager.GetUsersInRoleAsync(Roles.Supervisor);

            return supervisorUsers.Select(applicationUser => new SupervisorObject()
            {
                Id = applicationUser.Id.ToString(), Email = applicationUser.Email, Username = applicationUser.UserName
            }).ToList();
        }

        public async Task<DeleteUserResponse> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            
            if (user == null)
            {
                return new DeleteUserResponse()
                {
                    ErrorMessages = new[] {"User does not exist."}
                };
            }

            var deleteUserResult = await _userManager.DeleteAsync(user);
            
            if (!deleteUserResult.Succeeded)
            {
                return new DeleteUserResponse()
                {
                    ErrorMessages = deleteUserResult.Errors.Select(x => x.Description)
                };
            }

            return new DeleteUserResponse()
            {
                Success = true
            };
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
                new Claim("username", user.UserName),
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
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
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