using AutoMapper;
using Lartech.Application.Interfaces;
using Lartech.Application.Models;
using Lartech.Domain.Core.Comunicacao.Mediator;
using Lartech.Domain.Core.Messages.CommonMessges;
using Lartech.Domain.CQRS.Commands;
using Lartech.Domain.CQRS.Queries;
using Lartech.Domain.DTOS;
using Lartech.Domain.Entidades;
using MassTransit;
using MediatR;

namespace Lartech.Application.Services
{
    public class AppPessoa : IAppPessoa
    {
        private readonly DomainNotificationHandler _notifications;

        private readonly IMapper _mapper;
        private readonly IPessoaQuery _queryPessoa;
        private readonly IMediatrHandler _mediatrHandler;
        private readonly IPublishEndpoint _publish;
        private readonly IBus _bus;



        public AppPessoa(IMapper mapper,
                         INotificationHandler<DomainNotification> notifications,
                         IBus bus,
                         IPublishEndpoint publish,
                         IMediatrHandler mediatrHandler,
                         IPessoaQuery queryPessoa)
        {
            _mapper = mapper;
            _notifications = (DomainNotificationHandler)notifications;
            _mediatrHandler = mediatrHandler;
            _queryPessoa = queryPessoa;
            _publish = publish;
            _bus = bus;
        }

        protected bool OperacaoValida()
        {
            return (!_notifications.TemNotificacao());
        }

        protected List<string> ObterMensagensDeErro()
        {
            return _notifications.ObterNotificacoes().Select(c => c.Value).ToList();
        }

        protected void NotificarErros(string codigo, string mensagem)
        {
            _mediatrHandler.PublicarNotificacao(new DomainNotification(codigo, mensagem));
        }


        public async Task<IEnumerable<PessoaViewModel>> ObterTodos()
        {
            return await _queryPessoa.ObterTodos();
        }

        public async Task<IEnumerable<PessoaViewModel>> ObterAtivos()
        {
            return await _queryPessoa.ObterAtivos();
        }

        public async Task<IEnumerable<PessoaViewModel>> ObterInativos()
        {
            return await _queryPessoa.ObterInativos();
        }
        public async Task<PessoaViewModel?> ObterPorId(Guid id)
        {
            return await _queryPessoa.ObterPorId(id);
        }

        public async Task<PessoaViewModel?> ObterPorCpf(string cpf)
        {
            return await _queryPessoa.ObterPorCpf(cpf);
        }

        public async Task<IEnumerable<PessoaViewModel>> ObterPorParteDoNome(string nome)
        {
            return await _queryPessoa.ObterPorParteDoNome(nome);
        }

        public async Task<PessoaModel> IncluirPessoa(PessoaModel pessoa)
        {
            var _pessoa = _mapper.Map<Pessoa>(pessoa);
            foreach (var fone in pessoa.ListaTelefone)
            {
                _pessoa.AdicionarTelefoneNaLista(_mapper.Map<Telefone>(fone));
            }

            var command = new AdicionarPessoaCommand(_pessoa.Id, _pessoa.Nome, _pessoa.CPF, _pessoa.DataNascimento, _pessoa.Ativo, _pessoa.ListaTelefones);
            await _mediatrHandler.EnviarCommand(command);


            return await TranformaEmPessoaModel(command, _pessoa);

            //if (result != null)
            //{
            //    Uri uri = new Uri("rabbitmq://localhost/FilaPessoa");
            //    var endPoint = await _bus.GetSendEndpoint(uri);

            //    await endPoint.Send(new PessoaModelMessage
            //    {
            //        Id = result.Id,
            //        ListaErros = result.ListaErros,
            //        Nome = result.Nome,
            //        CPF = result.CPF,
            //        DataNascimento = result.DataNascimento,
            //        ListaTelefone = result.ListaTelefone,
            //        Ativo = result.Ativo

            //    });
            //}
        }

        public async Task<PessoaModel> AlterarPessoa(PessoaAlteracaoModel pessoa)
        {
            var _pessoa = _mapper.Map<Pessoa>(pessoa);
            var telefones = await _queryPessoa.ObterTelefonesDaPessoa(pessoa.Id);
            foreach (var fone in telefones)
            {
                _pessoa.AdicionarTelefoneNaLista(_mapper.Map<Telefone>(fone));
            }
            var command = new AlterarPessoaCommand(_pessoa.Id, _pessoa.Nome, _pessoa.CPF, _pessoa.DataNascimento, _pessoa.Ativo, _pessoa.ListaTelefones);
            await _mediatrHandler.EnviarCommand(command);

            return await TranformaEmPessoaModel(command, _pessoa);
        }


        public async Task<PessoaModel?> ExcluirPessoa(Guid id)
        {
            var command = new ExcluirPessoaCommand(id);
            await _mediatrHandler.EnviarCommand(command);
            var _pessoa = new PessoaModel();
            _pessoa.Id = id;
            _pessoa.ListaErros = ObterMensagensDeErro();
            return _pessoa;
        }

