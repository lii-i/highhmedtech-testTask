public class RequestAddPatient
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string? Patronymic { get; set; }
    public string SNILS { get; set; }
    public DateOnly BirthDate { get; set; }

    public int DoctorId { get; set; }
}