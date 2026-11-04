public class DoctorsServices
{
    private DoctorsRepository _doctorsRepository;

    public DoctorsServices(DoctorsRepository doctorsRepository)
    {
        this._doctorsRepository = doctorsRepository;
    }

    public DoctorEntity? GetDoctorById(int id)
    {
        DoctorEntity? doctor = _doctorsRepository.GetDoctorById(id);

        if (doctor == null)
        {
            return null;
        }

        return doctor;
    }
}