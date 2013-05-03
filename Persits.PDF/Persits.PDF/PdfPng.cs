using System;
namespace Persits.PDF
{
	internal class PdfPng : PdfTiff
	{
		private const int PNG_FILTER_VALUE_SUB = 1;
		private const int PNG_FILTER_VALUE_UP = 2;
		private const int PNG_FILTER_VALUE_AVG = 3;
		private const int PNG_FILTER_VALUE_PAETH = 4;
		private const int PNG_COLOR_MASK_PALETTE = 1;
		private const int PNG_COLOR_MASK_COLOR = 2;
		private const int PNG_COLOR_MASK_ALPHA = 4;
		private static ShiftMap[] m_arrPassShifts;
		private byte[][] m_arrPalette = new byte[256][];
		private byte[] m_arrTranspPalette = new byte[256];
		private byte[] m_arrTranspColor = new byte[6];
		public bool m_bPalette;
		private bool m_bHeaderRead;
		private int m_nColorType;
		private int m_nCompressionMethod;
		private int m_nFilterMethod;
		private int m_nInterlaceMethod;
		public bool m_bAlpha;
		private bool m_bGray;
		public bool m_bTransparency;
		public byte[] m_pAlphaData;
		private PdfStream m_objRaw;
		private byte[] m_pRaw;
		public PdfPng(PdfManager pManager, PdfDocument pDoc) : base(pManager, pDoc)
		{
			for (int i = 0; i < 256; i++)
			{
				this.m_arrPalette[i] = new byte[3];
			}
			this.m_bBigEndian = true;
			this.m_bHeaderRead = false;
			this.m_bTransparency = false;
			this.m_objRaw = new PdfStream();
		}
		~PdfPng()
		{
		}
		public override void ParseImage()
		{
			uint num = base.ReadLong();
			uint num2 = base.ReadLong();
			if (num != 2303741511u || num2 != 218765834u)
			{
				AuxException.Throw("Invalid PNG signature. Expected 137 80 78 71 13 10 26 10.", PdfErrors._ERROR_PNG);
			}
			bool flag = false;
			while (!flag && this.m_dwPtr < this.m_pInput.m_nSize - 1)
			{
				int num3 = (int)base.ReadLong();
				int num4 = (int)base.ReadLong();
				int num5 = num4;
				if (num5 <= 1229278788)
				{
					if (num5 != 1229209940)
					{
						if (num5 != 1229278788)
						{
							goto IL_A6;
						}
						flag = true;
					}
					else
					{
						this.ProcessDataChunk(num3);
					}
				}
				else
				{
					if (num5 != 1229472850)
					{
						if (num5 != 1347179589)
						{
							if (num5 != 1951551059)
							{
								goto IL_A6;
							}
							this.ProcessTransparencyChunk(num3);
						}
						else
						{
							this.ProcessPaletteChunk(num3);
						}
					}
					else
					{
						this.ProcessHeaderChunk(num3);
					}
				}
				IL_B4:
				this.m_dwPtr += 4;
				continue;
				IL_A6:
				this.m_dwPtr += num3;
				goto IL_B4;
			}
			this.DecompressData();
			if (this.m_nBitsPerComponent < 8 && (this.m_bPalette || this.m_nInterlaceMethod == 1))
			{
				this.m_nBitsPerComponent = 8;
				this.m_nComponentsPerSample = 3;
			}
			if (this.m_nBitsPerComponent > 8 && this.m_nInterlaceMethod == 1)
			{
				this.m_nBitsPerComponent = 8;
				this.m_nComponentsPerSample = 3;
			}
		}
		private void DecompressData()
		{
			if (this.m_objRaw.DecodeFlate() != 0)
			{
				AuxException.Throw("PNG data decompression failed.", PdfErrors._ERROR_PNG);
			}
			this.m_pRaw = this.m_objRaw.ToBytes();
			int num = this.m_nBitsPerComponent / 8;
			int num2 = 3 * this.m_nBitsPerComponent / 8;
			if (this.m_bPalette || this.m_bGray)
			{
				num2 = this.m_nBitsPerComponent / 8;
			}
			if (this.m_bAlpha)
			{
				num2 += this.m_nBitsPerComponent / 8;
			}
			int num3 = 3 * this.m_nBitsPerComponent / 8;
			if (this.m_bGray)
			{
				num3 = this.m_nBitsPerComponent / 8;
			}
			int num4 = this.m_nWidth * num2;
			int num5 = this.m_nWidth * num3;
			int num6 = 8 / this.m_nBitsPerComponent;
			byte b = 255;
			if (this.m_nBitsPerComponent < 8)
			{
				num2 = 1;
				num3 = 3;
				num4 = (this.m_nWidth + num6 - 1) / num6;
				num5 = this.m_nWidth * num3;
				if (this.m_nBitsPerComponent == 1)
				{
					b = 128;
				}
				else
				{
					if (this.m_nBitsPerComponent == 2)
					{
						b = 192;
					}
					else
					{
						b = 240;
					}
				}
			}
			if (this.m_bAlpha || this.m_bTransparency)
			{
				this.m_pAlphaData = new byte[this.m_nWidth * this.m_nHeight * num2];
			}
			if (this.m_nInterlaceMethod == 1)
			{
				this.HandleInterlaced(num2, num3);
				return;
			}
			byte[] array = new byte[num5];
			int num7 = 0;
			int num8 = 0;
			byte[] array2 = new byte[num4];
			for (int i = 0; i < this.m_nHeight; i++)
			{
				this.ApplyFilter(num7, i, num2, num4 + 1);
				if (!this.m_bAlpha && !this.m_bPalette)
				{
					Array.Copy(this.m_pRaw, num7 + 1, array2, 0, num4);
					this.m_objData.Append(array2);
					if (this.m_bTransparency)
					{
						for (int j = 0; j < num4; j += num2)
						{
                            this.m_pAlphaData[num8++] = (this.ColorEquals(this.m_pRaw, num7 + 1 + j) ? (byte)0 : byte.MaxValue);
						}
					}
				}
				else
				{
					if (this.m_bPalette)
					{
						int num9 = 0;
						int num10 = this.m_nWidth;
						for (int k = 0; k < num4; k++)
						{
							byte b2 = b;
							for (int l = 0; l < num6; l++)
							{
								byte b3 = (byte)((this.m_pRaw[num7 + k + 1] & b2) >> this.m_nBitsPerComponent * (num6 - l - 1));
								b2 = (byte)(b2 >> this.m_nBitsPerComponent);
								array[num9] = this.m_arrPalette[(int)b3][0];
								array[num9 + 1] = this.m_arrPalette[(int)b3][1];
								array[num9 + 2] = this.m_arrPalette[(int)b3][2];
								if (this.m_bTransparency)
								{
									this.m_pAlphaData[num8++] = this.m_arrTranspPalette[(int)b3];
								}
								num9 += 3;
								num10--;
								if (num10 == 0)
								{
									break;
								}
							}
						}
						this.m_objData.Append(array);
					}
					else
					{
						if (this.m_bAlpha)
						{
							int num11 = 0;
							for (int m = 0; m < num5; m++)
							{
								array[m] = this.m_pRaw[num7 + num11 + 1];
								num11++;
								if ((num11 + num) % num2 == 0)
								{
									this.m_pAlphaData[num8++] = this.m_pRaw[num7 + num11++ + 1];
									if (num == 2)
									{
										num11++;
									}
								}
							}
							this.m_objData.Append(array);
						}
					}
				}
				num7 += num4 + 1;
			}
		}
		private void HandleInterlaced(int nBpp, int nOutBpp)
		{
			int[][] array = new int[7][];
			for (int i = 0; i < 7; i++)
			{
				array[i] = new int[2];
			}
			int num = this.m_nWidth / 8;
			int num2 = this.m_nHeight / 8;
			int num3 = this.m_nWidth % 8;
			int num4 = this.m_nHeight % 8;
			array[0][0] = (this.m_nWidth + 7) / 8;
			array[0][1] = (this.m_nHeight + 7) / 8;
			array[1][0] = num + ((num3 >= 5) ? 1 : 0);
			array[1][1] = num2 + ((num4 >= 1) ? 1 : 0);
			array[2][0] = 2 * num + ((num3 >= 1) ? 1 : 0) + ((num3 >= 5) ? 1 : 0);
			array[2][1] = num2 + ((num4 >= 5) ? 1 : 0);
			array[3][0] = 2 * num + ((num3 >= 3) ? 1 : 0) + ((num3 >= 7) ? 1 : 0);
			array[3][1] = 2 * num2 + ((num4 >= 1) ? 1 : 0) + ((num4 >= 5) ? 1 : 0);
			array[4][0] = 4 * num + ((num3 >= 1) ? 1 : 0) + ((num3 >= 3) ? 1 : 0) + ((num3 >= 5) ? 1 : 0) + ((num3 >= 7) ? 1 : 0);
			array[4][1] = 2 * num2 + ((num4 >= 3) ? 1 : 0) + ((num4 >= 7) ? 1 : 0);
			array[5][0] = 4 * num + ((num3 >= 2) ? 1 : 0) + ((num3 >= 4) ? 1 : 0) + ((num3 >= 6) ? 1 : 0) + ((num3 >= 8) ? 1 : 0);
			array[5][1] = 4 * num2 + ((num4 >= 1) ? 1 : 0) + ((num4 >= 3) ? 1 : 0) + ((num4 >= 5) ? 1 : 0) + ((num4 >= 7) ? 1 : 0);
			array[6][0] = 8 * num + num3;
			array[6][1] = 4 * num2 + ((num4 >= 2) ? 1 : 0) + ((num4 >= 4) ? 1 : 0) + ((num4 >= 6) ? 1 : 0) + ((num4 >= 8) ? 1 : 0);
			byte[] array2 = new byte[this.m_nWidth * this.m_nHeight * 3];
			int num5 = 0;
			for (int j = 0; j < 7; j++)
			{
				if (array[j][0] != 0 && array[j][1] != 0)
				{
					PdfStream pdfStream = new PdfStream();
					int num6 = array[j][0];
					int num7 = array[j][1];
					int num8 = num6 * nBpp;
					if (this.m_nBitsPerComponent < 8)
					{
						num8 = (num8 + 8 / this.m_nBitsPerComponent - 1) / (8 / this.m_nBitsPerComponent);
					}
					byte[] array3 = new byte[num8];
					for (int k = 0; k < num7; k++)
					{
						this.ApplyFilter(num5, k, nBpp, num8 + 1);
						Array.Copy(this.m_pRaw, num5 + 1, array3, 0, num8);
						pdfStream.Append(array3);
						num5 += num8 + 1;
					}
					this.Spray(array2, pdfStream.ToBytes(), array[j][0], array[j][1], j, nBpp);
				}
			}
			this.m_bstrColorSpace = "DeviceRGB";
			this.m_objData.Set(array2);
		}
		private void Spray(byte[] pData, byte[] pSubImage, int nWidth, int nHeight, int nPass, int nBpp)
		{
			int num = this.m_bGray ? 0 : (this.m_nBitsPerComponent / 8);
			int num2 = 0;
			byte b = 255;
			if (this.m_nBitsPerComponent == 1)
			{
				b = 128;
			}
			else
			{
				if (this.m_nBitsPerComponent == 2)
				{
					b = 192;
				}
				else
				{
					if (this.m_nBitsPerComponent == 4)
					{
						b = 240;
					}
				}
			}
			int num3 = 8 / this.m_nBitsPerComponent;
			if (num3 == 0)
			{
				num3 = 1;
			}
			int num4 = 1;
			if (this.m_nBitsPerComponent == 1)
			{
				num4 = 255;
			}
			else
			{
				if (this.m_nBitsPerComponent == 2)
				{
					num4 = 85;
				}
				else
				{
					if (this.m_nBitsPerComponent == 4)
					{
						num4 = 17;
					}
				}
			}
			for (int i = 0; i < nHeight; i++)
			{
				int num5 = i % PdfPng.m_arrPassShifts[nPass].m_nHeight * PdfPng.m_arrPassShifts[nPass].m_nWidth;
				int num6 = 3 * (this.m_nWidth * PdfPng.m_arrPassShifts[nPass].m_arrXY[num5].y + PdfPng.m_arrPassShifts[nPass].m_arrXY[num5].x);
				num6 += 24 * this.m_nWidth * (i / PdfPng.m_arrPassShifts[nPass].m_nHeight);
				byte b2 = b;
				int num7 = 0;
				for (int j = 0; j < nWidth; j++)
				{
					byte b3 = (byte)((pSubImage[num2] & b2) >> this.m_nBitsPerComponent * (num3 - num7 - 1));
					num7 = (num7 + 1) % num3;
					b2 = (byte)(b2 >> this.m_nBitsPerComponent);
					if (b2 == 0 || j == nWidth - 1)
					{
						b2 = b;
					}
					else
					{
						num2 -= nBpp;
					}
					if (this.m_bPalette)
					{
						pData[num6] = this.m_arrPalette[(int)b3][0];
						pData[num6 + 1] = this.m_arrPalette[(int)b3][1];
						pData[num6 + 2] = this.m_arrPalette[(int)b3][2];
						if (this.m_bTransparency)
						{
							this.m_pAlphaData[num6 / 3] = this.m_arrTranspPalette[(int)b3];
						}
					}
					else
					{
						if (this.m_nBitsPerComponent < 8)
						{
							pData[num6] = (pData[num6 + 1] = (pData[num6 + 2] = (byte)((int)b3 * num4)));
						}
						else
						{
							pData[num6] = pSubImage[num2];
							pData[num6 + 1] = pSubImage[num2 + num];
							pData[num6 + 2] = pSubImage[num2 + num + num];
							if (this.m_bAlpha)
							{
								this.m_pAlphaData[num6 / 3] = pSubImage[num2 + 3 * num];
							}
							if (this.m_bTransparency)
							{
                                this.m_pAlphaData[num6 / 3] = (this.ColorEquals(pSubImage, num2) ? (byte)0 : byte.MaxValue);
							}
						}
					}
					num2 += nBpp;
					int x = PdfPng.m_arrPassShifts[nPass].m_arrXY[num5].x;
					num5++;
					if (num5 % PdfPng.m_arrPassShifts[nPass].m_nWidth == 0)
					{
						num5 = i % PdfPng.m_arrPassShifts[nPass].m_nHeight * PdfPng.m_arrPassShifts[nPass].m_nWidth;
					}
					int x2 = PdfPng.m_arrPassShifts[nPass].m_arrXY[num5].x;
					if (x2 > x)
					{
						num6 += 3 * (x2 - x);
					}
					else
					{
						num6 += 3 * (8 - x + x2);
					}
				}
			}
		}
		private bool ColorEquals(byte[] Array, int nPtr)
		{
			if (this.m_nBitsPerComponent == 8)
			{
				return Array[nPtr] == this.m_arrTranspColor[1] && Array[nPtr + 1] == this.m_arrTranspColor[3] && Array[nPtr + 2] == this.m_arrTranspColor[5];
			}
			return this.m_nBitsPerComponent == 16 && (Array[nPtr] == this.m_arrTranspColor[0] && Array[nPtr + 1] == this.m_arrTranspColor[1] && Array[nPtr + 2] == this.m_arrTranspColor[2] && Array[nPtr + 3] == this.m_arrTranspColor[3] && Array[nPtr + 4] == this.m_arrTranspColor[4]) && Array[nPtr + 5] == this.m_arrTranspColor[5];
		}
		private void ProcessHeaderChunk(int dwLength)
		{
			if (dwLength != 13)
			{
				AuxException.Throw(string.Format("Invalid PNG header length ({0}). Should be 13.", dwLength), PdfErrors._ERROR_PNG);
			}
			this.m_nWidth = (int)base.ReadLong();
			this.m_nHeight = (int)base.ReadLong();
			this.m_nBitsPerComponent = (int)base.ReadByte();
			this.m_nColorType = (int)base.ReadByte();
			this.m_nCompressionMethod = (int)base.ReadByte();
			this.m_nFilterMethod = (int)base.ReadByte();
			this.m_nInterlaceMethod = (int)base.ReadByte();
			this.m_bAlpha = ((this.m_nColorType & 4) > 0);
			this.m_bPalette = ((this.m_nColorType & 1) > 0);
			if (this.m_bAlpha && this.m_nBitsPerComponent != 8 && this.m_nBitsPerComponent != 16)
			{
				AuxException.Throw(string.Format("Invalid Bits-per-component value ({0}) when alpha channel is in use. Should be 8 or 16.", this.m_nBitsPerComponent), PdfErrors._ERROR_PNG);
			}
			if ((this.m_nColorType & 2) > 0)
			{
				this.m_bstrColorSpace = "DeviceRGB";
				this.m_bGray = false;
				this.m_nComponentsPerSample = 3;
			}
			else
			{
				this.m_bstrColorSpace = "DeviceGray";
				this.m_bGray = true;
				this.m_nComponentsPerSample = 1;
			}
			this.m_bHeaderRead = true;
		}
		private void ProcessDataChunk(int dwLength)
		{
			if (!this.m_bHeaderRead)
			{
				AuxException.Throw("PNG header information missing or misplaced.", PdfErrors._ERROR_PNG);
			}
			byte[] array = new byte[dwLength];
			this.m_pInput.ReadBytes(this.m_dwPtr, array, dwLength);
			this.m_objRaw.Append(array);
			this.m_dwPtr += dwLength;
		}
		private void ProcessPaletteChunk(int dwLength)
		{
			if (!this.m_bHeaderRead)
			{
				AuxException.Throw("PNG header information missing or misplaced.", PdfErrors._ERROR_PNG);
			}
			if (dwLength % 3 != 0)
			{
				AuxException.Throw(string.Format("Invalid PNG palette size ({0}). Should be divisible by 3.", dwLength), PdfErrors._ERROR_PNG);
			}
			byte[] array = new byte[dwLength];
			this.m_pInput.ReadBytes(this.m_dwPtr, array, dwLength);
			for (int i = 0; i < dwLength / 3; i++)
			{
				this.m_arrPalette[i][0] = array[3 * i];
				this.m_arrPalette[i][1] = array[3 * i + 1];
				this.m_arrPalette[i][2] = array[3 * i + 2];
			}
			this.m_dwPtr += dwLength;
		}
		private void ProcessTransparencyChunk(int dwLength)
		{
			if (!this.m_bHeaderRead)
			{
				AuxException.Throw("PNG header information missing or misplaced.", PdfErrors._ERROR_PNG);
			}
			byte[] array = new byte[dwLength];
			this.m_pInput.ReadBytes(this.m_dwPtr, array, dwLength);
			if (this.m_bPalette)
			{
				for (int i = 0; i < 256; i++)
				{
					this.m_arrTranspPalette[i] = 255;
				}
				Array.Copy(array, this.m_arrTranspPalette, dwLength);
			}
			else
			{
				Array.Copy(array, this.m_arrTranspColor, dwLength);
			}
			this.m_bTransparency = true;
			this.m_dwPtr += dwLength;
		}
		private byte PaethPredictor(byte a, byte b, byte c)
		{
			int num = (int)(a + b - c);
			int num2 = Math.Abs(num - (int)a);
			int num3 = Math.Abs(num - (int)b);
			int num4 = Math.Abs(num - (int)c);
			if (num2 <= num3 && num2 <= num4)
			{
				return a;
			}
			if (num3 <= num4)
			{
				return b;
			}
			return c;
		}
		private void ApplyFilter(int nPtr, int j, int nBpp, int nScanLineSize)
		{
			int num = (int)this.m_pRaw[nPtr];
			if (num == 1)
			{
				for (int i = 1; i < nScanLineSize; i++)
				{
					this.m_pRaw[nPtr + i] = (byte)((int)(this.m_pRaw[nPtr + i] + ((i <= nBpp) ? 0 : this.m_pRaw[nPtr + i - nBpp])) % 256);
				}
			}
			if (num == 2)
			{
				for (int k = 1; k < nScanLineSize; k++)
				{
					this.m_pRaw[nPtr + k] = (byte)((int)(this.m_pRaw[nPtr + k] + ((j == 0) ? 0 : this.m_pRaw[nPtr + k - nScanLineSize])) % 256);
				}
			}
			if (num == 3)
			{
				for (int l = 1; l < nScanLineSize; l++)
				{
					this.m_pRaw[nPtr + l] = (byte)((int)(this.m_pRaw[nPtr + l] + (((l <= nBpp) ? 0 : this.m_pRaw[nPtr + l - nBpp]) + ((j == 0) ? 0 : this.m_pRaw[nPtr + l - nScanLineSize])) / 2) % 256);
				}
			}
			if (num == 4)
			{
				for (int m = 1; m < nScanLineSize; m++)
				{
                    this.m_pRaw[nPtr + m] = (byte)(((int)this.m_pRaw[nPtr + m] + (int)this.PaethPredictor(m <= nBpp ? (byte)0 : this.m_pRaw[nPtr + m - nBpp], j == 0 ? (byte)0 : this.m_pRaw[nPtr + m - nScanLineSize], m <= nBpp || j == 0 ? (byte)0 : this.m_pRaw[nPtr + m - nScanLineSize - nBpp])) % 256);					
				}
			}
		}
		static PdfPng()
		{
			// Note: this type is marked as 'beforefieldinit'.
			ShiftMap[] array = new ShiftMap[7];
			ShiftMap[] arg_18_0 = array;
			int arg_18_1 = 0;
			int arg_13_0 = 1;
			int arg_13_1 = 1;
			int[] points = new int[2];
			arg_18_0[arg_18_1] = new ShiftMap(arg_13_0, arg_13_1, points);
			ShiftMap[] arg_2E_0 = array;
			int arg_2E_1 = 1;
			int arg_29_0 = 1;
			int arg_29_1 = 1;
			int[] array2 = new int[2];
			array2[0] = 4;
			arg_2E_0[arg_2E_1] = new ShiftMap(arg_29_0, arg_29_1, array2);
			array[2] = new ShiftMap(2, 1, new int[]
			{
				0,
				4,
				4,
				4
			});
			array[3] = new ShiftMap(2, 2, new int[]
			{
				2,
				0,
				6,
				0,
				2,
				4,
				6,
				4
			});
			array[4] = new ShiftMap(4, 2, new int[]
			{
				0,
				2,
				2,
				2,
				4,
				2,
				6,
				2,
				0,
				6,
				2,
				6,
				4,
				6,
				6,
				6
			});
			array[5] = new ShiftMap(4, 4, new int[]
			{
				1,
				0,
				3,
				0,
				5,
				0,
				7,
				0,
				1,
				2,
				3,
				2,
				5,
				2,
				7,
				2,
				1,
				4,
				3,
				4,
				5,
				4,
				7,
				4,
				1,
				6,
				3,
				6,
				5,
				6,
				7,
				6
			});
			array[6] = new ShiftMap(8, 4, new int[]
			{
				0,
				1,
				1,
				1,
				2,
				1,
				3,
				1,
				4,
				1,
				5,
				1,
				6,
				1,
				7,
				1,
				0,
				3,
				1,
				3,
				2,
				3,
				3,
				3,
				4,
				3,
				5,
				3,
				6,
				3,
				7,
				3,
				0,
				5,
				1,
				5,
				2,
				5,
				3,
				5,
				4,
				5,
				5,
				5,
				6,
				5,
				7,
				5,
				0,
				7,
				1,
				7,
				2,
				7,
				3,
				7,
				4,
				7,
				5,
				7,
				6,
				7,
				7,
				7
			});
			PdfPng.m_arrPassShifts = array;
		}
	}
}
