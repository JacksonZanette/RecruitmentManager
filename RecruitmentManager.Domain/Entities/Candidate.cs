namespace RecruitmentManager.Domain.Entities
{
    public class Candidate
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public int? Score { get; set; }
    }
}