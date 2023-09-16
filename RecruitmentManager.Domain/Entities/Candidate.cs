namespace RecruitmentManager.Domain.Entities
{
    public class Candidate
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? Score { get; set; }
    }
}