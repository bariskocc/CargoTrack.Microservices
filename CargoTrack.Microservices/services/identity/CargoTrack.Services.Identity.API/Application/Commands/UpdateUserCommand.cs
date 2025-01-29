using System;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Commands
{
    public class UpdateUserCommand : IRequest<UserDto>
    {
        public Guid UserId { get; }
        public UpdateUserDto UserDto { get; }

        public UpdateUserCommand(Guid userId, UpdateUserDto userDto)
        {
            UserId = userId;
            UserDto = userDto;
        }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            user.UpdateProfile(
                request.UserDto.FirstName,
                request.UserDto.LastName,
                request.UserDto.CompanyName,
                request.UserDto.PhoneNumber
            );

            await _userRepository.UpdateAsync(user);

            var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);
            var roles = await _userRepository.GetUsersInRoleAsync(user.Id.ToString());

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
                Roles = roles.Select(r => r.ToString()).ToList(),
                Permissions = permissions
            };
        }
    }
} 