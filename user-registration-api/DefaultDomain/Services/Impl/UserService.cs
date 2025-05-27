using user_registration_api.DefaultDomain.Models;
using user_registration_api.DefaultDomain.Repositories;
using EscuelaPolitecnicaNacional.DgipCommonsLib.Models;
using EscuelaPolitecnicaNacional.DgipCommonsLib.Models.Validators;
using FluentValidation;
using LanguageExt;
using InvalidDataException = EscuelaPolitecnicaNacional.DgipCommonsLib.Exceptions.InvalidDataException;

namespace user_registration_api.DefaultDomain.Services.Impl;

public class UserService(
    IUserRepository repository,
    IValidator<User> validator,
    IValidator<UserId> idValidator) : IUserService
{
    public async Task<Page<User>> SearchAsync(UserSearch? search)
    {
        return await repository.SearchAsync(search);
    }

    public async Task<Option<User>> FindByIdAsync(long id)
    {
        return await repository.FindByIdAsync(id);
    }

    public async Task<User> SaveAsync(User model)
    {
        var validationResult = await validator.ValidateAsync(model);
        if (!validationResult.IsValid) throw InvalidDataException.GetInstance(validationResult.Errors);
        return await repository.SaveAsync(model);
    }

    public async Task<Option<User>> UpdateAsync(User model)
    {
        var multipleValidator = new ModelMultipleValidator();
        multipleValidator.AddValidator(validator, model);
        multipleValidator.AddValidator(idValidator, UserId.From(model.IdUser));
        await multipleValidator.ExecuteValidatorsAsync();
        var validationResult = multipleValidator.GetResult();
        var idInvalid = !multipleValidator.GetIndividualResult(typeof(UserId)).IsValid;
        var modelInvalid = !multipleValidator.GetIndividualResult(typeof(User)).IsValid;
        if (model.IdUser.HasValue && idInvalid && !modelInvalid) return Option<User>.None;
        if (idInvalid || modelInvalid) throw InvalidDataException.GetInstance(validationResult.Errors);

        return await repository.UpdateAsync(model);
    }

    public async Task<Option<User>> DeleteAsync(long id)
    {
        var validationResult = await idValidator.ValidateAsync(UserId.From(id));
        if (!validationResult.IsValid) return Option<User>.None;
        return await repository.DeleteAsync(id);
    }
}