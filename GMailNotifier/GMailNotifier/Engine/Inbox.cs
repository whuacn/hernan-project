using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMailNotifier.Engine
{
    internal class Inbox
    {
        public string title { get; set; }
        public string tagline { get; set; }
        public int fullcount { get; set; }
        public string link { get; set; }
        public DateTime modified { get; set; }
        public List<Entry> entries;

        
    }
}
