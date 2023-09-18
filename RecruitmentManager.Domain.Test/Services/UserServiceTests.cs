using AutoFixture.Xunit2;
using AutoMapper;
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
    public class UsersServiceTests
    {
        private readonly AutoMocker _autoMocker;
        private readonly IUsersService _service;

        public UsersServiceTests()
        {
            _autoMocker = new AutoMocker();
            _autoMocker.Use(new MapperConfiguration(mc => mc.AddProfile(new AutoMapperConfiguration())).CreateMapper());
            _service = _autoMocker.CreateInstance<UsersService>();
        }

        [Trait(nameof(UsersService), nameof(UsersService.CreateAsync))]
        [Theory(DisplayName = "Create user"), AutoData]
        public async Task WhenCreateIsCalled_WithUserWithValidData_ShouldPersist(string name, string password)
        {
            //Arrange
            var userDto = new UserDto
            {
                Name = name,
                Password = password
            };

            //Act
            await _service.CreateAsync(userDto);

            //Assert
            _autoMocker.GetMock<IUsersRepository>().Verify(m => m.ExistsByNameAndPassword(userDto.Name, null), Times.Once);
            _autoMocker.GetMock<IUsersRepository>().Verify(m => m.CreateAsync(
                It.Is<User>(e => e.Name == userDto.Name && e.Password == userDto.Password)), Times.Once);
        }

        [Trait(nameof(UsersService), nameof(UsersService.CreateAsync))]
        [Theory(DisplayName = "Create user with invalid data")]
        [InlineData(null, "password", "The user name should be provided")]
        [InlineData("username", null, "The password should be provided")]
        public async Task WhenCreateIsCalled_WithInvalidData_ShouldThrowValidationException(string name, string password, string expected)
        {
            //Arrange
            var userDto = new UserDto
            {
                Name = name,
                Password = password
            };

            //Act
            var result = await Assert.ThrowsAsync<DomainValidationException>(async () => await _service.CreateAsync(userDto));

            //Assert
            Assert.Equal(expected, result.Message);

            _autoMocker.GetMock<IUsersRepository>().Verify(m => m.ExistsByNameAndPassword(userDto.Name, null), Times.Never);
            _autoMocker.GetMock<IUsersRepository>().Verify(m => m.CreateAsync(It.IsAny<User>()), Times.Never);
        }

        [Trait(nameof(UsersService), nameof(UsersService.CreateAsync))]
        [Theory(DisplayName = "Create user with same name already existing"), AutoData]
        public async Task WhenCreateIsCalled_WithAlreadyExistingUserWithSameName_ShouldThrowException(string name, string password)
        {
            //Arrange
            var userDto = new UserDto
            {
                Name = name,
                Password = password
            };

            _autoMocker.GetMock<IUsersRepository>()
                .Setup(m => m.ExistsByNameAndPassword(name, null))
                .ReturnsAsync(true);

            //Act
            var result = await Assert.ThrowsAsync<DomainValidationException>(async () => await _service.CreateAsync(userDto));

            //Assert
            Assert.Equal("The provided user name already exists", result.Message);

            _autoMocker.GetMock<IUsersRepository>().Verify(m => m.ExistsByNameAndPassword(userDto.Name, null), Times.Once);
            _autoMocker.GetMock<IUsersRepository>().Verify(m => m.CreateAsync(It.IsAny<User>()), Times.Never);
        }

        [Trait(nameof(UsersService), nameof(UsersService.GetUserNamesAsync))]
        [Theory(DisplayName = "Get existings users names"), AutoData]
        public async Task WhenGetUserNamesIsCalled_WithExistingUsers_ShouldReturnUsersNames(string firstName, string secondName)
        {
            //Arrange
            var users = new User[]
            {
                new()
                {
                    Id = 1,
                    Name = firstName
                },
                new()
                {
                    Id = 2,
                    Name = secondName
                },
            };

            _autoMocker.GetMock<IUsersRepository>()
                .Setup(m => m.GetAsync())
                .ReturnsAsync(users);

            //Act
            var result = await _service.GetUserNamesAsync();

            _autoMocker.GetMock<IUsersRepository>().Verify(m => m.GetAsync(), Times.Once);
            Assert.Equivalent(users.Select(e => e.Name), result);
        }

        [Trait(nameof(UsersService), nameof(UsersService.GetUserNamesAsync))]
        [Fact(DisplayName = "Get no users")]
        public async Task WhenGetUserNamesIsCalled_WithNoUsers_ShouldReturnEmpty()
        {
            //Arrange
            _autoMocker.GetMock<IUsersRepository>()
                .Setup(m => m.GetAsync())
                .ReturnsAsync(Enumerable.Empty<User>());

            //Act
            var result = await _service.GetUserNamesAsync();

            _autoMocker.GetMock<IUsersRepository>().Verify(m => m.GetAsync(), Times.Once);
            Assert.Empty(result);
        }

        [Trait(nameof(UsersService), nameof(UsersService.LoginAsync))]
        [Theory(DisplayName = "Login"), AutoData]
        public async Task WhenLoginIsCalled_WithValidUser_ShouldReturnToken(string name, string password, string token)
        {
            //Arrange
            var userDto = new UserDto
            {
                Name = name,
                Password = password
            };

            _autoMocker.GetMock<IUsersRepository>()
                .Setup(m => m.ExistsByNameAndPassword(name, password))
                .ReturnsAsync(true);

            _autoMocker.GetMock<ITokenService>()
                .Setup(m => m.GenerateToken(userDto))
                .Returns(token);

            //Act
            var result = await _service.LoginAsync(userDto);

            //Assert
            Assert.Equal(token, result);

            _autoMocker.GetMock<IUsersRepository>().Verify(m => m.ExistsByNameAndPassword(userDto.Name, userDto.Password), Times.Once);
            _autoMocker.GetMock<ITokenService>().Verify(m => m.GenerateToken(userDto), Times.Once);
        }

        [Trait(nameof(UsersService), nameof(UsersService.LoginAsync))]
        [Theory(DisplayName = "Incorrect login"), AutoData]
        public async Task WhenLoginIsCalled_WithInvalidUser_ShouldThrowException(string name, string password)
        {
            //Arrange
            var userDto = new UserDto
            {
                Name = name,
                Password = $"{password}wrong"
            };

            _autoMocker.GetMock<IUsersRepository>()
                .Setup(m => m.ExistsByNameAndPassword(name, password))
                .ReturnsAsync(true);

            _autoMocker.GetMock<IUsersRepository>()
                .Setup(m => m.ExistsByNameAndPassword(name, userDto.Password))
                .ReturnsAsync(false);

            //Act
            var result = await Assert.ThrowsAsync<DomainNotFoundException>(async () => await _service.LoginAsync(userDto));

            //Assert
            Assert.Equal("The provided user name and/or password are incorrect", result.Message);

            _autoMocker.GetMock<IUsersRepository>().Verify(m => m.ExistsByNameAndPassword(userDto.Name, userDto.Password), Times.Once);
        }
    }
}