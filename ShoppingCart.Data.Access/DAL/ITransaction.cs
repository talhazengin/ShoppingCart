using System;

namespace ShoppingCart.Data.Access.DAL
{
    public interface ITransaction : IDisposable
    {
        void Commit();

        void Rollback();
    }
}
