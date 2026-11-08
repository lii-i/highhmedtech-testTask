using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class DiseaseRepository
{
    public readonly MedTechDbContext medTechDbContext;

    public DiseaseRepository(MedTechDbContext medTechDbContext)
    {
        this.medTechDbContext = medTechDbContext;
    }

    public async Task<DiseaseEntity?> GetDiseaseByIdAsync(int id)
    {
        DiseaseEntity? disease = await medTechDbContext.Diseases.FindAsync(id);

        return disease;
    }

    public async Task<List<DiseaseEntity>> GetAllDiseasesAsync()
    {
        return await medTechDbContext.Diseases.ToListAsync();
    }

    public async Task<bool> FindDiseaseAsync(string Name)
    {
        return await medTechDbContext.Diseases.AnyAsync(d => d.Name == Name);
    }

    public async Task<DiseaseEntity> AddDiseaseAsync(RequestAddDisease requestAddDisease)
    {
        DiseaseEntity disease = new DiseaseEntity
        {
            Name = requestAddDisease.Name,
            Code = requestAddDisease.Code
        };

        medTechDbContext.Diseases.Add(disease);

        await medTechDbContext.SaveChangesAsync();

        return disease;
    }
}
