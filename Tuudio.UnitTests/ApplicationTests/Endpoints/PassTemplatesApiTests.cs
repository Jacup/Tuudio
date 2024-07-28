using FluentValidation.Results;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Tuudio.Domain.Entities.Passes;
using Tuudio.Domain.Exceptions;
using Tuudio.DTOs;
using Tuudio.Endpoints;
using Tuudio.Infrastructure.Services.Interfaces;
using Tuudio.Validators;
using Tuudio.Domain.Enums;
using Tuudio.Domain.Entities.Activities;

namespace Tuudio.UnitTests.ApplicationTests.Endpoints;

public class PassTemplatesApiTests
{
    private Mock<IPassTemplateRepository> repositoryMock;
    private Mock<IUnitOfWork> uowMock;

    private readonly List<PassTemplate> twoPassTemplates =
    [
        new()
        {
            Id = firstGuid,
            Name = "Foo",
            Price = new Price() { Amount = 50, Period = Period.Month },
            Duration = new Duration() { Amount = 3, Period = Period.Month },
            Entries = 0,
        },
        new()
        {
            Id = secondGuid,
            Name = "Bar",
            Price = new Price() { Amount = 100, Period = Period.Month },
            Duration = new Duration() { Amount = 1, Period = Period.Month },
            Entries = 0,
        },
    ];

    private static readonly Guid firstGuid = new("00000000-0000-0000-0010-000000000000");
    private static readonly Guid secondGuid = new("00000000-0000-0000-0010-000000000001");

    [SetUp]
    public void SetUp()
    {
        repositoryMock = new();
        uowMock = new();
    }

    [Test]
    public async Task GetAsync_MultipleObjects_EndpointShouldReturnValidResults()
    {
        // Arrange
        repositoryMock.Setup(m => m.GetAllAsync()).ReturnsAsync(twoPassTemplates);
        uowMock.Setup(m => m.PassTemplateRepository).Returns(repositoryMock.Object);

        // Act
        var result = await PassTemplatesApi.GetAsync(uowMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(Ok<IEnumerable<PassTemplateDetailedDto>>));

        var castedResult = (Ok<IEnumerable<PassTemplateDetailedDto>>)result;

        castedResult.StatusCode.ShouldBe(200);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.Count().ShouldBe(2);
    }

    [Test]
    public async Task GetAsync_NoObjects_EndpointShouldReturnValidResults()
    {
        // Arrange
        repositoryMock.Setup(m => m.GetAllAsync()).ReturnsAsync([]);
        uowMock.Setup(m => m.PassTemplateRepository).Returns(repositoryMock.Object);

        // Act
        var result = await PassTemplatesApi.GetAsync(uowMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(Ok<IEnumerable<PassTemplateDetailedDto>>));

        var castedResult = (Ok<IEnumerable<PassTemplateDetailedDto>>)result;

        castedResult.StatusCode.ShouldBe(200);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.Count().ShouldBe(0);
    }

    [Test]
    public async Task GetByIdAsync_MultipleObjectsExists_ShouldReturn200AndValidResult()
    {
        // Arrange
        var passTemplate = twoPassTemplates.First();

        repositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(passTemplate);
        uowMock.Setup(m => m.PassTemplateRepository).Returns(repositoryMock.Object);

        // Act
        var result = await PassTemplatesApi.GetByIdAsync(uowMock.Object, passTemplate.Id);

        // Assert
        result.ShouldBeOfType(typeof(Ok<PassTemplateDetailedDto>));

        var castedResult = (Ok<PassTemplateDetailedDto>)result;

        castedResult.StatusCode.ShouldBe(200);

        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.Name.ShouldBe(passTemplate.Name);
        castedResult.Value.Id.ShouldBe(passTemplate.Id);
    }

