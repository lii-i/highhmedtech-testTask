using Microsoft.Extensions.Logging;

public class DoctorsRepository
{
    public readonly MedTechDbContext medTechDbContext;
    public readonly ILogger logger;

    public DoctorsRepository(MedTechDbContext medTechDbContext, ILogger logger)
    {
        this.medTechDbContext = medTechDbContext;
        this.logger = logger;
    }

    public DoctorEntity? GetDoctorById(int id)
    {
        try
        {
            DoctorEntity? doctor = medTechDbContext.Doctors.Find(id);

            if (doctor == null)
            {
                return null;
            }
            return doctor;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Ошибка при получении записи доктора {DoctorId}", id);
            return null;
        }
    }

}