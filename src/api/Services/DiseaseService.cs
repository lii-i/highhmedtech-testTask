public class DiseaseService
{
    private readonly DiseaseRepository diseaseRepository;

    public DiseaseService(DiseaseRepository diseaseRepository)
    {
        this.diseaseRepository = diseaseRepository;
    }

    public async Task<Result<DiseaseResponse>> AddDiseaseAsync(RequestAddDisease request)
    {
        if (request.Name == null || request.Code == null)
        {
            return new Result<DiseaseResponse>
            {
                Value = null,
                errorType = ErrorType.Validation,
                Error = "поля Name, Code обязательны для заполнения"
            };
        }

        bool find = await diseaseRepository.FindDiseaseAsync(request.Name);

        if (find)
        {
            return new Result<DiseaseResponse>
            {
                Value = null,
                errorType = ErrorType.Conflict,
                Error = "Болезнь с таким названием уже существует"
            };
        }

        DiseaseEntity addedDisease = await diseaseRepository.AddDiseaseAsync(request);

        DiseaseResponse diseaseResponse = new DiseaseResponse
        {
            Id = addedDisease.Id,
            Name = addedDisease.Name,
            Code = addedDisease.Code
        };

        return new Result<DiseaseResponse> { Value = diseaseResponse, Error = null };
    }

    public async Task<Result<DiseaseResponse>> GetDiseaseByIdAsync(int id)
    {
        if (id <= 0)
        {
            return new Result<DiseaseResponse>
            {
                Value = null,
                errorType = ErrorType.Validation,
                Error = "id не может быть меньше или равен нулю"
            };
        }

        DiseaseEntity? disease = await diseaseRepository.GetDiseaseByIdAsync(id);

        if (disease == null)
        {
            return new Result<DiseaseResponse>
            {
                Value = null,
                errorType = ErrorType.NotFound,
                Error = "Записи с таким id не найдено"
            };
        }

        DiseaseResponse diseaseResponse = new DiseaseResponse
        {
            Id = disease.Id,
            Name = disease.Name,
            Code = disease.Code
        };

        return new Result<DiseaseResponse> { Value = diseaseResponse, Error = null };
    }

    public async Task<Result<List<DiseaseResponse>>> GetAllDiseasesAsync()
    {
        List<DiseaseEntity> diseases = await diseaseRepository.GetAllDiseasesAsync();

        List<DiseaseResponse> diseaseResponses = new List<DiseaseResponse>(diseases.Count);

        foreach (DiseaseEntity d in diseases)
        {
            diseaseResponses.Add(
                new DiseaseResponse
                {
                    Id = d.Id,
                    Name = d.Name,
                    Code = d.Code
                }
            );
        }

        return new Result<List<DiseaseResponse>> { Value = diseaseResponses, Error = null };
    }
}
