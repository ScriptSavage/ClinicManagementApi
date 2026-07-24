using Infrastructure.Context;

namespace Infrastructure.Repositories.Address;

public class AddressRepository : IAddressRepository
{
    private readonly DatabaseContext _context;

    public AddressRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Entities.Address entity)
    {
        await _context.Addresses.AddAsync(entity);
    }

    public void Delete(Entities.Address entity)
    {
        _context.Addresses.Remove(entity);
    }
}