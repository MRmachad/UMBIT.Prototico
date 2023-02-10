using UMBIT.Core.Repositorio.BaseEntity;
using System.Data.Entity.ModelConfiguration;

namespace UMBIT.Core.Repositorio.EntityConfigurate
{
    public abstract class CoreEntityConfigurate<T> : EntityTypeConfiguration<T> where T : CoreBaseEntity
    {
        public CoreEntityConfigurate()
        {
            HasKey((T be) => be.Id);
            ConfigureEntidade();
            Property((T be) => be.DataCriacao).IsRequired();
            Property((T be) => be.DataAtualizacao).IsRequired();
        }

        public abstract void ConfigureEntidade();
    }
}
