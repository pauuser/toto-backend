using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Toto.Extensions;

public static class DependencyInjectionExtensions
{
    [return: NotNull]
    public static T ThrowIfNull<T>(this T t, [CallerArgumentExpression("t")] string? paramName = null) where T : class?
    {
        ArgumentNullException.ThrowIfNull(argument: t, paramName: paramName);
        return t;
    }
}