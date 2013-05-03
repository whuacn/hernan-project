// Type: Persits.PDF.HtmlManager
// Assembly: Persits.PDF, Version=2.5.0.19989, Culture=neutral, PublicKeyToken=b8d8c63f1ff142a8
// Assembly location: C:\Users\hernan\Desktop\asppdf_net\Samples\Bin\Persits.PDF.dll

using System;
using System.Collections.Generic;
using System.Text;

namespace Persits.PDF
{
  internal class HtmlManager
  {
    private static string[] CharSets;
    private static int[] CharSetCodes;
    public PdfManager m_pManager;
    public PdfDocument m_pDocument;
    public PdfParam m_pParam;
    public StringBuilder m_bstrLog;
    public float m_fDefaultPageHeight;
    public float m_fOriginalPageWidth;
    private float m_fLeftMargin;
    private float m_fTopMargin;
    private float m_fBottomMargin;
    private float m_fRightMargin;
    private float m_fPageWidth;
    private float m_fPageHeight;
    public bool m_bNonRenderedItemsExist;
    public float m_fFrameY;
    public float m_fFrameHeight;
    public float m_fNextFrameY;
    public float m_fLowerMargin;
    public float m_fShiftX;
    public float m_fShiftY;
    public float m_fScale;
    public bool m_bLandscape;
    public int m_nStartPage;
    public int m_nCodePage;
    public int m_nStopAfter;
    public bool m_bDrawBackground;
    public bool m_bEnableHyperlinks;
    public bool m_bBackgroundObtained;
    public ushort m_nMediaFlags;
    public AuxRGB m_rgbBgColor;
    public HtmlImage m_pBkImage;
    public CssProperties m_nRepeatStyle;
    public CssProperties m_nRepeatPositionX;
    public CssProperties m_nRepeatPositionY;
    public bool m_bDebug;
    public bool m_bDirectHTML;
    public bool m_bSplitImages;
    public int m_nTimeout;
    public bool m_bInvertCMYK;
    public char[] m_pBuffer;
    public int m_nBufferSize;
    public int m_nPtr;
    public char m_c;
    public HtmlCSS m_objCSS;
    public HtmlTable m_pMasterTable;
    public HtmlXmlUrl m_pBaseUrl;
    public byte[] m_objContent;
    private List<ImageCacheStruct> m_arrImageCache;
    private List<FontCacheStruct> m_arrFontCache;
    public static float fEmpiricalScaleFactor;
    public static int TAB_SHIFT;
    public float m_fLowestYCoordinate;
    public int m_nLowestPageNumber;
    public bool m_bImportInfoSet;
    public List<PdfAnnot> m_arrBookmarkAnnots;
    public List<PdfDest> m_arrBookmarkDests;

    static HtmlManager()
    {
      HtmlManager.CharSets = new string[49]
      {
        "UTF-8",
        "UTF8",
        "UTF-7",
        "UTF7",
        "windows-1250",
        "cp1250",
        "windows-1251",
        "cp1251",
        "ascii",
        "windows-1252",
        "cp1252",
        "us-ascii",
        "windows-1253",
        "cp1253",
        "windows-1254",
        "cp1254",
        "windows-1255",
        "cp1255",
        "windows-1256",
        "cp1256",
        "windows-1257",
        "cp1257",
        "windows-1258",
        "cp1258",
        "ISO-8859-1",
        "ISO-8859-2",
        "ISO-8859-3",
        "ISO-8859-4",
        "ISO-8859-5",
        "ISO-8859-6",
        "ISO-8859-7",
        "ISO-8859-8",
        "ISO-8859-9",
        "ISO-8859-15",
        "cp866",
        "koi8-r",
        "koi8-u",
        "shift_jis",
        "ks_c_5601-1987",
        "korean",
        "EUC-KR",
        "BIG5",
        "GB2312",
        "chinese",
        "HZ-GB-2312",
        "EUC-JP",
        "iso-2022-jp",
        "csISO2022JP",
        "X-EUC-TW"
      };
      HtmlManager.CharSetCodes = new int[49]
      {
        65001,
        65001,
        65000,
        65000,
        1250,
        1250,
        1251,
        1251,
        1252,
        1252,
        1252,
        1252,
        1253,
        1253,
        1254,
        1254,
        1255,
        1255,
        1256,
        1256,
        1257,
        1257,
        1258,
        1258,
        28591,
        28592,
        28593,
        28594,
        28595,
        28596,
        28597,
        28598,
        28599,
        28605,
        866,
        20866,
        21866,
        932,
        949,
        949,
        51949,
        950,
        936,
        936,
        52936,
        51932,
        50220,
        50221,
        51950
      };
      HtmlManager.fEmpiricalScaleFactor = 0.75f;
      HtmlManager.TAB_SHIFT = 30;
    }

