using System.Runtime.Serialization;

namespace Toto.ApiGateway.Models;

/// <summary>
///     Provider of auth data for the application
/// </summary>
public enum AuthProviderDto
{
    /// <summary>
    ///     Auth via Apple
    /// </summary>
    [EnumMember(Value = "apple")]
    Apple = 0,
    
    /// <summary>
    ///     Auth via Google
    /// </summary>
    [EnumMember(Value = "google")]
    Google = 1
}