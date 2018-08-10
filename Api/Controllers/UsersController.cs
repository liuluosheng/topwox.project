using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.IServices;
using Data.Entitys;
using Microsoft.AspNetCore.Mvc;
namespace Ew.Api.Controllers
{

    public class UsersController : BaseController<User>
    {
        public UsersController(IBaseService<User> userService) : base(userService)
        {
        }
    }
}
