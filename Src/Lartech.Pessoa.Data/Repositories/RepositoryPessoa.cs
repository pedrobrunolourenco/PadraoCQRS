using Dapper;
using Lartech.Domain.DTOS;
using Lartech.Domain.Entidades;
using Lartech.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Lartech.Data.Repositories
{
    public class RepositoryPessoa : Repository<Pessoa>, IRepositoryPessoa
    {

        private string queryViewModelBase { get; set; }

        public RepositoryPessoa(DataContext context) : base(context)
        {
            queryViewModelBase = @" SELECT DISTINCT p.Id, 
                                   p.Nome,
                                   p.CPF,
                                   p.DataNascimento,
                                   p.Ativo,
                                   t.Numero,
                                   t.Tipo
                                   FROM Pessoas p WITH(NOLOCK) 
							       LEFT JOIN Telefones t  WITH(NOLOCK) ON (p.Id = t.PessoaId)
                                 ";
        }

        public async Task<PessoaViewModel?> ObterPorId(Guid id)
        {
            StringBuilder query = new StringBuilder();
            query.Append(queryViewModelBase);
            query.Append(@" WHERE p.ID = @ID ORDER BY p.Nome");
            var retorno =  _context.Database.GetDbConnection().QueryAsync<PessoaDTO>(query.ToString(), new { ID = id }).Result.ToList();
            return await Task.Run(() => TransformarDTO(retorno).FirstOrDefault());
        }

        public async Task<IEnumerable<PessoaViewModel>> ObterTodos()
        {
            StringBuilder query = new StringBuilder();
            query.Append(queryViewModelBase);
            query.Append(@" ORDER BY p.Nome");
            var retorno = _context.Database.GetDbConnection().QueryAsync<PessoaDTO>(query.ToString()).Result.ToList();
            return await Task.Run(() => TransformarDTO(retorno));
        }

        public async Task<IEnumerable<PessoaViewModel>> ObterAtivos()
        {
            StringBuilder query = new StringBuilder();
            query.Append(queryViewModelBase);
            query.Append(@"  WHERE p.Ativo = 1 ORDER BY p.Nome");
            var retorno = _context.Database.GetDbConnection().QueryAsync<PessoaDTO>(query.ToString()).Result.ToList();
            return await Task.Run(() => TransformarDTO(retorno));
        }

        public async Task<IEnumerable<PessoaViewModel>> ObterInativos()
        {
            StringBuilder query = new StringBuilder();
            query.Append(queryViewModelBase);
            query.Append(@"  WHERE p.Ativo = 0 ORDER BY p.Nome");
            var retorno = _context.Database.GetDbConnection().QueryAsync<PessoaDTO>(query.ToString()).Result.ToList();
            return await Task.Run(() => TransformarDTO(retorno));
        }


        public async Task<PessoaViewModel?> ObterPorCpf(string cpf)
        {
            StringBuilder query = new StringBuilder();
            query.Append(queryViewModelBase);
            query.Append(@" WHERE p.CPF = @CPF ORDER BY p.Nome");
            var retorno = _context.Database.GetDbConnection().QueryAsync<PessoaDTO>(query.ToString(), new { CPF = cpf }).Result.ToList();
            return await Task.Run(() => TransformarDTO(retorno).FirstOrDefault());
        }

        public async Task<PessoaViewModel?> ObterPorCpf(string cpf, Guid id)
        {
            StringBuilder query = new StringBuilder();
            query.Append(queryViewModelBase);
            query.Append(@" WHERE p.CPF = @CPF ORDER BY p.Nome");
            var retorno = _context.Database.GetDbConnection().QueryAsync<PessoaDTO>(query.ToString(), new { CPF = cpf }).Result.ToList();
            return await Task.Run(() => TransformarDTO(retorno).FirstOrDefault());
        }
        public async Task<IEnumerable<PessoaViewModel>> ObterPorParteDoNome(string nome)
        {
            StringBuilder query = new StringBuilder();
            query.Append(queryViewModelBase);
            query.Append(@" WHERE p.Nome LIKE @NOME ORDER BY p.Nome");
            var retorno = _context.Database.GetDbConnection().QueryAsync<PessoaDTO>(query.ToString(), new { NOME = "%" + nome + "%" }).Result.ToList();
            return await Task.Run(() => TransformarDTO(retorno));
        }


        public async Task<Pessoa> Ativar(Pessoa pessoa)
        {
            pessoa.Ativar();
            await Atualizar(pessoa);
            await Salvar();
            return pessoa;
        }

        public async Task<Pessoa> Inativar(Pessoa pessoa)
        {
            pessoa.Inativar();
            await Atualizar(pessoa);
            await Salvar();
            return pessoa;
        }

        private List<PessoaViewModel> TransformarDTO(List<PessoaDTO> dto)
        {
            var retorno = new List<PessoaViewModel>();
            foreach (var dtoItem in dto) 
            {
                var pessoa = retorno.Where(x => x.Id == dtoItem.Id).FirstOrDefault();
                if (pessoa == null)
                {
                    var item = new PessoaViewModel();
                    item.Id = dtoItem.Id;
                    item.Nome = dtoItem.Nome;
                    item.CPF = dtoItem.CPF;
                    item.DataNascimento = dtoItem.DataNascimento;
                    item.Ativo = dtoItem.Ativo;
                    item.Telefones.Add(new TelefoneDTO
                    {
                        Tipo = dtoItem.Tipo,
                        Numero = dtoItem.Numero
                    });
                    retorno.Add(item);
                }
                else
                {
                    pessoa.Telefones.Add(new TelefoneDTO
                    {
                        Tipo = dtoItem.Tipo,
                        Numero = dtoItem.Numero
                    });
                }
            }
            return retorno;
        }
    }
}
