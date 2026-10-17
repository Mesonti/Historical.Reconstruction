using Historical.Reconstruction.WebAPIs.Services;
using Microsoft.AspNetCore.Mvc;

namespace Historical.Reconstruction.WebAPIs.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class FindController : ControllerBase
    {
        private readonly IFindService _findService;
        public FindController(IFindService findService)
        {
            _findService = findService;
        }
    }
}
