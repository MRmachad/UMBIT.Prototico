using System;
using System.Collections.Generic;
using UMBIT.Core.Repositorio;
using UMBIT.Core.Repositorio.Repositorio;
using UMBIT.CORE.API.Servico.Interface;

namespace UMBIT.CORE.API.Servico
{
    public abstract class ServiceBase<T> : IServiceBase<T> where T : class
    {
        private IRepositorio<T> Repositorio { get; set; }
        public ServiceBase(IDataServiceFactory dataServiceFactory)
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
            catch (Exception ex)
            {
                throw new Exception("Erro Ao atualizar entidade", ex);
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
                throw new Exception("Erro Ao remover entidade", ex);
            }
        }

        public void AdicionaObjeto(T Entidade)
        {
            this.Repositorio.Adicione(Entidade);
            this.Repositorio.SalveAlteracoes();
        }
    }
}
