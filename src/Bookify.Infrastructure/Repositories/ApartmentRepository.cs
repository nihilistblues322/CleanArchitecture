using Bookify.Domain.Apartments;

namespace Bookify.Infrastructure.Repositories;

internal sealed class ApartmentRepository(ApplicationDbContext context) : Repository<Apartment>(context), IApartmentRepository
{
}