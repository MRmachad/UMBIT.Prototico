using System.Threading.Tasks;

namespace UMBIT.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}