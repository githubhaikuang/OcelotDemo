﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApiDemo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values

        private static int _count = 0;
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _count++;
            Console.WriteLine($"Get...{_count}");
            if (_count <= 3)
            {
                System.Threading.Thread.Sleep(5000);
            }

            return new string[] { $"ClinetService: {DateTime.Now.ToString(CultureInfo.InvariantCulture)} {Environment.MachineName} " +
                                  $"OS: {Environment.OSVersion.VersionString}" };

            //return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
