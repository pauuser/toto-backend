using System.ComponentModel.DataAnnotations;

namespace Toto.Extensions.PostgreSQL;

public class QueryTimeLogOptions
{
    public const string ConfigurationSectionName = "QueryTimeLogOptions";
    
    [Required]
    public bool Logging { get; set; }
    
    [Required]
    public int SlowQueryThresholdMs { get; set; }
}