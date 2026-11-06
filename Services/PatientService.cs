public class PatientService
{
    private PatientRepository _patientRepository;
    private DoctorRepository _doctorRepository;
    private DiseaseRepository _diseaseRepository;

    public PatientService(PatientRepository patientRepository, DoctorRepository doctorRepository, DiseaseRepository diseaseRepository)
    {
        this._patientRepository = patientRepository;
        this._doctorRepository = doctorRepository;
        this._diseaseRepository = diseaseRepository;
    }

    public async Task<Result<PatientResponse>> GetPatientByIdAsync(int id)
    {

        if (id <= 0)
        {
            return new Result<PatientResponse> { Value = null, errorType = ErrorType.Validation, Error = $"{id},  id не может быть меньше или равен нулю" };
        }

        PatientEntity? patient = await _patientRepository.GetPatientByIdAsync(id);

        if (patient == null)
        {
            return new Result<PatientResponse> { Value = null, errorType = ErrorType.NotFound, Error = "Записи с таким id не найдено" };
        }

        PatientResponse patientResponse = new PatientResponse
        {
            Id = patient.Id,
            LastName = patient.LastName,
            FirstName = patient.FirstName,
            Patronymic = patient.Patronymic,
            SNILS = patient.SNILS,
            BirthDate = patient.BirthDate,
            doctors = patient.DoctorPatients
                .Select(dp => new DoctorResponse
                {
                    Id = dp.Doctor.Id,
                    LastName = dp.Doctor.LastName,
                    FirstName = dp.Doctor.FirstName,
                    Patronymic = dp.Doctor.Patronymic,
                    Passport = dp.Doctor.Passport,
                    Specialty = dp.Doctor.Specialty
                })
                .ToList(),
            diseases = patient.DiseasePatients
                .Select(dp => new DiseaseDTO
                {
                    Id = dp.Disease.Id,
                    Name = dp.Disease.Name,
                    Code = dp.Disease.Code
                })
                .ToList()
        };

        return new Result<PatientResponse> { Value = patientResponse, Error = null };
    }

    public async Task<Result<PatientResponse>> AddPatientAsync(RequestAddPatient request)
    {
        if (request.LastName == null || request.FirstName == null || request.SNILS == null)
        {
            return new Result<PatientResponse> { Value = null, errorType = ErrorType.Validation, Error = "поля LastName, FirstName, SNILS обязательны для заполнения" };
        }

        if (request.BirthDate > DateOnly.FromDateTime(DateTime.Now))
        {
            return new Result<PatientResponse> { Value = null, errorType = ErrorType.Validation, Error = "Дата рождения не может быть в будущем" };
        }

        DoctorEntity? doctor = await _doctorRepository.GetDoctorByIdAsync(request.DoctorId);

        if (doctor == null)
        {
            return new Result<PatientResponse> { Value = null, errorType = ErrorType.NotFound, Error = "Доктора с таким id не существует" };
        }

        PatientEntity patient = new PatientEntity
        {
            LastName = request.LastName,
            FirstName = request.FirstName,
            Patronymic = request.Patronymic,
            SNILS = request.SNILS,
            BirthDate = request.BirthDate,
            DoctorPatients = { new DoctorPatientEntity { DoctorId = doctor.Id, Doctor = doctor } }
        };

        await _patientRepository.AddAsync(patient);

        PatientResponse patientResponse = new PatientResponse
        {
            Id = patient.Id,
            LastName = patient.LastName,
            FirstName = patient.FirstName,
            Patronymic = patient.Patronymic,
            SNILS = patient.SNILS,
            BirthDate = patient.BirthDate,
            doctors = new List<DoctorResponse>
            {
                new DoctorResponse
                {
                    Id = doctor.Id,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    Patronymic = doctor.Patronymic,
                    Passport = doctor.Passport,
                    Specialty = doctor.Specialty
                }
            },
            diseases = new List<DiseaseDTO>()
        };

        return new Result<PatientResponse> { Value = patientResponse, Error = null };
    }


    public async Task<Result<List<PatientResponse>>> GetAllPatientsAsync()
    {
        List<PatientEntity> patients = await _patientRepository.GetAllPatientsAsync();

        List<PatientResponse> patientResponses = new List<PatientResponse>(patients.Count);

        foreach (PatientEntity p in patients)
        {
            patientResponses.Add(
                new PatientResponse
                {
                    Id = p.Id,
                    LastName = p.LastName,
                    FirstName = p.FirstName,
                    Patronymic = p.Patronymic,
                    SNILS = p.SNILS,
                    BirthDate = p.BirthDate,
                    doctors = p.DoctorPatients
                        .Select(dp => new DoctorResponse
                        {
                            Id = dp.Doctor.Id,
                            LastName = dp.Doctor.LastName,
                            FirstName = dp.Doctor.FirstName,
                            Patronymic = dp.Doctor.Patronymic,
                            Passport = dp.Doctor.Passport,
                            Specialty = dp.Doctor.Specialty
                        })
                        .ToList(),
                    diseases = p.DiseasePatients
                        .Select(dp => new DiseaseDTO
                        {
                            Id = dp.Disease.Id,
                            Name = dp.Disease.Name,
                            Code = dp.Disease.Code
                        })
                        .ToList()
                }
            );
        }

        return new Result<List<PatientResponse>> { Value = patientResponses, Error = null };
    }

    public async Task<Result<PatientResponse>> AddDiseaseToPatientAsync(int patientId, RequestAddDiseaseToPatient request)
    {
        if (patientId <= 0 || request.DiseaseId <= 0 || request.DoctorId <= 0)
        {
            return new Result<PatientResponse> { Value = null, errorType = ErrorType.Validation, Error = "id пациента, болезни и врача должны быть положительными" };
        }

        PatientEntity? patient = await _patientRepository.GetPatientByIdAsync(patientId);

        if (patient == null)
        {
            return new Result<PatientResponse> { Value = null, errorType = ErrorType.NotFound, Error = "Пациента с таким id не существует" };
        }

        if (patient.DiseasePatients.Any(dp => dp.DiseaseId == request.DiseaseId))
        {
            return new Result<PatientResponse> { Value = null, errorType = ErrorType.Conflict, Error = "У пациента уже есть эта болезнь" };
        }

        DoctorEntity? doctor = await _doctorRepository.GetDoctorByIdAsync(request.DoctorId);

        if (doctor == null)
        {
            return new Result<PatientResponse> { Value = null, errorType = ErrorType.NotFound, Error = "Доктора с таким id не существует" };
        }

        DiseaseEntity? disease = await _diseaseRepository.GetDiseaseByIdAsync(request.DiseaseId);

        if (disease == null)
        {
            return new Result<PatientResponse> { Value = null, errorType = ErrorType.NotFound, Error = "Болезни с таким id не существует" };
        }

        if (request.DiagnosedAt > DateOnly.FromDateTime(DateTime.Now))
        {
            return new Result<PatientResponse> { Value = null, errorType = ErrorType.Validation, Error = "Дата постановки диагноза не может быть в будущем" };
        }

        DiseasePatientEntity diseasePatient = new DiseasePatientEntity
        {
            PatientId = patientId,
            DiseaseId = disease.Id,
            DoctorId = doctor.Id,
            DiagnosedAt = request.DiagnosedAt,
            IsCured = false
        };

        await _patientRepository.AddDiseaseToPatientAsync(diseasePatient);

        PatientEntity updatedPatient = await _patientRepository.GetPatientByIdAsync(patientId);

        PatientResponse patientResponse = new PatientResponse
        {
            Id = updatedPatient.Id,
            LastName = updatedPatient.LastName,
            FirstName = updatedPatient.FirstName,
            Patronymic = updatedPatient.Patronymic,
            SNILS = updatedPatient.SNILS,
            BirthDate = updatedPatient.BirthDate,
            doctors = updatedPatient.DoctorPatients
                .Select(dp => new DoctorResponse
                {
                    Id = dp.Doctor.Id,
                    LastName = dp.Doctor.LastName,
                    FirstName = dp.Doctor.FirstName,
                    Patronymic = dp.Doctor.Patronymic,
                    Passport = dp.Doctor.Passport,
                    Specialty = dp.Doctor.Specialty
                })
                .ToList(),
            diseases = updatedPatient.DiseasePatients
                .Select(dp => new DiseaseDTO
                {
                    Id = dp.Disease.Id,
                    Name = dp.Disease.Name,
                    Code = dp.Disease.Code
                })
                .ToList()
        };

        return new Result<PatientResponse> { Value = patientResponse, Error = null };
    }

}