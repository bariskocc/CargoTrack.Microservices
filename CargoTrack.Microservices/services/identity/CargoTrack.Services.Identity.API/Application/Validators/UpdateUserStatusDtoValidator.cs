using CargoTrack.Services.Identity.API.Application.DTOs;
using FluentValidation;

namespace CargoTrack.Services.Identity.API.Application.Validators
{
    public class UpdateUserStatusDtoValidator : AbstractValidator<UpdateUserStatusDto>
    {
        public UpdateUserStatusDtoValidator()
        {
            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("Aktiflik durumu boş olamaz.");
        }
    }
} 