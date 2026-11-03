using Microsoft.EntityFrameworkCore;

public class MedTechDbContext : DbContext
{
    public DbSet<Disease> Diseases { get; set; }
    public DbSet<DiseasePatient> DiseasePatients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<DoctorPatient> DoctorPatients { get; set; }
    public DbSet<Patient> Patients { get; set; }

    public MedTechDbContext(DbContextOptions<MedTechDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>(EntityBuilder =>
        {
            EntityBuilder.HasKey(d => d.Id);
            EntityBuilder.Property(d => d.Specialty).HasMaxLength(100).IsRequired();
        });
        modelBuilder.Entity<Disease>(EntityBuilder =>
        {
            EntityBuilder.HasKey(d => d.Id);
            EntityBuilder.Property(d => d.Title).HasMaxLength(200).IsRequired();
        });
        modelBuilder.Entity<Patient>(EntityBuilder =>
        {
            EntityBuilder.HasKey(p => p.Id);
            EntityBuilder.Property(p => p.LastName).HasMaxLength(100).IsRequired();
            EntityBuilder.Property(p => p.FirstName).HasMaxLength(100).IsRequired();
            EntityBuilder.Property(p => p.Patronymic).HasMaxLength(100);
        });

        modelBuilder.Entity<DiseasePatient>(EntityBuilder =>
        {
            EntityBuilder.HasKey(d => d.Id);
        });
        modelBuilder.Entity<DoctorPatient>(EntityBuilder =>
        {
            EntityBuilder.HasKey(d => d.Id);
        });


        modelBuilder.Entity<Doctor>()
        .HasMany(d => d.DoctorPatients)
        .WithOne(dp => dp.Doctor)
        .HasForeignKey(dp => dp.DoctorId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Doctor>()
        .HasMany(d => d.DiseasePatients)
        .WithOne(dd => dd.Doctor)
        .HasForeignKey(dd => dd.DoctorId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.DoctorPatients)
            .WithOne(dp => dp.Patient)
            .HasForeignKey(dp => dp.PatientId);

        modelBuilder.Entity<Disease>()
            .HasMany(d => d.DiseasePatients)
            .WithOne(dp => dp.Disease)
            .HasForeignKey(dp => dp.DiseaseId);

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.DiseasePatients)
            .WithOne(dp => dp.Patient)
            .HasForeignKey(dp => dp.PatientId);

    }

}