using Infrastructure.Context;

namespace Infrastructure.Repositories.Producer;

public class ProducerRepository : IProducerRepository
{
    private readonly DatabaseContext _context;

    public ProducerRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Entities.Producer entity)
    {
        await _context.Producers.AddAsync(entity);
    }

    public void Delete(Entities.Producer entity)
    {
        _context.Producers.Remove(entity);
    }
}