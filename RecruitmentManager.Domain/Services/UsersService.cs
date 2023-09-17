using AutoMapper;
using RecruitmentManager.Domain.Dtos;
using RecruitmentManager.Domain.Entities;
using RecruitmentManager.Domain.Interfaces.Repositories;
using RecruitmentManager.Domain.Interfaces.Services;
using RecruitmentManager.Domain.Models;

namespace RecruitmentManager.Domain.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UsersService(IUsersRepository usersRepository, ITokenService tokenService, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task CreateAsync(UserDto userDto)
        {
            ValidateUser(userDto);

            if (await _usersRepository.ExistsByNameAndPassword(userDto.Name))
                throw new DomainValidationException("The provided user name already exists");

            var user = _mapper.Map<UserDto, User>(userDto);
            await _usersRepository.CreateAsync(user);
        }

        public async Task<string> LoginAsync(UserDto userDto)
            => await _usersRepository.ExistsByNameAndPassword(userDto.Name, userDto.Password)
                ? _tokenService.GenerateToken(userDto)
                : throw new DomainNotFoundException("The provided user name and/or password are incorrect");

        public async Task<IEnumerable<string>> GetUserNamesAsync()
            => (await _usersRepository.GetAsync()).Select(e => e.Name);

        private static void ValidateUser(UserDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.Name))
                throw new DomainValidationException("The user name should be provided");

            if (string.IsNullOrEmpty(userDto.Password))
                throw new DomainValidationException("The password should be provided");
        }
    }
}