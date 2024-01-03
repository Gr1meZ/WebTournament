using WebTournament.Application.Common;
using WebTournament.Application.Configuration.Queries;

namespace WebTournament.Application.Select2.Queries;

public class Select2FightersQuery : Select2Request, IQuery<Select2Response>
{
    public Select2FightersQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}