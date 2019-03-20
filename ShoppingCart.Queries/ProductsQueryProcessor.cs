using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Expenses.Api.Common.Exceptions;
using Expenses.Security;

using ShoppingCart.Model;
using ShoppingCart.Data.Access.DAL;
using ShoppingCart.Data.Model;

namespace ShoppingCart.Queries
{
    public class ProductsQueryProcessor : IProductsQueryProcessor
    {
        private readonly IUnitOfWork _uow;
        private readonly ISecurityContext _securityContext;

        public ProductsQueryProcessor(IUnitOfWork uow, ISecurityContext securityContext)
        {
            _uow = uow;
            _securityContext = securityContext;
        }

        public IQueryable<Product> Get()
        {
            var query = GetQuery();
            return query;
        }
        
        private IQueryable<Product> GetQuery()
        {
            var q = _uow.Query<Product>()
                .Where(x => !x.IsDeleted);

            //if (!_securityContext.IsAdministrator)
            //{
            //    var userId = _securityContext.User.Id;
            //    q = q.Where(x => x.UserId == userId);
            //}

            return q;
        }

        public Product Get(int id)
        {
            var user = GetQuery().FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new NotFoundException("Expense is not found");
            }

            return user;
        }

        public async Task<Product> Create(CreateProductModel model)
        {
            var item = new Product
            {
                //UserId = _securityContext.User.Id,
                //Amount = model.Amount,
                //Comment = model.Comment,
                //Date = model.Date,
                Description = model.Description,
            };

            _uow.Add(item);
            await _uow.CommitAsync();

            return item;
        }

        public async Task<Product> Update(int id, UpdateProductModel model)
        {
            var expense = GetQuery().FirstOrDefault(x => x.Id == id);

            if (expense == null)
            {
                throw new NotFoundException("Expense is not found");
            }

            //expense.Amount = model.Amount;
            //expense.Comment = model.Comment;
            //expense.Description = model.Description;
            //expense.Date = model.Date;

            await _uow.CommitAsync();
            return expense;
        }

        public async Task Delete(int id)
        {
            var user = GetQuery().FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                throw new NotFoundException("Expense is not found");
            }

            if (user.IsDeleted) return;

            user.IsDeleted = true;
            await _uow.CommitAsync();
        }
    }
}
