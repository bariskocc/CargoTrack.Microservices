using CargoTrack.Services.Identity.API.Application.DTOs;
using FluentValidation;

namespace CargoTrack.Services.Identity.API.Application.Validators
{
    public class UpdateUserRolesDtoValidator : AbstractValidator<UpdateUserRolesDto>
    {
        public UpdateUserRolesDtoValidator()
        {
            RuleFor(x => x.Roles)
                .NotNull().WithMessage("Roller boş olamaz.");
        }
    }
} 