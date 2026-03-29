using MedicalAPI.BusinessLogic;
using MedicalAPI.Interface;
using MedicalAPI.Miscellaneous;
using MedicalAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;


namespace MedicalAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userBL;
        GlobalResponse glRespose = new GlobalResponse();
        public UserController(IUserService _userBL)
        {
            userBL = _userBL;
        }
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            glRespose.ReponseData = await userBL.GetAllUsersAsync();
            if (glRespose.ReponseData == null || glRespose.ReponseData == "")
                return NotFound("No users found.");
            else
                glRespose.Response_Message = "Record All fetch Successfully";
            return Ok(glRespose);
        }
        [HttpGet]
        [Route("GetDataByID")]
        public async Task<IActionResult> GetAllByUserID(int ID)
        {
            glRespose.ReponseData = await userBL.GetUserByIdAsync(ID);
            if (glRespose.ReponseData == null || glRespose.ReponseData == "")
                return NotFound("No users found.");
            else
                glRespose.Response_Message = "Record fetch Successfully by Userid : " + ID;
            return Ok(glRespose);
        }
        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUserRecord(CreateUserDTO _uM,int createdByUserId)
        {
            var K = await userBL.CreateUserAsync(_uM, createdByUserId);

            return Ok(glRespose.Response_Message);
        }
        [HttpPost]
        [Route("UpdateUser")]
        public async Task<IActionResult> UdpateUserRecord(int UserID,CreateUserDTO _uM)
        {
            await userBL.UpdateUserAsync(UserID,_uM);

            return Ok(glRespose.Response_Message);
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
             await userBL.DeleteUserAsync(id);

            return Ok(glRespose.Response_Message);
        }

    }
}
