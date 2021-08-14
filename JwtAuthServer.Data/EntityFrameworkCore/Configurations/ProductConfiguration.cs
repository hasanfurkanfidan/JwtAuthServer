using JwtAuthServer.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthServer.Data.EntityFrameworkCore.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(250);
            builder.Property(p => p.Stock).IsRequired();

            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p => p.UserId).IsRequired();
        }
    }
}
