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

    public async Task<IEnumerable<Entities.Doctor>> GetAllDoctorsAsync()
    {
        return await _context.Doctors
            .Include(e=>e.Specializations)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Entities.Doctor?> GetDoctorSpecializationsByIdAsync(Guid id)
    {
        return await  _context.Doctors
            .Include(e=>e.Specializations)
            .FirstOrDefaultAsync(e=>e.DoctorId == id);
    }

    public async Task<Entities.Doctor?> GetDoctorByIdAsync(Guid id) => await _context.Doctors
        .FirstOrDefaultAsync(e=>e.DoctorId == id);

    public void DeleteDoctor(Entities.Doctor doctor)
    {
        _context.Doctors.Remove(doctor);
    }
}