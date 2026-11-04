public class PatientsServices
{
    private PatientsRepository _patientsRepository;

    public PatientsServices(PatientsRepository patientsRepository)
    {
        this._patientsRepository = patientsRepository;
    }

    public PatientEntity? GetPatientById(int id)
    {
        PatientEntity? patient = _patientsRepository.GetPatientById(id);

        if (patient == null)
        {
            return null;
        }

        return patient;
    }
}