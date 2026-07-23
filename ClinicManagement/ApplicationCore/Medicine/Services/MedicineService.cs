using ApplicationCore.Exceptions;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Medicine.Dto;
using Infrastructure.Helpers;
using Infrastructure.Repositories.Medicine;
using Infrastructure.Repositories.Producer;

namespace ApplicationCore.Medicine.Services;

public class MedicineService : IMedicineService
{
    private readonly IMedicineRepository _medicineRepository;
    private readonly IProducerRepository _producerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MedicineService(IMedicineRepository medicineRepository, 
        IUnitOfWork unitOfWork,
        IProducerRepository producerRepository)
    {
        _medicineRepository = medicineRepository;
        _producerRepository = producerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task AddNewMedicineAsync(Guid producerId ,MedicineDto.Request dto)
    {
        var producer = await _producerRepository.GetProducer(producerId)
            ?? throw new DoesNotExistsException("Producer not found");
        
        
        var newMedicine = new Infrastructure.Entities.Medicine
        {
            Name = dto.Name,
            PharmaceuticalForm = dto.PharmaceuticalForm,
            ActiveSubstance = dto.ActiveSubstance
        };
        
       await _medicineRepository.CreateAsync(newMedicine);
       producer.Medicines.Add(newMedicine);
       await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteMedicineAsync(Guid id)
    {
        var medicine = await _medicineRepository.GetMedicineAsync(id);
       _medicineRepository.Delete(medicine);
       await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateMedicineAsync(Guid id, MedicineDto.Request dto)
    {
        var medicine = await _medicineRepository.GetMedicineAsync(id);
        
        if(!string.IsNullOrWhiteSpace(dto.Name)) medicine.Name = dto.Name;
        if(!string.IsNullOrWhiteSpace(dto.ActiveSubstance)) medicine.ActiveSubstance = dto.ActiveSubstance;
        if(!string.IsNullOrWhiteSpace(dto.PharmaceuticalForm)) medicine.PharmaceuticalForm = dto.PharmaceuticalForm;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PageResponse<MedicineDto.Request>> GetMedicinesAsync(int pageNumber, int pageSize)
    {
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Max(1, pageSize);

        var medicines = (await _medicineRepository.GetMedicinesAsync()).AsQueryable();

        var projectionData = medicines
            .OrderBy(x => x.Name)
            .Select(x => new MedicineDto.Request(x.Name, x.ActiveSubstance, x.PharmaceuticalForm));

        var totalRecords = projectionData.Count();
        var pagedData = projectionData
            .ApplyPagination(pageNumber, pageSize)
            .ToList();

        return new PageResponse<MedicineDto.Request>
        {
            Data = pagedData,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
        };
    }
}