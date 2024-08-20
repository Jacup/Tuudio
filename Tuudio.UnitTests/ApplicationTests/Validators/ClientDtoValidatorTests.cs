using FluentValidation.TestHelper;
using Tuudio.DTOs.People;
using Tuudio.Services.Validators;

namespace Tuudio.UnitTests.ApplicationTests.Validators;

[TestFixture]
internal class ClientDtoValidatorTests
{
    private ClientDtoValidator validator;

    [SetUp]
    public void Setup()
    {
        validator = new ClientDtoValidator();
    }

    #region FirstName

    [TestCase("A")]
    [TestCase("John")]
    [TestCase("John Adam")]
    [TestCase("John Adam2222")]
    [TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKA")]
    public async Task ValidateAsync_ValidFirstName_ShouldBeValid(string value)
    {
        var model = new ClientDto { FirstName = value, LastName = "Doe" };

        var result = await validator.TestValidateAsync(model);

        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [TestCase("")]
    [TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKAA")]
    public async Task ValidateAsync_InvalidFirstName_ShouldThrowValidationErrors(string value)
    {
        var model = new ClientDto { FirstName = value, LastName = "Doe" };

        var result = await validator.TestValidateAsync(model);

        result.IsValid.ShouldBeFalse();
        result.ShouldHaveValidationErrorFor("FirstName");
    }

    [Test]
    public async Task ValidateAsync_NullFirstName_ShouldThrowValidationErrors()
    {
        var model = new ClientDto { FirstName = null!, LastName = "Doe" };

        var result = await validator.TestValidateAsync(model);

        result.IsValid.ShouldBeFalse();
        result.ShouldHaveValidationErrorFor("FirstName");
    }

    #endregion

    #region LastName

    [TestCase("A")]
    [TestCase("Doe")]
    [TestCase("Doe Kowalski")]
    [TestCase("Doe Kowalski2222")]
    [TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKA")]
    public async Task ValidateAsync_ValidLastName_ShouldBeValid(string value)
    {
        var model = new ClientDto { FirstName = "John", LastName = value };

        var result = await validator.TestValidateAsync(model);

        result.IsValid.ShouldBeTrue();
        result.Errors.ShouldBeEmpty();
    }

    [TestCase("")]
    [TestCase("ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKAA")]
    public async Task ValidateAsync_InvalidLastName_ShouldThrowValidationErrors(string value)
    {
        var model = new ClientDto { FirstName = "John", LastName = value };

        var result = await validator.TestValidateAsync(model);

        result.IsValid.ShouldBeFalse();
        result.ShouldHaveValidationErrorFor("LastName");
    }

    [Test]
    public async Task ValidateAsync_NullLastName_ShouldThrowValidationErrors()
    {
        var model = new ClientDto { FirstName = "John", LastName = null! };

        var result = await validator.TestValidateAsync(model);

        result.IsValid.ShouldBeFalse();
        result.ShouldHaveValidationErrorFor("LastName");
    }

    #endregion

    #region Email

    [TestCase("plainaddress")]
    [TestCase("@domain.com")]
    [TestCase("user@")]
    public async Task ValidateAsync_InvalidEmail_ShouldThrowValidationErrors(string email)
    {
        var client = new ClientDto { FirstName = "John", LastName = "Doe", Email = email };

        var result = await validator.TestValidateAsync(client);

        result.IsValid.ShouldBeFalse();
        result.ShouldHaveValidationErrorFor(clientDto => clientDto.Email);
    }

    [TestCase("")]
    public async Task ValidateAsync_EmptyEmailString_ShouldThrowValidationErrors(string email)
    {
        var client = new ClientDto { FirstName = "John", LastName = "Doe", Email = email };

        var result = await validator.TestValidateAsync(client);

        result.IsValid.ShouldBeFalse();
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [TestCase("test@example.com")]
    [TestCase("user.name@domain.co")]
    [TestCase("user@subdomain.domain.com")]
    [TestCase("user+tag@domain.com")]
    [TestCase("user@domain.name")]
    public async Task ValidateAsync_ValidEmail_ShouldBeValid(string email)
    {
        var client = new ClientDto { FirstName = "John", LastName = "Doe", Email = email };

        var result = await validator.TestValidateAsync(client);

        result.IsValid.ShouldBeTrue();
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    #endregion

    #region PhoneNumber

    [TestCase("+48123456789")]
    public async Task ValidateAsync_ValidPhoneNumber_ShouldBeValid(string value)
    {
        var client = new ClientDto { FirstName = "John", LastName = "Doe", PhoneNumber = value };

        var result = await validator.TestValidateAsync(client);

        result.IsValid.ShouldBeTrue();
        result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [TestCase("123456789")]
    [TestCase("1234567890")]
    [TestCase("+481234567890")]
    [TestCase("+48123")]
    [TestCase("0")]
    public async Task ValidateAsync_InvalidPhoneNumber_ShouldThrowValidationErrors(string value)
    {
        var client = new ClientDto { FirstName = "John", LastName = "Doe", PhoneNumber = value };

        var result = await validator.TestValidateAsync(client);

        result.IsValid.ShouldBeFalse();
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    #endregion
}
