using Microsoft.EntityFrameworkCore;
using eUIT.API.Models;

namespace eUIT.API.Data;

public class eUITDbContext : DbContext
{
    public eUITDbContext(DbContextOptions<eUITDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DbSet cho quản lý Refresh Tokens
    /// Bảng này sẽ được tạo thủ công trong database bằng script SQL
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    // ========== THÊM 5 DbSets MỚI ==========
    
    /// <summary>
    /// DbSet cho quản lý các buổi báo bù
    /// </summary>
    public DbSet<Makeup> Makeups { get; set; }

    /// <summary>
    /// DbSet cho quản lý các buổi báo nghỉ
    /// </summary>
    public DbSet<Absence> Absences { get; set; }

    /// <summary>
    /// DbSet cho quản lý các thông báo gửi cho lớp
    /// </summary>
    public DbSet<Announcement> Announcements { get; set; }

    /// <summary>
    /// DbSet cho quản lý lịch thi
    /// </summary>
    public DbSet<Exam> Exams { get; set; }

    /// <summary>
    /// DbSet cho quản lý lịch họp
    /// </summary>
    public DbSet<Meeting> Meetings { get; set; }

    // ========== END DBSETS ==========

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ========== REFRESH TOKEN CONFIG ==========
        // Cấu hình cho RefreshToken entity
        // Lưu ý: Bảng và index đã được tạo sẵn trong database bằng script SQL
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            // Chỉ định tên bảng (đã tồn tại)
            entity.ToTable("refresh_tokens");
            
            // Mapping các cột (tên cột đã được định nghĩa trong attributes)
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TokenHash).HasColumnName("token_hash");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserRole).HasColumnName("user_role");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.IsRevoked).HasColumnName("is_revoked");
            entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");
            entity.Property(e => e.DeviceInfo).HasColumnName("device_info");
            entity.Property(e => e.IpAddress).HasColumnName("ip_address");
        });

        // ========== MAKEUP CONFIG ==========
        // Cấu hình cho Makeup entity (Báo bù)
        modelBuilder.Entity<Makeup>(entity =>
        {
            entity.ToTable("makeup_classes");
            entity.HasKey(e => e.Id);
            
            // Mapping properties to columns
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.GiangVienId)
                .HasColumnName("giang_vien_id")
                .IsRequired();
            
            entity.Property(e => e.ClassId)
                .HasColumnName("class_id")
                .IsRequired();
            
            entity.Property(e => e.MakeupDate)
                .HasColumnName("makeup_date")
                .IsRequired();
            
            entity.Property(e => e.RoomId)
                .HasColumnName("room_id");
            
            entity.Property(e => e.Reason)
                .HasColumnName("reason")
                .HasMaxLength(500);
            
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue("submitted")
                .HasMaxLength(20);
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");
            
            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at");

            // Create indexes for performance
            entity.HasIndex(e => e.GiangVienId)
                .HasDatabaseName("ix_makeup_giang_vien");
            
            entity.HasIndex(e => e.ClassId)
                .HasDatabaseName("ix_makeup_class");
            
            entity.HasIndex(e => e.CreatedAt)
                .HasDatabaseName("ix_makeup_created_at");
        });

        // ========== ABSENCE CONFIG ==========
        // Cấu hình cho Absence entity (Báo nghỉ)
        modelBuilder.Entity<Absence>(entity =>
        {
            entity.ToTable("absences");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.GiangVienId)
                .HasColumnName("giang_vien_id")
                .IsRequired();
            
            entity.Property(e => e.ClassId)
                .HasColumnName("class_id")
                .IsRequired();
            
            entity.Property(e => e.AbsenceDate)
                .HasColumnName("absence_date")
                .IsRequired();
            
            entity.Property(e => e.Reason)
                .HasColumnName("reason")
                .HasMaxLength(500);
            
            entity.Property(e => e.Type)
                .HasColumnName("type")
                .HasDefaultValue("personal")
                .HasMaxLength(20);
            
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue("submitted")
                .HasMaxLength(20);
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");
            
            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at");

            // Create indexes
            entity.HasIndex(e => e.GiangVienId)
                .HasDatabaseName("ix_absence_giang_vien");
            
            entity.HasIndex(e => e.ClassId)
                .HasDatabaseName("ix_absence_class");
            
            entity.HasIndex(e => e.CreatedAt)
                .HasDatabaseName("ix_absence_created_at");
        });

        // ========== ANNOUNCEMENT CONFIG ==========
        // Cấu hình cho Announcement entity (Thông báo)
        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.ToTable("announcements");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.ClassId)
                .HasColumnName("class_id")
                .IsRequired();
            
            entity.Property(e => e.Title)
                .HasColumnName("title")
                .IsRequired()
                .HasMaxLength(255);
            
            entity.Property(e => e.Content)
                .HasColumnName("content")
                .HasMaxLength(5000);
            
            entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.PublishedDate)
                .HasColumnName("published_date");
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            // Create indexes
            entity.HasIndex(e => e.ClassId)
                .HasDatabaseName("ix_announcement_class");
            
            entity.HasIndex(e => e.CreatedBy)
                .HasDatabaseName("ix_announcement_created_by");
            
            entity.HasIndex(e => e.PublishedDate)
                .HasDatabaseName("ix_announcement_published_date");
        });

        // ========== EXAM CONFIG ==========
        // Cấu hình cho Exam entity (Lịch thi)
        modelBuilder.Entity<Exam>(entity =>
        {
            entity.ToTable("exams");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.ClassId)
                .HasColumnName("class_id")
                .IsRequired();
            
            entity.Property(e => e.ExamDate)
                .HasColumnName("exam_date")
                .IsRequired();
            
            entity.Property(e => e.Room)
                .HasColumnName("room")
                .HasMaxLength(50);
            
            entity.Property(e => e.Invigilators)
                .HasColumnName("invigilators");
            
            entity.Property(e => e.Duration)
                .HasColumnName("duration");
            
            entity.Property(e => e.Semester)
                .HasColumnName("semester")
                .HasMaxLength(10);
            
            entity.Property(e => e.AcademicYear)
                .HasColumnName("academic_year")
                .HasMaxLength(10);

            // Create indexes
            entity.HasIndex(e => e.ClassId)
                .HasDatabaseName("ix_exam_class");
            
            entity.HasIndex(e => e.ExamDate)
                .HasDatabaseName("ix_exam_date");
            
            entity.HasIndex(e => new { e.Semester, e.AcademicYear })
                .HasDatabaseName("ix_exam_semester_year");
        });

        // ========== MEETING CONFIG ==========
        // Cấu hình cho Meeting entity (Lịch họp)
        modelBuilder.Entity<Meeting>(entity =>
        {
            entity.ToTable("meetings");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.Title)
                .HasColumnName("title")
                .IsRequired()
                .HasMaxLength(255);
            
            entity.Property(e => e.Description)
                .HasColumnName("description");
            
            entity.Property(e => e.MeetingDate)
                .HasColumnName("meeting_date")
                .IsRequired();
            
            entity.Property(e => e.Location)
                .HasColumnName("location")
                .HasMaxLength(255);
            
            entity.Property(e => e.Duration)
                .HasColumnName("duration");

            // Create indexes
            entity.HasIndex(e => e.MeetingDate)
                .HasDatabaseName("ix_meeting_date");
        });
    }
}
