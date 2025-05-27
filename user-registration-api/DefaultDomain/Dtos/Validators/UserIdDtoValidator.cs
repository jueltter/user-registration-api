using FluentValidation;

namespace user_registration_api.DefaultDomain.Dtos.Validators;

public class UserIdDtoValidator : AbstractValidator<UserIdDto>
{
    public UserIdDtoValidator()
    {
        RuleFor(x => x.IdUser)
            .NotNull()
            .WithMessage("El campo IdUser es obligatorio.");
    }
}