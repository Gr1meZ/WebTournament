using AutoMapper;
using WebTournament.Application.AgeGroup;
using WebTournament.Application.AgeGroup.CreateAgeGroup;
using WebTournament.Application.AgeGroup.UpdateAgeGroup;
using WebTournament.Application.Belt;
using WebTournament.Application.Belt.CreateBelt;
using WebTournament.Application.Belt.UpdateBelt;
using WebTournament.Application.Bracket;
using WebTournament.Application.Bracket.GenerateBracket;
using WebTournament.Application.Club;
using WebTournament.Application.Club.CreateClub;
using WebTournament.Application.Club.UpdateClub;
using WebTournament.Application.Fighter;
using WebTournament.Application.Fighter.CreateFighter;
using WebTournament.Application.Fighter.UpdateFighter;
using WebTournament.Application.Tournament;
using WebTournament.Application.Tournament.CreateTournament;
using WebTournament.Application.Tournament.UpdateTournament;
using WebTournament.Application.Trainer;
using WebTournament.Application.Trainer.CreateTrainer;
using WebTournament.Application.Trainer.UpdateTrainer;
using WebTournament.Application.WeightCategorie;
using WebTournament.Application.WeightCategorie.CreateWeightCategorie;
using WebTournament.Application.WeightCategorie.UpdateWeightCategorie;
using WebTournament.Domain.Extensions;
using WebTournament.Presentation.MVC.ViewModels;

namespace WebTournament.Presentation.MVC.AutoMapper;

public class PresentationProfile : Profile
{
    public PresentationProfile()
    {
        CreateMap<AgeGroupViewModel, CreateAgeGroupCommand>();
        CreateMap<AgeGroupViewModel, UpdateAgeGroupCommand>();
        CreateMap<AgeGroupResponse, AgeGroupViewModel>();
        
        CreateMap<BeltViewModel, CreateBeltCommand>();
        CreateMap<BeltViewModel, UpdateBeltCommand>();
        CreateMap<BeltResponse, BeltViewModel>();
        
        CreateMap<BracketStateResponse, BracketStateViewModel>();
        CreateMap<BracketStateViewModel, BracketStateRequest>();
        CreateMap<BracketViewModel, GenerateBracketCommand>();
        CreateMap<BracketWinnerResponse, BracketWinnerViewModel>();
        
        CreateMap<ClubViewModel, CreateClubCommand>();
        CreateMap<ClubViewModel, UpdateClubCommand>();
        CreateMap<ClubResponse, ClubViewModel>();
        
        CreateMap<FighterViewModel, CreateFighterCommand>();
        CreateMap<FighterViewModel, UpdateFighterCommand>();
        CreateMap<FighterResponse, FighterViewModel>();
        
        CreateMap<TournamentViewModel, CreateTournamentCommand>();
        CreateMap<TournamentViewModel, UpdateTournamentCommand>();
        CreateMap<TournamentResponse, TournamentViewModel>();

        CreateMap<TrainerViewModel, CreateTrainerCommand>();
        CreateMap<TrainerViewModel, UpdateTrainerCommand>();
        CreateMap<TrainerResponse, TrainerViewModel>();
        
        CreateMap<WeightCategorieViewModel, CreateWeightCategorieCommand>();
        CreateMap<WeightCategorieViewModel, UpdateWeightCategorieCommand>();
        CreateMap<WeightCategorieResponse, WeightCategorieViewModel>();
        
    }
}