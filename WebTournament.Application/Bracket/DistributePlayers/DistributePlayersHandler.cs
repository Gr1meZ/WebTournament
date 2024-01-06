using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebTournament.Application.Bracket.Validators;
using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.Bracket;
using WebTournament.Domain.Objects.BracketWinner;
using WebTournament.Domain.Objects.Fighter;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Bracket.DistributePlayers;

public class DistributePlayersHandler : ICommandHandler<DistributePlayersCommand>
{
    private readonly IFighterRepository _fighterRepository;
    private readonly IBracketRepository _bracketRepository;
    private readonly IBracketWinnerRepository _bracketWinnerRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DistributePlayersHandler(IFighterRepository fighterRepository, IUnitOfWork unitOfWork, IBracketRepository bracketRepository, IBracketWinnerRepository bracketWinnerRepository)
    {
        _fighterRepository = fighterRepository;
        _unitOfWork = unitOfWork;
        _bracketRepository = bracketRepository;
        _bracketWinnerRepository = bracketWinnerRepository;
    }

    public async Task Handle(DistributePlayersCommand request, CancellationToken cancellationToken)
    {
        var fighters = await _fighterRepository.GetAll(request.Id)
            .AsTracking()
            .ToListAsync(cancellationToken: cancellationToken);
        
        foreach (var fighter in fighters)
        {
            var bracketId = await _bracketRepository.GetAll()
                .Where(x => x.WeightCategorieId == fighter.WeightCategorieId && x.Division.Contains(fighter.BeltId) && x.TournamentId == request.Id)
                .Select(x => x.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            BracketValidator.IsFighterHaveBracket(bracketId, fighter.Surname, fighter.Name);
            fighter.SetBracket(bracketId);
        }
        
        await DrawFighters(request.Id, fighters); 
        await CreateBracketWinnersAsync();
        await _unitOfWork.CommitAsync(cancellationToken);
    }
     private async Task DrawFighters(Guid tournamentId, IReadOnlyCollection<Domain.Objects.Fighter.Fighter> fighters)
     {
         var brackets = await _bracketRepository.GetByTournamentId(tournamentId).ToListAsync();
        
        foreach (var bracket in brackets)
        {
            var bracketData = new BracketData()
            {
                teams = new List<List<Team>>(),
                results = new List<List<List<List<int?>>>>()
            };

            var bracketFighters = fighters.Where(x => x.BracketId == bracket.Id).ToList();

            if (bracketFighters.Count == 0)
            {
                _bracketRepository.Remove(bracket);
                continue;
            }
            
            while (bracketFighters.Count > 0)
            {
                var firstPlayer = bracketFighters.FirstOrDefault();
                var secondPlayer = bracketFighters.FirstOrDefault(x => x.Country != firstPlayer?.Country
                                                                       || x.City != firstPlayer.City
                                                                       || x.Trainer.ClubId != firstPlayer.Trainer.ClubId
                                                                       || x.TrainerId != firstPlayer.TrainerId) ?? bracketFighters.FirstOrDefault(x => x != firstPlayer);
                
                bracketData.teams.Add(new List<Team>()
                {
                    new() {name = $"{firstPlayer!.Surname} {firstPlayer!.Name} - {firstPlayer.Trainer.Club.Name}, {firstPlayer.City}", id = firstPlayer.Id.ToString()},
                    (secondPlayer == null ? null : new Team {name = $"{secondPlayer!.Surname} {secondPlayer!.Name} - {secondPlayer.Trainer.Club.Name}, {secondPlayer.City}", id = secondPlayer.Id.ToString()})! 
                });
                
                bracketFighters.Remove(firstPlayer);
                if (secondPlayer != null) bracketFighters.Remove(secondPlayer);
            }

            ValidateBracketData(bracketData);
            bracket.UpdateState(JsonConvert.SerializeObject(bracketData));
        }
    }
     private void ValidateBracketData(BracketData bracketData)
     {
         var count = bracketData.teams.Count;
         var nearestPowerOfTwo = 1;
        
         while (nearestPowerOfTwo < count)
         {
             nearestPowerOfTwo *= 2;
         }

         if (nearestPowerOfTwo <= count) return;
         var numberOfEmptyPlayers = nearestPowerOfTwo - count;
         for (var i = 0; i < numberOfEmptyPlayers; i++)
         {
             bracketData.teams.Add(new List<Team>()
             {
                 null!, null!
             });
         }
     }
    private async Task CreateBracketWinnersAsync()
    {
        var bracketIds = await _bracketRepository.GetAll().Select(x => x.Id).ToListAsync();
        if (!bracketIds.Any() || await _bracketWinnerRepository.GetAll().AnyAsync(x => bracketIds.Contains(x.Id))) return;
        {
            var bracketWinners = bracketIds.Select(x => BracketWinner.Create(x, null, null, null));

            await _bracketWinnerRepository.AddRangeAsync(bracketWinners);
        }
    }
}