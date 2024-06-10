using AutoMapper;
using project_get_discount_back._1_Domain.Dtos;
using project_get_discount_back._1_Domain.Queries;
using project_get_discount_back.Queries;

namespace project_get_discount_back._1_Domain.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LoginDto, GetLoginQuery>()
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.password, opt => opt.MapFrom(src => src.Password));

            CreateMap<RegisterUserDto, RegisterUserQuery>()
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.role, opt => opt.MapFrom(src => src.Role));

            CreateMap<RegisterPasswordDto, RegisterPasswordQuery>()
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.password, opt => opt.MapFrom(src => src.Password));

            CreateMap<TokenDto, GetTokenQuery>()
                .ForMember(dest => dest.refreshToken, opt => opt.MapFrom(src => src.RefreshToken))
                .ForMember(dest => dest.device, opt => opt.MapFrom(src => src.Device));
        }
    }
}
