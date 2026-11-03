public class DiseasePatient
{
    public int Id { get; set; }
    public int DiseaseId { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateOnly DiagnosedAt { get; set; }
    public bool IsCured { get; set; }

    public Disease Disease { get; set; }
    public Patient Patient { get; set; }
    public Doctor Doctor { get; set; }

}