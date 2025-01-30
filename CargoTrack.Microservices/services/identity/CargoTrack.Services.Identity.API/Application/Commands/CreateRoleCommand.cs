using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Domain.Entities;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Commands
{
    public class CreateRoleCommand : IRequest<RoleDto>
    {
        public CreateRoleDto RoleDto { get; }

        public CreateRoleCommand(CreateRoleDto roleDto)
        {
            RoleDto = roleDto;
        }
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;

        public CreateRoleCommandHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            if (await _roleRepository.IsNameUniqueAsync(request.RoleDto.Name) == false)
                throw new System.Exception("Bu rol adı zaten kullanımda.");

            var role = new Role(request.RoleDto.Name, request.RoleDto.Description);

            if (request.RoleDto.Permissions != null)
            {
                foreach (var permissionName in request.RoleDto.Permissions)
                {
                    var permission = await _permissionRepository.GetBySystemNameAsync(permissionName);
                    if (permission != null)
                    {
                        role.AddPermission(permission);
                    }
                }
            }

            await _roleRepository.AddAsync(role);

            var permissions = await _roleRepository.GetRolePermissionsAsync(role.Id);

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                CreatedDate = role.CreatedDate,
                LastModifiedDate = role.LastModifiedDate,
                Permissions = permissions.Select(p => p.SystemName).ToList()
            };
        }
    }
} 