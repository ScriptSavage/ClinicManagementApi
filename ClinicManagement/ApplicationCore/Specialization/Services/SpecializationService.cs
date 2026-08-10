using ApplicationCore.Doctor.Dto;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Specialization.Dto;
using Infrastructure.Helpers;
using Infrastructure.Repositories.Doctor;
using Infrastructure.Repositories.Specialization;

namespace ApplicationCore.Specialization.Services;

public class SpecializationService : ISpecializationService
{
    private readonly ISpecializationRepository _specializationRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SpecializationService(ISpecializationRepository specializationRepository, 
        IDoctorRepository doctorRepository,
        IUnitOfWork unitOfWork)
    {
        _specializationRepository = specializationRepository;
        _doctorRepository = doctorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task AddNewSpecialization(SpecializationDto.NewSpecialization dto)
    {
        var newSpecialization = new Infrastructure.Entities.Specialization
        {
            Name = dto.SpecializationName,
            Description = dto.SpecializationDescription,
        };

        await _specializationRepository.CreateAsync(newSpecialization);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PageResponse<SpecializationDto.Response>> GetSpecializations(int pageNumber, int pageSize)
    {
        var specializations = (await _specializationRepository.GetSpecializations())
            .AsQueryable();

        var data = specializations
            .Select(e => new SpecializationDto.Response(e.Name, e.Description))
            .ApplyPagination(pageNumber, pageSize)
            .ToList();

        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Max(1, pageSize);
        var dataCount = data.Count();
        
        return new PageResponse<SpecializationDto.Response>()
        {
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = dataCount,
            TotalPages = (int)Math.Ceiling((double)dataCount / pageSize),
        };
    }

    public async Task<SpecializationDto.Response> GetSpecialization(Guid specializationId)
    {
        var specialization = await _specializationRepository.GetSpecialization(specializationId);
        
        return new SpecializationDto.Response(
            specialization.Name, 
            specialization.Description);
    }

    public async Task ModifySpecialization(Guid specializationId, SpecializationDto.NewSpecialization specialization)
    {
        var specializationToUpdate = await _specializationRepository.GetSpecialization(specializationId);

        if (!string.IsNullOrWhiteSpace(specialization.SpecializationName))
        {
            specializationToUpdate.Name = specialization.SpecializationName;
        }
        
        if (!string.IsNullOrWhiteSpace(specialization.SpecializationDescription))
        {
            specializationToUpdate.Description = specialization.SpecializationDescription;
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PageResponse<DoctorDto.DoctorDetails>> GetDoctorsDetailsBySpecialization(Guid specializationId, 
        int pageNumber, int pageSize)
    {
        var specialization = await _specializationRepository.GetSpecialization(specializationId);

        var doctorsBySpecialization = (await _doctorRepository
                .GetDoctorsBySpecialization(specialization))
            .AsQueryable();


        var data = doctorsBySpecialization.Select(e => new DoctorDto.DoctorDetails(
            e.FirstName,
            e.LastName))
            .ApplyPagination(pageNumber, pageSize)
            .ToList();
        
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Max(1, pageSize);
        
        return new PageResponse<DoctorDto.DoctorDetails>()
        {
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = data.Count(),
            TotalPages = (int)Math.Ceiling((double)(data.Count / pageNumber)),
        };
        
        
    }
}