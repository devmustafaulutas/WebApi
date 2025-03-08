
using Repositories.Contracts;

namespace Repositories.Efcore
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;

        private readonly Lazy<IBookRepository> _bookRepository;
        
        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
            _bookRepository = new Lazy<IBookRepository>(() => new BookRepository(_context));
            //Lazy loading işlemi nesne ancak kullanıldığında newlenicek
        }

        // Bir class içinde başka bir class normal şartlarda newlenmez 
        public IBookRepository Book => _bookRepository.Value;

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}