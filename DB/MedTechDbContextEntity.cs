using Microsoft.EntityFrameworkCore;

public class MedTechDbContext : DbContext
{
    public DbSet<DiseaseEntity> Diseases { get; set; }
    public DbSet<DiseasePatientEntity> DiseasePatients { get; set; }
    public DbSet<DoctorEntity> Doctors { get; set; }
    public DbSet<DoctorPatientEntity> DoctorPatients { get; set; }
    public DbSet<PatientEntity> Patients { get; set; }

    public MedTechDbContext(DbContextOptions<MedTechDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DoctorEntity>(EntityBuilder =>
        {
            EntityBuilder.HasKey(d => d.Id);
            EntityBuilder.Property(d => d.Specialty).HasMaxLength(100).IsRequired();
        });
        modelBuilder.Entity<DiseaseEntity>(EntityBuilder =>
        {
            EntityBuilder.HasKey(d => d.Id);
            EntityBuilder.Property(d => d.Title).HasMaxLength(200).IsRequired();
        });
        modelBuilder.Entity<PatientEntity>(EntityBuilder =>
        {
            EntityBuilder.HasKey(p => p.Id);
            EntityBuilder.Property(p => p.LastName).HasMaxLength(100).IsRequired();
            EntityBuilder.Property(p => p.FirstName).HasMaxLength(100).IsRequired();
            EntityBuilder.Property(p => p.Patronymic).HasMaxLength(100);
        });

        modelBuilder.Entity<DiseasePatientEntity>(EntityBuilder =>
        {
            EntityBuilder.HasKey(d => d.Id);
        });
        modelBuilder.Entity<DoctorPatientEntity>(EntityBuilder =>
        {
            EntityBuilder.HasKey(d => d.Id);
        });


        modelBuilder.Entity<DoctorEntity>()
        .HasMany(d => d.DoctorPatients)
        .WithOne(dp => dp.Doctor)
        .HasForeignKey(dp => dp.DoctorId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DoctorEntity>()
        .HasMany(d => d.DiseasePatients)
        .WithOne(dd => dd.Doctor)
        .HasForeignKey(dd => dd.DoctorId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PatientEntity>()
            .HasMany(p => p.DoctorPatients)
            .WithOne(dp => dp.Patient)
            .HasForeignKey(dp => dp.PatientId);

        modelBuilder.Entity<DiseaseEntity>()
            .HasMany(d => d.DiseasePatients)
            .WithOne(dp => dp.Disease)
            .HasForeignKey(dp => dp.DiseaseId);

        modelBuilder.Entity<PatientEntity>()
            .HasMany(p => p.DiseasePatients)
            .WithOne(dp => dp.Patient)
            .HasForeignKey(dp => dp.PatientId);

    }

}