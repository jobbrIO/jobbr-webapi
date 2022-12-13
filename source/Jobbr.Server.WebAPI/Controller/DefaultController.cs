using Microsoft.AspNetCore.Mvc;
using System;

namespace Jobbr.Server.WebAPI.Controller
{
    /// <summary>
    /// The default controller.
    /// </summary>
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly JobbrWebApiConfiguration _configuration;

        public DefaultController(JobbrWebApiConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// The are you fine.
        /// </summary>
        /// <returns>
        /// The <see cref="IHttpActionResult"/>.
        /// </returns>
        [HttpGet]
        [Route("status")]
        public IActionResult AreYouFine()
        {
            return Ok("Fine");
        }

        [HttpGet]
        [Route("configuration")]
        public IActionResult GetConfiguration()
        {
            return Ok(_configuration);
        }

        [HttpGet]
        [Route("fail")]
        public IActionResult Fail()
        {
            throw new Exception("This has failed!");
        }
    }
}
