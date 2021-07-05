using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ISeeder
    {
        Task SeedAsync();
    }
    
    public interface ISeeder<T>
    {
        Task<T> SeedAsync();
    }
}