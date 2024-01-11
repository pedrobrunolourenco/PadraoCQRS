using Lartech.Domain.Core.Messages;

namespace Lartech.Domain.CQRS.Commands
{
    public class ExcluirTelefoneCommand : Command
    {
        public Guid Id { get; private set; }

        public ExcluirTelefoneCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

    }
}
