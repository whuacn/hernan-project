using System;
namespace Persits.PDF
{
	internal class PdfJpeg : PdfTiff
	{
		public PdfJpeg(PdfManager pManager, PdfDocument pDoc) : base(pManager, pDoc)
		{
			this.m_bBigEndian = true;
		}
		public override void ParseImage()
		{
			if (base.ReadShort() != 65496)
			{
				AuxException.Throw("Not a valid JPEG image. Expecting 0xFF 0xD8.", PdfErrors._ERROR_JPEG_2);
			}
			bool flag = false;
			int num = 72;
			int num2 = 72;
			int num3 = 0;
			while (true)
			{
				ushort num4 = base.ReadShort();
				if (num4 == 65497 || num4 == 65498)
				{
					goto IL_DF;
				}
				int num5 = (int)(base.ReadShort() - 2);
				int dwPtr = this.m_dwPtr;
				if (num4 == 65504)
				{
					this.m_dwPtr += 7;
					num3 = (int)base.ReadByte();
					num = (int)base.ReadShort();
					num2 = (int)base.ReadShort();
				}
				if (num4 == 65472 || num4 == 65474)
				{
					break;
				}
				this.m_dwPtr = dwPtr + num5;
				if ((num4 | 255) != 65535)
				{
					goto IL_DF;
				}
			}
			this.m_nBitsPerComponent = (int)base.ReadByte();
			this.m_nHeight = (int)base.ReadShort();
			this.m_nWidth = (int)base.ReadShort();
			this.m_nComponentsPerSample = (int)base.ReadByte();
			flag = true;
			IL_DF:
			if (!flag)
			{
				AuxException.Throw("This does not appear to be a valid JPEG image.", PdfErrors._ERROR_JPEG_2);
			}
			byte[] array = new byte[this.m_pInput.m_nSize];
			this.m_pInput.ReadBytes(0, array, this.m_pInput.m_nSize);
			this.m_objData.Set(array);
			switch (this.m_nComponentsPerSample)
			{
			case 1:
				this.m_bstrColorSpace = "DeviceGray";
				goto IL_16F;
			case 4:
				this.m_bstrColorSpace = "DeviceCMYK";
				goto IL_16F;
			}
			this.m_bstrColorSpace = "DeviceRGB";
			IL_16F:
			this.m_fResolutionX = (float)((num3 == 0) ? 72 : num);
			this.m_fResolutionY = (float)((num3 == 0) ? 72 : num2);
			if (num3 == 2)
			{
				this.m_fResolutionX *= 2.54f;
				this.m_fResolutionY *= 2.54f;
			}
			if (this.m_fResolutionX > 0f && this.m_fResolutionY > 0f)
			{
				this.m_fDefaultScaleX = 72f / this.m_fResolutionX;
				this.m_fDefaultScaleY = 72f / this.m_fResolutionY;
			}
		}
	}
}
