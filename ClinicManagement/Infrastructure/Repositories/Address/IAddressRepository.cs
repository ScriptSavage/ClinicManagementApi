namespace Infrastructure.Repositories.Address;

public interface IAddressRepository : IRepository<Entities.Address>
{
    Task<Entities.Address?> GetAddressByPatientId(Guid patientId);
}