using RecruitmentManager.Domain.Entities;
using RecruitmentManager.Domain.Interfaces.Repositories;
using RecruitmentManager.Domain.Interfaces.Services;
using RecruitmentManager.Domain.Models;

namespace RecruitmentManager.Domain.Services
{
    public class CandidatesService : ICandidatesService
    {
        private readonly ICandidatesRepository _candidatesRepository;

        public CandidatesService(ICandidatesRepository candidatesRepository)
        {
            _candidatesRepository = candidatesRepository;
        }

        public async Task Create(Candidate candidate, CancellationToken cancellationToken)
        {
            ValidateCandidate(candidate);

            await _candidatesRepository.Create(candidate, cancellationToken);
        }

        public async Task<Candidate> GetById(Guid id, CancellationToken cancellationToken)
            => await _candidatesRepository.GetById(id, cancellationToken);

        public async Task Update(Candidate candidate, CancellationToken cancellationToken)
        {
            ValidateCandidate(candidate);

            var existingCandidate = await _candidatesRepository.GetById(candidate.Id, cancellationToken)
                ?? throw new DomainNotFoundException("The candidate was not found for provided id");

            existingCandidate.Score = candidate.Score;
            await _candidatesRepository.Update(candidate, cancellationToken);
        }

        private static void ValidateCandidate(Candidate candidate)
        {
            if (candidate.Score is < 0 or > 100)
                throw new DomainValidationException("The provided candidate score is invalid");

            if (string.IsNullOrEmpty(candidate.Name))
                throw new DomainValidationException("The provided candidate name is invalid");
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
            => await _candidatesRepository.Delete(id, cancellationToken);
    }
}