using Microsoft.EntityFrameworkCore;
using VoucherManagementApi.DataAccess.Entities;

namespace VoucherManagementApi.DataAccess.Application
{
    public partial class BaseDbContext : DbContext
    {
        public BaseDbContext()
        {
        }

        public BaseDbContext(DbContextOptions<BaseDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<VmProduct> VmProducts { get; set; }
        public virtual DbSet<VmVoucher> GetVmVouchers { get; set; }
        public virtual DbSet<VmVoucherRules> VmVoucherRules{ get; set; }
        public virtual EntityState GetState(object entity)
        {
            return Entry(entity).State;
        }
        public virtual void SetState(object entity, EntityState state)
        {
            Entry(entity).State = state;
        }

        public virtual void SetEntityEntryValues(object currentEntity, object entity)
        {
            Entry(currentEntity).CurrentValues.SetValues(entity);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=localhost;user=sa;password=password;database=lb-glossary", providerOptions => providerOptions.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
