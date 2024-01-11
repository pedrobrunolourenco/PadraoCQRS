using Lartech.Domain.DTOS;
using Lartech.Domain.Entidades;
using Lartech.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lartech.Domain.CQRS.Queries
{
    public class PessoaQuery : IPessoaQuery
    {
        private readonly IRepositoryPessoa _repositoryPessoa;
        private readonly IRepositoryTelefone _repositoryTelefone;

        public PessoaQuery(IRepositoryPessoa repositoryPessoa,
                             IRepositoryTelefone repositoryTelefone)
        {
            _repositoryPessoa = repositoryPessoa;
            _repositoryTelefone = repositoryTelefone;
        }

        public async Task<PessoaViewModel?> ObterPorCpf(string cpf)
        {
            return await _repositoryPessoa.ObterPorCpf(cpf);
        }

        public async Task<PessoaViewModel?> ObterPorId(Guid id)
        {
            return await _repositoryPessoa.ObterPorId(id);
        }

        public async Task<IEnumerable<PessoaViewModel>> ObterPorParteDoNome(string nome)
        {

            return await _repositoryPessoa.ObterPorParteDoNome(nome);
        }

        public async Task<IEnumerable<PessoaViewModel>> ObterTodos()
        {
            return await _repositoryPessoa.ObterTodos();
        }

        public async Task<IEnumerable<PessoaViewModel>> ObterAtivos()
        {
            return await _repositoryPessoa.ObterAtivos();
        }

        public async Task<IEnumerable<PessoaViewModel>> ObterInativos()
        {
            return await _repositoryPessoa.ObterInativos();
        }

        public async Task<IEnumerable<Telefone>> ObterTelefonesDaPessoa(Guid idpessoa)
        {
            var telefones = await _repositoryTelefone.Listar();
            return telefones.Where(x => x.PessoaId == idpessoa).ToList();
        }


    }
}
