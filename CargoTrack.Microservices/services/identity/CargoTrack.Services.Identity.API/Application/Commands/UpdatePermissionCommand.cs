using System;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Commands
{
    public class UpdatePermissionCommand : IRequest<PermissionDto>
    {
        public Guid Id { get; }
        public UpdatePermissionDto PermissionDto { get; }

        public UpdatePermissionCommand(Guid id, UpdatePermissionDto permissionDto)
        {
            Id = id;
            PermissionDto = permissionDto;
        }
    }

    public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, PermissionDto>
    {
        private readonly IPermissionRepository _permissionRepository;

        public UpdatePermissionCommandHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<PermissionDto> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = await _permissionRepository.GetByIdAsync(request.Id);
            if (permission == null)
                throw new Exception("İzin bulunamadı.");

            if (!string.IsNullOrEmpty(request.PermissionDto.SystemName) && 
                permission.SystemName != request.PermissionDto.SystemName && 
                !await _permissionRepository.IsSystemNameUniqueAsync(request.PermissionDto.SystemName))
            {
                throw new Exception("Bu sistem adı zaten kullanımda.");
            }

            permission.Update(
                request.PermissionDto.Name,
                request.PermissionDto.SystemName,
                request.PermissionDto.Description,
                request.PermissionDto.Category
            );

            await _permissionRepository.UpdateAsync(permission);

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