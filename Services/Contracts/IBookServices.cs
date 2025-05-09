using Entities.DataTransferObjects;
using Entities.Models;
using Repositories;
using Entities.RequestFeatures;
namespace Services.Contracts
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllBooksAsync(BookParameters bookParameters , bool trackChanges);
        Task<BookDto> GetOneBookByIdAsync(int id , bool trackChanges);

        Task<BookDto> CreateOneBookAsync(BookDtoForInsertion book);

        // void UpdateOneBook(int id , Book book , bool trackChanges);
        Task UpdateOneBookAsync(int id , BookDtoForUpdate bookDto , bool trackChanges);

        Task DeleteOneBookAsync(int id , bool trackChanges);

        Task<(BookDtoForUpdate bookDtoForUpdate,Book book)> GetOneBookForPatchAsync(int id , bool trackChanges);
        Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book);
    }
}