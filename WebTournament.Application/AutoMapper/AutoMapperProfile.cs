using AutoMapper;
using WebTournament.Application.DTO;

namespace WebTournament.Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Domain.Objects.AgeGroup.AgeGroup, AgeGroupDto>().ReverseMap();
    }
}