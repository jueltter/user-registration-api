namespace user_registration_api.DefaultDomain.Models;

public class UserId
{
    public long? IdUser { get; set; }

    public static UserId From(long? idUser)
    {
        return new UserId
        {
            IdUser = idUser
        };
    }
}