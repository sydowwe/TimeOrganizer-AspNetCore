using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request.user;
using TimeOrganizer_net_core.model.DTO.response.user;
using TimeOrganizer_net_core.model.entity;

namespace TimeOrganizer_net_core.model.DTO.mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserRequest, User>();
        CreateMap<RegistrationRequest, User>();
        CreateMap<LoginRequest, User>();
        
        CreateMap<User, TwoFactorAuthResponse>();
        CreateMap<User, LoginResponse>();
        CreateMap<User, Oauth2LoginResponse>();
        CreateMap<User, UserResponse>();
        CreateMap<User, EditedUserResponse>();
    }
}