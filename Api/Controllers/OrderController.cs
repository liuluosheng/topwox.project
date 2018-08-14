using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Core.IServices;
using Data.Entitys;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
namespace Ew.Api.Controllers
{
    [Description("订单信息")]
    public class OrdersController : BaseController<User>
    {
        public OrdersController(IBaseService<User> userService) : base(userService)
        {
        }
        [EnableQuery]
        public IActionResult Page()
        {
            return Ok();
        }
    }
}
