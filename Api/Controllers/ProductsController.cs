using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using X.Core.IServices;
using X.Data.Entitys;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.OData.Routing;

namespace Ew.Api.Controllers
{
  
    public class ProductsController : BaseController<Product>
    {
        public ProductsController(IBaseService<Product> productsService) : base(productsService)
        {
        }
        [EnableQuery]
        [HttpGet]
        public IActionResult Page()
        {
            return Ok("page");
        }
    }
}
