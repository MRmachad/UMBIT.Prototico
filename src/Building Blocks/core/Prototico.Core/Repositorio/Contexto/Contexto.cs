using Microsoft.EntityFrameworkCore;
using UMBIT.Core.Repositorio.BaseEntity;
using UMBIT.Core.Repositorio.EntityConfigurate;
using System;
using System.Linq;
using System.Reflection;

namespace UMBIT.Core.Repositorio.Contexto
{

    public class ContextoDB : DbContext
    {
        public ContextoDB(DbContextOptions options) : base(options)
        {
        }
        
        protected virtual void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in a.GetTypes())
                {

                    if (t != null && t == typeof(CoreEntityConfigurate<>))
                    {
                        dynamic instanciaDeConfiguracao = Activator.CreateInstance(t);
                        modelBuilder.Configurations.Add(instanciaDeConfiguracao);
                    }
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
