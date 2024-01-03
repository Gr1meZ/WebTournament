using AutoMapper;
using WebTournament.Application.AgeGroup.CreateAgeGroup;
using WebTournament.Application.AgeGroup.GetAgeGroup;
using WebTournament.Application.AgeGroup.UpdateAgeGroup;
using WebTournament.Application.Belt.CreateBelt;
using WebTournament.Application.Belt.UpdateBelt;
using WebTournament.Application.Bracket.GenerateBracket;
using WebTournament.Application.Club.CreateClub;
using WebTournament.Application.Club.UpdateClub;
using WebTournament.Application.DTO;
using WebTournament.Application.Fighter.CreateFighter;
using WebTournament.Application.Fighter.UpdateFighter;
using WebTournament.Application.Tournament.CreateTournament;
using WebTournament.Application.Tournament.UpdateTournament;
using WebTournament.Application.Trainer.CreateTrainer;
using WebTournament.Application.Trainer.UpdateTrainer;
using WebTournament.Application.WeightCategorie.CreateWeightCategorie;
using WebTournament.Application.WeightCategorie.UpdateWeightCategorie;
using WebTournament.Domain.Extensions;

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
        
        CreateMap<TournamentDto, CreateTournamentCommand>();
        CreateMap<TournamentDto, UpdateTournamentCommand>();
        CreateMap<Domain.Objects.Tournament.Tournament, TournamentDto>();
        
        CreateMap<TrainerDto, CreateTrainerCommand>();
        CreateMap<TrainerDto, UpdateTrainerCommand>();
        CreateMap<Domain.Objects.Trainer.Trainer, TrainerDto>()
            .ForMember(x => x.ClubName, opt
                => opt.MapFrom(src => src.Club.Name));
        
        CreateMap<WeightCategorieDto, CreateWeightCategorieCommand>();
        CreateMap<WeightCategorieDto, UpdateWeightCategorieCommand>();
        CreateMap<Domain.Objects.WeightCategorie.WeightCategorie, WeightCategorieDto>()
            .ForMember(x => x.Gender, opt
                => opt.MapFrom(src => src.Gender.MapToString()))
            .ForMember(x => x.AgeGroupName, opt
                => opt.MapFrom(src => src.AgeGroup.Name));
        
        CreateMap<FighterDto, CreateFighterCommand>();
        CreateMap<FighterDto, UpdateFighterCommand>();
        CreateMap<Domain.Objects.Fighter.Fighter, FighterDto>()
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
                => opt.MapFrom(src => src.Trainer.Club.Name));

        CreateMap<BracketDto, GenerateBracketCommand>();


    }
}