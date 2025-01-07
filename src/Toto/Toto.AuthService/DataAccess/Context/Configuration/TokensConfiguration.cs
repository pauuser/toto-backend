using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Toto.AuthService.DataAccess.Models;

namespace Toto.AuthService.DataAccess.Context.Configuration;

public class TokensConfiguration : IEntityTypeConfiguration<TokensDb>
{
    public void Configure(EntityTypeBuilder<TokensDb> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();
    }
}