using Lartech.Domain.Entidades;

namespace Lartech.Domain.Interfaces.Repository
{
    public interface IRepository<TEntidade> : IDisposable where TEntidade : Entity
    {
        Task<IEnumerable<TEntidade>> Listar();
        Task<TEntidade> BuscarId(Guid id);
        Task Adicionar(TEntidade obj);
        Task Atualizar(TEntidade obj);
        Task Remover(TEntidade obj);
        Task Salvar();
    }
}
