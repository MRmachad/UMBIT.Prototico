using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMBIT.Core.Data.EventSourcing
{
    public class StoredEvent
    {
        public Guid Id { get; set; }
        public string Tipo { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public string Dados { get; set; }

        public StoredEvent(Guid id, string tipo, DateTime dataOcorrencia, string dados)
        {
            this.Id = id;
            this.Tipo = tipo;
            this.DataOcorrencia = dataOcorrencia;
            this.Dados = dados;
        }

    }
}
