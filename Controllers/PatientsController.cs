using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class PatientsController
{
    private readonly PatientsServices patientsServices;
    private readonly ILogger logger;

    public PatientsController(PatientsServices patientsServices, ILogger logger)
    {
        this.patientsServices = patientsServices;
        this.logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetPatientById(int id)
    {
        PatientEntity? patient = patientsServices.GetPatientById(id);

        if (patient == null)
        {
            return Results.NotFound();
        }

        PatientResponse response = new PatientResponse
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Patronymic = patient.Patronymic,
            BirthDate = patient.BirthDate
        };

        logger.LogInformation("Запись пациента отправлена {PatientId}", id);

        return Results.Ok<PatientEntity?>(patient);

    }
}