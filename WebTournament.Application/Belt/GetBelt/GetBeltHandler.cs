using AutoMapper;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Domain.Objects.Belt;

namespace WebTournament.Application.Belt.GetBelt;

public class GetBeltHandler : IQueryHandler<GetBeltQuery, BeltResponse>
{
    private readonly IBeltRepository _beltRepository;
    private readonly IMapper _mapper;
    public GetBeltHandler(IBeltRepository beltRepository, IMapper mapper)
    {
        _beltRepository = beltRepository;
        _mapper = mapper;
    }

    public async Task<BeltResponse> Handle(GetBeltQuery request, CancellationToken cancellationToken)
    {
        var belt = await _beltRepository.GetByIdAsync(request.Id);
        return _mapper.Map<BeltResponse>(belt);
    }
}