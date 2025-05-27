using FluentValidation;

namespace user_registration_api.DefaultDomain.Dtos.Validators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
    }
}