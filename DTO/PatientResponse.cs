public class PatientResponse
{
    public int Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string? Patronymic { get; set; }
    public string SNILS { get; set; }
    public DateOnly BirthDate { get; set; }

    public List<DoctorResponse> doctors { get; set; }
    public List<DiseaseDTO> diseases { get; set; }
}