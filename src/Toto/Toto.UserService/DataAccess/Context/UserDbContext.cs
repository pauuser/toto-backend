using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Toto.Extensions;
using Toto.Extensions.DI;
using Toto.Extensions.PostgreSQL;
using Toto.UserService.DataAccess.Configuration;
using Toto.UserService.DataAccess.Models;

namespace Toto.UserService.DataAccess.Context;

public class UserDbContext : DbContext
{
    public virtual DbSet<UserDb> Users { get; set; }
    
    private readonly ILogger<QueryTimeLogInterceptor> _queryTimeLogger;
    private readonly QueryTimeLogOptions _logQueryTimeOptions;

    public UserDbContext()
    {
    }
    
    public UserDbContext(ILogger<QueryTimeLogInterceptor> queryTimeLogger, 
        IOptions<QueryTimeLogOptions> logQueryTimeOptions)
    {
        _queryTimeLogger = queryTimeLogger.ThrowIfNull();
        _logQueryTimeOptions = logQueryTimeOptions?.Value.ThrowIfNull();
    }
    
    public UserDbContext(DbContextOptions<UserDbContext> options, 
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
    }
}