using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace TS.API.Interceptors
{
    public class QueryTimerInterceptor : DbCommandInterceptor
    {
        private readonly ILogger<QueryTimerInterceptor> _logger;

        public QueryTimerInterceptor(ILogger<QueryTimerInterceptor> logger)
        {
            _logger = logger;
        }

        // This method runs AFTER the SQL command finishes
        public override ValueTask<DbDataReader> ReaderExecutedAsync(
            DbCommand command,
            CommandExecutedEventData eventData,
            DbDataReader result,
            CancellationToken cancellationToken = default)
        {
            // eventData.Duration tells us exactly how long the SQL took
            double duration = eventData.Duration.TotalMilliseconds;

            // For 600k users, we want to know if a query is slow
            if (duration > 500)
            {
                _logger.LogWarning("SLOW QUERY DETECTED: {Duration}ms | SQL: {Sql}",
                    duration, command.CommandText);
            }
            else
            {
                _logger.LogInformation("SQL Execution: {Duration}ms", duration);
            }

            return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
        }

        // Logic for counting rows (Simplified for production stability)
        public override DbDataReader ReaderExecuted(
            DbCommand command,
            CommandExecutedEventData eventData,
            DbDataReader result)
        {
            // Note: Full row counting requires a custom DbDataReader wrapper. 
            // For now, we log the Command and the Duration.
            return base.ReaderExecuted(command, eventData, result);
        }
    }
}