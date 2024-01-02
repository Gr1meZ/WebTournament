using AutoMapper;
using WebTournament.Application.AgeGroup.CreateAgeGroup;
using WebTournament.Application.AgeGroup.GetAgeGroup;
using WebTournament.Application.AgeGroup.UpdateAgeGroup;
using WebTournament.Application.Belt.CreateBelt;
using WebTournament.Application.Belt.UpdateBelt;
using WebTournament.Application.Club.CreateClub;
using WebTournament.Application.Club.UpdateClub;
using WebTournament.Application.DTO;

namespace WebTournament.Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AgeGroupDto, CreateAgeGroupCommand>();
        CreateMap<AgeGroupDto, UpdateAgeGroupCommand>();
        CreateMap<Domain.Objects.AgeGroup.AgeGroup, AgeGroupDto>();
        
        CreateMap<BeltDto, CreateBeltCommand>();
        CreateMap<BeltDto, UpdateBeltCommand>();
        CreateMap<Domain.Objects.Belt.Belt, BeltDto>();
        
        CreateMap<ClubDto, CreateClubCommand>();
        CreateMap<ClubDto, UpdateClubCommand>();
        CreateMap<Domain.Objects.Club.Club, ClubDto>();
    }
}