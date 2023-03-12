using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Runtime.CompilerServices;

namespace UMBIT.Core.Repositorio.Repositorio
{
    public interface IRepositorio<T> where T : class
    {
        /// <summary>
        /// Obtenha Todos os Registros
        /// </summary>
        /// <returns>Lista de Registros</returns>
        IEnumerable<T> ObtenhaTodos();

        /// <summary>
        /// Obtenha objeto único a partir da chave primária
        /// </summary>
        /// <param name="args">Chave primária</param>
        /// <returns>Objeto único</returns>
        T ObtenhaUnico(params object[] args);

        /// <summary>
        /// Filtra objetos
        /// </summary>
        /// <param name="predicado">Predicado de filtragem</param>
        /// <returns>Lista de Registros filtrados</returns>
        IEnumerable<T> Filtre(Expression<Func<T, bool>> predicado);

        /// <summary>
        /// Obtenha objeto único a partir da chave primária
        /// </summary>
        /// <param name="args">Chave primária</param>
        /// <returns>Objeto único</returns>
        T Carregue(T Objeto);

        /// <summary>
        /// Adiciona objeto na Base de Dados
        /// </summary>
        /// <param name="objeto">Objeto a ser adicionado</param>
        void Adicione(T objeto);

        /// <summary>
        /// Adiciona objetos na Base de Dados
        /// </summary>
        /// <param name="objeto">Objetos a serem adicionados</param>
        void Adicione(IEnumerable<T> objetos);

        /// <summary>
        /// Atualiza objeto na Base de Dados
        /// </summary>
        /// <param name="objeto">Objeto a ser atualizado</param>
        void Atualize(T objeto);

        /// <summary>
        /// Remova objeto da base de dados
        /// </summary>
        /// <param name="objeto">Objeto a ser removido</param>
        void Remova(T objeto);
    }
}