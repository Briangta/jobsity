using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jobsity.RestApi.Models
{
    public class GlobarReturn<T>
    {
        public string error { get; set;}
        public T obj { get; set; }
    }
}