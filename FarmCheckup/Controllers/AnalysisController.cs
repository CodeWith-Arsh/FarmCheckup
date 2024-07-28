using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FarmCheckup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AnalysisController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> Analyze([FromForm] IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                return BadRequest("Photo is required.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await photo.CopyToAsync(memoryStream);
                var photoBytes = memoryStream.ToArray();

                // Analyze the photo (using a placeholder function here)
                var diagnosis = await AnalyzePhotoAsync(photoBytes);

                // Get solution from OpenAI
                var solution = await GetSolutionFromOpenAIAsync(diagnosis);

                return Ok(new { diagnosis, solution });
            }
        }

        private async Task<string> AnalyzePhotoAsync(byte[] photoBytes)
        {
            // Call an external API or use a local ML model to analyze the photo
            // Placeholder for the actual implementation
            return "Sample diagnosis";
        }

        private async Task<string> GetSolutionFromOpenAIAsync(string diagnosis)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "YOUR_OPENAI_API_KEY");

            var openAiRequest = new
            {
                prompt = $"Leaf diagnosis: {diagnosis}. What should be done?",
                max_tokens = 100
            };

            var response = await httpClient.PostAsJsonAsync("https://api.openai.com/v1/engines/davinci/completions", openAiRequest);
            var responseString = await response.Content.ReadAsStringAsync();

            var responseObject = JObject.Parse(responseString);
            return responseObject["choices"][0]["text"].ToString();
        }
    }
}
