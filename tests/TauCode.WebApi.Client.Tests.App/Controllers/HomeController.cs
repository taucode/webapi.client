using Microsoft.AspNetCore.Mvc;
using TauCode.WebApi.Client.Tests.App.Dto;

namespace TauCode.WebApi.Client.Tests.App.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("get-ok")]
        public IActionResult GetOk()
        {
            var person = new PersonDto
            {
                Name = "olia",
                Salary = 14.88m,
            };

            return this.Ok(person);
        }
    }
}
