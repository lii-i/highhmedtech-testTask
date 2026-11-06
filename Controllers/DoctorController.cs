using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class DoctorController
{
    private readonly DoctorService doctorService;
    private readonly ILogger<DoctorController> logger;

    public DoctorController(DoctorService doctorService, ILogger<DoctorController> logger)
    {
        this.doctorService = doctorService;
        this.logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetDoctorById(int id)
    {
        Result<DoctorResponse> result = await doctorService.GetDoctorByIdAsync(id);

        if (!result.IsSuccess)
        {
            logger.LogWarning("Доктор {DoctorId}: {Error} ", id, result.Error);
            return ResponseError(result);
        }

        logger.LogInformation("Запись доктора отправлена {DoctorId}", id);

        return Results.Ok<DoctorResponse>(result.Value);
    }

    [HttpPost("add")]
    public async Task<IResult> AddDoctor(
        [FromBody] RequestAddDoctor requestAddDoctor
    )
    {
        Result<DoctorResponse> result = await doctorService.AddDoctorAsync(requestAddDoctor);

        if (!result.IsSuccess)
        {
            logger.LogWarning("Ошибка добавления доктора: {requestDoctor}, Error: {Error} ", requestAddDoctor, result.Error);
            return ResponseError(result);
        }

        logger.LogInformation("Запись доктора добавленна {DoctorId}", result.Value.Id);

        return Results.Created<DoctorResponse>($"/api/Doctor/{result.Value.Id}", result.Value);

    }

    [HttpGet]
    public async Task<IResult> GetDoctorsForSpecialty(
        [FromQuery] string specialty
    )
    {
        Result<List<DoctorResponse>> result = await doctorService.GetDoctorsForSpecialtyAsync(specialty);

        if (!result.IsSuccess)
        {
            return ResponseError(result);
        }

        return Results.Ok<List<DoctorResponse>>(result.Value);
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