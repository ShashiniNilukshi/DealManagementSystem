using DealManagementSystem.Data;
using DealManagementSystem.Models;
using DealManagementSystem.Repositories;
using DealManagementSystem.Interfaces;
using System.Threading.Tasks;

namespace DealManagementSystem.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IGenericRepository<Deals>? _dealRepository;
        private IGenericRepository<Hotel>? _hotelRepository;
        private IGenericRepository<Itinerary>? _itineraryRepository;
        private IGenericRepository<Media>? _mediaRepository;
        private IGenericRepository<User>? _userRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Deals> Deals =>
            _dealRepository ??= new GenericRepository<Deals>(_context);

        public IGenericRepository<Hotel> Hotels =>
            _hotelRepository ??= new GenericRepository<Hotel>(_context);

        public IGenericRepository<Itinerary> Itineraries =>
            _itineraryRepository ??= new GenericRepository<Itinerary>(_context);

        public IGenericRepository<Media> Media =>
            _mediaRepository ??= new GenericRepository<Media>(_context);

        public IGenericRepository<User> Users =>
            _userRepository ??= new GenericRepository<User>(_context);

        // Commits the changes to the database and returns a boolean indicating success
        public async Task<bool> CompleteAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        // Disposes the context, releasing the resources
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}