using FluentValidation;

namespace user_registration_api.DefaultDomain.Models.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
    }
}