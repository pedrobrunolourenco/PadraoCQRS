using FluentValidation;
using Lartech.Domain.Core.Extensions;
using Lartech.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace Lartech.Domain.Entidades
{
    public class Pessoa : Entity, IAggregateRoot
    {
        public Pessoa()
        {
            
        }
        public Pessoa(string nome, string cpf, DateTime datanascimento, bool ativo)
        {
            Nome = nome;
            CPF = cpf;
            DataNascimento = datanascimento;
            Ativo = ativo;
            ListaTelefones = new List<Telefone?>();
        }


        public string Nome { get; private set; }
        public string CPF { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public bool Ativo { get; private set; }
        public List<Telefone?> ListaTelefones { get; private set; }

        private readonly List<Telefone> _telefones;

        public IReadOnlyCollection<Telefone> Telefones=> _telefones;

        public void Ativar()
        {
            Ativo = true;
        }

        public void Inativar()
        {
            Ativo = false;
        }

        public void AtriuirNome(string nome)
        {
            Nome = nome;
        }
        public void AtriuirCPF(string cpf)
        {
            CPF = cpf;
        }

        public void AtriuirDataNascimento(DateTime datanascimento)
        {
            DataNascimento = datanascimento;
        }

        public void AdicionarTelefoneNaLista(Telefone telefone)
        {
            telefone.AtribuirIdPessoa(Id);
            ListaTelefones.Add(telefone);
        }

        public override bool Validar()
        {
            ValidationResult = new PessoaValidation().Validate(this);
            foreach (var erro in ValidationResult.Errors)
            {
               ListaErros.Add(erro.ErrorMessage);
            }
            return ValidationResult.IsValid;
        }

        public class PessoaValidation : AbstractValidator<Pessoa>
        {
            public PessoaValidation()
            {
                RuleFor(p => p.Id)
                     .NotEqual(Guid.Empty)
                     .WithMessage("Id não pode ser vazio.");

                RuleFor(p => p.Nome)
                    .NotEmpty()
                    .WithMessage("O nome deve ser informado.");

                RuleFor(p => p.Nome)
                    .MinimumLength(5)
                    .WithMessage("O nome deve ter no mínimo 5 caracteres.");
           
                RuleFor(p => p.Nome)
                    .MaximumLength(100)
                    .WithMessage("O nome deve ter no máximo 100 caracteres.");

                RuleFor(p => p.DataNascimento)
                     .NotNull()
                     .WithMessage("Data de nascimento deve ser informada.");

                RuleFor(p => p.DataNascimento)
                    .LessThan(DateTime.Today.AddDays(-1))
                    .WithMessage("Data de nascimento deve ser inferior ao dia de hoje.");

                RuleFor(p => p.CPF)
                    .Must(ValidarCPF)
                    .WithMessage("CPF inválido.");

            }


            public static bool ValidarCPF(string cpf)
            {
                return ValidacaoCpfExtension.ValidarCPF(cpf);
            }
        }
    }
}
