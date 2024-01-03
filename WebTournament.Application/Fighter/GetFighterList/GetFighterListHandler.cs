using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Common;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;
using WebTournament.Domain.Objects.Fighter;

namespace WebTournament.Application.Fighter.GetFighterList;

public class GetFighterListHandler : IQueryHandler<GetFighterListQuery, PagedResponse<FighterDto[]>>
{
    private readonly IFighterRepository _fighterRepository;
    private readonly IMapper _mapper;

    public GetFighterListHandler(IFighterRepository fighterRepository, IMapper mapper)
    {
        _fighterRepository = fighterRepository;
        _mapper = mapper;
    }

    public async Task<PagedResponse<FighterDto[]>> Handle(GetFighterListQuery request, CancellationToken cancellationToken)
    {
        var fightersQuery = _fighterRepository.GetAll(request.TournamentId);
        var lowerQ = request.Search?.ToLower();
            if (!string.IsNullOrWhiteSpace(lowerQ))
            {
                fightersQuery = (lowerQ.Split(' ')).Aggregate(fightersQuery, (current, searchWord) =>
                    current.Where(f =>
                        f.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.Age.ToString().ToLower().Contains(searchWord.ToLower()) ||
                        f.BirthDate.ToString().ToLower().Contains(searchWord.ToLower()) ||
                        f.City.ToLower().Contains(searchWord.ToLower()) ||
                        f.Surname.ToLower().Contains(searchWord.ToLower()) ||
                        f.Country.ToLower().Contains(searchWord.ToLower()) ||
                        f.Belt.ShortName.ToLower().Contains(searchWord.ToLower()) ||
                        f.Belt.BeltNumber.ToString().ToLower().Contains(searchWord.ToLower()) ||
                        f.Trainer.Surname.ToLower().Contains(searchWord.ToLower()) ||
                        f.WeightCategorie.WeightName.ToLower().Contains(searchWord.ToLower()) ||
                        f.WeightCategorie.AgeGroup.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.Trainer.Club.Name.ToLower().Contains(searchWord.ToLower())
                    ));
            }

            if (!string.IsNullOrWhiteSpace(request.OrderColumn) && !string.IsNullOrWhiteSpace(request.OrderDir))
            {
                fightersQuery = request.OrderColumn switch
                {
                    "name" => (request.OrderDir.Equals("asc"))
                        ? fightersQuery.OrderBy(o => o.Name)
                        : fightersQuery.OrderByDescending(o => o.Name),
                    "age" => (request.OrderDir.Equals("asc"))
                    ? fightersQuery.OrderBy(o => o.Age)
                    : fightersQuery.OrderByDescending(o => o.Age),
                    "birthDate" => (request.OrderDir.Equals("asc"))
                    ? fightersQuery.OrderBy(o => o.BirthDate)
                    : fightersQuery.OrderByDescending(o => o.BirthDate),
                    "city" => (request.OrderDir.Equals("asc"))
                        ? fightersQuery.OrderBy(o => o.City)
                        : fightersQuery.OrderByDescending(o => o.City),
                    "surname" => (request.OrderDir.Equals("asc"))
                    ? fightersQuery.OrderBy(o => o.Surname)
                    : fightersQuery.OrderByDescending(o => o.Surname),
                    "country" => (request.OrderDir.Equals("asc"))
                    ? fightersQuery.OrderBy(o => o.Country)
                    : fightersQuery.OrderByDescending(o => o.Country),
                    "gender" => (request.OrderDir.Equals("asc"))
                    ? fightersQuery.OrderBy(o => o.Gender)
                    : fightersQuery.OrderByDescending(o => o.Gender),
                    "beltShortName" => (request.OrderDir.Equals("asc"))
                    ? fightersQuery.OrderBy(o => o.Belt.ShortName)
                    : fightersQuery.OrderByDescending(o => o.Belt.ShortName),
                    "trainerName" => (request.OrderDir.Equals("asc"))
                    ? fightersQuery.OrderBy(o => o.Trainer.Surname)
                    : fightersQuery.OrderByDescending(o => o.Trainer.Surname),
                    "weightCategorieName" => (request.OrderDir.Equals("asc"))
                    ? fightersQuery.OrderBy(o => o.WeightCategorie.WeightName)
                    : fightersQuery.OrderByDescending(o => o.WeightCategorie.WeightName),
                    "clubName" => (request.OrderDir.Equals("asc"))
                   ? fightersQuery.OrderBy(o => o.Trainer.Club.Name)
                   : fightersQuery.OrderByDescending(o => o.Trainer.Club.Name),
                    _ => (request.OrderDir.Equals("asc"))
                        ? fightersQuery.OrderBy(o => o.Id)
                        : fightersQuery.OrderByDescending(o => o.Id)
                };
            }

            var totalItemCount =  await fightersQuery.CountAsync(cancellationToken: cancellationToken);

            fightersQuery = fightersQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            var dbItems = await fightersQuery.Select(x => _mapper.Map<FighterDto>(x)).ToArrayAsync(cancellationToken: cancellationToken);

            return new PagedResponse<FighterDto[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
    }
}