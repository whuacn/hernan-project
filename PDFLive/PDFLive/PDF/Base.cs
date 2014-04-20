using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDFLive
{
    public class Base : IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
        ~Base()
        {
            Dispose(false);
        }
        public void Close()
        {
            Dispose();
        }
    }
}
