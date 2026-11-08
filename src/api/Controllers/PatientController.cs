using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly PatientService patientService;
    private readonly ILogger<PatientController> logger;

    public PatientController(PatientService patientService, ILogger<PatientController> logger)
    {
        this.patientService = patientService;
        this.logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetPatientByIdAsync(int id)
    {
        Result<PatientResponse?> result = await patientService.GetPatientByIdAsync(id);

        if (!result.IsSuccess)
        {
            return ResponseError(result);
        }

        logger.LogInformation("Запись пациента отправлена {PatientId}", id);

        return Results.Ok<PatientResponse>(result.Value);

    }

    [HttpGet("Allpatients")]
    public async Task<IResult> GetAllPatientsAsync()
    {
        Result<List<PatientResponse>> result = await patientService.GetAllPatientsAsync();

        if (!result.IsSuccess)
        {
            return ResponseError(result);
        }

        logger.LogInformation("Все пациенты получены");

        return Results.Ok<List<PatientResponse>>(result.Value);
    }

    [HttpPost("add")]
    public async Task<IResult> AddPatient([FromBody] RequestAddPatient request)
    {
        Result<PatientResponse> result = await patientService.AddPatientAsync(request);

        if (!result.IsSuccess)
        {
            return ResponseError(result);
        }

        return Results.Created<PatientResponse>($"/api/Patient/{result.Value.Id}", result.Value);
    }

    [HttpPost("{patientId}/diseases")]
    public async Task<IResult> AddDiseaseToPatient(int patientId, [FromBody] RequestAddDiseaseToPatient request)
    {
        Result<PatientResponse> result = await patientService.AddDiseaseToPatientAsync(patientId, request);

        if (!result.IsSuccess)
        {
            logger.LogWarning("Ошибка привязки болезни к пациенту {PatientId}: {Error} ", patientId, result.Error);
            return ResponseError(result);
        }

        logger.LogInformation("Болезнь привязана к пациенту {PatientId}", patientId);

        return Results.Ok<PatientResponse>(result.Value);
    }


    public IResult ResponseError<T>(Result<T> result)
    {
        return result.errorType switch
        {
            ErrorType.Validation => Results.BadRequest(result.Error),
            ErrorType.NotFound => Results.NotFound(result.Error),
            ErrorType.Conflict => Results.Conflict(result.Error),
            _ => Results.InternalServerError(result.Error)
        };
    }
}