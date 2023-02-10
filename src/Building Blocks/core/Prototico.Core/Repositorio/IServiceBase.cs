
using UMBIT.Core.Repositorio.Repositorio;
using System;
using System.Collections.Generic;

namespace UMBIT.Core.Repositorio
{
    public interface IServiceBase<T> where T : class
    {
        void AdicionaObjeto(T Entidade);
        T ObtenhaEntidade(Guid id);
        IEnumerable<T> ObtenhaEntidades();
        void RemovaEntidade(T Entidade);
        void AtualizeEntidade(T Entidade);

    }

    public abstract class ServiceBase<T> : IServiceBase<T> where T : class
    {
        private IRepositorio<T> Repositorio { get; set; }
        public ServiceBase(DataServiceFactory dataServiceFactory)
        {
            this.Repositorio = dataServiceFactory.GetRepositorio<T>();
        }
        public void AtualizeEntidade(T Entidade)
        {
            try
            {
                this.Repositorio.Atualize(Entidade);
                this.Repositorio.SalveAlteracoes();
            }
            catch(Exception ex)
            {
                throw new Exception("Erro Ao atualizar entidade",ex);
            }
        }   

        public T ObtenhaEntidade(Guid id)
        {
            try
            {
               return this.Repositorio.ObtenhaUnico(id);
                
            }
            catch (Exception ex)
            {
                throw new Exception("Erro Ao obter entidade", ex);
            }
        }

        public IEnumerable<T> ObtenhaEntidades()
        {
            try
            {
               return this.Repositorio.ObtenhaTodos();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro Ao obter entidades", ex);
            }
        }

        public void RemovaEntidade(T Entidade)
        {
            try
            {
                this.Repositorio.Remova(Entidade);
                this.Repositorio.SalveAlteracoes();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro Ao remover entidade");
            }
        }

        public void AdicionaObjeto(T Entidade)
        {
            this.AdicionaObjeto(Entidade);
            this.Repositorio.SalveAlteracoes();
        }
    }
}
