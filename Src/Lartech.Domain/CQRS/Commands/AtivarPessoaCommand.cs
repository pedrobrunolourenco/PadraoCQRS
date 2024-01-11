using Lartech.Domain.Core.Messages;

namespace Lartech.Domain.CQRS.Commands
{
    public class AtivarPessoaCommand : Command
    {
        public Guid Id { get; private set; }

        public AtivarPessoaCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

    }
}
