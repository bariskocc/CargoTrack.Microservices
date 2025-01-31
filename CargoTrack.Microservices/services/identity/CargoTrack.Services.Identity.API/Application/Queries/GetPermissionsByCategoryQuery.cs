using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Queries
{
    public class GetPermissionsByCategoryQuery : IRequest<IEnumerable<PermissionDto>>
    {
        public string Category { get; }

        public GetPermissionsByCategoryQuery(string category)
        {
            Category = category;
        }
    }

    public class GetPermissionsByCategoryQueryHandler : IRequestHandler<GetPermissionsByCategoryQuery, IEnumerable<PermissionDto>>
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetPermissionsByCategoryQueryHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var permissions = await _permissionRepository.GetByCategoryAsync(request.Category);
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