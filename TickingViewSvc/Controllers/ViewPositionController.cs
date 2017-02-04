using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TickingViewSvc.Models;
using TickingViewSvc.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TickingViewSvc.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    public class ViewPositionController : Controller
    {
        private IViewEngine viewEngine;
        public ViewPositionController(IViewEngine viewEngine)
        {
            this.viewEngine = viewEngine;
        }
        // GET: api/ViewPosition
        [HttpGet]
        public IEnumerable<Exposure> GetAll()
        {
            return viewEngine.GetPositionView();
        }
    }
}
