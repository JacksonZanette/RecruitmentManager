using AutoMapper;
using RecruitmentManager.Domain.Dtos;
using RecruitmentManager.Domain.Entities;
using RecruitmentManager.Domain.Interfaces.Repositories;
using RecruitmentManager.Domain.Interfaces.Services;

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
            var user = _mapper.Map<UserDto, User>(userDto);
            await _usersRepository.CreateAsync(user);
        }

        public async Task<string> LoginAsync(UserDto userDto)
        {
            var user = await _usersRepository.ExistsByNameAndPassword(userDto.Name, userDto.Password);
            return _tokenService.GenerateToken(userDto);
        }

        public async Task<IEnumerable<string>> GetUserNamesAsync()
        {
            var users = await _usersRepository.GetAsync();
            return users.Select(e => e.Name);
        }
    }
}