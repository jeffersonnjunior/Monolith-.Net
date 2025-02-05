using AutoMapper;

namespace Application.Profiles;

public class AutoMapperConfig
{
    public static MapperConfiguration RegisterMappings()
    {
        return new MapperConfiguration(cfg => { cfg.AddProfile(new ProfileConfigurationMap()); });
    }
}
