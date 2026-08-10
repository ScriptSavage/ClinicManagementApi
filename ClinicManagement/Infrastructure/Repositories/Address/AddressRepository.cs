using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Entities.Address?> GetAddressByPatientId(Guid patientId)
    {
        return await _context.Addresses
            .FirstOrDefaultAsync(a => a.PatientId == patientId);
    }
}