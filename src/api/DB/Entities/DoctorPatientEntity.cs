public class DoctorPatientEntity
{
    public int Id { get; set; }
    public DateOnly DateVisit { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }

    public DoctorEntity Doctor { get; set; }
    public PatientEntity Patient { get; set; }
}