using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UMBIT.Core.Repositorio.BaseEntity;

namespace UMBIT.Core.Repositorio.EntityConfigurate
{
    public abstract class CoreEntityConfigurate<T> : IEntityTypeConfiguration<T> where T : CoreBaseEntity
    {

        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey((T be) => be.Id);
            ConfigureEntidade(builder);
            builder.Property((T be) => be.DataCriacao).IsRequired();
            builder.Property((T be) => be.DataAtualizacao).IsRequired();
        }

        
        public abstract void ConfigureEntidade(EntityTypeBuilder<T> builder);
    }
}
