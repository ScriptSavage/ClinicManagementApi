namespace Infrastructure.Repositories.Producer;

public interface IProducerRepository : IRepository<Entities.Producer>
{
    Task<Entities.Producer> GetProducer(Guid id);
    
    Task<IEnumerable<Entities.Producer>> GetProducersAsync();
}