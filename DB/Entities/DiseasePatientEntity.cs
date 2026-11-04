public class DiseasePatientEntity
{
    public int Id { get; set; }
    public int DiseaseId { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateOnly DiagnosedAt { get; set; }
    public bool IsCured { get; set; }

    public DiseaseEntity Disease { get; set; }
    public PatientEntity Patient { get; set; }
    public DoctorEntity Doctor { get; set; }

}