        public async Task<TelefoneModel> AdicionarTelefone(TelefoneModel fone, Guid idpessoa)
        {
            var command = new AdicionarTelefoneCommand(fone.Id, idpessoa, fone.Tipo, fone.Numero);
            await _mediatrHandler.EnviarCommand(command);
            return new TelefoneModel
            {
                Id = command.Id,
                PessoaId = command.PessoaId,
                Tipo = command.Tipo,
                Numero = command.Numero,
                ListaErros = ObterMensagensDeErro()
            };
        }

        public async Task<TelefoneModel> AlterarTelefone(TelefoneAlteracaoModel fone)
        {
            var command = new AlterarTelefoneCommand(fone.Id, new Guid(), fone.Tipo, fone.Numero);
            await _mediatrHandler.EnviarCommand(command);
            return new TelefoneModel
            {
                Id = command.Id,
                PessoaId = command.PessoaId,
                Tipo = command.Tipo,
                Numero = command.Numero,
                ListaErros = ObterMensagensDeErro()
            };
        }
        public async Task<TelefoneModel> ExcluirTelefone(Guid idtelefone)
        {
            var command = new ExcluirTelefoneCommand(idtelefone);
            await _mediatrHandler.EnviarCommand(command);
            return new TelefoneModel
            {
                Id = command.Id,
                ListaErros = ObterMensagensDeErro()
            };
        }

        public async Task<PessoaModel> Ativar(Guid id)
        {
            var command = new AtivarPessoaCommand(id);
            await _mediatrHandler.EnviarCommand(command);
            return await TranformaEmPessoaModel(command);
        }

        public async Task<PessoaModel> Inativar(Guid id)
        {
            var command = new DesativarPessoaCommand(id);
            await _mediatrHandler.EnviarCommand(command);
            return await TranformaEmPessoaModel(command);
        }

        private Task<PessoaModel> TranformaEmPessoaModel(AdicionarPessoaCommand command, Pessoa _pessoa)
        {
            var _retorno = new PessoaModel
            {
                Id = command.IdPessoa,
                Nome = command.Nome,
                CPF = command.CPF,
                DataNascimento = command.DataNascimento,
                Ativo = command.Ativo,
                ListaErros = ObterMensagensDeErro()
            };
            var listatelefones = _mapper.Map<IEnumerable<TelefoneModel>>(_pessoa.ListaTelefones);
            foreach (var item in listatelefones)
            {
                _retorno.ListaTelefone.Add(item);
            }
            return Task.Run(() => _retorno);
        }

        private Task<PessoaModel> TranformaEmPessoaModel(AlterarPessoaCommand command, Pessoa _pessoa)
        {
            var _retorno = new PessoaModel
            {
                Id = command.IdPessoa,
                Nome = command.Nome,
                CPF = command.CPF,
                DataNascimento = command.DataNascimento,
                Ativo = command.Ativo,
                ListaErros = ObterMensagensDeErro()
            };
            var listatelefones = _mapper.Map<IEnumerable<TelefoneModel>>(_pessoa.ListaTelefones);
            foreach (var item in listatelefones)
            {
                _retorno.ListaTelefone.Add(item);
            }
            return Task.Run(() => _retorno);
        }

        private async Task<PessoaModel> TranformaEmPessoaModel(AtivarPessoaCommand command)
        {
            var pessoa = await ObterPorId(command.Id);
            var _retorno = new PessoaModel
            {
                Id = command.Id,
                Nome = pessoa != null ? pessoa.Nome : null,
                CPF = pessoa != null ? pessoa.CPF : null,
                DataNascimento = pessoa != null ? pessoa.DataNascimento : DateTime.Today,
                Ativo = pessoa != null ? pessoa.Ativo : false,
                ListaErros = ObterMensagensDeErro()
            };
            if (pessoa != null)
            {
                var listatelefones = _mapper.Map<IEnumerable<TelefoneModel>>(pessoa.Telefones);
                foreach (var item in listatelefones)
                {
                    _retorno.ListaTelefone.Add(item);
                }
            }
            return _retorno;
        }

        private async Task<PessoaModel> TranformaEmPessoaModel(DesativarPessoaCommand command)
        {
            var pessoa = await ObterPorId(command.Id);

            var _retorno = new PessoaModel
            {
                Id = command.Id,
                Nome = pessoa != null ? pessoa.Nome : null,
                CPF = pessoa != null ? pessoa.CPF : null,
                DataNascimento = pessoa != null ? pessoa.DataNascimento : DateTime.Today,
                Ativo = pessoa != null ? pessoa.Ativo : false,
                ListaErros = ObterMensagensDeErro()
            };
            if (pessoa != null)
            {
                var listatelefones = _mapper.Map<IEnumerable<TelefoneModel>>(pessoa.Telefones);
                foreach (var item in listatelefones)
                {
                    _retorno.ListaTelefone.Add(item);
                }
            }
            return _retorno;
        }

    }
}
