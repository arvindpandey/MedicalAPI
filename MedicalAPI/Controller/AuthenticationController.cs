using MedicalAPI.BusinessLogic;
using MedicalAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationBL authenticationBL; 

        public AuthenticationController(IAuthenticationBL _authenticationBL)
        {
            authenticationBL = _authenticationBL;
        }
        [HttpPost]
        public async Task<IActionResult> Login(AuthenticationRequestModel authenticationRequestModel)
        {
            AuthenticationResponseModel objeAuthResponse = new AuthenticationResponseModel();
            objeAuthResponse = await authenticationBL.Login(authenticationRequestModel);

            return Ok (objeAuthResponse);
        }
    }
}
