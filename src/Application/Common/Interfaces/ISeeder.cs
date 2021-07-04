using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ISeeder<T>
    {
        Task<T> Seed();
    }
}