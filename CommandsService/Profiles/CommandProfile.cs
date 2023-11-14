using AutoMapper;

public class CommandProfile: Profile
{
    public CommandProfile()
    {
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Command, CommandReadDto>();
        CreateMap<PlatformPublishedDto, Platform>()
                .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.Id));
        CreateMap<PlatformReadDto, PlatformPublishedDto>();




    }
}