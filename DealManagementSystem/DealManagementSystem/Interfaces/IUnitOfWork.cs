using DealManagementSystem.Models;
using DealManagementSystem.Repositories;
using System.Threading.Tasks;

namespace DealManagementSystem.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Deals> Deals { get; }
        IGenericRepository<Hotel> Hotels { get; }
        IGenericRepository<Itinerary> Itineraries { get; }
        IGenericRepository<Media> Media { get; }

        IGenericRepository<User> Users { get; }

        Task<bool> CompleteAsync();
        void Dispose();
    }
}
