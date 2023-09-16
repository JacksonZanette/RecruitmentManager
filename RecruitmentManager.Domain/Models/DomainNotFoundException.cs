namespace RecruitmentManager.Domain.Models
{
    public class DomainNotFoundException : Exception
    {
        public DomainNotFoundException(string message) : base(message)
        {
        }
    }
}