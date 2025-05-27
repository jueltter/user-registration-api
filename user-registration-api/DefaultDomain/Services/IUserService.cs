using user_registration_api.DefaultDomain.Models;
using EscuelaPolitecnicaNacional.DgipCommonsLib.Models;
using LanguageExt;

namespace user_registration_api.DefaultDomain.Services;

public interface IUserService
{
    Task<Page<User>> SearchAsync(UserSearch? search);

    Task<Option<User>> FindByIdAsync(long id);

    Task<User> SaveAsync(User model);

    Task<Option<User>> UpdateAsync(User model);

    Task<Option<User>> DeleteAsync(long id);
}