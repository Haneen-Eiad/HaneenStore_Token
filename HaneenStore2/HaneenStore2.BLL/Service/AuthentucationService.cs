using HaneenStore2.DAL.DTOs.Request;
using HaneenStore2.DAL.DTOs.Response;
using HaneenStore2.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
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
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthentucationService(UserManager<ApplicationUser> userManager,IConfiguration configuration,
            IEmailSender emailSender,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _signInManager = signInManager;
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
                if( await _userManager.IsLockedOutAsync(User))
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Account is locked, try again Later"
                    };   
                }
                var result = await _signInManager.CheckPasswordSignInAsync(User, loginRequest.Password,true);
                if (result.IsLockedOut)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = " Account locked due to multipule failed attepmes"
                    };

                }
                else if (result.IsNotAllowed)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "please confirm your email"
                    };
                }
                if (!result.Succeeded) {

                    return new LoginResponse()
                    {
                        Success = false,
                        Message = " Invalied password ☺"
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
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = Uri.EscapeDataString(token);
                var emailUser = $"https://localhost:7091/api/auth/Account/ConfirmEmail?token={token}&userId={user.Id}";
                await _emailSender.SendEmailAsync(user.Email, "welcome", $"<h1>welcome..{user.UserName}</h1> "
                    +$" <a href='{emailUser}'>confirm email</a>");

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
        public async Task<bool> ConfirmEmailAsync(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return false;
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if(!result.Succeeded)
            { return false; } 
            return true;

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

        public async Task<ForgetPasswordResponse> RequestPasswordReset(ForgetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                return new ForgetPasswordResponse()
                {
                    Success = false,
                    Message = " Email not found"
                };
            }
            var Random = new Random();
            var code = Random.Next(1000,9999).ToString();
            user.CodeResetPassword=code;
            user.CodeResetPasswordExpired = DateTime.UtcNow.AddMinutes(15);
            await _userManager.UpdateAsync(user);
            await _emailSender.SendEmailAsync(request.Email, "reset Password", $"<p>code is {code}</p>");
            return new ForgetPasswordResponse()
            {
                Success = true,
                Message = " code sent to your email ☺"
            };
        }


        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = " Email not found"
                };
            }

            else if (user.CodeResetPassword != request.Code)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = " Invalied Code"
                };
            }

            else if (user.CodeResetPasswordExpired < DateTime.UtcNow )
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = " Code Expired"

                };
            }

            var token =  await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token,request.NewPassword);

            if(!result.Succeeded)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = " Password Reset Failed",
                    Errors = result.Errors.Select(e => e.Description).ToList()


                };
            }
               
           
            await _userManager.UpdateAsync(user);
            await _emailSender.SendEmailAsync(request.Email, "Chaned Password", $"<p>your password is changed</p>");
            return new ResetPasswordResponse()
            {
                Success = true,
                Message = " password reset successfully ..♥"
            };
        }
    }

    
}
