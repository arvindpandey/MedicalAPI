using MedicalAPI.Interface;
using MedicalAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SymptomAIController : ControllerBase
    {
        private readonly ISymptomAIService _symptomAIService;

        public SymptomAIController(ISymptomAIService symptomAIService)
        {
            _symptomAIService = symptomAIService;
        }
        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeSymptoms([FromBody] SymptomAIRequest request)
        {
            if (request.Symptoms == null || !request.Symptoms.Any())
                return BadRequest("At least one symptom is required.");

            var result = await _symptomAIService.AnalyzeSymptomsAsync(request);

            if (!result.Success)
                return StatusCode(500, result);

            return Ok(result);
        }
    }
}
