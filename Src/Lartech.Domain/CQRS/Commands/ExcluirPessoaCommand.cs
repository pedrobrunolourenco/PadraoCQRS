using Lartech.Domain.Core.Messages;

namespace Lartech.Domain.CQRS.Commands
{
    public class ExcluirPessoaCommand : Command
    {
        public Guid IdPessoa { get; private set; }


        public ExcluirPessoaCommand(Guid idpessoa)
        {
            IdPessoa = idpessoa;
            AggregateId = idpessoa;
        }
    }
}
