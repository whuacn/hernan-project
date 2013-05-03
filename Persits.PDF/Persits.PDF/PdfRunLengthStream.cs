using System;
namespace Persits.PDF
{
	internal class PdfRunLengthStream : PdfDecoderHelper
	{
		private PdfStream str;
		private byte[] buf = new byte[128];
		private RavenColorPtr bufPtr;
		private RavenColorPtr bufEnd;
		private bool eof;
		internal PdfRunLengthStream(PdfStream strA)
		{
			this.bufPtr = new RavenColorPtr(this.buf);
			this.bufEnd = new RavenColorPtr(this.buf);
			this.eof = false;
			this.str = strA;
		}
		internal override void reset()
		{
			this.str.reset();
			this.bufPtr.Set(this.buf, 0);
			this.bufEnd.Set(this.buf, 0);
			this.eof = false;
		}
		internal override int getChar()
		{
			if (this.bufPtr.offset >= this.bufEnd.offset && !this.fillBuf())
			{
				return -1;
			}
            byte result = (byte)((uint)this.bufPtr.ptr & (uint)byte.MaxValue);
			this.bufPtr = ++this.bufPtr;
			return (int)result;
		}
		internal int lookChar()
		{
			if (this.bufPtr.offset < this.bufEnd.offset || this.fillBuf())
			{
				return (int)(this.bufPtr.ptr & 255);
			}
			return -1;
		}
		private void CopyTo(PdfStream pTo)
		{
			int @char;
			while ((@char = this.getChar()) != -1)
			{
				pTo.AppendChar((byte)@char);
			}
		}
		private bool fillBuf()
		{
			if (this.eof)
			{
				return false;
			}
			int @char = this.str.getChar();
			if (@char == 128 || @char == -1)
			{
				this.eof = true;
				return false;
			}
			int num;
			if (@char < 128)
			{
				num = @char + 1;
				for (int i = 0; i < num; i++)
				{
					this.buf[i] = (byte)this.str.getChar();
				}
			}
			else
			{
				num = 257 - @char;
				@char = this.str.getChar();
				for (int i = 0; i < num; i++)
				{
					this.buf[i] = (byte)@char;
				}
			}
			this.bufPtr.Set(this.buf, 0);
			this.bufEnd.Set(this.buf, num);
			return true;
		}
	}
}
