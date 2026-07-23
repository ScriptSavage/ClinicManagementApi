using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Medicine.Dto;

namespace ApplicationCore.Medicine.Services;

public interface IMedicineService
{
    Task AddNewMedicineAsync(Guid producerId, MedicineDto.Request dto);
    
    Task DeleteMedicineAsync(Guid id);
    
    Task UpdateMedicineAsync(Guid id, MedicineDto.Request dto);
    
    Task<PageResponse<MedicineDto.Request>>  GetMedicinesAsync(int pageNumber = 1, int pageSize = 10);
}