    [Test]
    public async Task GetClientByIdAsync_NotFoundObjectWithGivenId_ShouldReturn404AndNoResults()
    {
        // Arrange
        repositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>()));
        uowMock.Setup(m => m.PassTemplateRepository).Returns(repositoryMock.Object);

        // Act
        var result = await PassTemplatesApi.GetByIdAsync(uowMock.Object, It.IsAny<Guid>());

        // Assert
        result.ShouldBeOfType(typeof(NotFound<string>));

        var castedResult = (NotFound<string>)result;

        castedResult.StatusCode.ShouldBe(404);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("PassTemplate with ID \"00000000-0000-0000-0000-000000000000\" not found");
    }

    [Test]
    public async Task AddAsync_ValidDtoProvided_ShouldReturn201AndCreatedDetailedDto()
    {
        // Arrange
        var passTemplateDto = new PassTemplateDto()
        {
            Name = "Foo",
            Price_Amount = 50,
            Price_Period = Period.Month,
            Duration_Amount = 3,
            Duration_Period = Period.Month,
            Entries = 0,
            Activities = [Guid.Parse("00000000-0000-0000-0001-000000000001")]
        };

        uowMock
            .Setup(m => m.PassTemplateRepository)
            .Returns(repositoryMock.Object);
        uowMock
            .Setup(m => m.ActivityRepository.GetByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync([]);

        var validatorMock = new Mock<IValidator<PassTemplateDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<PassTemplateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await PassTemplatesApi.AddAsync(uowMock.Object, passTemplateDto, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(Created<PassTemplateDetailedDto>));

        var castedResult = (Created<PassTemplateDetailedDto>)result;

        castedResult.StatusCode.ShouldBe(201);

        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.Id.ShouldNotBe(Guid.Empty);
        castedResult.Value.Name = passTemplateDto.Name;
        castedResult.Value.Description.ShouldBeNull();

        var id = castedResult.Value.Id;
        castedResult.Location.ShouldNotBeNull();
        castedResult.Location.ShouldBe($"/passtemplates/{id}");
    }

    [Test]
    public async Task AddAsync_NullDtoProvided_ShouldReturn400AndErrorMessage()
    {
        // Arrange
        uowMock
            .Setup(m => m.PassTemplateRepository)
            .Returns(repositoryMock.Object);

        var validatorMock = new Mock<IValidator<PassTemplateDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<PassTemplateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await PassTemplatesApi.AddAsync(uowMock.Object, null!, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(BadRequest<string>));

        var castedResult = (BadRequest<string>)result;

        castedResult.StatusCode.ShouldBe(400);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("PassTemplate data is required.");
    }

    [Test]
    public async Task AddAsync_ValidatorReturnsInvalidDtoData_ShouldReturn400AndValidationErrors()
    {
        // Arrange
        ValidationFailure validationFailure = new("Name", "Invalid Value");
        ValidationResult validationResult = new(new List<ValidationFailure> { validationFailure });

        var passTemplateDto = new PassTemplateDto()
        {
            Name = "Foo",
            Price_Amount = 50,
            Price_Period = Period.Month,
            Duration_Amount = 3,
            Duration_Period = Period.Month,
            Entries = 0,
            Activities = [Guid.Parse("00000000-0000-0000-0001-000000000001")]
        };

        uowMock
            .Setup(m => m.PassTemplateRepository)
            .Returns(repositoryMock.Object);

        var validatorMock = new Mock<IValidator<PassTemplateDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<PassTemplateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await PassTemplatesApi.AddAsync(uowMock.Object, passTemplateDto, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(BadRequest<List<ValidationFailure>>));

        var castedResult = (BadRequest<List<ValidationFailure>>)result;

        castedResult.StatusCode.ShouldBe(400);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldContain(validationFailure);
    }

    [Test]
    public async Task UpdateAsync_ValidDtoProvided_ShouldReturn201AndUpdatedDetailedDto()
    {
        // Arrange
        var passTemplateDto = new PassTemplateDto()
        {
            Name = "Foo",
            Price_Amount = 50,
            Price_Period = Period.Month,
            Duration_Amount = 3,
            Duration_Period = Period.Month,
            Entries = 0,
            Activities = [Guid.Parse("00000000-0000-0000-0001-000000000001")]
        };

        uowMock
            .Setup(m => m.PassTemplateRepository)
            .Returns(repositoryMock.Object);

        uowMock
            .Setup(m => m.ActivityRepository.GetByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync([]);

        var validatorMock = new Mock<IValidator<PassTemplateDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<PassTemplateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await PassTemplatesApi.UpdateAsync(uowMock.Object, firstGuid, passTemplateDto, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(Ok<PassTemplateDetailedDto>));

        var castedResult = (Ok<PassTemplateDetailedDto>)result;

        castedResult.StatusCode.ShouldBe(200);

        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.Id.ShouldNotBe(Guid.Empty);
        castedResult.Value.Name = passTemplateDto.Name;

        var id = castedResult.Value.Id;
    }

    [Test]
    public async Task UpdateAsync_NullDtoProvided_ShouldReturn400AndErrorMessage()
    {
        // Arrange
        uowMock
            .Setup(m => m.PassTemplateRepository)
            .Returns(repositoryMock.Object);
        uowMock
            .Setup(m => m.ActivityRepository.GetByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync([]);

        var validatorMock = new Mock<IValidator<PassTemplateDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<PassTemplateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await PassTemplatesApi.UpdateAsync(uowMock.Object, firstGuid, null!, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(BadRequest<string>));

        var castedResult = (BadRequest<string>)result;

        castedResult.StatusCode.ShouldBe(400);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("PassTemplate data is required.");
    }

    [Test]
    public async Task UpdateClientAsync_EmptyIdProvided_ShouldReturn400AndErrorMessage()
    {
        // Arrange
        var passTemplateDto = new PassTemplateDto()
        {
            Name = "Foo",
            Price_Amount = 50,
            Price_Period = Period.Month,
            Duration_Amount = 3,
            Duration_Period = Period.Month,
            Entries = 0,
            Activities = [Guid.Parse("00000000-0000-0000-0001-000000000001")]
        };

        uowMock
            .Setup(m => m.PassTemplateRepository)
            .Returns(repositoryMock.Object);
        uowMock
            .Setup(m => m.ActivityRepository.GetByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync([]);

        var validatorMock = new Mock<IValidator<PassTemplateDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<PassTemplateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await PassTemplatesApi.UpdateAsync(uowMock.Object, Guid.Empty, passTemplateDto, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(BadRequest<string>));

        var castedResult = (BadRequest<string>)result;

        castedResult.StatusCode.ShouldBe(400);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("PassTemplate id is required.");
    }

    [Test]
    public async Task UpdateClientAsync_ValidatorReturnsInvalidDtoData_ShouldReturn400AndValidationErrors()
    {
        // Arrange
        ValidationFailure validationFailure = new("Name", "Invalid Value");
        ValidationResult validationResult = new(new List<ValidationFailure> { validationFailure });

        var passTemplateDto = new PassTemplateDto()
        {
            Name = "Foo",
            Price_Amount = 50,
            Price_Period = Period.Month,
            Duration_Amount = 3,
            Duration_Period = Period.Month,
            Entries = 0,
            Activities = [Guid.Parse("00000000-0000-0000-0001-000000000001")]
        };

        uowMock
            .Setup(m => m.PassTemplateRepository)
            .Returns(repositoryMock.Object);

        var validatorMock = new Mock<IValidator<PassTemplateDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<PassTemplateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await PassTemplatesApi.UpdateAsync(uowMock.Object, firstGuid, passTemplateDto, validatorMock.Object);

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
            .Setup(m => m.PassTemplateRepository)
            .Returns(repositoryMock.Object);

        // Act
        var result = await PassTemplatesApi.DeleteAsync(uowMock.Object, It.IsAny<Guid>());

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
            .ThrowsAsync(new EntityNotFoundException<PassTemplate>(It.IsAny<Guid>()));

        uowMock
            .Setup(m => m.PassTemplateRepository)
            .Returns(repositoryMock.Object);

        // Act
        var result = await PassTemplatesApi.DeleteAsync(uowMock.Object, It.IsAny<Guid>());

        // Assert
        result.ShouldBeOfType(typeof(NotFound<string>));

        var castedResult = (NotFound<string>)result;

        castedResult.StatusCode.ShouldBe(404);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("Entity of type 'PassTemplate' with ID = '00000000-0000-0000-0000-000000000000' not found");
    }
}
