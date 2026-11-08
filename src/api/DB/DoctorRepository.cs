using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class DoctorRepository
{
    public readonly MedTechDbContext medTechDbContext;

    public DoctorRepository(MedTechDbContext medTechDbContext)
    {
        this.medTechDbContext = medTechDbContext;
    }

    public async Task<DoctorEntity?> GetDoctorByIdAsync(int id)
    {
        DoctorEntity? doctor = await medTechDbContext.Doctors.FindAsync(id);

        return doctor;
    }

    public async Task<bool> FindDoctorAsync(string Passport)
    {
        return await medTechDbContext.Doctors.AnyAsync(d => d.Passport == Passport);
    }

    public async Task<DoctorEntity> AddDoctorAsync(RequestAddDoctor requestAddDoctor)
    {
        DoctorEntity doctor = new DoctorEntity
        {
            LastName = requestAddDoctor.LastName,
            FirstName = requestAddDoctor.FirstName,
            Patronymic = requestAddDoctor.Patronymic,
            Passport = requestAddDoctor.Passport,
            Specialty = requestAddDoctor.Specialty
        };

        medTechDbContext.Doctors.Add(doctor);

        await medTechDbContext.SaveChangesAsync();

        return doctor;
    }

    public async Task<List<DoctorEntity>> GetDoctorsForSpecialtyAsync(string specialty)
    {
        return await medTechDbContext.Doctors.Where(d => d.Specialty == specialty).ToListAsync();
    }

}