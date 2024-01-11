using FluentValidation;
using Lartech.Domain.Core.Enum;
using Lartech.Domain.Core.Messages;

namespace Lartech.Domain.CQRS.Commands
{
    public class AlterarTelefoneCommand : Command
    {
        public Guid Id { get; private set; }
        public Guid PessoaId { get; private set; }
        public TipoTelefone Tipo { get; private set; }
        public string Numero { get; private set; }
        public List<string> ListaErros { get; private set; }

        public AlterarTelefoneCommand(Guid id, Guid pessoaId, TipoTelefone tipo, string numero)
        {
            Id = id;
            PessoaId = pessoaId;
            Tipo = tipo;
            Numero = numero;
            ListaErros = new List<string>();
            AggregateId = pessoaId;
        }

        public override bool EhValido()
        {
            ValidationResult = new AlterarTelefoneCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }


        public class AlterarTelefoneCommandValidation : AbstractValidator<AlterarTelefoneCommand>
        {
            public AlterarTelefoneCommandValidation()
            {
                RuleFor(t => t.Id)
                     .NotEqual(Guid.Empty)
                     .WithMessage("Id não pode ser vazio.");

                RuleFor(t => t.Tipo)
                    .IsInEnum();

                RuleFor(t => t.Numero)
                    .MinimumLength(10)
                    .WithMessage("Informe o telefone com o DDD.");

                RuleFor(t => t.Numero)
                    .MaximumLength(11)
                    .WithMessage("O telefone deve ter 11 digitos.");
            }
        }

    }
}
