using Lartech.Domain.Core.Comunicacao.Mediator;
using Lartech.Domain.Core.Messages;
using Lartech.Domain.Core.Messages.CommonMessges;
using Lartech.Domain.Entidades;
using Lartech.Domain.Interfaces.Repository;
using MediatR;
using System.Reflection.Metadata.Ecma335;

namespace Lartech.Domain.CQRS.Commands
{
    public class PessoaCommandHandler :
        IRequestHandler<AdicionarPessoaCommand, bool>,
        IRequestHandler<AlterarPessoaCommand, bool>
    {
        private readonly IRepositoryPessoa _repositoryPessoa;
        private readonly IRepositoryTelefone _repositoryTelefone;
        private readonly IMediatrHandler _mediatorHandler;


        public PessoaCommandHandler(IRepositoryPessoa repositoryPessoa,
                                    IRepositoryTelefone repositoryTelefone,
                                    IMediatrHandler mediatorHandler)
        {
            _repositoryPessoa = repositoryPessoa;
            _repositoryTelefone = repositoryTelefone;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(AdicionarPessoaCommand message, CancellationToken cancellationToken)
        {

            if (!ValidarComando(message)) return false;
            if (await VerificarSeCPFJaExiste(message)) return false ;
            if (!await AdicionouTelefone(message)) return false;
            var pessoa = new Pessoa(message.Nome, message.CPF,message.DataNascimento,true);
            if (!pessoa.Validar()) return false;
            pessoa.AtribuirId(message.IdPessoa);
            await _repositoryPessoa.Adicionar(pessoa);
            await _repositoryPessoa.Salvar();
            return true;
        }

        public async Task<bool> Handle(AlterarPessoaCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;
            var _pessoa = await _repositoryPessoa.BuscarId(message.IdPessoa);
            if (_pessoa == null) return false;

            _pessoa.AtriuirNome(_pessoa.Nome);
            _pessoa.AtriuirCPF(_pessoa.CPF);
            _pessoa.AtriuirDataNascimento(_pessoa.DataNascimento);

            if (!_pessoa.Validar()) return false;
            if( await VerificarSeCPFJaExisteAlteracao(message) ) return false; ;
            await _repositoryPessoa.Atualizar(_pessoa);
            await _repositoryPessoa.Salvar();
            return true;
        }


        private async Task<bool> AdicionouTelefone(AdicionarPessoaCommand message)
        {
            foreach (var telefone in message.ListaTelefones)
            {
                if (telefone.Validar())
                {
                    telefone.AtribuirIdPessoa(message.IdPessoa);
                    await _repositoryTelefone.Adicionar(telefone);
                }
                else
                {
                    foreach (var erroTelefone in telefone.ListaErros)
                    {
                        message.ListaErros.Add(erroTelefone);
                    }
                    return false;
                }
            }
            return true;
        }


        private async Task<bool> VerificarSeCPFJaExiste(AdicionarPessoaCommand message)
        {
            var result = await _repositoryPessoa.ObterPorCpf(message.CPF, message.IdPessoa);
            if (result != null && result.Id != message.IdPessoa) 
            {
                message.ListaErros.Add($"O CPF {message.CPF} já existe para outra pessoa.");
                return true;
            }
            return false;
        }

        private async Task<bool> VerificarSeCPFJaExisteAlteracao(AlterarPessoaCommand message)
        {
            var result = await _repositoryPessoa.ObterPorCpf(message.CPF);
            if (result != null)
            {
                message.ListaErros.Add($"O CPF {message.CPF} já existe para outra pessoa.");
                return true;
            }
            return false;
        }


        private bool ValidarComando(AdicionarPessoaCommand message)
        {
            if (message.EhValido()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {

                message.ListaErros.Add(error.ErrorMessage);
                _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }

        private bool ValidarComando(AlterarPessoaCommand message)
        {
            if (message.EhValido()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {

                message.ListaErros.Add(error.ErrorMessage);
                _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }


    }
}
