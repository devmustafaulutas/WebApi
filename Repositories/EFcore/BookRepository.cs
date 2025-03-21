using Entities.Exceptions;
using Entities.Models;
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

        public IQueryable<Book> GetAllBooks(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(b => b.Id);
            
        public Book GetOneBookById(int id, bool trackChanges)
        {
            var book = FindByCondition(b => b.Id == id, trackChanges).FirstOrDefault();
            Console.WriteLine($"{book}");
            if (book is null)
            {
                Console.WriteLine($"❌ Book with ID {id} not found.");
                throw new BookNotFoundException(id); 
            }

            // Log atalım
            Console.WriteLine($"✅ Kitap Bulundu: {book.Title} - {book.Price}");

            return book;
        }

    }
}