using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Entities.Producer> GetProducer(Guid id) =>
        await _context.Producers.FirstOrDefaultAsync(e => e.ProducerId == id);
}