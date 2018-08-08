using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entitys;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace Ew.Api.Controllers
{
    public abstract class BaseController<T> : ODataController where T : EntityBase
    {


    }
}