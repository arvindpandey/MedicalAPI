using MedicalAPI.Interface;
using MedicalAPI.MedicalEntity;
using MedicalAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL _userBL;
        //This is for User Controller
        public UserController(IUserBL userBL)
        {
            _userBL = userBL;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userBL.GetAllUsers();
            return Ok(users);

        }
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userBL.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult AddUser(TblUser userModel)
        {
            _userBL.AddUser(userModel);
            return CreatedAtAction(nameof(GetUserById), new { id = userModel.UserId }, userModel);
        }


    }
}
