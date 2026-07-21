namespace Infrastructure.Repositories.Specialization;

public interface ISpecializationRepository : IRepository<Entities.Specialization>
{
    Task<IEnumerable<Entities.Specialization>> GetSpecializations(IEnumerable<Guid> ids);
    
    Task<Entities.Specialization> GetSpecialization(Guid id);
}