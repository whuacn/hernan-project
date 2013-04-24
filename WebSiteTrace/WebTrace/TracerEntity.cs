using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebTrace
{
    public class TracerEntity
    {
        public TracerEntity()
        {
            Date = DateTime.Now;
        }
        public DateTime Date { get; set; }        
        public string Page { get; set; }
        public string Message { get; set; }
        public TracerType Type { get; set; }
        public HttpContext Context { get; set; }
    }
}
