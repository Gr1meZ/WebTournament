using Moq;
using WebTournament.Domain.Objects.Belt;

namespace WebTournament.UnitTests.Builders;

public class BeltBuilder
{
    public static async Task<Belt> BuildAsync(Guid id)
    {
        var beltRepository = new Mock<IBeltRepository>();
        
        beltRepository.Setup(method => method.IsExistsAsync(1, "гып"))
            .ReturnsAsync(false);
        
        return await Belt.CreateAsync(id, 1,"гып", "Зеленый пояс", 
                beltRepository.Object);
    }
}