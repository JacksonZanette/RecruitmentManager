﻿using RecruitmentManager.Domain.Entities;

namespace RecruitmentManager.Domain.Interfaces.Repositories
{
    public interface ICandidatesRepository
    {
        Task Create(Candidate candidate, CancellationToken cancellationToken);

        Task<Candidate> GetById(Guid id, CancellationToken none);

        Task Update(Candidate candidate, CancellationToken cancellationToken);

        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}