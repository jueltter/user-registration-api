using user_registration_api.DefaultDomain.Repositories;
using FluentValidation;

namespace user_registration_api.DefaultDomain.Models.Validators;

public class UserIdValidator : AbstractValidator<UserId>
{
    public UserIdValidator(IUserRepository repository)
    {
        RuleFor(x => x.IdUser)
            .MustAsync(async (property, _) =>
            {
                if (!property.HasValue)
                {
                    return false;
                }

                return (await repository.CountAsync(UserSearch.From(property))) != 0;
            })
            .WithMessage("El campo IdUser es nulo o no corresponde a ningún registro.");
    }
}