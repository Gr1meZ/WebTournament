using AutoMapper;
using WebTournament.Application.AgeGroup;
using WebTournament.Application.Belt;
using WebTournament.Application.Club;
using WebTournament.Application.Fighter;
using WebTournament.Application.Tournament;
using WebTournament.Application.Trainer;
using WebTournament.Application.WeightCategorie;
using WebTournament.Domain.Extensions;

namespace WebTournament.Application.Configuration.AutoMapper;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<Domain.Objects.AgeGroup.AgeGroup, AgeGroupResponse>();
        
        CreateMap<Domain.Objects.Belt.Belt, BeltResponse>();
        
        CreateMap<Domain.Objects.Club.Club, ClubResponse>();
        
        CreateMap<Domain.Objects.Fighter.Fighter, FighterResponse>()
            .ForMember(x => x.Gender, opt
                => opt.MapFrom(src => src.Gender.MapToString()))
            .ForMember(x => x.BeltShortName, opt
                => opt.MapFrom(src => $"{src.Belt.BeltNumber} {src.Belt.ShortName}"))
            .ForMember(x => x.TournamentName, opt
                => opt.MapFrom(src => src.Tournament.Name))
            .ForMember(x => x.TrainerName, opt
                => opt.MapFrom(src => $"{src.Trainer.Surname} {src.Trainer.Name[0]}.{src.Trainer.Patronymic[0]}"))
            .ForMember(x => x.WeightCategorieName, opt
                => opt.MapFrom(src => $"{src.WeightCategorie.AgeGroup.Name} {src.WeightCategorie.WeightName}"))
            .ForMember(x => x.ClubName, opt
                => opt.MapFrom(src => src.Trainer.Club.Name))
            .ForMember(x => x.WeightNumber, opt
                => opt.MapFrom(src => src.WeightCategorie.MaxWeight))
            .ForMember(x => x.BeltNumber, opt
                => opt.MapFrom(src => src.Belt.BeltNumber));
        
        CreateMap<Domain.Objects.Tournament.Tournament, TournamentResponse>();
        
        CreateMap<Domain.Objects.Trainer.Trainer, TrainerResponse>()
            .ForMember(x => x.ClubName, opt
                => opt.MapFrom(src => src.Club.Name));
        
        CreateMap<Domain.Objects.WeightCategorie.WeightCategorie, WeightCategorieResponse>()
            .ForMember(x => x.Gender, opt
                => opt.MapFrom(src => src.Gender.MapToString()))
            .ForMember(x => x.AgeGroupName, opt
                => opt.MapFrom(src => src.AgeGroup.Name));
    }
}