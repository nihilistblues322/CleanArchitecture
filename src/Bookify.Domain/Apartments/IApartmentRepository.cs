namespace Bookify.Domain.Apartments;

public interface IApartmentRepository
{
    Task<Apartment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}