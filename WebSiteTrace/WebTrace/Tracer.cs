using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace WebTrace
{
    public class Tracer
    {
        public static void Write(string message, TracerType type)
        {
            if (TracerisActive())
            {
                TracerEntity tracer = TracerFactory(message, type);
                TracerSave(tracer);
            }
        }

        static void TracerSave(TracerEntity tracer)
        {
            TracerList.Add(tracer);
        }

        public static void TracerClear()
        {
            TracerList = new List<TracerEntity>();
        }

        static bool TracerisActive()
        {
            if (HttpContext.Current.Session["TracerActive"] != null)
            {
                if (HttpContext.Current.Session["TracerActive"] == "On")
                {
                    return true;
                }
            }
            return false;
        }

        public static List<TracerEntity> TracerList
        {
            get
            {
                if (HttpContext.Current.Session["TracerList"] != null)
                {
                    try
                    {
                        return (List<TracerEntity>)HttpContext.Current.Session["TracerList"];
                    }
                    catch (Exception)
                    {
                        HttpContext.Current.Session["TracerList"] = new List<TracerEntity>();
                        return (List<TracerEntity>)HttpContext.Current.Session["TracerList"];
                    }
                }
                else
                {
                    HttpContext.Current.Session["TracerList"] = new List<TracerEntity>();
                }

                return (List<TracerEntity>)HttpContext.Current.Session["TracerList"];
            }
            set
            {
                HttpContext.Current.Session["TracerList"] = value;    
            }
        }

        static TracerEntity TracerFactory(string message, TracerType type)
        {
            TracerEntity tracer = new TracerEntity();
            tracer.Message = message;
            tracer.Type = type;
            tracer.Context = HttpContext.Current;
            tracer.Page = HttpContext.Current.Request.Url.ToString();
            return tracer;
        }
    }
}
