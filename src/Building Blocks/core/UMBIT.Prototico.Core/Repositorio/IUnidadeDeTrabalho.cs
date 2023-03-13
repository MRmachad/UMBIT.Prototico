
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Runtime.CompilerServices;
using UMBIT.Core.Repositorio.Repositorio;
namespace UMBIT.Core.Repositorio
{
    public interface IUnidadeDeTrabalho : IDisposable
    {
        IRepositorio<T> GetRepositorio<T>() where T : class;

        int SalveAlteracoes();
        void InicieTransacao([CallerFilePath] string arquivo = null, [CallerMemberName] string metodo = null);

        void FinalizeTransacao([CallerFilePath] string arquivo = null, [CallerMemberName] string metodo = null);

        void RevertaTransacao([CallerFilePath] string arquivo = null, [CallerMemberName] string metodo = null);
    }

}
