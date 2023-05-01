using System;

namespace Domain.Entities;

public class Profile : Entity
{
    public Guid UserId { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Country { get; set; }
}