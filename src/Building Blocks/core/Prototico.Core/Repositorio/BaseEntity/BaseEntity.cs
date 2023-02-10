using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMBIT.Core.Repositorio.BaseEntity
{
    public abstract class CoreBaseEntity
    {
        public Guid Id { get; set; } 
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get;set; }
    }
}
