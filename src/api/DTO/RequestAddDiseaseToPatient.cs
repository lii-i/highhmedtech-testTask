public class RequestAddDiseaseToPatient
{
    public int DiseaseId { get; set; }
    public int DoctorId { get; set; }
    public DateOnly DiagnosedAt { get; set; }
}
