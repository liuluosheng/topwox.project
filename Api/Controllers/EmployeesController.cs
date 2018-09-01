using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using X.Core.IServices;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using X.Data.Dto;
using User = X.Data.Entitys.User;
using X.Data.Entitys;

namespace Ew.Api.Controllers
{
    public class EmployeesController : BaseController<Employees>
    {
        public EmployeesController(IBaseService<Employees> emplyessService) : base(emplyessService)
        {
        }
       
    }
}
