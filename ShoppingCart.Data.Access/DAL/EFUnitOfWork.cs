using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace ShoppingCart.Data.Access.DAL
{
    public class EFUnitOfWork : IUnitOfWork
    {
        public EFUnitOfWork(ShoppingCartDbContext context)
        {
            Context = context;
        }

        public ShoppingCartDbContext Context { get; private set; }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Snapshot)
        {
            return new DbTransaction(Context.Database.BeginTransaction(isolationLevel));
        }

        public void Add<T>(T obj)
            where T : class
        {
            DbSet<T> set = Context.Set<T>();
            set.Add(obj);
        }

        public void Update<T>(T obj)
            where T : class
        {
            DbSet<T> set = Context.Set<T>();
            set.Attach(obj);
            Context.Entry(obj).State = EntityState.Modified;
        }

        void IUnitOfWork.Remove<T>(T obj)
        {
            DbSet<T> set = Context.Set<T>();
            set.Remove(obj);
        }

        public IQueryable<T> Query<T>()
            where T : class
        {
            return Context.Set<T>();
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await Context.SaveChangesAsync();
        }

        public void Attach<T>(T newUser) where T : class
        {
            DbSet<T> set = Context.Set<T>();
            set.Attach(newUser);
        }

        public void Dispose()
        {
            Context = null;
        }
    }
}