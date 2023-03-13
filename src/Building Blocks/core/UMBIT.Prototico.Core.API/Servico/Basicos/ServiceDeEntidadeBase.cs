using System;
using System.Collections.Generic;
using UMBIT.Core.Repositorio;
using UMBIT.Core.Repositorio.Repositorio;
using UMBIT.CORE.API.Servico.Interface;

namespace UMBIT.Prototico.Core.API.Servico.Basicos
{
    public abstract class ServicoDeEntidadeBase<T> : IServicoDeEntidadeBase<T> where T : class
    {
        private IRepositorio<T> Repositorio { get; set; }
        private IUnidadeDeTrabalho UnidadeDeTrabalho { get; set; }
        public ServicoDeEntidadeBase(IUnidadeDeTrabalho dataServiceFactory)
        {
            Repositorio = dataServiceFactory.GetRepositorio<T>();
        }
        public void AtualizeEntidade(T Entidade)
        {
            try
            {
                Repositorio.Atualize(Entidade);
                UnidadeDeTrabalho.SalveAlteracoes();
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
                return Repositorio.ObtenhaUnico(id);

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
                return Repositorio.ObtenhaTodos();
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
                Repositorio.Remova(Entidade);
                UnidadeDeTrabalho.SalveAlteracoes();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro Ao remover entidade", ex);
            }
        }

        public void AdicionaObjeto(T Entidade)
        {
            Repositorio.Adicione(Entidade);
            UnidadeDeTrabalho.SalveAlteracoes();
        }
    }
}
