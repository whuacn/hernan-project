using System;
namespace Persits.PDF
{
	internal class JPXStream : PdfDecoderHelper
	{
		private const int jpxCoeffSignificantB = 0;
		private const int jpxCoeffTouchedB = 1;
		private const int jpxCoeffFirstMagRefB = 2;
		private const int jpxCoeffSignB = 7;
		private const int jpxCoeffSignificant = 1;
		private const int jpxCoeffTouched = 2;
		private const int jpxCoeffFirstMagRef = 4;
		private const int jpxCoeffSign = 128;
		private const int jpxNContexts = 19;
		private const int jpxContextSigProp = 0;
		private const int jpxContextSign = 9;
		private const int jpxContextMagRef = 14;
		private const int jpxContextRunLength = 17;
		private const int jpxContextUniform = 18;
		private const int jpxPassSigProp = 0;
		private const int jpxPassMagRef = 1;
		private const int jpxPassCleanup = 2;
		private const double idwtAlpha = -1.5861343420599241;
		private const double idwtBeta = -0.052980118572961;
		private const double idwtGamma = 0.882911075530934;
		private const double idwtDelta = 0.443506852043971;
		private const double idwtKappa = 1.2301741049140009;
		private const double idwtIKappa = 0.8128930661159609;
		private const int fracBits = 16;
		private PdfStream str;
		private PdfStream bufStr;
		private uint nComps;
		private uint[] bpc;
		private uint width;
		private uint height;
		private bool haveImgHdr;
		private bool haveCS;
		private JPXPalette palette;
		private bool havePalette;
		private JPXCompMap compMap;
		private bool haveCompMap;
		private JPXChannelDefn channelDefn;
		private bool haveChannelDefn;
		private JPXImage img;
		private uint bitBuf;
		private int bitBufLen;
		private bool bitBufSkip;
		private uint byteCount;
		private uint curX;
		private uint curY;
		private uint curComp;
		private uint readBuf;
		private uint readBufLen;
		internal static uint[,,,] sigPropContext = new uint[,,,]
		{

			{

				{

					{
						0u,
						0u,
						0u
					},

					{
						1u,
						1u,
						3u
					},

					{
						2u,
						2u,
						6u
					},

					{
						2u,
						2u,
						8u
					},

					{
						2u,
						2u,
						8u
					}
				},

				{

					{
						5u,
						3u,
						1u
					},

					{
						6u,
						3u,
						4u
					},

					{
						6u,
						3u,
						7u
					},

					{
						6u,
						3u,
						8u
					},

					{
						6u,
						3u,
						8u
					}
				},

				{

					{
						8u,
						4u,
						2u
					},

					{
						8u,
						4u,
						5u
					},

					{
						8u,
						4u,
						7u
					},

					{
						8u,
						4u,
						8u
					},

					{
						8u,
						4u,
						8u
					}
				}
			},

			{

				{

					{
						3u,
						5u,
						1u
					},

					{
						3u,
						6u,
						4u
					},

					{
						3u,
						6u,
						7u
					},

					{
						3u,
						6u,
						8u
					},

					{
						3u,
						6u,
						8u
					}
				},

				{

					{
						7u,
						7u,
						2u
					},

					{
						7u,
						7u,
						5u
					},

					{
						7u,
						7u,
						7u
					},

					{
						7u,
						7u,
						8u
					},

					{
						7u,
						7u,
						8u
					}
				},

				{

					{
						8u,
						7u,
						2u
					},

					{
						8u,
						7u,
						5u
					},

					{
						8u,
						7u,
						7u
					},

					{
						8u,
						7u,
						8u
					},

					{
						8u,
						7u,
						8u
					}
				}
			},

			{

				{

					{
						4u,
						8u,
						2u
					},

					{
						4u,
						8u,
						5u
					},

					{
						4u,
						8u,
						7u
					},

					{
						4u,
						8u,
						8u
					},

					{
						4u,
						8u,
						8u
					}
				},

				{

					{
						7u,
						8u,
						2u
					},

					{
						7u,
						8u,
						5u
					},

					{
						7u,
						8u,
						7u
					},

					{
						7u,
						8u,
						8u
					},

					{
						7u,
						8u,
						8u
					}
				},

				{

					{
						8u,
						8u,
						2u
					},

					{
						8u,
						8u,
						5u
					},

					{
						8u,
						8u,
						7u
					},

					{
						8u,
						8u,
						8u
					},

					{
						8u,
						8u,
						8u
					}
				}
			}
		};
		internal static uint[,,] signContext = new uint[,,]
		{

			{

				{
					13u,
					1u
				},

				{
					13u,
					1u
				},

				{
					12u,
					1u
				},

				{
					11u,
					1u
				},

				{
					11u,
					1u
				}
			},

			{

				{
					13u,
					1u
				},

				{
					13u,
					1u
				},

				{
					12u,
					1u
				},

				{
					11u,
					1u
				},

				{
					11u,
					1u
				}
			},

			{

				{
					10u,
					1u
				},

				{
					10u,
					1u
				},

				{
					9u,
					0u
				},

				{
					10u,
					0u
				},

				{
					10u,
					0u
				}
			},

			{

				{
					11u,
					0u
				},

				{
					11u,
					0u
				},

				{
					12u,
					0u
				},

				{
					13u,
					0u
				},

				{
					13u,
					0u
				}
			},

			{

				{
					11u,
					0u
				},

				{
					11u,
					0u
				},

				{
					12u,
					0u
				},

				{
					13u,
					0u
				},

				{
					13u,
					0u
				}
			}
		};
		internal JPXStream(PdfStream strA)
		{
			this.str = strA;
			this.bufStr = new PdfStream(strA);
			this.nComps = 0u;
			this.bpc = null;
			this.width = (this.height = 0u);
			this.haveCS = false;
			this.havePalette = false;
			this.haveCompMap = false;
			this.haveChannelDefn = false;
			this.img = new JPXImage();
			this.img.tiles = null;
			this.bitBuf = 0u;
			this.bitBufLen = 0;
			this.bitBufSkip = false;
			this.byteCount = 0u;
		}
		~JPXStream()
		{
			this.close();
		}
		internal void close()
		{
			this.bpc = null;
			if (this.havePalette)
			{
				this.havePalette = false;
			}
			if (this.haveCompMap)
			{
				this.haveCompMap = false;
			}
			if (this.haveChannelDefn)
			{
				this.haveChannelDefn = false;
			}
			if (this.img.tiles != null)
			{
				for (uint num = 0u; num < this.img.nXTiles * this.img.nYTiles; num += 1u)
				{
					JPXTile jPXTile = this.img.tiles[(int)((UIntPtr)num)];
					if (jPXTile.tileComps != null)
					{
						for (uint num2 = 0u; num2 < this.img.nComps; num2 += 1u)
						{
							JPXTileComp jPXTileComp = jPXTile.tileComps[(int)((UIntPtr)num2)];
							if (jPXTileComp.resLevels != null)
							{
								for (uint num3 = 0u; num3 <= jPXTileComp.nDecompLevels; num3 += 1u)
								{
									JPXResLevel jPXResLevel = jPXTileComp.resLevels[(int)((UIntPtr)num3)];
									if (jPXResLevel.precincts != null)
									{
										for (uint num4 = 0u; num4 < 1u; num4 += 1u)
										{
											JPXPrecinct jPXPrecinct = jPXResLevel.precincts[(int)((UIntPtr)num4)];
											if (jPXPrecinct.subbands != null)
											{
												for (uint num5 = 0u; num5 < ((num3 == 0u) ? 1u : 3u); num5 += 1u)
												{
													JPXSubband jPXSubband = jPXPrecinct.subbands[(int)((UIntPtr)num5)];
													if (jPXSubband.cbs != null)
													{
														for (uint num6 = 0u; num6 < jPXSubband.nXCBs * jPXSubband.nYCBs; num6 += 1u)
														{
															JPXCodeBlock jPXCodeBlock = jPXSubband.cbs[(int)((UIntPtr)num6)];
															JArithmeticDecoder arg_F1_0 = jPXCodeBlock.arithDecoder;
															JArithmeticDecoderStats arg_F9_0 = jPXCodeBlock.stats;
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				this.img.tiles = null;
			}
		}
		internal override void reset()
		{
			this.bufStr.reset();
			if (this.readBoxes())
			{
				this.curY = this.img.yOffset;
			}
			else
			{
				this.curY = this.img.ySize;
			}
			this.curX = this.img.xOffset;
			this.curComp = 0u;
			this.readBufLen = 0u;
		}
		internal override int getChar()
		{
			if (this.readBufLen < 8u)
			{
				this.fillReadBuf();
			}
			int result;
			if (this.readBufLen == 8u)
			{
				result = (int)(this.readBuf & 255u);
				this.readBufLen = 0u;
			}
			else
			{
				if (this.readBufLen > 8u)
				{
					result = ((int)this.readBuf >> (int)(this.readBufLen - 8u) & 255);
					this.readBufLen -= 8u;
				}
				else
				{
					if (this.readBufLen == 0u)
					{
						result = -1;
					}
					else
					{
						result = (int)(this.readBuf << (int)(8u - this.readBufLen) & 255u);
						this.readBufLen = 0u;
					}
				}
			}
			return result;
		}
		internal int lookChar()
		{
			if (this.readBufLen < 8u)
			{
				this.fillReadBuf();
			}
			int result;
			if (this.readBufLen == 8u)
			{
				result = (int)(this.readBuf & 255u);
			}
			else
			{
				if (this.readBufLen > 8u)
				{
					result = (int)(this.readBuf >> (int)(this.readBufLen - 8u) & 255u);
				}
				else
				{
					if (this.readBufLen == 0u)
					{
						result = -1;
					}
					else
					{
						result = (int)(this.readBuf << (int)(8u - this.readBufLen) & 255u);
					}
				}
			}
			return result;
		}
		private void fillReadBuf()
		{
			while (this.curY < this.img.ySize)
			{
				uint num = (this.curY - this.img.yTileOffset) / this.img.yTileSize * this.img.nXTiles + (this.curX - this.img.xTileOffset) / this.img.xTileSize;
				JPXTileComp jPXTileComp = this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)this.curComp)];
				uint num2 = this.jpxCeilDiv((this.curX - this.img.xTileOffset) % this.img.xTileSize, jPXTileComp.hSep);
				uint num3 = this.jpxCeilDiv((this.curY - this.img.yTileOffset) % this.img.yTileSize, jPXTileComp.vSep);
				int num4 = jPXTileComp.data[(int)((UIntPtr)(num3 * (jPXTileComp.x1 - jPXTileComp.x0) + num2))];
				int num5 = (int)jPXTileComp.prec;
				if ((this.curComp += 1u) == this.img.nComps)
				{
					this.curComp = 0u;
					if ((this.curX += 1u) == this.img.xSize)
					{
						this.curX = this.img.xOffset;
						this.curY += 1u;
						if (num5 < 8)
						{
							num4 <<= 8 - num5;
							num5 = 8;
						}
					}
				}
				if (num5 == 8)
				{
					this.readBuf = (this.readBuf << 8 | (uint)(num4 & 255));
				}
				else
				{
					this.readBuf = (this.readBuf << num5 | (uint)(num4 & (1 << num5) - 1));
				}
				this.readBufLen += (uint)num5;
				if (this.readBufLen >= 8u)
				{
					return;
				}
			}
		}
		internal void getImageParams(ref int bitsPerComponent, StreamColorSpaceMode csMode)
		{
			uint num = 0u;
			uint num2 = 0u;
			uint num3 = 0u;
			JPXColorSpaceType jPXColorSpaceType = JPXColorSpaceType.jpxCSYCCK;
			uint num4 = 0u;
			uint num5 = 0u;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			bool flag2;
			bool flag = flag2 = false;
			this.bufStr.reset();
			if (this.bufStr.lookChar() == 255)
			{
				this.getImageParams2(ref bitsPerComponent, csMode);
				return;
			}
			while (this.readBoxHdr(ref num, ref num2, ref num3))
			{
				if (num == 1785737832u)
				{
					this.cover(0);
				}
				else
				{
					if (num == 1768449138u)
					{
						this.cover(1);
						if (this.readULong(ref num5) && this.readULong(ref num5) && this.readUWord(ref num5) && this.readUByte(ref num4) && this.readUByte(ref num5) && this.readUByte(ref num5) && this.readUByte(ref num5))
						{
							bitsPerComponent = (int)(num4 + 1u);
							flag2 = true;
						}
					}
					else
					{
						if (num == 1668246642u)
						{
							this.cover(2);
							if (this.readByte(ref num6) && this.readByte(ref num7) && this.readByte(ref num8))
							{
								if (num6 == 1)
								{
									if (this.readULong2(ref jPXColorSpaceType))
									{
										StreamColorSpaceMode streamColorSpaceMode = StreamColorSpaceMode.streamCSNone;
										if (jPXColorSpaceType == JPXColorSpaceType.jpxCSBiLevel || jPXColorSpaceType == JPXColorSpaceType.jpxCSGrayscale)
										{
											streamColorSpaceMode = StreamColorSpaceMode.streamCSDeviceGray;
										}
										else
										{
											if (jPXColorSpaceType == JPXColorSpaceType.jpxCSCMYK)
											{
												streamColorSpaceMode = StreamColorSpaceMode.streamCSDeviceCMYK;
											}
											else
											{
												if (jPXColorSpaceType == JPXColorSpaceType.jpxCSsRGB || jPXColorSpaceType == JPXColorSpaceType.jpxCSCISesRGB || jPXColorSpaceType == JPXColorSpaceType.jpxCSROMMRGB)
												{
													streamColorSpaceMode = StreamColorSpaceMode.streamCSDeviceRGB;
												}
											}
										}
										if (streamColorSpaceMode != StreamColorSpaceMode.streamCSNone && (!flag || num7 > num9))
										{
											csMode = streamColorSpaceMode;
											num9 = num7;
											flag = true;
										}
										for (uint num10 = 0u; num10 < num3 - 7u; num10 += 1u)
										{
											this.bufStr.getChar();
										}
									}
								}
								else
								{
									for (uint num10 = 0u; num10 < num3 - 3u; num10 += 1u)
									{
										this.bufStr.getChar();
									}
								}
							}
						}
						else
						{
							if (num == 1785737827u)
							{
								this.cover(3);
								if (!flag2 || !flag)
								{
									this.getImageParams2(ref bitsPerComponent, csMode);
									return;
								}
								break;
							}
							else
							{
								this.cover(4);
								for (uint num10 = 0u; num10 < num3; num10 += 1u)
								{
									this.bufStr.getChar();
								}
							}
						}
					}
				}
			}
		}
		private void getImageParams2(ref int bitsPerComponent, StreamColorSpaceMode csMode)
		{
			int num = 0;
			uint num2 = 0u;
			uint num3 = 0u;
			uint num4 = 0u;
			uint num5 = 0u;
			while (this.readMarkerHdr(ref num, ref num2))
			{
				if (num == 81)
				{
					this.cover(5);
					if (!this.readUWord(ref num5) || !this.readULong(ref num5) || !this.readULong(ref num5) || !this.readULong(ref num5) || !this.readULong(ref num5) || !this.readULong(ref num5) || !this.readULong(ref num5) || !this.readULong(ref num5) || !this.readULong(ref num5) || !this.readUWord(ref num3) || !this.readUByte(ref num4))
					{
						break;
					}
					bitsPerComponent = (int)((num4 & 127u) + 1u);
					if (num3 == 1u)
					{
						return;
					}
					if (num3 == 3u)
					{
						return;
					}
					if (num3 == 4u)
					{
						return;
					}
					break;
				}
				else
				{
					this.cover(6);
					if (num2 > 2u)
					{
						for (uint num6 = 0u; num6 < num2 - 2u; num6 += 1u)
						{
							this.bufStr.getChar();
						}
					}
				}
			}
		}
		private bool readBoxes()
		{
			uint num = 0u;
			uint num2 = 0u;
			uint num3 = 0u;
			uint num4 = 0u;
			uint num5 = 0u;
			uint num6 = 0u;
			uint num7 = 0u;
			this.haveImgHdr = false;
			if (this.bufStr.lookChar() != 255)
			{
				while (this.readBoxHdr(ref num, ref num2, ref num3))
				{
					uint num8 = num;
					if (num8 <= 1668246642u)
					{
						if (num8 <= 1667523942u)
						{
							if (num8 != 1651532643u)
							{
								if (num8 == 1667523942u)
								{
									this.cover(14);
									if (!this.readUWord(ref this.channelDefn.nChannels))
									{
										AuxException.Throw("Unexpected EOF in JPX stream.");
										return false;
									}
									this.channelDefn.idx = new uint[this.channelDefn.nChannels];
									this.channelDefn.type = new uint[this.channelDefn.nChannels];
									this.channelDefn.assoc = new uint[this.channelDefn.nChannels];
									for (uint num9 = 0u; num9 < this.channelDefn.nChannels; num9 += 1u)
									{
										if (!this.readUWord(ref this.channelDefn.idx[(int)((UIntPtr)num9)]) || !this.readUWord(ref this.channelDefn.type[(int)((UIntPtr)num9)]) || !this.readUWord(ref this.channelDefn.assoc[(int)((UIntPtr)num9)]))
										{
											AuxException.Throw("Unexpected EOF in JPX stream.");
											return false;
										}
									}
									this.haveChannelDefn = true;
									continue;
								}
							}
							else
							{
								this.cover(10);
								if (!this.haveImgHdr)
								{
									AuxException.Throw("Found bits per component box before image header box in JPX stream.");
									return false;
								}
								if (num3 != this.nComps)
								{
									AuxException.Throw("Invalid bits per component box in JPX stream.");
									return false;
								}
								for (uint num9 = 0u; num9 < this.nComps; num9 += 1u)
								{
									if (!this.readUByte(ref this.bpc[(int)((UIntPtr)num9)]))
									{
										AuxException.Throw("Unexpected EOF in JPX stream.");
										return false;
									}
								}
								continue;
							}
						}
						else
						{
							if (num8 == 1668112752u)
							{
								this.cover(13);
								this.compMap.nChannels = num3 / 4u;
								this.compMap.comp = new uint[this.compMap.nChannels];
								this.compMap.type = new uint[this.compMap.nChannels];
								this.compMap.pComp = new uint[this.compMap.nChannels];
								for (uint num9 = 0u; num9 < this.compMap.nChannels; num9 += 1u)
								{
									if (!this.readUWord(ref this.compMap.comp[(int)((UIntPtr)num9)]) || !this.readUByte(ref this.compMap.type[(int)((UIntPtr)num9)]) || !this.readUByte(ref this.compMap.pComp[(int)((UIntPtr)num9)]))
									{
										AuxException.Throw("Unexpected EOF in JPX stream.");
										return false;
									}
								}
								this.haveCompMap = true;
								continue;
							}
							if (num8 == 1668246642u)
							{
								this.cover(11);
								if (!this.readColorSpecBox(num3))
								{
									return false;
								}
								continue;
							}
						}
					}
					else
					{
						if (num8 <= 1785737827u)
						{
							if (num8 != 1768449138u)
							{
								if (num8 == 1785737827u)
								{
									this.cover(15);
									if (this.bpc == null)
									{
										AuxException.Throw("JPX stream is missing the image header box.");
									}
									if (!this.haveCS)
									{
										AuxException.Throw("JPX stream has no supported color spec.");
									}
									if (!this.readCodestream(num3))
									{
										return false;
									}
									continue;
								}
							}
							else
							{
								this.cover(9);
								if (!this.readULong(ref this.height) || !this.readULong(ref this.width) || !this.readUWord(ref this.nComps) || !this.readUByte(ref num4) || !this.readUByte(ref num5) || !this.readUByte(ref num6) || !this.readUByte(ref num7))
								{
									AuxException.Throw("Unexpected EOF in JPX stream.");
									return false;
								}
								if (num5 != 7u)
								{
									AuxException.Throw("Unknown compression type in JPX stream.");
									return false;
								}
								this.bpc = new uint[this.nComps];
								for (uint num9 = 0u; num9 < this.nComps; num9 += 1u)
								{
									this.bpc[(int)((UIntPtr)num9)] = num4;
								}
								this.haveImgHdr = true;
								continue;
							}
						}
						else
						{
							if (num8 == 1785737832u)
							{
								this.cover(8);
								continue;
							}
							if (num8 == 1885564018u)
							{
								this.cover(12);
								if (!this.readUWord(ref this.palette.nEntries) || !this.readUByte(ref this.palette.nComps))
								{
									AuxException.Throw("Unexpected EOF in JPX stream.");
									return false;
								}
								this.palette.bpc = new uint[this.palette.nComps];
								this.palette.c = new int[this.palette.nEntries * this.palette.nComps];
								for (uint num9 = 0u; num9 < this.palette.nComps; num9 += 1u)
								{
									if (!this.readUByte(ref this.palette.bpc[(int)((UIntPtr)num9)]))
									{
										AuxException.Throw("Unexpected EOF in JPX stream.");
										return false;
									}
									this.palette.bpc[(int)((UIntPtr)num9)] += 1u;
								}
								for (uint num9 = 0u; num9 < this.palette.nEntries; num9 += 1u)
								{
									for (uint num10 = 0u; num10 < this.palette.nComps; num10 += 1u)
									{
										if (!this.readNBytes((int)((this.palette.bpc[(int)((UIntPtr)num10)] & 127u) + 7u >> 3), (this.palette.bpc[(int)((UIntPtr)num10)] & 128u) != 0u, ref this.palette.c[(int)((UIntPtr)(num9 * this.palette.nComps + num10))]))
										{
											AuxException.Throw("Unexpected EOF in JPX stream.");
											return false;
										}
									}
								}
								this.havePalette = true;
								continue;
							}
						}
					}
					this.cover(16);
					for (uint num9 = 0u; num9 < num3; num9 += 1u)
					{
						if (this.bufStr.getChar() == -1)
						{
							AuxException.Throw("Unexpected EOF in JPX stream.");
							return false;
						}
					}
				}
				return true;
			}
			this.cover(7);
			AuxException.Throw("Naked JPEG 2000 codestream, missing JP2/JPX wrapper.");
			if (!this.readCodestream(0u))
			{
				return false;
			}
			this.nComps = this.img.nComps;
			this.bpc = new uint[this.nComps];
			for (uint num9 = 0u; num9 < this.nComps; num9 += 1u)
			{
				this.bpc[(int)((UIntPtr)num9)] = this.img.tiles[0].tileComps[(int)((UIntPtr)num9)].prec;
			}
			this.width = this.img.xSize - this.img.xOffset;
			this.height = this.img.ySize - this.img.yOffset;
			return true;
		}
		private bool readColorSpecBox(uint dataLen)
		{
			int num = 0;
			uint num2 = 0u;
			uint num3 = 0u;
			uint num4 = 0u;
			uint num5 = 0u;
			uint num6 = 0u;
			bool flag4;
			bool flag3;
			bool flag2;
			bool flag = flag2 = (flag3 = (flag4 = false));
			while (this.readMarkerHdr(ref num, ref num4))
			{
				int num7 = num;
				uint num8;
				switch (num7)
				{
				case 79:
					this.cover(19);
					break;
				case 80:
				case 84:
				case 86:
				case 88:
				case 89:
				case 90:
				case 91:
				case 97:
				case 98:
					goto IL_21BC;
				case 81:
					this.cover(20);
					if (flag2)
					{
						AuxException.Throw("Duplicate SIZ marker segment in JPX stream.");
						return false;
					}
					if (!this.readUWord(ref num5) || !this.readULong(ref this.img.xSize) || !this.readULong(ref this.img.ySize) || !this.readULong(ref this.img.xOffset) || !this.readULong(ref this.img.yOffset) || !this.readULong(ref this.img.xTileSize) || !this.readULong(ref this.img.yTileSize) || !this.readULong(ref this.img.xTileOffset) || !this.readULong(ref this.img.yTileOffset) || !this.readUWord(ref this.img.nComps))
					{
						AuxException.Throw("Invalid JPX SIZ marker segment.");
						return false;
					}
					if (this.haveImgHdr && this.img.nComps != this.nComps)
					{
						AuxException.Throw("Different number of components in JPX SIZ marker segment.");
						return false;
					}
					if (this.img.xSize == 0u || this.img.ySize == 0u || this.img.xOffset >= this.img.xSize || this.img.yOffset >= this.img.ySize || this.img.xTileSize == 0u || this.img.yTileSize == 0u || this.img.xTileOffset > this.img.xOffset || this.img.yTileOffset > this.img.yOffset || this.img.xTileSize + this.img.xTileOffset <= this.img.xOffset || this.img.yTileSize + this.img.yTileOffset <= this.img.yOffset)
					{
						AuxException.Throw("Invalid JPX SIZ marker segment.");
						return false;
					}
					this.img.nXTiles = (this.img.xSize - this.img.xTileOffset + this.img.xTileSize - 1u) / this.img.xTileSize;
					this.img.nYTiles = (this.img.ySize - this.img.yTileOffset + this.img.yTileSize - 1u) / this.img.yTileSize;
					if (this.img.nXTiles <= 0u || this.img.nYTiles <= 0u || this.img.nXTiles >= 2147483647u / this.img.nYTiles)
					{
						AuxException.Throw("Bad tile count in JPX SIZ marker segment.");
						return false;
					}
					this.img.tiles = new JPXTile[this.img.nXTiles * this.img.nYTiles];
					for (num8 = 0u; num8 < this.img.nXTiles * this.img.nYTiles; num8 += 1u)
					{
						this.img.tiles[(int)((UIntPtr)num8)] = new JPXTile();
						this.img.tiles[(int)((UIntPtr)num8)].init = false;
						this.img.tiles[(int)((UIntPtr)num8)].tileComps = new JPXTileComp[this.img.nComps];
						for (num6 = 0u; num6 < this.img.nComps; num6 += 1u)
						{
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)] = new JPXTileComp();
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps = null;
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].data = null;
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].buf = null;
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels = null;
						}
					}
					for (num6 = 0u; num6 < this.img.nComps; num6 += 1u)
					{
						if (!this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].prec) || !this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].hSep) || !this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].vSep))
						{
							AuxException.Throw("Invalid JPX SIZ marker segment.");
							return false;
						}
						if (this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].hSep == 0u || this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].vSep == 0u)
						{
							AuxException.Throw("Invalid JPX SIZ marker segment.");
							return false;
						}
						this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].sgned = ((this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].prec & 128u) != 0u);
						this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].prec = (this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].prec & 127u) + 1u;
						for (num8 = 1u; num8 < this.img.nXTiles * this.img.nYTiles; num8 += 1u)
						{
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)] = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].copy();
						}
					}
					flag2 = true;
					break;
				case 82:
					this.cover(21);
					if (!flag2)
					{
						AuxException.Throw("JPX COD marker segment before SIZ segment.");
						return false;
					}
					if (!this.readUByte(ref this.img.tiles[0].tileComps[0].style) || !this.readUByte(ref this.img.tiles[0].progOrder) || !this.readUWord(ref this.img.tiles[0].nLayers) || !this.readUByte(ref this.img.tiles[0].multiComp) || !this.readUByte(ref this.img.tiles[0].tileComps[0].nDecompLevels) || !this.readUByte(ref this.img.tiles[0].tileComps[0].codeBlockW) || !this.readUByte(ref this.img.tiles[0].tileComps[0].codeBlockH) || !this.readUByte(ref this.img.tiles[0].tileComps[0].codeBlockStyle) || !this.readUByte(ref this.img.tiles[0].tileComps[0].transform))
					{
						AuxException.Throw("Invalid JPX COD marker segment.");
						return false;
					}
					if (this.img.tiles[0].tileComps[0].nDecompLevels > 32u || this.img.tiles[0].tileComps[0].codeBlockW > 8u || this.img.tiles[0].tileComps[0].codeBlockH > 8u)
					{
						AuxException.Throw("Invalid JPX COD marker segment.");
						return false;
					}
					this.img.tiles[0].tileComps[0].codeBlockW += 2u;
					this.img.tiles[0].tileComps[0].codeBlockH += 2u;
					for (num8 = 0u; num8 < this.img.nXTiles * this.img.nYTiles; num8 += 1u)
					{
						if (num8 != 0u)
						{
							this.img.tiles[(int)((UIntPtr)num8)].progOrder = this.img.tiles[0].progOrder;
							this.img.tiles[(int)((UIntPtr)num8)].nLayers = this.img.tiles[0].nLayers;
							this.img.tiles[(int)((UIntPtr)num8)].multiComp = this.img.tiles[0].multiComp;
						}
						for (num6 = 0u; num6 < this.img.nComps; num6 += 1u)
						{
							if (num8 != 0u || num6 != 0u)
							{
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].style = this.img.tiles[0].tileComps[0].style;
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nDecompLevels = this.img.tiles[0].tileComps[0].nDecompLevels;
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].codeBlockW = this.img.tiles[0].tileComps[0].codeBlockW;
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].codeBlockH = this.img.tiles[0].tileComps[0].codeBlockH;
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].codeBlockStyle = this.img.tiles[0].tileComps[0].codeBlockStyle;
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].transform = this.img.tiles[0].tileComps[0].transform;
							}
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels = new JPXResLevel[this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nDecompLevels + 1u];
							for (uint num9 = 0u; num9 <= this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nDecompLevels; num9 += 1u)
							{
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)] = new JPXResLevel();
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precincts = null;
							}
						}
					}
					for (uint num9 = 0u; num9 <= this.img.tiles[0].tileComps[0].nDecompLevels; num9 += 1u)
					{
						if ((this.img.tiles[0].tileComps[0].style & 1u) != 0u)
						{
							this.cover(91);
							if (!this.readUByte(ref num2))
							{
								AuxException.Throw("Invalid JPX COD marker segment.");
								return false;
							}
							this.img.tiles[0].tileComps[0].resLevels[(int)((UIntPtr)num9)].precinctWidth = (num2 & 15u);
							this.img.tiles[0].tileComps[0].resLevels[(int)((UIntPtr)num9)].precinctHeight = (num2 >> 4 & 15u);
						}
						else
						{
							this.img.tiles[0].tileComps[0].resLevels[(int)((UIntPtr)num9)].precinctWidth = 15u;
							this.img.tiles[0].tileComps[0].resLevels[(int)((UIntPtr)num9)].precinctHeight = 15u;
						}
					}
					for (num8 = 0u; num8 < this.img.nXTiles * this.img.nYTiles; num8 += 1u)
					{
						for (num6 = 0u; num6 < this.img.nComps; num6 += 1u)
						{
							if (num8 != 0u || num6 != 0u)
							{
								for (uint num9 = 0u; num9 <= this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nDecompLevels; num9 += 1u)
								{
									this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctWidth = this.img.tiles[0].tileComps[0].resLevels[(int)((UIntPtr)num9)].precinctWidth;
									this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctHeight = this.img.tiles[0].tileComps[0].resLevels[(int)((UIntPtr)num9)].precinctHeight;
								}
							}
						}
					}
					flag = true;
					break;
				case 83:
					this.cover(22);
					if (!flag)
					{
						AuxException.Throw("JPX COC marker segment before COD segment.");
						return false;
					}
					if ((this.img.nComps > 256u && !this.readUWord(ref num6)) || (this.img.nComps <= 256u && !this.readUByte(ref num6)) || num6 >= this.img.nComps || !this.readUByte(ref num3) || !this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nDecompLevels) || !this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockW) || !this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockH) || !this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockStyle) || !this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].transform))
					{
						AuxException.Throw("Invalid JPX COC marker segment.");
						return false;
					}
					if (this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nDecompLevels > 32u || this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockW > 8u || this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockH > 8u)
					{
						AuxException.Throw("Invalid JPX COC marker segment.");
						return false;
					}
					this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].style = (uint)(((ulong)this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].style & 18446744073709551614uL) | (ulong)(num3 & 1u));
					this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockW += 2u;
					this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockH += 2u;
					for (num8 = 0u; num8 < this.img.nXTiles * this.img.nYTiles; num8 += 1u)
					{
						if (num8 != 0u)
						{
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].style = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].style;
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nDecompLevels = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nDecompLevels;
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].codeBlockW = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockW;
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].codeBlockH = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockH;
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].codeBlockStyle = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockStyle;
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].transform = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].transform;
						}
						JPXResLevel[] array = new JPXResLevel[this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nDecompLevels + 1u];
						if (this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels != null)
						{
							Array.Copy(this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels, array, this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels.Length);
						}
						this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels = array;
						for (uint num9 = 0u; num9 <= this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nDecompLevels; num9 += 1u)
						{
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precincts = null;
						}
					}
					for (uint num9 = 0u; num9 <= this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nDecompLevels; num9 += 1u)
					{
						if ((this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].style & 1u) != 0u)
						{
							if (!this.readUByte(ref num2))
							{
								AuxException.Throw("Invalid JPX COD marker segment.");
								return false;
							}
							this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctWidth = (num2 & 15u);
							this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctHeight = (num2 >> 4 & 15u);
						}
						else
						{
							this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctWidth = 15u;
							this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctHeight = 15u;
						}
					}
					for (num8 = 1u; num8 < this.img.nXTiles * this.img.nYTiles; num8 += 1u)
					{
						for (uint num9 = 0u; num9 <= this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nDecompLevels; num9 += 1u)
						{
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctWidth = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctWidth;
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctHeight = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctHeight;
						}
					}
					break;
				case 85:
					this.cover(28);
					for (num8 = 0u; num8 < num4 - 2u; num8 += 1u)
					{
						if (this.bufStr.getChar() == -1)
						{
							AuxException.Throw("Invalid JPX TLM marker segment.");
							return false;
						}
					}
					break;
				case 87:
					this.cover(29);
					for (num8 = 0u; num8 < num4 - 2u; num8 += 1u)
					{
						if (this.bufStr.getChar() == -1)
						{
							AuxException.Throw("Invalid JPX PLM marker segment.");
							return false;
						}
					}
					break;
				case 92:
					this.cover(23);
					if (!flag2)
					{
						AuxException.Throw("JPX QCD marker segment before SIZ segment.");
						return false;
					}
					if (!this.readUByte(ref this.img.tiles[0].tileComps[0].quantStyle))
					{
						AuxException.Throw("Invalid JPX QCD marker segment.");
						return false;
					}
					if ((this.img.tiles[0].tileComps[0].quantStyle & 31u) == 0u)
					{
						if (num4 <= 3u)
						{
							AuxException.Throw("Invalid JPX QCD marker segment.");
							return false;
						}
						this.img.tiles[0].tileComps[0].nQuantSteps = num4 - 3u;
						uint[] array2 = new uint[this.img.tiles[0].tileComps[0].nQuantSteps];
						if (this.img.tiles[0].tileComps[0].quantSteps != null)
						{
							Array.Copy(this.img.tiles[0].tileComps[0].quantSteps, array2, this.img.tiles[0].tileComps[0].quantSteps.Length);
						}
						this.img.tiles[0].tileComps[0].quantSteps = array2;
						for (num8 = 0u; num8 < this.img.tiles[0].tileComps[0].nQuantSteps; num8 += 1u)
						{
							if (!this.readUByte(ref this.img.tiles[0].tileComps[0].quantSteps[(int)((UIntPtr)num8)]))
							{
								AuxException.Throw("Invalid JPX QCD marker segment.");
								return false;
							}
						}
					}
					else
					{
						if ((this.img.tiles[0].tileComps[0].quantStyle & 31u) == 1u)
						{
							this.img.tiles[0].tileComps[0].nQuantSteps = 1u;
							uint[] array3 = new uint[this.img.tiles[0].tileComps[0].nQuantSteps];
							if (this.img.tiles[0].tileComps[0].quantSteps != null)
							{
								Array.Copy(this.img.tiles[0].tileComps[0].quantSteps, array3, this.img.tiles[0].tileComps[0].quantSteps.Length);
							}
							this.img.tiles[0].tileComps[0].quantSteps = array3;
							if (!this.readUWord(ref this.img.tiles[0].tileComps[0].quantSteps[0]))
							{
								AuxException.Throw("Invalid JPX QCD marker segment");
								return false;
							}
						}
						else
						{
							if ((this.img.tiles[0].tileComps[0].quantStyle & 31u) != 2u)
							{
								AuxException.Throw("Invalid JPX QCD marker segment.");
								return false;
							}
							if (num4 < 5u)
							{
								AuxException.Throw("Invalid JPX QCD marker segment");
								return false;
							}
							this.img.tiles[0].tileComps[0].nQuantSteps = (num4 - 3u) / 2u;
							uint[] array4 = new uint[this.img.tiles[0].tileComps[0].nQuantSteps];
							if (this.img.tiles[0].tileComps[0].quantSteps != null)
							{
								Array.Copy(this.img.tiles[0].tileComps[0].quantSteps, array4, this.img.tiles[0].tileComps[0].quantSteps.Length);
							}
							this.img.tiles[0].tileComps[0].quantSteps = array4;
							for (num8 = 0u; num8 < this.img.tiles[0].tileComps[0].nQuantSteps; num8 += 1u)
							{
								if (!this.readUWord(ref this.img.tiles[0].tileComps[0].quantSteps[(int)((UIntPtr)num8)]))
								{
									AuxException.Throw("Invalid JPX QCD marker segment.");
									return false;
								}
							}
						}
					}
					for (num8 = 0u; num8 < this.img.nXTiles * this.img.nYTiles; num8 += 1u)
					{
						for (num6 = 0u; num6 < this.img.nComps; num6 += 1u)
						{
							if (num8 != 0u || num6 != 0u)
							{
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantStyle = this.img.tiles[0].tileComps[0].quantStyle;
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nQuantSteps = this.img.tiles[0].tileComps[0].nQuantSteps;
								uint[] array5 = new uint[this.img.tiles[0].tileComps[0].nQuantSteps];
								if (this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps != null)
								{
									Array.Copy(this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps, array5, this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps.Length);
								}
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps = array5;
								for (uint num10 = 0u; num10 < this.img.tiles[0].tileComps[0].nQuantSteps; num10 += 1u)
								{
									this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps[(int)((UIntPtr)num10)] = this.img.tiles[0].tileComps[0].quantSteps[(int)((UIntPtr)num10)];
								}
							}
						}
					}
					flag3 = true;
					break;
				case 93:
					this.cover(24);
					if (!flag3)
					{
						AuxException.Throw("JPX QCC marker segment before QCD segment.");
						return false;
					}
					if ((this.img.nComps > 256u && !this.readUWord(ref num6)) || (this.img.nComps <= 256u && !this.readUByte(ref num6)) || num6 >= this.img.nComps || !this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantStyle))
					{
						AuxException.Throw("Invalid JPX QCC marker segment.");
						return false;
					}
					if ((this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantStyle & 31u) == 0u)
					{
						if (num4 <= ((this.img.nComps > 256u) ? 5u : 4u))
						{
							AuxException.Throw("Invalid JPX QCC marker segment.");
							return false;
						}
						this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps = (uint)((ulong)num4 - (ulong)((this.img.nComps > 256u) ? 5L : 4L));
						uint[] array6 = new uint[this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps];
						if (this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps != null)
						{
							Array.Copy(this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps, array6, this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps.Length);
						}
						this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps = array6;
						for (num8 = 0u; num8 < this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps; num8 += 1u)
						{
							if (!this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps[(int)((UIntPtr)num8)]))
							{
								AuxException.Throw("Invalid JPX QCC marker segment.");
								return false;
							}
						}
					}
					else
					{
						if ((this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantStyle & 31u) == 1u)
						{
							this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps = 1u;
							uint[] array7 = new uint[this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps];
							if (this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps != null)
							{
								Array.Copy(this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps, array7, this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps.Length);
							}
							this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps = array7;
							if (!this.readUWord(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps[0]))
							{
								AuxException.Throw("Invalid JPX QCC marker segment.");
								return false;
							}
						}
						else
						{
							if ((this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantStyle & 31u) != 2u)
							{
								AuxException.Throw("Invalid JPX QCC marker segment.");
								return false;
							}
							if (num4 < ((this.img.nComps > 256u) ? 5u : 4u) + 2u)
							{
								AuxException.Throw("Invalid JPX QCC marker segment.");
								return false;
							}
							this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps = (uint)(((ulong)num4 - (ulong)((this.img.nComps > 256u) ? 5L : 4L)) / 2uL);
							uint[] array8 = new uint[this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps];
							if (this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps != null)
							{
								Array.Copy(this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps, array8, this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps.Length);
							}
							this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps = array8;
							for (num8 = 0u; num8 < this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps; num8 += 1u)
							{
								if (!this.readUWord(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps[(int)((UIntPtr)num8)]))
								{
									AuxException.Throw("Invalid JPX QCD marker segment.");
									return false;
								}
							}
						}
					}
					for (num8 = 1u; num8 < this.img.nXTiles * this.img.nYTiles; num8 += 1u)
					{
						this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantStyle = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantStyle;
						this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nQuantSteps = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps;
						uint[] array9 = new uint[this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps];
						if (this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps != null)
						{
							Array.Copy(this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps, array9, this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps.Length);
						}
						this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps = array9;
						for (uint num10 = 0u; num10 < this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps; num10 += 1u)
						{
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps[(int)((UIntPtr)num10)] = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps[(int)((UIntPtr)num10)];
						}
					}
					break;
				case 94:
					this.cover(25);
					AuxException.Throw("got a JPX RGN segment.");
					for (num8 = 0u; num8 < num4 - 2u; num8 += 1u)
					{
						if (this.bufStr.getChar() == -1)
						{
							AuxException.Throw("Invalid JPX RGN marker segment.");
							return false;
						}
					}
					break;
				case 95:
					this.cover(26);
					AuxException.Throw("got a JPX POC segment.");
					for (num8 = 0u; num8 < num4 - 2u; num8 += 1u)
					{
						if (this.bufStr.getChar() == -1)
						{
							AuxException.Throw("Invalid JPX POC marker segment.");
							return false;
						}
					}
					break;
				case 96:
					this.cover(27);
					AuxException.Throw("Got a JPX PPM segment.");
					for (num8 = 0u; num8 < num4 - 2u; num8 += 1u)
					{
						if (this.bufStr.getChar() == -1)
						{
							AuxException.Throw("Invalid JPX PPM marker segment.");
							return false;
						}
					}
					break;
				case 99:
					this.cover(30);
					for (num8 = 0u; num8 < num4 - 2u; num8 += 1u)
					{
						if (this.bufStr.getChar() == -1)
						{
							AuxException.Throw("Invalid JPX CRG marker segment.");
							return false;
						}
					}
					break;
				case 100:
					this.cover(31);
					for (num8 = 0u; num8 < num4 - 2u; num8 += 1u)
					{
						if (this.bufStr.getChar() == -1)
						{
							AuxException.Throw("Invalid JPX COM marker segment.");
							return false;
						}
					}
					break;
				default:
					if (num7 != 144)
					{
						goto IL_21BC;
					}
					this.cover(32);
					flag4 = true;
					break;
				}
				IL_21E5:
				if (!flag4)
				{
					continue;
				}
				if (!flag2)
				{
					return false;
				}
				if (!flag)
				{
					AuxException.Throw("Missing COD marker segment in JPX stream.");
					return false;
				}
				if (!flag3)
				{
					AuxException.Throw("Missing QCD marker segment in JPX stream.");
					return false;
				}
				while (this.readTilePart())
				{
					if (!this.readMarkerHdr(ref num, ref num4))
					{
						AuxException.Throw("Invalid JPX codestream.");
						return false;
					}
					if (num != 144)
					{
						if (num != 217)
						{
							AuxException.Throw("Missing EOC marker in JPX codestream.");
							return false;
						}
						for (num8 = 0u; num8 < this.img.nXTiles * this.img.nYTiles; num8 += 1u)
						{
							JPXTile jPXTile = this.img.tiles[(int)((UIntPtr)num8)];
							if (!jPXTile.init)
							{
								AuxException.Throw("Uninitialized tile in JPX codestream.");
								return false;
							}
							for (num6 = 0u; num6 < this.img.nComps; num6 += 1u)
							{
								JPXTileComp tileComp = jPXTile.tileComps[(int)((UIntPtr)num6)];
								this.inverseTransform(tileComp);
							}
							if (!this.inverseMultiCompAndDC(jPXTile))
							{
								return false;
							}
						}
						return true;
					}
				}
				return false;
				IL_21BC:
				this.cover(33);
				num8 = 0u;
				while (num8 < num4 - 2u && this.bufStr.getChar() != -1)
				{
					num8 += 1u;
				}
				goto IL_21E5;
			}
			return false;
		}
		private bool readCodestream(uint len)
		{
			int num = 0;
			uint num2 = 0u;
			uint num3 = 0u;
			uint num4 = 0u;
			uint num5 = 0u;
			uint num6 = 0u;
			bool flag4;
			bool flag3;
			bool flag2;
			bool flag = flag2 = (flag3 = (flag4 = false));
			while (this.readMarkerHdr(ref num, ref num4))
			{
				int num7 = num;
				uint num8;
				switch (num7)
				{
				case 79:
					this.cover(19);
					break;
				case 80:
				case 84:
				case 86:
				case 88:
				case 89:
				case 90:
				case 91:
				case 97:
				case 98:
					goto IL_216E;
				case 81:
					this.cover(20);
					if (flag2)
					{
						AuxException.Throw("Duplicate SIZ marker segment in JPX stream.");
						return false;
					}
					if (!this.readUWord(ref num5) || !this.readULong(ref this.img.xSize) || !this.readULong(ref this.img.ySize) || !this.readULong(ref this.img.xOffset) || !this.readULong(ref this.img.yOffset) || !this.readULong(ref this.img.xTileSize) || !this.readULong(ref this.img.yTileSize) || !this.readULong(ref this.img.xTileOffset) || !this.readULong(ref this.img.yTileOffset) || !this.readUWord(ref this.img.nComps))
					{
						AuxException.Throw("Invalid JPX SIZ marker segment.");
						return false;
					}
					if (this.haveImgHdr && this.img.nComps != this.nComps)
					{
						AuxException.Throw("Different number of components in JPX SIZ marker segment.");
						return false;
					}
					if (this.img.xSize == 0u || this.img.ySize == 0u || this.img.xOffset >= this.img.xSize || this.img.yOffset >= this.img.ySize || this.img.xTileSize == 0u || this.img.yTileSize == 0u || this.img.xTileOffset > this.img.xOffset || this.img.yTileOffset > this.img.yOffset || this.img.xTileSize + this.img.xTileOffset <= this.img.xOffset || this.img.yTileSize + this.img.yTileOffset <= this.img.yOffset)
					{
						AuxException.Throw("Invalid JPX SIZ marker segment.");
						return false;
					}
					this.img.nXTiles = (this.img.xSize - this.img.xTileOffset + this.img.xTileSize - 1u) / this.img.xTileSize;
					this.img.nYTiles = (this.img.ySize - this.img.yTileOffset + this.img.yTileSize - 1u) / this.img.yTileSize;
					if (this.img.nXTiles <= 0u || this.img.nYTiles <= 0u || this.img.nXTiles >= 2147483647u / this.img.nYTiles)
					{
						AuxException.Throw("Bad tile count in JPX SIZ marker segment.");
						return false;
					}
					this.img.tiles = new JPXTile[this.img.nXTiles * this.img.nYTiles];
					for (num8 = 0u; num8 < this.img.nXTiles * this.img.nYTiles; num8 += 1u)
					{
						this.img.tiles[(int)((UIntPtr)num8)].init = false;
						this.img.tiles[(int)((UIntPtr)num8)].tileComps = new JPXTileComp[this.img.nComps];
						for (num6 = 0u; num6 < this.img.nComps; num6 += 1u)
						{
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps = null;
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].data = null;
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].buf = null;
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels = null;
						}
					}
					for (num6 = 0u; num6 < this.img.nComps; num6 += 1u)
					{
						if (!this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].prec) || !this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].hSep) || !this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].vSep))
						{
							AuxException.Throw("Invalid JPX SIZ marker segment.");
							return false;
						}
						if (this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].hSep == 0u || this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].vSep == 0u)
						{
							AuxException.Throw("Invalid JPX SIZ marker segment.");
							return false;
						}
						this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].sgned = ((this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].prec & 128u) != 0u);
						this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].prec = (this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].prec & 127u) + 1u;
						for (num8 = 1u; num8 < this.img.nXTiles * this.img.nYTiles; num8 += 1u)
						{
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)] = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].copy();
						}
					}
					flag2 = true;
					break;
				case 82:
					this.cover(21);
					if (!flag2)
					{
						AuxException.Throw("JPX COD marker segment before SIZ segment.");
						return false;
					}
					if (!this.readUByte(ref this.img.tiles[0].tileComps[0].style) || !this.readUByte(ref this.img.tiles[0].progOrder) || !this.readUWord(ref this.img.tiles[0].nLayers) || !this.readUByte(ref this.img.tiles[0].multiComp) || !this.readUByte(ref this.img.tiles[0].tileComps[0].nDecompLevels) || !this.readUByte(ref this.img.tiles[0].tileComps[0].codeBlockW) || !this.readUByte(ref this.img.tiles[0].tileComps[0].codeBlockH) || !this.readUByte(ref this.img.tiles[0].tileComps[0].codeBlockStyle) || !this.readUByte(ref this.img.tiles[0].tileComps[0].transform))
					{
						AuxException.Throw("Invalid JPX COD marker segment.");
						return false;
					}
					if (this.img.tiles[0].tileComps[0].nDecompLevels > 32u || this.img.tiles[0].tileComps[0].codeBlockW > 8u || this.img.tiles[0].tileComps[0].codeBlockH > 8u)
					{
						AuxException.Throw("Invalid JPX COD marker segment.");
						return false;
					}
					this.img.tiles[0].tileComps[0].codeBlockW += 2u;
					this.img.tiles[0].tileComps[0].codeBlockH += 2u;
					for (num8 = 0u; num8 < this.img.nXTiles * this.img.nYTiles; num8 += 1u)
					{
						if (num8 != 0u)
						{
							this.img.tiles[(int)((UIntPtr)num8)].progOrder = this.img.tiles[0].progOrder;
							this.img.tiles[(int)((UIntPtr)num8)].nLayers = this.img.tiles[0].nLayers;
							this.img.tiles[(int)((UIntPtr)num8)].multiComp = this.img.tiles[0].multiComp;
						}
						for (num6 = 0u; num6 < this.img.nComps; num6 += 1u)
						{
							if (num8 != 0u || num6 != 0u)
							{
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].style = this.img.tiles[0].tileComps[0].style;
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nDecompLevels = this.img.tiles[0].tileComps[0].nDecompLevels;
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].codeBlockW = this.img.tiles[0].tileComps[0].codeBlockW;
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].codeBlockH = this.img.tiles[0].tileComps[0].codeBlockH;
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].codeBlockStyle = this.img.tiles[0].tileComps[0].codeBlockStyle;
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].transform = this.img.tiles[0].tileComps[0].transform;
							}
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels = new JPXResLevel[this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nDecompLevels + 1u];
							for (uint num9 = 0u; num9 <= this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nDecompLevels; num9 += 1u)
							{
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precincts = null;
							}
						}
					}
					for (uint num9 = 0u; num9 <= this.img.tiles[0].tileComps[0].nDecompLevels; num9 += 1u)
					{
						if ((this.img.tiles[0].tileComps[0].style & 1u) != 0u)
						{
							this.cover(91);
							if (!this.readUByte(ref num2))
							{
								AuxException.Throw("Invalid JPX COD marker segment.");
								return false;
							}
							this.img.tiles[0].tileComps[0].resLevels[(int)((UIntPtr)num9)].precinctWidth = (num2 & 15u);
							this.img.tiles[0].tileComps[0].resLevels[(int)((UIntPtr)num9)].precinctHeight = (num2 >> 4 & 15u);
						}
						else
						{
							this.img.tiles[0].tileComps[0].resLevels[(int)((UIntPtr)num9)].precinctWidth = 15u;
							this.img.tiles[0].tileComps[0].resLevels[(int)((UIntPtr)num9)].precinctHeight = 15u;
						}
					}
					for (num8 = 0u; num8 < this.img.nXTiles * this.img.nYTiles; num8 += 1u)
					{
						for (num6 = 0u; num6 < this.img.nComps; num6 += 1u)
						{
							if (num8 != 0u || num6 != 0u)
							{
								for (uint num9 = 0u; num9 <= this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nDecompLevels; num9 += 1u)
								{
									this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctWidth = this.img.tiles[0].tileComps[0].resLevels[(int)((UIntPtr)num9)].precinctWidth;
									this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctHeight = this.img.tiles[0].tileComps[0].resLevels[(int)((UIntPtr)num9)].precinctHeight;
								}
							}
						}
					}
					flag = true;
					break;
				case 83:
					this.cover(22);
					if (!flag)
					{
						AuxException.Throw("JPX COC marker segment before COD segment.");
						return false;
					}
					if ((this.img.nComps > 256u && !this.readUWord(ref num6)) || (this.img.nComps <= 256u && !this.readUByte(ref num6)) || num6 >= this.img.nComps || !this.readUByte(ref num3) || !this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nDecompLevels) || !this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockW) || !this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockH) || !this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockStyle) || !this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].transform))
					{
						AuxException.Throw("Invalid JPX COC marker segment.");
						return false;
					}
					if (this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nDecompLevels > 32u || this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockW > 8u || this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockH > 8u)
					{
						AuxException.Throw("Invalid JPX COC marker segment.");
						return false;
					}
					this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].style = ((uint)((ulong)this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].style & 18446744073709551614uL) | (num3 & 1u));
					this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockW += 2u;
					this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockH += 2u;
					for (num8 = 0u; num8 < this.img.nXTiles * this.img.nYTiles; num8 += 1u)
					{
						if (num8 != 0u)
						{
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].style = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].style;
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nDecompLevels = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nDecompLevels;
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].codeBlockW = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockW;
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].codeBlockH = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockH;
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].codeBlockStyle = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].codeBlockStyle;
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].transform = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].transform;
						}
						JPXResLevel[] array = new JPXResLevel[this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nDecompLevels + 1u];
						if (this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels != null)
						{
							Array.Copy(this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels, array, this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels.Length);
						}
						this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels = array;
						for (uint num9 = 0u; num9 <= this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nDecompLevels; num9 += 1u)
						{
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precincts = null;
						}
					}
					for (uint num9 = 0u; num9 <= this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nDecompLevels; num9 += 1u)
					{
						if ((this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].style & 1u) != 0u)
						{
							if (!this.readUByte(ref num2))
							{
								AuxException.Throw("Invalid JPX COD marker segment.");
								return false;
							}
							this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctWidth = (num2 & 15u);
							this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctHeight = (num2 >> 4 & 15u);
						}
						else
						{
							this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctWidth = 15u;
							this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctHeight = 15u;
						}
					}
					for (num8 = 1u; num8 < this.img.nXTiles * this.img.nYTiles; num8 += 1u)
					{
						for (uint num9 = 0u; num9 <= this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nDecompLevels; num9 += 1u)
						{
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctWidth = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctWidth;
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctHeight = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].resLevels[(int)((UIntPtr)num9)].precinctHeight;
						}
					}
					break;
				case 85:
					this.cover(28);
					for (num8 = 0u; num8 < num4 - 2u; num8 += 1u)
					{
						if (this.bufStr.getChar() == -1)
						{
							AuxException.Throw("Invalid JPX TLM marker segment.");
							return false;
						}
					}
					break;
				case 87:
					this.cover(29);
					for (num8 = 0u; num8 < num4 - 2u; num8 += 1u)
					{
						if (this.bufStr.getChar() == -1)
						{
							AuxException.Throw("Invalid JPX PLM marker segment.");
							return false;
						}
					}
					break;
				case 92:
					this.cover(23);
					if (!flag2)
					{
						AuxException.Throw("JPX QCD marker segment before SIZ segment.");
						return false;
					}
					if (!this.readUByte(ref this.img.tiles[0].tileComps[0].quantStyle))
					{
						AuxException.Throw("Invalid JPX QCD marker segment.");
						return false;
					}
					if ((this.img.tiles[0].tileComps[0].quantStyle & 31u) == 0u)
					{
						if (num4 <= 3u)
						{
							AuxException.Throw("Invalid JPX QCD marker segment.");
							return false;
						}
						this.img.tiles[0].tileComps[0].nQuantSteps = num4 - 3u;
						uint[] array2 = new uint[this.img.tiles[0].tileComps[0].nQuantSteps];
						if (this.img.tiles[0].tileComps[0].quantSteps != null)
						{
							Array.Copy(this.img.tiles[0].tileComps[0].quantSteps, array2, this.img.tiles[0].tileComps[0].quantSteps.Length);
						}
						this.img.tiles[0].tileComps[0].quantSteps = array2;
						for (num8 = 0u; num8 < this.img.tiles[0].tileComps[0].nQuantSteps; num8 += 1u)
						{
							if (!this.readUByte(ref this.img.tiles[0].tileComps[0].quantSteps[(int)((UIntPtr)num8)]))
							{
								AuxException.Throw("Invalid JPX QCD marker segment.");
								return false;
							}
						}
					}
					else
					{
						if ((this.img.tiles[0].tileComps[0].quantStyle & 31u) == 1u)
						{
							this.img.tiles[0].tileComps[0].nQuantSteps = 1u;
							uint[] array3 = new uint[this.img.tiles[0].tileComps[0].nQuantSteps];
							if (this.img.tiles[0].tileComps[0].quantSteps != null)
							{
								Array.Copy(this.img.tiles[0].tileComps[0].quantSteps, array3, this.img.tiles[0].tileComps[0].quantSteps.Length);
							}
							this.img.tiles[0].tileComps[0].quantSteps = array3;
							if (!this.readUWord(ref this.img.tiles[0].tileComps[0].quantSteps[0]))
							{
								AuxException.Throw("Invalid JPX QCD marker segment.");
								return false;
							}
						}
						else
						{
							if ((this.img.tiles[0].tileComps[0].quantStyle & 31u) != 2u)
							{
								AuxException.Throw("Invalid JPX QCD marker segment.");
								return false;
							}
							if (num4 < 5u)
							{
								AuxException.Throw("Invalid JPX QCD marker segment.");
								return false;
							}
							this.img.tiles[0].tileComps[0].nQuantSteps = (num4 - 3u) / 2u;
							uint[] array4 = new uint[this.img.tiles[0].tileComps[0].nQuantSteps];
							if (this.img.tiles[0].tileComps[0].quantSteps != null)
							{
								Array.Copy(this.img.tiles[0].tileComps[0].quantSteps, array4, this.img.tiles[0].tileComps[0].quantSteps.Length);
							}
							this.img.tiles[0].tileComps[0].quantSteps = array4;
							for (num8 = 0u; num8 < this.img.tiles[0].tileComps[0].nQuantSteps; num8 += 1u)
							{
								if (!this.readUWord(ref this.img.tiles[0].tileComps[0].quantSteps[(int)((UIntPtr)num8)]))
								{
									AuxException.Throw("Invalid JPX QCD marker segment.");
									return false;
								}
							}
						}
					}
					for (num8 = 0u; num8 < this.img.nXTiles * this.img.nYTiles; num8 += 1u)
					{
						for (num6 = 0u; num6 < this.img.nComps; num6 += 1u)
						{
							if (num8 != 0u || num6 != 0u)
							{
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantStyle = this.img.tiles[0].tileComps[0].quantStyle;
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nQuantSteps = this.img.tiles[0].tileComps[0].nQuantSteps;
								uint[] array5 = new uint[this.img.tiles[0].tileComps[0].nQuantSteps];
								if (this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps != null)
								{
									Array.Copy(this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps, array5, this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps.Length);
								}
								this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps = array5;
								for (uint num10 = 0u; num10 < this.img.tiles[0].tileComps[0].nQuantSteps; num10 += 1u)
								{
									this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps[(int)((UIntPtr)num10)] = this.img.tiles[0].tileComps[0].quantSteps[(int)((UIntPtr)num10)];
								}
							}
						}
					}
					flag3 = true;
					break;
				case 93:
					this.cover(24);
					if (!flag3)
					{
						AuxException.Throw("JPX QCC marker segment before QCD segment.");
						return false;
					}
					if ((this.img.nComps > 256u && !this.readUWord(ref num6)) || (this.img.nComps <= 256u && !this.readUByte(ref num6)) || num6 >= this.img.nComps || !this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantStyle))
					{
						AuxException.Throw("Invalid JPX QCC marker segment.");
						return false;
					}
					if ((this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantStyle & 31u) == 0u)
					{
						if (num4 <= ((this.img.nComps > 256u) ? 5u : 4u))
						{
							AuxException.Throw("Invalid JPX QCC marker segment.");
							return false;
						}
						this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps = (uint)((ulong)num4 - (ulong)((this.img.nComps > 256u) ? 5L : 4L));
						uint[] array6 = new uint[this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps];
						if (this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps != null)
						{
							Array.Copy(this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps, array6, this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps.Length);
						}
						this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps = array6;
						for (num8 = 0u; num8 < this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps; num8 += 1u)
						{
							if (!this.readUByte(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps[(int)((UIntPtr)num8)]))
							{
								AuxException.Throw("Invalid JPX QCC marker segment.");
								return false;
							}
						}
					}
					else
					{
						if ((this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantStyle & 31u) == 1u)
						{
							this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps = 1u;
							uint[] array7 = new uint[this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps];
							if (this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps != null)
							{
								Array.Copy(this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps, array7, this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps.Length);
							}
							this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps = array7;
							if (!this.readUWord(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps[0]))
							{
								AuxException.Throw("Invalid JPX QCC marker segment.");
								return false;
							}
						}
						else
						{
							if ((this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantStyle & 31u) != 2u)
							{
								AuxException.Throw("Invalid JPX QCC marker segment.");
								return false;
							}
							if (num4 < ((this.img.nComps > 256u) ? 5u : 4u) + 2u)
							{
								AuxException.Throw("Invalid JPX QCC marker segment.");
								return false;
							}
							this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps = (uint)(((ulong)num4 - (ulong)((this.img.nComps > 256u) ? 5L : 4L)) / 2uL);
							uint[] array8 = new uint[this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps];
							if (this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps != null)
							{
								Array.Copy(this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps, array8, this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps.Length);
							}
							this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps = array8;
							for (num8 = 0u; num8 < this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps; num8 += 1u)
							{
								if (!this.readUWord(ref this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps[(int)((UIntPtr)num8)]))
								{
									AuxException.Throw("Invalid JPX QCD marker segment.");
									return false;
								}
							}
						}
					}
					for (num8 = 1u; num8 < this.img.nXTiles * this.img.nYTiles; num8 += 1u)
					{
						this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantStyle = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantStyle;
						this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].nQuantSteps = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps;
						uint[] array9 = new uint[this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps];
						if (this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps != null)
						{
							Array.Copy(this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps, array9, this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps.Length);
						}
						this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps = array9;
						for (uint num10 = 0u; num10 < this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].nQuantSteps; num10 += 1u)
						{
							this.img.tiles[(int)((UIntPtr)num8)].tileComps[(int)((UIntPtr)num6)].quantSteps[(int)((UIntPtr)num10)] = this.img.tiles[0].tileComps[(int)((UIntPtr)num6)].quantSteps[(int)((UIntPtr)num10)];
						}
					}
					break;
				case 94:
					this.cover(25);
					AuxException.Throw("got a JPX RGN segment.");
					for (num8 = 0u; num8 < num4 - 2u; num8 += 1u)
					{
						if (this.bufStr.getChar() == -1)
						{
							AuxException.Throw("Invalid JPX RGN marker segment.");
							return false;
						}
					}
					break;
				case 95:
					this.cover(26);
					AuxException.Throw("got a JPX POC segment.");
					for (num8 = 0u; num8 < num4 - 2u; num8 += 1u)
					{
						if (this.bufStr.getChar() == -1)
						{
							AuxException.Throw("Invalid JPX POC marker segment.");
							return false;
						}
					}
					break;
				case 96:
					this.cover(27);
					AuxException.Throw("Got a JPX PPM segment.");
					for (num8 = 0u; num8 < num4 - 2u; num8 += 1u)
					{
						if (this.bufStr.getChar() == -1)
						{
							AuxException.Throw("Invalid JPX PPM marker segment.");
							return false;
						}
					}
					break;
				case 99:
					this.cover(30);
					for (num8 = 0u; num8 < num4 - 2u; num8 += 1u)
					{
						if (this.bufStr.getChar() == -1)
						{
							AuxException.Throw("Invalid JPX CRG marker segment.");
							return false;
						}
					}
					break;
				case 100:
					this.cover(31);
					for (num8 = 0u; num8 < num4 - 2u; num8 += 1u)
					{
						if (this.bufStr.getChar() == -1)
						{
							AuxException.Throw("Invalid JPX COM marker segment.");
							return false;
						}
					}
					break;
				default:
					if (num7 != 144)
					{
						goto IL_216E;
					}
					this.cover(32);
					flag4 = true;
					break;
				}
				IL_21AC:
				if (!flag4)
				{
					continue;
				}
				if (!flag2)
				{
					return false;
				}
				if (!flag)
				{
					AuxException.Throw("Missing COD marker segment in JPX stream.");
					return false;
				}
				if (!flag3)
				{
					AuxException.Throw("Missing QCD marker segment in JPX stream.");
					return false;
				}
				while (this.readTilePart())
				{
					if (!this.readMarkerHdr(ref num, ref num4))
					{
						AuxException.Throw("Invalid JPX codestream.");
						return false;
					}
					if (num != 144)
					{
						if (num != 217)
						{
							return false;
						}
						for (num8 = 0u; num8 < this.img.nXTiles * this.img.nYTiles; num8 += 1u)
						{
							JPXTile jPXTile = this.img.tiles[(int)((UIntPtr)num8)];
							if (!jPXTile.init)
							{
								AuxException.Throw("Uninitialized tile in JPX codestream.");
								return false;
							}
							for (num6 = 0u; num6 < this.img.nComps; num6 += 1u)
							{
								JPXTileComp tileComp = jPXTile.tileComps[(int)((UIntPtr)num6)];
								this.inverseTransform(tileComp);
							}
							if (!this.inverseMultiCompAndDC(jPXTile))
							{
								return false;
							}
						}
						return true;
					}
				}
				return false;
				IL_216E:
				this.cover(33);
				AuxException.Throw(string.Format("Unknown marker segment {0:02x} in JPX stream.", num));
				num8 = 0u;
				while (num8 < num4 - 2u && this.bufStr.getChar() != -1)
				{
					num8 += 1u;
				}
				goto IL_21AC;
			}
			AuxException.Throw("Invalid JPX codestream.");
			return false;
		}
		private bool readTilePart()
		{
			uint num = 0u;
			uint num2 = 0u;
			uint num3 = 0u;
			uint num4 = 0u;
			uint num5 = 0u;
			uint num6 = 0u;
			uint num7 = 0u;
			uint num8 = 0u;
			int num9 = 0;
			if (!this.readUWord(ref num) || !this.readULong(ref num2) || !this.readUByte(ref num3) || !this.readUByte(ref num4))
			{
				AuxException.Throw("Invalid JPX SOT marker segment.");
				return false;
			}
			if ((num3 > 0u && !this.img.tiles[(int)((UIntPtr)num)].init) || num >= this.img.nXTiles * this.img.nYTiles)
			{
				AuxException.Throw("Weird tile index in JPX stream.");
				return false;
			}
			bool tilePartToEOC = num2 == 0u;
			num2 -= 12u;
			bool flag = false;
			while (this.readMarkerHdr(ref num9, ref num8))
			{
				num2 -= 2u + num8;
				int num10 = num9;
				switch (num10)
				{
				case 82:
					this.cover(34);
					if (!this.readUByte(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[0].style) || !this.readUByte(ref this.img.tiles[(int)((UIntPtr)num)].progOrder) || !this.readUWord(ref this.img.tiles[(int)((UIntPtr)num)].nLayers) || !this.readUByte(ref this.img.tiles[(int)((UIntPtr)num)].multiComp) || !this.readUByte(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[0].nDecompLevels) || !this.readUByte(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[0].codeBlockW) || !this.readUByte(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[0].codeBlockH) || !this.readUByte(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[0].codeBlockStyle) || !this.readUByte(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[0].transform))
					{
						AuxException.Throw("Invalid JPX COD marker segment.");
						return false;
					}
					if (this.img.tiles[(int)((UIntPtr)num)].tileComps[0].nDecompLevels > 32u || this.img.tiles[(int)((UIntPtr)num)].tileComps[0].codeBlockW > 8u || this.img.tiles[(int)((UIntPtr)num)].tileComps[0].codeBlockH > 8u)
					{
						AuxException.Throw("Invalid JPX COD marker segment.");
						return false;
					}
					this.img.tiles[(int)((UIntPtr)num)].tileComps[0].codeBlockW += 2u;
					this.img.tiles[(int)((UIntPtr)num)].tileComps[0].codeBlockH += 2u;
					for (num7 = 0u; num7 < this.img.nComps; num7 += 1u)
					{
						if (num7 != 0u)
						{
							this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].style = this.img.tiles[(int)((UIntPtr)num)].tileComps[0].style;
							this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].nDecompLevels = this.img.tiles[(int)((UIntPtr)num)].tileComps[0].nDecompLevels;
							this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].codeBlockW = this.img.tiles[(int)((UIntPtr)num)].tileComps[0].codeBlockW;
							this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].codeBlockH = this.img.tiles[(int)((UIntPtr)num)].tileComps[0].codeBlockH;
							this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].codeBlockStyle = this.img.tiles[(int)((UIntPtr)num)].tileComps[0].codeBlockStyle;
							this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].transform = this.img.tiles[(int)((UIntPtr)num)].tileComps[0].transform;
						}
						JPXResLevel[] array = new JPXResLevel[this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].nDecompLevels + 1u];
						if (this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].resLevels != null)
						{
							Array.Copy(this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].resLevels, array, this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].resLevels.Length);
						}
						this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].resLevels = array;
						for (uint num11 = 0u; num11 <= this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].nDecompLevels; num11 += 1u)
						{
							this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].resLevels[(int)((UIntPtr)num11)].precincts = null;
						}
					}
					for (uint num11 = 0u; num11 <= this.img.tiles[(int)((UIntPtr)num)].tileComps[0].nDecompLevels; num11 += 1u)
					{
						if ((this.img.tiles[(int)((UIntPtr)num)].tileComps[0].style & 1u) != 0u)
						{
							if (!this.readUByte(ref num5))
							{
								AuxException.Throw("Invalid JPX COD marker segment.");
								return false;
							}
							this.img.tiles[(int)((UIntPtr)num)].tileComps[0].resLevels[(int)((UIntPtr)num11)].precinctWidth = (num5 & 15u);
							this.img.tiles[(int)((UIntPtr)num)].tileComps[0].resLevels[(int)((UIntPtr)num11)].precinctHeight = (num5 >> 4 & 15u);
						}
						else
						{
							this.img.tiles[(int)((UIntPtr)num)].tileComps[0].resLevels[(int)((UIntPtr)num11)].precinctWidth = 15u;
							this.img.tiles[(int)((UIntPtr)num)].tileComps[0].resLevels[(int)((UIntPtr)num11)].precinctHeight = 15u;
						}
					}
					for (num7 = 1u; num7 < this.img.nComps; num7 += 1u)
					{
						for (uint num11 = 0u; num11 <= this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].nDecompLevels; num11 += 1u)
						{
							this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].resLevels[(int)((UIntPtr)num11)].precinctWidth = this.img.tiles[(int)((UIntPtr)num)].tileComps[0].resLevels[(int)((UIntPtr)num11)].precinctWidth;
							this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].resLevels[(int)((UIntPtr)num11)].precinctHeight = this.img.tiles[(int)((UIntPtr)num)].tileComps[0].resLevels[(int)((UIntPtr)num11)].precinctHeight;
						}
					}
					break;
				case 83:
				{
					this.cover(35);
					if ((this.img.nComps > 256u && !this.readUWord(ref num7)) || (this.img.nComps <= 256u && !this.readUByte(ref num7)) || num7 >= this.img.nComps || !this.readUByte(ref num6) || !this.readUByte(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].nDecompLevels) || !this.readUByte(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].codeBlockW) || !this.readUByte(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].codeBlockH) || !this.readUByte(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].codeBlockStyle) || !this.readUByte(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].transform))
					{
						AuxException.Throw("Invalid JPX COC marker segment.");
						return false;
					}
					if (this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].nDecompLevels > 32u || this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].codeBlockW > 8u || this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].codeBlockH > 8u)
					{
						AuxException.Throw("Invalid JPX COD marker segment.");
						return false;
					}
					this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].style = (uint)(((ulong)this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].style & 18446744073709551614uL) | (ulong)(num6 & 1u));
					this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].codeBlockW += 2u;
					this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].codeBlockH += 2u;
					JPXResLevel[] array2 = new JPXResLevel[this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].nDecompLevels + 1u];
					if (this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].resLevels != null)
					{
						Array.Copy(this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].resLevels, array2, this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].resLevels.Length);
					}
					this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].resLevels = array2;
					for (uint num11 = 0u; num11 <= this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].nDecompLevels; num11 += 1u)
					{
						this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].resLevels[(int)((UIntPtr)num11)].precincts = null;
					}
					for (uint num11 = 0u; num11 <= this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].nDecompLevels; num11 += 1u)
					{
						if ((this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].style & 1u) != 0u)
						{
							if (!this.readUByte(ref num5))
							{
								AuxException.Throw("Invalid JPX COD marker segment.");
								return false;
							}
							this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].resLevels[(int)((UIntPtr)num11)].precinctWidth = (num5 & 15u);
							this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].resLevels[(int)((UIntPtr)num11)].precinctHeight = (num5 >> 4 & 15u);
						}
						else
						{
							this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].resLevels[(int)((UIntPtr)num11)].precinctWidth = 15u;
							this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].resLevels[(int)((UIntPtr)num11)].precinctHeight = 15u;
						}
					}
					break;
				}
				default:
				{
					uint num12;
					switch (num10)
					{
					case 88:
						this.cover(41);
						for (num12 = 0u; num12 < num8 - 2u; num12 += 1u)
						{
							if (this.bufStr.getChar() == -1)
							{
								AuxException.Throw("Invalid JPX PLT marker segment.");
								return false;
							}
						}
						goto IL_17C5;
					case 89:
					case 90:
					case 91:
					case 96:
					case 98:
					case 99:
						break;
					case 92:
						this.cover(36);
						if (!this.readUByte(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantStyle))
						{
							AuxException.Throw("Invalid JPX QCD marker segment.");
							return false;
						}
						if ((this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantStyle & 31u) == 0u)
						{
							if (num8 <= 3u)
							{
								AuxException.Throw("Invalid JPX QCD marker segment.");
								return false;
							}
							this.img.tiles[(int)((UIntPtr)num)].tileComps[0].nQuantSteps = num8 - 3u;
							uint[] array3 = new uint[this.img.tiles[(int)((UIntPtr)num)].tileComps[0].nQuantSteps];
							if (this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps != null)
							{
								Array.Copy(this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps, array3, this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps.Length);
							}
							this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps = array3;
							for (num12 = 0u; num12 < this.img.tiles[(int)((UIntPtr)num)].tileComps[0].nQuantSteps; num12 += 1u)
							{
								if (!this.readUByte(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps[(int)((UIntPtr)num12)]))
								{
									AuxException.Throw("Invalid JPX QCD marker segment.");
									return false;
								}
							}
						}
						else
						{
							if ((this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantStyle & 31u) == 1u)
							{
								this.img.tiles[(int)((UIntPtr)num)].tileComps[0].nQuantSteps = 1u;
								uint[] array4 = new uint[this.img.tiles[(int)((UIntPtr)num)].tileComps[0].nQuantSteps];
								if (this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps != null)
								{
									Array.Copy(this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps, array4, this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps.Length);
								}
								this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps = array4;
								if (!this.readUWord(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps[0]))
								{
									AuxException.Throw("Invalid JPX QCD marker segment.");
									return false;
								}
							}
							else
							{
								if ((this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantStyle & 31u) != 2u)
								{
									AuxException.Throw("Invalid JPX QCD marker segment.");
									return false;
								}
								if (num8 < 5u)
								{
									AuxException.Throw("Invalid JPX QCD marker segment.");
									return false;
								}
								this.img.tiles[(int)((UIntPtr)num)].tileComps[0].nQuantSteps = (num8 - 3u) / 2u;
								uint[] array5 = new uint[this.img.tiles[(int)((UIntPtr)num)].tileComps[0].nQuantSteps];
								if (this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps != null)
								{
									Array.Copy(this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps, array5, this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps.Length);
								}
								this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps = array5;
								for (num12 = 0u; num12 < this.img.tiles[(int)((UIntPtr)num)].tileComps[0].nQuantSteps; num12 += 1u)
								{
									if (!this.readUWord(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps[(int)((UIntPtr)num12)]))
									{
										AuxException.Throw("Invalid JPX QCD marker segment.");
										return false;
									}
								}
							}
						}
						for (num7 = 1u; num7 < this.img.nComps; num7 += 1u)
						{
							this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantStyle = this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantStyle;
							this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].nQuantSteps = this.img.tiles[(int)((UIntPtr)num)].tileComps[0].nQuantSteps;
							uint[] array6 = new uint[this.img.tiles[(int)((UIntPtr)num)].tileComps[0].nQuantSteps];
							if (this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps != null)
							{
								Array.Copy(this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps, array6, this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps.Length);
							}
							this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps = array6;
							for (uint num13 = 0u; num13 < this.img.tiles[(int)((UIntPtr)num)].tileComps[0].nQuantSteps; num13 += 1u)
							{
								this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantSteps[(int)((UIntPtr)num13)] = this.img.tiles[(int)((UIntPtr)num)].tileComps[0].quantSteps[(int)((UIntPtr)num13)];
							}
						}
						goto IL_17C5;
					case 93:
						this.cover(37);
						if ((this.img.nComps > 256u && !this.readUWord(ref num7)) || (this.img.nComps <= 256u && !this.readUByte(ref num7)) || num7 >= this.img.nComps || !this.readUByte(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantStyle))
						{
							AuxException.Throw("Invalid JPX QCC marker segment.");
							return false;
						}
						if ((this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantStyle & 31u) == 0u)
						{
							if (num8 <= ((this.img.nComps > 256u) ? 5u : 4u))
							{
								AuxException.Throw("Invalid JPX QCC marker segment.");
								return false;
							}
							this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].nQuantSteps = (uint)((ulong)num8 - (ulong)((this.img.nComps > 256u) ? 5L : 4L));
							uint[] array7 = new uint[this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].nQuantSteps];
							if (this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantSteps != null)
							{
								Array.Copy(this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantSteps, array7, this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantSteps.Length);
							}
							this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantSteps = array7;
							for (num12 = 0u; num12 < this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].nQuantSteps; num12 += 1u)
							{
								if (!this.readUByte(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantSteps[(int)((UIntPtr)num12)]))
								{
									AuxException.Throw("Invalid JPX QCC marker segment.");
									return false;
								}
							}
							goto IL_17C5;
						}
						else
						{
							if ((this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantStyle & 31u) == 1u)
							{
								this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].nQuantSteps = 1u;
								uint[] array8 = new uint[this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].nQuantSteps];
								if (this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantSteps != null)
								{
									Array.Copy(this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantSteps, array8, this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantSteps.Length);
								}
								this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantSteps = array8;
								if (!this.readUWord(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantSteps[0]))
								{
									AuxException.Throw("Invalid JPX QCC marker segment.");
									return false;
								}
								goto IL_17C5;
							}
							else
							{
								if ((this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantStyle & 31u) != 2u)
								{
									AuxException.Throw("Invalid JPX QCC marker segment.");
									return false;
								}
								if (num8 < ((this.img.nComps > 256u) ? 5u : 4u) + 2u)
								{
									AuxException.Throw("Invalid JPX QCC marker segment.");
									return false;
								}
								this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].nQuantSteps = (uint)(((ulong)num8 - (ulong)((this.img.nComps > 256u) ? 5L : 4L)) / 2uL);
								uint[] array9 = new uint[this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].nQuantSteps];
								if (this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantSteps != null)
								{
									Array.Copy(this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantSteps, array9, this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantSteps.Length);
								}
								this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantSteps = array9;
								for (num12 = 0u; num12 < this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].nQuantSteps; num12 += 1u)
								{
									if (!this.readUWord(ref this.img.tiles[(int)((UIntPtr)num)].tileComps[(int)((UIntPtr)num7)].quantSteps[(int)((UIntPtr)num12)]))
									{
										AuxException.Throw("Invalid JPX QCD marker segment.");
										return false;
									}
								}
								goto IL_17C5;
							}
						}
						break;
					case 94:
						this.cover(38);
						AuxException.Throw("Got a JPX RGN segment.");
						for (num12 = 0u; num12 < num8 - 2u; num12 += 1u)
						{
							if (this.bufStr.getChar() == -1)
							{
								AuxException.Throw("Invalid JPX RGN marker segment.");
								return false;
							}
						}
						goto IL_17C5;
					case 95:
						this.cover(39);
						AuxException.Throw("Got a JPX POC segment.");
						for (num12 = 0u; num12 < num8 - 2u; num12 += 1u)
						{
							if (this.bufStr.getChar() == -1)
							{
								AuxException.Throw("Invalid JPX POC marker segment.");
								return false;
							}
						}
						goto IL_17C5;
					case 97:
						this.cover(40);
						AuxException.Throw("Got a JPX PPT segment.");
						for (num12 = 0u; num12 < num8 - 2u; num12 += 1u)
						{
							if (this.bufStr.getChar() == -1)
							{
								AuxException.Throw("Invalid JPX PPT marker segment.");
								return false;
							}
						}
						goto IL_17C5;
					case 100:
						this.cover(42);
						for (num12 = 0u; num12 < num8 - 2u; num12 += 1u)
						{
							if (this.bufStr.getChar() == -1)
							{
								AuxException.Throw("Invalid JPX COM marker segment.");
								return false;
							}
						}
						goto IL_17C5;
					default:
						if (num10 == 147)
						{
							this.cover(43);
							flag = true;
							goto IL_17C5;
						}
						break;
					}
					this.cover(44);
					AuxException.Throw(string.Format("Unknown marker segment {0:02x} in JPX tile-part stream", num9));
					num12 = 0u;
					while (num12 < num8 - 2u && this.bufStr.getChar() != -1)
					{
						num12 += 1u;
					}
					break;
				}
				}
				IL_17C5:
				if (flag)
				{
					if (num3 == 0u)
					{
						JPXTile jPXTile = this.img.tiles[(int)((UIntPtr)num)];
						jPXTile.init = true;
						uint num12 = num / this.img.nXTiles;
						uint num13 = num % this.img.nXTiles;
						if ((jPXTile.x0 = this.img.xTileOffset + num13 * this.img.xTileSize) < this.img.xOffset)
						{
							jPXTile.x0 = this.img.xOffset;
						}
						if ((jPXTile.y0 = this.img.yTileOffset + num12 * this.img.yTileSize) < this.img.yOffset)
						{
							jPXTile.y0 = this.img.yOffset;
						}
						if ((jPXTile.x1 = this.img.xTileOffset + (num13 + 1u) * this.img.xTileSize) > this.img.xSize)
						{
							jPXTile.x1 = this.img.xSize;
						}
						if ((jPXTile.y1 = this.img.yTileOffset + (num12 + 1u) * this.img.yTileSize) > this.img.ySize)
						{
							jPXTile.y1 = this.img.ySize;
						}
						jPXTile.comp = 0u;
						jPXTile.res = 0u;
						jPXTile.precinct = 0u;
						jPXTile.layer = 0u;
						jPXTile.maxNDecompLevels = 0u;
						for (num7 = 0u; num7 < this.img.nComps; num7 += 1u)
						{
							JPXTileComp jPXTileComp = jPXTile.tileComps[(int)((UIntPtr)num7)];
							if (jPXTileComp.nDecompLevels > jPXTile.maxNDecompLevels)
							{
								jPXTile.maxNDecompLevels = jPXTileComp.nDecompLevels;
							}
							jPXTileComp.x0 = this.jpxCeilDiv(jPXTile.x0, jPXTileComp.hSep);
							jPXTileComp.y0 = this.jpxCeilDiv(jPXTile.y0, jPXTileComp.vSep);
							jPXTileComp.x1 = this.jpxCeilDiv(jPXTile.x1, jPXTileComp.hSep);
							jPXTileComp.y1 = this.jpxCeilDiv(jPXTile.y1, jPXTileComp.vSep);
							jPXTileComp.w = jPXTileComp.x1 - jPXTileComp.x0;
							jPXTileComp.cbW = 1u << (int)jPXTileComp.codeBlockW;
							jPXTileComp.cbH = 1u << (int)jPXTileComp.codeBlockH;
							jPXTileComp.data = new int[(jPXTileComp.x1 - jPXTileComp.x0) * (jPXTileComp.y1 - jPXTileComp.y0)];
							uint num14;
							if (jPXTileComp.x1 - jPXTileComp.x0 > jPXTileComp.y1 - jPXTileComp.y0)
							{
								num14 = jPXTileComp.x1 - jPXTileComp.x0;
							}
							else
							{
								num14 = jPXTileComp.y1 - jPXTileComp.y0;
							}
							jPXTileComp.buf = new int[num14 + 8u];
							for (uint num11 = 0u; num11 <= jPXTileComp.nDecompLevels; num11 += 1u)
							{
								JPXResLevel jPXResLevel = jPXTileComp.resLevels[(int)((UIntPtr)num11)];
								uint num15 = (num11 == 0u) ? jPXTileComp.nDecompLevels : (jPXTileComp.nDecompLevels - num11 + 1u);
								jPXResLevel.x0 = this.jpxCeilDivPow2(jPXTileComp.x0, num15);
								jPXResLevel.y0 = this.jpxCeilDivPow2(jPXTileComp.y0, num15);
								jPXResLevel.x1 = this.jpxCeilDivPow2(jPXTileComp.x1, num15);
								jPXResLevel.y1 = this.jpxCeilDivPow2(jPXTileComp.y1, num15);
								if (num11 == 0u)
								{
									jPXResLevel.bx0[0] = jPXResLevel.x0;
									jPXResLevel.by0[0] = jPXResLevel.y0;
									jPXResLevel.bx1[0] = jPXResLevel.x1;
									jPXResLevel.by1[0] = jPXResLevel.y1;
								}
								else
								{
									jPXResLevel.bx0[0] = this.jpxCeilDivPow2((int)((ulong)jPXTileComp.x0 - (ulong)(1L << (int)(num15 - 1u & 31u))), (int)num15);
									jPXResLevel.by0[0] = jPXResLevel.y0;
									jPXResLevel.bx1[0] = this.jpxCeilDivPow2((int)((ulong)jPXTileComp.x1 - (ulong)(1L << (int)(num15 - 1u & 31u))), (int)num15);
									jPXResLevel.by1[0] = jPXResLevel.y1;
									jPXResLevel.bx0[1] = jPXResLevel.x0;
									jPXResLevel.by0[1] = this.jpxCeilDivPow2((int)((ulong)jPXTileComp.y0 - (ulong)(1L << (int)(num15 - 1u & 31u))), (int)num15);
									jPXResLevel.bx1[1] = jPXResLevel.x1;
									jPXResLevel.by1[1] = this.jpxCeilDivPow2((int)((ulong)jPXTileComp.y1 - (ulong)(1L << (int)(num15 - 1u & 31u))), (int)num15);
									jPXResLevel.bx0[2] = this.jpxCeilDivPow2((int)((ulong)jPXTileComp.x0 - (ulong)(1L << (int)(num15 - 1u & 31u))), (int)num15);
									jPXResLevel.by0[2] = this.jpxCeilDivPow2((int)((ulong)jPXTileComp.y0 - (ulong)(1L << (int)(num15 - 1u & 31u))), (int)num15);
									jPXResLevel.bx1[2] = this.jpxCeilDivPow2((int)((ulong)jPXTileComp.x1 - (ulong)(1L << (int)(num15 - 1u & 31u))), (int)num15);
									jPXResLevel.by1[2] = this.jpxCeilDivPow2((int)((ulong)jPXTileComp.y1 - (ulong)(1L << (int)(num15 - 1u & 31u))), (int)num15);
								}
								jPXResLevel.precincts = new JPXPrecinct[1];
								for (uint num16 = 0u; num16 < 1u; num16 += 1u)
								{
									jPXResLevel.precincts[(int)((UIntPtr)num16)] = new JPXPrecinct();
									JPXPrecinct jPXPrecinct = jPXResLevel.precincts[(int)((UIntPtr)num16)];
									jPXPrecinct.x0 = jPXResLevel.x0;
									jPXPrecinct.y0 = jPXResLevel.y0;
									jPXPrecinct.x1 = jPXResLevel.x1;
									jPXPrecinct.y1 = jPXResLevel.y1;
									uint num17 = (num11 == 0u) ? 1u : 3u;
									jPXPrecinct.subbands = new JPXSubband[num17];
									for (uint num18 = 0u; num18 < num17; num18 += 1u)
									{
										jPXPrecinct.subbands[(int)((UIntPtr)num18)] = new JPXSubband();
										JPXSubband jPXSubband = jPXPrecinct.subbands[(int)((UIntPtr)num18)];
										jPXSubband.x0 = jPXResLevel.bx0[(int)((UIntPtr)num18)];
										jPXSubband.y0 = jPXResLevel.by0[(int)((UIntPtr)num18)];
										jPXSubband.x1 = jPXResLevel.bx1[(int)((UIntPtr)num18)];
										jPXSubband.y1 = jPXResLevel.by1[(int)((UIntPtr)num18)];
										jPXSubband.nXCBs = this.jpxCeilDivPow2(jPXSubband.x1, jPXTileComp.codeBlockW) - this.jpxFloorDivPow2(jPXSubband.x0, jPXTileComp.codeBlockW);
										jPXSubband.nYCBs = this.jpxCeilDivPow2(jPXSubband.y1, jPXTileComp.codeBlockH) - this.jpxFloorDivPow2(jPXSubband.y0, jPXTileComp.codeBlockH);
										num14 = ((jPXSubband.nXCBs > jPXSubband.nYCBs) ? jPXSubband.nXCBs : jPXSubband.nYCBs);
										jPXSubband.maxTTLevel = 0u;
										for (num14 -= 1u; num14 != 0u; num14 >>= 1)
										{
											jPXSubband.maxTTLevel += 1u;
										}
										num14 = 0u;
										for (int i = (int)jPXSubband.maxTTLevel; i >= 0; i--)
										{
											uint num19 = this.jpxCeilDivPow2(jPXSubband.nXCBs, (uint)i);
											uint num20 = this.jpxCeilDivPow2(jPXSubband.nYCBs, (uint)i);
											num14 += num19 * num20;
										}
										jPXSubband.inclusion = new JPXTagTreeNode[num14];
										jPXSubband.zeroBitPlane = new JPXTagTreeNode[num14];
										for (num15 = 0u; num15 < num14; num15 += 1u)
										{
											jPXSubband.inclusion[(int)((UIntPtr)num15)] = new JPXTagTreeNode();
											jPXSubband.zeroBitPlane[(int)((UIntPtr)num15)] = new JPXTagTreeNode();
											jPXSubband.inclusion[(int)((UIntPtr)num15)].finished = false;
											jPXSubband.inclusion[(int)((UIntPtr)num15)].val = 0u;
											jPXSubband.zeroBitPlane[(int)((UIntPtr)num15)].finished = false;
											jPXSubband.zeroBitPlane[(int)((UIntPtr)num15)].val = 0u;
										}
										jPXSubband.cbs = new JPXCodeBlock[jPXSubband.nXCBs * jPXSubband.nYCBs];
										for (int j = 0; j < jPXSubband.cbs.Length; j++)
										{
											jPXSubband.cbs[j] = new JPXCodeBlock();
										}
										uint num21 = this.jpxFloorDivPow2(jPXSubband.x0, jPXTileComp.codeBlockW);
										uint num22 = this.jpxFloorDivPow2(jPXSubband.y0, jPXTileComp.codeBlockH);
										RavenPtr<int> p;
										if (num11 == 0u)
										{
											p = new RavenPtr<int>(jPXTileComp.data);
										}
										else
										{
											if (num18 == 0u)
											{
												p = new RavenPtr<int>(jPXTileComp.data, (int)(jPXResLevel.bx1[1] - jPXResLevel.bx0[1]));
											}
											else
											{
												if (num18 == 1u)
												{
													p = new RavenPtr<int>(jPXTileComp.data, (int)((jPXResLevel.by1[0] - jPXResLevel.by0[0]) * jPXTileComp.w));
												}
												else
												{
													p = new RavenPtr<int>(jPXTileComp.data, (int)((jPXResLevel.by1[0] - jPXResLevel.by0[0]) * jPXTileComp.w + jPXResLevel.bx1[1] - jPXResLevel.bx0[1]));
												}
											}
										}
										int num23 = 0;
										JPXCodeBlock jPXCodeBlock = (jPXSubband.cbs.Length > 0) ? jPXSubband.cbs[num23] : null;
										for (uint num24 = 0u; num24 < jPXSubband.nYCBs; num24 += 1u)
										{
											for (uint num25 = 0u; num25 < jPXSubband.nXCBs; num25 += 1u)
											{
												jPXCodeBlock.x0 = num21 + num25 << (int)jPXTileComp.codeBlockW;
												jPXCodeBlock.x1 = jPXCodeBlock.x0 + jPXTileComp.cbW;
												if (jPXSubband.x0 > jPXCodeBlock.x0)
												{
													jPXCodeBlock.x0 = jPXSubband.x0;
												}
												if (jPXSubband.x1 < jPXCodeBlock.x1)
												{
													jPXCodeBlock.x1 = jPXSubband.x1;
												}
												jPXCodeBlock.y0 = num22 + num24 << (int)jPXTileComp.codeBlockH;
												jPXCodeBlock.y1 = jPXCodeBlock.y0 + jPXTileComp.cbH;
												if (jPXSubband.y0 > jPXCodeBlock.y0)
												{
													jPXCodeBlock.y0 = jPXSubband.y0;
												}
												if (jPXSubband.y1 < jPXCodeBlock.y1)
												{
													jPXCodeBlock.y1 = jPXSubband.y1;
												}
												jPXCodeBlock.seen = false;
												jPXCodeBlock.lBlock = 3u;
												jPXCodeBlock.nextPass = 2u;
												jPXCodeBlock.nZeroBitPlanes = 0u;
												jPXCodeBlock.dataLenSize = 1u;
												jPXCodeBlock.dataLen = new uint[1];
												jPXCodeBlock.coeffs = new RavenPtr<int>(p, (int)((jPXCodeBlock.y0 - jPXSubband.y0) * jPXTileComp.w + (jPXCodeBlock.x0 - jPXSubband.x0)));
												jPXCodeBlock.touched = new char[1 << (int)(jPXTileComp.codeBlockW + jPXTileComp.codeBlockH)];
												jPXCodeBlock.len = 0;
												for (uint num26 = 0u; num26 < jPXCodeBlock.y1 - jPXCodeBlock.y0; num26 += 1u)
												{
													for (uint num27 = 0u; num27 < jPXCodeBlock.x1 - jPXCodeBlock.x0; num27 += 1u)
													{
														jPXCodeBlock.coeffs[(int)(num26 * jPXTileComp.w + num27)] = 0;
													}
												}
												jPXCodeBlock.arithDecoder = null;
												jPXCodeBlock.stats = null;
												num23++;
												jPXCodeBlock = ((num23 < jPXSubband.cbs.Length) ? jPXSubband.cbs[num23] : null);
											}
										}
									}
								}
							}
						}
					}
					return this.readTilePartData(num, num2, tilePartToEOC);
				}
			}
			AuxException.Throw("Invalid JPX tile-part codestream.");
			return false;
		}
		private bool readTilePartData(uint tileIdx, uint tilePartLen, bool tilePartToEOC)
		{
			uint num = 0u;
			JPXTile jPXTile = this.img.tiles[(int)((UIntPtr)tileIdx)];
			while (true)
			{
				if (tilePartToEOC)
				{
					this.cover(93);
				}
				else
				{
					if (tilePartLen == 0u)
					{
						return true;
					}
				}
				JPXTileComp jPXTileComp = jPXTile.tileComps[(int)((UIntPtr)jPXTile.comp)];
				JPXResLevel jPXResLevel = jPXTileComp.resLevels[(int)((UIntPtr)jPXTile.res)];
				JPXPrecinct jPXPrecinct = jPXResLevel.precincts[(int)((UIntPtr)jPXTile.precinct)];
				this.startBitBuf(tilePartLen);
				if ((jPXTileComp.style & 2u) != 0u)
				{
					this.skipSOP();
				}
				if (!this.readBits(1, ref num))
				{
					break;
				}
				if (num == 0u)
				{
					this.cover(45);
					for (uint num2 = 0u; num2 < ((jPXTile.res == 0u) ? 1u : 3u); num2 += 1u)
					{
						JPXSubband jPXSubband = jPXPrecinct.subbands[(int)((UIntPtr)num2)];
						for (uint num3 = 0u; num3 < jPXSubband.nYCBs; num3 += 1u)
						{
							for (uint num4 = 0u; num4 < jPXSubband.nXCBs; num4 += 1u)
							{
								JPXCodeBlock jPXCodeBlock = jPXSubband.cbs[(int)((UIntPtr)(num3 * jPXSubband.nXCBs + num4))];
								jPXCodeBlock.included = 0u;
							}
						}
					}
				}
				else
				{
					for (uint num2 = 0u; num2 < ((jPXTile.res == 0u) ? 1u : 3u); num2 += 1u)
					{
						JPXSubband jPXSubband = jPXPrecinct.subbands[(int)((UIntPtr)num2)];
						for (uint num3 = 0u; num3 < jPXSubband.nYCBs; num3 += 1u)
						{
							for (uint num4 = 0u; num4 < jPXSubband.nXCBs; num4 += 1u)
							{
								JPXCodeBlock jPXCodeBlock = jPXSubband.cbs[(int)((UIntPtr)(num3 * jPXSubband.nXCBs + num4))];
								if (jPXCodeBlock.x0 >= jPXCodeBlock.x1 || jPXCodeBlock.y0 >= jPXCodeBlock.y1)
								{
									this.cover(46);
									jPXCodeBlock.included = 0u;
								}
								else
								{
									if (jPXCodeBlock.seen)
									{
										this.cover(47);
										if (!this.readBits(1, ref jPXCodeBlock.included))
										{
											goto Block_11;
										}
									}
									else
									{
										this.cover(48);
										uint num5 = 0u;
										uint num6 = 0u;
										int i;
										for (i = (int)jPXSubband.maxTTLevel; i >= 0; i--)
										{
											uint num7 = this.jpxCeilDivPow2(jPXSubband.nXCBs, (uint)i);
											uint num8 = this.jpxCeilDivPow2(jPXSubband.nYCBs, (uint)i);
											uint num9 = num6 + (num3 >> i) * num7 + (num4 >> i);
											if (!jPXSubband.inclusion[(int)((UIntPtr)num9)].finished && jPXSubband.inclusion[(int)((UIntPtr)num9)].val == 0u)
											{
												jPXSubband.inclusion[(int)((UIntPtr)num9)].val = num5;
											}
											else
											{
												num5 = jPXSubband.inclusion[(int)((UIntPtr)num9)].val;
											}
											while (!jPXSubband.inclusion[(int)((UIntPtr)num9)].finished && num5 <= jPXTile.layer)
											{
												if (!this.readBits(1, ref num))
												{
													goto IL_9C2;
												}
												if (num == 1u)
												{
													jPXSubband.inclusion[(int)((UIntPtr)num9)].finished = true;
												}
												else
												{
													num5 += 1u;
												}
											}
											jPXSubband.inclusion[(int)((UIntPtr)num9)].val = num5;
											if (num5 > jPXTile.layer)
											{
												break;
											}
											num6 += num7 * num8;
										}
										jPXCodeBlock.included = ((i < 0) ? 1u : 0u);
									}
									if (jPXCodeBlock.included != 0u)
									{
										this.cover(49);
										if (!jPXCodeBlock.seen)
										{
											this.cover(50);
											uint num5 = 0u;
											uint num6 = 0u;
											for (int i = (int)jPXSubband.maxTTLevel; i >= 0; i--)
											{
												uint num7 = this.jpxCeilDivPow2(jPXSubband.nXCBs, (uint)i);
												uint num8 = this.jpxCeilDivPow2(jPXSubband.nYCBs, (uint)i);
												uint num9 = num6 + (num3 >> i) * num7 + (num4 >> i);
												if (!jPXSubband.zeroBitPlane[(int)((UIntPtr)num9)].finished && jPXSubband.zeroBitPlane[(int)((UIntPtr)num9)].val == 0u)
												{
													jPXSubband.zeroBitPlane[(int)((UIntPtr)num9)].val = num5;
												}
												else
												{
													num5 = jPXSubband.zeroBitPlane[(int)((UIntPtr)num9)].val;
												}
												while (!jPXSubband.zeroBitPlane[(int)((UIntPtr)num9)].finished)
												{
													if (!this.readBits(1, ref num))
													{
														goto IL_9C2;
													}
													if (num == 1u)
													{
														jPXSubband.zeroBitPlane[(int)((UIntPtr)num9)].finished = true;
													}
													else
													{
														num5 += 1u;
													}
												}
												jPXSubband.zeroBitPlane[(int)((UIntPtr)num9)].val = num5;
												num6 += num7 * num8;
											}
											jPXCodeBlock.nZeroBitPlanes = num5;
										}
										if (this.readBits(1, ref num))
										{
											if (num == 0u)
											{
												this.cover(51);
												jPXCodeBlock.nCodingPasses = 1u;
											}
											else
											{
												if (!this.readBits(1, ref num))
												{
													goto IL_9C2;
												}
												if (num == 0u)
												{
													this.cover(52);
													jPXCodeBlock.nCodingPasses = 2u;
												}
												else
												{
													this.cover(53);
													if (!this.readBits(2, ref num))
													{
														goto IL_9C2;
													}
													if (num < 3u)
													{
														this.cover(54);
														jPXCodeBlock.nCodingPasses = 3u + num;
													}
													else
													{
														this.cover(55);
														if (!this.readBits(5, ref num))
														{
															goto IL_9C2;
														}
														if (num < 31u)
														{
															this.cover(56);
															jPXCodeBlock.nCodingPasses = 6u + num;
														}
														else
														{
															this.cover(57);
															if (!this.readBits(7, ref num))
															{
																goto IL_9C2;
															}
															jPXCodeBlock.nCodingPasses = 37u + num;
														}
													}
												}
											}
											while (this.readBits(1, ref num))
											{
												if (num != 0u)
												{
													jPXCodeBlock.lBlock += 1u;
												}
												else
												{
													if ((jPXTileComp.codeBlockStyle & 4u) != 0u)
													{
														if (jPXCodeBlock.nCodingPasses > jPXCodeBlock.dataLenSize)
														{
															jPXCodeBlock.dataLenSize = jPXCodeBlock.nCodingPasses;
															uint[] array = new uint[jPXCodeBlock.dataLenSize];
															if (jPXCodeBlock.dataLen != null)
															{
																Array.Copy(jPXCodeBlock.dataLen, array, jPXCodeBlock.dataLen.Length);
															}
															jPXCodeBlock.dataLen = array;
														}
														for (uint num6 = 0u; num6 < jPXCodeBlock.nCodingPasses; num6 += 1u)
														{
															if (!this.readBits((int)jPXCodeBlock.lBlock, ref jPXCodeBlock.dataLen[(int)((UIntPtr)num6)]))
															{
																goto IL_9C2;
															}
														}
														goto IL_5CB;
													}
													uint num10 = jPXCodeBlock.lBlock;
													for (uint num6 = jPXCodeBlock.nCodingPasses >> 1; num6 != 0u; num6 >>= 1)
													{
														num10 += 1u;
													}
													if (this.readBits((int)num10, ref jPXCodeBlock.dataLen[0]))
													{
														goto IL_5CB;
													}
													break;
												}
											}
											goto IL_9C2;
										}
										goto IL_9C2;
									}
								}
								IL_5CB:;
							}
						}
					}
				}
				if ((jPXTileComp.style & 4u) != 0u)
				{
					this.skipEPH();
				}
				tilePartLen = this.finishBitBuf();
				for (uint num2 = 0u; num2 < ((jPXTile.res == 0u) ? 1u : 3u); num2 += 1u)
				{
					JPXSubband jPXSubband = jPXPrecinct.subbands[(int)((UIntPtr)num2)];
					for (uint num3 = 0u; num3 < jPXSubband.nYCBs; num3 += 1u)
					{
						for (uint num4 = 0u; num4 < jPXSubband.nXCBs; num4 += 1u)
						{
							JPXCodeBlock jPXCodeBlock = jPXSubband.cbs[(int)((UIntPtr)(num3 * jPXSubband.nXCBs + num4))];
							if (jPXCodeBlock.included != 0u)
							{
								if (!this.readCodeBlockData(jPXTileComp, jPXResLevel, jPXPrecinct, jPXSubband, jPXTile.res, num2, jPXCodeBlock))
								{
									return false;
								}
								if ((jPXTileComp.codeBlockStyle & 4u) != 0u)
								{
									for (uint num6 = 0u; num6 < jPXCodeBlock.nCodingPasses; num6 += 1u)
									{
										tilePartLen -= jPXCodeBlock.dataLen[(int)((UIntPtr)num6)];
									}
								}
								else
								{
									tilePartLen -= jPXCodeBlock.dataLen[0];
								}
								jPXCodeBlock.seen = true;
							}
						}
					}
				}
				switch (jPXTile.progOrder)
				{
				case 0u:
					this.cover(58);
					if ((jPXTile.comp += 1u) == this.img.nComps)
					{
						jPXTile.comp = 0u;
						if ((jPXTile.res += 1u) == jPXTile.maxNDecompLevels + 1u)
						{
							jPXTile.res = 0u;
							if ((jPXTile.layer += 1u) == jPXTile.nLayers)
							{
								jPXTile.layer = 0u;
							}
						}
					}
					break;
				case 1u:
					this.cover(59);
					if ((jPXTile.comp += 1u) == this.img.nComps)
					{
						jPXTile.comp = 0u;
						if ((jPXTile.layer += 1u) == jPXTile.nLayers)
						{
							jPXTile.layer = 0u;
							if ((jPXTile.res += 1u) == jPXTile.maxNDecompLevels + 1u)
							{
								jPXTile.res = 0u;
							}
						}
					}
					break;
				case 2u:
					this.cover(60);
					if ((jPXTile.layer += 1u) == jPXTile.nLayers)
					{
						jPXTile.layer = 0u;
						if ((jPXTile.comp += 1u) == this.img.nComps)
						{
							jPXTile.comp = 0u;
							if ((jPXTile.res += 1u) == jPXTile.maxNDecompLevels + 1u)
							{
								jPXTile.res = 0u;
							}
						}
					}
					break;
				case 3u:
					this.cover(61);
					if ((jPXTile.layer += 1u) == jPXTile.nLayers)
					{
						jPXTile.layer = 0u;
						if ((jPXTile.res += 1u) == jPXTile.maxNDecompLevels + 1u)
						{
							jPXTile.res = 0u;
							if ((jPXTile.comp += 1u) == this.img.nComps)
							{
								jPXTile.comp = 0u;
							}
						}
					}
					break;
				case 4u:
					this.cover(62);
					if ((jPXTile.layer += 1u) == jPXTile.nLayers)
					{
						jPXTile.layer = 0u;
						if ((jPXTile.res += 1u) == jPXTile.maxNDecompLevels + 1u)
						{
							jPXTile.res = 0u;
							if ((jPXTile.comp += 1u) == this.img.nComps)
							{
								jPXTile.comp = 0u;
							}
						}
					}
					break;
				}
			}
			Block_11:
			IL_9C2:
			AuxException.Throw("Invalid JPX stream.");
			return false;
		}
		private bool readCodeBlockData(JPXTileComp tileComp, JPXResLevel resLevel, JPXPrecinct precinct, JPXSubband subband, uint res, uint sb, JPXCodeBlock cb)
		{
			if (cb.arithDecoder != null)
			{
				this.cover(63);
				cb.arithDecoder.restart((int)cb.dataLen[0]);
			}
			else
			{
				this.cover(64);
				cb.arithDecoder = new JArithmeticDecoder();
				cb.arithDecoder.setStream(this.bufStr, (int)cb.dataLen[0]);
				cb.arithDecoder.start();
				cb.stats = new JArithmeticDecoderStats(19);
				cb.stats.setEntry(0u, 4, 0);
				cb.stats.setEntry(17u, 3, 0);
				cb.stats.setEntry(18u, 46, 0);
			}
			for (uint num = 0u; num < cb.nCodingPasses; num += 1u)
			{
				if ((tileComp.codeBlockStyle & 4u) != 0u && num > 0u)
				{
					cb.arithDecoder.setStream(this.bufStr, (int)cb.dataLen[(int)((UIntPtr)num)]);
					cb.arithDecoder.start();
				}
				switch (cb.nextPass)
				{
				case 0u:
				{
					this.cover(65);
					uint num2 = cb.y0;
					RavenPtr<int> ravenPtr = new RavenPtr<int>(cb.coeffs, 0);
					RavenPtr<char> ravenPtr2 = new RavenPtr<char>(cb.touched);
					while (num2 < cb.y1)
					{
						uint num3 = cb.x0;
						RavenPtr<int> ravenPtr3 = new RavenPtr<int>(ravenPtr, 0);
						RavenPtr<char> ravenPtr4 = new RavenPtr<char>(ravenPtr2, 0);
						while (num3 < cb.x1)
						{
							uint num4 = 0u;
							RavenPtr<int> ravenPtr5 = new RavenPtr<int>(ravenPtr3, 0);
							RavenPtr<char> ravenPtr6 = new RavenPtr<char>(ravenPtr4, 0);
							while (num4 < 4u && num2 + num4 < cb.y1)
							{
								if (ravenPtr5.ptr == 0)
								{
									uint num7;
									uint num6;
									uint num5 = num6 = (num7 = 0u);
									int num9;
									int num8 = num9 = 2;
									if (num3 > cb.x0)
									{
										if (ravenPtr5[-1] != 0)
										{
											num6 += 1u;
											num9 += ((ravenPtr5[-1] < 0) ? -1 : 1);
										}
										if (num2 + num4 > cb.y0)
										{
											num7 += ((ravenPtr5[(int)(-tileComp.w - 1u)] != 0) ? 1u : 0u);
										}
										if (num2 + num4 < cb.y1 - 1u && ((tileComp.codeBlockStyle & 8u) == 0u || num4 < 3u))
										{
											num7 += ((ravenPtr5[(int)(tileComp.w - 1u)] != 0) ? 1u : 0u);
										}
									}
									if (num3 < cb.x1 - 1u)
									{
										if (ravenPtr5[1] != 0)
										{
											num6 += 1u;
											num9 += ((ravenPtr5[1] < 0) ? -1 : 1);
										}
										if (num2 + num4 > cb.y0)
										{
											num7 += ((ravenPtr5[(int)(-tileComp.w + 1u)] != 0) ? 1u : 0u);
										}
										if (num2 + num4 < cb.y1 - 1u && ((tileComp.codeBlockStyle & 8u) == 0u || num4 < 3u))
										{
											num7 += ((ravenPtr5[(int)(tileComp.w + 1u)] != 0) ? 1u : 0u);
										}
									}
									if (num2 + num4 > cb.y0 && ravenPtr5[(int)(-(int)tileComp.w)] != 0)
									{
										num5 += 1u;
										num8 += ((ravenPtr5[(int)(-(int)tileComp.w)] < 0) ? -1 : 1);
									}
									if (num2 + num4 < cb.y1 - 1u && ((tileComp.codeBlockStyle & 8u) == 0u || num4 < 3u) && ravenPtr5[(int)tileComp.w] != 0)
									{
										num5 += 1u;
										num8 += ((ravenPtr5[(int)tileComp.w] < 0) ? -1 : 1);
									}
									uint num10 = JPXStream.sigPropContext[(int)((UIntPtr)num6), (int)((UIntPtr)num5), (int)((UIntPtr)num7), (int)((UIntPtr)((res == 0u) ? 1u : sb))];
									if (num10 != 0u)
									{
										if (cb.arithDecoder.decodeBit(num10, cb.stats) != 0)
										{
											num10 = JPXStream.signContext[num9, num8, 0];
											uint num11 = JPXStream.signContext[num9, num8, 1];
											if (((long)cb.arithDecoder.decodeBit(num10, cb.stats) ^ (long)((ulong)num11)) != 0L)
											{
												ravenPtr5.ptr = -1;
											}
											else
											{
												ravenPtr5.ptr = 1;
											}
										}
										ravenPtr6.ptr = '\u0001';
									}
								}
								num4 += 1u;
								ravenPtr5.Inc(tileComp.w);
								ravenPtr6.Inc(tileComp.cbW);
							}
							num3 += 1u;
							ravenPtr3 = ++ravenPtr3;
							ravenPtr4 = ++ravenPtr4;
						}
						num2 += 4u;
						ravenPtr.Inc(4u * tileComp.w);
						ravenPtr2.Inc(4 << (int)tileComp.codeBlockW);
					}
					cb.nextPass += 1u;
					break;
				}
				case 1u:
				{
					this.cover(66);
					uint num2 = cb.y0;
					RavenPtr<int> ravenPtr = new RavenPtr<int>(cb.coeffs, 0);
					RavenPtr<char> ravenPtr2 = new RavenPtr<char>(cb.touched);
					while (num2 < cb.y1)
					{
						uint num3 = cb.x0;
						RavenPtr<int> ravenPtr3 = new RavenPtr<int>(ravenPtr, 0);
						RavenPtr<char> ravenPtr4 = new RavenPtr<char>(ravenPtr2, 0);
						while (num3 < cb.x1)
						{
							uint num4 = 0u;
							RavenPtr<int> ravenPtr5 = new RavenPtr<int>(ravenPtr3, 0);
							RavenPtr<char> ravenPtr6 = new RavenPtr<char>(ravenPtr4, 0);
							while (num4 < 4u && num2 + num4 < cb.y1)
							{
								if (ravenPtr5.ptr != 0 && ravenPtr6.ptr == '\0')
								{
									uint num10;
									if (ravenPtr5.ptr == 1 || ravenPtr5.ptr == -1)
									{
										uint num12 = 0u;
										if (num3 > cb.x0)
										{
											num12 += ((ravenPtr5[-1] != 0) ? 1u : 0u);
											if (num2 + num4 > cb.y0)
											{
												num12 += ((ravenPtr5[(int)(-tileComp.w - 1u)] != 0) ? 1u : 0u);
											}
											if (num2 + num4 < cb.y1 - 1u && ((tileComp.codeBlockStyle & 8u) == 0u || num4 < 3u))
											{
												num12 += ((ravenPtr5[(int)(tileComp.w - 1u)] != 0) ? 1u : 0u);
											}
										}
										if (num3 < cb.x1 - 1u)
										{
											num12 += ((ravenPtr5[1] != 0) ? 1u : 0u);
											if (num2 + num4 > cb.y0)
											{
												num12 += ((ravenPtr5[(int)(-tileComp.w + 1u)] != 0) ? 1u : 0u);
											}
											if (num2 + num4 < cb.y1 - 1u && ((tileComp.codeBlockStyle & 8u) == 0u || num4 < 3u))
											{
												num12 += ((ravenPtr5[(int)(tileComp.w + 1u)] != 0) ? 1u : 0u);
											}
										}
										if (num2 + num4 > cb.y0)
										{
											num12 += ((ravenPtr5[(int)(-(int)tileComp.w)] != 0) ? 1u : 0u);
										}
										if (num2 + num4 < cb.y1 - 1u && ((tileComp.codeBlockStyle & 8u) == 0u || num4 < 3u))
										{
											num12 += ((ravenPtr5[(int)tileComp.w] != 0) ? 1u : 0u);
										}
										num10 = ((num12 != 0u) ? 15u : 14u);
									}
									else
									{
										num10 = 16u;
									}
									int num13 = cb.arithDecoder.decodeBit(num10, cb.stats);
									if (ravenPtr5.ptr < 0)
									{
										ravenPtr5.ptr = (ravenPtr5.ptr << 1) - num13;
									}
									else
									{
										ravenPtr5.ptr = (ravenPtr5.ptr << 1) + num13;
									}
									ravenPtr6.ptr = '\u0001';
								}
								num4 += 1u;
								ravenPtr5.Inc(tileComp.w);
								ravenPtr6.Inc(tileComp.cbW);
							}
							num3 += 1u;
							ravenPtr3 = ++ravenPtr3;
							ravenPtr4 = ++ravenPtr4;
						}
						num2 += 4u;
						ravenPtr.Inc(4u * tileComp.w);
						ravenPtr2.Inc(4 << (int)tileComp.codeBlockW);
					}
					cb.nextPass += 1u;
					break;
				}
				case 2u:
				{
					this.cover(67);
					uint num2 = cb.y0;
					RavenPtr<int> ravenPtr = new RavenPtr<int>(cb.coeffs, 0);
					RavenPtr<char> ravenPtr2 = new RavenPtr<char>(cb.touched);
					while (num2 < cb.y1)
					{
						uint num3 = cb.x0;
						RavenPtr<int> ravenPtr3 = new RavenPtr<int>(ravenPtr, 0);
						RavenPtr<char> ravenPtr4 = new RavenPtr<char>(ravenPtr2, 0);
						while (num3 < cb.x1)
						{
							uint num4 = 0u;
							RavenPtr<int> ravenPtr5;
							if (num2 + 3u < cb.y1 && ravenPtr4.ptr == '\0' && ravenPtr4[(int)tileComp.cbW] == '\0' && ravenPtr4[(int)(2u * tileComp.cbW)] == '\0' && ravenPtr4[(int)(3u * tileComp.cbW)] == '\0' && (num3 == cb.x0 || num2 == cb.y0 || ravenPtr3[(int)(-tileComp.w - 1u)] == 0) && (num2 == cb.y0 || ravenPtr3[(int)(-(int)tileComp.w)] == 0) && (num3 == cb.x1 - 1u || num2 == cb.y0 || ravenPtr3[(int)(-tileComp.w + 1u)] == 0) && (num3 == cb.x0 || (ravenPtr3[-1] == 0 && ravenPtr3[(int)(tileComp.w - 1u)] == 0 && ravenPtr3[(int)(2u * tileComp.w - 1u)] == 0 && ravenPtr3[(int)(3u * tileComp.w - 1u)] == 0)) && (num3 == cb.x1 - 1u || (ravenPtr3[1] == 0 && ravenPtr3[(int)(tileComp.w + 1u)] == 0 && ravenPtr3[(int)(2u * tileComp.w + 1u)] == 0 && ravenPtr3[(int)(3u * tileComp.w + 1u)] == 0)) && ((tileComp.codeBlockStyle & 8u) != 0u || ((num3 == cb.x0 || num2 + 4u == cb.y1 || ravenPtr3[(int)(4u * tileComp.w - 1u)] == 0) && (num2 + 4u == cb.y1 || ravenPtr3[(int)(4u * tileComp.w)] == 0) && (num3 == cb.x1 - 1u || num2 + 4u == cb.y1 || ravenPtr3[(int)(4u * tileComp.w + 1u)] == 0))))
							{
								if (cb.arithDecoder.decodeBit(17u, cb.stats) != 0)
								{
									num4 = (uint)cb.arithDecoder.decodeBit(18u, cb.stats);
									num4 = (num4 << 1 | (uint)cb.arithDecoder.decodeBit(18u, cb.stats));
									ravenPtr5 = new RavenPtr<int>(ravenPtr3, (int)(num4 * tileComp.w));
									uint num10 = JPXStream.signContext[2, 2, 0];
									uint num11 = JPXStream.signContext[2, 2, 1];
									if (((long)cb.arithDecoder.decodeBit(num10, cb.stats) ^ (long)((ulong)num11)) != 0L)
									{
										ravenPtr5.ptr = -1;
									}
									else
									{
										ravenPtr5.ptr = 1;
									}
									num4 += 1u;
								}
								else
								{
									num4 = 4u;
								}
							}
							ravenPtr5 = new RavenPtr<int>(ravenPtr3, (int)(num4 * tileComp.w));
							RavenPtr<char> ravenPtr6 = new RavenPtr<char>(ravenPtr4, (int)((int)num4 << (int)tileComp.codeBlockW));
							while (num4 < 4u && num2 + num4 < cb.y1)
							{
								if (ravenPtr6.ptr == '\0')
								{
									uint num7;
									uint num6;
									uint num5 = num6 = (num7 = 0u);
									int num9;
									int num8 = num9 = 2;
									if (num3 > cb.x0)
									{
										if (ravenPtr5[-1] != 0)
										{
											num6 += 1u;
											num9 += ((ravenPtr5[-1] < 0) ? -1 : 1);
										}
										if (num2 + num4 > cb.y0)
										{
											num7 += ((ravenPtr5[(int)(-tileComp.w - 1u)] != 0) ? 1u : 0u);
										}
										if (num2 + num4 < cb.y1 - 1u && ((tileComp.codeBlockStyle & 8u) == 0u || num4 < 3u))
										{
											num7 += ((ravenPtr5[(int)(tileComp.w - 1u)] != 0) ? 1u : 0u);
										}
									}
									if (num3 < cb.x1 - 1u)
									{
										if (ravenPtr5[1] != 0)
										{
											num6 += 1u;
											num9 += ((ravenPtr5[1] < 0) ? -1 : 1);
										}
										if (num2 + num4 > cb.y0)
										{
											num7 += ((ravenPtr5[(int)(-tileComp.w + 1u)] != 0) ? 1u : 0u);
										}
										if (num2 + num4 < cb.y1 - 1u && ((tileComp.codeBlockStyle & 8u) == 0u || num4 < 3u))
										{
											num7 += ((ravenPtr5[(int)(tileComp.w + 1u)] != 0) ? 1u : 0u);
										}
									}
									if (num2 + num4 > cb.y0 && ravenPtr5[(int)(-(int)tileComp.w)] != 0)
									{
										num5 += 1u;
										num8 += ((ravenPtr5[(int)(-(int)tileComp.w)] < 0) ? -1 : 1);
									}
									if (num2 + num4 < cb.y1 - 1u && ((tileComp.codeBlockStyle & 8u) == 0u || num4 < 3u) && ravenPtr5[(int)tileComp.w] != 0)
									{
										num5 += 1u;
										num8 += ((ravenPtr5[(int)tileComp.w] < 0) ? -1 : 1);
									}
									uint num10 = JPXStream.sigPropContext[(int)((UIntPtr)num6), (int)((UIntPtr)num5), (int)((UIntPtr)num7), (int)((UIntPtr)((res == 0u) ? 1u : sb))];
									if (cb.arithDecoder.decodeBit(num10, cb.stats) != 0)
									{
										num10 = JPXStream.signContext[num9, num8, 0];
										uint num11 = JPXStream.signContext[num9, num8, 1];
										if (((long)cb.arithDecoder.decodeBit(num10, cb.stats) ^ (long)((ulong)num11)) != 0L)
										{
											ravenPtr5.ptr = -1;
										}
										else
										{
											ravenPtr5.ptr = 1;
										}
									}
								}
								else
								{
									ravenPtr6.ptr = '\0';
								}
								num4 += 1u;
								ravenPtr5.Inc(tileComp.w);
								ravenPtr6.Inc(tileComp.cbW);
							}
							num3 += 1u;
							ravenPtr3 = ++ravenPtr3;
							ravenPtr4 = ++ravenPtr4;
						}
						num2 += 4u;
						ravenPtr.Inc(4u * tileComp.w);
						ravenPtr2.Inc(4 << (int)tileComp.codeBlockW);
					}
					cb.len += 1;
					if ((tileComp.codeBlockStyle & 32u) != 0u)
					{
						int num14 = cb.arithDecoder.decodeBit(18u, cb.stats) << 3;
						num14 |= cb.arithDecoder.decodeBit(18u, cb.stats) << 2;
						num14 |= cb.arithDecoder.decodeBit(18u, cb.stats) << 1;
						num14 |= cb.arithDecoder.decodeBit(18u, cb.stats);
						if (num14 != 10)
						{
							AuxException.Throw("Missing or invalid segmentation symbol in JPX stream.");
						}
					}
					cb.nextPass = 0u;
					break;
				}
				}
				if ((tileComp.codeBlockStyle & 2u) != 0u)
				{
					cb.stats.reset();
					cb.stats.setEntry(0u, 4, 0);
					cb.stats.setEntry(17u, 3, 0);
					cb.stats.setEntry(18u, 46, 0);
				}
				if ((tileComp.codeBlockStyle & 4u) != 0u)
				{
					cb.arithDecoder.cleanup();
				}
			}
			cb.arithDecoder.cleanup();
			return true;
		}
		private void inverseTransform(JPXTileComp tileComp)
		{
			this.cover(68);
			JPXResLevel jPXResLevel = tileComp.resLevels[0];
			JPXPrecinct jPXPrecinct = jPXResLevel.precincts[0];
			JPXSubband jPXSubband = jPXPrecinct.subbands[0];
			uint num = tileComp.quantStyle & 31u;
			uint num2 = tileComp.quantStyle >> 5 & 7u;
			uint num4;
			double num5;
			if (num == 0u)
			{
				this.cover(69);
				uint num3 = tileComp.quantSteps[0] >> 3 & 31u;
				num4 = num2 + num3 - 1u;
				num5 = 0.0;
			}
			else
			{
				this.cover(70);
				num4 = num2 - 1u + tileComp.prec;
				num5 = (2048u + (tileComp.quantSteps[0] & 2047u)) / 2048.0;
			}
			if (tileComp.transform == 0u)
			{
				this.cover(71);
				num4 += 16u;
			}
			int num6 = 0;
			JPXCodeBlock jPXCodeBlock = jPXSubband.cbs[num6];
			for (uint num7 = 0u; num7 < jPXSubband.nYCBs; num7 += 1u)
			{
				for (uint num8 = 0u; num8 < jPXSubband.nXCBs; num8 += 1u)
				{
					uint num9 = jPXCodeBlock.y0;
					RavenPtr<int> ravenPtr = new RavenPtr<int>(jPXCodeBlock.coeffs, 0);
					RavenPtr<char> ravenPtr2 = new RavenPtr<char>(jPXCodeBlock.touched);
					while (num9 < jPXCodeBlock.y1)
					{
						uint num10 = jPXCodeBlock.x0;
						RavenPtr<int> ravenPtr3 = new RavenPtr<int>(ravenPtr, 0);
						RavenPtr<char> ravenPtr4 = new RavenPtr<char>(ravenPtr2, 0);
						while (num10 < jPXCodeBlock.x1)
						{
							int num11 = ravenPtr3.ptr;
							if (num11 != 0)
							{
								int num12 = (int)(num4 - (jPXCodeBlock.nZeroBitPlanes + (uint)jPXCodeBlock.len + (uint)ravenPtr4.ptr));
								if (num12 > 0)
								{
									this.cover(94);
									if (num11 < 0)
									{
										num11 = (num11 << num12) - (1 << num12 - 1);
									}
									else
									{
										num11 = (num11 << num12) + (1 << num12 - 1);
									}
								}
								else
								{
									this.cover(95);
									num11 >>= -num12;
								}
								if (num == 0u)
								{
									this.cover(96);
									if (tileComp.transform == 0u)
									{
										this.cover(97);
										num11 &= -65536;
									}
								}
								else
								{
									this.cover(98);
									num11 = (int)((double)num11 * num5);
								}
							}
							ravenPtr3.ptr = num11;
							num10 += 1u;
							ravenPtr3 = ++ravenPtr3;
							ravenPtr4 = ++ravenPtr4;
						}
						num9 += 1u;
						ravenPtr.Inc(tileComp.w);
						ravenPtr2.Inc(tileComp.cbW);
					}
					num6++;
					jPXCodeBlock = ((num6 < jPXSubband.cbs.Length) ? jPXSubband.cbs[num6] : null);
				}
			}
			for (uint num13 = 1u; num13 <= tileComp.nDecompLevels; num13 += 1u)
			{
				jPXResLevel = tileComp.resLevels[(int)((UIntPtr)num13)];
				this.inverseTransformLevel(tileComp, num13, jPXResLevel);
			}
		}
		private void inverseTransformLevel(JPXTileComp tileComp, uint r, JPXResLevel resLevel)
		{
			uint num = tileComp.quantStyle & 31u;
			uint num2 = tileComp.quantStyle >> 5 & 7u;
			JPXPrecinct jPXPrecinct = resLevel.precincts[0];
			uint num11;
			uint num12;
			for (uint num3 = 0u; num3 < 3u; num3 += 1u)
			{
				uint num5;
				double num6;
				if (num == 0u)
				{
					this.cover(100);
					uint num4 = tileComp.quantSteps[(int)((UIntPtr)(3u * r - 2u + num3))] >> 3 & 31u;
					num5 = num2 + num4 - 1u;
					num6 = 0.0;
				}
				else
				{
					this.cover(101);
					num5 = num2 + tileComp.prec;
					if (num3 == 2u)
					{
						this.cover(102);
						num5 += 1u;
					}
					uint num7 = tileComp.quantSteps[(int)((UIntPtr)((num == 1u) ? 0u : (3u * r - 2u + num3)))];
					num6 = (2048u + (num7 & 2047u)) / 2048.0;
				}
				if (tileComp.transform == 0u)
				{
					this.cover(103);
					num5 += 16u;
				}
				JPXSubband jPXSubband = jPXPrecinct.subbands[(int)((UIntPtr)num3)];
				int num8 = 0;
				JPXCodeBlock jPXCodeBlock = (jPXSubband.cbs.Length > 0) ? jPXSubband.cbs[num8] : null;
				for (uint num9 = 0u; num9 < jPXSubband.nYCBs; num9 += 1u)
				{
					for (uint num10 = 0u; num10 < jPXSubband.nXCBs; num10 += 1u)
					{
						num11 = jPXCodeBlock.y0;
						RavenPtr<int> ravenPtr = new RavenPtr<int>(jPXCodeBlock.coeffs, 0);
						RavenPtr<char> ravenPtr2 = new RavenPtr<char>(jPXCodeBlock.touched);
						while (num11 < jPXCodeBlock.y1)
						{
							num12 = jPXCodeBlock.x0;
							RavenPtr<int> ravenPtr3 = new RavenPtr<int>(ravenPtr, 0);
							RavenPtr<char> ravenPtr4 = new RavenPtr<char>(ravenPtr2, 0);
							while (num12 < jPXCodeBlock.x1)
							{
								int num13 = ravenPtr3.ptr;
								if (num13 != 0)
								{
									int num14 = (int)(num5 - (jPXCodeBlock.nZeroBitPlanes + (uint)jPXCodeBlock.len + (uint)ravenPtr4.ptr));
									if (num14 > 0)
									{
										this.cover(74);
										if (num13 < 0)
										{
											num13 = (num13 << num14) - (1 << num14 - 1);
										}
										else
										{
											num13 = (num13 << num14) + (1 << num14 - 1);
										}
									}
									else
									{
										this.cover(75);
										num13 >>= -num14;
									}
									if (num == 0u)
									{
										this.cover(76);
										if (tileComp.transform == 0u)
										{
											num13 &= -65536;
										}
									}
									else
									{
										this.cover(77);
										num13 = (int)((double)num13 * num6);
									}
								}
								ravenPtr3.ptr = num13;
								num12 += 1u;
								ravenPtr3 = ++ravenPtr3;
								ravenPtr4 = ++ravenPtr4;
							}
							num11 += 1u;
							ravenPtr.Inc(tileComp.w);
							ravenPtr2.Inc(tileComp.cbW);
						}
						num8++;
						jPXCodeBlock = ((num8 < jPXSubband.cbs.Length) ? jPXSubband.cbs[num8] : null);
					}
				}
			}
			uint num15 = jPXPrecinct.subbands[1].x1 - jPXPrecinct.subbands[1].x0;
			uint num16 = num15 + jPXPrecinct.subbands[0].x1 - jPXPrecinct.subbands[0].x0;
			uint num17 = jPXPrecinct.subbands[0].y1 - jPXPrecinct.subbands[0].y0;
			uint num18 = num17 + jPXPrecinct.subbands[1].y1 - jPXPrecinct.subbands[1].y0;
			uint num19;
			if (r == tileComp.nDecompLevels)
			{
				num19 = 3u + (tileComp.x0 & 1u);
			}
			else
			{
				num19 = 3u + (tileComp.resLevels[(int)((UIntPtr)(r + 1u))].x0 & 1u);
			}
			num11 = 0u;
			RavenPtr<int> ravenPtr5 = new RavenPtr<int>(tileComp.data);
			while (num11 < num18)
			{
				RavenPtr<int> ravenPtr6;
				if (jPXPrecinct.subbands[0].x0 == jPXPrecinct.subbands[1].x0)
				{
					num12 = 0u;
					ravenPtr6 = new RavenPtr<int>(tileComp.buf, (int)num19);
					while (num12 < num15)
					{
						ravenPtr6.ptr = ravenPtr5[(int)num12];
						num12 += 1u;
						ravenPtr6.Inc(2);
					}
					num12 = num15;
					ravenPtr6 = new RavenPtr<int>(tileComp.buf, (int)(num19 + 1u));
					while (num12 < num16)
					{
						ravenPtr6.ptr = ravenPtr5[(int)num12];
						num12 += 1u;
						ravenPtr6.Inc(2);
					}
				}
				else
				{
					num12 = 0u;
					ravenPtr6 = new RavenPtr<int>(tileComp.buf, (int)(num19 + 1u));
					while (num12 < num15)
					{
						ravenPtr6.ptr = ravenPtr5[(int)num12];
						num12 += 1u;
						ravenPtr6.Inc(2);
					}
					num12 = num15;
					ravenPtr6 = new RavenPtr<int>(tileComp.buf, (int)num19);
					while (num12 < num16)
					{
						ravenPtr6.ptr = ravenPtr5[(int)num12];
						num12 += 1u;
						ravenPtr6.Inc(2);
					}
				}
				this.inverseTransform1D(tileComp, tileComp.buf, (int)num19, (int)num16);
				num12 = 0u;
				ravenPtr6 = new RavenPtr<int>(tileComp.buf, (int)num19);
				while (num12 < num16)
				{
					ravenPtr5[(int)num12] = ravenPtr6.ptr;
					num12 += 1u;
					ravenPtr6 = ++ravenPtr6;
				}
				num11 += 1u;
				ravenPtr5.Inc(tileComp.w);
			}
			if (r == tileComp.nDecompLevels)
			{
				num19 = 3u + (tileComp.y0 & 1u);
			}
			else
			{
				num19 = 3u + (tileComp.resLevels[(int)((UIntPtr)(r + 1u))].y0 & 1u);
			}
			num12 = 0u;
			ravenPtr5 = new RavenPtr<int>(tileComp.data);
			while (num12 < num16)
			{
				RavenPtr<int> ravenPtr6;
				if (jPXPrecinct.subbands[1].y0 == jPXPrecinct.subbands[0].y0)
				{
					num11 = 0u;
					ravenPtr6 = new RavenPtr<int>(tileComp.buf, (int)num19);
					while (num11 < num17)
					{
						ravenPtr6.ptr = ravenPtr5[(int)(num11 * tileComp.w)];
						num11 += 1u;
						ravenPtr6.Inc(2);
					}
					num11 = num17;
					ravenPtr6 = new RavenPtr<int>(tileComp.buf, (int)(num19 + 1u));
					while (num11 < num18)
					{
						ravenPtr6.ptr = ravenPtr5[(int)(num11 * tileComp.w)];
						num11 += 1u;
						ravenPtr6.Inc(2);
					}
				}
				else
				{
					num11 = 0u;
					ravenPtr6 = new RavenPtr<int>(tileComp.buf, (int)(num19 + 1u));
					while (num11 < num17)
					{
						ravenPtr6.ptr = ravenPtr5[(int)(num11 * tileComp.w)];
						num11 += 1u;
						ravenPtr6.Inc(2);
					}
					num11 = num17;
					ravenPtr6 = new RavenPtr<int>(tileComp.buf, (int)num19);
					while (num11 < num18)
					{
						ravenPtr6.ptr = ravenPtr5[(int)(num11 * tileComp.w)];
						num11 += 1u;
						ravenPtr6.Inc(2);
					}
				}
				this.inverseTransform1D(tileComp, tileComp.buf, (int)num19, (int)num18);
				num11 = 0u;
				ravenPtr6 = new RavenPtr<int>(tileComp.buf, (int)num19);
				while (num11 < num18)
				{
					ravenPtr5[(int)(num11 * tileComp.w)] = ravenPtr6.ptr;
					num11 += 1u;
					ravenPtr6 = ++ravenPtr6;
				}
				num12 += 1u;
				ravenPtr5 = ++ravenPtr5;
			}
		}
		private void inverseTransform1D(JPXTileComp tileComp, int[] data, int offset, int n)
		{
			if (n == 1)
			{
				this.cover(79);
				if (offset == 4)
				{
					this.cover(104);
					data[0] >>= 1;
					return;
				}
			}
			else
			{
				this.cover(80);
				int num = offset + n;
				data[num] = data[num - 2];
				if (n == 2)
				{
					this.cover(81);
					data[num + 1] = data[offset + 1];
					data[num + 2] = data[offset];
					data[num + 3] = data[offset + 1];
				}
				else
				{
					this.cover(82);
					data[num + 1] = data[num - 3];
					if (n == 3)
					{
						this.cover(105);
						data[num + 2] = data[offset + 1];
						data[num + 3] = data[offset + 2];
					}
					else
					{
						this.cover(106);
						data[num + 2] = data[num - 4];
						if (n == 4)
						{
							this.cover(107);
							data[num + 3] = data[offset + 1];
						}
						else
						{
							this.cover(108);
							data[num + 3] = data[num - 5];
						}
					}
				}
				data[offset - 1] = data[offset + 1];
				data[offset - 2] = data[offset + 2];
				data[offset - 3] = data[offset + 3];
				if (offset == 4)
				{
					this.cover(83);
					data[0] = data[offset + 4];
				}
				if (tileComp.transform == 0u)
				{
					this.cover(84);
					for (int i = 1; i <= num + 2; i += 2)
					{
						data[i] = (int)(1.2301741049140009 * (double)data[i]);
					}
					for (int i = 0; i <= num + 3; i += 2)
					{
						data[i] = (int)(0.8128930661159609 * (double)data[i]);
					}
					for (int i = 1; i <= num + 2; i += 2)
					{
						data[i] = (int)((double)data[i] - 0.443506852043971 * (double)(data[i - 1] + data[i + 1]));
					}
					for (int i = 2; i <= num + 1; i += 2)
					{
						data[i] = (int)((double)data[i] - 0.882911075530934 * (double)(data[i - 1] + data[i + 1]));
					}
					for (int i = 3; i <= num; i += 2)
					{
						data[i] = (int)((double)data[i] - -0.052980118572961 * (double)(data[i - 1] + data[i + 1]));
					}
					for (int i = 4; i <= num - 1; i += 2)
					{
						data[i] = (int)((double)data[i] - -1.5861343420599241 * (double)(data[i - 1] + data[i + 1]));
					}
					return;
				}
				this.cover(85);
				for (int i = 3; i <= num; i += 2)
				{
					data[i] -= data[i - 1] + data[i + 1] + 2 >> 2;
				}
				for (int i = 4; i < num; i += 2)
				{
					data[i] += data[i - 1] + data[i + 1] >> 1;
				}
			}
		}
		private bool inverseMultiCompAndDC(JPXTile tile)
		{
			if (tile.multiComp == 1u)
			{
				this.cover(86);
				if (this.img.nComps < 3u || tile.tileComps[0].hSep != tile.tileComps[1].hSep || tile.tileComps[0].vSep != tile.tileComps[1].vSep || tile.tileComps[1].hSep != tile.tileComps[2].hSep || tile.tileComps[1].vSep != tile.tileComps[2].vSep)
				{
					return false;
				}
				if (tile.tileComps[0].transform == 0u)
				{
					this.cover(87);
					int num = 0;
					int num2 = 0;
					while ((long)num2 < (long)((ulong)(tile.tileComps[0].y1 - tile.tileComps[0].y0)))
					{
						int num3 = 0;
						while ((long)num3 < (long)((ulong)(tile.tileComps[0].x1 - tile.tileComps[0].x0)))
						{
							int num4 = tile.tileComps[0].data[num];
							int num5 = tile.tileComps[1].data[num];
							int num6 = tile.tileComps[2].data[num];
							tile.tileComps[0].data[num] = (int)((double)num4 + 1.402 * (double)num6 + 0.5);
							tile.tileComps[1].data[num] = (int)((double)num4 - 0.34413 * (double)num5 - 0.71414 * (double)num6 + 0.5);
							tile.tileComps[2].data[num] = (int)((double)num4 + 1.772 * (double)num5 + 0.5);
							num++;
							num3++;
						}
						num2++;
					}
				}
				else
				{
					this.cover(88);
					int num = 0;
					int num2 = 0;
					while ((long)num2 < (long)((ulong)(tile.tileComps[0].y1 - tile.tileComps[0].y0)))
					{
						int num3 = 0;
						while ((long)num3 < (long)((ulong)(tile.tileComps[0].x1 - tile.tileComps[0].x0)))
						{
							int num4 = tile.tileComps[0].data[num];
							int num5 = tile.tileComps[1].data[num];
							int num6 = tile.tileComps[2].data[num];
							int num7 = tile.tileComps[1].data[num] = num4 - (num6 + num5 >> 2);
							tile.tileComps[0].data[num] = num6 + num7;
							tile.tileComps[2].data[num] = num5 + num7;
							num++;
							num3++;
						}
						num2++;
					}
				}
			}
			int num8 = 0;
			while ((long)num8 < (long)((ulong)this.img.nComps))
			{
				JPXTileComp jPXTileComp = tile.tileComps[num8];
				if (jPXTileComp.sgned)
				{
					this.cover(89);
					int num9 = -(1 << (int)(jPXTileComp.prec - 1u));
					int num10 = (1 << (int)(jPXTileComp.prec - 1u)) - 1;
					RavenPtr<int> ravenPtr = new RavenPtr<int>(jPXTileComp.data);
					int num2 = 0;
					while ((long)num2 < (long)((ulong)(jPXTileComp.y1 - jPXTileComp.y0)))
					{
						int num3 = 0;
						while ((long)num3 < (long)((ulong)(jPXTileComp.x1 - jPXTileComp.x0)))
						{
							int num11 = ravenPtr.ptr;
							if (jPXTileComp.transform == 0u)
							{
								this.cover(109);
								num11 >>= 16;
							}
							if (num11 < num9)
							{
								this.cover(110);
								num11 = num9;
							}
							else
							{
								if (num11 > num10)
								{
									this.cover(111);
									num11 = num10;
								}
							}
							ravenPtr.ptr = num11;
							ravenPtr = ++ravenPtr;
							num3++;
						}
						num2++;
					}
				}
				else
				{
					this.cover(90);
					int num10 = (1 << (int)jPXTileComp.prec) - 1;
					int num12 = 1 << (int)(jPXTileComp.prec - 1u);
					RavenPtr<int> ravenPtr = new RavenPtr<int>(jPXTileComp.data);
					int num2 = 0;
					while ((long)num2 < (long)((ulong)(jPXTileComp.y1 - jPXTileComp.y0)))
					{
						int num3 = 0;
						while ((long)num3 < (long)((ulong)(jPXTileComp.x1 - jPXTileComp.x0)))
						{
							int num11 = ravenPtr.ptr;
							if (jPXTileComp.transform == 0u)
							{
								this.cover(112);
								num11 >>= 16;
							}
							num11 += num12;
							if (num11 < 0)
							{
								this.cover(113);
								num11 = 0;
							}
							else
							{
								if (num11 > num10)
								{
									this.cover(114);
									num11 = num10;
								}
							}
							ravenPtr.ptr = num11;
							ravenPtr = ++ravenPtr;
							num3++;
						}
						num2++;
					}
				}
				num8++;
			}
			return true;
		}
		private bool readBoxHdr(ref uint boxType, ref uint boxLen, ref uint dataLen)
		{
			uint num = 0u;
			uint num2 = 0u;
			if (!this.readULong(ref num) || !this.readULong(ref boxType))
			{
				return false;
			}
			if (num == 1u)
			{
				if (!this.readULong(ref num2) || !this.readULong(ref num))
				{
					return false;
				}
				if (num2 != 0u)
				{
					AuxException.Throw("JPX stream contains a box larger than 2^32 bytes.");
					return false;
				}
				boxLen = num;
				dataLen = num - 16u;
			}
			else
			{
				if (num == 0u)
				{
					boxLen = 0u;
					dataLen = 0u;
				}
				else
				{
					boxLen = num;
					dataLen = num - 8u;
				}
			}
			return true;
		}
		private bool readMarkerHdr(ref int segType, ref uint segLen)
		{
			IL_00:
			int @char;
			while ((@char = this.bufStr.getChar()) != -1)
			{
				if (@char == 255)
				{
					while ((@char = this.bufStr.getChar()) != -1)
					{
						if (@char != 255)
						{
							if (@char == 0)
							{
								goto IL_00;
							}
							segType = @char;
							if ((@char >= 48 && @char <= 63) || @char == 79 || @char == 146 || @char == 147 || @char == 217)
							{
								segLen = 0u;
								return true;
							}
							return this.readUWord(ref segLen);
						}
					}
					return false;
				}
			}
			return false;
		}
		private bool readBits(int nBits, ref int x)
		{
			uint num = 0u;
			if (!this.readBits(nBits, ref num))
			{
				return false;
			}
			x = (int)num;
			return true;
		}
		private bool readBits(int nBits, ref uint x)
		{
			while (this.bitBufLen < nBits)
			{
				int @char;
				if (this.byteCount == 0u || (@char = this.bufStr.getChar()) == -1)
				{
					return false;
				}
				this.byteCount -= 1u;
				if (this.bitBufSkip)
				{
					this.bitBuf = (this.bitBuf << 7 | (uint)(@char & 127));
					this.bitBufLen += 7;
				}
				else
				{
					this.bitBuf = (this.bitBuf << 8 | (uint)(@char & 255));
					this.bitBufLen += 8;
				}
				this.bitBufSkip = (@char == 255);
			}
			x = (uint)((ulong)(this.bitBuf >> this.bitBufLen - nBits) & (ulong)((long)((1 << nBits) - 1)));
			this.bitBufLen -= nBits;
			return true;
		}
		private bool readUByte(ref uint x)
		{
			int @char;
			if ((@char = this.bufStr.getChar()) == -1)
			{
				return false;
			}
			x = (uint)@char;
			return true;
		}
		private bool readByte(ref int x)
		{
			int @char;
			if ((@char = this.bufStr.getChar()) == -1)
			{
				return false;
			}
			x = @char;
			if ((@char & 128) != 0)
			{
				x |= -256;
			}
			return true;
		}
		private bool readUWord(ref uint x)
		{
			int @char;
			int char2;
			if ((@char = this.bufStr.getChar()) == -1 || (char2 = this.bufStr.getChar()) == -1)
			{
				return false;
			}
			x = (uint)(@char << 8 | char2);
			return true;
		}
		private bool readULong2(ref JPXColorSpaceType x)
		{
			uint num = 0u;
			if (!this.readULong(ref num))
			{
				return false;
			}
			x = (JPXColorSpaceType)num;
			return true;
		}
		private bool readULong(ref uint x)
		{
			int @char;
			int char2;
			int char3;
			int char4;
			if ((@char = this.bufStr.getChar()) == -1 || (char2 = this.bufStr.getChar()) == -1 || (char3 = this.bufStr.getChar()) == -1 || (char4 = this.bufStr.getChar()) == -1)
			{
				return false;
			}
			x = (uint)(@char << 24 | char2 << 16 | char3 << 8 | char4);
			return true;
		}
		private bool readNBytes(int nBytes, bool signd, ref int x)
		{
			int num = 0;
			for (int i = 0; i < nBytes; i++)
			{
				int @char;
				if ((@char = this.bufStr.getChar()) == -1)
				{
					return false;
				}
				num = (num << 8) + @char;
			}
			if (signd && (num & 1 << 8 * nBytes - 1) != 0)
			{
				num |= -1 << 8 * nBytes;
			}
			x = num;
			return true;
		}
		private void startBitBuf(uint byteCountA)
		{
			this.bitBufLen = 0;
			this.bitBufSkip = false;
			this.byteCount = byteCountA;
		}
		private void skipSOP()
		{
			if (this.byteCount >= 6u && this.bufStr.lookChar(0) == 255 && this.bufStr.lookChar(1) == 145)
			{
				for (int i = 0; i < 6; i++)
				{
					this.bufStr.getChar();
				}
				this.byteCount -= 6u;
				this.bitBufLen = 0;
				this.bitBufSkip = false;
			}
		}
		private void skipEPH()
		{
			int num = this.bitBufSkip ? 1 : 0;
			if (this.byteCount >= (uint)(num + 2) && this.bufStr.lookChar(num) == 255 && this.bufStr.lookChar(num + 1) == 146)
			{
				for (int i = 0; i < num + 2; i++)
				{
					this.bufStr.getChar();
				}
				this.byteCount -= (uint)(num + 2);
				this.bitBufLen = 0;
				this.bitBufSkip = false;
			}
		}
		private uint finishBitBuf()
		{
			if (this.bitBufSkip)
			{
				this.bufStr.getChar();
				this.byteCount -= 1u;
			}
			return this.byteCount;
		}
		private double jpxFloorDiv(double x, double y)
		{
			return x / y;
		}
		private uint jpxFloorDivPow2(uint x, uint y)
		{
			return x >> (int)y;
		}
		private uint jpxCeilDiv(uint x, uint y)
		{
			return (x + y - 1u) / y;
		}
		private uint jpxCeilDivPow2(uint x, uint y)
		{
			return (uint)((ulong)x + (ulong)(1L << (int)(y & 31u)) - 1uL >> (int)y);
		}
		private uint jpxCeilDivPow2(int x, int y)
		{
			return (uint)(x + (1 << y) - 1 >> y);
		}
		private void cover(int i)
		{
		}
	}
}
