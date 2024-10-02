using Smirnov_AP_kt_42_21.Database.Helpers;
using Smirnov_AP_kt_42_21.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Smirnov_AP_kt_42_21.Database.Configurations
{
    public class WorkloadConfiguration : IEntityTypeConfiguration<Workload>
    {
        private const string TableName = "workload";
        public void Configure(EntityTypeBuilder<Workload> builder)
        {
            builder
                .HasKey(e => e.Id)
                .HasName($"pk_{TableName}_id");
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasComment("Идентификатор нагрузки");
            builder.Property(e => e.NumberOfHours)
                .IsRequired()
                .HasColumnName("numberofhours")
                .HasColumnType(ColumnType.Int)
                .HasComment("Количество часов нагрузки");
            builder.Property(e => e.ProfessorId)
                .IsRequired()
                .HasColumnName("professor_id")
                .HasColumnType(ColumnType.Int)
                .HasComment("Идентификатор профессора");
            builder.Property(e => e.EducationalSubjectId)
                .IsRequired()
                .HasColumnName("educationsubject_id")
                .HasColumnType(ColumnType.Int)
                .HasComment("Идентификатор дисциплины");
            builder.ToTable(TableName)
                .HasOne(e => e.Professor)
                .WithMany()
                .HasForeignKey(e => e.ProfessorId)
                .HasConstraintName("fk_f_professor_id")
                .OnDelete(DeleteBehavior.Cascade);
            builder.ToTable(TableName)
                .HasOne(e => e.EducationalSubject)
                .WithMany()
                .HasForeignKey(e => e.EducationalSubjectId)
                .HasConstraintName("fk_f_educationalsubject_id")
                .OnDelete(DeleteBehavior.Cascade);
            builder.Navigation(e => e.Professor)
                .AutoInclude();
            builder.Navigation(e => e.EducationalSubject)
                .AutoInclude();
        }
    }
}