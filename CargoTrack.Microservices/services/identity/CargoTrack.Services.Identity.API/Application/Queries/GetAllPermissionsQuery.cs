using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Queries
{
    public class GetAllPermissionsQuery : IRequest<IEnumerable<PermissionDto>>
    {
    }

    public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, IEnumerable<PermissionDto>>
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetAllPermissionsQueryHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<PermissionDto>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permissions = await _permissionRepository.GetAllAsync();
            return permissions.Select(p => new PermissionDto
            {
                Id = p.Id,
                Name = p.Name,
                SystemName = p.SystemName,
                Description = p.Description,
                Category = p.Category,
                CreatedDate = p.CreatedDate,
                LastModifiedDate = p.LastModifiedDate
            });
        }
    }
} 