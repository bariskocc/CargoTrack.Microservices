using System;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Queries
{
    public class GetPermissionByIdQuery : IRequest<PermissionDto>
    {
        public Guid Id { get; }

        public GetPermissionByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, PermissionDto>
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetPermissionByIdQueryHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<PermissionDto> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
        {
            var permission = await _permissionRepository.GetByIdAsync(request.Id);
            if (permission == null)
                throw new Exception("İzin bulunamadı.");

            return new PermissionDto
            {
                Id = permission.Id,
                Name = permission.Name,
                SystemName = permission.SystemName,
                Description = permission.Description,
                Category = permission.Category,
                CreatedDate = permission.CreatedDate,
                LastModifiedDate = permission.LastModifiedDate
            };
        }
    }
} 