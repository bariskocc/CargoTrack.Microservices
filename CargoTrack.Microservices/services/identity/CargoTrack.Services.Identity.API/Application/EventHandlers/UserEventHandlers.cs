using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Domain.Events;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using CargoTrack.Services.Identity.API.Domain.Services;
using Microsoft.Extensions.Logging;

namespace CargoTrack.Services.Identity.API.Application.EventHandlers
{
    public class UserCreatedEventHandler : IDomainEventHandler<UserCreatedEvent>
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<UserCreatedEventHandler> _logger;

        public UserCreatedEventHandler(IEmailSender emailSender, ILogger<UserCreatedEventHandler> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Yeni kullanıcı oluşturuldu: {Username} ({Email})", 
                notification.Username, notification.Email);

            // Hoşgeldiniz e-postası gönder
            await _emailSender.SendEmailAsync(
                notification.Email,
                "CargoTrack'e Hoş Geldiniz",
                $"Merhaba {notification.Username},<br><br>" +
                "CargoTrack'e hoş geldiniz. Hesabınız başarıyla oluşturuldu.");
        }
    }

    public class UserLockedOutEventHandler : IDomainEventHandler<UserLockedOutEvent>
    {
        private readonly IEmailSender _emailSender;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserLockedOutEventHandler> _logger;

        public UserLockedOutEventHandler(
            IEmailSender emailSender,
            IUserRepository userRepository,
            ILogger<UserLockedOutEventHandler> logger)
        {
            _emailSender = emailSender;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Handle(UserLockedOutEvent notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(notification.UserId);
            if (user == null) return;

            _logger.LogWarning("Kullanıcı hesabı kilitlendi: {Username} (ID: {UserId})", 
                user.Username, notification.UserId);

            await _emailSender.SendAccountLockedNotificationAsync(
                user.Email,
                $"Hesabınız {notification.LockoutEnd.ToLocalTime()} tarihine kadar kilitlendi.");
        }
    }

    public class UserRolesUpdatedEventHandler : IDomainEventHandler<UserRolesUpdatedEvent>
    {
        private readonly ILogger<UserRolesUpdatedEventHandler> _logger;

        public UserRolesUpdatedEventHandler(ILogger<UserRolesUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(UserRolesUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Kullanıcı rolleri güncellendi. UserId: {UserId}, Yeni Roller: {Roles}",
                notification.UserId,
                string.Join(", ", notification.RoleNames));

            return Task.CompletedTask;
        }
    }
} 