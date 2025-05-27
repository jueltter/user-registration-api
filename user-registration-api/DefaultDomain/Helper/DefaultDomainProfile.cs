using AutoMapper;
using user_registration_api.DefaultDomain.Dtos;
using user_registration_api.DefaultDomain.Models;

namespace user_registration_api.DefaultDomain.Helper;

public class DefaultDomainProfile: Profile
{
    public DefaultDomainProfile()
    {
        CreateMap<User, User>();
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<UserSearch, UserSearchDto>();
        CreateMap<UserSearchDto, UserSearch>();
    }
}