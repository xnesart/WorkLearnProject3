using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace WorkLearnProject3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController:ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public void Post([FromQuery] string value)
        {
            
        }
    }
}