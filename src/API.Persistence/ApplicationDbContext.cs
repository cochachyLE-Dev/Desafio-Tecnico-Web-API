using API.Domain.Entities;
using API.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking; 
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
        public async Task SetValues<TEntity>(TEntity entity)
        {
            await Task.Run(() =>base.Entry(entity).CurrentValues.SetValues(entity));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Order>().HasKey(x => x.Id);
            modelBuilder.Entity<Order>().Property(p => p.Id).ValueGeneratedNever();

            modelBuilder.Entity<OrderDetail>().HasKey(x => new { x.OrderId, x.ProductId });
            modelBuilder.Entity<Client>().HasKey(x => x.Id);
            modelBuilder.Entity<Product>().HasKey(x => x.Id);

            modelBuilder.Entity<Order>().HasOne<Client>(x => x.Client);
            modelBuilder.Entity<Order>().HasMany<OrderDetail>(x => x.Details).WithOne();
            modelBuilder.Entity<OrderDetail>().HasOne<Order>().WithMany(d => d.Details).HasForeignKey(o =>o.OrderId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderDetail>().HasOne<Product>(x => x.Product);

            modelBuilder.Entity<Client>().Ignore(x => x.FullName);

            modelBuilder.Entity<Client>().HasData(DefaultClients.ClientList());
            modelBuilder.Entity<Product>().HasData(DefaultProducts.ProductList());
        }
    }
}
