using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaneenStore2.DAL.DTOs.Response
{
    public class LoginResponse : BaseResponse
    {
        
        public string? AccessToken { get; set; }
    }
}
