﻿namespace Tuudio.DTOs.People.Detailed;

public abstract record PersonDetailedDto : DbObjectDto
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }
}
