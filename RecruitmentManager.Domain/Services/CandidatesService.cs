using AutoMapper;
using RecruitmentManager.Domain.Dtos;
using RecruitmentManager.Domain.Entities;
using RecruitmentManager.Domain.Interfaces.Repositories;
using RecruitmentManager.Domain.Interfaces.Services;
using RecruitmentManager.Domain.Models;

namespace RecruitmentManager.Domain.Services
{
    public class CandidatesService : ICandidatesService
    {
        private readonly ICandidatesRepository _candidatesRepository;
        private readonly IMapper _mapper;

        public CandidatesService(ICandidatesRepository candidatesRepository, IMapper mapper)
        {
            _candidatesRepository = candidatesRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(CandidateSaveDto candidateSaveDto, CancellationToken cancellationToken)
        {
            ValidateCandidate(candidateSaveDto);

            var candidate = _mapper.Map<CandidateSaveDto, Candidate>(candidateSaveDto);

            await _candidatesRepository.CreateAsync(candidate, cancellationToken);
        }

        public async Task<IEnumerable<CandidateDto>> GetAsync(CancellationToken cancellationToken)
            => _mapper.Map<IEnumerable<Candidate>, IEnumerable<CandidateDto>>(await _candidatesRepository.GetAsync(cancellationToken));

        public async Task<CandidateDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => _mapper.Map<Candidate, CandidateDto>(await _candidatesRepository.GetByIdAsync(id, cancellationToken));

        public async Task UpdateAsync(Guid id, CandidateSaveDto candidateSaveDto, CancellationToken cancellationToken)
        {
            ValidateCandidate(candidateSaveDto);

            var existingCandidate = await _candidatesRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new DomainNotFoundException("The candidate was not found for provided id");

            _mapper.Map(candidateSaveDto, existingCandidate);

            await _candidatesRepository.UpdateAsync(existingCandidate, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
            => await _candidatesRepository.DeleteAsync(id, cancellationToken);

        private static void ValidateCandidate(CandidateSaveDto candidateSaveDto)
        {
            if (candidateSaveDto.Score is < 0 or > 100)
                throw new DomainValidationException("The provided candidate score is invalid");

            if (string.IsNullOrEmpty(candidateSaveDto.Name))
                throw new DomainValidationException("The provided candidate name is invalid");
        }
    }
}