using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/Doctors[controller]")]
public class DoctorsController
{
    private readonly DoctorsServices doctorsServices;
    private readonly ILogger logger;

    public DoctorsController(DoctorsServices doctorsServices, ILogger logger)
    {
        this.doctorsServices = doctorsServices;
        this.logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetDoctorById(int id)
    {
        DoctorEntity? doctor = doctorsServices.GetDoctorById(id);

        if (doctor == null)
        {
            return Results.NotFound();
        }

        DoctorResponse response = new DoctorResponse
        {
            Id = doctor.Id,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Patronymic = doctor.Patronymic,
            Specialty = doctor.Specialty
        };

        logger.LogInformation("Запись доктора отправлена {DoctorId}", id);

        return Results.Ok<DoctorResponse?>(response);
    }


}