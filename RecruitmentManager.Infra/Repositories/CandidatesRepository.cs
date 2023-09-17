using RecruitmentManager.Domain.Entities;
using RecruitmentManager.Domain.Interfaces.Repositories;

namespace RecruitmentManager.Infra.Repositories
{
    public class CandidatesRepository : ICandidatesRepository
    {
        public async Task CreateAsync(Candidate candidate, CancellationToken cancellationToken)
            => await Task.CompletedTask;

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
            => await Task.CompletedTask;

        public async Task<IEnumerable<Candidate>> GetAsync(CancellationToken cancellationToken)
            => await Task.FromResult(new Candidate[]
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Jackson",
                    Score = 100
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "José",
                    Score = 50
                }
            });

        public async Task<Candidate> GetByIdAsync(Guid id, CancellationToken none)
            => await Task.FromResult(new Candidate
            {
                Id = Guid.NewGuid(),
                Name = "Jackson",
                Score = 100
            });

        public async Task UpdateAsync(Candidate candidate, CancellationToken cancellationToken)
            => await Task.CompletedTask;
    }
}