namespace SoftGym.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using SoftGym.Data.Common.Models;
    using SoftGym.Data.Models;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ApplicationDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<ClientTrainer> ClientsTrainers { get; set; }

        public DbSet<EatingPlan> EatingPlans { get; set; }

        public DbSet<Meal> Meals { get; set; }

        public DbSet<MealPlan> MealsPlans { get; set; }

        public DbSet<WorkoutTrainingDay> WorkoutsTrainingDays { get; set; }

        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }

        public DbSet<Facility> Facilities { get; set; }

        public DbSet<FoodPreference> FoodPreferences { get; set; }

        public DbSet<MealPreference> MealsPreferences { get; set; }

        public DbSet<TrainingDay> TrainingDays { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Message> Messages { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            this.ConfigureUserIdentityRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            // Configure table relations
            builder
                .Entity<Card>()
                .HasOne(c => c.User)
                .WithOne(u => u.Card)
                .HasForeignKey<ApplicationUser>(u => u.CardId);

            builder
               .Entity<ClientTrainer>()
               .HasKey(x => new { x.ClientId, x.TrainerId });

            builder
                .Entity<ClientTrainer>()
                .HasOne(ct => ct.Client)
                .WithMany(c => c.Trainers)
                .HasForeignKey(ct => ct.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<ClientTrainer>()
                .HasOne(ct => ct.Trainer)
                .WithMany(t => t.Clients)
                .HasForeignKey(ct => ct.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<EatingPlan>()
                .HasOne(ep => ep.User)
                .WithMany(u => u.EatingPlans)
                .HasForeignKey(ep => ep.UserId);

            builder
                .Entity<MealPlan>()
                .HasKey(mp => new { mp.MealId, mp.EatingPlanId });

            builder
                .Entity<MealPlan>()
                .HasOne(mp => mp.EatingPlan)
                .WithMany(ep => ep.Meals)
                .HasForeignKey(mp => mp.EatingPlanId);

            builder
                .Entity<MealPlan>()
                .HasOne(mp => mp.Meal)
                .WithMany(m => m.Plans)
                .HasForeignKey(mp => mp.MealId);

            builder
                .Entity<WorkoutTrainingDay>()
                .HasKey(x => new { x.TrainingDayId, x.ExerciseId });

            builder
                .Entity<WorkoutTrainingDay>()
                .HasOne(we => we.TrainingDay)
                .WithMany(wp => wp.Exercises)
                .HasForeignKey(we => we.TrainingDayId);

            builder
                .Entity<WorkoutTrainingDay>()
                .HasOne(we => we.Exercise)
                .WithMany(e => e.WorkoutPlans)
                .HasForeignKey(we => we.ExerciseId);

            builder
                .Entity<MealPreference>()
                .HasKey(x => new { x.MealId, x.PreferenceId });

            builder
                .Entity<MealPreference>()
                .HasOne(mp => mp.Preference)
                .WithMany(p => p.Meals)
                .HasForeignKey(mp => mp.PreferenceId);

            builder
                .Entity<MealPreference>()
                .HasOne(mp => mp.Meal)
                .WithMany(m => m.FoodPreferences)
                .HasForeignKey(mp => mp.MealId);

            builder
                .Entity<Appointment>()
                .HasOne(a => a.Trainer)
                .WithMany(t => t.TrainerAppointments)
                .HasForeignKey(a => a.TrainerId);

            builder
                .Entity<Appointment>()
                .HasOne(a => a.Client)
                .WithMany(c => c.ClientAppointments)
                .HasForeignKey(a => a.ClientId);

            builder
                .Entity<Message>()
                .HasOne(m => m.Reciever)
                .WithMany(r => r.RecievedMessages)
                .HasForeignKey(m => m.RecieverId);

            builder
                .Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(s => s.SentMessages)
                .HasForeignKey(m => m.SenderId);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        // Applies configurations
        private void ConfigureUserIdentityRelations(ModelBuilder builder)
             => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
