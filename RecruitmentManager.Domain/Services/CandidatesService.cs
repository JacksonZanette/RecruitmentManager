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

        public async Task CreateAsync(CandidateSaveDto candidateSaveDto)
        {
            ValidateCandidate(candidateSaveDto);

            var candidate = _mapper.Map<CandidateSaveDto, Candidate>(candidateSaveDto);

            await _candidatesRepository.CreateAsync(candidate);
        }

        public async Task<IEnumerable<CandidateDto>> GetAsync()
            => _mapper.Map<IEnumerable<Candidate>, IEnumerable<CandidateDto>>(await _candidatesRepository.GetAsync());

        public async Task<CandidateDto> GetByIdAsync(int id)
            => _mapper.Map<Candidate, CandidateDto>(await _candidatesRepository.GetByIdAsync(id));

        public async Task UpdateAsync(int id, CandidateSaveDto candidateSaveDto)
        {
            ValidateCandidate(candidateSaveDto);

            var existingCandidate = await _candidatesRepository.GetByIdAsync(id)
                ?? throw new DomainNotFoundException("The candidate was not found for provided id");

            _mapper.Map(candidateSaveDto, existingCandidate);

            await _candidatesRepository.UpdateAsync(existingCandidate);
        }

        public async Task DeleteAsync(int id)
            => await _candidatesRepository.DeleteAsync(id);

        private static void ValidateCandidate(CandidateSaveDto candidateSaveDto)
        {
            if (candidateSaveDto.Score is < 0 or > 100)
                throw new DomainValidationException("The provided candidate score is invalid");

            if (string.IsNullOrEmpty(candidateSaveDto.Name))
                throw new DomainValidationException("The provided candidate name is invalid");
        }
    }
}