using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ShoppingCart.Data.Model;
using ShoppingCart.Filters;
using ShoppingCart.Model;
using ShoppingCart.Queries;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IProductsQueryProcessor _queryProcessor;

        public ShoppingCartController(IProductsQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            return _queryProcessor.Get().ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            return _queryProcessor.Get(id);
        }

        [HttpPost]
        [ValidateModel]
        public async void Post([FromBody] CreateProductModel requestModel)
        {
            await _queryProcessor.Create(requestModel);
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async void Put(int id, [FromBody] UpdateProductModel requestModel)
        {
            await _queryProcessor.Update(id, requestModel);
        }

        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            await _queryProcessor.Delete(id);
        }
    }
}
