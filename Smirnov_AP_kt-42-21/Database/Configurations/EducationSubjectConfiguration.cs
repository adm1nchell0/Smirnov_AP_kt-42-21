using Smirnov_AP_kt_42_21.Database.Helpers;
using Smirnov_AP_kt_42_21.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Smirnov_AP_kt_42_21.Database.Configurations
{
    public class EducationSubjectConfiguration : IEntityTypeConfiguration<EducationalSubject>
    {
        private const string TableName = "educationalsubject";
        public void Configure(EntityTypeBuilder<EducationalSubject> builder)
        {
            builder
                .HasKey(e => e.Id)
                .HasName($"pk_{TableName}_id");
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasComment("Идентификатор дисциплины");
            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasColumnType(ColumnType.String).HasMaxLength(100)
                .HasComment("Наименование дисциплины");
            builder.ToTable(TableName);
        }
    }
}