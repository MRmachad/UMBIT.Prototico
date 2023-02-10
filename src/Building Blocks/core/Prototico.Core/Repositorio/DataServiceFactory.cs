
using UMBIT.Core.Repositorio.Repositorio;
using System.Data.Entity;

namespace UMBIT.Core.Repositorio
{
    public interface IDataServiceFactory
    {
        Repositorio<T> GetRepositorio<T>() where T : class;
    }

    public class DataServiceFactory : IDataServiceFactory
    {
        private DbContext DbContext { get; set; }
        public DataServiceFactory(DbContext dbContext)
        {
            this.DbContext = dbContext;
        }
        public Repositorio<T> GetRepositorio<T>() where T : class
        {

            return new Repositorio<T>(this.DbContext);
        }

    }
}
