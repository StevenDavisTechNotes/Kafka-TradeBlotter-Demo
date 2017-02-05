using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using TickingViewSvc_Net.Models;
using TickingViewSvc_Net.Services;

namespace TickingViewSvc_Net.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ViewPositionController : ApiController
    {
        private readonly IViewEngine _viewEngine;
        public ViewPositionController()
        {
            this._viewEngine = new ViewEngine(); // viewEngine;
        }
        // GET: api/ViewPosition
        [HttpGet]
        public IEnumerable<Exposure> GetAll()
        {
            return _viewEngine.GetPositionView();
        }
    }
}
