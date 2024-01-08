using WebTournament.Domain.SeedWork;

namespace WebTournament.UnitTests.Builders;

internal interface IEntityBaseBuilder<TEntity> where TEntity : Entity
{
    static abstract Task<TEntity> BuildAsync(Guid id);
}