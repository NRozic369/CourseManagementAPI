using CourseManagementAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.DataAccessLayer
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attendee> Attendees { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendee>(entity =>
            {
                entity.HasIndex(e => e.CourseId, "IX_Attendees_CourseId");

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Attendees)
                    .HasForeignKey(d => d.CourseId);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.CourseDescription).HasMaxLength(4000);

                entity.Property(e => e.CourseTeacher)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.CourseTeacherEmail)
                    .HasMaxLength(150)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.CourseTitle).HasMaxLength(150);

                entity.Property(e => e.EditDeleteCoursePin)
                    .HasColumnName("EditDeleteCoursePIN")
                    .HasDefaultValueSql("(N'')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
