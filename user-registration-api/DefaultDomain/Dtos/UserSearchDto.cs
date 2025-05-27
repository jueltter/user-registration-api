using EscuelaPolitecnicaNacional.DgipCommonsLib.Models;

namespace user_registration_api.DefaultDomain.Dtos;

public class UserSearchDto : AbstractSearch
{
    public long? IdUser { get; set; }
}