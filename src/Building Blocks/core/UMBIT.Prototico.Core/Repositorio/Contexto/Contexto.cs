using Microsoft.EntityFrameworkCore;
using UMBIT.Core.Repositorio.BaseEntity;
using UMBIT.Core.Repositorio.EntityConfigurate;
using System;
using System.Linq;
using System.Reflection;
namespace UMBIT.Core.Repositorio.Contexto
{

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    if (assembly != null)
                        assembly.GetTypes()
                        .Where(t =>
                            t != null &&
                            t.Namespace != null &&
                            t.BaseType != null &&
                            t.IsClass &&
                            t.BaseType.IsGenericType &&
                            (t.BaseType.GetGenericTypeDefinition() == typeof(CoreEntityConfigurate<>)))
                        .ToList().ForEach((t) =>
                        {
                            dynamic instanciaDeConfiguracao = Activator.CreateInstance(t);
                            modelBuilder.ApplyConfiguration(instanciaDeConfiguracao);
                        });
                }
                catch
                {
                    continue;
                }

            }
        }

        public override int SaveChanges()
        {
            var objetos = ChangeTracker.Entries().Where(x =>
                      x.Entity is CoreBaseEntity && (x.State == EntityState.Added ||
                      x.State == EntityState.Modified || x.State == EntityState.Deleted));

            foreach (var objeto in objetos)
            {
                var baseEntity = objeto.Entity as CoreBaseEntity;

                if (objeto.State == EntityState.Added)
                {
                    if (baseEntity.Id == Guid.Empty)
                    {
                        baseEntity.Id = Guid.NewGuid();
                    }

                    baseEntity.DataCriacao = DateTime.Now;
                    baseEntity.DataAtualizacao = DateTime.Now;
                }
                else if (objeto.State == EntityState.Modified)
                {
                    baseEntity.DataAtualizacao = DateTime.Now;
                }
            }
            return base.SaveChanges();

        }

    }

}
