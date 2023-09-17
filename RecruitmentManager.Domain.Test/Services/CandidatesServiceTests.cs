using AutoFixture.Xunit2;
using AutoMapper;
using Bogus;
using Moq;
using Moq.AutoMock;
using RecruitmentManager.Domain.Configuration;
using RecruitmentManager.Domain.Dtos;
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
            _autoMocker.Use(new MapperConfiguration(mc => mc.AddProfile(new AutoMapperConfiguration())).CreateMapper());
            _service = _autoMocker.CreateInstance<CandidatesService>();
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.CreateAsync))]
        [Theory(DisplayName = "Create candidate without score"), AutoData]
        public async Task WhenCreateIsCalled_WithCandidateWithoutScore_ShouldPersist(string name)
        {
            //Arrange
            var candidateSaveDto = new CandidateSaveDto
            {
                Name = name,
            };

            //Act
            await _service.CreateAsync(candidateSaveDto);

            //Assert
            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.CreateAsync(
                It.Is<Candidate>(e => e.Name == candidateSaveDto.Name)), Times.Once);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.CreateAsync))]
        [Theory(DisplayName = "Create candidate with valid score"), AutoData]
        public async Task WhenCreateIsCalled_WithCandidateWithValidScore_ShouldPersist(string name)
        {
            //Arrange
            var candidateSaveDto = new CandidateSaveDto
            {
                Name = name,
                Score = GetRandomValidCandidateScore()
            };

            //Act
            await _service.CreateAsync(candidateSaveDto);

            //Assert
            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.CreateAsync(
                It.Is<Candidate>(e => e.Name == candidateSaveDto.Name && e.Score == candidateSaveDto.Score)), Times.Once);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.CreateAsync))]
        [Theory(DisplayName = "Create candidate with invalid score")]
        [InlineAutoData(-1)]
        [InlineAutoData(-101)]
        public async Task WhenCreateIsCalled_WithCandidateWithInvalidScore_ShouldNotPersist(int score, string name)
        {
            //Arrange
            var candidateSaveDto = new CandidateSaveDto
            {
                Name = name,
                Score = score
            };

            //Act
            var exception = await Assert.ThrowsAsync<DomainValidationException>(async () => await _service.CreateAsync(candidateSaveDto));

            //Assert
            Assert.Equal("The provided candidate score is invalid", exception.Message);
            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.CreateAsync(It.IsAny<Candidate>()), Times.Never);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.CreateAsync))]
        [Theory(DisplayName = "Create candidate with invalid name")]
        [InlineAutoData(null)]
        [InlineAutoData("")]
        public async Task WhenCreateIsCalled_WithCandidateWithInvalidName_ShouldNotPersist(string name)
        {
            //Arrange
            var candidateSaveDto = new CandidateSaveDto
            {
                Name = name,
                Score = GetRandomValidCandidateScore()
            };

            //Act
            var exception = await Assert.ThrowsAsync<DomainValidationException>(async () => await _service.CreateAsync(candidateSaveDto));

            //Assert
            Assert.Equal("The provided candidate name is invalid", exception.Message);
            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.CreateAsync(It.IsAny<Candidate>()), Times.Never);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.UpdateAsync))]
        [Theory(DisplayName = "Update candidate with invalid score")]
        [InlineAutoData(-1)]
        [InlineAutoData(-101)]
        public async Task WhenUpdateIsCalled_WithCandidateWithInvalidScoreForUpdate_ShouldPersist(int score, int id, string name)
        {
            //Arrange
            _autoMocker.GetMock<ICandidatesRepository>()
                .Setup(m => m.GetByIdAsync(id))
                .ReturnsAsync(new Candidate
                {
                    Id = id,
                    Name = name
                });

            var candidateSaveDto = new CandidateSaveDto
            {
                Name = name,
                Score = score
            };

            //Act
            var exception = await Assert.ThrowsAsync<DomainValidationException>(async () => await _service.UpdateAsync(id, candidateSaveDto));

            //Assert
            Assert.Equal("The provided candidate score is invalid", exception.Message);

            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.GetByIdAsync(id), Times.Never);
            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.UpdateAsync(It.IsAny<Candidate>()), Times.Never);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.UpdateAsync))]
        [Theory(DisplayName = "Update inexistent candidate"), AutoData]
        public async Task WhenUpdateIsCalled_WithCandidateNotFound_ShouldNotPersist(int id, string name)
        {
            //Arrange
            var candidateSaveDto = new CandidateSaveDto
            {
                Name = name,
                Score = GetRandomValidCandidateScore()
            };

            //Act
            var exception = await Assert.ThrowsAsync<DomainNotFoundException>(async () => await _service.UpdateAsync(id, candidateSaveDto));

            //Assert
            Assert.Equal("The candidate was not found for provided id", exception.Message);

            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.GetByIdAsync(id), Times.Once);
            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.UpdateAsync(It.IsAny<Candidate>()), Times.Never);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.UpdateAsync))]
        [Theory(DisplayName = "Update candidate with valid score"), AutoData]
        public async Task WhenUpdateIsCalled_WithCandidateForUpdate_ShouldPersist(int id, string name)
        {
            //Arrange
            var score = GetRandomValidCandidateScore();

            _autoMocker.GetMock<ICandidatesRepository>()
                .Setup(m => m.GetByIdAsync(id))
                .ReturnsAsync(new Candidate
                {
                    Id = id,
                    Name = name
                });

            var candidateSaveDto = new CandidateSaveDto
            {
                Name = name,
                Score = score
            };

            //Act
            await _service.UpdateAsync(id, candidateSaveDto);

            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.GetByIdAsync(id), Times.Once);
            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.UpdateAsync(
                It.Is<Candidate>(e => e.Score == score && e.Name == name)), Times.Once);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.GetAsync))]
        [Fact(DisplayName = "Get existings candidates")]
        public async Task WhenGetCalled_WithExistingCandidates_ShouldReturnCandidates()
        {
            //Arrange
            var faker = new Faker();
            var candidates = new Candidate[]
            {
                new()
                {
                    Id = 1,
                    Name = faker.Random.String(),
                    Score = GetRandomValidCandidateScore()
                },
                new()
                {
                    Id = 2,
                    Name = faker.Random.String(),
                    Score = GetRandomValidCandidateScore()
                },
            };

            _autoMocker.GetMock<ICandidatesRepository>()
                .Setup(m => m.GetAsync())
                .ReturnsAsync(candidates);

            //Act
            var result = await _service.GetAsync();

            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.GetAsync(), Times.Once);
            Assert.Equivalent(candidates, result);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.GetAsync))]
        [Fact(DisplayName = "Get no candidates")]
        public async Task WhenGetCalled_WithNoCandidates_ShouldReturnCandidates()
        {
            //Arrange
            _autoMocker.GetMock<ICandidatesRepository>()
                .Setup(m => m.GetAsync())
                .ReturnsAsync(Enumerable.Empty<Candidate>());

            //Act
            var result = await _service.GetAsync();

            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.GetAsync(), Times.Once);
            Assert.Empty(result);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.GetByIdAsync))]
        [Theory(DisplayName = "Get existing candidate by id"), AutoData]
        public async Task WhenGetByIdIsCalled_WithExistingCandidate_ShouldReturnCandidate(int id, string name)
        {
            //Arrange
            var candidate = new Candidate
            {
                Id = id,
                Name = name,
                Score = GetRandomValidCandidateScore()
            };

            _autoMocker.GetMock<ICandidatesRepository>()
                .Setup(m => m.GetByIdAsync(id))
                .ReturnsAsync(candidate);

            //Act
            var result = await _service.GetByIdAsync(id);

            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.GetByIdAsync(id), Times.Once);
            Assert.Equivalent(candidate, result);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.GetByIdAsync))]
        [Theory(DisplayName = "Get inexisting candidate by id"), AutoData]
        public async Task WhenGetByIdIsCalled_WithInexistingCandidate_ShouldReturnCandidate(int id, string name)
        {
            //Arrange
            var candidate = new Candidate
            {
                Id = id,
                Name = name,
                Score = GetRandomValidCandidateScore()
            };

            //Act
            var result = await _service.GetByIdAsync(id);

            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.GetByIdAsync(id), Times.Once);
            Assert.Null(result);
        }

        [Trait(nameof(CandidatesService), nameof(CandidatesService.DeleteAsync))]
        [Theory(DisplayName = "Delete candidate by id"), AutoData]
        public async Task WhenDeleteIsCalled_WithInexistingCandidateById_ShouldReturnCandidate(int id)
        {
            //Act
            await _service.DeleteAsync(id);

            //Assert
            _autoMocker.GetMock<ICandidatesRepository>().Verify(m => m.DeleteAsync(id), Times.Once);
        }

        private static int GetRandomValidCandidateScore() => new Faker().Random.Int(0, 100);
    }
}