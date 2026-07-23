namespace Infrastructure.Repositories.Medicine;

public interface IMedicineRepository : IRepository<Entities.Medicine>
{
    Task<Entities.Medicine> GetMedicineAsync(Guid id);
    
    Task<IEnumerable<Entities.Medicine>> GetMedicinesAsync();
}