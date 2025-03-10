
using Entities.Models;

namespace Repositories.Contracts
{
    // Book nesnesi için IRepositoryBase de ki Crud işlemleri devralınmış oldu
    public interface IBookRepository : IRepositoryBase<Book>
    {
            IQueryable<Book> GetAllBooks(bool trackChanges);
            Book GetOneBookById(int id , bool trackChanges);
            void CreateOneBook(Book book);

            void DeleteOneBook(Book book);

            void UpdateOneBook(Book book);
    }
}