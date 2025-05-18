using Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.Property(d => d.SubTotal).HasColumnType("decimal(8,2)");
            builder.HasMany(o => o.Items).WithOne();
            builder.OwnsOne(o => o.Address).WithOwner(); //include the address in the order table
        }
    }
}
