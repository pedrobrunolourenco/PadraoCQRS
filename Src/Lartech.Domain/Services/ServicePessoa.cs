using Lartech.Domain.DTOS;
using Lartech.Domain.Entidades;
using Lartech.Domain.Interfaces.Repository;
using Lartech.Domain.Interfaces.Service;

namespace Lartech.Domain.Services
{
    public class ServicePessoa : IServicePessoa
    {

        private readonly IRepositoryPessoa _repositoryPessoa;
        private readonly IRepositoryTelefone _repositoryTelefone;

        public ServicePessoa(IRepositoryPessoa repositoryPessoa,
                             IRepositoryTelefone repositoryTelefone)
        {
            _repositoryPessoa = repositoryPessoa;
            _repositoryTelefone = repositoryTelefone;
        }


        public async Task<Pessoa> IncluirPessoa(Pessoa pessoa)
        {
            if (!pessoa.Validar()) return pessoa;
            if(NaoAdicionouTodosOsTelefones(pessoa)) return pessoa;
            var errosDominio = await ValidarRegrasDeDominio(pessoa);
            if (errosDominio.ListaErros.Any()) return pessoa;
            pessoa.Ativar();
            await _repositoryPessoa.Adicionar(pessoa);
            await _repositoryPessoa.Salvar();
            return pessoa;
        }

        public async Task<Pessoa> AlterarPessoa(Pessoa pessoa, Guid id)
        {
            var _pessoa = await _repositoryPessoa.BuscarId(id);
            if (_pessoa == null) return pessoa;

            _pessoa.AtriuirNome(pessoa.Nome);
            _pessoa.AtriuirCPF(pessoa.CPF);
            _pessoa.AtriuirDataNascimento(pessoa.DataNascimento);

            if (!_pessoa.Validar()) return _pessoa;
            var errosDominio = await ValidarRegrasDeDominio(_pessoa);
            if (errosDominio.ListaErros.Any()) return _pessoa;
            await _repositoryPessoa.Atualizar(_pessoa);
            await _repositoryPessoa.Salvar();
            return pessoa;
        }


        public async Task<Pessoa> AtivarPessoa(Guid id)
        {
            var pessoa = await _repositoryPessoa.BuscarId(id);
            if (pessoa == null)
            {
                pessoa = new Pessoa();
                pessoa.ListaErros.Add("Pessoa não localizada.");
                return pessoa;
            }
            pessoa.Ativar();
            await _repositoryPessoa.Atualizar(pessoa);
            await _repositoryPessoa.Salvar();
            return pessoa;
        }

        public async Task<Pessoa> InativarPessoa(Guid id)
        {
            var pessoa = await _repositoryPessoa.BuscarId(id);
            if (pessoa == null)
            {
                pessoa = new Pessoa();
                pessoa.ListaErros.Add("Pessoa não localizada.");
                return pessoa;
            }
            pessoa.Inativar();
            await _repositoryPessoa.Atualizar(pessoa);
            await _repositoryPessoa.Salvar();
            return pessoa;
        }


        public async Task<Pessoa> ExcluirPessoa(Guid id)
        {
            var pessoa = await _repositoryPessoa.BuscarId(id);
            if (pessoa == null)
            {
                pessoa = new Pessoa();
                pessoa.ListaErros.Add("Pessoa não localizada.");
                return pessoa;
            }
            await _repositoryPessoa.Remover(pessoa);
            await _repositoryPessoa.Salvar();
            return pessoa;
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
            return telefones.Where(x => x.PessoaId == idpessoa);
        }

        public async Task<Telefone> AdicionarTelefone(Telefone fone, Guid idpessoa)
        {
            fone.AtribuirIdPessoa(idpessoa);
            if (!fone.Validar()) return fone;
            if (await VerificarSeTelefoneJaExiste(fone)) 
            { 
                fone.ListaErros.Add($"O telefone {fone.Numero} já existe para esta pessoa." );
                return fone;
            }
            await _repositoryTelefone.Adicionar(fone);
            await _repositoryPessoa.Salvar();
            return fone;
        }

        public async Task<Telefone> AlterarTelefone(Telefone fone)
        {
            var telefone = await _repositoryTelefone.BuscarId(fone.Id);
            if (telefone == null) return fone;
            telefone.AtribuirTipo(fone.Tipo);
            telefone.AtribuirNumero(fone.Numero);
            if (!telefone.Validar()) return telefone;
            await _repositoryTelefone.Atualizar(telefone);
            await _repositoryTelefone.Salvar();
            return fone;
        }

        public async Task<Telefone> ExcluirTelefone(Guid idtelefone)
        {
            var fone = await _repositoryTelefone.BuscarId(idtelefone);
            if (fone == null)
            {
                fone = new Telefone();
                fone.ListaErros.Add($"Telefone não localizado");
                return fone;
            }
            await _repositoryTelefone.Remover(fone);
            await _repositoryTelefone.Salvar();
            return fone;
        }

        private async Task<bool> VerificarSeTelefoneJaExiste(Telefone telefone)
        {
            var telefones = await _repositoryTelefone.Listar();
            return telefones.Where(t => t.Numero == telefone.Numero && t.PessoaId == telefone.PessoaId).Any();
        }

        private async Task<bool> VerificarSeCPFJaExiste(Pessoa pessoa)
        {
            var result = await _repositoryPessoa.Listar();
            var retorno = result.Where(p => p.CPF == pessoa.CPF && p.Id != pessoa.Id).Any();
            return retorno;
        }

        private bool NaoAdicionouTodosOsTelefones(Pessoa pessoa)
        {
            foreach (var telefone in pessoa.ListaTelefones)
            {
                if (telefone.Validar())
                {
                    _repositoryTelefone.Adicionar(telefone);
                }
                else
                {
                    foreach (var erroTelefone in telefone.ListaErros)
                    {
                        pessoa.ListaErros.Add(erroTelefone);
                    }
                }
            }
            return pessoa.ListaErros.Any();
        }

        private async Task<Pessoa> ValidarRegrasDeDominio(Pessoa pessoa)
        {
            if (await VerificarSeCPFJaExiste(pessoa)) pessoa.ListaErros.Add($"O CPF {pessoa.CPF} já existe para outra pessoa.");
            return pessoa;
        }

    }
}
