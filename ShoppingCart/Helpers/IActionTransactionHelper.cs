using Microsoft.AspNetCore.Mvc.Filters;

namespace ShoppingCart.Helpers
{
    public interface IActionTransactionHelper
    {
        void BeginTransaction();
        void EndTransaction(ActionExecutedContext filterContext);
        void CloseSession();
    }
}