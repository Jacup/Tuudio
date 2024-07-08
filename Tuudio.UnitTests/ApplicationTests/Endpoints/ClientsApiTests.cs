using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Tuudio.Domain.Entities.People;
using Tuudio.Domain.Exceptions;
using Tuudio.DTOs.People;
using Tuudio.DTOs.People.Detailed;
using Tuudio.Endpoints;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.UnitTests.ApplicationTests.Endpoints;

[TestFixture]
public class ClientsApiTests
{
    private Mock<IClientRepository> repositoryMock;
    private Mock<IUnitOfWork> uowMock;

    private readonly List<Client> twoClientList =
    [
        new() { Id = firstGuid, FirstName = "John", LastName = "Doe"},
        new() { Id = new Guid("00000000-0000-0000-0000-000000000002"), FirstName = "Jane", LastName = "Smith"},
    ];

    private static readonly Guid firstGuid = new("00000000-0000-0000-0000-000000000001");

    [SetUp]
    public void SetUp()
    {
        repositoryMock = new();
        uowMock = new();
    }

    [Test]
    public async Task GetClientsAsync_MultipleObjects_EndpointShouldReturnValidResults()
    {
        // Arrange
        repositoryMock.Setup(m => m.GetAllAsync()).ReturnsAsync(twoClientList);
        uowMock.Setup(m => m.ClientRepository).Returns(repositoryMock.Object);

        // Act
        var result = await ClientsApi.GetClientsAsync(uowMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(Ok<IEnumerable<ClientDetailedDto>>));

        var castedResult = (Ok<IEnumerable<ClientDetailedDto>>)result;

        castedResult.StatusCode.ShouldBe(200);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.Count().ShouldBe(2);
    }

    [Test]
    public async Task GetClientsAsync_NoObjects_EndpointShouldReturnValidResults()
    {
        // Arrange
        repositoryMock.Setup(m => m.GetAllAsync()).ReturnsAsync([]);
        uowMock.Setup(m => m.ClientRepository).Returns(repositoryMock.Object);

        // Act
        var result = await ClientsApi.GetClientsAsync(uowMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(Ok<IEnumerable<ClientDetailedDto>>));

        var castedResult = (Ok<IEnumerable<ClientDetailedDto>>)result;

        castedResult.StatusCode.ShouldBe(200);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.Count().ShouldBe(0);
    }

    [Test]
    public async Task GetClientByIdAsync_MultipleObjectsExists_ShouldReturn200AndValidResult()
    {
        // Arrange
        var client = twoClientList.First();

        repositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(client);
        uowMock.Setup(m => m.ClientRepository).Returns(repositoryMock.Object);

        // Act
        var result = await ClientsApi.GetClientByIdAsync(uowMock.Object, client.Id);

        // Assert
        result.ShouldBeOfType(typeof(Ok<ClientDetailedDto>));

        var castedResult = (Ok<ClientDetailedDto>)result;

        castedResult.StatusCode.ShouldBe(200);

        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.FirstName.ShouldBe(client.FirstName);
        castedResult.Value.LastName.ShouldBe(client.LastName);
        castedResult.Value.Id.ShouldBe(client.Id);
    }

