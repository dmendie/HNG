using HNG.Abstractions.Enums;
using HNG.Abstractions.Helpers;

namespace HNG.Mappers.Profiles
{
    public class OrganisationProfile : AutoMapper.Profile
    {
        public override string ProfileName => nameof(OrganisationProfile);

        public OrganisationProfile()
        {
            CreateMap<Abstractions.Models.Organisation, Abstractions.Contracts.OrganisationDTO>();
            CreateMap<Abstractions.Contracts.OrganisationDTO, Abstractions.Contracts.ResponseDataDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ResponseStatusType.Success.GetDescription()))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => "Organisation request successful"))
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src));
            CreateMap<Abstractions.Contracts.OrganisationListDTO, Abstractions.Contracts.ResponseDataDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ResponseStatusType.Success.GetDescription()))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => "Organisation request successful"))
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src));

        }
    }
}
