using EntityFrameworkCore.CommonTools;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shop.Models.Entities;
using Shops = Shop.Models.Entities.Shop;

namespace Shop.DAL
{
    public  class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Shops> Shops { get; set; }
        public DbSet<Product> Products { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder builder) 
        { 
            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.UpdateTrackableEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity) where TEntity : BaseEntity<int>
        {
            var entityEntry = await base.AddAsync(entity);
            await SaveChangesAsync();

            return entityEntry;
        }

        public async ValueTask<EntityEntry<TEntity>> RemoveAsync<TEntity>(TEntity entity) where TEntity : BaseEntity<int>
        {
            var entityEntry = base.Remove(entity);
            await SaveChangesAsync();
            return entityEntry;
        }

        public async ValueTask<EntityEntry<TEntity>> UpdateAsync<TEntity>(TEntity entity) where TEntity : BaseEntity<int>
        {
            var entityEntry = base.Update(entity);
            await SaveChangesAsync();
            return entityEntry;
        }
    }
}
