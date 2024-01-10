using AutoMapper;
using Lartech.Application.Interfaces;
using Lartech.Application.Models;
using Lartech.Domain.Core.Comunicacao.Mediator;
using Lartech.Domain.Core.Messages;
using Lartech.Domain.CQRS.Commands;
using Lartech.Domain.DTOS;
using Lartech.Domain.Entidades;
using Lartech.Domain.Interfaces.Service;
using System.Reflection;

namespace Lartech.Application.Services
{
    public class AppPessoa : IAppPessoa
    {

        private readonly IMapper _mapper;
        private readonly IServicePessoa _servicePessoa;
        private readonly IMediatrHandler _mediatrHandler;


        public AppPessoa(IMapper mapper,
                         IMediatrHandler mediatrHandler,
                         IServicePessoa servicePessoa)
        {
            _mapper = mapper;
            _mediatrHandler = mediatrHandler;
            _servicePessoa = servicePessoa;
        }

        public async Task<IEnumerable<PessoaViewModel>> ObterTodos()
        {
            return await _servicePessoa.ObterTodos();
        }

        public async Task<IEnumerable<PessoaViewModel>> ObterAtivos()
        {
            return await _servicePessoa.ObterAtivos();
        }

        public async Task<IEnumerable<PessoaViewModel>> ObterInativos()
        {
            return await _servicePessoa.ObterInativos();
        }
        public async Task<PessoaViewModel?> ObterPorId(Guid id)
        {
            return await _servicePessoa.ObterPorId(id);
        }

        public async Task<PessoaViewModel?> ObterPorCpf(string cpf)
        {
            return await _servicePessoa.ObterPorCpf(cpf);
        }

        public async Task<IEnumerable<PessoaViewModel>> ObterPorParteDoNome(string nome)
        {
            return await _servicePessoa.ObterPorParteDoNome(nome);
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
            var _pessoa = _mapper.Map<PessoaModel>(_servicePessoa.AlterarPessoa(_mapper.Map<Pessoa>(pessoa), pessoa.Id));
            var telefones = _mapper.Map<IEnumerable<TelefoneModel>>(await _servicePessoa.ObterTelefonesDaPessoa(pessoa.Id));
            foreach (var tel in telefones)
            {
                _pessoa.ListaTelefone.Add(tel);
            }
            return _pessoa;
        }

        public async Task<PessoaModel?> ExcluirPessoa(Guid id)
        {
            return _mapper.Map<PessoaModel>(await _servicePessoa.ExcluirPessoa(id));
        }

        public async Task<TelefoneModel> AdicionarTelefone(TelefoneModel fone, Guid idpessoa)
        {
            return _mapper.Map<TelefoneModel>(await _servicePessoa.AdicionarTelefone(_mapper.Map<Telefone>(fone), idpessoa));
        }

        public async Task<TelefoneModel> AlterarTelefone(TelefoneAlteracaoModel fone)
        {
            return _mapper.Map<TelefoneModel>(await _servicePessoa.AlterarTelefone(_mapper.Map<Telefone>(fone)));
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


    }
}
