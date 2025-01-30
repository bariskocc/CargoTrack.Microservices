using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Domain.Entities;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;
using BC = BCrypt.Net.BCrypt;

namespace CargoTrack.Services.Identity.API.Application.Commands
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public CreateUserDto UserDto { get; }

        public CreateUserCommand(CreateUserDto userDto)
        {
            UserDto = userDto;
        }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public CreateUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Email ve kullanıcı adı kontrolü
            if (await _userRepository.IsEmailUniqueAsync(request.UserDto.Email) == false)
                throw new System.Exception("Bu email adresi zaten kullanımda.");

            if (await _userRepository.IsUsernameUniqueAsync(request.UserDto.Username) == false)
                throw new System.Exception("Bu kullanıcı adı zaten kullanımda.");

            // Şifre hash'leme
            string passwordHash = BC.HashPassword(request.UserDto.Password);

            // Yeni kullanıcı oluşturma
            var user = new User(
                request.UserDto.Email,
                request.UserDto.Username,
                passwordHash,
                request.UserDto.FirstName,
                request.UserDto.LastName,
                request.UserDto.CompanyName,
                request.UserDto.PhoneNumber
            );

            // Rolleri ekleme
            if (request.UserDto.Roles != null)
            {
                foreach (var roleName in request.UserDto.Roles)
                {
                    var role = await _roleRepository.GetByNameAsync(roleName);
                    if (role != null)
                    {
                        user.AddRole(role);
                    }
                }
            }

            // Kullanıcıyı kaydetme
            await _userRepository.AddAsync(user);

            // Kullanıcı izinlerini alma
            var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);

            // DTO oluşturma ve döndürme
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CompanyName = user.CompanyName,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                CreatedDate = user.CreatedDate,
                LastModifiedDate = user.LastModifiedDate,
                LastLoginDate = user.LastLoginDate,
                Roles = request.UserDto.Roles ?? new List<string>(),
                Permissions = permissions.ToList()
            };
        }
    }
} 