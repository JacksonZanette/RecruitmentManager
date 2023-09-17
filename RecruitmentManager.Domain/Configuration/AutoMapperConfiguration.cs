using AutoMapper;
using RecruitmentManager.Domain.Dtos;
using RecruitmentManager.Domain.Entities;

namespace RecruitmentManager.Domain.Configuration
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Candidate, CandidateDto>();
            CreateMap<CandidateSaveDto, Candidate>();
        }
    }
}