using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class PatientRepository
{
    public readonly MedTechDbContext medTechDbContext;
    public readonly ILogger<PatientRepository> logger;

    public PatientRepository(MedTechDbContext medTechDbContext, ILogger<PatientRepository> logger)
    {
        this.medTechDbContext = medTechDbContext;
        this.logger = logger;
    }


    public async Task<PatientEntity?> GetPatientByIdAsync(int id)
    {
        return await medTechDbContext.Patients
            .Include(p => p.DoctorPatients)
                .ThenInclude(dp => dp.Doctor)
            .Include(p => p.DiseasePatients)
                .ThenInclude(dp => dp.Disease)
            .Include(p => p.DiseasePatients)
                .ThenInclude(dp => dp.Doctor)
            .FirstOrDefaultAsync(p => p.Id == id);
    }


    public async Task<List<PatientEntity>> GetAllPatientsAsync()
    {
        return await medTechDbContext.Patients
            .Include(p => p.DoctorPatients)
                .ThenInclude(dp => dp.Doctor)
            .Include(p => p.DiseasePatients)
                .ThenInclude(dp => dp.Disease)
            .Include(p => p.DiseasePatients)
                .ThenInclude(dp => dp.Doctor)
            .ToListAsync();
    }


    public async Task AddAsync(PatientEntity patient)
    {
        medTechDbContext.Patients.Add(patient);
        await medTechDbContext.SaveChangesAsync();
    }

    public async Task AddDiseaseToPatientAsync(DiseasePatientEntity diseasePatient)
    {
        medTechDbContext.DiseasePatients.Add(diseasePatient);

        await medTechDbContext.SaveChangesAsync();
    }

}