    public HtmlManager()
    {
      this.m_bstrLog = new StringBuilder();
      this.m_rgbBgColor = new AuxRGB();
      this.m_objCSS = new HtmlCSS();
      this.m_arrImageCache = new List<ImageCacheStruct>();
      this.m_arrFontCache = new List<FontCacheStruct>();
      this.m_arrBookmarkAnnots = new List<PdfAnnot>();
      this.m_arrBookmarkDests = new List<PdfDest>();
      HtmlTag pTag = new HtmlTag(this);
      pTag.m_nType = TagType.TAG_UNKNOWN;
      this.m_pMasterTable = new HtmlTable(this, (HtmlCell) null, pTag);
      this.m_pMasterTable.m_bAbsolutePosition = true;
      HtmlCell htmlCell = this.m_pMasterTable.AddCell(pTag);
      htmlCell.m_arrPadding[0] = htmlCell.m_arrPadding[1] = htmlCell.m_arrPadding[2] = htmlCell.m_arrPadding[3] = 0.0f;
      this.m_objCSS.m_pHtmlManager = this;
      this.m_fPageHeight = 792f;
      this.m_fPageWidth = 612f;
      this.m_fLeftMargin = this.m_fTopMargin = this.m_fBottomMargin = this.m_fRightMargin = 54f;
      this.m_nStopAfter = 200;
      this.m_fScale = 1f;
      this.m_nStartPage = 1;
      this.m_bDebug = false;
      this.m_bDirectHTML = false;
      this.m_bInvertCMYK = false;
      this.m_bSplitImages = false;
      this.m_nMediaFlags = (ushort) byte.MaxValue;
      this.m_nTimeout = 0;
      this.m_bNonRenderedItemsExist = true;
      this.m_pBuffer = (char[]) null;
      this.m_bLandscape = false;
      this.m_bDrawBackground = false;
      this.m_bBackgroundObtained = false;
      this.m_bEnableHyperlinks = false;
      this.m_pBkImage = (HtmlImage) null;
      this.m_nRepeatStyle = CssProperties.BACKGROUND_REPEAT;
      this.m_objCSS.InitializeWithDefaultRules();
      this.m_pBaseUrl = (HtmlXmlUrl) null;
      this.m_fLowestYCoordinate = 0.0f;
    }

    public HtmlManager(bool b)
    {
      this.m_bstrLog = new StringBuilder();
      this.m_rgbBgColor = new AuxRGB();
      this.m_objCSS = new HtmlCSS();
      this.m_arrImageCache = new List<ImageCacheStruct>();
      this.m_arrFontCache = new List<FontCacheStruct>();
      this.m_arrBookmarkAnnots = new List<PdfAnnot>();
      this.m_arrBookmarkDests = new List<PdfDest>();
    }

