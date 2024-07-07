using HNG.Abstractions.Enums;
using HNG.Abstractions.Helpers;

namespace HNG.Mappers.Profiles
{
    public class UserProfile : AutoMapper.Profile
    {
        public override string ProfileName => nameof(UserProfile);

        public UserProfile()
        {
            //me - model -> contract
            CreateMap<Abstractions.Models.User, Abstractions.Contracts.UserDTO>();
            CreateMap<Abstractions.Contracts.UserDTO, Abstractions.Contracts.ResponseDataDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ResponseStatusType.Success.GetDescription()))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => "User request successful"))
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src));
            CreateMap<Abstractions.Contracts.UserTokenDTO, Abstractions.Contracts.ResponseDataDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ResponseStatusType.Success.GetDescription()))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => "Registration successful"))
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src));
        }
    }
}
