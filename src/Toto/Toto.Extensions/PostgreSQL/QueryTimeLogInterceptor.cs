using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Toto.Extensions.DI;

namespace Toto.Extensions.PostgreSQL;

public class QueryTimeLogInterceptor(ILogger<QueryTimeLogInterceptor> logger, QueryTimeLogOptions options): DbCommandInterceptor
{
    private readonly ILogger<QueryTimeLogInterceptor> _logger = logger.ThrowIfNull();
    private readonly QueryTimeLogOptions _options = options.ThrowIfNull();

    public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Duration.TotalMilliseconds > _options.SlowQueryThresholdMs) {
            _logger.LogWarning("Slow query: ({QueryTime} ms): {QueryText}", eventData.Duration.TotalMilliseconds, command.CommandText);
        }
        else
        {
            _logger.LogInformation("Query completed: ({QueryTime} ms): {QueryText}", eventData.Duration.TotalMilliseconds, command.CommandText);
        }

        return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }
}