using jobsity.RestApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace jobsity.RestApi.Controllers
{
    public class ChatBotController : ApiController
    {
        [HttpGet]
        public IEnumerable<string> GetAllowedCommands()
        {            
            return ConfigurationManager.AppSettings["AllowedCommands"].Split(',');
        }

        public GlobarReturn<aapl> getStock(string stock_code)
        {
            var obj = aapl.aapls.Where(r => r.Symbol == stock_code).FirstOrDefault();
            if (obj == null)
                return new GlobarReturn<aapl> { error = "Code not Found" };
            return new GlobarReturn<aapl> { obj=obj};
        }


    }
}