using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationToken.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentInfoController : ControllerBase
    {

        [HttpGet("payments")]
        [Authorize]
        public IEnumerable<string> Payments()
        {
            //Authorize tag worked. User is a valid user and has a valid token. Return some fake data.
            return new List<string> { "Payment1", "Payment2", "Payment3" };
        }
    }
}
