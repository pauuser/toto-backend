using System;
using System.ComponentModel.DataAnnotations;

namespace Toto.AuthService.Domain.Configuration;

public class JwtTokenConfiguration
{
    public static readonly string ConfigurationSectionName = "JwtTokenConfiguration";
    
    /// <summary>
    ///     Audience
    /// </summary>
    [Required]
    public required string Audience { get; set; }
    
    /// <summary>
    ///     Issuer
    /// </summary>
    [Required]
    public required string Issuer { get; set; }
    
    /// <summary>
    ///     Access token lifetime
    /// </summary>
    [Required]
    public required TimeSpan AccessLifetimeTimeSpan { get; set; }
    
    /// <summary>
    ///     Refresh token lifetime
    /// </summary>
    [Required]
    public required int RefreshLifetimeDays { get; set; }
    
    /// <summary>
    ///     Secret key (should not be exposed!)
    /// </summary>
    [Required]
    public required string Key { get; init; }
    
    /// <summary>
    ///     Allowed time skew when comparing
    /// </summary>
    [Required]
    public required int ClockSkewSeconds { get; init; }
}