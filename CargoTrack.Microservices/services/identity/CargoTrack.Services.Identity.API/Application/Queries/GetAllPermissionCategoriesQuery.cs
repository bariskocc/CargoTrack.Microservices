using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Queries
{
    public class GetAllPermissionCategoriesQuery : IRequest<IEnumerable<string>>
    {
    }

    public class GetAllPermissionCategoriesQueryHandler : IRequestHandler<GetAllPermissionCategoriesQuery, IEnumerable<string>>
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetAllPermissionCategoriesQueryHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<string>> Handle(GetAllPermissionCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _permissionRepository.GetAllCategoriesAsync();
        }
    }
} 