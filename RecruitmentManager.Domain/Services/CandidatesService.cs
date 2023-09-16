using RecruitmentManager.Domain.Entities;
using RecruitmentManager.Domain.Interfaces.Repositories;
using RecruitmentManager.Domain.Models;

namespace RecruitmentManager.Domain.Services
{
    public class CandidatesService
    {
        private readonly ICandidatesRepository _candidatesRepository;

        public CandidatesService(ICandidatesRepository candidatesRepository)
        {
            _candidatesRepository = candidatesRepository;
        }

        public async Task Save(Candidate candidate, CancellationToken cancellationToken)
        {
            if (candidate.Score is < 0 or > 100)
                throw new DomainException("The provided candidate score is invalid");

            await _candidatesRepository.Save(candidate, cancellationToken);
        }
    }
}