using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions; // Bunu eklemeyi unutma!
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    // T = Type (Book, Category, Product gibi)
    public interface IRepositoryBase<T>
    {
        // CRUD TANIMLARI (R)
        IQueryable<T> FindAll(bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);

        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
