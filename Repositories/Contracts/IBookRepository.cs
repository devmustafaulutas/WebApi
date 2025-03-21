
using Entities.Models;

namespace Repositories.Contracts
{
    // Book nesnesi için IRepositoryBase de ki Crud işlemleri devralınmış oldu
    public interface IBookRepository : IRepositoryBase<Book>
    {
            Task<IEnumerable<Book>>  GetAllBooksAsync(bool trackChanges);
            Task<Book> GetOneBookByIdAsync(int id , bool trackChanges);
            void CreateOneBook(Book book);

            void DeleteOneBook(Book book);

            void UpdateOneBook(Book book);
    }
}