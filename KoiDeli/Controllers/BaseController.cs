using Microsoft.AspNetCore.Mvc;

namespace KoiDeli.Controllers
{
    //cac controller khac sẽ thừa kế basecontroller
    [Route("/api/v1/[controller]/")]
    [ApiController]
    public class BaseController: ControllerBase
    {
    }
}
