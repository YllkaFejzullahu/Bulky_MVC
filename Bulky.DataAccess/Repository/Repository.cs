using System.Linq.Expressions;
using BulkyWeb.Data;
using Bulky.DataAccess.Repository.IRepository;

namespace Bulky.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _db;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<T> GetAll() => _db.Set<T>().ToList();

        public T Get(Expression<Func<T, bool>> filter) => _db.Set<T>().FirstOrDefault(filter)!;

        public void Add(T entity) => _db.Set<T>().Add(entity);

        public void Remove(T entity) => _db.Set<T>().Remove(entity);

        public void Update(T entity) => _db.Set<T>().Update(entity);
        public void Save() => _db.SaveChanges();
    }
}
