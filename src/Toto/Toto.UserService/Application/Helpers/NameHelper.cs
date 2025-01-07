using System;
using System.Collections.Generic;

namespace Toto.UserService.Application.Helpers;

public static class NameHelper
{
    private static readonly List<string> Names = [ "John", "William", "Victor", "Alex", "Mary", "Suzy" ];
    private static readonly List<string> Surnames = [ "Smith", "Vinny", "Blau", "Waldron" ];
     
    public static string CreateRandomFirstName()
    {
        var random = new Random();
        
        return Names[random.Next(Names.Count)];
    }
    
    public static string CreateRandomLastName()
    {
        var random = new Random();
        
        return Surnames[random.Next(Names.Count)];
    }
}