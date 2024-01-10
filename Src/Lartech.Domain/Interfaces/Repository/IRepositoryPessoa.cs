using Lartech.Domain.DTOS;
using Lartech.Domain.Entidades;

namespace Lartech.Domain.Interfaces.Repository
{
    public interface IRepositoryPessoa : IRepository<Pessoa>
    {
        Task<PessoaViewModel?> ObterPorId(Guid id);
        Task<IEnumerable<PessoaViewModel>> ObterTodos();
        Task<IEnumerable<PessoaViewModel>> ObterPorParteDoNome(string nome);
        Task<IEnumerable<PessoaViewModel>> ObterAtivos();
        Task<IEnumerable<PessoaViewModel>> ObterInativos();

        Task<PessoaViewModel?> ObterPorCpf(string cpf);
        Task<PessoaViewModel?> ObterPorCpf(string cpf, Guid id);
        Task<Pessoa> Inativar(Pessoa pessoa);
        Task<Pessoa> Ativar(Pessoa pessoa);

    }
}
