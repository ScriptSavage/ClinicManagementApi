using ApplicationCore.Doctor.Dto;
using ApplicationCore.Exceptions;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Medicine.Dto;
using ApplicationCore.Patient.Dto;
using ApplicationCore.Prescription.Dto;
using ApplicationCore.Specialization.Dto;
using ApplicationCore.Visit.Dto;
using FluentValidation;
using Infrastructure.Repositories.Doctor;
using Infrastructure.Entities;
using Infrastructure.Helpers;
using Infrastructure.Repositories.Prescription;
using Infrastructure.Repositories.Specialization;
using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Doctor.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository  _doctorRepository;
    private readonly ISpecializationRepository _specializationRepository;
    private readonly IPrescriptionRepository _prescriptionRepository;
    private readonly IUnitOfWork  _unitOfWork;
    private readonly IValidator<DoctorDto.CreateDoctorDto> _createDoctorValidator;
    private readonly UserManager<ApplicationUser> _userManager;
    

    public DoctorService(IDoctorRepository doctorRepository,
        ISpecializationRepository specializationRepository,
        IPrescriptionRepository prescriptionRepository,
        IUnitOfWork unitOfWork,
        IValidator<DoctorDto.CreateDoctorDto> createDoctorValidator,
        UserManager<ApplicationUser> userManager)
    {
        _doctorRepository = doctorRepository;
        _specializationRepository = specializationRepository;
        _prescriptionRepository = prescriptionRepository;
        _unitOfWork = unitOfWork;
        _createDoctorValidator = createDoctorValidator;
        _userManager = userManager;
    }

    public async Task AddNewDoctor(DoctorDto.CreateDoctorDto dto)
    {
        var createNewDoctorValidator = await _createDoctorValidator.ValidateAsync(dto);

        if (!createNewDoctorValidator.IsValid)
        {
            throw new ValidationException(createNewDoctorValidator.Errors);
        }

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

            var specializations = await _specializationRepository.
                GetSpecializations(dto.SpecializationsIds);
            
            foreach (var specialization in specializations)
            {
                newDoctor.Specializations.Add(specialization);
            }

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

    public async Task<PageResponse<DoctorDto.Response>> GetAllDoctorsAsync(int pageNumber, int pageSize)
    {

        var doctors = (await _doctorRepository.GetAllDoctorsAsync()).AsQueryable();

        var data = doctors.Select(e => new DoctorDto.Response(
            e.FirstName,
            e.LastName,
            e.Specializations.Select(x=>new SpecializationDto.NewSpecialization(
                x.Name,
                x.Description))))
            .ToList();

        
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Max(1, pageSize);
        
        var totalRecords = doctors.Count();

        return new PageResponse<DoctorDto.Response>()
        {
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
        };
    }

    public async Task<DoctorDto.Response> GetDoctorByIdAsync(Guid id)
    {
        var data = await _doctorRepository.GetDoctorSpecializationsByIdAsync(id);
        
        var doctor = new DoctorDto.Response(
            data.FirstName,
            data.LastName,
            data.Specializations.Select(e=>new SpecializationDto.NewSpecialization(
                e.Name,
                e.Description)));
        
        return doctor;
    }

    public async Task UpdateDoctor(Guid id, DoctorDto.UpdateDoctorDto dto)
    {
        var doctor = await _doctorRepository.GetDoctorByIdAsync(id);
        
        

        if (doctor is null)
        {
            throw new DoesNotExistsException("Doctor not found");
        }

        if(!string.IsNullOrWhiteSpace(dto.FirstName)) doctor.FirstName = dto.FirstName;
        if (!string.IsNullOrWhiteSpace(dto.LastName)) doctor.LastName = dto.LastName;
        if (!string.IsNullOrWhiteSpace(dto.Pwz)) doctor.PWZ = dto.Pwz;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteDoctor(Guid id)
    {
        var doctor = await _doctorRepository.GetDoctorByIdAsync(id);
        if (doctor is null)
        {
            throw new DoesNotExistsException("Doctor not found");
        }
        
        _doctorRepository.DeleteDoctor(doctor);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<SpecializationDto.Response>> GetDoctorSpecializationById(Guid id)
    {
        var doctor = await _doctorRepository.GetDoctorSpecializationsByIdAsync(id);

        if (doctor is null)
        {
            throw new DoesNotExistsException("Doctor not found");
        }

        var result =  doctor.Specializations.Select(x => 
            new SpecializationDto.Response(
                x.Name, 
                x.Description));

        return result;
    }

    public async Task AddNewSpecializationToDoctor(Guid id, Guid specializationId)
    {
        var doctor = await _doctorRepository.GetDoctorByIdAsync(id);
        if (doctor is null)
        {
            throw new DoesNotExistsException("Doctor not found");
        }
        
        var specialization = await _specializationRepository.GetSpecialization(specializationId);

        if (specialization is null)
        {
            throw new DoesNotExistsException("Specialization not found");
        }
        
        doctor.Specializations.Add(specialization);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteDoctorSpecialization(Guid id, Guid specializationId)
    {
        var doctor = await _doctorRepository.GetDoctorByIdAsync(id);
        if (doctor is null)
        {
            throw new DoesNotExistsException("Doctor not found");
        }
        
        var specialization = await _specializationRepository.GetSpecialization(specializationId);
        if (specialization is null)
        {
            throw new DoesNotExistsException("Specialization not found");
        }
        
        doctor.Specializations.Remove(specialization);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<DoctorDto.MyDetails> GetMyDetails(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            throw new DoesNotExistsException("User not found");
        }

        var doctor = await _doctorRepository.GetDoctorByUserIdAsync(user.Id);

        var doctorSpecializations =  _specializationRepository.GetSpecializationsByDoctorId(doctor.DoctorId);
        
        return new DoctorDto.MyDetails(doctor.FirstName, 
            doctor.LastName, 
            doctor.PWZ,
            doctorSpecializations.Select(e=>new SpecializationDto.NewSpecialization(e.Name,
                e.Description))
            );
    }

    public async Task<IEnumerable<VisitDto.VisitDetailsResponse>> GetDoctorVisitsByUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new DoesNotExistsException("User not found");
        }
        
        var doctor = await _doctorRepository.GetDoctorByUserIdAsync(user.Id);

        if (doctor is null)
        {
            throw new DoesNotExistsException("Doctor not found");
        }
        
        var doctorVisits = await _doctorRepository.GetDoctorVisitsDetailsAsync(doctor.DoctorId);

        return doctorVisits.Select(e => new VisitDto.VisitDetailsResponse(new DoctorDto.UpdateDoctorDto(
                e.Doctor.FirstName,
                e.Doctor.LastName,
                e.Doctor.PWZ),
            new PatientDto.Response(
                e.Patient.FirstName,
                e.Patient.LastName,
                e.Patient.Pesel),
            e.Date,
            e.Description
        )).ToList();
        
    }

    public async Task<IEnumerable<PrescriptionDto.PrescriptionDetails>> GetDoctorPrescriptionsDetails(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new DoesNotExistsException("User not found");
        }
        
        var doctor = await _doctorRepository.GetDoctorByIdAsync(user.Id);
        if (doctor is null)
        {
            throw new DoesNotExistsException("Doctor not found");
        }
        
        var doctorPrescriptions = await _prescriptionRepository
            .GetPrescriptionsByDoctorIdAsync(doctor.DoctorId);


       return doctorPrescriptions.Select(e => new PrescriptionDto.PrescriptionDetails(
            new PatientDto.Response(
                e.Patient.FirstName,
                e.Patient.LastName,
                e.Patient.Pesel),
            e.CreatedAt,
            e.Code,
            e.MedicinePrescriptions.Select(m => new MedicineDto.Details(
                m.Medicine.Name,
                m.Medicine.ActiveSubstance,
                m.Medicine.PharmaceuticalForm,
                m.Dosage,
                m.Frequency,
                m.Instructions
            )))).ToList();
       
    }
}