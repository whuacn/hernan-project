using System;
namespace Persits.PDF
{
	internal class PdfDCTStream : PdfDecoderHelper
	{
		private const int dctCos1 = 4017;
		private const int dctSin1 = 799;
		private const int dctCos3 = 3406;
		private const int dctSin3 = 2276;
		private const int dctCos6 = 1567;
		private const int dctSin6 = 3784;
		private const int dctSqrt2 = 5793;
		private const int dctSqrt1d2 = 2896;
		private const int dctCrToR = 91881;
		private const int dctCbToG = -22553;
		private const int dctCrToG = -46802;
		private const int dctCbToB = 116130;
		private PdfStream str;
		private int width;
		private int height;
		private int numComps;
		private bool progressive;
		private bool interleaved;
		private int mcuWidth;
		private int mcuHeight;
		private int bufWidth;
		private int bufHeight;
		private DCTCompInfo[] compInfo = new DCTCompInfo[4];
		private DCTScanInfo scanInfo = new DCTScanInfo();
		private int colorXform;
		private bool gotJFIFMarker;
		private int restartInterval;
		private byte[][] quantTables;
		private int numQuantTables;
		private DCTHuffTable[] dcHuffTables = new DCTHuffTable[4];
		private DCTHuffTable[] acHuffTables = new DCTHuffTable[4];
		private int numDCHuffTables;
		private int numACHuffTables;
		private byte[][][] rowBuf;
		private int[][] frameBuf = new int[4][];
		private int comp;
		private int x;
		private int y;
		private int dy;
		private int restartCtr;
		private int restartMarker;
		private int eobRun;
		private int inputBuf;
		private int inputBits;
		private static int dctClipOffset = 256;
		private static byte[] dctClip = new byte[768];
		private static int dctClipInit = 0;
		private static int[] dctZigZag = new int[]
		{
			0,
			1,
			8,
			16,
			9,
			2,
			3,
			10,
			17,
			24,
			32,
			25,
			18,
			11,
			4,
			5,
			12,
			19,
			26,
			33,
			40,
			48,
			41,
			34,
			27,
			20,
			13,
			6,
			7,
			14,
			21,
			28,
			35,
			42,
			49,
			56,
			57,
			50,
			43,
			36,
			29,
			22,
			15,
			23,
			30,
			37,
			44,
			51,
			58,
			59,
			52,
			45,
			38,
			31,
			39,
			46,
			53,
			60,
			61,
			54,
			47,
			55,
			62,
			63
		};
		internal PdfDCTStream(PdfStream strA)
		{
			this.str = strA;
			this.colorXform = -1;
			this.progressive = (this.interleaved = false);
			this.width = (this.height = 0);
			this.mcuWidth = (this.mcuHeight = 0);
			this.numComps = 0;
			this.comp = 0;
			this.x = (this.y = (this.dy = 0));
			this.rowBuf = new byte[4][][];
			for (int i = 0; i < 4; i++)
			{
				this.rowBuf[i] = new byte[32][];
			}
			this.frameBuf = new int[4][];
			for (int j = 0; j < 4; j++)
			{
				for (int k = 0; k < 32; k++)
				{
					this.rowBuf[j][k] = null;
				}
				this.frameBuf[j] = null;
			}
			if (PdfDCTStream.dctClipInit == 0)
			{
				for (int j = -256; j < 0; j++)
				{
					PdfDCTStream.dctClip[PdfDCTStream.dctClipOffset + j] = 0;
				}
				for (int j = 0; j < 256; j++)
				{
					PdfDCTStream.dctClip[PdfDCTStream.dctClipOffset + j] = (byte)j;
				}
				for (int j = 256; j < 512; j++)
				{
					PdfDCTStream.dctClip[PdfDCTStream.dctClipOffset + j] = 255;
				}
				PdfDCTStream.dctClipInit = 1;
			}
			this.quantTables = new byte[4][];
			for (int l = 0; l < 4; l++)
			{
				this.quantTables[l] = new byte[64];
			}
			for (int m = 0; m < 4; m++)
			{
				this.compInfo[m] = new DCTCompInfo();
				this.dcHuffTables[m] = new DCTHuffTable();
				this.acHuffTables[m] = new DCTHuffTable();
			}
		}
		internal override void reset()
		{
			this.str.reset();
			this.progressive = (this.interleaved = false);
			this.width = (this.height = 0);
			this.numComps = 0;
			this.numQuantTables = 0;
			this.numDCHuffTables = 0;
			this.numACHuffTables = 0;
			this.gotJFIFMarker = false;
			this.restartInterval = 0;
			if (!this.readHeader())
			{
				this.y = this.height;
				return;
			}
			if (this.numComps == 1)
			{
				this.compInfo[0].hSample = (this.compInfo[0].vSample = 1);
			}
			this.mcuWidth = this.compInfo[0].hSample;
			this.mcuHeight = this.compInfo[0].vSample;
			for (int i = 1; i < this.numComps; i++)
			{
				if (this.compInfo[i].hSample > this.mcuWidth)
				{
					this.mcuWidth = this.compInfo[i].hSample;
				}
				if (this.compInfo[i].vSample > this.mcuHeight)
				{
					this.mcuHeight = this.compInfo[i].vSample;
				}
			}
			this.mcuWidth *= 8;
			this.mcuHeight *= 8;
			if (this.colorXform == -1)
			{
				if (this.numComps == 3)
				{
					if (this.gotJFIFMarker)
					{
						this.colorXform = 1;
					}
					else
					{
						if (this.compInfo[0].id == 82 && this.compInfo[1].id == 71 && this.compInfo[2].id == 66)
						{
							this.colorXform = 0;
						}
						else
						{
							this.colorXform = 1;
						}
					}
				}
				else
				{
					this.colorXform = 0;
				}
			}
			if (!this.progressive && this.interleaved)
			{
				this.bufWidth = (this.width + this.mcuWidth - 1) / this.mcuWidth * this.mcuWidth;
				for (int i = 0; i < this.numComps; i++)
				{
					for (int j = 0; j < this.mcuHeight; j++)
					{
						this.rowBuf[i][j] = new byte[this.bufWidth];
					}
				}
				this.comp = 0;
				this.x = 0;
				this.y = 0;
				this.dy = this.mcuHeight;
				this.restartMarker = 208;
				this.restart();
				return;
			}
			this.bufWidth = (this.width + this.mcuWidth - 1) / this.mcuWidth * this.mcuWidth;
			this.bufHeight = (this.height + this.mcuHeight - 1) / this.mcuHeight * this.mcuHeight;
			if (this.bufWidth <= 0 || this.bufHeight <= 0 || this.bufWidth > 2147483647 / this.bufWidth / 4)
			{
				AuxException.Throw("Invalid image size in DCT stream");
				this.y = this.height;
				return;
			}
			for (int i = 0; i < this.numComps; i++)
			{
				this.frameBuf[i] = new int[this.bufWidth * this.bufHeight];
				RavenTypes.MemSet(this.frameBuf[i], 0, 0, this.bufWidth * this.bufHeight);
			}
			do
			{
				this.restartMarker = 208;
				this.restart();
				this.readScan();
			}
			while (this.readHeader());
			this.decodeImage();
			this.comp = 0;
			this.x = 0;
			this.y = 0;
		}
		internal override int getChar()
		{
			if (this.y >= this.height)
			{
				return -1;
			}
			int result;
			if (this.progressive || !this.interleaved)
			{
				result = this.frameBuf[this.comp][this.y * this.bufWidth + this.x];
				if (++this.comp == this.numComps)
				{
					this.comp = 0;
					if (++this.x == this.width)
					{
						this.x = 0;
						this.y++;
					}
				}
			}
			else
			{
				if (this.dy >= this.mcuHeight)
				{
					if (!this.readMCURow())
					{
						this.y = this.height;
						return -1;
					}
					this.comp = 0;
					this.x = 0;
					this.dy = 0;
				}
				result = (int)this.rowBuf[this.comp][this.dy][this.x];
				if (++this.comp == this.numComps)
				{
					this.comp = 0;
					if (++this.x == this.width)
					{
						this.x = 0;
						this.y++;
						this.dy++;
						if (this.y == this.height)
						{
							this.readTrailer();
						}
					}
				}
			}
			return result;
		}
		internal int lookChar()
		{
			if (this.y >= this.height)
			{
				return -1;
			}
			if (this.progressive || !this.interleaved)
			{
				return this.frameBuf[this.comp][this.y * this.bufWidth + this.x];
			}
			if (this.dy >= this.mcuHeight)
			{
				if (!this.readMCURow())
				{
					this.y = this.height;
					return -1;
				}
				this.comp = 0;
				this.x = 0;
				this.dy = 0;
			}
			return (int)this.rowBuf[this.comp][this.dy][this.x];
		}
		private void restart()
		{
			this.inputBits = 0;
			this.restartCtr = this.restartInterval;
			for (int i = 0; i < this.numComps; i++)
			{
				this.compInfo[i].prevDC = 0;
			}
			this.eobRun = 0;
		}
		private bool readMCURow()
		{
			int[] array = new int[64];
			byte[] array2 = new byte[64];
			for (int i = 0; i < this.width; i += this.mcuWidth)
			{
				if (this.restartInterval > 0 && this.restartCtr == 0)
				{
					int num = this.readMarker();
					if (num != this.restartMarker)
					{
						AuxException.Throw("Invalid DCT data: incorrect restart marker.");
						return false;
					}
					if (++this.restartMarker == 216)
					{
						this.restartMarker = 208;
					}
					this.restart();
				}
				for (int j = 0; j < this.numComps; j++)
				{
					int hSample = this.compInfo[j].hSample;
					int vSample = this.compInfo[j].vSample;
					int num2 = this.mcuWidth / hSample;
					int num3 = this.mcuHeight / vSample;
					int num4 = num2 / 8;
					int num5 = num3 / 8;
					for (int k = 0; k < this.mcuHeight; k += num3)
					{
						for (int l = 0; l < this.mcuWidth; l += num2)
						{
							if (!this.readDataUnit(this.dcHuffTables[this.scanInfo.dcHuffTable[j]], this.acHuffTables[this.scanInfo.acHuffTable[j]], ref this.compInfo[j].prevDC, array))
							{
								return false;
							}
							this.transformDataUnit(new RavenColorPtr(this.quantTables[this.compInfo[j].quantTable]), array, array2);
							if (num4 == 1 && num5 == 1)
							{
								int m = 0;
								int num6 = 0;
								while (m < 8)
								{
									RavenColorPtr ravenColorPtr = new RavenColorPtr(this.rowBuf[j][k + m], i + l);
									ravenColorPtr[0] = array2[num6];
									ravenColorPtr[1] = array2[num6 + 1];
									ravenColorPtr[2] = array2[num6 + 2];
									ravenColorPtr[3] = array2[num6 + 3];
									ravenColorPtr[4] = array2[num6 + 4];
									ravenColorPtr[5] = array2[num6 + 5];
									ravenColorPtr[6] = array2[num6 + 6];
									ravenColorPtr[7] = array2[num6 + 7];
									m++;
									num6 += 8;
								}
							}
							else
							{
								if (num4 == 2 && num5 == 2)
								{
									int m = 0;
									int num6 = 0;
									while (m < 16)
									{
										RavenColorPtr ravenColorPtr = new RavenColorPtr(this.rowBuf[j][k + m], i + l);
										RavenColorPtr ravenColorPtr2 = new RavenColorPtr(this.rowBuf[j][k + m + 1], i + l);
										ravenColorPtr[0] = (ravenColorPtr[1] = (ravenColorPtr2[0] = (ravenColorPtr2[1] = array2[num6])));
										ravenColorPtr[2] = (ravenColorPtr[3] = (ravenColorPtr2[2] = (ravenColorPtr2[3] = array2[num6 + 1])));
										ravenColorPtr[4] = (ravenColorPtr[5] = (ravenColorPtr2[4] = (ravenColorPtr2[5] = array2[num6 + 2])));
										ravenColorPtr[6] = (ravenColorPtr[7] = (ravenColorPtr2[6] = (ravenColorPtr2[7] = array2[num6 + 3])));
										ravenColorPtr[8] = (ravenColorPtr[9] = (ravenColorPtr2[8] = (ravenColorPtr2[9] = array2[num6 + 4])));
										ravenColorPtr[10] = (ravenColorPtr[11] = (ravenColorPtr2[10] = (ravenColorPtr2[11] = array2[num6 + 5])));
										ravenColorPtr[12] = (ravenColorPtr[13] = (ravenColorPtr2[12] = (ravenColorPtr2[13] = array2[num6 + 6])));
										ravenColorPtr[14] = (ravenColorPtr[15] = (ravenColorPtr2[14] = (ravenColorPtr2[15] = array2[num6 + 7])));
										m += 2;
										num6 += 8;
									}
								}
								else
								{
									int num6 = 0;
									int m = 0;
									int num7 = 0;
									while (m < 8)
									{
										int n = 0;
										int num8 = 0;
										while (n < 8)
										{
											for (int num9 = 0; num9 < num5; num9++)
											{
												for (int num10 = 0; num10 < num4; num10++)
												{
													this.rowBuf[j][k + num7 + num9][i + l + num8 + num10] = array2[num6];
												}
											}
											num6++;
											n++;
											num8 += num4;
										}
										m++;
										num7 += num5;
									}
								}
							}
						}
					}
				}
				this.restartCtr--;
				if (this.colorXform != 0)
				{
					if (this.numComps == 3)
					{
						for (int k = 0; k < this.mcuHeight; k++)
						{
							for (int l = 0; l < this.mcuWidth; l++)
							{
								int num11 = (int)this.rowBuf[0][k][i + l];
								int num12 = (int)(this.rowBuf[1][k][i + l] - 128);
								int num13 = (int)(this.rowBuf[2][k][i + l] - 128);
								int num14 = (num11 << 16) + 91881 * num13 + 32768 >> 16;
								this.rowBuf[0][k][i + l] = PdfDCTStream.dctClip[PdfDCTStream.dctClipOffset + num14];
								int num15 = (num11 << 16) + -22553 * num12 + -46802 * num13 + 32768 >> 16;
								this.rowBuf[1][k][i + l] = PdfDCTStream.dctClip[PdfDCTStream.dctClipOffset + num15];
								int num16 = (num11 << 16) + 116130 * num12 + 32768 >> 16;
								this.rowBuf[2][k][i + l] = PdfDCTStream.dctClip[PdfDCTStream.dctClipOffset + num16];
							}
						}
					}
					else
					{
						if (this.numComps == 4)
						{
							for (int k = 0; k < this.mcuHeight; k++)
							{
								for (int l = 0; l < this.mcuWidth; l++)
								{
									int num11 = (int)this.rowBuf[0][k][i + l];
									int num12 = (int)(this.rowBuf[1][k][i + l] - 128);
									int num13 = (int)(this.rowBuf[2][k][i + l] - 128);
									int num14 = (num11 << 16) + 91881 * num13 + 32768 >> 16;
									this.rowBuf[0][k][i + l] = (byte) ((uint) byte.MaxValue - (uint) PdfDCTStream.dctClip[PdfDCTStream.dctClipOffset + num14]);
									int num15 = (num11 << 16) + -22553 * num12 + -46802 * num13 + 32768 >> 16;
									this.rowBuf[1][k][i + l] = (byte) ((uint) byte.MaxValue - (uint) PdfDCTStream.dctClip[PdfDCTStream.dctClipOffset +  num15]);
									int num16 = (num11 << 16) + 116130 * num12 + 32768 >> 16;
									this.rowBuf[2][k][i + l] = (byte) ((uint) byte.MaxValue - (uint) PdfDCTStream.dctClip[PdfDCTStream.dctClipOffset +  num16]);
								}
							}
						}
					}
				}
			}
			return true;
		}
		private void readScan()
		{
			int[] array = new int[64];
			int num;
			int num2;
			if (this.scanInfo.numComps == 1)
			{
				int i = 0;
				while (i < this.numComps && !this.scanInfo.comp[i])
				{
					i++;
				}
				num = this.mcuWidth / this.compInfo[i].hSample;
				num2 = this.mcuHeight / this.compInfo[i].vSample;
			}
			else
			{
				num = this.mcuWidth;
				num2 = this.mcuHeight;
			}
			for (int j = 0; j < this.height; j += num2)
			{
				for (int k = 0; k < this.width; k += num)
				{
					if (this.restartInterval > 0 && this.restartCtr == 0)
					{
						int num3 = this.readMarker();
						if (num3 != this.restartMarker)
						{
							AuxException.Throw("Invalid DCT data: incorrect restart marker.");
							return;
						}
						if (++this.restartMarker == 216)
						{
							this.restartMarker = 208;
						}
						this.restart();
					}
					for (int i = 0; i < this.numComps; i++)
					{
						if (this.scanInfo.comp[i])
						{
							int hSample = this.compInfo[i].hSample;
							int vSample = this.compInfo[i].vSample;
							int num4 = this.mcuWidth / hSample;
							int num5 = this.mcuHeight / vSample;
							int num6 = num5 / 8;
							for (int l = 0; l < num2; l += num5)
							{
								for (int m = 0; m < num; m += num4)
								{
									RavenPtr<int> ravenPtr = new RavenPtr<int>(this.frameBuf[i], (j + l) * this.bufWidth + (k + m));
									int n = 0;
									int num7 = 0;
									while (n < 8)
									{
										array[num7] = ravenPtr[0];
										array[num7 + 1] = ravenPtr[1];
										array[num7 + 2] = ravenPtr[2];
										array[num7 + 3] = ravenPtr[3];
										array[num7 + 4] = ravenPtr[4];
										array[num7 + 5] = ravenPtr[5];
										array[num7 + 6] = ravenPtr[6];
										array[num7 + 7] = ravenPtr[7];
										ravenPtr.Inc(this.bufWidth * num6);
										n++;
										num7 += 8;
									}
									if (this.progressive)
									{
										if (!this.readProgressiveDataUnit(this.dcHuffTables[this.scanInfo.dcHuffTable[i]], this.acHuffTables[this.scanInfo.acHuffTable[i]], ref this.compInfo[i].prevDC, array))
										{
											return;
										}
									}
									else
									{
										if (!this.readDataUnit(this.dcHuffTables[this.scanInfo.dcHuffTable[i]], this.acHuffTables[this.scanInfo.acHuffTable[i]], ref this.compInfo[i].prevDC, array))
										{
											return;
										}
									}
									ravenPtr = new RavenPtr<int>(this.frameBuf[i], (j + l) * this.bufWidth + (k + m));
									n = 0;
									num7 = 0;
									while (n < 8)
									{
										ravenPtr[0] = array[num7];
										ravenPtr[1] = array[num7 + 1];
										ravenPtr[2] = array[num7 + 2];
										ravenPtr[3] = array[num7 + 3];
										ravenPtr[4] = array[num7 + 4];
										ravenPtr[5] = array[num7 + 5];
										ravenPtr[6] = array[num7 + 6];
										ravenPtr[7] = array[num7 + 7];
										ravenPtr.Inc(this.bufWidth * num6);
										n++;
										num7 += 8;
									}
								}
							}
						}
					}
					this.restartCtr--;
				}
			}
		}
		private bool readDataUnit(DCTHuffTable dcHuffTable, DCTHuffTable acHuffTable, ref int prevDC, int[] data)
		{
			int num;
			if ((num = this.readHuffSym(dcHuffTable)) == 9999)
			{
				return false;
			}
			int num2;
			if (num > 0)
			{
				if ((num2 = this.readAmp(num)) == 9999)
				{
					return false;
				}
			}
			else
			{
				num2 = 0;
			}
			data[0] = (prevDC += num2);
			int i;
			for (i = 1; i < 64; i++)
			{
				data[i] = 0;
			}
			i = 1;
			while (i < 64)
			{
				int num3 = 0;
				int num4;
				while ((num4 = this.readHuffSym(acHuffTable)) == 240 && num3 < 48)
				{
					num3 += 16;
				}
				if (num4 == 9999)
				{
					return false;
				}
				if (num4 == 0)
				{
					break;
				}
				num3 += (num4 >> 4 & 15);
				num = (num4 & 15);
				num2 = this.readAmp(num);
				if (num2 == 9999)
				{
					return false;
				}
				i += num3;
				if (i < 64)
				{
					int num5 = PdfDCTStream.dctZigZag[i++];
					data[num5] = num2;
				}
			}
			return true;
		}
		private bool readProgressiveDataUnit(DCTHuffTable dcHuffTable, DCTHuffTable acHuffTable, ref int prevDC, int[] data)
		{
			int i = this.scanInfo.firstCoeff;
			if (i == 0)
			{
				if (this.scanInfo.ah == 0)
				{
					int num;
					if ((num = this.readHuffSym(dcHuffTable)) == 9999)
					{
						return false;
					}
					int num2;
					if (num > 0)
					{
						if ((num2 = this.readAmp(num)) == 9999)
						{
							return false;
						}
					}
					else
					{
						num2 = 0;
					}
					data[0] += (prevDC += num2) << this.scanInfo.al;
				}
				else
				{
					int num3;
					if ((num3 = this.readBit()) == 9999)
					{
						return false;
					}
					data[0] += num3 << this.scanInfo.al;
				}
				i++;
			}
			if (this.scanInfo.lastCoeff == 0)
			{
				return true;
			}
			if (this.eobRun > 0)
			{
				while (i <= this.scanInfo.lastCoeff)
				{
					int num4 = PdfDCTStream.dctZigZag[i++];
					if (data[num4] != 0)
					{
						int num3;
						if ((num3 = this.readBit()) == -1)
						{
							return false;
						}
						if (num3 != 0)
						{
							data[num4] += 1 << this.scanInfo.al;
						}
					}
				}
				this.eobRun--;
				return true;
			}
			while (i <= this.scanInfo.lastCoeff)
			{
				int num5;
				if ((num5 = this.readHuffSym(acHuffTable)) == 9999)
				{
					return false;
				}
				if (num5 == 240)
				{
					int j = 0;
					while (j < 16)
					{
						int num4 = PdfDCTStream.dctZigZag[i++];
						if (data[num4] == 0)
						{
							j++;
						}
						else
						{
							int num3;
							if ((num3 = this.readBit()) == -1)
							{
								return false;
							}
							if (num3 != 0)
							{
								data[num4] += 1 << this.scanInfo.al;
							}
						}
					}
				}
				else
				{
					int num4;
					int j;
					if ((num5 & 15) == 0)
					{
						num4 = num5 >> 4;
						this.eobRun = 0;
						for (j = 0; j < num4; j++)
						{
							int num3;
							if ((num3 = this.readBit()) == -1)
							{
								return false;
							}
							this.eobRun = (this.eobRun << 1 | num3);
						}
						this.eobRun += 1 << num4;
						while (i <= this.scanInfo.lastCoeff)
						{
							num4 = PdfDCTStream.dctZigZag[i++];
							if (data[num4] != 0)
							{
								int num3;
								if ((num3 = this.readBit()) == -1)
								{
									return false;
								}
								if (num3 != 0)
								{
									data[num4] += 1 << this.scanInfo.al;
								}
							}
						}
						this.eobRun--;
						break;
					}
					int num6 = num5 >> 4 & 15;
					int num = num5 & 15;
					int num2;
					if ((num2 = this.readAmp(num)) == 9999)
					{
						return false;
					}
					j = 0;
					do
					{
						num4 = PdfDCTStream.dctZigZag[i++];
						while (data[num4] != 0)
						{
							int num3;
							if ((num3 = this.readBit()) == -1)
							{
								return false;
							}
							if (num3 != 0)
							{
								data[num4] += 1 << this.scanInfo.al;
							}
							num4 = PdfDCTStream.dctZigZag[i++];
						}
						j++;
					}
					while (j <= num6);
					data[num4] = num2 << this.scanInfo.al;
					continue;
				}
			}
			return true;
		}
		private void decodeImage()
		{
			int[] array = new int[64];
			byte[] array2 = new byte[64];
			for (int i = 0; i < this.bufHeight; i += this.mcuHeight)
			{
				for (int j = 0; j < this.bufWidth; j += this.mcuWidth)
				{
					for (int k = 0; k < this.numComps; k++)
					{
						RavenColorPtr quantTable = new RavenColorPtr(this.quantTables[this.compInfo[k].quantTable]);
						int hSample = this.compInfo[k].hSample;
						int vSample = this.compInfo[k].vSample;
						int num = this.mcuWidth / hSample;
						int num2 = this.mcuHeight / vSample;
						int num3 = num / 8;
						int num4 = num2 / 8;
						for (int l = 0; l < this.mcuHeight; l += num2)
						{
							for (int m = 0; m < this.mcuWidth; m += num)
							{
								RavenPtr<int> ravenPtr = new RavenPtr<int>(this.frameBuf[k], (i + l) * this.bufWidth + (j + m));
								int n = 0;
								int num5 = 0;
								while (n < 8)
								{
									array[num5] = ravenPtr[0];
									array[num5 + 1] = ravenPtr[1];
									array[num5 + 2] = ravenPtr[2];
									array[num5 + 3] = ravenPtr[3];
									array[num5 + 4] = ravenPtr[4];
									array[num5 + 5] = ravenPtr[5];
									array[num5 + 6] = ravenPtr[6];
									array[num5 + 7] = ravenPtr[7];
									ravenPtr.Inc(this.bufWidth * num4);
									n++;
									num5 += 8;
								}
								this.transformDataUnit(quantTable, array, array2);
								ravenPtr = new RavenPtr<int>(this.frameBuf[k], (i + l) * this.bufWidth + (j + m));
								if (num3 == 1 && num4 == 1)
								{
									n = 0;
									num5 = 0;
									while (n < 8)
									{
										ravenPtr[0] = (int)(array2[num5] & 255);
										ravenPtr[1] = (int)(array2[num5 + 1] & 255);
										ravenPtr[2] = (int)(array2[num5 + 2] & 255);
										ravenPtr[3] = (int)(array2[num5 + 3] & 255);
										ravenPtr[4] = (int)(array2[num5 + 4] & 255);
										ravenPtr[5] = (int)(array2[num5 + 5] & 255);
										ravenPtr[6] = (int)(array2[num5 + 6] & 255);
										ravenPtr[7] = (int)(array2[num5 + 7] & 255);
										ravenPtr.Inc(this.bufWidth);
										n++;
										num5 += 8;
									}
								}
								else
								{
									if (num3 == 2 && num4 == 2)
									{
										RavenPtr<int> ravenPtr2 = new RavenPtr<int>(ravenPtr, this.bufWidth);
										n = 0;
										num5 = 0;
										while (n < 16)
										{
											ravenPtr[0] = (ravenPtr[1] = (ravenPtr2[0] = (ravenPtr2[1] = (int)(array2[num5] & 255))));
											ravenPtr[2] = (ravenPtr[3] = (ravenPtr2[2] = (ravenPtr2[3] = (int)(array2[num5 + 1] & 255))));
											ravenPtr[4] = (ravenPtr[5] = (ravenPtr2[4] = (ravenPtr2[5] = (int)(array2[num5 + 2] & 255))));
											ravenPtr[6] = (ravenPtr[7] = (ravenPtr2[6] = (ravenPtr2[7] = (int)(array2[num5 + 3] & 255))));
											ravenPtr[8] = (ravenPtr[9] = (ravenPtr2[8] = (ravenPtr2[9] = (int)(array2[num5 + 4] & 255))));
											ravenPtr[10] = (ravenPtr[11] = (ravenPtr2[10] = (ravenPtr2[11] = (int)(array2[num5 + 5] & 255))));
											ravenPtr[12] = (ravenPtr[13] = (ravenPtr2[12] = (ravenPtr2[13] = (int)(array2[num5 + 6] & 255))));
											ravenPtr[14] = (ravenPtr[15] = (ravenPtr2[14] = (ravenPtr2[15] = (int)(array2[num5 + 7] & 255))));
											ravenPtr.Inc(this.bufWidth * 2);
											ravenPtr2.Inc(this.bufWidth * 2);
											n += 2;
											num5 += 8;
										}
									}
									else
									{
										num5 = 0;
										n = 0;
										int num6 = 0;
										while (n < 8)
										{
											int num7 = 0;
											int num8 = 0;
											while (num7 < 8)
											{
												RavenPtr<int> ravenPtr2 = new RavenPtr<int>(ravenPtr, num8);
												for (int num9 = 0; num9 < num4; num9++)
												{
													for (int num10 = 0; num10 < num3; num10++)
													{
														ravenPtr2[num10] = (int)(array2[num5] & 255);
													}
													ravenPtr2.Inc(this.bufWidth);
												}
												num5++;
												num7++;
												num8 += num3;
											}
											ravenPtr.Inc(this.bufWidth * num4);
											n++;
											num6 += num4;
										}
									}
								}
							}
						}
					}
					if (this.colorXform != 0)
					{
						if (this.numComps == 3)
						{
							for (int l = 0; l < this.mcuHeight; l++)
							{
								RavenPtr<int> ravenPtr3 = new RavenPtr<int>(this.frameBuf[0], (i + l) * this.bufWidth + j);
								RavenPtr<int> ravenPtr = new RavenPtr<int>(this.frameBuf[1], (i + l) * this.bufWidth + j);
								RavenPtr<int> ravenPtr2 = new RavenPtr<int>(this.frameBuf[2], (i + l) * this.bufWidth + j);
								for (int m = 0; m < this.mcuWidth; m++)
								{
									int ptr = ravenPtr3.ptr;
									int num11 = ravenPtr.ptr - 128;
									int num12 = ravenPtr2.ptr - 128;
									int num13 = (ptr << 16) + 91881 * num12 + 32768 >> 16;
									ravenPtr3.ptr = (int)PdfDCTStream.dctClip[PdfDCTStream.dctClipOffset + num13];
									ravenPtr3 = ++ravenPtr3;
									int num14 = (ptr << 16) + -22553 * num11 + -46802 * num12 + 32768 >> 16;
									ravenPtr.ptr = (int)PdfDCTStream.dctClip[PdfDCTStream.dctClipOffset + num14];
									ravenPtr = ++ravenPtr;
									int num15 = (ptr << 16) + 116130 * num11 + 32768 >> 16;
									ravenPtr2.ptr = (int)PdfDCTStream.dctClip[PdfDCTStream.dctClipOffset + num15];
									ravenPtr2 = ++ravenPtr2;
								}
							}
						}
						else
						{
							if (this.numComps == 4)
							{
								for (int l = 0; l < this.mcuHeight; l++)
								{
									RavenPtr<int> ravenPtr3 = new RavenPtr<int>(this.frameBuf[0], (i + l) * this.bufWidth + j);
									RavenPtr<int> ravenPtr = new RavenPtr<int>(this.frameBuf[1], (i + l) * this.bufWidth + j);
									RavenPtr<int> ravenPtr2 = new RavenPtr<int>(this.frameBuf[2], (i + l) * this.bufWidth + j);
									for (int m = 0; m < this.mcuWidth; m++)
									{
										int ptr = ravenPtr3.ptr;
										int num11 = ravenPtr.ptr - 128;
										int num12 = ravenPtr2.ptr - 128;
										int num13 = (ptr << 16) + 91881 * num12 + 32768 >> 16;
										ravenPtr3.ptr = (int)(255 - PdfDCTStream.dctClip[PdfDCTStream.dctClipOffset + num13]);
										ravenPtr3 = ++ravenPtr3;
										int num14 = (ptr << 16) + -22553 * num11 + -46802 * num12 + 32768 >> 16;
										ravenPtr.ptr = (int)(255 - PdfDCTStream.dctClip[PdfDCTStream.dctClipOffset + num14]);
										ravenPtr = ++ravenPtr;
										int num15 = (ptr << 16) + 116130 * num11 + 32768 >> 16;
										ravenPtr2.ptr = (int)(255 - PdfDCTStream.dctClip[PdfDCTStream.dctClipOffset + num15]);
										ravenPtr2 = ++ravenPtr2;
									}
								}
							}
						}
					}
				}
			}
		}
		private void transformDataUnit(RavenColorPtr quantTable, int[] dataIn, byte[] dataOut)
		{
			for (int i = 0; i < 64; i++)
			{
				dataIn[i] *= (int)quantTable[i];
			}
			for (int i = 0; i < 64; i += 8)
			{
				RavenPtr<int> ravenPtr = new RavenPtr<int>(dataIn, i);
				if (ravenPtr[1] == 0 && ravenPtr[2] == 0 && ravenPtr[3] == 0 && ravenPtr[4] == 0 && ravenPtr[5] == 0 && ravenPtr[6] == 0 && ravenPtr[7] == 0)
				{
					int num = 5793 * ravenPtr[0] + 512 >> 10;
					ravenPtr[0] = num;
					ravenPtr[1] = num;
					ravenPtr[2] = num;
					ravenPtr[3] = num;
					ravenPtr[4] = num;
					ravenPtr[5] = num;
					ravenPtr[6] = num;
					ravenPtr[7] = num;
				}
				else
				{
					int num2 = 5793 * ravenPtr[0] + 128 >> 8;
					int num3 = 5793 * ravenPtr[4] + 128 >> 8;
					int num4 = ravenPtr[2];
					int num5 = ravenPtr[6];
					int num6 = 2896 * (ravenPtr[1] - ravenPtr[7]) + 128 >> 8;
					int num7 = 2896 * (ravenPtr[1] + ravenPtr[7]) + 128 >> 8;
					int num8 = ravenPtr[3] << 4;
					int num9 = ravenPtr[5] << 4;
					int num = num2 - num3 + 1 >> 1;
					num2 = num2 + num3 + 1 >> 1;
					num3 = num;
					num = num4 * 3784 + num5 * 1567 + 128 >> 8;
					num4 = num4 * 1567 - num5 * 3784 + 128 >> 8;
					num5 = num;
					num = num6 - num9 + 1 >> 1;
					num6 = num6 + num9 + 1 >> 1;
					num9 = num;
					num = num7 + num8 + 1 >> 1;
					num8 = num7 - num8 + 1 >> 1;
					num7 = num;
					num = num2 - num5 + 1 >> 1;
					num2 = num2 + num5 + 1 >> 1;
					num5 = num;
					num = num3 - num4 + 1 >> 1;
					num3 = num3 + num4 + 1 >> 1;
					num4 = num;
					num = num6 * 2276 + num7 * 3406 + 2048 >> 12;
					num6 = num6 * 3406 - num7 * 2276 + 2048 >> 12;
					num7 = num;
					num = num8 * 799 + num9 * 4017 + 2048 >> 12;
					num8 = num8 * 4017 - num9 * 799 + 2048 >> 12;
					num9 = num;
					ravenPtr[0] = num2 + num7;
					ravenPtr[7] = num2 - num7;
					ravenPtr[1] = num3 + num9;
					ravenPtr[6] = num3 - num9;
					ravenPtr[2] = num4 + num8;
					ravenPtr[5] = num4 - num8;
					ravenPtr[3] = num5 + num6;
					ravenPtr[4] = num5 - num6;
				}
			}
			for (int i = 0; i < 8; i++)
			{
				RavenPtr<int> ravenPtr = new RavenPtr<int>(dataIn, i);
				if (ravenPtr[8] == 0 && ravenPtr[16] == 0 && ravenPtr[24] == 0 && ravenPtr[32] == 0 && ravenPtr[40] == 0 && ravenPtr[48] == 0 && ravenPtr[56] == 0)
				{
					int num = 5793 * dataIn[i] + 8192 >> 14;
					ravenPtr[0] = num;
					ravenPtr[8] = num;
					ravenPtr[16] = num;
					ravenPtr[24] = num;
					ravenPtr[32] = num;
					ravenPtr[40] = num;
					ravenPtr[48] = num;
					ravenPtr[56] = num;
				}
				else
				{
					int num2 = 5793 * ravenPtr[0] + 2048 >> 12;
					int num3 = 5793 * ravenPtr[32] + 2048 >> 12;
					int num4 = ravenPtr[16];
					int num5 = ravenPtr[48];
					int num6 = 2896 * (ravenPtr[8] - ravenPtr[56]) + 2048 >> 12;
					int num7 = 2896 * (ravenPtr[8] + ravenPtr[56]) + 2048 >> 12;
					int num8 = ravenPtr[24];
					int num9 = ravenPtr[40];
					int num = num2 - num3 + 1 >> 1;
					num2 = num2 + num3 + 1 >> 1;
					num3 = num;
					num = num4 * 3784 + num5 * 1567 + 2048 >> 12;
					num4 = num4 * 1567 - num5 * 3784 + 2048 >> 12;
					num5 = num;
					num = num6 - num9 + 1 >> 1;
					num6 = num6 + num9 + 1 >> 1;
					num9 = num;
					num = num7 + num8 + 1 >> 1;
					num8 = num7 - num8 + 1 >> 1;
					num7 = num;
					num = num2 - num5 + 1 >> 1;
					num2 = num2 + num5 + 1 >> 1;
					num5 = num;
					num = num3 - num4 + 1 >> 1;
					num3 = num3 + num4 + 1 >> 1;
					num4 = num;
					num = num6 * 2276 + num7 * 3406 + 2048 >> 12;
					num6 = num6 * 3406 - num7 * 2276 + 2048 >> 12;
					num7 = num;
					num = num8 * 799 + num9 * 4017 + 2048 >> 12;
					num8 = num8 * 4017 - num9 * 799 + 2048 >> 12;
					num9 = num;
					ravenPtr[0] = num2 + num7;
					ravenPtr[56] = num2 - num7;
					ravenPtr[8] = num3 + num9;
					ravenPtr[48] = num3 - num9;
					ravenPtr[16] = num4 + num8;
					ravenPtr[40] = num4 - num8;
					ravenPtr[24] = num5 + num6;
					ravenPtr[32] = num5 - num6;
				}
			}
			for (int i = 0; i < 64; i++)
			{
				dataOut[i] = PdfDCTStream.dctClip[PdfDCTStream.dctClipOffset + 128 + (dataIn[i] + 8 >> 4)];
			}
		}
		private int readHuffSym(DCTHuffTable table)
		{
			ushort num = 0;
			int num2 = 0;
			int num3;
			while ((num3 = this.readBit()) != -1)
			{
				num = (ushort)(((int)num << 1) + num3);
				num2++;
				if (num - table.firstCode[num2] < table.numCodes[num2])
				{
					num -= table.firstCode[num2];
					return (int)table.sym[(int)((ushort)table.firstSym[num2] + num)];
				}
				if (num2 >= 16)
				{
					AuxException.Throw("Invalid Huffman code in DCT stream.");
					return 9999;
				}
			}
			return 9999;
		}
		private int readAmp(int size)
		{
			int num = 0;
			for (int i = 0; i < size; i++)
			{
				int num2;
				if ((num2 = this.readBit()) == -1)
				{
					return 9999;
				}
				num = (num << 1) + num2;
			}
			if (num < 1 << size - 1)
			{
				num -= (1 << size) - 1;
			}
			return num;
		}
		private int readBit()
		{
			if (this.inputBits == 0)
			{
				int @char;
				if ((@char = this.str.getChar()) == -1)
				{
					return -1;
				}
				if (@char == 255)
				{
					int char2;
					do
					{
						char2 = this.str.getChar();
					}
					while (char2 == 255);
					if (char2 != 0)
					{
						AuxException.Throw("Invalid DCT data: missing 00 after ff.");
						return -1;
					}
				}
				this.inputBuf = @char;
				this.inputBits = 8;
			}
			int result = this.inputBuf >> this.inputBits - 1 & 1;
			this.inputBits--;
			return result;
		}
		private bool readHeader()
		{
			bool flag = false;
			while (!flag)
			{
				int num = this.readMarker();
				int num2 = num;
				if (num2 <= 196)
				{
					if (num2 == -1)
					{
						AuxException.Throw("Invalid DCT header.");
						return false;
					}
					switch (num2)
					{
					case 192:
					case 193:
						if (!this.readBaselineSOF())
						{
							return false;
						}
						continue;
					case 194:
						if (!this.readProgressiveSOF())
						{
							return false;
						}
						continue;
					case 196:
						if (!this.readHuffmanTables())
						{
							return false;
						}
						continue;
					}
				}
				else
				{
					switch (num2)
					{
					case 216:
						continue;
					case 217:
						return false;
					case 218:
						if (!this.readScanInfo())
						{
							return false;
						}
						flag = true;
						continue;
					case 219:
						if (!this.readQuantTables())
						{
							return false;
						}
						continue;
					case 220:
					case 222:
					case 223:
						break;
					case 221:
						if (!this.readRestartInterval())
						{
							return false;
						}
						continue;
					case 224:
						if (!this.readJFIFMarker())
						{
							return false;
						}
						continue;
					default:
						if (num2 == 238)
						{
							if (!this.readAdobeMarker())
							{
								return false;
							}
							continue;
						}
						break;
					}
				}
				if (num < 224)
				{
					AuxException.Throw("Invalid DCT marker.");
					return false;
				}
				int num3 = this.read16() - 2;
				for (int i = 0; i < num3; i++)
				{
					this.str.getChar();
				}
			}
			return true;
		}
		private bool readBaselineSOF()
		{
			this.read16();
			int @char = this.str.getChar();
			this.height = this.read16();
			this.width = this.read16();
			this.numComps = this.str.getChar();
			if (@char != 8)
			{
				AuxException.Throw("Invalid DCT precision.");
				return false;
			}
			for (int i = 0; i < this.numComps; i++)
			{
				this.compInfo[i].id = this.str.getChar();
				int char2 = this.str.getChar();
				this.compInfo[i].hSample = (char2 >> 4 & 15);
				this.compInfo[i].vSample = (char2 & 15);
				this.compInfo[i].quantTable = this.str.getChar();
			}
			this.progressive = false;
			return true;
		}
		private bool readProgressiveSOF()
		{
			this.read16();
			int @char = this.str.getChar();
			this.height = this.read16();
			this.width = this.read16();
			this.numComps = this.str.getChar();
			if (@char != 8)
			{
				AuxException.Throw("Invalid DCT precision.");
				return false;
			}
			for (int i = 0; i < this.numComps; i++)
			{
				this.compInfo[i].id = this.str.getChar();
				int char2 = this.str.getChar();
				this.compInfo[i].hSample = (char2 >> 4 & 15);
				this.compInfo[i].vSample = (char2 & 15);
				this.compInfo[i].quantTable = this.str.getChar();
			}
			this.progressive = true;
			return true;
		}
		private bool readScanInfo()
		{
			int num = this.read16() - 2;
			this.scanInfo.numComps = this.str.getChar();
			num--;
			if (num != 2 * this.scanInfo.numComps + 3)
			{
				AuxException.Throw("Invalid DCT scan info block.");
				return false;
			}
			this.interleaved = (this.scanInfo.numComps == this.numComps);
			for (int i = 0; i < this.numComps; i++)
			{
				this.scanInfo.comp[i] = false;
			}
			int char2;
			for (int j = 0; j < this.scanInfo.numComps; j++)
			{
				int @char = this.str.getChar();
				int i;
				if (@char == this.compInfo[j].id)
				{
					i = j;
				}
				else
				{
					i = 0;
					while (i < this.numComps && @char != this.compInfo[i].id)
					{
						i++;
					}
					if (i == this.numComps)
					{
						AuxException.Throw("Invalid DCT component ID in scan info block.");
						return false;
					}
				}
				this.scanInfo.comp[i] = true;
				char2 = this.str.getChar();
				this.scanInfo.dcHuffTable[i] = (char2 >> 4 & 15);
				this.scanInfo.acHuffTable[i] = (char2 & 15);
			}
			this.scanInfo.firstCoeff = this.str.getChar();
			this.scanInfo.lastCoeff = this.str.getChar();
			char2 = this.str.getChar();
			this.scanInfo.ah = (char2 >> 4 & 15);
			this.scanInfo.al = (char2 & 15);
			return true;
		}
		private bool readQuantTables()
		{
			for (int i = this.read16() - 2; i > 0; i -= 65)
			{
				int @char = this.str.getChar();
				if ((@char & 240) != 0 || @char >= 4)
				{
					AuxException.Throw("Invalid DCT quantization table.");
					return false;
				}
				if (@char == this.numQuantTables)
				{
					this.numQuantTables = @char + 1;
				}
				for (int j = 0; j < 64; j++)
				{
					this.quantTables[@char][PdfDCTStream.dctZigZag[j]] = (byte)this.str.getChar();
				}
			}
			return true;
		}
		private bool readHuffmanTables()
		{
			byte b;
			for (int i = this.read16() - 2; i > 0; i -= (int)b)
			{
				int num = this.str.getChar();
				i--;
				if ((num & 15) >= 4)
				{
					AuxException.Throw("Invalid DCT Huffman table.");
					return false;
				}
				DCTHuffTable dCTHuffTable;
				if ((num & 16) != 0)
				{
					num &= 15;
					if (num >= this.numACHuffTables)
					{
						this.numACHuffTables = num + 1;
					}
					dCTHuffTable = this.acHuffTables[num];
				}
				else
				{
					if (num >= this.numDCHuffTables)
					{
						this.numDCHuffTables = num + 1;
					}
					dCTHuffTable = this.dcHuffTables[num];
				}
				b = 0;
				ushort num2 = 0;
				for (int j = 1; j <= 16; j++)
				{
					int @char = this.str.getChar();
					dCTHuffTable.firstSym[j] = b;
					dCTHuffTable.firstCode[j] = num2;
					dCTHuffTable.numCodes[j] = (ushort)@char;
					b += (byte)@char;
					num2 = (ushort)((int)num2 + @char << 1);
				}
				i -= 16;
				for (int j = 0; j < (int)b; j++)
				{
					dCTHuffTable.sym[j] = (byte)this.str.getChar();
				}
			}
			return true;
		}
		private bool readRestartInterval()
		{
			int num = this.read16();
			if (num != 4)
			{
				AuxException.Throw("Invalid DCT restart interval.");
				return false;
			}
			this.restartInterval = this.read16();
			return true;
		}
		private bool readJFIFMarker()
		{
			char[] array = new char[5];
			int i = this.read16();
			i -= 2;
			if (i >= 5)
			{
				for (int j = 0; j < 5; j++)
				{
					int @char;
					if ((@char = this.str.getChar()) == -1)
					{
						AuxException.Throw("Invalid DCT APP0 marker.");
						return false;
					}
					array[j] = (char)@char;
				}
				i -= 5;
				if (string.Compare(new string(array, 0, 5), 0, "JFIF\0", 0, 5) == 0)
				{
					this.gotJFIFMarker = true;
				}
			}
			while (i > 0)
			{
				if (this.str.getChar() == -1)
				{
					AuxException.Throw("Invalid DCT APP0 marker.");
					return false;
				}
				i--;
			}
			return true;
		}
		private bool readAdobeMarker()
		{
			char[] array = new char[12];
			int num = this.read16();
			if (num >= 14)
			{
				for (int i = 0; i < 12; i++)
				{
					int @char;
					if ((@char = this.str.getChar()) == -1)
					{
						goto IL_71;
					}
					array[i] = (char)@char;
				}
				if (string.Compare(new string(array, 0, 5), "Adobe") == 0)
				{
					this.colorXform = (int)array[11];
					for (int i = 14; i < num; i++)
					{
						if (this.str.getChar() == -1)
						{
							goto IL_71;
						}
					}
					return true;
				}
			}
			IL_71:
			AuxException.Throw("Invalid DCT Adobe APP14 marker");
			return false;
		}
		private bool readTrailer()
		{
			int num = this.readMarker();
			return num == 217;
		}
		private int readMarker()
		{
			int @char;
			while (true)
			{
				@char = this.str.getChar();
				if (@char == 255)
				{
					do
					{
						@char = this.str.getChar();
					}
					while (@char == 255);
					if (@char != 0)
					{
						break;
					}
				}
			}
			return @char;
		}
		private int read16()
		{
			int @char;
			if ((@char = this.str.getChar()) == -1)
			{
				return -1;
			}
			int char2;
			if ((char2 = this.str.getChar()) == -1)
			{
				return -1;
			}
			return (@char << 8) + char2;
		}
	}
}
