using System;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Commands
{
    public class DeletePermissionCommand : IRequest<bool>
    {
        public Guid Id { get; }

        public DeletePermissionCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand, bool>
    {
        private readonly IPermissionRepository _permissionRepository;

        public DeletePermissionCommandHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<bool> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = await _permissionRepository.GetByIdAsync(request.Id);
            if (permission == null)
                throw new Exception("İzin bulunamadı.");

            await _permissionRepository.DeleteAsync(permission);
            return true;
        }
    }
} 