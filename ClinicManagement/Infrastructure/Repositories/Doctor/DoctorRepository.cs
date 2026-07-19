using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Doctor;

public class DoctorRepository : IDoctorRepository
{
    private readonly DatabaseContext _context;

    public DoctorRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddNewDoctorAsync(Entities.Doctor doctor)
    {
        await _context.Doctors.AddAsync(doctor);
    }

    public async Task<bool> DoesDoctorExistAsync(string Pwz)
    {
        return await _context.Doctors.AnyAsync(e=>e.PWZ == Pwz);
        
    }
}