namespace RecruitmentManager.Domain.Entities
{
    public class Candidate
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? Score { get; set; }
    }
}