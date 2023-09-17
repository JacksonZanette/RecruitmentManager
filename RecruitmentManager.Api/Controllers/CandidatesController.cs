using Microsoft.AspNetCore.Mvc;
using RecruitmentManager.Domain.Dtos;
using RecruitmentManager.Domain.Interfaces.Services;

namespace RecruitmentManager.Api.Controllers
{
    [ApiController]
    [Route("api/v1/candidates")]
    public class CandidatesController : ControllerBase
    {
        private readonly ICandidatesService _candidatesService;

        public CandidatesController(ICandidatesService candidatesService)
            => _candidatesService = candidatesService;

        [HttpPost]
        public async Task<IActionResult> Create(CandidateSaveDto candidateSaveDto)
        {
            await _candidatesService.CreateAsync(candidateSaveDto);
            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<CandidateDto>> Get()
            => await _candidatesService.GetAsync();

        [HttpGet("{id}")]
        public async Task<CandidateDto> GetById(int id)
            => await _candidatesService.GetByIdAsync(id);

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CandidateSaveDto candidateSaveDto)
        {
            await _candidatesService.UpdateAsync(id, candidateSaveDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _candidatesService.DeleteAsync(id);
            return NoContent();
        }
    }
}