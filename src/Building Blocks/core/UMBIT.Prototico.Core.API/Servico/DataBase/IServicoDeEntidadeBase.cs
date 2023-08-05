using System;
using System.Collections.Generic;

namespace UMBIT.Prototico.Core.API.Servico.DataBase
{
    public interface IServicoDeEntidadeBase<T> where T : class
    {
        void AdicionaObjeto(T Entidade);
        T ObtenhaEntidade(Guid id);
        IEnumerable<T> ObtenhaEntidades();
        void RemovaEntidade(T Entidade);
        void AtualizeEntidade(T Entidade);

    }
}
