using ApplicationCore.Doctor.Dto;
using ApplicationCore.Exceptions;
using Infrastructure.Repositories.Doctor;
using Infrastructure.Entities;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Doctor.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository  _doctorRepository;
    private readonly IUnitOfWork  _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public DoctorService(IDoctorRepository doctorRepository,
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager)
    {
        _doctorRepository = doctorRepository;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task AddNewDoctor(DoctorDto.CreateDoctorDto dto)
    {

        var doesDoctorExistAsync = await _doctorRepository.DoesDoctorExistAsync(dto.Pwz);

        if (doesDoctorExistAsync)
        {
            throw new AlreadyExistsException("Doctor already exists");
        }
        
        var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var newDoctor = new Infrastructure.Entities.Doctor()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PWZ = dto.Pwz
            };
            
            var newUser = new ApplicationUser()
            {
               UserName = dto.Login,
               Email = dto.Email
            };
            
            newDoctor.User = newUser;

          
            await _doctorRepository.AddNewDoctorAsync(newDoctor);
            await _userManager.CreateAsync(newUser, dto.Password);
            await _userManager.AddToRoleAsync(newUser, "Doctor");
            await transaction.CommitAsync();
           await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
             await transaction.RollbackAsync();
            throw;
        }
        
    }
}