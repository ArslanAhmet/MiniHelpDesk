using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniHelpDesk.Services.TicketManagement.Core.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace MiniHelpDesk.Services.TicketManagement.Infrastructure.Data
{
    public class TicketContext : DbContext
    {
        public TicketContext(DbContextOptions<TicketContext> options)
           : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Ticket>(ConfigureTicket);
            
        }

        private void ConfigureTicket(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets");
            builder.HasKey(ci => ci.Id);
            builder.Property(cb => cb.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(cb => cb.Description)
               .HasMaxLength(500);
   
            builder.Property(cb => cb.IsDeleted);
            builder.Property(cb => cb.Created)
                .IsRequired();
            builder.Property(cb => cb.Modified);
            builder.HasQueryFilter(p => !p.IsDeleted);
        }
        public async Task<int> SaveChangesAsync()
        {
            Audit();
            return await base.SaveChangesAsync();
        }

        private void Audit()
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((BaseEntity)entry.Entity).Created = DateTime.Now;
                }
                else
                {
                    ((BaseEntity)entry.Entity).Modified = DateTime.Now;
                }

            }
        }
    }
}
