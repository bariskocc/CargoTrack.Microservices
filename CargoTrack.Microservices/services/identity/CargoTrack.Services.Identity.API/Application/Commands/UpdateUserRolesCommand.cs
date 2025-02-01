using System;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Commands
{
    public class UpdateUserRolesCommand : IRequest<UserDto>
    {
        public Guid UserId { get; }
        public UpdateUserRolesDto UpdateRolesDto { get; }

        public UpdateUserRolesCommand(Guid userId, UpdateUserRolesDto updateRolesDto)
        {
            UserId = userId;
            UpdateRolesDto = updateRolesDto;
        }
    }

    public class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UpdateUserRolesCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<UserDto> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            user.ClearRoles();

            if (request.UpdateRolesDto.Roles != null)
            {
                foreach (var roleName in request.UpdateRolesDto.Roles)
                {
                    var role = await _roleRepository.GetByNameAsync(roleName);
                    if (role != null)
                    {
                        user.AddRole(role);
                    }
                }
            }

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
                Permissions = permissions.ToList()
            };
        }
    }
} 