
public class DoctorService
{
    private DoctorRepository _doctorRepository;

    public DoctorService(DoctorRepository doctorRepository)
    {
        this._doctorRepository = doctorRepository;
    }

    public async Task<Result<DoctorResponse>> GetDoctorByIdAsync(int id)
    {

        if (id <= 0)
        {
            return new Result<DoctorResponse> { Value = null, errorType = ErrorType.Validation, Error = $"{id},  id не может быть меньше или равен нулю" };
        }

        DoctorEntity? doctor = await _doctorRepository.GetDoctorByIdAsync(id);

        if (doctor == null)
        {
            return new Result<DoctorResponse> { Value = null, errorType = ErrorType.NotFound, Error = "Записи с таким id не найдено" };
        }

        DoctorResponse DoctorResponse = new DoctorResponse
        {
            Id = doctor.Id,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Patronymic = doctor.Patronymic,
            Passport = doctor.Passport,
            Specialty = doctor.Specialty
        };

        return new Result<DoctorResponse> { Value = DoctorResponse, Error = null };
    }

    public async Task<Result<DoctorResponse>> AddDoctorAsync(RequestAddDoctor requestAddDoctor)
    {
        if (string.IsNullOrWhiteSpace(requestAddDoctor.LastName) || string.IsNullOrWhiteSpace(requestAddDoctor.FirstName) || string.IsNullOrWhiteSpace(requestAddDoctor.Passport) || string.IsNullOrWhiteSpace(requestAddDoctor.Specialty))
        {
            return new Result<DoctorResponse> { Value = null, errorType = ErrorType.Validation, Error = "поля LastName, FirstName, Passport, Specialty обязательны для заполнения" };
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(requestAddDoctor.Passport, @"^\d{10}$"))
        {
            return new Result<DoctorResponse>
            {
                Value = null,
                errorType = ErrorType.Validation,
                Error = "паспорт должен содержать ровно 10 цифр"
            };
        }

        requestAddDoctor.LastName = requestAddDoctor.LastName.Trim();
        requestAddDoctor.FirstName = requestAddDoctor.FirstName.Trim();
        requestAddDoctor.Patronymic = requestAddDoctor.Patronymic?.Trim();
        requestAddDoctor.Passport = requestAddDoctor.Passport.Trim();
        requestAddDoctor.Specialty = requestAddDoctor.Specialty.Trim().ToLower();

        bool find = await _doctorRepository.FindDoctorAsync(requestAddDoctor.Passport);
        if (find)
        {
            return new Result<DoctorResponse> { Value = null, errorType = ErrorType.Conflict, Error = "Такая запись доктора уже существует" };
        }

        DoctorEntity addingDoctor = await _doctorRepository.AddDoctorAsync(requestAddDoctor);


        DoctorResponse doctorResponse = new DoctorResponse
        {
            Id = addingDoctor.Id,
            FirstName = addingDoctor.FirstName,
            LastName = addingDoctor.LastName,
            Patronymic = addingDoctor.Patronymic,
            Passport = addingDoctor.Passport,
            Specialty = addingDoctor.Specialty
        };

        return new Result<DoctorResponse> { Value = doctorResponse, Error = null };
    }

    public async Task<Result<List<DoctorResponse>>> GetDoctorsForSpecialtyAsync(string specialty)
    {

        if (string.IsNullOrWhiteSpace(specialty))
            return new Result<List<DoctorResponse>> { Value = null, errorType = ErrorType.Validation, Error = "Не указана специальность" };

        specialty = specialty.Trim().ToLower();

        List<DoctorEntity> doctors = await _doctorRepository.GetDoctorsForSpecialtyAsync(specialty);

        List<DoctorResponse> doctorResponses = new List<DoctorResponse>(doctors.Count());
        foreach (DoctorEntity d in doctors)
        {
            doctorResponses.Add(
                new DoctorResponse
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Patronymic = d.Patronymic,
                    Passport = d.Passport,
                    Specialty = d.Specialty
                }
            );
        }

        return new Result<List<DoctorResponse>> { Value = doctorResponses, Error = null };

    }

}