using Entities.DataTransferObjects;
using Entities.Models;
using Repositories;
namespace Services.Contracts
{
    public interface IBookService
    {
        IEnumerable<BookDto> GetAllBooks(bool trackChanges);
        Book GetOneBookById(int id , bool trackChanges);

        Book CreateOneBook(Book book);

        // void UpdateOneBook(int id , Book book , bool trackChanges);
        void UpdateOneBook(int id , BookDtoForUpdate bookDto , bool trackChanges);

        void DeleteOneBook(int id , bool trackChanges);
    }
}