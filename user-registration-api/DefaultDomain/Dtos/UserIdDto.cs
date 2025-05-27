namespace user_registration_api.DefaultDomain.Dtos;

public class UserIdDto
{
    public long? IdUser { get; set; }

    public static UserIdDto From(long? idUser)
    {
        return new UserIdDto
        {
            IdUser = idUser
        };
    }
}