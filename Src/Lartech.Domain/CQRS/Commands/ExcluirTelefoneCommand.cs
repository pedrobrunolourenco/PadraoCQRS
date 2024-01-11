using Lartech.Domain.Core.Messages;

namespace Lartech.Domain.CQRS.Commands
{
    public class ExcluirTelefoneCommand : Command
    {
        public Guid Id { get; private set; }

        public List<string> ListaErros { get; private set; }

        public ExcluirTelefoneCommand(Guid id)
        {
            Id = id;
            ListaErros = new List<string>();
            AggregateId = id;
        }

    }
}
