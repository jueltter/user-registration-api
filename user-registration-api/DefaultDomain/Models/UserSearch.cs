using EscuelaPolitecnicaNacional.DgipCommonsLib.Models;

namespace user_registration_api.DefaultDomain.Models;

public class UserSearch : AbstractSearch
{
    public long? IdUser { get; set; }

    public static UserSearch From(long? idUser)
    {
        return new UserSearch
        {
            IdUser = idUser
        };
    }
}