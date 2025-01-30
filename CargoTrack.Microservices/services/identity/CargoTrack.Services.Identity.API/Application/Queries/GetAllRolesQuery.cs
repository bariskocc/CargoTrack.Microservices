using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Queries
{
    public class GetAllRolesQuery : IRequest<IEnumerable<RoleDto>>
    {
        public bool IncludeDeleted { get; }

        public GetAllRolesQuery(bool includeDeleted = false)
        {
            IncludeDeleted = includeDeleted;
        }
    }

    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
    {
        private readonly IRoleRepository _roleRepository;

        public GetAllRolesQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.GetRolesWithPermissionsAsync();
            var roleDtos = new List<RoleDto>();

            foreach (var role in roles)
            {
                if (!request.IncludeDeleted && role.IsDeleted)
                    continue;

                var permissions = await _roleRepository.GetRolePermissionsAsync(role.Id);

                roleDtos.Add(new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    CreatedDate = role.CreatedDate,
                    LastModifiedDate = role.LastModifiedDate,
                    Permissions = permissions.Select(p => p.SystemName).ToList()
                });
            }

            return roleDtos;
        }
    }
} 