using MedicalAPI.BusinessLogic;
using MedicalAPI.Interface;
using MedicalAPI.MedicalEntity;
using MedicalAPI.Miscellaneous;
using MedicalAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MedicalAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService) 
        { 
            _patientService = patientService; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _patientService.GetAllPatientsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePatientDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState); 
            var userId = 4;//int.Parse(TblUser.FindFirst(ClaimTypes.NameIdentifier).Value);
            var id = await _patientService.CreatePatientAsync(dto, userId );
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreatePatientDTO dto)
        {
            await _patientService.UpdatePatientAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Delete(int id)
        {
            await _patientService.DeletePatientAsync(id);
            return NoContent();
        }
    }
}
