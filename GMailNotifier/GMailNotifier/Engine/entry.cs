using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMailNotifier.Engine
{
    internal class Entry
    {
        public string id { get; set; }
        public string title { get; set; }
        public string summary { get; set; }
        public string link { get; set; }
        public DateTime modified { get; set; }
        public DateTime issued { get; set; }
        public author author { get; set; }
        public author contributor { get; set; }
        public bool Notify { get; set; }


        public override bool Equals(object obj)
        {
            if (obj is Entry)
                return false;

            if (((Entry)obj).id == this.id)
                return true;

            return false;
        }
    }
}
