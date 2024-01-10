using FluentValidation;
using System.Text.RegularExpressions;

namespace Lartech.Domain.Entidades
{

    public enum TipoTelefone
    {
        Celular,
        Residencial,
        Comercial
    }



    public class Telefone : Entity
    {

        public Telefone()
        {

        }

        public Telefone(Guid pessoaId, TipoTelefone tipo, string numero)
        {
            PessoaId = pessoaId;
            Tipo = tipo;
            Numero = numero;
        }


        public Guid PessoaId { get; private set; }
        public TipoTelefone Tipo {  get; private set; }
        public string Numero { get; private set; }

        // EF
        public Pessoa Pessoa { get; set; }

        public void AtribuirIdPessoa(Guid idpessoa)
        {
            PessoaId = idpessoa;
        }

        public void AtribuirTipo(TipoTelefone tipo)
        {
            Tipo = tipo;
        }

        public void AtribuirNumero(string numero)
        {
            Numero = numero;
        }

        public override bool Validar()
        {
            ValidationResult = new TelefoneValidation().Validate(this);
            foreach (var erro in ValidationResult.Errors)
            {
                ListaErros.Add(erro.ErrorMessage);
            }
            return ValidationResult.IsValid;
        }

        public class TelefoneValidation : AbstractValidator<Telefone>
        {
            public TelefoneValidation()
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