    public void OpenUrl(string Url, string Username, string Password)
    {
      this.m_pParam.GetNumberIfSet("PageWidth", 0, ref this.m_fPageWidth);
      this.m_pParam.GetNumberIfSet("PageHeight", 0, ref this.m_fPageHeight);
      this.m_pParam.GetNumberIfSet("LeftMargin", 0, ref this.m_fLeftMargin);
      this.m_pParam.GetNumberIfSet("RightMargin", 0, ref this.m_fRightMargin);
      this.m_pParam.GetNumberIfSet("TopMargin", 0, ref this.m_fTopMargin);
      this.m_pParam.GetNumberIfSet("BottomMargin", 0, ref this.m_fBottomMargin);
      this.m_pParam.GetNumberIfSet("LeftMargin1", 0, ref this.m_fLeftMargin);
      this.m_pParam.GetNumberIfSet("RightMargin1", 0, ref this.m_fRightMargin);
      this.m_pParam.GetNumberIfSet("TopMargin1", 0, ref this.m_fTopMargin);
      this.m_pParam.GetNumberIfSet("BottomMargin1", 0, ref this.m_fBottomMargin);
      if (this.m_pParam.IsSet("StopAfter"))
        this.m_nStopAfter = (int) this.m_pParam.Number("StopAfter");
      if (this.m_pParam.IsSet("DrawBackground"))
        this.m_bDrawBackground = this.m_pParam.Bool("DrawBackground");
      if (this.m_pParam.IsSet("StartPage"))
        this.m_nStartPage = (int) this.m_pParam.Number("StartPage");
      if (this.m_pParam.IsSet("Timeout"))
        this.m_nTimeout = (int) this.m_pParam.Number("Timeout") * 1000;
      if (this.m_pParam.IsSet("Debug"))
        this.m_bDebug = this.m_pParam.Bool("Debug");
      if (this.m_pParam.IsSet("SplitImages"))
        this.m_bSplitImages = this.m_pParam.Bool("SplitImages");
      if (this.m_pParam.IsSet("InvertCMYK"))
        this.m_bInvertCMYK = this.m_pParam.Bool("InvertCMYK");
      if ((double) this.m_fPageHeight - (double) this.m_fTopMargin - (double) this.m_fBottomMargin <= 0.0)
        AuxException.Throw("The sum of TopMargin and BottomMargin parameters cannot exceed PageHeight.", PdfErrors._ERROR_INVALIDARG);
      if ((double) this.m_fPageWidth - (double) this.m_fLeftMargin - (double) this.m_fRightMargin <= 0.0)
        AuxException.Throw("The sum of LeftMargin and RightMargin parameters cannot exceed PageWidth.", PdfErrors._ERROR_INVALIDARG);
      this.m_bLandscape = this.m_pParam.IsTrue("Landscape");
      this.m_bEnableHyperlinks = this.m_pParam.IsTrue("Hyperlinks");
      if (this.m_pParam.IsSet("Media"))
        this.m_nMediaFlags = (ushort) this.m_pParam.Number("Media");
      this.m_pParam.GetNumberIfSet("Scale", 0, ref this.m_fScale);
      if ((double) this.m_fScale <= 0.0)
        AuxException.Throw("The Scale parameter must be a positive number.", PdfErrors._ERROR_INVALIDARG);
      this.ApplyScale(ref this.m_fPageWidth, ref this.m_fPageHeight, ref this.m_fLeftMargin, ref this.m_fRightMargin, ref this.m_fTopMargin, ref this.m_fBottomMargin);
      if (Url != null && (Url.IndexOf("<HTM") >= 0 || Url.IndexOf("<html") >= 0))
        this.m_bDirectHTML = true;
      if (this.m_bDirectHTML)
      {
        int length = Url.Length;
        this.m_pBuffer = new char[length + 10];
        Array.Copy((Array) Url.ToCharArray(), (Array) this.m_pBuffer, length);
        this.m_pBuffer[length] = ' ';
        this.m_pBuffer[length + 1] = char.MinValue;
        this.m_nBufferSize = length + 1;
      }
      try
      {
        this.m_pBaseUrl = new HtmlXmlUrl(Url, Username, Password, this);
        this.m_pBaseUrl.ReadUrl(ref this.m_objContent);
        this.m_nPtr = 0;
        this.ConvertToUnicode();
        this.m_pMasterTable.m_arrCells[0].X = 0.0f;
        this.m_pMasterTable.m_arrCells[0].Y = 0.0f;
        this.m_pMasterTable.m_arrCells[0].Width = this.m_bLandscape ? this.m_fPageHeight - this.m_fTopMargin - this.m_fBottomMargin : this.m_fPageWidth - this.m_fLeftMargin - this.m_fRightMargin;
        this.m_fOriginalPageWidth = this.m_pMasterTable.m_arrCells[0].Width;
        this.m_fDefaultPageHeight = this.m_bLandscape ? this.m_fPageWidth - this.m_fLeftMargin - this.m_fRightMargin : this.m_fPageHeight - this.m_fTopMargin - this.m_fBottomMargin;
        this.m_pMasterTable.m_arrCells[0].Next();
        this.m_pMasterTable.m_arrCells[0].Parse();
      }
      catch (PdfException ex)
      {
        if (ex.m_nCode != 55)
          throw ex;
      }
    }