    [Test]
    public async Task GetClientByIdAsync_NotFoundObjectWithGivenId_ShouldReturn404AndNoResults()
    {
        // Arrange
        repositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>()));
        uowMock.Setup(m => m.ClientRepository).Returns(repositoryMock.Object);

        // Act
        var result = await ClientsApi.GetClientByIdAsync(uowMock.Object, It.IsAny<Guid>());

        // Assert
        result.ShouldBeOfType(typeof(NotFound<string>));

        var castedResult = (NotFound<string>)result;

        castedResult.StatusCode.ShouldBe(404);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("Client with ID \"00000000-0000-0000-0000-000000000000\" not found");
    }

    [Test]
    public async Task AddClientAsync_ValidDtoProvided_ShouldReturn201AndCreatedDetailedDto()
    {
        // Arrange
        var clientDto = new ClientDto()
        {
            FirstName = "John",
            LastName = "Doe",
        };

        uowMock
            .Setup(m => m.ClientRepository)
            .Returns(repositoryMock.Object);

        var validatorMock = new Mock<IValidator<ClientDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<ClientDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await ClientsApi.AddClientAsync(uowMock.Object, clientDto, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(Created<ClientDetailedDto>));

        var castedResult = (Created<ClientDetailedDto>)result;

        castedResult.StatusCode.ShouldBe(201);

        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.Id.ShouldNotBe(Guid.Empty);
        castedResult.Value.FirstName = clientDto.FirstName;
        castedResult.Value.LastName = clientDto.LastName;
        castedResult.Value.Email = null;
        castedResult.Value.PhoneNumber = null;

        var id = castedResult.Value.Id;
        castedResult.Location.ShouldNotBeNull();
        castedResult.Location.ShouldBe($"/clients/{id}");
    }

    [Test]
    public async Task AddClientAsync_NullDtoProvided_ShouldReturn400AndErrorMessage()
    {
        // Arrange
        uowMock
            .Setup(m => m.ClientRepository)
            .Returns(repositoryMock.Object);

        var validatorMock = new Mock<IValidator<ClientDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<ClientDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await ClientsApi.AddClientAsync(uowMock.Object, null!, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(BadRequest<string>));

        var castedResult = (BadRequest<string>)result;

        castedResult.StatusCode.ShouldBe(400);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("Client data is required.");
    }

    [Test]
    public async Task AddClientAsync_ValidatorReturnsInvalidDtoData_ShouldReturn400AndValidationErrors()
    {
        // Arrange
        ValidationFailure validationFailure = new("FirstName", "Invalid Value");
        ValidationResult validationResult = new(new List<ValidationFailure> { validationFailure });

        var clientDto = new ClientDto()
        {
            FirstName = "John",
            LastName = "Doe",
        };

        uowMock
            .Setup(m => m.ClientRepository)
            .Returns(repositoryMock.Object);

        var validatorMock = new Mock<IValidator<ClientDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<ClientDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await ClientsApi.AddClientAsync(uowMock.Object, clientDto, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(BadRequest<List<ValidationFailure>>));

        var castedResult = (BadRequest<List<ValidationFailure>>)result;

        castedResult.StatusCode.ShouldBe(400);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldContain(validationFailure);
    }

    [Test]
    public async Task UpdateClientAsync_ValidDtoProvided_ShouldReturn201AndUpdatedDetailedDto()
    {
        // Arrange
        var clientDto = new ClientDto()
        {
            FirstName = "John",
            LastName = "Doe",
        };

        uowMock
            .Setup(m => m.ClientRepository)
            .Returns(repositoryMock.Object);

        var validatorMock = new Mock<IValidator<ClientDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<ClientDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await ClientsApi.UpdateClientAsync(uowMock.Object, firstGuid, clientDto, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(Ok<ClientDetailedDto>));

        var castedResult = (Ok<ClientDetailedDto>)result;

        castedResult.StatusCode.ShouldBe(200);

        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.Id.ShouldNotBe(Guid.Empty);
        castedResult.Value.FirstName = clientDto.FirstName;
        castedResult.Value.LastName = clientDto.LastName;
        castedResult.Value.Email = null;
        castedResult.Value.PhoneNumber = null;

        var id = castedResult.Value.Id;
    }

    [Test]
    public async Task UpdateClientAsync_NullDtoProvided_ShouldReturn400AndErrorMessage()
    {
        // Arrange
        uowMock
            .Setup(m => m.ClientRepository)
            .Returns(repositoryMock.Object);

        var validatorMock = new Mock<IValidator<ClientDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<ClientDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await ClientsApi.UpdateClientAsync(uowMock.Object, firstGuid, null!, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(BadRequest<string>));

        var castedResult = (BadRequest<string>)result;

        castedResult.StatusCode.ShouldBe(400);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("Client data is required.");
    }

    [Test]
    public async Task UpdateClientAsync_EmptyIdProvided_ShouldReturn400AndErrorMessage()
    {
        // Arrange
        var clientDto = new ClientDto()
        {
            FirstName = "John",
            LastName = "Doe",
        };

        uowMock
            .Setup(m => m.ClientRepository)
            .Returns(repositoryMock.Object);

        var validatorMock = new Mock<IValidator<ClientDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<ClientDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await ClientsApi.UpdateClientAsync(uowMock.Object, Guid.Empty, clientDto, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(BadRequest<string>));

        var castedResult = (BadRequest<string>)result;

        castedResult.StatusCode.ShouldBe(400);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("Client id is required.");
    }

    [Test]
    public async Task UpdateClientAsync_ValidatorReturnsInvalidDtoData_ShouldReturn400AndValidationErrors()
    {
        // Arrange
        ValidationFailure validationFailure = new("FirstName", "Invalid Value");
        ValidationResult validationResult = new(new List<ValidationFailure> { validationFailure });

        var clientDto = new ClientDto()
        {
            FirstName = "John",
            LastName = "Doe",
        };

        uowMock
            .Setup(m => m.ClientRepository)
            .Returns(repositoryMock.Object);

        var validatorMock = new Mock<IValidator<ClientDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<ClientDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await ClientsApi.UpdateClientAsync(uowMock.Object, firstGuid, clientDto, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(BadRequest<List<ValidationFailure>>));

        var castedResult = (BadRequest<List<ValidationFailure>>)result;

        castedResult.StatusCode.ShouldBe(400);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldContain(validationFailure);
    }

    [Test]
    public async Task DeleteClientAsync_ValidIdProvided_ShouldReturn204()
    {
        // Arrange
        uowMock
            .Setup(m => m.ClientRepository)
            .Returns(repositoryMock.Object);

        // Act
        var result = await ClientsApi.DeleteClientAsync(uowMock.Object, It.IsAny<Guid>());

        // Assert
        result.ShouldBeOfType(typeof(NoContent));

        var castedResult = (NoContent)result;

        castedResult.StatusCode.ShouldBe(204);
    }

    [Test]
    public async Task DeleteClientAsync_InvalidIdProvided_ShouldReturn404()
    {
        // Arrange
        repositoryMock
            .Setup(m => m.DeleteAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new ClientNotFoundException(It.IsAny<Guid>()));
        
        uowMock
            .Setup(m => m.ClientRepository)
            .Returns(repositoryMock.Object);

        // Act
        var result = await ClientsApi.DeleteClientAsync(uowMock.Object, It.IsAny<Guid>());

        // Assert
        result.ShouldBeOfType(typeof(NotFound<string>));

        var castedResult = (NotFound<string>)result;

        castedResult.StatusCode.ShouldBe(404);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("Entity of type 'Tuudio.Domain.Entities.People.Client' with ID = '00000000-0000-0000-0000-000000000000' not found");
    }
}
