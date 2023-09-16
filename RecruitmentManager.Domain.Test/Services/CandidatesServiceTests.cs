using AutoFixture.Xunit2;
using Bogus;
using Moq;
using Moq.AutoMock;
using RecruitmentManager.Domain.Entities;
using RecruitmentManager.Domain.Interfaces.Repositories;
using RecruitmentManager.Domain.Interfaces.Services;
using RecruitmentManager.Domain.Models;
using RecruitmentManager.Domain.Services;

namespace RecruitmentManager.Domain.Test.Services
{
    public class CandidatesServiceTests
    {
        private readonly AutoMocker _autoMocker;
        private readonly ICandidatesService _service;

        public CandidatesServiceTests()
        {
            _autoMocker = new AutoMocker();
            _service = _autoMocker.CreateInstance<CandidatesService>();
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.Create))]
        [Theory(DisplayName = "Create candidate without score"), AutoData]
        public async Task WhenCreateIsCalled_WithCandidateWithoutScore_ShouldPersist(Guid id, string name)
        {
            //Arrange
            var candidate = new Candidate
            {
                Id = id,
                Name = name,
            };

            //Act
            await _service.Create(candidate, CancellationToken.None);

            //Assert
            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.Create(candidate, CancellationToken.None), Times.Once);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.Create))]
        [Theory(DisplayName = "Create candidate with valid score"), AutoData]
        public async Task WhenCreateIsCalled_WithCandidateWithValidScore_ShouldPersist(Guid id, string name)
        {
            //Arrange
            var candidate = new Candidate
            {
                Id = id,
                Name = name,
                Score = GetRandomValidCandidateScore()
            };

            //Act
            await _service.Create(candidate, CancellationToken.None);

            //Assert
            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.Create(candidate, CancellationToken.None), Times.Once);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.Create))]
        [Theory(DisplayName = "Create candidate with invalid score")]
        [InlineAutoData(-1)]
        [InlineAutoData(-101)]
        public async Task WhenCreateIsCalled_WithCandidateWithInvalidScore_ShouldNotPersist(int score, Guid id, string name)
        {
            //Arrange
            var candidate = new Candidate
            {
                Id = id,
                Name = name,
                Score = score
            };

            //Act
            var exception = await Assert.ThrowsAsync<DomainValidationException>(async () => await _service.Create(candidate, CancellationToken.None));

            //Assert
            Assert.Equal("The provided candidate score is invalid", exception.Message);
            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.Create(candidate, CancellationToken.None), Times.Never);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.Create))]
        [Theory(DisplayName = "Create candidate with invalid name")]
        [InlineAutoData(null)]
        [InlineAutoData("")]
        public async Task WhenCreateIsCalled_WithCandidateWithInvalidName_ShouldNotPersist(string name, Guid id)
        {
            //Arrange
            var candidate = new Candidate
            {
                Id = id,
                Name = name,
                Score = GetRandomValidCandidateScore()
            };

            //Act
            var exception = await Assert.ThrowsAsync<DomainValidationException>(async () => await _service.Create(candidate, CancellationToken.None));

            //Assert
            Assert.Equal("The provided candidate name is invalid", exception.Message);
            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.Create(candidate, CancellationToken.None), Times.Never);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.Update))]
        [Theory(DisplayName = "Update candidate with invalid score")]
        [InlineAutoData(-1)]
        [InlineAutoData(-101)]
        public async Task WhenUpdateIsCalled_WithCandidateWithInvalidScoreForUpdate_ShouldPersist(int score, Guid id, string name)
        {
            //Arrange
            _autoMocker.GetMock<ICandidatesRepository>()
                .Setup(m => m.GetById(id, CancellationToken.None))
                .ReturnsAsync(new Candidate
                {
                    Id = id,
                    Name = name
                });

            var candidate = new Candidate
            {
                Id = id,
                Name = name,
                Score = score
            };

            //Act
            var exception = await Assert.ThrowsAsync<DomainValidationException>(async () => await _service.Update(candidate, CancellationToken.None));

            //Assert
            Assert.Equal("The provided candidate score is invalid", exception.Message);

            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.GetById(id, CancellationToken.None), Times.Never);
            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.Update(It.Is<Candidate>(e => e.Score == score), CancellationToken.None), Times.Never);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.Update))]
        [Theory(DisplayName = "Update inexistent candidate"), AutoData]
        public async Task WhenUpdateIsCalled_WithCandidateNotFound_ShouldNotPersist(int score, Guid id, string name)
        {
            //Arrange
            var candidate = new Candidate
            {
                Id = id,
                Name = name,
                Score = GetRandomValidCandidateScore()
            };

            //Act
            var exception = await Assert.ThrowsAsync<DomainNotFoundException>(async () => await _service.Update(candidate, CancellationToken.None));

            //Assert
            Assert.Equal("The candidate was not found for provided id", exception.Message);

            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.GetById(id, CancellationToken.None), Times.Once);
            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.Update(It.Is<Candidate>(e => e.Score == score), CancellationToken.None), Times.Never);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.Update))]
        [Theory(DisplayName = "Update candidate with valid score"), AutoData]
        public async Task WhenUpdateIsCalled_WithCandidateForUpdate_ShouldPersist(Guid id, string name)
        {
            //Arrange
            var score = GetRandomValidCandidateScore();

            _autoMocker.GetMock<ICandidatesRepository>()
                .Setup(m => m.GetById(id, CancellationToken.None))
                .ReturnsAsync(new Candidate
                {
                    Id = id,
                    Name = name
                });

            var candidate = new Candidate
            {
                Id = id,
                Name = name,
                Score = score
            };

            //Act
            await _service.Update(candidate, CancellationToken.None);

            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.GetById(id, CancellationToken.None), Times.Once);
            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.Update(It.Is<Candidate>(e => e.Score == score), CancellationToken.None), Times.Once);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.GetById))]
        [Theory(DisplayName = "Get existing candidate by id"), AutoData]
        public async Task WhenGetByIdIsCalled_WithExistingCandidate_ShouldReturnCandidate(Guid id, string name)
        {
            //Arrange
            var candidate = new Candidate
            {
                Id = id,
                Name = name,
                Score = GetRandomValidCandidateScore()
            };

            _autoMocker.GetMock<ICandidatesRepository>()
                .Setup(m => m.GetById(id, CancellationToken.None))
                .ReturnsAsync(candidate);

            //Act
            var result = await _service.GetById(id, CancellationToken.None);

            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.GetById(id, CancellationToken.None), Times.Once);
            Assert.Equivalent(candidate, result);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.GetById))]
        [Theory(DisplayName = "Get inexisting candidate by id"), AutoData]
        public async Task WhenGetByIdIsCalled_WithInexistingCandidate_ShouldReturnCandidate(Guid id, string name)
        {
            //Arrange
            var candidate = new Candidate
            {
                Id = id,
                Name = name,
                Score = GetRandomValidCandidateScore()
            };

            //Act
            var result = await _service.GetById(id, CancellationToken.None);

            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.GetById(id, CancellationToken.None), Times.Once);
            Assert.Null(result);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.Delete))]
        [Theory(DisplayName = "Delete candidate by id"), AutoData]
        public async Task WhenDeleteIsCalled_WithInexistingCandidateById_ShouldReturnCandidate(Guid id, string name)
        {
            //Act
            await _service.Delete(id, CancellationToken.None);

            //Assert
            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.Delete(id, CancellationToken.None), Times.Once);
        }

        private static int GetRandomValidCandidateScore() => new Faker().Random.Int(0, 100);
    }
}