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
        public async Task<IActionResult> Create(CandidateSaveDto candidateSaveDto, CancellationToken cancellationToken)
        {
            await _candidatesService.CreateAsync(candidateSaveDto, cancellationToken);
            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<CandidateDto>> Get(CancellationToken cancellationToken)
            => await _candidatesService.GetAsync(cancellationToken);

        [HttpGet("{id}")]
        public async Task<CandidateDto> GetById(Guid id, CancellationToken cancellationToken)
            => await _candidatesService.GetByIdAsync(id, cancellationToken);
    }
}