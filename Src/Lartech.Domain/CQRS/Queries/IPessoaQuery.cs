using Lartech.Domain.DTOS;
using Lartech.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lartech.Domain.CQRS.Queries
{
    public interface IPessoaQuery
    {
        Task<IEnumerable<PessoaViewModel>> ObterTodos();
        Task<PessoaViewModel?> ObterPorId(Guid id);
        Task<PessoaViewModel?> ObterPorCpf(string cpf);
        Task<IEnumerable<PessoaViewModel>> ObterPorParteDoNome(string nome);
        Task<IEnumerable<PessoaViewModel>> ObterAtivos();
        Task<IEnumerable<PessoaViewModel>> ObterInativos();
        Task<IEnumerable<Telefone>> ObterTelefonesDaPessoa(Guid idpessoa);


    }
}
