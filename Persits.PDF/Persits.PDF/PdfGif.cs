using System;
namespace Persits.PDF
{
	internal class PdfGif : PdfTiff
	{
		private const int MAX_LZW_BITS = 12;
		private const int LZW_TABLE_SIZE = 4096;
		private const byte GIF_TERMINATOR = 59;
		private const byte GIF_IMAGEBLOCK = 44;
		private const byte GIF_EXTENSION = 33;
		private const int GIF_NUMCOLORS = 3;
		private const int CM_RED = 0;
		private const int CM_GREEN = 1;
		private const int CM_BLUE = 2;
		private bool m_bGlobalColorTableExists;
		private int m_nBitsPerPixel;
		private int m_nGlobalColorTableSize;
		private int m_nBackgroundColor;
		private ushort[] m_symbol_head;
		private byte[] m_symbol_tail;
		private byte[] m_symbol_stack;
		private int m_sp;
		public bool m_bTransparent;
		private byte m_nTranspIndex;
		private byte[] m_code_buf = new byte[260];
		private byte[] m_code_buf2 = new byte[260];
		private int m_last_byte;
		private int m_last_bit;
		private int m_cur_bit;
		private bool m_out_of_blocks;
		private int m_input_code_size;
		private int m_clear_code;
		private int m_end_code;
		private int m_code_size;
		private int m_limit_code;
		private int m_max_code;
		private bool m_first_time;
		private byte[][] m_arrGCT;
		public uint m_dwTransparencyColor;
		public PdfStream m_objTranspData = new PdfStream();
		private int m_oldcode;
		private int m_firstcode;
		public PdfGif(PdfManager pManager, PdfDocument pDoc) : base(pManager, pDoc)
		{
			this.m_bTransparent = false;
			this.m_nTranspIndex = 0;
			this.m_nBitsPerComponent = 8;
			this.m_nComponentsPerSample = 3;
			this.m_bstrColorSpace = "DeviceRGB";
			this.m_symbol_head = new ushort[4096];
			this.m_symbol_tail = new byte[4096];
			this.m_symbol_stack = new byte[4096];
			this.m_arrGCT = new byte[256][];
			for (int i = 0; i < 256; i++)
			{
				this.m_arrGCT[i] = new byte[3];
			}
			this.InitLZWCode();
		}
		~PdfGif()
		{
			this.m_symbol_head = null;
			this.m_symbol_tail = null;
			this.m_symbol_stack = null;
			this.m_arrGCT = null;
		}
		public override void ParseImage()
		{
			ulong num = (ulong)base.ReadLong();
			ushort num2 = base.ReadShort();
			if (num != 944130375uL || (num2 != 24889 && num2 != 25145 && num2 != 24887 && num2 != 25143))
			{
				AuxException.Throw("Invalid GIF signature.", PdfErrors._ERROR_GIF);
			}
			this.m_nWidth = (int)base.ReadShort();
			this.m_nHeight = (int)base.ReadShort();
			byte b = base.ReadByte();
			this.m_bGlobalColorTableExists = ((b & 128) > 0);
			this.m_nBitsPerPixel = ((b & 112) >> 4) + 1;
			this.m_nGlobalColorTableSize = 1 << (int)((b & 7) + 1);
			this.m_nBackgroundColor = (int)base.ReadByte();
			base.ReadByte();
			if (this.m_bGlobalColorTableExists)
			{
				for (int i = 0; i < this.m_nGlobalColorTableSize; i++)
				{
					this.m_arrGCT[i][0] = base.ReadByte();
					this.m_arrGCT[i][1] = base.ReadByte();
					this.m_arrGCT[i][2] = base.ReadByte();
				}
			}
			while (true)
			{
				byte b2 = base.ReadByte();
				if (b2 == 59)
				{
					break;
				}
				if (b2 == 33)
				{
					this.DoExtension();
				}
				else
				{
					if (b2 == 44)
					{
						goto Block_8;
					}
				}
			}
			return;
			Block_8:
			this.ParseImageBlock();
		}
		private void ParseImageBlock()
		{
			base.ReadShort();
			base.ReadShort();
			this.m_nWidth = (int)base.ReadShort();
			this.m_nHeight = (int)base.ReadShort();
			int num = this.m_nHeight * ((this.m_nWidth + 7) / 8);
			byte[] array = new byte[this.m_nWidth * this.m_nHeight * 3];
			byte[] array2 = null;
			if (this.m_bTransparent)
			{
				array2 = new byte[num];
			}
			byte b = base.ReadByte();
			bool flag = (b & 128) > 0;
			int num2 = 1 << (int)((b & 7) + 1);
			bool flag2 = (b & 64) > 0;
			if (flag)
			{
				for (int i = 0; i < num2; i++)
				{
					this.m_arrGCT[i][0] = base.ReadByte();
					this.m_arrGCT[i][1] = base.ReadByte();
					this.m_arrGCT[i][2] = base.ReadByte();
				}
			}
			if (this.m_bTransparent)
			{
				this.m_dwTransparencyColor = PdfGif.RGB(this.m_arrGCT[(int)this.m_nTranspIndex][2], this.m_arrGCT[(int)this.m_nTranspIndex][1], this.m_arrGCT[(int)this.m_nTranspIndex][0]);
			}
			this.m_input_code_size = (int)base.ReadByte();
			this.InitLZWCode();
			int num3 = 0;
			int num4 = 0;
			int[] array3 = new int[]
			{
				8,
				8,
				4,
				2
			};
			int[] array4 = new int[]
			{
				0,
				4,
				2,
				1
			};
			int num5 = 0;
			int j = 0;
			byte b2 = 128;
			for (int k = 0; k < this.m_nHeight; k++)
			{
				if (flag2)
				{
					num3 = 3 * this.m_nWidth * j;
					if (this.m_bTransparent)
					{
						num4 = j * ((this.m_nWidth + 7) / 8);
					}
				}
				for (int l = this.m_nWidth; l > 0; l--)
				{
					int num6 = this.LZWReadByte();
					array[num3++] = this.m_arrGCT[num6][0];
					array[num3++] = this.m_arrGCT[num6][1];
					array[num3++] = this.m_arrGCT[num6][2];
					if (this.m_bTransparent)
					{
						if (num6 == (int)this.m_nTranspIndex)
						{
							byte[] expr_1F4_cp_0 = array2;
							int expr_1F4_cp_1 = num4;
							expr_1F4_cp_0[expr_1F4_cp_1] |= b2;
						}
						if ((b2 = (byte)(b2 >> 1)) == 0 || l == 1)
						{
							num4++;
							b2 = 128;
						}
					}
				}
				if (flag2 && k != this.m_nHeight - 1)
				{
					for (j += array3[num5]; j >= this.m_nHeight; j = array4[num5])
					{
						num5++;
					}
				}
			}
			this.m_objData.Set(array);
			if (this.m_bTransparent)
			{
				this.m_objTranspData.Set(array2);
			}
		}
		private void ReInitLZW()
		{
			this.m_code_size = this.m_input_code_size + 1;
			this.m_limit_code = this.m_clear_code << 1;
			this.m_max_code = this.m_clear_code + 2;
			this.m_sp = 0;
		}
		private void InitLZWCode()
		{
			this.m_last_byte = 2;
			this.m_last_bit = 0;
			this.m_cur_bit = 0;
			this.m_out_of_blocks = false;
			this.m_clear_code = 1 << this.m_input_code_size;
			this.m_end_code = this.m_clear_code + 1;
			this.m_first_time = true;
			this.ReInitLZW();
		}
		private int GetDataBlock(byte[] buff)
		{
			int num = (int)base.ReadByte();
			if (num > 0)
			{
				this.m_pInput.ReadBytes(this.m_dwPtr, buff, num);
				this.m_dwPtr += num;
			}
			return num;
		}
		private void SkipDataBlocks()
		{
			byte[] buff = new byte[256];
			while (this.GetDataBlock(buff) > 0)
			{
			}
		}
		public static uint RGB(byte r, byte g, byte b)
		{
			return (uint)((int)r + ((int)g << 8) + ((int)b << 16));
		}
		private int LZWReadByte()
		{
			int i;
			if (this.m_first_time)
			{
				this.m_first_time = false;
				i = this.m_clear_code;
			}
			else
			{
				if (this.m_sp > 0)
				{
					return (int)this.m_symbol_stack[--this.m_sp];
				}
				i = this.GetCode();
			}
			if (i == this.m_clear_code)
			{
				this.ReInitLZW();
				do
				{
					i = this.GetCode();
				}
				while (i == this.m_clear_code);
				if (i > this.m_clear_code)
				{
					i = 0;
				}
				this.m_firstcode = (this.m_oldcode = i);
				return i;
			}
			if (i == this.m_end_code)
			{
				if (!this.m_out_of_blocks)
				{
					this.SkipDataBlocks();
					this.m_out_of_blocks = true;
				}
				return 0;
			}
			int oldcode = i;
			if (i >= this.m_max_code)
			{
				if (i > this.m_max_code)
				{
					oldcode = 0;
				}
				this.m_symbol_stack[this.m_sp++] = (byte)this.m_firstcode;
				i = this.m_oldcode;
			}
			while (i >= this.m_clear_code)
			{
				this.m_symbol_stack[this.m_sp++] = this.m_symbol_tail[i];
				i = (int)this.m_symbol_head[i];
			}
			this.m_firstcode = i;
			if ((i = this.m_max_code) < 4096)
			{
				this.m_symbol_head[i] = (ushort)this.m_oldcode;
				this.m_symbol_tail[i] = (byte)this.m_firstcode;
				this.m_max_code++;
				if (this.m_max_code >= this.m_limit_code && this.m_code_size < 12)
				{
					this.m_code_size++;
					this.m_limit_code <<= 1;
				}
			}
			this.m_oldcode = oldcode;
			return this.m_firstcode;
		}
		private int GetCode()
		{
			while (this.m_cur_bit + this.m_code_size > this.m_last_bit)
			{
				if (this.m_out_of_blocks)
				{
					return this.m_end_code;
				}
				this.m_code_buf[0] = this.m_code_buf[this.m_last_byte - 2];
				this.m_code_buf[1] = this.m_code_buf[this.m_last_byte - 1];
				int dataBlock;
				if ((dataBlock = this.GetDataBlock(this.m_code_buf2)) == 0)
				{
					this.m_out_of_blocks = true;
					return this.m_end_code;
				}
				Array.Copy(this.m_code_buf2, 0, this.m_code_buf, 2, dataBlock);
				this.m_cur_bit = this.m_cur_bit - this.m_last_bit + 16;
				this.m_last_byte = 2 + dataBlock;
				this.m_last_bit = this.m_last_byte * 8;
			}
			int num = this.m_cur_bit >> 3;
			int num2 = (int)(this.m_code_buf[num + 2] & 255);
			num2 <<= 8;
			num2 |= (int)(this.m_code_buf[num + 1] & 255);
			num2 <<= 8;
			num2 |= (int)(this.m_code_buf[num] & 255);
			num2 >>= (this.m_cur_bit & 7);
			int result = num2 & (1 << this.m_code_size) - 1;
			this.m_cur_bit += this.m_code_size;
			return result;
		}
		private void ReadColorMap(int cmaplen, byte[][] cmap)
		{
			for (int i = 0; i < cmaplen; i++)
			{
				cmap[0][i] = base.ReadByte();
				cmap[1][i] = base.ReadByte();
				cmap[2][i] = base.ReadByte();
			}
		}
		private void DoExtension()
		{
			int num = (int)base.ReadByte();
			if (num == 249)
			{
				int num2 = (int)base.ReadByte();
				if (num2 == 4)
				{
					ulong num3 = (ulong)base.ReadLong();
					this.m_bTransparent = ((num3 & 1uL) > 0uL);
                    this.m_nTranspIndex = (byte)((num3 & 4278190080UL) >> 24);
					if (this.m_bTransparent)
					{
						int arg_50_0 = this.m_nHeight;
						int arg_5B_0 = (this.m_nWidth + 7) / 8;
					}
				}
				else
				{
					this.m_dwPtr--;
				}
			}
			this.SkipDataBlocks();
		}
	}
}
