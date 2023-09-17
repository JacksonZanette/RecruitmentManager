namespace RecruitmentManager.Domain.Dtos
{
    public record CandidateSaveDto
    {
        public string? Name { get; set; }
        public int? Score { get; set; }
    }
}