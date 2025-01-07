using System.Runtime.Serialization;

namespace Toto.Contracts.Models;

public enum AuthProviderContractDto
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