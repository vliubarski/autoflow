using AutoFlow.Models;
using AutoMapper;

namespace AutoFlow.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ContactModelUi, ContactModelDb>();
        CreateMap<ContactModelDb, ContactModelUi>();
    }
}
