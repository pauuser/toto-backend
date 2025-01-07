using Toto.Contracts.Models;

namespace Toto.Contracts;

public abstract class RequestResult
{
    public bool IsSuccess { get; init; } = true;

    public ErrorContractDto? Error { get; init; } = null;
}