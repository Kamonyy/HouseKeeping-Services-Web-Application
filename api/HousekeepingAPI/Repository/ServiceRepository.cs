using HousekeepingAPI.Data;
using HousekeepingAPI.Interfaces;

namespace HousekeepingAPI.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<Models.Service> GetAll()
        {
            return _context.Services.OrderBy(s => s.Id).ToList();
        }

        public Models.Service GetById(int id)
        {
            return _context.Services.Where(s => s.Id == id).FirstOrDefault()!;
        }
    }
}
