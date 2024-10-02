using Smirnov_AP_kt_42_21.Models;
using Smirnov_AP_kt_42_21.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Smirnov_AP_kt_42_21.Database
{
    public class SmirnovDbContext : DbContext
    {
        public DbSet<EducationalSubject> EducationalSubjects { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Workload> Workloads { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EducationSubjectConfiguration());
            modelBuilder.ApplyConfiguration(new ProfessorConfiguration());
            modelBuilder.ApplyConfiguration(new WorkloadConfiguration());
        }
        public SmirnovDbContext(DbContextOptions options) : base(options)
        {
        }
    }
    public class SmirnovDbContextFactory : IDesignTimeDbContextFactory<SmirnovDbContext>
    {
        public SmirnovDbContext CreateDbContext(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var optionsBuilder = new DbContextOptionsBuilder<SmirnovDbContext>();
            optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            return new SmirnovDbContext(optionsBuilder.Options);
        }
    }
}