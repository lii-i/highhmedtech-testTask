public class Disease
{
    public int Id { get; set; }
    public string Title { get; set; }

    public List<DiseasePatient> DiseasePatients { get; set; } = new List<DiseasePatient>();
}