using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/identity")]
    public class IdentityController : ControllerBase
    {
        private readonly Mediator _mediator;

        [HttpGet("get-something")]
        [Authorize]
        public async Task<ActionResult> Get()
        {
            return StatusCode(401);
        }
    }
}