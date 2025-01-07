using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Toto.AuthService.DataAccess.Context.Configuration;
using Toto.AuthService.DataAccess.Models;
using Toto.Extensions.DI;
using Toto.Extensions.PostgreSQL;

namespace Toto.AuthService.DataAccess.Context;

public class AuthDbContext : DbContext
{
    public virtual DbSet<TokensDb> Tokens { get; set; }
    
    private readonly ILogger<QueryTimeLogInterceptor> _queryTimeLogger;
    private readonly QueryTimeLogOptions _logQueryTimeOptions;

    public AuthDbContext()
    {
    }
    
    public AuthDbContext(ILogger<QueryTimeLogInterceptor> queryTimeLogger, 
        IOptions<QueryTimeLogOptions> logQueryTimeOptions)
    {
        _queryTimeLogger = queryTimeLogger.ThrowIfNull();
        _logQueryTimeOptions = logQueryTimeOptions?.Value.ThrowIfNull();
    }
    
    public AuthDbContext(DbContextOptions<AuthDbContext> options, 
        ILogger<QueryTimeLogInterceptor> queryTimeLogger, 
        IOptions<QueryTimeLogOptions> logQueryTimeOptions) : base(options)
    {
        _queryTimeLogger = queryTimeLogger.ThrowIfNull();
        _logQueryTimeOptions = logQueryTimeOptions.Value.ThrowIfNull();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_logQueryTimeOptions.Logging)
        {
            optionsBuilder.AddInterceptors(new QueryTimeLogInterceptor(_queryTimeLogger, _logQueryTimeOptions));    
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TokensConfiguration).Assembly);
    }
}