    public void Test(string strUrl)
    {
      new HtmlXmlUrl(strUrl, (string) null, (string) null, this).ReadUrl(ref this.m_objContent);
      this.ConvertToUnicode();
    }

    private void ApplyScale(ref float p1, ref float p2, ref float p3, ref float p4, ref float p5, ref float p6)
    {
      p1 /= this.m_fScale;
      p2 /= this.m_fScale;
      p3 /= this.m_fScale;
      p4 /= this.m_fScale;
      p5 /= this.m_fScale;
      p6 /= this.m_fScale;
    }

    private void ApplyScale(ref float p1)
    {
      p1 /= this.m_fScale;
    }

    public void ConvertToUnicode()
    {
      if (this.m_bDirectHTML)
        return;
      this.m_nCodePage = 1252;
      if (this.m_pParam.IsSet("CodePage"))
        this.m_nCodePage = (int) this.m_pParam.Number("CodePage");
      else if (this.m_objContent.Length >= 2 && (int) this.m_objContent[0] + (int) this.m_objContent[1] == 509)
      {
        int length = this.m_objContent.Length / 2;
        bool flag = (int) this.m_objContent[0] == (int) byte.MaxValue;
        this.m_pBuffer = new char[length];
        int num;
        for (num = 1; num < length; ++num)
          this.m_pBuffer[num - 1] = !flag ? (char) ((uint) this.m_objContent[num * 2] * 256U + (uint) this.m_objContent[num * 2 + 1]) : (char) ((uint) this.m_objContent[num * 2] + (uint) this.m_objContent[num * 2 + 1] * 256U);
        this.m_pBuffer[num - 1] = char.MinValue;
        this.m_nBufferSize = num - 1;
        return;
      }
      else if (this.m_objContent.Length >= 3 && (int) this.m_objContent[0] == 239 && ((int) this.m_objContent[1] == 187 && (int) this.m_objContent[2] == 191))
      {
        this.m_pBuffer = Encoding.UTF8.GetChars(this.m_objContent, 3, this.m_objContent.Length - 3);
        this.m_nBufferSize = this.m_pBuffer.Length;
        return;
      }
      else
      {
        byte[] bytes1 = Encoding.UTF8.GetBytes("<META");
        byte[] bytes2 = Encoding.UTF8.GetBytes(">");
        int nFrom = 0;
        bool flag = false;
        while (true)
        {
          int stringIgnoreCase1 = PdfStream.FindStringIgnoreCase(this.m_objContent, nFrom, bytes1);
          if (stringIgnoreCase1 != -1)
          {
            int stringIgnoreCase2 = PdfStream.FindStringIgnoreCase(this.m_objContent, stringIgnoreCase1, bytes2);
            if (stringIgnoreCase2 != -1)
            {
              string str = Encoding.UTF8.GetString(this.m_objContent, stringIgnoreCase1, stringIgnoreCase2 - stringIgnoreCase1 + 1).ToLower();
              int num1;
              if (str.IndexOf("http-equiv") >= 0 && str.IndexOf("content-type") >= 0 && (num1 = str.IndexOf("charset")) >= 0)
              {
                int index1 = num1 + 7;
                while ((int) str[index1] == 32)
                  ++index1;
                if ((int) str[index1] == 61)
                {
                  int index2 = index1 + 1;
                  while ((int) str[index2] == 32)
                    ++index2;
                  StringBuilder stringBuilder = new StringBuilder();
                  int num2 = 0;
                  while (index2 < str.Length && (int) str[index2] != 32 && ((int) str[index2] != 59 && (int) str[index2] != 44) && ((int) str[index2] != 34 && num2 < 20))
                    stringBuilder.Append(str[index2++]);
                  this.CharsetToCodePage(((object) stringBuilder).ToString(), ref this.m_nCodePage);
                }
                flag = true;
              }
              if (!flag)
                nFrom = stringIgnoreCase2 + 1;
              else
                break;
            }
            else
              break;
          }
          else
            break;
        }
      }
      this.m_pBuffer = Encoding.GetEncoding(this.m_nCodePage).GetString(this.m_objContent).ToCharArray();
      this.m_nBufferSize = this.m_pBuffer.Length;
    }

