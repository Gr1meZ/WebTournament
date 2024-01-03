using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Common;
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

        var dbQuery = ageGroups;
        var total = await ageGroups.CountAsync(cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            dbQuery = dbQuery.Where(x => x.Name.ToLower().Contains(request.Search.ToLower()));
        }

        if (request.PageSize != -1)
            dbQuery = dbQuery.Skip(request.Skip).Take(request.PageSize);

        var data = await dbQuery.Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToArrayAsync(cancellationToken: cancellationToken);

        return new Select2Response()
        {
            Data = data,
            Total = total
        };
    }

    public async Task<Select2Response> Handle(Select2BeltQuery request, CancellationToken cancellationToken)
    {
        var belts = _beltRepository.GetAll();

        var dbQuery = belts;
        var total = await belts.CountAsync(cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            dbQuery = dbQuery.Where(x => x.ShortName.ToLower().Contains(request.Search.ToLower()) || x.BeltNumber.ToString().Contains(request.Search.ToLower()));
        }

        if (request.PageSize != -1)
            dbQuery = dbQuery.Skip(request.Skip).Take(request.PageSize);

        var data = await dbQuery.Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = $"{x.BeltNumber} {x.ShortName}"
            })
            .ToArrayAsync(cancellationToken: cancellationToken);

        return new Select2Response()
        {
            Data = data,
            Total = total
        };
    }

    public async Task<Select2Response> Handle(Select2ClubsQuery request, CancellationToken cancellationToken)
    {
        var clubs = _clubRepository.GetAll();

        var dbQuery = clubs;
        var total = await clubs.CountAsync(cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            dbQuery = dbQuery.Where(x => x.Name.ToLower().Contains(request.Search.ToLower()));
        }

        if (request.PageSize != -1)
            dbQuery = dbQuery.Skip(request.Skip).Take(request.PageSize);

        var data = dbQuery.Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToArray();

        return new Select2Response()
        {
            Data = data,
            Total = total
        };
    }

    public async Task<Select2Response> Handle(Select2TrainersQuery request, CancellationToken cancellationToken)
    {
        var ageGroups = _trainerRepository.GetAll();

        var dbQuery = ageGroups;
        var total = await ageGroups.CountAsync(cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            dbQuery = dbQuery.Where(x => x.Name.ToLower().Contains(request.Search.ToLower()) ||
                                         x.Surname.ToLower().Contains(request.Search.ToLower()) ||
                                         x.Patronymic.ToLower().Contains(request.Search.ToLower()));
        }

        if (request.PageSize != -1)
            dbQuery = dbQuery.Skip(request.Skip).Take(request.PageSize);

        var data = dbQuery.Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = $"{x.Surname} {x.Name[0]}.{x.Patronymic}"
            })
            .ToArray();

        return new Select2Response()
        {
            Data = data,
            Total = total
        };
    }

    public async Task<Select2Response> Handle(Select2WeightCategorieQuery request, CancellationToken cancellationToken)
    {
        var weightCategoriesQuery = _weightCategorieRepository.GetAll();
        var total = await weightCategoriesQuery.CountAsync(cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            weightCategoriesQuery = weightCategoriesQuery.Where(x => x.WeightName.ToLower().Contains(request.Search.ToLower()));
        }

        if (request.PageSize != -1)
            weightCategoriesQuery = weightCategoriesQuery.Skip(request.Skip).Take(request.PageSize);

        var data = weightCategoriesQuery.Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = x.WeightName
            })
            .ToArray();

        return new Select2Response()
        {
            Data = data,
            Total = total
        };
    }

    public async Task<Select2Response> Handle(Select2FightersQuery request, CancellationToken cancellationToken)
    {
        var fightersQuery = _fighterRepository.GetAll().Where(x => x.BracketId == request.Id);
        var total = await fightersQuery.CountAsync(cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            fightersQuery = fightersQuery.Where(x => x.Surname.ToLower().Contains(request.Search.ToLower()));
        }

        if (request.PageSize != -1)
            fightersQuery = fightersQuery.Skip(request.Skip).Take(request.PageSize);

        var data = fightersQuery.Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = $"{x.Surname}"
            })
            .ToArray();

        return new Select2Response()
        {
            Data = data,
            Total = total
        };

    }
}