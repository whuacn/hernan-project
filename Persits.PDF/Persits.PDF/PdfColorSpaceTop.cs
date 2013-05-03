using System;
namespace Persits.PDF
{
	internal abstract class PdfColorSpaceTop
	{
		internal abstract enumColorSpaceMode getMode();
		internal abstract void getGray(PdfColor color, PdfGray gray);
		internal abstract void getRGB(PdfColor color, PdfRGB rgb);
		internal abstract void getCMYK(PdfColor color, PdfCMYK cmyk);
		internal abstract void getDefaultColor(PdfColor color);
		internal abstract PdfColorSpaceTop copy();
		internal abstract int getNComps();
		internal bool isNonMarking()
		{
			return false;
		}
		internal virtual void getDefaultRanges(double[] decodeLow, double[] decodeRange, int maxImgPixel)
		{
			for (int i = 0; i < this.getNComps(); i++)
			{
				decodeLow[i] = 0.0;
				decodeRange[i] = 1.0;
			}
		}
		internal static PdfColorSpaceTop parse(PdfObject pObj, PdfPreview pPreview)
		{
			PdfColorSpaceTop result = null;
			if (pObj.m_nType == enumType.pdfName)
			{
				PdfName pdfName = (PdfName)pObj;
				if (string.Compare(pdfName.m_bstrName, "DeviceGray") == 0 || string.Compare(pdfName.m_bstrName, "G") == 0)
				{
					result = new PdfDeviceGrayColorSpace();
				}
				else
				{
					if (string.Compare(pdfName.m_bstrName, "DeviceRGB") == 0 || string.Compare(pdfName.m_bstrName, "RGB") == 0)
					{
						result = new PdfDeviceRGBColorSpace();
					}
					else
					{
						if (string.Compare(pdfName.m_bstrName, "DeviceCMYK") == 0 || string.Compare(pdfName.m_bstrName, "CMYK") == 0)
						{
							result = new PdfDeviceCMYKColorSpace(pPreview);
						}
						else
						{
							if (string.Compare(pdfName.m_bstrName, "Pattern") == 0)
							{
								result = new PdfPatternColorSpace(null);
							}
							else
							{
								string err = string.Format("Invalid color space '{0}'.", pdfName.m_bstrName);
								AuxException.Throw(err, PdfErrors._ERROR_PREVIEW_PARSE);
							}
						}
					}
				}
			}
			else
			{
				if (pObj.m_nType == enumType.pdfArray)
				{
					PdfObject @object = ((PdfArray)pObj).GetObject(0);
					if (@object.m_nType != enumType.pdfName)
					{
						return null;
					}
					PdfName pdfName2 = (PdfName)@object;
					if (string.Compare(pdfName2.m_bstrName, "DeviceGray") == 0 || string.Compare(pdfName2.m_bstrName, "G") == 0)
					{
						result = new PdfDeviceGrayColorSpace();
					}
					else
					{
						if (string.Compare(pdfName2.m_bstrName, "DeviceRGB") == 0 || string.Compare(pdfName2.m_bstrName, "RGB") == 0)
						{
							result = new PdfDeviceRGBColorSpace();
						}
						else
						{
							if (string.Compare(pdfName2.m_bstrName, "DeviceCMYK") == 0 || string.Compare(pdfName2.m_bstrName, "CMYK") == 0)
							{
								result = new PdfDeviceCMYKColorSpace(pPreview);
							}
							else
							{
								if (string.Compare(pdfName2.m_bstrName, "CalGray") == 0)
								{
									result = PdfCalGrayColorSpace.parse((PdfArray)pObj);
								}
								else
								{
									if (string.Compare(pdfName2.m_bstrName, "CalRGB") == 0)
									{
										result = PdfCalRGBColorSpace.parse((PdfArray)pObj);
									}
									else
									{
										if (string.Compare(pdfName2.m_bstrName, "Lab") == 0)
										{
											result = PdfLabColorSpace.parse((PdfArray)pObj);
										}
										else
										{
											if (string.Compare(pdfName2.m_bstrName, "ICCBased") == 0)
											{
												result = PdfICCBasedColorSpace.parse((PdfArray)pObj, pPreview);
											}
											else
											{
												if (string.Compare(pdfName2.m_bstrName, "Indexed") == 0 || string.Compare(pdfName2.m_bstrName, "I") == 0)
												{
													result = PdfIndexedColorSpace.parse((PdfArray)pObj, pPreview);
												}
												else
												{
													if (string.Compare(pdfName2.m_bstrName, "Separation") == 0)
													{
														result = PdfSeparationColorSpace.parse((PdfArray)pObj, pPreview);
													}
													else
													{
														if (string.Compare(pdfName2.m_bstrName, "DeviceN") == 0)
														{
															result = PdfDeviceNColorSpace.parse((PdfArray)pObj, pPreview);
														}
														else
														{
															if (string.Compare(pdfName2.m_bstrName, "Pattern") == 0)
															{
																result = PdfPatternColorSpace.parse((PdfArray)pObj, pPreview);
															}
															else
															{
																AuxException.Throw("Invalid color space.", PdfErrors._ERROR_PREVIEW_PARSE);
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
				}
				else
				{
					AuxException.Throw("Invalid color space - should be name or array.", PdfErrors._ERROR_PREVIEW_PARSE);
				}
			}
			return result;
		}
	}
}
