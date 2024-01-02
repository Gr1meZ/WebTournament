using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Common;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;
using WebTournament.Domain.Objects.Trainer;

namespace WebTournament.Application.Trainer.GetTrainerList;

public class GetTrainerListHandler : IQueryHandler<GetTrainerListQuery, PagedResponse<TrainerDto[]>>
{
    private readonly ITrainerRepository _trainerRepository;
    private readonly IMapper _mapper;
    public GetTrainerListHandler(ITrainerRepository trainerRepository, IMapper mapper)
    {
        _trainerRepository = trainerRepository;
        _mapper = mapper;
    }

    public async Task<PagedResponse<TrainerDto[]>> Handle(GetTrainerListQuery request, CancellationToken cancellationToken)
    {
        var trainersQuery = _trainerRepository.GetAll();

            var lowerQ = request.Search?.ToLower();
            if (!string.IsNullOrWhiteSpace(lowerQ))
            {
                trainersQuery = (lowerQ.Split(' ')).Aggregate(trainersQuery, (current, searchWord) =>
                    current.Where(f =>
                        f.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.Surname.ToLower().Contains(searchWord.ToLower()) ||
                        f.Patronymic.ToLower().Contains(searchWord.ToLower()) ||
                        f.Phone.ToLower().Contains(searchWord.ToLower()) ||
                        f.Club.Name.ToLower().Contains(searchWord.ToLower())
                    ));
            }

            if (!string.IsNullOrWhiteSpace(request.OrderColumn) && !string.IsNullOrWhiteSpace(request.OrderDir))
            {
                trainersQuery = request.OrderColumn switch
                {
                    "name" => (request.OrderDir.Equals("asc"))
                        ? trainersQuery.OrderBy(o => o.Name)
                        : trainersQuery.OrderByDescending(o => o.Name),
                    "surname" => (request.OrderDir.Equals("asc"))
                    ? trainersQuery.OrderBy(o => o.Surname)
                    : trainersQuery.OrderByDescending(o => o.Surname),
                    "patronymic" => (request.OrderDir.Equals("asc"))
                        ? trainersQuery.OrderBy(o => o.Patronymic)
                        : trainersQuery.OrderByDescending(o => o.Patronymic),
                    "phone" => (request.OrderDir.Equals("asc"))
                    ? trainersQuery.OrderBy(o => o.Phone)
                    : trainersQuery.OrderByDescending(o => o.Phone),
                    _ => (request.OrderDir.Equals("asc"))
                        ? trainersQuery.OrderBy(o => o.Id)
                        : trainersQuery.OrderByDescending(o => o.Id)
                };
            }

            var totalItemCount = await trainersQuery.CountAsync(cancellationToken: cancellationToken);

            trainersQuery = trainersQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            var dbItems = await trainersQuery
                .Select(x => _mapper.Map<TrainerDto>(x))
                .ToArrayAsync(cancellationToken: cancellationToken);

            return new PagedResponse<TrainerDto[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
    }
}