using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Tuudio.Domain.Entities.Activities;
using Tuudio.Domain.Exceptions;
using Tuudio.DTOs;
using Tuudio.Endpoints;
using Tuudio.Infrastructure.Services.Interfaces;
using Tuudio.Services.Mapping;

namespace Tuudio.UnitTests.ApplicationTests.Endpoints;

[TestFixture]
public class ActivitiesApiTests
{
    private Mock<IActivityRepository> activityRepositoryMock;
    private Mock<IPassTemplateRepository> passTemplatesRepositoryMock;
    private Mock<IUnitOfWork> uowMock;

    private readonly List<Activity> twoActivitiesList =
    [
        new() { Id = firstGuid, Name = "Foo"},
        new() { Id = secondGuid, Name = "Bar"},
    ];

    private static readonly Guid firstGuid = new("00000000-0000-0000-0000-000000000001");
    private static readonly Guid secondGuid = new("00000000-0000-0000-0000-000000000002");

    [SetUp]
    public void SetUp()
    {
        MapsterConfiguration.Configure();

        activityRepositoryMock = new();
        passTemplatesRepositoryMock = new();
        uowMock = new();
    }

    [Test]
    public async Task GetAsync_MultipleObjects_EndpointShouldReturnValidResults()
    {
        // Arrange
        activityRepositoryMock.Setup(m => m.GetAllAsync()).ReturnsAsync(twoActivitiesList);
        uowMock.Setup(m => m.ActivityRepository).Returns(activityRepositoryMock.Object);

        // Act
        var result = await ActivitiesApi.GetAsync(uowMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(Ok<IEnumerable<ActivityDetailedDto>>));

        var castedResult = (Ok<IEnumerable<ActivityDetailedDto>>)result;

        castedResult.StatusCode.ShouldBe(200);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.Count().ShouldBe(2);
    }

    [Test]
    public async Task GetAsync_NoObjects_EndpointShouldReturnValidResults()
    {
        // Arrange
        activityRepositoryMock.Setup(m => m.GetAllAsync()).ReturnsAsync([]);
        uowMock.Setup(m => m.ActivityRepository).Returns(activityRepositoryMock.Object);

        // Act
        var result = await ActivitiesApi.GetAsync(uowMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(Ok<IEnumerable<ActivityDetailedDto>>));

        var castedResult = (Ok<IEnumerable<ActivityDetailedDto>>)result;

        castedResult.StatusCode.ShouldBe(200);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.Count().ShouldBe(0);
    }

    [Test]
    public async Task GetByIdAsync_MultipleObjectsExists_ShouldReturn200AndValidResult()
    {
        // Arrange
        var activity = twoActivitiesList.First();

        activityRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(activity);
        uowMock.Setup(m => m.ActivityRepository).Returns(activityRepositoryMock.Object);

        // Act
        var result = await ActivitiesApi.GetByIdAsync(uowMock.Object, activity.Id);

        // Assert
        result.ShouldBeOfType(typeof(Ok<ActivityDetailedDto>));

        var castedResult = (Ok<ActivityDetailedDto>)result;

        castedResult.StatusCode.ShouldBe(200);

        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.Name.ShouldBe(activity.Name);
        castedResult.Value.Id.ShouldBe(activity.Id);
    }

