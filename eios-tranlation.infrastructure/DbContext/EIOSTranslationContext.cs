namespace eios_translation.infrastructure.DbContext
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using eios_translation.core.Common;
    using eios_translation.infrastructure.EntityClass;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Db Context Class for EIOSTranslationContext.
    /// </summary>
    public class EIOSTranslationContext : DbContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public EIOSTranslationContext(DbContextOptions<EIOSTranslationContext> options)
            : base(options)
        {
        }

        public EIOSTranslationContext(DbContextOptions<EIOSTranslationContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets or sets Labels.
        /// </summary>
        public virtual DbSet<LabelGroup> LabelGroups { get; set; }

        /// <summary>
        /// Gets or sets Labels.
        /// </summary>
        public virtual DbSet<Language> Languages { get; set; }


        /// <summary>
        /// Gets or sets Labels.
        /// </summary>
        public virtual DbSet<Label> Labels { get; set; }

        /// <summary>
        /// Overridden implmentation of context.SaveChanges().
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">AcceptAllChangesOnSuccess.</param>
        /// <returns>Affected Rows.</returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// Overridden implmentation of context.SaveChangesAsync().
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">AcceptAllChangesOnSuccess.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns>Affected Rows.</returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// OnModelCreating Override.
        /// </summary>
        /// <param name="modelBuilder">Model Builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Remove all the deleted sso methods from include.
            //modelBuilder.Entity<Label>().HasQueryFilter(x => !x.IsActive);
        }

        /// <summary>
        /// Fill the Audit Properties.
        /// </summary>
        private void OnBeforeSaving()
        {
            // TO DO: After authentication mechanism is identified, logged in profileId should be fetched from user identity.
            var currentLoginId = -1;
            var entries = this.ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is ITrackable trackable)
                {
                    var now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedAt = now;
                            trackable.UpdatedBy = currentLoginId;
                            break;

                        case EntityState.Added:
                            trackable.CreatedAt = now;
                            trackable.CreatedBy = currentLoginId;
                            trackable.UpdatedAt = now;
                            trackable.UpdatedBy = currentLoginId;
                            break;
                    }
                }
            }
        }
    }
    
}
