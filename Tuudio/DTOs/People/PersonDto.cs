﻿namespace Tuudio.DTOs.People;

public abstract record PersonDto
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }
}
