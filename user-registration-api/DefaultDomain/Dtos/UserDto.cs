namespace user_registration_api.DefaultDomain.Dtos;

public class UserDto
{
    public long? IdUser { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public DateTime? CreatedAt { get; set; }
}