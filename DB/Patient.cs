public class Patient
{
    public int Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string? Patronymic { get; set; }
    public DateOnly BirthDate { get; set; }

    public List<DoctorPatient> DoctorPatients { get; set; } = new List<DoctorPatient>();
    public List<DiseasePatient> DiseasePatients { get; set; } = new List<DiseasePatient>();

}