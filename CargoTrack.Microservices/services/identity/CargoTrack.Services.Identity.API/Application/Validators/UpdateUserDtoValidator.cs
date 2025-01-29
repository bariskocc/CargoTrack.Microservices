using CargoTrack.Services.Identity.API.Application.DTOs;
using FluentValidation;

namespace CargoTrack.Services.Identity.API.Application.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad boş olamaz.")
                .MaximumLength(50).WithMessage("Ad 50 karakterden uzun olamaz.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad boş olamaz.")
                .MaximumLength(50).WithMessage("Soyad 50 karakterden uzun olamaz.");

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Firma adı boş olamaz.")
                .MaximumLength(100).WithMessage("Firma adı 100 karakterden uzun olamaz.");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithMessage("Telefon numarası 20 karakterden uzun olamaz.")
                .Matches("^[0-9+()-]*$").WithMessage("Geçerli bir telefon numarası giriniz.");
        }
    }
} 