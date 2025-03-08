using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace Repositories.Efcore
{
    // T generic anlamına gelir bu repositorynin herhangi bir entity ile çalışabileceği anlamına gelir.
    public abstract class RepositoryBase<T> : IRepositoryBase<T>
        where T : class
    {
        // Veritabanı bağlantısı _context ile erişiyoruz
        protected readonly RepositoryContext _context;
        
        // Bu metot sınıfın bir örneği oluşturulurken RepositoryContext nesnesini alıp sınıf içerisinde ki _context alanına atar
        // Bu sayede veritabanı işlemleri yapılabilir.
        public RepositoryBase(RepositoryContext context)
        {
            _context = context;
        }

        // Create metodu 
        // Set ef core da ki dbcontext sınıfının bir metodu veritabanında ki tabloyu temsil eden bir DbSet nesnesini döndürür

        public void Create(T entity) 
        {
            _context.Set<T>().Add(entity);
        }

        // Delete metodu
        public void Delete(T entity) 
        {
            _context.Set<T>().Remove(entity);
        }

        // FindAll metodu
        // : ifadesi if kısaltması gibi düşünülebilir .
        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ?
            _context.Set<T>().AsNoTracking() :
            _context.Set<T>();


        // FindByCondition metodu
        // Expression<Func<T, bool>> expression
        // yukarı LINQ ve Query yazmak için kullanılan bir tür ifade
        // <Func<T, bool> -> bir delegat türüdür yani T türünde generic bir parametre alıp bool döndürüren bir fonksiyon
        // T türü için koşul oluşturmamızı sağlar
        // ÖRNEK :
        // Expression<Func<Book, bool>> expression = x => x.Name == "C# Programming";
        // Bu ifade, Book türündeki nesneler için Name özelliği "C# Programming" olanları filtreler.
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, 
            bool trackChanges) =>
            !trackChanges ?
            _context.Set<T>().Where(expression).AsNoTracking() :
            _context.Set<T>();
        //AsNoTracking() Ef Core da değişiklik yapılıp yapılmadığını izlemez



        // Update metodu
        public void Update(T entity) 
        {
            _context.Set<T>().Update(entity);
        }
    }
}
