using System;

namespace Toto.UserService.DataAccess.Models;

public class UserDb
{
    public Guid Id { get; set; }
    
    public string Email { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime RegisteredAtUtc { get; set; }

    public UserDb()
    {
    }

    public UserDb(Guid id, string email, string firstName, string lastName, DateTime registeredAtUtc)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        RegisteredAtUtc = registeredAtUtc;
    }
}