    private void CharsetToCodePage(string charset, ref int CodePage)
    {
      if (charset.Length == 0)
        return;
      for (int index = 0; index < HtmlManager.CharSets.Length; ++index)
      {
        if (string.Compare(HtmlManager.CharSets[index], charset, true) == 0)
        {
          CodePage = HtmlManager.CharSetCodes[index];
          break;
        }
      }
    }

    public void AssignCoordinates()
    {
      while (true)
      {
        try
        {
          this.m_pMasterTable.m_arrCells[0].X = 0.0f;
          this.m_pMasterTable.m_arrCells[0].Y = 0.0f;
          this.m_pMasterTable.m_arrCells[0].m_fCurX = this.m_pMasterTable.m_arrCells[0].X;
          this.m_pMasterTable.m_arrCells[0].m_fCurY = this.m_pMasterTable.m_arrCells[0].Y;
          this.m_pMasterTable.m_arrCells[0].AssignCoordinates();
          break;
        }
        catch (PdfException ex)
        {
          int num = ex.m_nCode;
          this.m_pMasterTable.m_arrCells[0].chgWidth(ex.m_fWidth);
          this.m_pMasterTable.m_arrCells[0].ResetInvisibleObjects();
          this.m_pMasterTable.m_arrCells[0].ClearArrays();
        }
      }
    }

