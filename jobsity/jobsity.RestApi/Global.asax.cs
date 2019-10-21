using jobsity.RestApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace jobsity.RestApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            new Thread(loadFile).Start();
        }

        public void loadFile()
        {
            string path = Path.Combine(HttpRuntime.AppDomainAppPath, "aapl.us.csv");
            while (true)
            {
                List<List<string>> data = ExcelUtility.CargarExcel(new FileStream(path, FileMode.Open));
                aapl.aapls = data.Select(r => new aapl { Symbol = r[0], Date = r[1], Open = r[2], High = r[3], Low = r[4], Close = r[5], Volume = r[6] }).ToList();

                Thread.Sleep(60000);
            }
        }
    }
}
