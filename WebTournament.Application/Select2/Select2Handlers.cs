using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.Select2.Queries;
using WebTournament.Domain.Objects.AgeGroup;
using WebTournament.Domain.Objects.Belt;
using WebTournament.Domain.Objects.Club;
using WebTournament.Domain.Objects.Fighter;
using WebTournament.Domain.Objects.Trainer;
using WebTournament.Domain.Objects.WeightCategorie;

namespace WebTournament.Application.Select2;

public class Select2Handlers : IQueryHandler<Select2AgeGroupsQuery, Select2Response>,
    IQueryHandler<Select2BeltQuery, Select2Response>,
    IQueryHandler<Select2ClubsQuery, Select2Response>,
    IQueryHandler<Select2TrainersQuery, Select2Response>,
    IQueryHandler<Select2WeightCategorieQuery, Select2Response>,
    IQueryHandler<Select2FightersQuery, Select2Response>
{
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly IBeltRepository _beltRepository;
    private readonly IClubRepository _clubRepository;
    private readonly ITrainerRepository _trainerRepository;
    private readonly IWeightCategorieRepository _weightCategorieRepository;
    private readonly IFighterRepository _fighterRepository;
    public Select2Handlers(IAgeGroupRepository ageGroupRepository, IBeltRepository beltRepository, IClubRepository clubRepository, ITrainerRepository trainerRepository, IWeightCategorieRepository weightCategorieRepository, IFighterRepository fighterRepository)
    {
        _ageGroupRepository = ageGroupRepository;
        _beltRepository = beltRepository;
        _clubRepository = clubRepository;
        _trainerRepository = trainerRepository;
        _weightCategorieRepository = weightCategorieRepository;
        _fighterRepository = fighterRepository;
    }

    public async Task<Select2Response> Handle(Select2AgeGroupsQuery request, CancellationToken cancellationToken)
    {
        var ageGroups = _ageGroupRepository.GetAll();
        
        return await request.GetPagedResult(ageGroups, cancellationToken);
    }

    public async Task<Select2Response> Handle(Select2BeltQuery request, CancellationToken cancellationToken)
    {
        var belts = _beltRepository.GetAll();

        return await request.GetPagedResult(belts, cancellationToken);
    }

    public async Task<Select2Response> Handle(Select2ClubsQuery request, CancellationToken cancellationToken)
    {
        var clubs = _clubRepository.GetAll();

        return await request.GetPagedResult(clubs, cancellationToken);
    }

    public async Task<Select2Response> Handle(Select2TrainersQuery request, CancellationToken cancellationToken)
    {
        var trainers = _trainerRepository.GetAll();
        
        return await request.GetPagedResult(trainers, cancellationToken);
    }

    public async Task<Select2Response> Handle(Select2WeightCategorieQuery request, CancellationToken cancellationToken)
    {
        var weightCategories = _weightCategorieRepository.GetAll();
        
        return await request.GetPagedResult(weightCategories, cancellationToken);
    }

    public async Task<Select2Response> Handle(Select2FightersQuery request, CancellationToken cancellationToken)
    {
        var fighters = _fighterRepository.GetAll().Where(x => x.BracketId == request.Id);
       
        return await request.GetPagedResult(fighters, cancellationToken);
    }
}