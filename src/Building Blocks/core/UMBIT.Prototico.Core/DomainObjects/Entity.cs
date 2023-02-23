using System;

namespace UMBIT.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public DateTime DateCriacao { get; set; }
        public DateTime DateAtualizacao { get; set; }
    }
}
