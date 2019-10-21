using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace jobsity.RestApi.Models
{
    public class aapl
    {
        public static List<aapl> aapls;

        [Key]
        public string Symbol { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public string Open { get; set; }

        public string High { get; set; }

        public string Low { get; set; }

        public string Close { get; set; }

        public string Volume { get; set; }
    }
}