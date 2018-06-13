using SwaggerExample.FalconRequests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SwaggerExample.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    public class AuthorizeController : Controller
    {
        readonly IFalconPort _falconPort;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="falconPort"></param>
        public AuthorizeController(IFalconPort falconPort)
        {
            _falconPort = falconPort;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [Consumes("application/x-www-form-urlencoded")]
        [Produces("application/json")]
        public async Task<IActionResult> Login([FromQuery] string UserId, [FromQuery] string Password)
        {
            try
            {
                var result = await _falconPort.SubmitCosecRequest(new { UserId, password = Password, Operation = "Login", TargetURL = "", ClientTime = DateTime.Now.ToString("o") });
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (HttpStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Processing error [{ ex }] received for { this.GetMethodName() }.  Verify input request data.");
            }
        }
    }
}
