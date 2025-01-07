using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Toto.Extensions.DI;

public static class DependencyInjectionExtensions
{
    [return: NotNull]
    public static T ThrowIfNull<T>(this T t, [CallerArgumentExpression("t")] string? paramName = null) where T : class?
    {
        ArgumentNullException.ThrowIfNull(argument: t, paramName: paramName);
        return t;
    }
    
    public static IServiceCollection AddValidatedOption<T>(this IServiceCollection collection, string section) where T : class
    {
        collection.AddOptions<T>()
            .BindConfiguration(section)
            .ValidateOnStart()
            .ValidateDataAnnotations()
            .Validate<IConfiguration>((opt, configuration) =>
        {
            var properties = typeof(T).GetProperties();
            foreach (var prop in properties)
            {
                if (prop.PropertyType.IsValueType &&
                    prop.CustomAttributes.Any(x => x.AttributeType == typeof(RequiredAttribute)))
                {
                    var value = configuration.GetValue(prop.PropertyType, string.Join(':', section, prop.Name));
                    if (value == null)
                    {
                        throw new ValidationException($"The \"{prop.Name}\' field is missing in the \"{section}\" section in configuration of the project");
                    }
                }
            }
            return true;
        });
        return collection;
    }
}