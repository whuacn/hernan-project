using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.html.simpleparser;

namespace PDFLive.ConvertHTML
{
    public class Structures
    {
        public struct TagHeader
        {
            public float Height;
            public String auth;
            public String texto;
            public String baseurl;
            public StyleSheet css;
        }

        public struct TagFooter
        {
            public float Height;
            public String auth;
            public String texto;
            public String baseurl;
            public StyleSheet css;
        }

    }
}
