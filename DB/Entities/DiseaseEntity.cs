public class DiseaseEntity
{
    public int Id { get; set; }
    public string Title { get; set; }

    public List<DiseasePatientEntity> DiseasePatients { get; set; } = new List<DiseasePatientEntity>();
}