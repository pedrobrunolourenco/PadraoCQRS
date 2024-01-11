using AutoMapper;
using Lartech.Application.Interfaces;
using Lartech.Application.Models;
using Lartech.Domain.Core.Comunicacao.Mediator;
using Lartech.Domain.Core.Enum;
using Lartech.Domain.Core.Messages;
using Lartech.Domain.CQRS.Commands;
using Lartech.Domain.CQRS.Queries;
using Lartech.Domain.DTOS;
using Lartech.Domain.Entidades;
using Lartech.Domain.Interfaces.Service;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace Lartech.Application.Services
{
    public class AppPessoa : IAppPessoa
    {

        private readonly IMapper _mapper;
        private readonly IPessoaQuery _queryPessoa;
        private readonly IServicePessoa _servicePessoa;
        private readonly IMediatrHandler _mediatrHandler;


        public AppPessoa(IMapper mapper,
                         IMediatrHandler mediatrHandler,
                         IPessoaQuery queryPessoa,
                         IServicePessoa servicePessoa)
        {
            _mapper = mapper;
            _mediatrHandler = mediatrHandler;
            _servicePessoa = servicePessoa;
            _queryPessoa = queryPessoa;
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
        }

        public async Task<PessoaModel> AlterarPessoa(PessoaAlteracaoModel pessoa)
        {
            var _pessoa = _mapper.Map<Pessoa>(pessoa);
            var command = new AlterarPessoaCommand(_pessoa.Id, _pessoa.Nome, _pessoa.CPF, _pessoa.DataNascimento, _pessoa.Ativo, _pessoa.ListaTelefones);
            await _mediatrHandler.EnviarCommand(command);

            var telefones = _mapper.Map<IEnumerable<TelefoneModel>>(_queryPessoa.ObterTelefonesDaPessoa(pessoa.Id));
            foreach (var fone in telefones)
            {
                _pessoa.AdicionarTelefoneNaLista(_mapper.Map<Telefone>(fone));
            }
            return await TranformaEmPessoaModel(command, _pessoa);
        }


        public async Task<PessoaModel?> ExcluirPessoa(Guid id)
        {
            var command = new ExcluirPessoaCommand(id);
            await _mediatrHandler.EnviarCommand(command);
            var _pessoa = new PessoaModel();
            _pessoa.Id = id;
            _pessoa.ListaErros = command.ListaErros;
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
                ListaErros = command.ListaErros
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
                ListaErros = command.ListaErros
            };
        }
        public async Task<TelefoneModel> ExcluirTelefone(Guid idtelefone)
        {
            return _mapper.Map<TelefoneModel>(await _servicePessoa.ExcluirTelefone(idtelefone));
        }

        public async Task<PessoaModel> Ativar(Guid id)
        {
            return _mapper.Map<PessoaModel>(await _servicePessoa.AtivarPessoa(id));
        }

        public async Task<PessoaModel> Inativar(Guid id)
        {
            return _mapper.Map<PessoaModel>(await _servicePessoa.InativarPessoa(id));
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
                ListaErros = command.ListaErros
            };
            var listatelefones = _mapper.Map<IEnumerable<TelefoneModel>>(_pessoa.ListaTelefones);
            foreach (var item in listatelefones)
            {
                _retorno.ListaTelefone.Add(item);
            }
            return Task.Run( () => _retorno );
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
                ListaErros = command.ListaErros
            };
            var listatelefones = _mapper.Map<IEnumerable<TelefoneModel>>(_pessoa.ListaTelefones);
            foreach (var item in listatelefones)
            {
                _retorno.ListaTelefone.Add(item);
            }
            return Task.Run(() => _retorno);
        }

    }
}
