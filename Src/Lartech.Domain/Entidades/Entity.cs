using FluentValidation.Results;

namespace Lartech.Domain.Entidades
{
    public abstract class Entity
    {
        public Entity()
        {
            Id = Guid.NewGuid();
            ListaErros = new List<string>();
        }
        public Guid Id { get; private set; }
        public List<string> ListaErros { get; private set; }

        public void AtribuirId(Guid id)
        {
            Id = id;
        }

        public ValidationResult ValidationResult { get; set; }
        public abstract bool Validar();


    }
}
