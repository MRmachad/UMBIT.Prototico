namespace UMBIT.CORE.API.Servico.Interface
{
    public interface IServiceBase<T> where T : class
    {
        void AdicionaObjeto(T Entidade);
        T ObtenhaEntidade(Guid id);
        IEnumerable<T> ObtenhaEntidades();
        void RemovaEntidade(T Entidade);
        void AtualizeEntidade(T Entidade);

    }
}
