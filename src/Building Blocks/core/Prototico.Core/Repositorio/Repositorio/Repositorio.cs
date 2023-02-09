using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Prototico.Core.Repositorio.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        protected readonly DbContext Contexto;

        protected const int TAMANHO_PAGINA = 50;
        protected DbContextTransaction Transacao { get; set; }
        private bool TransacaoAberta { get; set; }
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
                return this.Contexto.Set<T>().Attach(objeto);
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

        public int SalveAlteracoes()
        {
            return this.Contexto.SaveChanges();
        }

        public void InicieTransacao([CallerFilePath] string arquivo = null, [CallerMemberName] string metodo = null)
        {
            if (!this.TransacaoAberta)
            {
                this.Transacao = this.Contexto.Database.BeginTransaction();

                this.TransacaoAberta = true;
            }
        }

        public void FinalizeTransacao([CallerFilePath] string arquivo = null, [CallerMemberName] string metodo = null)
        {
            if (this.TransacaoAberta)
            {
                this.Transacao.Commit();
                this.Transacao.Dispose();

                this.TransacaoAberta = false;
            }
        }

        public void RevertaTransacao([CallerFilePath] string arquivo = null, [CallerMemberName] string metodo = null)
        {
            if (this.TransacaoAberta )
            {
                this.Transacao.Rollback();
                this.Transacao.Dispose();

                this.TransacaoAberta = false;
            }
        }

        public void Dispose()
        {
            if (this.Transacao != null)
            {
                this.Transacao.Dispose();
            }

            this.Contexto.Dispose();
        }
    }
}
