using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Tuudio.Domain.Entities.Passes;
using Tuudio.Domain.Exceptions;
using Tuudio.DTOs;
using Tuudio.Endpoints;
using Tuudio.Infrastructure.Services.Interfaces;
using Tuudio.Services.Mapping;
using Tuudio.UnitTests.Helpers.DataFactories;

namespace Tuudio.UnitTests.ApplicationTests.Endpoints;

[TestFixture]
internal class PassApiTests
{
    private Mock<IPassRepository> repositoryMock;
    private Mock<IUnitOfWork> uowMock;

    private readonly List<Pass> twoPasss =
    [
        new()
        {
            Id = firstGuid,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now.AddDays(1),

            FromDate = DateOnly.ParseExact("01-01-2024", "dd-MM-yyyy"),
            ToDate = DateOnly.ParseExact("01-03-2024", "dd-MM-yyyy"),

            ClientId = new Guid("00000000-0000-0000-0000-000000000001"),
            PassTemplateId = new Guid("00000000-0000-0000-0002-000000000001"),

        },
        new()
        {
            Id = secondGuid,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now.AddDays(1),

            FromDate = DateOnly.ParseExact("01-01-2024", "dd-MM-yyyy"),
            ToDate = DateOnly.ParseExact("01-03-2024", "dd-MM-yyyy"),

            ClientId = new Guid("00000000-0000-0000-0000-000000000002"),
            PassTemplateId = new Guid("00000000-0000-0000-0002-000000000002"),
        },
    ];

    private static readonly Guid firstGuid = new("00000000-0000-0000-0003-000000000001");
    private static readonly Guid secondGuid = new("00000000-0000-0000-0003-000000000002");

    [SetUp]
    public void SetUp()
    {
        repositoryMock = new();
        uowMock = new();
        MapsterConfiguration.Configure();
    }

    [Test]
    public async Task GetAsync_MultipleObjects_EndpointShouldReturnValidResults()
    {
        // Arrange
        repositoryMock.Setup(m => m.GetAllAsync()).ReturnsAsync(twoPasss);
        uowMock.Setup(m => m.PassRepository).Returns(repositoryMock.Object);

        // Act
        var result = await PassApi.GetAsync(uowMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(Ok<IEnumerable<PassDetailedDto>>));

        var castedResult = (Ok<IEnumerable<PassDetailedDto>>)result;

        castedResult.StatusCode.ShouldBe(200);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.Count().ShouldBe(2);
    }

    [Test]
    public async Task GetAsync_NoObjects_EndpointShouldReturnValidResults()
    {
        // Arrange
        repositoryMock.Setup(m => m.GetAllAsync()).ReturnsAsync([]);
        uowMock.Setup(m => m.PassRepository).Returns(repositoryMock.Object);

        // Act
        var result = await PassApi.GetAsync(uowMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(Ok<IEnumerable<PassDetailedDto>>));

        var castedResult = (Ok<IEnumerable<PassDetailedDto>>)result;

        castedResult.StatusCode.ShouldBe(200);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.Count().ShouldBe(0);
    }

    [Test]
    public async Task GetByIdAsync_MultipleObjectsExists_ShouldReturn200AndValidResult()
    {
        // Arrange
        var pass = twoPasss.First();

        repositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(pass);
        uowMock.Setup(m => m.PassRepository).Returns(repositoryMock.Object);

        // Act
        var result = await PassApi.GetByIdAsync(uowMock.Object, pass.Id);

        // Assert
        result.ShouldBeOfType(typeof(Ok<PassDetailedDto>));

        var castedResult = (Ok<PassDetailedDto>)result;

        castedResult.StatusCode.ShouldBe(200);

        castedResult.Value.ShouldNotBeNull();

        castedResult.Value.Id.ShouldBe(pass.Id);
        castedResult.Value.FromDate.ShouldBe(pass.FromDate);
        castedResult.Value.ToDate.ShouldBe(pass.ToDate);
        castedResult.Value.ClientId.ShouldBe(pass.ClientId);
        castedResult.Value.PassTemplateId.ShouldBe(pass.PassTemplateId);
    }

