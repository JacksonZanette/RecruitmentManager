﻿namespace RecruitmentManager.Domain.Models
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }
    }
}