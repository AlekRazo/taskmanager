using Microsoft.EntityFrameworkCore;
using TaskManager.Models.Domain;

namespace TaskManager.Models;

public class TaskManagerDbContext : DbContext
{
    public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options) {}

    public DbSet<TaskModel> Tasks { get; set; }
    public DbSet<UserModel> Users { get; set; }
    public DbSet<PriorityModel> Priorities { get; set; }
    public DbSet<StatusModel> Status { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PriorityModel>(entity =>
        {
            entity.ToTable("priority");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Priority)
                .HasColumnName("priority")
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();
        }); 

        modelBuilder.Entity<StatusModel>(entity =>
        {
            entity.ToTable("status");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();
        });

        modelBuilder.Entity<UserModel>(entity =>
        {
            entity.ToTable("user");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.UserName)
                .HasColumnName("username")
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();
            entity.Property(e => e.Password)
                .HasColumnName("password")
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsRequired();
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();
        });

        modelBuilder.Entity<TaskModel>(entity =>
        {
            entity.ToTable("task", "dbo", tb => tb.HasTrigger("TRG_during_update_task"));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.Title)
                .HasColumnName("title")
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();
            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PriorityId)
                .HasColumnName("priority_id")
                .IsRequired();
            entity.Property(e => e.CreateDate)
                .HasColumnName("create_date")
                .HasColumnType("date")
                .IsRequired();
            entity.Property(e => e.FinishDate)
                .HasColumnName("finish_date")
                .HasColumnType("date");
            entity.Property(e => e.LimitDate)
                .HasColumnName("limit_date")
                .HasColumnType("date");
            entity.Property(e => e.StatusId)
                .HasColumnName("status_id")
                .IsRequired();
            entity.Property(e => e.UserId)
                .HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            entity.HasOne(e => e.Priority)
                .WithMany(p => p.Tasks)
                .HasForeignKey(e => e.PriorityId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_task_priority");

            entity.HasOne(e => e.Status)
                .WithMany(s => s.Tasks)
                .HasForeignKey(e => e.StatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_task_status");

            entity.HasOne(e => e.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_task_user");
        });
    }
}