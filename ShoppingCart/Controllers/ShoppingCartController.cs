﻿using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ShoppingCart.Data.Model;
using ShoppingCart.Filters;
using ShoppingCart.Maps;
using ShoppingCart.Model;
using ShoppingCart.Queries;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExceptionFilter]
    [AllowAnonymous] // Security and authorization features are not implemented for this demo project.
    public class ShoppingCartController : ControllerBase
    {
        private readonly IProductsQueryProcessor _queryProcessor;
        private readonly IAutoMapper _autoMapper;

        public ShoppingCartController(IProductsQueryProcessor queryProcessor, IAutoMapper autoMapper)
        {
            _queryProcessor = queryProcessor;
            _autoMapper = autoMapper;
        }

        [HttpGet]
        [ApiExceptionFilter]
        public ActionResult<IEnumerable<ProductModel>> Get()
        {
            IQueryable<Product> products = _queryProcessor.GetAll();

            IQueryable<ProductModel> productModels = _autoMapper.Map<Product, ProductModel>(products);

            return productModels.ToList();
        }

        [HttpGet("{id}")]
        [ApiExceptionFilter]
        public ActionResult<ProductModel> Get(int id)
        {
            Product product = _queryProcessor.Get(id);

            return _autoMapper.Map<ProductModel>(product);
        }

        [HttpPost]
        [ValidateModel]
        [ApiExceptionFilter]
        public async void Post([FromBody] CreateProductModel requestModel)
        {
            await _queryProcessor.Create(requestModel);
        }

        [HttpPut("{id}")]
        [ValidateModel]
        [ApiExceptionFilter]
        public async void Put(int id, [FromBody] UpdateProductModel requestModel)
        {
            await _queryProcessor.Update(id, requestModel);
        }

        [HttpDelete("{id}")]
        [ApiExceptionFilter]
        public async void Delete(int id)
        {
            await _queryProcessor.Delete(id);
        }
    }
}
