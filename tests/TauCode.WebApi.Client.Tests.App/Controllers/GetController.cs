﻿using Microsoft.AspNetCore.Mvc;
using System;
using TauCode.WebApi.Client.Tests.App.Dto;

namespace TauCode.WebApi.Client.Tests.App.Controllers
{
    [ApiController]
    public class GetController : ControllerBase
    {
        [HttpGet]
        [Route("get-from-route/{name}/{salary}/{bornAt}")]
        public IActionResult GetFromRoute(
            [FromRoute]string name,
            [FromRoute]decimal salary,
            [FromRoute]DateTime bornAt)
        {
            var person = new PersonDto
            {
                Name = name,
                Salary = salary,
                BornAt = bornAt,
            };

            return this.Ok(person);
        }

        [HttpGet]
        [Route("get-returns-notfound")]
        public IActionResult GetReturnsNotFound()
        {
            return this.NotFound(new
            {
                firstProp = "first-prop",
                secondProp = "second-prop"
            });
        }
    }
}
