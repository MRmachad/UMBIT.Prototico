using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace UMBIT.Core.Repositorio.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        protected readonly DbContext Contexto;

        protected const int TAMANHO_PAGINA = 50;

        public Repositorio(DbContext contexto)
        {
            this.Contexto = contexto;
        }

        public virtual IEnumerable<T> ObtenhaTodos()
        {
            return MiddlewareDeRepositorio(() =>
            {
                return this.Contexto.Set<T>().AsNoTracking();
            });
        }

        public virtual T ObtenhaUnico(params object[] args)
        {
            return MiddlewareDeRepositorio(() =>
            {
                return this.Contexto.Set<T>().Find(args);
            });
        }

        public virtual T Carregue(T objeto)
        {
            return MiddlewareDeRepositorio(() =>
            {
                return this.Contexto.Set<T>().Attach(objeto) as T;
            });
        }

        public virtual IEnumerable<T> Filtre(Expression<Func<T, bool>> predicado)
        {
            return MiddlewareDeRepositorio(() =>
            {
                return this.Contexto.Set<T>().AsNoTracking().Where(predicado);
            });
        }

        public virtual void Adicione(T objeto)
        {
            MiddlewareDeRepositorio(() =>
            {
                this.Contexto.Set<T>().Add(objeto);
            });
        }

        public virtual void Adicione(IEnumerable<T> objetos)
        {
            MiddlewareDeRepositorio(() =>
            {
                this.Contexto.Set<T>().AddRange(objetos);
            });
        }

        public virtual void Atualize(T objeto)
        {
            MiddlewareDeRepositorio(() =>
            {
                var entry = this.Contexto.Entry(objeto);
                if (entry.State == EntityState.Detached)
                {
                    this.Carregue(objeto);
                }

                this.Contexto.Entry<T>(objeto).State = EntityState.Modified;
            });
        }

        public virtual void Remova(T objeto)
        {
            MiddlewareDeRepositorio(() =>
            {
                var entry = this.Contexto.Entry(objeto);
                if (entry.State == EntityState.Detached)
                {
                    this.Carregue(objeto);
                }

                this.Contexto.Set<T>().Remove(objeto);
            });
        }

        protected void MiddlewareDeRepositorio(Action method)
        {
            try
            {
                method();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro no processamento do banco de dados. Contate o administrador.", ex);
            }
        }

        protected TRes MiddlewareDeRepositorio<TRes>(Func<TRes> method)
        {
            try
            {
                return method();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro no processamento do banco de dados. Contate o administrador.", ex);
            }
        }

        
    }
}
