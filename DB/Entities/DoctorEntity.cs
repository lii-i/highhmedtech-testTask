public class DoctorEntity
{
    public int Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string? Patronymic { get; set; }
    public string Specialty { get; set; }

    public List<DoctorPatientEntity> DoctorPatients { get; set; } = new List<DoctorPatientEntity>();
    public List<DiseasePatientEntity> DiseasePatients { get; set; } = new List<DiseasePatientEntity>();
}