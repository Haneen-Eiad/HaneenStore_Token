using HaneenStore2.DAL.DTOs.Response;
using HaneenStore2.DAL.DTOs.Request;
using HaneenStore2.DAL.Models;
using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaneenStore2.BLL.Service
{
    public interface IAuthenticationService
    {
     
        Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
        Task <LoginResponse> LoginAsync(LoginRequest loginRequest);
        Task<bool> ConfirmEmailAsync(string token, string userId);
        Task<ForgetPasswordResponse> RequestPasswordReset(ForgetPasswordRequest request);
        Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request);
    }
}
