using CargoTrack.Services.Identity.API.Application.DTOs;
using FluentValidation;

namespace CargoTrack.Services.Identity.API.Application.Validators
{
    public class UpdateRoleDtoValidator : AbstractValidator<UpdateRoleDto>
    {
        public UpdateRoleDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Rol adı boş olamaz.")
                .MinimumLength(3).WithMessage("Rol adı en az 3 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Rol adı 50 karakterden uzun olamaz.")
                .Matches("^[a-zA-Z0-9._-]+$").WithMessage("Rol adı sadece harf, rakam ve ._- karakterlerini içerebilir.");

            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("Açıklama 200 karakterden uzun olamaz.");
        }
    }
} 