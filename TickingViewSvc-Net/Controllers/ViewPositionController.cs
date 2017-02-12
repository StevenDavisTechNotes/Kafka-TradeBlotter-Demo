using System.Collections.Generic;
using System.Threading;
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

        public ViewPositionController(IViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }
        // GET: api/ViewPosition
        [HttpGet]
        public IEnumerable<Exposure> GetAll()
        {
            return _viewEngine.GetPositionView();
        }
    }
}
