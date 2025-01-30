using System;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Commands
{
    public class UpdateRoleCommand : IRequest<RoleDto>
    {
        public Guid RoleId { get; }
        public UpdateRoleDto RoleDto { get; }

        public UpdateRoleCommand(Guid roleId, UpdateRoleDto roleDto)
        {
            RoleId = roleId;
            RoleDto = roleDto;
        }
    }

    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleDto>
    {
        private readonly IRoleRepository _roleRepository;

        public UpdateRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RoleDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(request.RoleId);
            if (role == null)
                throw new Exception("Rol bulunamadı.");

            if (role.Name != request.RoleDto.Name && await _roleRepository.IsNameUniqueAsync(request.RoleDto.Name) == false)
                throw new Exception("Bu rol adı zaten kullanımda.");

            role.UpdateDetails(request.RoleDto.Name, request.RoleDto.Description);
            await _roleRepository.UpdateAsync(role);

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