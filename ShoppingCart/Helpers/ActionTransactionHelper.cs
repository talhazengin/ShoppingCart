using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using ShoppingCart.Data.Access.DAL;

namespace ShoppingCart.Helpers
{
    public class ActionTransactionHelper : IActionTransactionHelper
    {
        private readonly ILogger _logger;

        private IUnitOfWork _unitOfWork;
        private ITransaction _transaction;

        public ActionTransactionHelper(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        private bool TransactionHandled { get; set; }

        private bool SessionClosed { get; set; }
        
        public void BeginTransaction()
        {
            _transaction = _unitOfWork.BeginTransaction();
        }

        public void EndTransaction(ActionExecutedContext filterContext)
        {
            if (_transaction == null)
            {
                throw new NotSupportedException();
            }

            if (filterContext.Exception == null)
            {
                _unitOfWork.Commit();
                _transaction.Commit();
            }
            else
            {
                try
                {
                    _transaction.Rollback();
                }
                catch (Exception ex)
                {
                    throw new AggregateException(filterContext.Exception, ex);
                }
            }

            TransactionHandled = true;
        }

        public void CloseSession()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            if (_unitOfWork != null)
            {
                _unitOfWork.Dispose();
                _unitOfWork = null;
            }

            SessionClosed = true;
        }
    }
}