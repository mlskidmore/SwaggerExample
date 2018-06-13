using SwaggerExample.FalconRequests;
using SwaggerExample.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace SwaggerExample.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/xml", "application/json")]
    [Consumes("application/xml", "application/json")]
    public class MobileBackEndController : Controller
    {
        readonly IFalconPort _falconPort;
        const string _aspPage = "MobileBackEnd.asp";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="falconPort"></param>
        public MobileBackEndController(IFalconPort falconPort)
        {
            _falconPort = falconPort;
        }

        /// <summary>
        /// Adds attachment from mobile device
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code = "201">Returns saved attachment info</response>
        /// <response code = "400">Invalid input parameters</response>
        /// <response code = "401">Active authorization is missing</response>
        /// <response code = "1000">Work_Assignment ID Missing</response>
        /// <response code = "2000">Role Code Missing</response>
        /// <response code = "3000">usiness Category Missing Missing</response>
        /// <response code = "4000">Invalid Ticket</response>
        //[SwaggerRequestExample(typeof(string),typeof(OONLookupVehicleExample))]
        [HttpPost("MobileBackEndAddAttachment")]
        [ProducesResponseType(typeof(MBESVRAddAttachmentRequest), 201)]
        public async Task<IActionResult> MobileBackEndAddAttachment([FromBody] MBESVRAddAttachmentRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest($"Invalid input data received. Verify input request data.");
                }
                //var xmlOutput = request.ToXml("Request");
                var xmlOutput = await _falconPort.SubmitFalconRequest(_aspPage, SwaggerExampleRequest<MBESVRAddAttachmentRequest>.CreateRequest(request, this.GetMethodName()));
                return StatusCode(StatusCodes.Status201Created, xmlOutput);
            }
            catch (Exception ex)
            {
                return BadRequest($"Processing error [{ ex }] received for PostAttachments request [{ request.ToXml() }].  Verify input request data.");
            }
        }
        /// <summary>
        /// Adds event from mobile device
        /// </summary>
        [HttpPost("MobileBackEndAddAttEvent")]
        [ProducesResponseType(typeof(MBESVRAddAttachmentRequest), 201)]
        public async Task<IActionResult> MobileBackEndAddAttEvent([FromBody] MBESVRAddAttEventRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest($"Invalid input data received. Verify input request data.");
                }
                var xmlOutput = await _falconPort.SubmitFalconRequest(_aspPage, SwaggerExampleRequest<MBESVRAddAttEventRequest>.CreateRequest(request, this.GetMethodName()));
                return StatusCode(StatusCodes.Status201Created, xmlOutput);
            }
            catch (Exception ex)
            {
                return BadRequest($"Processing error [{ ex }] received for PostAttachments request [{ request.ToXml() }].  Verify input request data.");
            }
        }

        /// <summary>
        /// Updates a vehicle from a mobile device
        /// </summary>
        [HttpPost("MobileBackEndUpdateVehicle")]
        [ProducesResponseType(typeof(MBESVRAddAttachmentRequest), 201)]
        public async Task<IActionResult> MobileBackEndUpdateVehicle([FromBody] MBESVRUpdateVehicle request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest($"Invalid input data received. Verify input request data.");
                }
                var xmlOutput = request.ToXml("Request");
                xmlOutput = await _falconPort.SubmitFalconRequest(_aspPage, SwaggerExampleRequest<MBESVRUpdateVehicle>.CreateRequest(request, this.GetMethodName()));
                return StatusCode(StatusCodes.Status201Created, request);
            }
            catch (Exception ex)
            {
                return BadRequest($"Processing error [{ ex }] received for PostAttachments request [{ request.ToXml() }].  Verify input request data.");
            }
        }
    }
}