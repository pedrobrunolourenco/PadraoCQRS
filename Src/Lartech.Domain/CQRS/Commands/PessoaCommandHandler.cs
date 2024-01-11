using Lartech.Domain.Core.Comunicacao.Mediator;
using Lartech.Domain.Core.Messages;
using Lartech.Domain.Core.Messages.CommonMessges;
using Lartech.Domain.Entidades;
using Lartech.Domain.Interfaces.Repository;
using MediatR;

namespace Lartech.Domain.CQRS.Commands
{
    public class PessoaCommandHandler :
        IRequestHandler<AdicionarPessoaCommand, bool>,
        IRequestHandler<AlterarPessoaCommand, bool>,
        IRequestHandler<ExcluirPessoaCommand, bool>,
        IRequestHandler<AdicionarTelefoneCommand, bool>,
        IRequestHandler<AlterarTelefoneCommand, bool>,
        IRequestHandler<ExcluirTelefoneCommand, bool>,
        IRequestHandler<AtivarPessoaCommand, bool>,
        IRequestHandler<DesativarPessoaCommand, bool>


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
            if (await VerificarSeCPFJaExiste(message.CPF, message.IdPessoa)) 
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, $"O CPF {message.CPF} já existe para outra pessoa."));
                return false;
            }
            if (!await AdicionouTelefone(message)) return false;
            var pessoa = new Pessoa(message.Nome, message.CPF, message.DataNascimento, true);
            pessoa.AtribuirId(message.IdPessoa);
            await _repositoryPessoa.Adicionar(pessoa);
            await _repositoryPessoa.Salvar();
            return true;
        }

        public async Task<bool> Handle(AlterarPessoaCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;
            var _pessoa = await _repositoryPessoa.BuscarId(message.IdPessoa);
            if (_pessoa == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, "Pessoa não localizado."));
                return false;
            }
            _pessoa.AtriuirNome(message.Nome);
            _pessoa.AtriuirCPF(message.CPF);
            _pessoa.AtriuirDataNascimento(message.DataNascimento);
            if (await VerificarSeCPFJaExiste(message.CPF, message.IdPessoa))
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, $"O CPF {message.CPF} já existe para outra pessoa."));
                return false;
            }

            await _repositoryPessoa.Atualizar(_pessoa);
            await _repositoryPessoa.Salvar();
            return true;
        }

        public async Task<bool> Handle(ExcluirPessoaCommand message, CancellationToken cancellationToken)
        {

            var pessoa = await _repositoryPessoa.BuscarId(message.IdPessoa);
            if (pessoa == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, "Pessoa não localizada."));
                return false;
            }
            await _repositoryPessoa.Remover(pessoa);
            await _repositoryPessoa.Salvar();
            return true;
        }

        public async Task<bool> Handle(AdicionarTelefoneCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;
            var telefone = new Telefone(message.PessoaId, message.Tipo, message.Numero);
            if (await VerificarSeTelefoneJaExiste(message)) return false;
            telefone.AtribuirId(message.Id);
            telefone.AtribuirIdPessoa(message.PessoaId);
            await _repositoryTelefone.Adicionar(telefone);
            await _repositoryPessoa.Salvar();
            return true;
        }

        public async Task<bool> Handle(AlterarTelefoneCommand message, CancellationToken cancellationToken)
        {

            if (!ValidarComando(message)) return false;
            var telefone = await _repositoryTelefone.BuscarId(message.Id);
            if (telefone == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, "Telefone não localizado."));
                return false;
            }
            telefone.AtribuirId(telefone.Id);
            telefone.AtribuirIdPessoa(telefone.PessoaId);
            telefone.AtribuirTipo(message.Tipo);
            telefone.AtribuirNumero(message.Numero);
            await _repositoryTelefone.Atualizar(telefone);
            await _repositoryTelefone.Salvar();
            return true;
        }

        public async Task<bool> Handle(ExcluirTelefoneCommand message, CancellationToken cancellationToken)
        {
            var telefone = await _repositoryTelefone.BuscarId(message.Id);
            if (telefone == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, "Telefone não localizado."));
                return false;
            }
            await _repositoryTelefone.Remover(telefone);
            await _repositoryTelefone.Salvar();
            return true;
        }

        public async Task<bool> Handle(AtivarPessoaCommand message, CancellationToken cancellationToken)
        {
            var pessoa = await _repositoryPessoa.BuscarId(message.Id);
            if (pessoa == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, "Pessoa não localizada."));
                return false;
            }
            pessoa.Ativar();
            await _repositoryPessoa.Atualizar(pessoa);
            await _repositoryPessoa.Salvar();
            return true;
        }

        public async Task<bool> Handle(DesativarPessoaCommand message, CancellationToken cancellationToken)
        {
            var pessoa = await _repositoryPessoa.BuscarId(message.Id);
            if (pessoa == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, "Pessoa não localizada."));
                return false;
            }
            pessoa.Inativar();
            await _repositoryPessoa.Atualizar(pessoa);
            await _repositoryPessoa.Salvar();
            return true;
        }

        private async Task<bool> AdicionouTelefone(AdicionarPessoaCommand message)
        {
            foreach (var telefone in message.ListaTelefones)
            {
                if (telefone != null && telefone.Validar())
                {
                    telefone.AtribuirIdPessoa(message.IdPessoa);
                    await _repositoryTelefone.Adicionar(telefone);
                }
                else
                {
                    if (telefone != null)
                    {
                        foreach (var erroTelefone in telefone.ListaErros)
                        {
                            await _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, erroTelefone));
                        }
                    }
                    return false;
                }
            }
            return true;
        }

        private async Task<bool> VerificarSeTelefoneJaExiste(AdicionarTelefoneCommand message)
        {
            var telefones = await _repositoryTelefone.Listar();
            var result = telefones.Where(t => t.Numero == message.Numero && t.PessoaId == message.PessoaId);
            if (result.Any())
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, $"O telefone {message.Numero} já existe para esta pessoa."));
                return true;
            }
            return false;
        }


        private async Task<bool> VerificarSeCPFJaExiste(string cpf , Guid idpessoa)
        {
            var result = await _repositoryPessoa.ObterPorCpf(cpf, idpessoa);
            if (result != null && result.Id != idpessoa)
            {
                return true;
            }
            return false;
        }


        private bool ValidarComando(Command message)
        {
            if (message.EhValido()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, error.ErrorMessage));
            }
            return false;
        }

    }
}
