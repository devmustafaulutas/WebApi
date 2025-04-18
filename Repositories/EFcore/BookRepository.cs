using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.Efcore.Config;

namespace Repositories.Efcore
{

    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {

        }

        public void CreateOneBook(Book book) => Create(book);

        public void DeleteOneBook(Book book) => Delete(book);
        


        public void UpdateOneBook(Book book) => Update(book);

        public async Task<IEnumerable<Book>> GetAllBooksAsync(BookParameters bookParameters ,
            bool trackChanges) => 
            await FindAll(trackChanges)
                .OrderBy(b => b.Id)
                .Skip((bookParameters.PageNumber-1)*bookParameters.PageSize)
                .Take(bookParameters.PageSize)
                    .ToListAsync();
            
        public async Task<Book> GetOneBookByIdAsync(int id, bool trackChanges) =>
            await FindByCondition(b => b.Id.Equals(id) , trackChanges)
                .FirstOrDefaultAsync();
    }
}