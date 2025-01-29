using CargoTrack.Services.Identity.API.Application.DTOs;
using FluentValidation;

namespace CargoTrack.Services.Identity.API.Application.Validators
{
    public class UserChangePasswordDtoValidator : AbstractValidator<UserChangePasswordDto>
    {
        public UserChangePasswordDtoValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Mevcut şifre boş olamaz.")
                .MinimumLength(8).WithMessage("Mevcut şifre en az 8 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Mevcut şifre 50 karakterden uzun olamaz.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Yeni şifre boş olamaz.")
                .MinimumLength(8).WithMessage("Yeni şifre en az 8 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Yeni şifre 50 karakterden uzun olamaz.")
                .Matches("[A-Z]").WithMessage("Yeni şifre en az bir büyük harf içermelidir.")
                .Matches("[a-z]").WithMessage("Yeni şifre en az bir küçük harf içermelidir.")
                .Matches("[0-9]").WithMessage("Yeni şifre en az bir rakam içermelidir.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Yeni şifre en az bir özel karakter içermelidir.")
                .NotEqual(x => x.CurrentPassword).WithMessage("Yeni şifre mevcut şifre ile aynı olamaz.");

            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty().WithMessage("Şifre tekrarı boş olamaz.")
                .Equal(x => x.NewPassword).WithMessage("Şifreler eşleşmiyor.");
        }
    }
} 