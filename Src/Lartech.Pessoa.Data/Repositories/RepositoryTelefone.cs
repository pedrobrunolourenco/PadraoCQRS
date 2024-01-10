using Lartech.Domain.Entidades;
using Lartech.Domain.Interfaces.Repository;

namespace Lartech.Data.Repositories
{
    public class RepositoryTelefone :  Repository<Telefone>, IRepositoryTelefone
    {
        public RepositoryTelefone(DataContext context) : base(context)
        {

        }
    }
}
