using Microsoft.AspNetCore.Mvc;
using SharedSource.Contract;
using TranslationDemo.Api.DTO;

namespace TranslationDemo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleTranslateController : ControllerBase
    {
        private readonly ITranslateService _translateService;

        public GoogleTranslateController(ITranslateService translateService)
        {
            _translateService = translateService;
        }
        [HttpPost]
        public async Task<IActionResult> Post(TranslateRequestDto requestDto)
        {
            try
            {
                var result = await _translateService.TranslateAsync(requestDto.MapToProtoRequest());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("Information")]
        public async Task<IActionResult> Get()
        {
            var stat = await _translateService.Information();
            return Ok(stat);
        }
    }
}
