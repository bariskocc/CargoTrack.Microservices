using CargoTrack.Services.Identity.API.Application.DTOs;
using FluentValidation;

namespace CargoTrack.Services.Identity.API.Application.Validators
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email adresi boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
                .MaximumLength(100).WithMessage("Email adresi 100 karakterden uzun olamaz.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz.")
                .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Şifre 50 karakterden uzun olamaz.");
        }
    }
} 