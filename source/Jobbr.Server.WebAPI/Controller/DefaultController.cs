using System;
using Microsoft.AspNetCore.Mvc;

namespace Jobbr.Server.WebAPI.Controller
{
    /// <summary>
    /// The default controller.
    /// </summary>
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly JobbrWebApiConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultController"/> class.
        /// </summary>
        /// <param name="configuration">Jobbr Web API configuration.</param>
        public DefaultController(JobbrWebApiConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Service health check.
        /// </summary>
        /// <returns>OkResult with the text "Fine".</returns>
        [HttpGet("status")]
        public IActionResult AreYouFine()
        {
            return Ok("Fine");
        }

        /// <summary>
        /// Get the Jobbr Web API configuration object.
        /// </summary>
        /// <returns>ActionResult with the configuration object.</returns>
        [HttpGet("configuration")]
        public IActionResult GetConfiguration()
        {
            return Ok(_configuration);
        }

        /// <summary>
        /// Intentionally throw an exception.
        /// </summary>
        /// <returns>Never returns.</returns>
        /// <exception cref="Exception">Always throws.</exception>
        [HttpGet("fail")]
        public IActionResult Fail()
        {
            throw new Exception("This has failed!");
        }
    }
}
