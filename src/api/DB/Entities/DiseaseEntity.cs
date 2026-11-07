public class DiseaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Code { get; set; }

    public List<DiseasePatientEntity> DiseasePatients { get; set; } = new List<DiseasePatientEntity>();
}