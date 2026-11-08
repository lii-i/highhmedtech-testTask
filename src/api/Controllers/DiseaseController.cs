using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class DiseaseController : ControllerBase
{
    private readonly DiseaseService diseaseService;
    private readonly ILogger<DiseaseController> logger;

    public DiseaseController(DiseaseService diseaseService, ILogger<DiseaseController> logger)
    {
        this.diseaseService = diseaseService;
        this.logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetDiseaseById(int id)
    {
        Result<DiseaseResponse> result = await diseaseService.GetDiseaseByIdAsync(id);

        if (!result.IsSuccess)
        {
            logger.LogWarning("Болезнь {DiseaseId}: {Error} ", id, result.Error);
            return ResponseError(result);
        }

        logger.LogInformation("Запись болезни отправлена {DiseaseId}", id);

        return Results.Ok<DiseaseResponse>(result.Value);
    }

    [HttpGet("AllDiseases")]
    public async Task<IResult> GetAllDiseases()
    {
        Result<List<DiseaseResponse>> result = await diseaseService.GetAllDiseasesAsync();

        if (!result.IsSuccess)
        {
            return ResponseError(result);
        }

        logger.LogInformation("Все болезни получены");

        return Results.Ok<List<DiseaseResponse>>(result.Value);
    }

    [HttpPost("add")]
    public async Task<IResult> AddDisease([FromBody] RequestAddDisease request)
    {
        Result<DiseaseResponse> result = await diseaseService.AddDiseaseAsync(request);

        if (!result.IsSuccess)
        {
            logger.LogWarning("Ошибка добавления болезни: {requestDisease}, Error: {Error} ", request, result.Error);
            return ResponseError(result);
        }

        logger.LogInformation("Запись болезни добавлена {DiseaseId}", result.Value.Id);

        return Results.Created<DiseaseResponse>($"/api/Disease/{result.Value.Id}", result.Value);
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
