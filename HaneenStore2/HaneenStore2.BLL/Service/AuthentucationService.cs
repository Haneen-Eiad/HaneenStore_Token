using HaneenStore2.DAL.DTOs.Request;
using HaneenStore2.DAL.DTOs.Response;
using HaneenStore2.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HaneenStore2.BLL.Service
{
    public class AuthentucationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthentucationService(UserManager<ApplicationUser> userManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                var User = await _userManager.FindByEmailAsync(loginRequest.Email);
                if (User is null) {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Invalid Email",
                        

                    };
                }

                var result = await _userManager.CheckPasswordAsync(User, loginRequest.Password);
                if (!result)
                {
                    return new LoginResponse
                    {
                        Success = true,
                        Message = "Invalid Password",


                    };

                }
                return new LoginResponse()
                {
                    Success = true,
                    Message ="Login successfully",
                    AccessToken = await GenereteAccessToken(User)
                };

            }
            catch (Exception ex) {
                return new LoginResponse
                {
                    Success = false,
                    Message = "An Unexpected error",
                    Errors = new List<string> { ex.Message }

                };

            }
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            try
            {

                var user = registerRequest.Adapt<ApplicationUser>();
                var result = await _userManager.CreateAsync(user, registerRequest.Password);
                if (!result.Succeeded)
                {
                    return new RegisterResponse
                    {
                        Success = false,
                        Message = "User creation Failed",
                        Errors = result.Errors.Select(e => e.Description).ToList()

                    };


                }
                await _userManager.AddToRoleAsync(user, "User");
                return new RegisterResponse
                {
                    Success = true,
                    Message = "success"
                };
            }
            catch (Exception ex) {
                return new RegisterResponse
                {
                    Success = false,
                    Message = "An Unexpected error",
                    Errors = new List<string> { ex.Message }

                };
            }
        }

        private async Task<string> GenereteAccessToken (ApplicationUser user)
        {
            var userClime = new List<Claim>()
            {
                new Claim (ClaimTypes.NameIdentifier,user.Id),
                 new Claim (ClaimTypes.Name,user.UserName),
                  new Claim (ClaimTypes.Email,user.Email),

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClime,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    
}
