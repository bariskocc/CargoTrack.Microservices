using CargoTrack.Services.Identity.API.Application.DTOs;
using FluentValidation;

namespace CargoTrack.Services.Identity.API.Application.Validators
{
    public class UpdatePermissionDtoValidator : AbstractValidator<UpdatePermissionDto>
    {
        public UpdatePermissionDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("İzin adı boş olamaz.")
                .MaximumLength(256).WithMessage("İzin adı 256 karakterden uzun olamaz.");

            RuleFor(x => x.SystemName)
                .NotEmpty().WithMessage("Sistem adı boş olamaz.")
                .MaximumLength(256).WithMessage("Sistem adı 256 karakterden uzun olamaz.")
                .Matches("^[a-zA-Z0-9._-]+$").WithMessage("Sistem adı sadece harf, rakam ve ._- karakterlerini içerebilir.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama 500 karakterden uzun olamaz.");

            RuleFor(x => x.Category)
                .MaximumLength(100).WithMessage("Kategori 100 karakterden uzun olamaz.");
        }
    }
} 