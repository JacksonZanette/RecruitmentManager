﻿namespace RecruitmentManager.Domain.Dtos
{
    public record CandidateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? Score { get; set; }
    }
}