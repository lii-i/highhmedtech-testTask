public class DiagnosisDto
{
    public int DiseaseId { get; set; }
    public string DiseaseName { get; set; }
    public string? DiseaseCode { get; set; }
    public int DoctorId { get; set; }
    public DoctorResponse Doctor { get; set; }
    public DateOnly DiagnosedAt { get; set; }
    public bool IsCured { get; set; }
}