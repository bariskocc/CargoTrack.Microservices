using System;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Queries
{
    public class GetRoleByIdQuery : IRequest<RoleDto>
    {
        public Guid RoleId { get; }

        public GetRoleByIdQuery(Guid roleId)
        {
            RoleId = roleId;
        }
    }

    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto>
    {
        private readonly IRoleRepository _roleRepository;

        public GetRoleByIdQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(request.RoleId);
            if (role == null)
                throw new Exception("Rol bulunamadı.");

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