    [Test]
    public async Task GetClientByIdAsync_NotFoundObjectWithGivenId_ShouldReturn404AndNoResults()
    {
        // Arrange
        activityRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>()));
        uowMock.Setup(m => m.ActivityRepository).Returns(activityRepositoryMock.Object);

        // Act
        var result = await ActivitiesApi.GetByIdAsync(uowMock.Object, It.IsAny<Guid>());

        // Assert
        result.ShouldBeOfType(typeof(NotFound<string>));

        var castedResult = (NotFound<string>)result;

        castedResult.StatusCode.ShouldBe(404);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("Activity with ID \"00000000-0000-0000-0000-000000000000\" not found");
    }

    [Test]
    public async Task AddAsync_ValidDtoProvided_ShouldReturn201AndCreatedDetailedDto()
    {
        // Arrange
        var activityDto = new ActivityDto()
        {
            Name = "Foo"
        };

        uowMock
            .Setup(m => m.ActivityRepository)
            .Returns(activityRepositoryMock.Object);

        uowMock
            .Setup(m => m.PassTemplateRepository)
            .Returns(passTemplatesRepositoryMock.Object);

        var validatorMock = new Mock<IValidator<ActivityDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<ActivityDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await ActivitiesApi.AddAsync(uowMock.Object, activityDto, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(Created<ActivityDetailedDto>));

        var castedResult = (Created<ActivityDetailedDto>)result;

        castedResult.StatusCode.ShouldBe(201);

        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.Id.ShouldNotBe(Guid.Empty);
        castedResult.Value.Name = activityDto.Name;
        castedResult.Value.Description.ShouldBeNull();

        var id = castedResult.Value.Id;
        castedResult.Location.ShouldNotBeNull();
        castedResult.Location.ShouldBe($"/activities/{id}");
    }

    [Test]
    public async Task AddAsync_NullDtoProvided_ShouldReturn400AndErrorMessage()
    {
        // Arrange
        uowMock
            .Setup(m => m.ActivityRepository)
            .Returns(activityRepositoryMock.Object);

        var validatorMock = new Mock<IValidator<ActivityDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<ActivityDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await ActivitiesApi.AddAsync(uowMock.Object, null!, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(BadRequest<string>));

        var castedResult = (BadRequest<string>)result;

        castedResult.StatusCode.ShouldBe(400);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("Activity data is required.");
    }

    [Test]
    public async Task AddAsync_ValidatorReturnsInvalidDtoData_ShouldReturn400AndValidationErrors()
    {
        // Arrange
        ValidationFailure validationFailure = new("Name", "Invalid Value");
        ValidationResult validationResult = new(new List<ValidationFailure> { validationFailure });

        var activityDto = new ActivityDto()
        {
            Name = "Foo"
        };

        uowMock
            .Setup(m => m.ActivityRepository)
            .Returns(activityRepositoryMock.Object);

        var validatorMock = new Mock<IValidator<ActivityDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<ActivityDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await ActivitiesApi.AddAsync(uowMock.Object, activityDto, validatorMock.Object);

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
        var activityDto = new ActivityDto()
        {
            Name = "Foo"
        };

        uowMock
            .Setup(m => m.ActivityRepository)
            .Returns(activityRepositoryMock.Object);
        uowMock
            .Setup(m => m.PassTemplateRepository)
            .Returns(passTemplatesRepositoryMock.Object);

        var validatorMock = new Mock<IValidator<ActivityDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<ActivityDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await ActivitiesApi.UpdateAsync(uowMock.Object, firstGuid, activityDto, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(Ok<ActivityDetailedDto>));

        var castedResult = (Ok<ActivityDetailedDto>)result;

        castedResult.StatusCode.ShouldBe(200);

        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.Id.ShouldNotBe(Guid.Empty);
        castedResult.Value.Name = activityDto.Name;

        var id = castedResult.Value.Id;
    }

    [Test]
    public async Task UpdateAsync_NullDtoProvided_ShouldReturn400AndErrorMessage()
    {
        // Arrange
        uowMock
            .Setup(m => m.ActivityRepository)
            .Returns(activityRepositoryMock.Object);

        var validatorMock = new Mock<IValidator<ActivityDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<ActivityDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await ActivitiesApi.UpdateAsync(uowMock.Object, firstGuid, null!, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(BadRequest<string>));

        var castedResult = (BadRequest<string>)result;

        castedResult.StatusCode.ShouldBe(400);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("Activity data is required.");
    }

    [Test]
    public async Task UpdateClientAsync_EmptyIdProvided_ShouldReturn400AndErrorMessage()
    {
        // Arrange
        var activityDto = new ActivityDto()
        {
            Name = "Foo"
        };

        uowMock
            .Setup(m => m.ActivityRepository)
            .Returns(activityRepositoryMock.Object);

        var validatorMock = new Mock<IValidator<ActivityDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<ActivityDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await ActivitiesApi.UpdateAsync(uowMock.Object, Guid.Empty, activityDto, validatorMock.Object);

        // Assert
        result.ShouldBeOfType(typeof(BadRequest<string>));

        var castedResult = (BadRequest<string>)result;

        castedResult.StatusCode.ShouldBe(400);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("Activity id is required.");
    }

    [Test]
    public async Task UpdateClientAsync_ValidatorReturnsInvalidDtoData_ShouldReturn400AndValidationErrors()
    {
        // Arrange
        ValidationFailure validationFailure = new("Name", "Invalid Value");
        ValidationResult validationResult = new(new List<ValidationFailure> { validationFailure });

        var activityDto = new ActivityDto()
        {
            Name = "Foo"
        };

        uowMock
            .Setup(m => m.ActivityRepository)
            .Returns(activityRepositoryMock.Object);

        var validatorMock = new Mock<IValidator<ActivityDto>>();
        validatorMock
            .Setup(m => m.ValidateAsync(It.IsAny<ActivityDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await ActivitiesApi.UpdateAsync(uowMock.Object, firstGuid, activityDto, validatorMock.Object);

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
            .Setup(m => m.ActivityRepository)
            .Returns(activityRepositoryMock.Object);

        // Act
        var result = await ActivitiesApi.DeleteAsync(uowMock.Object, It.IsAny<Guid>());

        // Assert
        result.ShouldBeOfType(typeof(NoContent));

        var castedResult = (NoContent)result;

        castedResult.StatusCode.ShouldBe(204);
    }

    [Test]
    public async Task DeleteAsync_InvalidIdProvided_ShouldReturn404()
    {
        // Arrange
        activityRepositoryMock
            .Setup(m => m.DeleteAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new EntityNotFoundException<Activity>(It.IsAny<Guid>()));

        uowMock
            .Setup(m => m.ActivityRepository)
            .Returns(activityRepositoryMock.Object);

        // Act
        var result = await ActivitiesApi.DeleteAsync(uowMock.Object, It.IsAny<Guid>());

        // Assert
        result.ShouldBeOfType(typeof(NotFound<string>));

        var castedResult = (NotFound<string>)result;

        castedResult.StatusCode.ShouldBe(404);
        castedResult.Value.ShouldNotBeNull();
        castedResult.Value.ShouldBe("Entity of type 'Activity' with ID = '00000000-0000-0000-0000-000000000000' not found");
    }
}
