using System;
namespace Persits.PDF
{
	internal class PdfBmp : PdfTiff
	{
		private byte[] m_pPalette;
		private int m_nPaletteSize;
		public PdfBmp(PdfManager pManager, PdfDocument pDoc) : base(pManager, pDoc)
		{
			this.m_nBitsPerComponent = 8;
		}
		public override void ParseImage()
		{
			int nSize = this.m_pInput.m_nSize;
			if (nSize < 52)
			{
				AuxException.Throw("The file to is too small to be a valid BMP image.", PdfErrors._ERROR_FILETOOSMALL);
			}
			int num = 0;
			byte[] pBuffer = new byte[2];
			num += this.m_pInput.ReadBytes(num, pBuffer, 2);
			uint num2;
			num += this.m_pInput.ReadUint(num, out num2);
			ushort num3;
			num += this.m_pInput.ReadUshort(num, out num3);
			ushort num4;
			num += this.m_pInput.ReadUshort(num, out num4);
			uint num5;
			num += this.m_pInput.ReadUint(num, out num5);
			uint num6;
			num += this.m_pInput.ReadUint(num, out num6);
			int num7;
			num += this.m_pInput.ReadInt(num, out num7);
			int num8;
			num += this.m_pInput.ReadInt(num, out num8);
			ushort num9;
			num += this.m_pInput.ReadUshort(num, out num9);
			ushort num10;
			num += this.m_pInput.ReadUshort(num, out num10);
			uint num11;
			num += this.m_pInput.ReadUint(num, out num11);
			uint num12;
			num += this.m_pInput.ReadUint(num, out num12);
			int num13;
			num += this.m_pInput.ReadInt(num, out num13);
			int num14;
			num += this.m_pInput.ReadInt(num, out num14);
			uint num15;
			num += this.m_pInput.ReadUint(num, out num15);
			uint num16;
			num += this.m_pInput.ReadUint(num, out num16);
			bool flag = false;
			if (num11 != 0u)
			{
				flag = true;
			}
			if ((ulong)num6 > (ulong)((long)nSize) || num6 != 40u)
			{
				AuxException.Throw("This does not appear to be a valid BMP file.", PdfErrors._ERROR_CURRUPTIMAGE);
			}
			long num17 = (long)((ulong)(num5 - 2u - 12u - 40u));
			if (num17 > 0L)
			{
				this.m_pPalette = new byte[num17];
				this.m_nPaletteSize = (int)num17 / 4;
				num += this.m_pInput.ReadBytes(num, this.m_pPalette, this.m_pPalette.Length);
			}
			int num18 = (int)num12;
			if (num18 == 0)
			{
				num18 = nSize;
			}
			if (num18 > this.m_pInput.m_nSize - num)
			{
				num18 = this.m_pInput.m_nSize - num;
			}
			byte[] array = new byte[num18 + 1];
			num += this.m_pInput.ReadBytes(num, array, num18);
			int paddedWidth = 4 * ((num7 * (int)num10 + 31) / 32);
			this.m_nWidth = num7;
			this.m_nHeight = num8;
			this.m_bstrColorSpace = ((num10 == 1) ? "DeviceGray" : "DeviceRGB");
			this.m_nComponentsPerSample = ((num10 == 1) ? 1 : 3);
			byte[] buffer = null;
			if (flag)
			{
				this.DecodeCompressedBitmap(ref buffer, array, (int)num10, num7, num8, paddedWidth);
			}
			else
			{
				this.DecodeBitmap(ref buffer, array, (int)num10, num7, num8, paddedWidth);
			}
			this.m_objData.Set(buffer);
		}
		private void DecodeCompressedBitmap(ref byte[] pDest, byte[] pSource, int BitsPerPix, int Width, int Height, int PaddedWidth)
		{
			pDest = new byte[Height * Width * 3];
			int num = 0;
			for (int i = 0; i < Height * Width; i++)
			{
				pDest[num++] = this.m_pPalette[2];
				pDest[num++] = this.m_pPalette[1];
				pDest[num++] = this.m_pPalette[0];
			}
			int num2 = 0;
			bool flag = false;
			int num3 = 0;
			int num4 = Height - 1;
			if (BitsPerPix == 8)
			{
				num3 = 3 * (Height - 1) * Width;
				while (num2 < pSource.Length && !flag)
				{
					if (pSource[num2] > 0)
					{
						this.CompressedBitmapHelper(pDest, ref num3, pSource[num2 + 1], (int)pSource[num2], num4, Width);
						num2 += 2;
					}
					else
					{
						switch (pSource[num2 + 1])
						{
						case 0:
							num4--;
							num3 = 3 * num4 * Width;
							num2 += 2;
							break;
						case 1:
							flag = true;
							break;
						case 2:
						{
							int num5 = (int)pSource[num2 + 2];
							int num6 = (int)pSource[num2 + 3];
							num3 -= 3 * (Width * num6);
							num3 += 3 * num5;
							num4 -= num6;
							num2 += 4;
							break;
						}
						default:
						{
							int j = (int)pSource[num2 + 1];
							num2 += 2;
							while (j > 0)
							{
								this.CompressedBitmapHelper(pDest, ref num3, pSource[num2++], 1, num4, Width);
								j--;
								if (j == 0)
								{
									num2++;
									break;
								}
								this.CompressedBitmapHelper(pDest, ref num3, pSource[num2++], 1, num4, Width);
								j--;
							}
							break;
						}
						}
					}
				}
			}
			if (BitsPerPix == 4)
			{
				num3 = 3 * (Height - 1) * Width;
				while (num2 < pSource.Length && !flag)
				{
					if (pSource[num2] > 0)
					{
						for (int k = 0; k < (int)pSource[num2]; k++)
						{
                            this.CompressedBitmapHelper(pDest, ref num3, k % 2 != 0 ? (byte)((uint)pSource[num2 + 1] & 15U) : (byte)((uint)pSource[num2 + 1] >> 4), 1, num4, Width);
						}
						num2 += 2;
					}
					else
					{
						switch (pSource[num2 + 1])
						{
						case 0:
							num4--;
							num3 = 3 * num4 * Width;
							num2 += 2;
							break;
						case 1:
							flag = true;
							break;
						case 2:
						{
							int num5 = (int)pSource[num2 + 2];
							int num6 = (int)pSource[num2 + 3];
							num3 -= 3 * (Width * num6);
							num3 += 3 * num5;
							num2 += 4;
							num4 -= num6;
							break;
						}
						default:
						{
							int j = (int)pSource[num2 + 1];
							num2 += 2;
							while (j > 0)
							{
								this.CompressedBitmapHelper(pDest, ref num3, (byte)(pSource[num2] >> 4), 1, num4, Width);
								j--;
								if (j == 0)
								{
									num2 += 2;
									break;
								}
                                this.CompressedBitmapHelper(pDest, ref num3, (byte)((uint)pSource[num2] & 15U), 1, num4, Width);
								j--;
								if (j == 0)
								{
									num2 += 2;
									break;
								}
								this.CompressedBitmapHelper(pDest, ref num3, (byte)(pSource[num2 + 1] >> 4), 1, num4, Width);
								j--;
								if (j == 0)
								{
									num2 += 2;
									break;
								}
                                this.CompressedBitmapHelper(pDest, ref num3, (byte)((uint)pSource[num2] & 15U), 1, num4, Width);
                                
								j--;
								if (j == 0)
								{
									num2 += 2;
									break;
								}
								num2 += 2;
							}
							break;
						}
						}
					}
				}
			}
		}
		private void CompressedBitmapHelper(byte[] pDest, ref int nDestPtr, byte ColorIndex, int nCount, int nCurLine, int Width)
		{
			if (nDestPtr + nCount * 3 > pDest.Length || nDestPtr + nCount * 3 > (nCurLine + 1) * Width * 3)
			{
				return;
			}
			if ((int)ColorIndex >= this.m_nPaletteSize)
			{
				return;
			}
			for (int i = 0; i < nCount; i++)
			{
				pDest[nDestPtr++] = this.m_pPalette[(int)(4 * ColorIndex + 2)];
				pDest[nDestPtr++] = this.m_pPalette[(int)(4 * ColorIndex + 1)];
				pDest[nDestPtr++] = this.m_pPalette[(int)(4 * ColorIndex)];
			}
		}
		private void DecodeBitmap(ref byte[] pDest, byte[] pSource, int BitsPerPix, int Width, int Height, int PaddedWidth)
		{
			int num = 0;
			if (BitsPerPix == 32)
			{
				pDest = new byte[Height * Width * 3];
				for (int i = Height - 1; i >= 0; i--)
				{
					for (int j = 0; j < Width; j++)
					{
						int num2 = i * PaddedWidth + j * 4;
						byte b = pSource[num2++];
						byte b2 = pSource[num2++];
						byte b3 = pSource[num2++];
						byte arg_49_0 = pSource[num2];
						pDest[num++] = b3;
						pDest[num++] = b2;
						pDest[num++] = b;
					}
				}
				return;
			}
			if (BitsPerPix == 24)
			{
				pDest = new byte[Height * Width * 3];
				for (int k = Height - 1; k >= 0; k--)
				{
					for (int l = 0; l < Width; l++)
					{
						int num2 = k * PaddedWidth + l * 3;
						byte b = pSource[num2++];
						byte b2 = pSource[num2++];
						byte b3 = pSource[num2];
						pDest[num++] = b3;
						pDest[num++] = b2;
						pDest[num++] = b;
					}
				}
				return;
			}
			if (BitsPerPix == 8)
			{
				pDest = new byte[Height * Width * 3];
				for (int m = Height - 1; m >= 0; m--)
				{
					for (int n = 0; n < Width; n++)
					{
						int num2 = m * PaddedWidth + n;
						byte b4 = pSource[num2++];
						pDest[num++] = this.m_pPalette[(int)(4 * b4 + 2)];
						pDest[num++] = this.m_pPalette[(int)(4 * b4 + 1)];
						pDest[num++] = this.m_pPalette[(int)(4 * b4)];
					}
				}
				return;
			}
			if (BitsPerPix == 4)
			{
				pDest = new byte[Height * Width * 3];
				int num3 = (Width % 2 == 0) ? (Width / 2) : (Width / 2 + 1);
				for (int num4 = Height - 1; num4 >= 0; num4--)
				{
					for (int num5 = 0; num5 < num3; num5++)
					{
						int num2 = num4 * PaddedWidth + num5;
						byte b5 = pSource[num2++];
						byte b6 = (byte)(b5 >> 4);
                        byte b7 = (byte)((uint)b5 & 15U);
						pDest[num++] = this.m_pPalette[(int)(4 * b6 + 2)];
						pDest[num++] = this.m_pPalette[(int)(4 * b6 + 1)];
						pDest[num++] = this.m_pPalette[(int)(4 * b6)];
						if (num5 != num3 - 1 || Width % 2 == 0)
						{
							pDest[num++] = this.m_pPalette[(int)(4 * b7 + 2)];
							pDest[num++] = this.m_pPalette[(int)(4 * b7 + 1)];
							pDest[num++] = this.m_pPalette[(int)(4 * b7)];
						}
					}
				}
				return;
			}
			if (BitsPerPix == 1)
			{
				int num6 = Width / 8;
				if (num6 == 0 || Width % 8 > 0)
				{
					num6++;
				}
				pDest = new byte[Height * num6];
				for (int num7 = Height - 1; num7 >= 0; num7--)
				{
					for (int num8 = 0; num8 < num6; num8++)
					{
						int num2 = num7 * PaddedWidth + num8;
						if (this.m_pPalette != null && this.m_pPalette[0] == 255)
						{
                            pDest[num++] = this.m_pPalette == null || (int)this.m_pPalette[0] != (int)byte.MaxValue ? pSource[num2] : (byte)((uint)pSource[num2] ^ (uint)byte.MaxValue);
						}
						else
						{
							pDest[num++] = pSource[num2];
						}
					}
				}
				this.m_nBitsPerComponent = 1;
				return;
			}
			AuxException.Throw(string.Format("BMP images with the bits-per-pixel value of {0} are not supported.", BitsPerPix), PdfErrors._ERROR_CURRUPTIMAGE);
		}
	}
}
