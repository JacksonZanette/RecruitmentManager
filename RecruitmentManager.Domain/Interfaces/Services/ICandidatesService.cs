using RecruitmentManager.Domain.Dtos;

namespace RecruitmentManager.Domain.Interfaces.Services
{
    public interface ICandidatesService
    {
        Task CreateAsync(CandidateSaveDto candidateSaveDto);

        Task<IEnumerable<CandidateDto>> GetAsync();

        Task<CandidateDto> GetByIdAsync(int id);

        Task UpdateAsync(int id, CandidateSaveDto candidateSaveDto);

        Task DeleteAsync(int id);
    }
}