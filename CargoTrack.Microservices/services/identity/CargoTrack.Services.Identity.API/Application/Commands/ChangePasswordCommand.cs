using System;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;
using BC = BCrypt.Net.BCrypt;

namespace CargoTrack.Services.Identity.API.Application.Commands
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public Guid UserId { get; }
        public UserChangePasswordDto PasswordDto { get; }

        public ChangePasswordCommand(Guid userId, UserChangePasswordDto passwordDto)
        {
            UserId = userId;
            PasswordDto = passwordDto;
        }
    }

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public ChangePasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            if (!BC.Verify(request.PasswordDto.CurrentPassword, user.PasswordHash))
                throw new Exception("Mevcut şifre yanlış.");

            if (request.PasswordDto.NewPassword != request.PasswordDto.ConfirmNewPassword)
                throw new Exception("Yeni şifre ve şifre tekrarı eşleşmiyor.");

            string newPasswordHash = BC.HashPassword(request.PasswordDto.NewPassword);
            user.ChangePassword(newPasswordHash);

            await _userRepository.UpdateAsync(user);

            return true;
        }
    }
} 