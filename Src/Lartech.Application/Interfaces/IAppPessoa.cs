using Lartech.Application.Models;
using Lartech.Domain.CQRS.Commands;
using Lartech.Domain.DTOS;

namespace Lartech.Application.Interfaces
{
    public interface IAppPessoa
    {
        Task<IEnumerable<PessoaViewModel>> ObterTodos();
        Task<PessoaViewModel?> ObterPorId(Guid id);
        Task<PessoaViewModel?> ObterPorCpf(string cpf);
        Task<IEnumerable<PessoaViewModel>> ObterPorParteDoNome(string nome);

        Task<IEnumerable<PessoaViewModel>> ObterAtivos();
        Task<IEnumerable<PessoaViewModel>> ObterInativos();

        Task<PessoaModel> IncluirPessoa(PessoaModel pessoa);
        Task<PessoaModel> AlterarPessoa(PessoaAlteracaoModel pessoa);
        Task<PessoaModel?> ExcluirPessoa(Guid id);

        Task<TelefoneModel> AdicionarTelefone(TelefoneModel fone, Guid idpessoa);
        Task<TelefoneModel> AlterarTelefone(TelefoneAlteracaoModel fone);
        Task<TelefoneModel> ExcluirTelefone(Guid idtelefone);

        Task<PessoaModel> Ativar(Guid id);
        Task<PessoaModel> Inativar(Guid id);

    }
}
