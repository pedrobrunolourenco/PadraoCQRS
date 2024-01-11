﻿using FluentValidation;
using Lartech.Domain.Core.Extensions;
using Lartech.Domain.Core.Messages;
using Lartech.Domain.Entidades;
using System.Text.RegularExpressions;

namespace Lartech.Domain.CQRS.Commands
{
    public class AdicionarPessoaCommand : Command
    {
        public Guid IdPessoa { get; private set; }
        public string Nome { get; private set; }
        public string CPF { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public bool Ativo { get; private set; }
        public List<Telefone?> ListaTelefones { get; private set; }
        public List<string> ListaErros { get; private set; }




        public AdicionarPessoaCommand(Guid idpessoa, string nome, string cpf, DateTime datanasimento, bool ativo, List<Telefone?> listaTelefone)
        {
            IdPessoa = idpessoa;
            Nome = nome;
            CPF = cpf;  
            DataNascimento = datanasimento;
            Ativo = ativo;
            ListaTelefones = listaTelefone;
            ListaErros = new List<string>();
            AggregateId = idpessoa;
        }


        public override bool EhValido()
        {
            ValidationResult = new AdicionarPessoaCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }


    }


    public class AdicionarPessoaCommandValidation : AbstractValidator<AdicionarPessoaCommand>
    {
        public AdicionarPessoaCommandValidation()
        {
            RuleFor(p => p.IdPessoa)
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

            RuleFor(p => p.ListaTelefones)
                .NotEmpty()
                .WithMessage("Informe um telefone.");
        }

        public static bool ValidarCPF(string cpf)
        {
            return ValidacaoCpfExtension.ValidarCPF(cpf);
        }
    }

}