    public void Render()
    {
      PdfPages pages = this.m_pDocument.Pages;
      int count = pages.Count;
      float Width = this.m_fPageWidth * this.m_fScale;
      float Height = this.m_fPageHeight * this.m_fScale;
      this.m_bBackgroundObtained = false;
      this.m_fFrameY = 0.0f;
      this.m_fNextFrameY = 0.0f;
      PdfPage pPage;
      if (count == 0)
      {
        pPage = pages.Add(Width, Height);
      }
      else
      {
        if (this.m_nStartPage > count)
          this.m_nStartPage = count;
        pPage = pages[this.m_nStartPage];
      }
      int num = 0;
      while (true)
      {
        ++num;
        if (this.m_pParam.GetNumberIfSet("LeftMargin%d", num, ref this.m_fLeftMargin))
          this.ApplyScale(ref this.m_fLeftMargin);
        if (this.m_pParam.GetNumberIfSet("RightMargin%d", num, ref this.m_fRightMargin))
          this.ApplyScale(ref this.m_fRightMargin);
        if (this.m_pParam.GetNumberIfSet("TopMargin%d", num, ref this.m_fTopMargin))
          this.ApplyScale(ref this.m_fTopMargin);
        if (this.m_pParam.GetNumberIfSet("BottomMargin%d", num, ref this.m_fBottomMargin))
          this.ApplyScale(ref this.m_fBottomMargin);
        if ((double) Math.Abs(this.m_fOriginalPageWidth - (!this.m_bLandscape ? this.m_fPageWidth - this.m_fLeftMargin - this.m_fRightMargin : this.m_fPageHeight - this.m_fTopMargin - this.m_fBottomMargin)) > 0.0001)
          AuxException.Throw("When overriding default page margin parameters, the effective page width must be kept constant.", PdfErrors._ERROR_INVALIDARG);
        this.m_fFrameHeight = this.m_bLandscape ? this.m_fPageWidth - this.m_fLeftMargin - this.m_fRightMargin : this.m_fPageHeight - this.m_fTopMargin - this.m_fBottomMargin;
        this.m_fLowerMargin = this.m_bLandscape ? this.m_fRightMargin : this.m_fBottomMargin;
        this.m_fNextFrameY += this.m_fFrameHeight;
        PdfCanvas canvas = pPage.Canvas;
        canvas.SaveState();
        canvas.AddRect(this.m_fLeftMargin * this.m_fScale, this.m_fBottomMargin * this.m_fScale, (this.m_fPageWidth - this.m_fLeftMargin - this.m_fRightMargin) * this.m_fScale, (this.m_fPageHeight - this.m_fTopMargin - this.m_fBottomMargin) * this.m_fScale);
        canvas.Clip(false);
        this.m_bNonRenderedItemsExist = false;
        this.m_fShiftY = this.m_fPageHeight + this.m_fFrameY - this.m_fTopMargin;
        this.m_fShiftX = this.m_fLeftMargin;
        this.m_fLowestYCoordinate = this.m_fPageHeight;
        if ((double) this.m_fScale != 1.0)
          canvas.SetCTM(this.m_fScale, 0.0f, 0.0f, this.m_fScale, 0.0f, 0.0f);
        if (this.m_bLandscape)
        {
          pPage.Rotate = 90;
          canvas.SetCTM(0.0f, 1f, -1f, 0.0f, this.m_fPageWidth, 0.0f);
          this.m_fShiftY = this.m_fPageWidth - this.m_fLeftMargin + this.m_fFrameY;
          this.m_fShiftX = this.m_fBottomMargin;
        }
        this.DrawBackground(canvas, pPage, num);
        try
        {
          this.m_pMasterTable.m_arrCells[0].Render(pPage, canvas, this.m_fShiftX, 0.0f);
        }
        catch (PdfException ex)
        {
        }
        canvas.RestoreState();
        if (this.m_bNonRenderedItemsExist)
        {
          if (pPage.m_nIndex + 1 <= this.m_nStopAfter)
          {
            this.m_fFrameY = this.m_fNextFrameY;
            pPage = pPage.NextPage;
          }
          else
            break;
        }
        else
          goto label_28;
      }
      if (!this.m_bDebug)
        return;
      this.m_bstrLog.Append("The number of pages has exceeded an allowed maximum. Try specifying a larger value for the StopAfter parameter (200 by default).");
      return;
label_28:
      this.m_nLowestPageNumber = this.m_nStartPage + num - 1;
      this.m_bImportInfoSet = true;
      this.HandleBookmarks();
    }

    public void AddToImageCache(string Url, PdfImage pPtr)
    {
      if (Url == null || Url.Length == 0 || pPtr == null)
        return;
      ImageCacheStruct imageCacheStruct = new ImageCacheStruct();
      imageCacheStruct.m_bstrUrl = Url;
      imageCacheStruct.m_piImagePtr = pPtr;
      this.m_arrImageCache.Add(imageCacheStruct);
    }

    public bool LookUpImageCache(string Url, ref PdfImage ppPtr)
    {
      ppPtr = (PdfImage) null;
      if (Url == null || Url.Length == 0)
        return false;
      foreach (ImageCacheStruct imageCacheStruct in this.m_arrImageCache)
      {
        if (string.Compare(imageCacheStruct.m_bstrUrl, Url, true) == 0)
        {
          ppPtr = imageCacheStruct.m_piImagePtr;
          return true;
        }
      }
      return false;
    }

