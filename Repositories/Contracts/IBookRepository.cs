
using Entities.Models;

namespace Repositories.Contracts
{
    // Book nesnesi için IRepositoryBase de ki Crud işlemleri devralınmış oldu
    public interface IBookRepository : IRepositoryBase<Book>
    {
        
    }
}