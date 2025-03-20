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
            var book = FindByCondition(b => b.Id == id, trackChanges).SingleOrDefault();

            if (book is null)
            {
                // Eğer kitap bulunamazsa, null döndürülmeden önce loglama veya hata mesajı ekleyebilirsiniz.
                Console.WriteLine($"Book with ID {id} not found.");
            }
            return book;
        }


    }
}