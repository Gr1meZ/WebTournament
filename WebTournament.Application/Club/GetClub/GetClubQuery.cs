using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;

namespace WebTournament.Application.Club.GetClub;

public class GetClubQuery : IQuery<ClubDto>
{
    public Guid Id { get; private set; }

    public GetClubQuery(Guid id)
    {
        Id = id;
    }
}