    public void HandleSkippableContainer(HtmlTag pTag, HtmlCell pCell, HtmlCell pCurrentCell)
    {
      if (pCell.m_nType == TagType.TAG_TITLE)
      {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (HtmlObject htmlObject in pCell.m_arrObjects)
        {
          if (htmlObject.Type == ObjectType.htmlWord)
          {
            foreach (HtmlChunk htmlChunk in ((HtmlWord) htmlObject).m_arrChunks)
              stringBuilder.Append((object) htmlChunk.m_bstrText);
          }
        }
        this.m_pDocument.Title = ((object) stringBuilder).ToString();
      }
      if (pCell.m_nType != TagType.TAG_SELECT)
        return;
      pCell.HandleSelectOption((HtmlTag) null);
      HtmlTag pTag1 = new HtmlTag(this);
      pTag1.m_nType = TagType.TAG_INPUT;
      pTag1.m_arrNames.Add("value");
      pTag1.m_arrValues.Add(pCell.m_bstrOptionValue);
      pTag1.m_arrNames.Add("type");
      pTag1.m_arrValues.Add("_select");
      if (pCell.m_bSelectMultiple)
      {
        pTag1.m_arrNames.Add("multiple");
        pTag1.m_arrValues.Add("");
        string pVal = (string) null;
        if (pTag.GetStringAttribute("size", ref pVal))
        {
          pTag1.m_arrNames.Add("size");
          pTag1.m_arrValues.Add(pVal);
        }
      }
      pCurrentCell.AddImage(pTag1);
    }

    private void DrawBackground(PdfCanvas pCanvas, PdfPage pPage, int nCount)
    {
      if (!this.m_bDrawBackground)
        return;
      HtmlTreeItem htmlTreeItem = this.m_objCSS.m_pRoot;
      if (htmlTreeItem == null || htmlTreeItem.m_nType != TagType.TAG_BODY)
        return;
      if (!this.m_bBackgroundObtained)
      {
        htmlTreeItem.GetColorProperty(CssProperties.BACKGROUND_COLOR, ref this.m_rgbBgColor, false);
        string pValue = (string) null;
        this.m_pBkImage = (HtmlImage) null;
        if (htmlTreeItem.GetStringProperty(CssProperties.BACKGROUND_IMAGE, ref pValue, false))
        {
          this.m_nRepeatStyle = CssProperties.BACKGROUND_REPEAT;
          htmlTreeItem.GetEnumProperty(CssProperties.BACKGROUND_REPEAT, ref this.m_nRepeatStyle, false);
          this.m_nRepeatPositionX = CssProperties.V_LEFT;
          this.m_nRepeatPositionY = CssProperties.V_TOP;
          htmlTreeItem.GetBackgroundPositionProperties(CssProperties.BACKGROUND_POSITION, ref this.m_nRepeatPositionX, ref this.m_nRepeatPositionY, false);
          if (this.m_pBkImage != null)
            this.m_pBkImage = (HtmlImage) null;
          this.m_pBkImage = new HtmlImage(this, 0.0f);
          HtmlTag pTag = new HtmlTag(this);
          pTag.m_arrNames.Add("SRC");
          pTag.m_arrValues.Add(pValue);
          this.m_pBkImage.Load(pTag, true);
          if (this.m_pBkImage.m_bNotFound)
            this.m_pBkImage = (HtmlImage) null;
          else if (this.m_nRepeatStyle == CssProperties.BACKGROUND_REPEAT)
            this.m_rgbBgColor.Reset();
        }
        this.m_bBackgroundObtained = true;
      }
      if (this.m_rgbBgColor.m_bIsSet)
      {
        pCanvas.SaveState();
        pCanvas.SetFillColor(this.m_rgbBgColor.r, this.m_rgbBgColor.g, this.m_rgbBgColor.b);
        if (this.m_bLandscape)
          pCanvas.FillRect(this.m_fBottomMargin, this.m_fLeftMargin, this.m_fPageHeight - this.m_fTopMargin - this.m_fBottomMargin, this.m_fPageWidth - this.m_fLeftMargin - this.m_fRightMargin);
        else
          pCanvas.FillRect(this.m_fLeftMargin, this.m_fBottomMargin, this.m_fPageWidth - this.m_fLeftMargin - this.m_fRightMargin, this.m_fPageHeight - this.m_fTopMargin - this.m_fBottomMargin);
        pCanvas.RestoreState();
      }
      if (nCount > 1 && (this.m_nRepeatStyle == CssProperties.V_NO_REPEAT || this.m_nRepeatStyle == CssProperties.V_REPEAT_X) || this.m_pBkImage == null)
        return;
      new HtmlTable(this, this.m_pMasterTable.m_arrCells[0], new HtmlTag(this)).TileImage(this.m_fShiftX, this.m_fFrameY, this.m_bLandscape ? this.m_fPageHeight - this.m_fTopMargin - this.m_fBottomMargin : this.m_fPageWidth - this.m_fLeftMargin - this.m_fRightMargin, this.m_bLandscape ? this.m_fPageWidth - this.m_fLeftMargin - this.m_fRightMargin : this.m_fPageHeight - this.m_fTopMargin - this.m_fBottomMargin, this.m_pBkImage, pPage, pCanvas, this.m_nRepeatStyle, this.m_nRepeatPositionX, this.m_nRepeatPositionY);
    }

