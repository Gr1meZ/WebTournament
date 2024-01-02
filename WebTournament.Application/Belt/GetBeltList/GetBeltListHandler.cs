using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Common;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;
using WebTournament.Domain.Objects.Belt;

namespace WebTournament.Application.Belt.GetBeltList;

public class GetBeltListHandler : IQueryHandler<GetBeltListQuery, PagedResponse<BeltDto[]>>
{
    private readonly IBeltRepository _beltRepository;
    private readonly IMapper _mapper;

    public GetBeltListHandler(IBeltRepository beltRepository, IMapper mapper)
    {
        _beltRepository = beltRepository;
        _mapper = mapper;
    }

    public async Task<PagedResponse<BeltDto[]>> Handle(GetBeltListQuery request, CancellationToken cancellationToken)
    {
        var beltQuery = _beltRepository.GetAll();
        
         var lowerQ = request.Search.ToLower();
            if (!string.IsNullOrWhiteSpace(lowerQ))
            {
                beltQuery = lowerQ.Split(' ').Aggregate(beltQuery, (current, searchWord) =>
                    current.Where(f =>
                        f.ShortName.ToLower().Contains(searchWord.ToLower()) ||
                        f.FullName.ToLower().Contains(searchWord.ToLower()) ||
                        f.BeltNumber.ToString().ToLower().Contains(searchWord.ToLower())
                    ));
            }

            if (!string.IsNullOrWhiteSpace(request.OrderColumn) && !string.IsNullOrWhiteSpace(request.OrderDir))
            {
                beltQuery = request.OrderColumn switch
                {
                    "shortName" => (request.OrderDir.Equals("asc"))
                        ? beltQuery.OrderBy(o => o.ShortName)
                        : beltQuery.OrderByDescending(o => o.ShortName),
                    "fullName" => (request.OrderDir.Equals("asc"))
                    ? beltQuery.OrderBy(o => o.FullName)
                    : beltQuery.OrderByDescending(o => o.FullName),
                    "beltNumber" => (request.OrderDir.Equals("asc"))
                        ? beltQuery.OrderBy(o => o.BeltNumber)
                        : beltQuery.OrderByDescending(o => o.BeltNumber),
                    _ => (request.OrderDir.Equals("asc"))
                        ? beltQuery.OrderBy(o => o.Id)
                        : beltQuery.OrderByDescending(o => o.Id)
                };
            }

            var totalItemCount = beltQuery.Count();

            beltQuery = beltQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            var dbItems = await beltQuery
                .Select(x => _mapper.Map<BeltDto>(x))
                .ToArrayAsync(cancellationToken: cancellationToken);

            return new PagedResponse<BeltDto[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
    }
}