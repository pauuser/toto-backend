namespace Toto.Contracts.Models;

public enum ErrorContractDto
{
    #region Common errors

    InternalServerError = 0,

    #endregion
    
    #region Toto.AuthService errors
    
    TokensNotFound = 1,
    InvalidToken = 2,
    
    #endregion
    
    #region Toto.UserService errors
    
    #endregion
}