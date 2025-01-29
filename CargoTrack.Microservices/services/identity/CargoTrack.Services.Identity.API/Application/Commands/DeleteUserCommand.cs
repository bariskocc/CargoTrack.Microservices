using System;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Commands
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public Guid UserId { get; }

        public DeleteUserCommand(Guid userId)
        {
            UserId = userId;
        }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            user.Delete();
            await _userRepository.UpdateAsync(user);

            return true;
        }
    }
} 