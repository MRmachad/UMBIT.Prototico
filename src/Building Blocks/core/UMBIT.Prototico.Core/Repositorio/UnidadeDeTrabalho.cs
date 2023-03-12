
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Runtime.CompilerServices;
using UMBIT.Core.Repositorio.Repositorio;
namespace UMBIT.Core.Repositorio
{
    public class UnidadeDeTrabalho : IUnidadeDeTrabalho
    {
        private DbContext DbContext { get; set; }
        protected IDbContextTransaction Transacao { get; set; }
        private bool TransacaoAberta { get; set; }
        public UnidadeDeTrabalho(DbContext dbContext)
        {
            this.DbContext = dbContext;
        }
        public IRepositorio<T> GetRepositorio<T>() where T : class
        {
            return new Repositorio<T>(this.DbContext);
        }
        public int SalveAlteracoes()
        {
            return this.DbContext.SaveChanges();
        }

        public void InicieTransacao([CallerFilePath] string arquivo = null, [CallerMemberName] string metodo = null)
        {
            if (!this.TransacaoAberta)
            {
                this.Transacao = this.DbContext.Database.BeginTransaction();

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
            if (this.TransacaoAberta)
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

            this.DbContext.Dispose();
        }
    }
}
