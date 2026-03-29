using MedicalAPI.BusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        //private readonly IUserRole userRole;

        //public UserRoleController(IUserRole _userRole)
        //{
        //    userRole = _userRole;
        //}
        //[HttpGet]
        //public async Task<IActionResult> GetUserRole()
        //{
        //    var GetUserRoles = await userRole.GetRolesDetails();
        //    return Ok(GetUserRoles);
        //}

    }
}
