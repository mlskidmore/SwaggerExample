using System;
using System.Threading.Tasks;
using SwaggerExample.FalconRequests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Examples;

namespace SwaggerExample.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/xml", "application/json")]
    [Consumes("application/xml", "application/json")]

    public class MobileServicesController : Controller
    {
        readonly IFalconPort _falconPort;
        const string _aspPage = "Mobile.asp";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="falconPort"></param>
        public MobileServicesController(IFalconPort falconPort)
        {
            _falconPort = falconPort;
        }

        /// <summary>
        /// Looks up the Vehicle Details
        /// </summary>
        /// <param name="xmlInput"></param>
        /// <returns></returns>
        /// <response code = "201">Returns saved attachment info</response>
        /// <response code = "400">Invalid input parameters</response>
        /// <response code = "401">Active authorization is missing</response>
        //[SwaggerRequestExample(typeof(string),typeof(OONLookupVehicleExample))]
        [HttpPost("OONVehicleLookup")]
        [ProducesResponseType(typeof(string), 201)]
        public async Task<IActionResult> OONVehicleLookup([FromBody]string xmlInput)
        {
            try
            {
                if (xmlInput == null)
                {
                    string msg = string.Format("Invalid input data received <{0}>.  Verify input request data.", xmlInput);
                    return BadRequest(msg);
                }
                var xmlOutput = await _falconPort.SubmitFalconRequest(_aspPage, xmlInput);
                return StatusCode(StatusCodes.Status201Created, xmlOutput);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Processing error <{0}> received for PostAttachments request <{1}>.  Verify input request data.", ex.Message, xmlInput);
                return BadRequest(msg);
            }
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        //// GET api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
    }
}
