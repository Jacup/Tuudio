using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Tuudio.Infrastructure.Data;
using Tuudio.Infrastructure.Services;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.UnitTests.InfrastructureTests.Services;

[TestFixture]
public class UnitOfWorkTests
{
    private Mock<TuudioDbContext> dbContextMock;
    private Mock<DatabaseFacade> databaseFacadeMock;
    private Mock<IClientRepository> clientRepositoryMock;
    private Mock<IActivityRepository> activityRepositoryMock;
    private Mock<IPassTemplateRepository> passTemplateRepositoryMock;
    private Mock<IPassRepository> passRepositoryMock;
    private UnitOfWork unitOfWork;

    [SetUp]
    public void SetUp()
    {
        // Tworzymy mocka DbContextOptions
        var optionsMock = new Mock<DbContextOptions<TuudioDbContext>>();

        // Tworzymy mocka DbContext
        dbContextMock = new Mock<TuudioDbContext>();

        // Tworzymy mocka DatabaseFacade
        databaseFacadeMock = new Mock<DatabaseFacade>(dbContextMock.Object);

        // Ustawiamy zachowanie mocka DatabaseFacade
        databaseFacadeMock.Setup(f => f.BeginTransactionAsync(default))
            .ReturnsAsync(new Mock<IDbContextTransaction>().Object);

        dbContextMock.Setup(c => c.Database).Returns(databaseFacadeMock.Object);

        clientRepositoryMock = new Mock<IClientRepository>();
        activityRepositoryMock = new Mock<IActivityRepository>();
        passTemplateRepositoryMock = new Mock<IPassTemplateRepository>();
        passRepositoryMock = new Mock<IPassRepository>();

        unitOfWork = new UnitOfWork(
            dbContextMock.Object,
            clientRepositoryMock.Object,
            activityRepositoryMock.Object,
            passTemplateRepositoryMock.Object,
            passRepositoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        unitOfWork?.Dispose();
    }

    [Test]
    public void Dispose_ShouldCallDisposeOnContext()
    {
        unitOfWork.Dispose();

        dbContextMock.Verify(x => x.Dispose());
    }

}