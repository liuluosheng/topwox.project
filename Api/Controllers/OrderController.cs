﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using X.Core.IServices;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using X.Data.Dto;
using User = X.Data.Entitys.User;

namespace Ew.Api.Controllers
{
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
