namespace RecruitmentManager.Domain.Models
{
    public class DomainValidationException : Exception
    {
        public DomainValidationException(string message) : base(message)
        {
        }
    }
}