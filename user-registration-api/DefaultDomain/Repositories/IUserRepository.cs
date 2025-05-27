using user_registration_api.DefaultDomain.Models;
using EscuelaPolitecnicaNacional.DgipCommonsLib.Models;
using LanguageExt;

namespace user_registration_api.DefaultDomain.Repositories;

public interface IUserRepository
{
    Task<Page<User>> SearchAsync(UserSearch? search);

    Task<Option<User>> FindByIdAsync(long id);

    Task<User> SaveAsync(User model);

    Task<User> UpdateAsync(User model);

    Task<User> DeleteAsync(long id);

    Task<int> CountAsync(UserSearch? search);
}