    [Test]
    public async Task GetClientByIdAsync_NotFoundObjectWithGivenId_ShouldReturn404AndNoResults()
    {
        // Arrange
        repositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>()));
        uowMock.Setup(m => m.PassRepository).Returns(repositoryMock.Object);

        // Act
        var result = await PassApi.GetByIdAsync(uowMock.Object, It.IsAny<Guid>());

        // Assert
        result.ShouldBeOfType(typeof(NotFound<string>));

        var castedResult = (NotFound<string>)result;

        castedResult.StatusCode.ShouldBe(404);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("Pass with ID \"00000000-0000-0000-0000-000000000000\" not found");
    }

    [Test]
    public async Task AddAsync_ValidDtoProvided_ShouldReturn201AndCreatedDetailedDto()
    {
        // Arrange
        var passDto = PassFactory.GetPassDto();

        uowMock
            .Setup(m => m.PassRepository)
            .Returns(repositoryMock.Object);
        uowMock
            .Setup(m => m.ActivityRepository.GetByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync([]);

        var validatorMock = new Mock<IValidator<PassDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<PassDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await PassApi.AddAsync(uowMock.Object, passDto, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(Created<PassDetailedDto>));

        var castedResult = (Created<PassDetailedDto>)result;

        castedResult.StatusCode.ShouldBe(201);

        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.Id.ShouldNotBe(Guid.Empty);
        castedResult.Value.FromDate.ShouldBe(passDto.FromDate);
        castedResult.Value.ToDate.ShouldBe(passDto.ToDate);
        castedResult.Value.ClientId.ShouldBe(passDto.ClientId);
        castedResult.Value.PassTemplateId.ShouldBe(passDto.PassTemplateId);

        var id = castedResult.Value.Id;
        castedResult.Location.ShouldNotBeNull();
        castedResult.Location.ShouldBe($"/passes/{id}");
    }

    [Test]
    public async Task AddAsync_NullDtoProvided_ShouldReturn400AndErrorMessage()
    {
        // Arrange
        uowMock
            .Setup(m => m.PassRepository)
            .Returns(repositoryMock.Object);

        var validatorMock = new Mock<IValidator<PassDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<PassDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await PassApi.AddAsync(uowMock.Object, null!, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(BadRequest<string>));

        var castedResult = (BadRequest<string>)result;

        castedResult.StatusCode.ShouldBe(400);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("Pass data is required.");
    }

    [Test]
    public async Task AddAsync_ValidatorReturnsInvalidDtoData_ShouldReturn400AndValidationErrors()
    {
        // Arrange
        ValidationFailure validationFailure = new("Name", "Invalid Value");
        ValidationResult validationResult = new(new List<ValidationFailure> { validationFailure });

        var passDto = PassFactory.GetPassDto();

        uowMock
            .Setup(m => m.PassRepository)
            .Returns(repositoryMock.Object);

        var validatorMock = new Mock<IValidator<PassDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<PassDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await PassApi.AddAsync(uowMock.Object, passDto, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(BadRequest<List<ValidationFailure>>));

        var castedResult = (BadRequest<List<ValidationFailure>>)result;

        castedResult.StatusCode.ShouldBe(400);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldContain(validationFailure);
    }

    [Test]
    public async Task UpdateAsync_ValidDtoProvided_ShouldReturn200AndUpdatedDetailedDto()
    {
        // Arrange
        var passDto = PassFactory.GetPassDto();

        uowMock
            .Setup(m => m.PassRepository)
            .Returns(repositoryMock.Object);

        uowMock
            .Setup(m => m.ActivityRepository.GetByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync([]);

        var validatorMock = new Mock<IValidator<PassDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<PassDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await PassApi.UpdateAsync(uowMock.Object, firstGuid, passDto, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(Ok<PassDetailedDto>));

        var castedResult = (Ok<PassDetailedDto>)result;

        castedResult.StatusCode.ShouldBe(200);

        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.Id.ShouldBe(firstGuid);
        castedResult.Value.FromDate.ShouldBe(passDto.FromDate);
        castedResult.Value.ToDate.ShouldBe(passDto.ToDate);
        castedResult.Value.ClientId.ShouldBe(passDto.ClientId);
        castedResult.Value.PassTemplateId.ShouldBe(passDto.PassTemplateId);
    }

    [Test]
    public async Task UpdateAsync_NullDtoProvided_ShouldReturn400AndErrorMessage()
    {
        // Arrange
        uowMock
            .Setup(m => m.PassRepository)
            .Returns(repositoryMock.Object);
        uowMock
            .Setup(m => m.ActivityRepository.GetByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync([]);

        var validatorMock = new Mock<IValidator<PassDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<PassDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await PassApi.UpdateAsync(uowMock.Object, firstGuid, null!, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(BadRequest<string>));

        var castedResult = (BadRequest<string>)result;

        castedResult.StatusCode.ShouldBe(400);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("Pass data is required.");
    }

    [Test]
    public async Task UpdateClientAsync_EmptyIdProvided_ShouldReturn400AndErrorMessage()
    {
        // Arrange
        var passDto = PassFactory.GetPassDto();

        uowMock
            .Setup(m => m.PassRepository)
            .Returns(repositoryMock.Object);
        uowMock
            .Setup(m => m.ActivityRepository.GetByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync([]);

        var validatorMock = new Mock<IValidator<PassDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<PassDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await PassApi.UpdateAsync(uowMock.Object, Guid.Empty, passDto, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(BadRequest<string>));

        var castedResult = (BadRequest<string>)result;

        castedResult.StatusCode.ShouldBe(400);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("Pass id is required.");
    }

    [Test]
    public async Task UpdateClientAsync_ValidatorReturnsInvalidDtoData_ShouldReturn400AndValidationErrors()
    {
        // Arrange
        ValidationFailure validationFailure = new("Name", "Invalid Value");
        ValidationResult validationResult = new(new List<ValidationFailure> { validationFailure });

        var passDto = PassFactory.GetPassDto();

        uowMock
            .Setup(m => m.PassRepository)
            .Returns(repositoryMock.Object);

        var validatorMock = new Mock<IValidator<PassDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<PassDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await PassApi.UpdateAsync(uowMock.Object, firstGuid, passDto, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(BadRequest<List<ValidationFailure>>));

        var castedResult = (BadRequest<List<ValidationFailure>>)result;

        castedResult.StatusCode.ShouldBe(400);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldContain(validationFailure);
    }

    [Test]
    public async Task DeleteAsync_ValidIdProvided_ShouldReturn204()
    {
        // Arrange
        uowMock
            .Setup(m => m.PassRepository)
            .Returns(repositoryMock.Object);

        // Act
        var result = await PassApi.DeleteAsync(uowMock.Object, It.IsAny<Guid>());

        // Assert
        result.ShouldBeOfType(typeof(NoContent));

        var castedResult = (NoContent)result;

        castedResult.StatusCode.ShouldBe(204);
    }

    [Test]
    public async Task DeleteAsync_InvalidIdProvided_ShouldReturn404()
    {
        // Arrange
        repositoryMock
            .Setup(m => m.DeleteAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new EntityNotFoundException<Pass>(It.IsAny<Guid>()));

        uowMock
            .Setup(m => m.PassRepository)
            .Returns(repositoryMock.Object);

        // Act
        var result = await PassApi.DeleteAsync(uowMock.Object, It.IsAny<Guid>());

        // Assert
        result.ShouldBeOfType(typeof(NotFound<string>));

        var castedResult = (NotFound<string>)result;

        castedResult.StatusCode.ShouldBe(404);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("Entity of type 'Pass' with ID = '00000000-0000-0000-0000-000000000000' not found");
    }

}
