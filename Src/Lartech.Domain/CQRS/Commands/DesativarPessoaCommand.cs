using Lartech.Domain.Core.Messages;

namespace Lartech.Domain.CQRS.Commands
{
    public class DesativarPessoaCommand : Command
    {
        public Guid Id { get; private set; }

        public DesativarPessoaCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

    }
}
