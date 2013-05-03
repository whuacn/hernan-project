using System;
namespace Persits.PDF
{
	public class PdfImage
	{
		internal PdfManager m_pManager;
		internal PdfDocument m_pDoc;
		internal bool m_bInvert;
		internal bool m_bInterpolate;
		internal int m_nTiffIndex = 1;
		internal bool m_IsMask;
		internal float m_fDefaultScaleX = 1f;
		internal float m_fDefaultScaleY = 1f;
		internal float m_fResolutionX = 72f;
		internal float m_fResolutionY = 72f;
		internal int m_nBitsPerComponent = 8;
		internal int m_nComponentsPerSample = 3;
		internal bool m_bTransparencyColorSet;
		internal uint m_dwTransparencyColor;
		internal string m_bstrID = "";
		internal float m_fWidth;
		internal float m_fHeight;
		internal PdfIndirectObj m_pImageObj;
		internal string m_bstrColorSpace = "";
		internal string m_bstrFormat;
		internal bool m_bIsMask;
		public string Format
		{
			get
			{
				return this.m_bstrFormat;
			}
		}
		public float Width
		{
			get
			{
				return this.m_fWidth;
			}
		}
		public float Height
		{
			get
			{
				return this.m_fHeight;
			}
		}
		public float ResolutionX
		{
			get
			{
				return this.m_fResolutionX;
			}
		}
		public float ResolutionY
		{
			get
			{
				return this.m_fResolutionY;
			}
		}
		public int BitsPerComponent
		{
			get
			{
				return this.m_nBitsPerComponent;
			}
		}
		public int ComponentsPerSample
		{
			get
			{
				return this.m_nComponentsPerSample;
			}
		}
		public string ColorSpace
		{
			get
			{
				return this.m_bstrColorSpace;
			}
		}
		public bool TransparencyColorExists
		{
			get
			{
				return this.m_bTransparencyColorSet;
			}
		}
		public PdfParam TransparencyColor
		{
			get
			{
				if (!this.m_bTransparencyColorSet)
				{
					AuxException.Throw("A transparency color does not exist for this image.", PdfErrors._ERROR_OUTOFRANGE);
				}
				int rValue = (int)AuxRGB.GetRValue((ulong)this.m_dwTransparencyColor);
				int gValue = (int)AuxRGB.GetGValue((ulong)this.m_dwTransparencyColor);
				int bValue = (int)AuxRGB.GetBValue((ulong)this.m_dwTransparencyColor);
				string paramString = string.Format("Min1={0}; Max1={1}; Min2={2}; Max2={3}; Min3={4}; Max3={5}", new object[]
				{
					bValue,
					bValue,
					gValue,
					gValue,
					rValue,
					rValue
				});
				return this.m_pManager.CreateParam(paramString);
			}
		}
		public bool IsMask
		{
			get
			{
				return this.m_bIsMask;
			}
			set
			{
				if (this.m_nBitsPerComponent != 1)
				{
					AuxException.Throw("Only a monochrome image (BitsPerComponent=1) can be used as a mask.", PdfErrors._ERROR_OUTOFRANGE);
				}
				if (value)
				{
					this.m_pImageObj.AddBool("ImageMask", true);
					this.m_pImageObj.m_objAttributes.RemoveByName("ColorSpace");
				}
				else
				{
					this.m_pImageObj.m_objAttributes.RemoveByName("ImageMask");
					this.m_pImageObj.AddName("ColorSpace", this.m_bstrColorSpace);
				}
				this.m_bIsMask = value;
			}
		}
		internal PdfImage()
		{
			this.m_nBitsPerComponent = 8;
			this.m_nComponentsPerSample = 3;
			this.m_bInvert = false;
			this.m_bInterpolate = false;
			this.m_bIsMask = false;
			this.m_fDefaultScaleX = (this.m_fDefaultScaleY = 1f);
			this.m_fResolutionX = (this.m_fResolutionY = 72f);
			this.m_nTiffIndex = 1;
		}
		internal bool Open(string Path, byte[] pData)
		{
			bool result = true;
			bool flag = false;
			if (pData == null)
			{
				AuxFile auxFile = new AuxFile();
				auxFile.Open(Path);
				this.m_pImageObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectImage);
				byte[] array = new byte[5];
				auxFile.Read(array);
				auxFile.Close();
				if (array[0] == 66 && array[1] == 77)
				{
					this.OpenBMP(Path);
				}
				else
				{
					if (array[0] == 71 && array[1] == 73 && array[2] == 70)
					{
						this.OpenGif(Path, null);
					}
					else
					{
						if ((array[0] == 73 && array[1] == 73) || (array[0] == 77 && array[1] == 77))
						{
							result = this.OpenTiff(Path, null, ref flag);
						}
						else
						{
							if (array[0] == 137 && array[1] == 80 && array[2] == 78 && array[3] == 71)
							{
								this.OpenPng(Path, null);
							}
							else
							{
								this.OpenJpeg(Path, null);
								flag = true;
							}
						}
					}
				}
			}
			else
			{
				this.m_pImageObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectImage);
				if (pData[0] == 66 && pData[1] == 77)
				{
					this.OpenBMPBinary(pData);
				}
				else
				{
					if (pData[0] == 71 && pData[1] == 73 && pData[2] == 70)
					{
						this.OpenGif(null, pData);
					}
					else
					{
						if ((pData[0] == 73 && pData[1] == 73) || (pData[0] == 77 && pData[1] == 77))
						{
							result = this.OpenTiff(null, pData, ref flag);
						}
						else
						{
							if (pData[0] == 137 && pData[1] == 80 && pData[2] == 78 && pData[3] == 71)
							{
								this.OpenPng(null, pData);
							}
							else
							{
								this.OpenJpeg(null, pData);
								flag = true;
							}
						}
					}
				}
			}
			this.m_pImageObj.AddName("Type", "XObject");
			this.m_pImageObj.AddName("Subtype", "Image");
			this.m_pImageObj.AddInt("Width", (int)this.m_fWidth);
			this.m_pImageObj.AddInt("Height", (int)this.m_fHeight);
			if (this.m_bstrColorSpace == "Lab")
			{
				PdfArray pdfArray = this.m_pImageObj.AddArray("ColorSpace");
				pdfArray.Add(new PdfName(null, "Lab"));
				PdfDict pdfDict = new PdfDict(null);
				PdfArray pdfArray2 = new PdfArray("WhitePoint");
				pdfArray2.Add(new PdfNumber(null, 1.0));
				pdfArray2.Add(new PdfNumber(null, 1.0));
				pdfArray2.Add(new PdfNumber(null, 1.0));
				pdfDict.Add(pdfArray2);
				PdfArray pdfArray3 = new PdfArray("Range");
				pdfArray3.Add(new PdfNumber(null, -128.0));
				pdfArray3.Add(new PdfNumber(null, 127.0));
				pdfArray3.Add(new PdfNumber(null, -128.0));
				pdfArray3.Add(new PdfNumber(null, 127.0));
				pdfDict.Add(pdfArray3);
				pdfArray.Add(pdfDict);
			}
			else
			{
				this.m_pImageObj.AddName("ColorSpace", this.m_bstrColorSpace);
			}
			this.m_pImageObj.AddInt("BitsPerComponent", this.m_nBitsPerComponent);
			this.m_pImageObj.AddInt("Length", this.m_pImageObj.m_objStream.Length);
			if (this.m_bInvert)
			{
				PdfArray pdfArray4 = this.m_pImageObj.AddArray("Decode");
				for (int i = 0; i < this.m_nComponentsPerSample; i++)
				{
					pdfArray4.Add(new PdfNumber(null, 1.0));
					pdfArray4.Add(new PdfNumber(null, 0.0));
				}
			}
			if (this.m_bInterpolate)
			{
				this.m_pImageObj.AddBool("Interpolate", true);
			}
			this.SetIndex();
			if (flag)
			{
				this.m_pImageObj.AddName("Filter", "DCTDecode");
			}
			else
			{
				int num = this.m_pImageObj.m_objStream.Encode(enumEncoding.PdfEncFlate);
				if (num != 0)
				{
					AuxException.Throw("Compression of an image failed.", PdfErrors._ERROR_COMPRESSIONFAILED);
				}
				this.m_pImageObj.AddName("Filter", "FlateDecode");
			}
			return result;
		}
		internal void SetIndex()
		{
			int num = 0;
			string text;
			do
			{
				num++;
				text = string.Format("Im{0}", num);
			}
			while (this.m_pDoc.m_listXObjectNames.GetObjectByName(text) != null);
			this.m_bstrID = text;
			this.m_pDoc.m_listXObjectNames.Add(new PdfName(text, null));
		}
		internal void OpenJpeg(string Path, byte[] pData)
		{
			PdfJpeg pdfJpeg = new PdfJpeg(this.m_pManager, this.m_pDoc);
			if (pData == null)
			{
				pdfJpeg.LoadFromFile(Path);
			}
			else
			{
				pdfJpeg.LoadFromMemory(pData);
			}
			this.m_bstrColorSpace = pdfJpeg.m_bstrColorSpace;
			this.m_nComponentsPerSample = pdfJpeg.m_nComponentsPerSample;
			this.m_nBitsPerComponent = pdfJpeg.m_nBitsPerComponent;
			this.m_pImageObj.m_objStream.Set(pdfJpeg.m_objData);
			this.m_fWidth = (float)pdfJpeg.m_nWidth;
			this.m_fHeight = (float)pdfJpeg.m_nHeight;
			this.m_fDefaultScaleX = pdfJpeg.m_fDefaultScaleX;
			this.m_fDefaultScaleY = pdfJpeg.m_fDefaultScaleY;
			this.m_fResolutionX = pdfJpeg.m_fResolutionX;
			this.m_fResolutionY = pdfJpeg.m_fResolutionY;
			this.m_bstrFormat = "JPG";
			pdfJpeg.Close();
		}
		internal void OpenBMP(string Path)
		{
			this.OpenBMPHelper(Path, null);
		}
		internal void OpenBMPBinary(byte[] pData)
		{
			this.OpenBMPHelper(null, pData);
		}
		internal void OpenBMPHelper(string Path, byte[] pData)
		{
			PdfBmp pdfBmp = new PdfBmp(this.m_pManager, this.m_pDoc);
			if (pData == null)
			{
				pdfBmp.LoadFromFile(Path);
			}
			else
			{
				pdfBmp.LoadFromMemory(pData);
			}
			this.m_bstrColorSpace = pdfBmp.m_bstrColorSpace;
			this.m_nComponentsPerSample = pdfBmp.m_nComponentsPerSample;
			this.m_nBitsPerComponent = pdfBmp.m_nBitsPerComponent;
			this.m_pImageObj.m_objStream.Set(pdfBmp.m_objData);
			this.m_fWidth = (float)pdfBmp.m_nWidth;
			this.m_fHeight = (float)pdfBmp.m_nHeight;
			this.m_bstrFormat = "BMP";
			pdfBmp.Close();
		}
		internal void OpenGif(string Path, byte[] pData)
		{
			PdfGif pdfGif = new PdfGif(this.m_pManager, this.m_pDoc);
			if (pData == null)
			{
				pdfGif.LoadFromFile(Path);
			}
			else
			{
				pdfGif.LoadFromMemory(pData);
			}
			this.m_bstrColorSpace = pdfGif.m_bstrColorSpace;
			this.m_nComponentsPerSample = pdfGif.m_nComponentsPerSample;
			this.m_nBitsPerComponent = pdfGif.m_nBitsPerComponent;
			this.m_pImageObj.m_objStream.Set(pdfGif.m_objData);
			this.m_fWidth = (float)pdfGif.m_nWidth;
			this.m_fHeight = (float)pdfGif.m_nHeight;
			this.m_fDefaultScaleX = pdfGif.m_fDefaultScaleX;
			this.m_fDefaultScaleY = pdfGif.m_fDefaultScaleY;
			this.m_fResolutionX = pdfGif.m_fResolutionX;
			this.m_fResolutionY = pdfGif.m_fResolutionY;
			this.m_bstrFormat = "GIF";
			if (pdfGif.m_bTransparent)
			{
				PdfIndirectObj pdfIndirectObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectImage);
				pdfIndirectObj.AddName("Type", "XObject");
				pdfIndirectObj.AddName("Subtype", "Image");
				pdfIndirectObj.AddInt("Width", pdfGif.m_nWidth);
				pdfIndirectObj.AddInt("Height", pdfGif.m_nHeight);
				pdfIndirectObj.AddInt("BitsPerComponent", 1);
				pdfIndirectObj.AddBool("ImageMask", true);
				pdfIndirectObj.m_objStream.Set(pdfGif.m_objTranspData);
				pdfIndirectObj.m_objStream.Encode(enumEncoding.PdfEncFlate);
				pdfIndirectObj.AddName("Filter", "FlateDecode");
				this.m_pImageObj.AddReference("Mask", pdfIndirectObj);
				this.m_bTransparencyColorSet = true;
				this.m_dwTransparencyColor = pdfGif.m_dwTransparencyColor;
			}
			pdfGif.Close();
		}
		internal bool OpenTiff(string Path, byte[] pData, ref bool pJpeg)
		{
			PdfTiff pdfTiff = new PdfTiff(this.m_pManager, this.m_pDoc);
			pdfTiff.m_nImageIndex = this.m_nTiffIndex;
			if (pData == null)
			{
				pdfTiff.LoadFromFile(Path);
			}
			else
			{
				pdfTiff.LoadFromMemory(pData);
			}
			pdfTiff.Close();
			if (pdfTiff.m_bPageIndexOutOfRange)
			{
				return false;
			}
			this.m_bstrColorSpace = pdfTiff.m_bstrColorSpace;
			this.m_nComponentsPerSample = pdfTiff.m_nComponentsPerSample;
			this.m_nBitsPerComponent = pdfTiff.m_nBitsPerComponent;
			this.m_pImageObj.m_objStream.Set(pdfTiff.m_objData);
			this.m_fWidth = (float)pdfTiff.m_nWidth;
			this.m_fHeight = (float)pdfTiff.m_nHeight;
			this.m_fDefaultScaleX = pdfTiff.m_fDefaultScaleX;
			this.m_fDefaultScaleY = pdfTiff.m_fDefaultScaleY;
			this.m_fResolutionX = pdfTiff.m_fResolutionX;
			this.m_fResolutionY = pdfTiff.m_fResolutionY;
			if (pdfTiff.m_nCompression == TiffCompressions.TIFF_COMPRESSION_JPEG)
			{
				pJpeg = true;
			}
			this.m_bstrFormat = "TIF";
			return true;
		}
		internal void OpenPng(string Path, byte[] pData)
		{
			PdfPng pdfPng = new PdfPng(this.m_pManager, this.m_pDoc);
			if (pData == null)
			{
				pdfPng.LoadFromFile(Path);
			}
			else
			{
				pdfPng.LoadFromMemory(pData);
			}
			this.m_bstrColorSpace = pdfPng.m_bstrColorSpace;
			this.m_nComponentsPerSample = pdfPng.m_nComponentsPerSample;
			this.m_nBitsPerComponent = pdfPng.m_nBitsPerComponent;
			this.m_pImageObj.m_objStream.Set(pdfPng.m_objData);
			this.m_fWidth = (float)pdfPng.m_nWidth;
			this.m_fHeight = (float)pdfPng.m_nHeight;
			this.m_fDefaultScaleX = pdfPng.m_fDefaultScaleX;
			this.m_fDefaultScaleY = pdfPng.m_fDefaultScaleY;
			this.m_fResolutionX = pdfPng.m_fResolutionX;
			this.m_fResolutionY = pdfPng.m_fResolutionY;
			this.m_bstrFormat = "PNG";
			if (pdfPng.m_bAlpha || pdfPng.m_bTransparency)
			{
				PdfIndirectObj pdfIndirectObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectImage);
				pdfIndirectObj.AddName("Type", "XObject");
				pdfIndirectObj.AddName("Subtype", "Image");
				pdfIndirectObj.AddName("ColorSpace", "DeviceGray");
				pdfIndirectObj.AddInt("Width", pdfPng.m_nWidth);
				pdfIndirectObj.AddInt("Height", pdfPng.m_nHeight);
				pdfIndirectObj.AddInt("BitsPerComponent", (pdfPng.m_nBitsPerComponent > 8) ? 8 : pdfPng.m_nBitsPerComponent);
				pdfIndirectObj.m_objStream.Set(pdfPng.m_pAlphaData);
				pdfIndirectObj.m_objStream.Encode(enumEncoding.PdfEncFlate);
				pdfIndirectObj.AddName("Filter", "FlateDecode");
				this.m_pImageObj.AddReference("SMask", pdfIndirectObj);
			}
			pdfPng.Close();
		}
		public void SetColorMask(object Param)
		{
			PdfParam pdfParam = this.m_pManager.VariantToParam(Param);
			int i = this.m_nBitsPerComponent;
			long num = 1L;
			while (i > 0)
			{
				num *= 2L;
				i--;
			}
			PdfObject objectByName = this.m_pImageObj.m_objAttributes.GetObjectByName("Mask");
			if (objectByName != null && objectByName.m_nType == enumType.pdfReference)
			{
				this.ClearMask();
			}
			PdfArray pdfArray = this.m_pImageObj.AddArray("Mask");
			for (int j = 0; j < this.m_nComponentsPerSample; j++)
			{
				string name = string.Format("Min{0}", j + 1);
				string name2 = string.Format("Max{0}", j + 1);
				if (!pdfParam.IsSet(name) || !pdfParam.IsSet(name2))
				{
					AuxException.Throw("You must specify values for Min1, Max1, ..., MinN, MaxN where N is the number of color components per sample.", PdfErrors._ERROR_OUTOFRANGE);
				}
				int num2 = pdfParam.Long(name);
				int num3 = pdfParam.Long(name2);
				if (num2 < 0 || (long)num2 >= num || num3 < 0 || (long)num3 >= num)
				{
					AuxException.Throw("Min and Max values must be between 0 and (2 ^ BitsPerComponent - 1).", PdfErrors._ERROR_OUTOFRANGE);
				}
				pdfArray.Add(new PdfNumber(null, (double)num2));
				pdfArray.Add(new PdfNumber(null, (double)num3));
			}
		}
		public void ClearMask()
		{
			this.m_pImageObj.m_objAttributes.RemoveByName("Mask");
		}
		public void SetImageMask(PdfImage Image)
		{
			if (Image == null)
			{
				AuxException.Throw("Image argument is empty.", PdfErrors._ERROR_OUTOFRANGE);
			}
			if (!Image.m_bIsMask)
			{
				AuxException.Throw("Image object must have the IsMask property set to True.", PdfErrors._ERROR_OUTOFRANGE);
			}
			this.m_pImageObj.AddReference("Mask", Image.m_pImageObj);
		}
		public void SetColorSpace(PdfColorSpace ColorSpace)
		{
			if (ColorSpace == null)
			{
				AuxException.Throw("ColorSpace parameter is empty.", PdfErrors._ERROR_OUTOFRANGE);
			}
			if (ColorSpace.m_nComponents != (long)this.m_nComponentsPerSample)
			{
				AuxException.Throw("The number of color components in the image and colorspace must match.", PdfErrors._ERROR_OUTOFRANGE);
			}
			ColorSpace.Validate();
			this.m_pImageObj.m_objAttributes.RemoveByName("ColorSpace");
			this.m_pImageObj.AddReference("ColorSpace", ColorSpace.m_pCSObj);
		}
	}
}
