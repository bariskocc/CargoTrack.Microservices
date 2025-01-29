using CargoTrack.Services.Identity.API.Application.DTOs;
using FluentValidation;

namespace CargoTrack.Services.Identity.API.Application.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email adresi boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
                .MaximumLength(100).WithMessage("Email adresi 100 karakterden uzun olamaz.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz.")
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Kullanıcı adı 50 karakterden uzun olamaz.")
                .Matches("^[a-zA-Z0-9._-]+$").WithMessage("Kullanıcı adı sadece harf, rakam ve ._- karakterlerini içerebilir.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz.")
                .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Şifre 50 karakterden uzun olamaz.")
                .Matches("[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
                .Matches("[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
                .Matches("[0-9]").WithMessage("Şifre en az bir rakam içermelidir.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Şifre en az bir özel karakter içermelidir.");

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

            RuleFor(x => x.Roles)
                .NotNull().WithMessage("Roller boş olamaz.");
        }
    }
} 