using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Medicine;

public class MedicineRepository : IMedicineRepository
{
    private readonly DatabaseContext _context;

    public MedicineRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Entities.Medicine entity)
    {
        await _context.Medicines.AddAsync(entity);
    }

    public void Delete(Entities.Medicine entity)
    {
        _context.Medicines.Remove(entity);
    }

    public async Task<Entities.Medicine> GetMedicineAsync(Guid id)
    {
        return await _context.Medicines.FirstOrDefaultAsync(e=>e.MedicineId == id);
    }

    public async Task<IEnumerable<Entities.Medicine>> GetMedicinesAsync()
    {
       return await _context.Medicines
           .Include(e=>e.Producer)
           .ToListAsync();
    }
}