    public bool ObtainFont(string Font, uint dwFlags, ref PdfFont ppPtr)
    {
      if (Font == null || Font.Length == 0)
        return false;
      foreach (FontCacheStruct fontCacheStruct in this.m_arrFontCache)
      {
        if (string.Compare(fontCacheStruct.m_bstrFont, Font, true) == 0 && (int) fontCacheStruct.m_dwFlags == (int) dwFlags)
        {
          if (fontCacheStruct.m_piFontPtr == null)
            return false;
          ppPtr = fontCacheStruct.m_piFontPtr;
          return true;
        }
      }
      FontCacheStruct fontCacheStruct1 = new FontCacheStruct();
      fontCacheStruct1.m_bstrFont = Font;
      fontCacheStruct1.m_dwFlags = dwFlags;
      this.m_arrFontCache.Add(fontCacheStruct1);
      PdfFonts fonts = this.m_pDocument.Fonts;
      string str = Font;
      char[] chArray = new char[1]
      {
        ','
      };
      string text = "";
      foreach (string s in str.Split(chArray))
      {
          text = s;
          HtmlObject.Trim(ref text);
        try
        {
          fontCacheStruct1.m_piFontPtr = fonts.LoadTrueTypeFont(s, ((int) dwFlags & 1) != 0, ((int) dwFlags & 2) != 0);
          break;
        }
        catch (Exception ex1)
        {
            if (string.Compare(text, "Helvetica", true) == 0)
          {
            try
            {
              fontCacheStruct1.m_piFontPtr = fonts.LoadTrueTypeFont("Arial", ((int) dwFlags & 1) != 0, ((int) dwFlags & 2) != 0);
              break;
            }
            catch (Exception ex2)
            {
            }
          }
        }
        try
        {
            fontCacheStruct1.m_piFontPtr = fonts[text];
        }
        catch (Exception ex)
        {
        }
      }
      if (fontCacheStruct1.m_piFontPtr == null)
      {
        this.LogError("Font", Font, "Font name cannot be found.");
        try
        {
          fontCacheStruct1.m_piFontPtr = fonts.LoadTrueTypeFont("Times New Roman", ((int) dwFlags & 1) != 0, ((int) dwFlags & 2) != 0);
        }
        catch (Exception ex)
        {
          return false;
        }
      }
      ppPtr = fontCacheStruct1.m_piFontPtr;
      return true;
    }

    public void LogError(string szAction, string Url, string Error)
    {
      if (!this.m_bDebug)
        return;
      HtmlObject.Trim(ref Error);
      this.m_bstrLog.Append(szAction + ": " + Error + "\r\nData: " + Url + "\r\n\r\n");
    }

    public void HandleBookmarks()
    {
      foreach (PdfAnnot pdfAnnot in this.m_arrBookmarkAnnots)
      {
        string strA = pdfAnnot.m_bstrName;
        foreach (PdfDest Dest in this.m_arrBookmarkDests)
        {
          string strB = Dest.m_bstrName;
          if (strA.Length > 0 && strB.Length > 0 && string.Compare(strA, strB, true) == 0)
            pdfAnnot.SetDest(Dest);
        }
      }
    }
  }
}
