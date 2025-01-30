using CargoTrack.Services.Identity.API.Application.DTOs;
using FluentValidation;

namespace CargoTrack.Services.Identity.API.Application.Validators
{
    public class RolePermissionsUpdateDtoValidator : AbstractValidator<RolePermissionsUpdateDto>
    {
        public RolePermissionsUpdateDtoValidator()
        {
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Rol ID boş olamaz.");

            RuleFor(x => x.Permissions)
                .NotNull().WithMessage("İzinler boş olamaz.");
        }
    }
} 