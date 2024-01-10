using Lartech.Domain.DTOS;
using Lartech.Domain.Entidades;

namespace Lartech.Domain.Interfaces.Service
{
    public interface IServicePessoa
    {
        Task<IEnumerable<PessoaViewModel>> ObterTodos();
        Task<PessoaViewModel?> ObterPorId(Guid id);
        Task<PessoaViewModel?> ObterPorCpf(string cpf);
        Task<IEnumerable<PessoaViewModel>> ObterPorParteDoNome(string nome);

        Task<IEnumerable<PessoaViewModel>> ObterAtivos();
        Task<IEnumerable<PessoaViewModel>> ObterInativos();

        Task<Pessoa> IncluirPessoa(Pessoa pessoa);
        Task<Pessoa> AlterarPessoa(Pessoa pessoa, Guid id);
        Task<Pessoa> ExcluirPessoa(Guid id);

        Task<IEnumerable<Telefone>> ObterTelefonesDaPessoa(Guid idpessoa);
        Task<Telefone> AdicionarTelefone(Telefone fone, Guid idpessoa);
        Task<Telefone> AlterarTelefone(Telefone fone);
        Task<Telefone> ExcluirTelefone(Guid idtelefone);

        Task<Pessoa> AtivarPessoa(Guid id);
        Task<Pessoa> InativarPessoa(Guid id);

    }
}
