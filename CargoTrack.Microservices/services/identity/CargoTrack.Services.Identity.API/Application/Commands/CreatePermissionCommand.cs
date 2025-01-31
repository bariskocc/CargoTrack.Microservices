using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Domain.Entities;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Commands
{
    public class CreatePermissionCommand : IRequest<PermissionDto>
    {
        public CreatePermissionDto PermissionDto { get; }

        public CreatePermissionCommand(CreatePermissionDto permissionDto)
        {
            PermissionDto = permissionDto;
        }
    }

    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, PermissionDto>
    {
        private readonly IPermissionRepository _permissionRepository;

        public CreatePermissionCommandHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<PermissionDto> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            if (await _permissionRepository.IsSystemNameUniqueAsync(request.PermissionDto.SystemName) == false)
                throw new System.Exception("Bu sistem adı zaten kullanımda.");

            var permission = new Permission(
                request.PermissionDto.Name,
                request.PermissionDto.SystemName,
                request.PermissionDto.Description,
                request.PermissionDto.Category
            );

            await _permissionRepository.AddAsync(permission);

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