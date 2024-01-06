using WebTournament.Application.Configuration.Queries;

namespace WebTournament.Application.Club.GetClub;

public class GetClubQuery : IQuery<ClubResponse>
{
    public Guid Id { get; private set; }

    public GetClubQuery(Guid id)
    {
        Id = id;
    }
}