using System;
using System.IO;
namespace Persits.PDF.zlib
{
	internal class ZInputStream : BinaryReader
	{
		protected ZStream z = new ZStream();
		protected int bufsize = 512;
		protected int flush;
		protected byte[] buf;
		protected byte[] buf1 = new byte[1];
		protected bool compress;
		internal Stream in_Renamed;
		internal bool nomoreinput;
		public virtual int FlushMode
		{
			get
			{
				return this.flush;
			}
			set
			{
				this.flush = value;
			}
		}
		public virtual long TotalIn
		{
			get
			{
				return this.z.total_in;
			}
		}
		public virtual long TotalOut
		{
			get
			{
				return this.z.total_out;
			}
		}
		internal void InitBlock()
		{
			this.flush = 0;
			this.buf = new byte[this.bufsize];
		}
		public ZInputStream(Stream in_Renamed) : base(in_Renamed)
		{
			this.InitBlock();
			this.in_Renamed = in_Renamed;
			this.z.inflateInit();
			this.compress = false;
			this.z.next_in = this.buf;
			this.z.next_in_index = 0;
			this.z.avail_in = 0;
		}
		public ZInputStream(Stream in_Renamed, int level) : base(in_Renamed)
		{
			this.InitBlock();
			this.in_Renamed = in_Renamed;
			this.z.deflateInit(level);
			this.compress = true;
			this.z.next_in = this.buf;
			this.z.next_in_index = 0;
			this.z.avail_in = 0;
		}
		public override int Read()
		{
			if (this.read(this.buf1, 0, 1) == -1)
			{
				return -1;
			}
			return (int)(this.buf1[0] & 255);
		}
		public int read(byte[] b, int off, int len)
		{
			if (len == 0)
			{
				return 0;
			}
			this.z.next_out = b;
			this.z.next_out_index = off;
			this.z.avail_out = len;
			while (true)
			{
				if (this.z.avail_in == 0 && !this.nomoreinput)
				{
					this.z.next_in_index = 0;
					this.z.avail_in = SupportClass.ReadInput(this.in_Renamed, this.buf, 0, this.bufsize);
					if (this.z.avail_in == -1)
					{
						this.z.avail_in = 0;
						this.nomoreinput = true;
					}
				}
				int num;
				if (this.compress)
				{
					num = this.z.deflate(this.flush);
				}
				else
				{
					num = this.z.inflate(this.flush);
				}
				if (this.nomoreinput && num == -5)
				{
					break;
				}
				if (num != 0 && num != 1)
				{
					goto Block_9;
				}
				if (this.nomoreinput && this.z.avail_out == len)
				{
					return -1;
				}
				if (this.z.avail_out != len || num != 0)
				{
					goto IL_12D;
				}
			}
			return -1;
			Block_9:
			throw new PdfZLibException((this.compress ? "de" : "in") + "flating: " + this.z.msg);
			IL_12D:
			return len - this.z.avail_out;
		}
		public long skip(long n)
		{
			int num = 512;
			if (n < (long)num)
			{
				num = (int)n;
			}
			byte[] array = new byte[num];
			return (long)SupportClass.ReadInput(this.BaseStream, array, 0, array.Length);
		}
		public override void Close()
		{
			this.in_Renamed.Close();
		}
	}
}
