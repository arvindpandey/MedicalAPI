using MedicalAPI.Interface;
using MedicalAPI.MedicalEntity;
using MedicalAPI.Model;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MedicalAPI.BusinessLogic
{
    public class SymptomAIBL : ISymptomAIService
    {
        private readonly MedicalDbContext _context;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SymptomAIBL> _logger;

        public SymptomAIBL(MedicalDbContext context, IConfiguration config, IHttpClientFactory httpClientFactory, ILogger<SymptomAIBL> logger)
        {   
            _context = context;
            _config = config;
            _httpClientFactory = httpClientFactory;
            _logger = logger;

        }
        public async Task<SymptomAIResponse> AnalyzeSymptomsAsync(SymptomAIRequest request)
        {
            try
            {
                 

                var symptomKey = string.Join("|", request.Symptoms.Select(x => x.Trim().ToLower()).OrderBy(x => x));
                _logger.LogInformation("Symptom looking for key: {key}", symptomKey);

               

                var cached = _context.TblAiSymptomCaches.FirstOrDefault(c => c.SymptomKey == symptomKey);

                if (cached != null)
                {
                    
                    cached.HitCount = cached.HitCount + 1;
                    _logger.LogInformation("Cache HIT for symptoms: {Key}", symptomKey);

                    return new SymptomAIResponse
                    {
                        Success = true,
                        AIAdvice = cached.Aiadvice,
                        Severity = cached.Severity ?? "Unknown",
                        SuggestedAction = cached.SuggestedAction ?? "Consult a doctor",
                        ServedFromCache = true
                    };

                }
                 
                _logger.LogInformation("Cache MISS — calling Groq AI for: {Key}", symptomKey);
                var aiResult = await CallGroqAIAsync(request.Symptoms, request.PatientAge);

                if (!aiResult.Success)
                    return aiResult;

                 
                var newCache = new TblAiSymptomCache
                {
                    SymptomKey = symptomKey,
                    Aiadvice = aiResult.AIAdvice,
                    Severity = aiResult.Severity,
                    SuggestedAction = aiResult.SuggestedAction,
                    CreatedDate = DateTime.UtcNow,
                    HitCount = 1
                };

                _context.TblAiSymptomCaches.Add(newCache);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Saved new AI advice to DB cache for: {Key}", symptomKey);

                aiResult.ServedFromCache = false;
                return aiResult;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AnalyzeSymptomsAsync");
                return new SymptomAIResponse
                {
                    Success = false,
                    Error = "An error occurred analyzing symptoms. Please try again."
                };
            }
        }

        private async Task<SymptomAIResponse> CallGroqAIAsync(List<string> symptoms, int? patientAge)
        {
            var symptomList = string.Join(", ", symptoms);
            var ageContext = patientAge.HasValue ? $"Patient age: {patientAge} years." : "";

            
            var systemPrompt = """
            You are a medical assistant AI. Analyze patient symptoms and respond 
            ONLY in this exact JSON format, no extra text:
            {
              "advice": "Detailed medical advice here",
              "severity": "Mild|Moderate|Severe",
              "suggestedAction": "Rest and fluids|Visit clinic|Go to emergency room"
            }
            Be clear, concise, and always recommend consulting a real doctor for diagnosis.
            """;

            var userPrompt = $"{ageContext} Patient symptoms: {symptomList}. Analyze and advise.";

            var requestBody = new
            {
                model = _config["GroqAI:Model"] ?? "llama-3.3-70b-versatile",
                messages = new[]
                {
                new { role = "system", content = systemPrompt },
                new { role = "user",   content = userPrompt   }
            },
                temperature = 0.3 
            };

            var client = _httpClientFactory.CreateClient("GroqClient");
            var response = await client.PostAsJsonAsync(
                "openai/v1/chat/completions", requestBody);

            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                _logger.LogError("Groq API error: {Error}", err);
                return new SymptomAIResponse
                {
                    Success = false,
                    Error = "AI service unavailable. Please try again."
                };
            }

            var groqResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
            var rawText = groqResponse
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "";

            return ParseAIResponse(rawText);
        }
        private SymptomAIResponse ParseAIResponse(string rawText)
        {
            try
            {
                var clean = Regex.Replace(rawText, @"```[a-z]*\n?|\n?```", "").Trim();
                var parsed = JsonDocument.Parse(clean).RootElement;

                return new SymptomAIResponse
                {
                    Success = true,
                    AIAdvice = parsed.GetProperty("advice").GetString() ?? rawText,
                    Severity = parsed.GetProperty("severity").GetString() ?? "Moderate",
                    SuggestedAction = parsed.GetProperty("suggestedAction").GetString() ?? "Consult a doctor",
                    ServedFromCache = false
                };
            }
            catch
            {
               
                _logger.LogWarning("Could not parse structured AI response, using raw text");
                return new SymptomAIResponse
                {
                    Success = true,
                    AIAdvice = rawText,
                    Severity = "Unknown",
                    SuggestedAction = "Please consult a doctor",
                    ServedFromCache = false
                };
            }
        }

    }
}
