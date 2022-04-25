using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Models.Models;

#nullable disable

namespace WebApi
{
    public partial class UniversityContext : DbContext
    {
        public UniversityContext()
        {
        }

        public UniversityContext(DbContextOptions<UniversityContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<HomeTask> HomeTasks { get; set; }
        public virtual DbSet<HomeTaskAssessment> HomeTaskAssessments { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<HomeTask>(entity =>
            {
                entity.HasIndex(e => e.CourseId, "IX_HomeTasks_CourseId");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.HomeTasks)
                    .HasForeignKey(d => d.CourseId);
            });

            modelBuilder.Entity<HomeTaskAssessment>(entity =>
            {
                entity.ToTable("HomeTaskAssessment");

                entity.HasIndex(e => e.HomeTaskId, "IX_HomeTaskAssessment_HomeTaskId");

                entity.HasIndex(e => e.StudentId, "IX_HomeTaskAssessment_StudentId");

                entity.HasOne(d => d.HomeTask)
                    .WithMany(p => p.HomeTaskAssessments)
                    .HasForeignKey(d => d.HomeTaskId);

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.HomeTaskAssessments)
                    .HasForeignKey(d => d.StudentId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}