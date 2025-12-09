using MedicalAPI.BusinessLogic;
using MedicalAPI.MedicalEntity;
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
        private readonly IUserBL userBL;
        GlobalResponse glRespose = new GlobalResponse();
        public UserController(IUserBL _userBL)
        {
            userBL = _userBL;
        }
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            glRespose.ReponseData = await userBL.GetAll();
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
            glRespose.ReponseData = await userBL.GetAllByUserID(ID);
            if (glRespose.ReponseData == null || glRespose.ReponseData == "")
                return NotFound("No users found.");
            else
                glRespose.Response_Message = "Record fetch Successfully by Userid : " + ID;
            return Ok(glRespose);
        }
        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUserRecord(UserModel _uM)
        {
            glRespose.Response_Message = await userBL.Adds(_uM); 

            return Ok(glRespose.Response_Message);
        }
        [HttpPost]
        [Route("UpdateUser")]
        public async Task<IActionResult> UdpateUserRecord(UserModel _uM)
        {
            glRespose.Response_Message = await userBL.Update(_uM);

            return Ok(glRespose.Response_Message);
        } 
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            glRespose.Response_Message = await userBL.DeleteRecord(id);

            return Ok(glRespose.Response_Message);
        }

    }
}
