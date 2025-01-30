using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Commands
{
    public class UpdateRolePermissionsCommand : IRequest<RoleDto>
    {
        public RolePermissionsUpdateDto UpdateDto { get; }

        public UpdateRolePermissionsCommand(RolePermissionsUpdateDto updateDto)
        {
            UpdateDto = updateDto;
        }
    }

    public class UpdateRolePermissionsCommandHandler : IRequestHandler<UpdateRolePermissionsCommand, RoleDto>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;

        public UpdateRolePermissionsCommandHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<RoleDto> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(request.UpdateDto.RoleId);
            if (role == null)
                throw new System.Exception("Rol bulunamadı.");

            // Mevcut izinleri temizle
            var currentPermissions = await _roleRepository.GetRolePermissionsAsync(role.Id);
            foreach (var permission in currentPermissions)
            {
                role.RemovePermission(permission);
            }

            // Yeni izinleri ekle
            if (request.UpdateDto.Permissions != null)
            {
                foreach (var permissionName in request.UpdateDto.Permissions)
                {
                    var permission = await _permissionRepository.GetBySystemNameAsync(permissionName);
                    if (permission != null)
                    {
                        role.AddPermission(permission);
                    }
                }
            }

            await _roleRepository.UpdateAsync(role);

            var updatedPermissions = await _roleRepository.GetRolePermissionsAsync(role.Id);

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                CreatedDate = role.CreatedDate,
                LastModifiedDate = role.LastModifiedDate,
                Permissions = updatedPermissions.Select(p => p.SystemName).ToList()
            };
        }
    }
} 