namespace Infrastructure.Repositories.Visit;

public interface IVisitRepository
{
    Task AddNewVisitAsync(Entities.Visit visit);
}