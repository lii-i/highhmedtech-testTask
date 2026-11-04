using Microsoft.Extensions.Logging;

public class PatientsRepository
{
    public readonly MedTechDbContext medTechDbContext;
    public readonly ILogger logger;

    public PatientsRepository(MedTechDbContext medTechDbContext, ILogger logger)
    {
        this.medTechDbContext = medTechDbContext;
        this.logger = logger;
    }

    public PatientEntity? GetPatientById(int id)
    {
        try
        {
            PatientEntity? patient = medTechDbContext.Patients.Find(id);

            if (patient == null)
            {
                return null;
            }

            return patient;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Ошибка получения записи пациента {PatientId}", id);
            return null;
        }

    }

}