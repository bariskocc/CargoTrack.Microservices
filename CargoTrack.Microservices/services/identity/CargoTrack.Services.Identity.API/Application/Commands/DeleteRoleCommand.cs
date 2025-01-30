using System;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Commands
{
    public class DeleteRoleCommand : IRequest<bool>
    {
        public Guid RoleId { get; }

        public DeleteRoleCommand(Guid roleId)
        {
            RoleId = roleId;
        }
    }

    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;

        public DeleteRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(request.RoleId);
            if (role == null)
                throw new Exception("Rol bulunamadı.");

            role.Delete();
            await _roleRepository.UpdateAsync(role);

            return true;
        }
    }
} 