using AutoFixture.Xunit2;
using Bogus;
using Moq;
using Moq.AutoMock;
using RecruitmentManager.Domain.Entities;
using RecruitmentManager.Domain.Interfaces.Repositories;
using RecruitmentManager.Domain.Models;
using RecruitmentManager.Domain.Services;

namespace RecruitmentManager.Domain.Test.Services
{
    public class CandidatesServiceTests
    {
        private readonly AutoMocker _autoMocker;
        private readonly CandidatesService _service;

        public CandidatesServiceTests()
        {
            _autoMocker = new AutoMocker();
            _service = _autoMocker.CreateInstance<CandidatesService>();
        }

        [Trait("CandidatesService", "Save")]
        [Theory(DisplayName = "Save candidate without score"), AutoData]
        public async Task WhenSaveIsCalled_WithCandidateWithoutScore_ShouldPersist(Guid id, string name)
        {
            //Arrange
            var candidate = new Candidate
            {
                Id = id,
                Name = name,
            };

            //Act
            await _service.Save(candidate, CancellationToken.None);

            //Assert
            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.Save(candidate, CancellationToken.None), Times.Once);
        }

        [Trait("CandidatesService", "Save")]
        [Theory(DisplayName = "Save candidate with valid score"), AutoData]
        public async Task WhenSaveIsCalled_WithCandidateWithValidScore_ShouldPersist(Guid id, string name)
        {
            //Arrange
            var candidate = new Candidate
            {
                Id = id,
                Name = name,
                Score = new Faker().Random.Int(0, 100)
            };

            //Act
            await _service.Save(candidate, CancellationToken.None);

            //Assert
            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.Save(candidate, CancellationToken.None), Times.Once);
        }

        [Trait("CandidatesService", "Save")]
        [Theory(DisplayName = "Save candidate with invalid scores"), AutoData]
        [InlineAutoData(-1)]
        [InlineAutoData(-101)]
        public async Task WhenSaveIsCalled_WithCandidateWithInvalidScore_ShouldNotPersist(int? score, Guid id, string name)
        {
            //Arrange
            var candidate = new Candidate
            {
                Id = id,
                Name = name,
                Score = score
            };

            //Assert
            await Assert.ThrowsAsync<DomainException>(async () => await _service.Save(candidate, CancellationToken.None));
        }
    }
}