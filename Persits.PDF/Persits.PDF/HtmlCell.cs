using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	internal class HtmlCell : HtmlBox
	{
		public int m_nRowNum;
		public int m_nColNum;
		public HtmlTable m_pTable;
		public HtmlAttributes m_nVAlign;
		public int m_nColSpan;
		public int m_nRowSpan;
		public float m_fSpecifiedWidth;
		public float m_fSpecifiedHeight;
		public bool m_bFlexibleWidth;
		public float m_fPercentWidth;
		public HtmlTag m_pTag;
		public AuxRGB m_rgbBgColor = new AuxRGB();
		public HtmlImage m_pBkImage;
		public CssProperties m_nRepeatStyle;
		public CssProperties m_nRepeatPositionX;
		public CssProperties m_nRepeatPositionY;
		public float[] m_arrPadding = new float[4];
		public bool m_bNoWrap;
		private bool m_bAbsoluteObjectsAdjusted;
		private bool m_bFirstOrSelectedOption;
		public string m_bstrOptionValue;
		private int m_nOptionCount;
		public bool m_bSelectMultiple;
		public float m_fCurX;
		public float m_fCurY;
		public float m_fMaxItemWidth;
		public float m_fMaxLineLength;
		public float m_fMaxHorizExtent;
		public TagType m_nType;
		public List<HtmlObject> m_arrObjects = new List<HtmlObject>();
		private List<FloatRect> m_arrLeftSide = new List<FloatRect>();
		private List<FloatRect> m_arrRightSide = new List<FloatRect>();
		public override ObjectType Type
		{
			get
			{
				return ObjectType.htmlCell;
			}
		}
		public HtmlCell(HtmlTable pTable) : base(pTable.m_pHtmlManager)
		{
			this.m_pTable = pTable;
			this.m_nColSpan = (this.m_nRowSpan = 1);
			this.m_fWidth = (this.m_fHeight = 0f);
			this.m_fSpecifiedWidth = (this.m_fSpecifiedHeight = 0f);
			this.m_nVAlign = HtmlAttributes.htmlMiddle;
			this.m_pTag = null;
			this.m_bFlexibleWidth = false;
			this.m_fPercentWidth = 100f;
			this.m_pBkImage = null;
			this.m_bNoWrap = false;
			this.m_fMaxHorizExtent = 0f;
			this.m_arrPadding[0] = (this.m_arrPadding[1] = (this.m_arrPadding[2] = (this.m_arrPadding[3] = 0f)));
			this.m_fCurX = (this.m_fCurY = 0f);
			this.m_bFirstOrSelectedOption = false;
			this.m_nOptionCount = 0;
			this.m_bSelectMultiple = false;
			this.m_bAbsolutePosition = (this.m_bAbsoluteLeft = (this.m_bAbsoluteTop = false));
			this.m_bAbsoluteObjectsAdjusted = false;
		}
		~HtmlCell()
		{
			this.m_arrObjects.Clear();
			this.ClearArrays();
		}
		public override void CalculateSize()
		{
		}
		public override void Parse()
		{
			string text = "";
			HtmlTag htmlTag = new HtmlTag(this.m_pHtmlManager);
			while (true)
			{
				if (base.m_c == '<' && base.ParseTag(ref htmlTag))
				{
					this.m_pHtmlManager.m_objCSS.AddToTree(htmlTag);
					if (this.m_nType == htmlTag.m_nType && htmlTag.m_bClosing && this.m_nType != TagType.TAG_UNKNOWN)
					{
						goto IL_461;
					}
					if (this.m_nType == TagType.TAG_LI && (htmlTag.m_nType == TagType.TAG_OL || htmlTag.m_nType == TagType.TAG_UL || htmlTag.m_nType == TagType.TAG_BQ) && htmlTag.m_bClosing)
					{
						break;
					}
					if ((htmlTag.m_nType == TagType.TAG_IMG || htmlTag.m_nType == TagType.TAG_INPUT) && !htmlTag.m_bClosing)
					{
						this.AddImage(htmlTag);
						continue;
					}
					if (htmlTag.m_nType == TagType.TAG_BR && !htmlTag.m_bClosing)
					{
						this.AddBreak(htmlTag);
						continue;
					}
					if (htmlTag.m_nType == TagType.TAG_A && !htmlTag.m_bClosing && (htmlTag.GetStringAttribute("Name", ref text) || htmlTag.GetStringAttribute("ID", ref text)))
					{
						this.AddBookmark(htmlTag);
						continue;
					}
					if (htmlTag.m_nType == TagType.TAG_TEXTAREA && !htmlTag.m_bClosing)
					{
						this.AddTextarea(htmlTag);
						continue;
					}
					if (htmlTag.m_nType == TagType.TAG_OPTION && !htmlTag.m_bClosing)
					{
						this.HandleSelectOption(htmlTag);
						continue;
					}
					if (htmlTag.m_nType == TagType.TAG_HR && !htmlTag.m_bClosing)
					{
						this.AddHR(htmlTag);
						continue;
					}
					if (htmlTag.m_nType == TagType.TAG_BASE && !htmlTag.m_bClosing)
					{
						this.HandleBase(htmlTag);
						continue;
					}
					if (htmlTag.IsOneOf(20223u) && htmlTag.m_bClosing && this.m_nType == TagType.TAG_P)
					{
						goto Block_26;
					}
					if (this.m_nType == TagType.TAG_P && (htmlTag.m_nType == TagType.TAG_TR || htmlTag.m_nType == TagType.TAG_TD || htmlTag.m_nType == TagType.TAG_TH) && !htmlTag.m_bClosing)
					{
						goto Block_30;
					}
					if (htmlTag.IsOneOf(17407u) && !htmlTag.m_bClosing)
					{
						if (this.m_nType == TagType.TAG_LI && (htmlTag.m_nType == TagType.TAG_LI || htmlTag.m_nType == TagType.TAG_BQ))
						{
							goto IL_260;
						}
						if (this.m_nType == TagType.TAG_P && htmlTag.IsOneOf(991u))
						{
							goto Block_36;
						}
						this.AddTable(htmlTag);
						continue;
					}
					else
					{
						if (this.m_nType == TagType.TAG_TD && !htmlTag.m_bClosing && (htmlTag.m_nType == TagType.TAG_TD || htmlTag.m_nType == TagType.TAG_TR))
						{
							goto IL_2E0;
						}
						if (this.m_nType == TagType.TAG_TH && !htmlTag.m_bClosing && (htmlTag.m_nType == TagType.TAG_TH || htmlTag.m_nType == TagType.TAG_TR))
						{
							goto IL_321;
						}
						if ((this.m_nType == TagType.TAG_TD || this.m_nType == TagType.TAG_TH) && htmlTag.m_bClosing && htmlTag.m_nType == TagType.TAG_TABLE)
						{
							goto Block_45;
						}
						if ((htmlTag.m_nType == TagType.TAG_TD || htmlTag.m_nType == TagType.TAG_TH) && htmlTag.m_bClosing && (this.m_nType == TagType.TAG_TD || this.m_nType == TagType.TAG_TH))
						{
							goto IL_39D;
						}
						if (htmlTag.m_nType == TagType.TAG_TR && htmlTag.m_bClosing && (this.m_nType == TagType.TAG_TD || this.m_nType == TagType.TAG_TH))
						{
							goto IL_461;
						}
						if ((htmlTag.m_nType == TagType.TAG_STYLE || htmlTag.m_nType == TagType.TAG_LINK) && !htmlTag.m_bClosing)
						{
							this.m_pHtmlManager.m_objCSS.LoadStyle(htmlTag);
							continue;
						}
						if ((htmlTag.m_nType == TagType.TAG_SCRIPT || htmlTag.m_nType == TagType.TAG_TITLE || htmlTag.m_nType == TagType.TAG_SELECT) && !htmlTag.m_bClosing)
						{
							this.AddSkippableContainer(htmlTag);
							continue;
						}
					}
				}
				if (HtmlObject.IsSpace(base.m_c))
				{
					this.AddSpace();
					base.Next();
				}
				else
				{
					this.AddWordAndParse();
				}
			}
			this.m_pHtmlManager.m_objCSS.ReverseLastClosure();
			base.RestoreState();
			goto IL_461;
			Block_26:
			this.m_pHtmlManager.m_objCSS.ReverseLastClosure();
			base.RestoreState();
			goto IL_461;
			Block_30:
			this.m_pHtmlManager.m_objCSS.ReverseLastClosure();
			base.RestoreState();
			goto IL_461;
			IL_260:
			this.m_pHtmlManager.m_objCSS.RemoveLastLeaf();
			base.RestoreState();
			goto IL_461;
			Block_36:
			this.m_pHtmlManager.m_objCSS.RemoveLastLeaf();
			base.RestoreState();
			goto IL_461;
			IL_2E0:
			this.m_pHtmlManager.m_objCSS.RemoveLastLeaf();
			base.RestoreState();
			goto IL_461;
			IL_321:
			this.m_pHtmlManager.m_objCSS.RemoveLastLeaf();
			base.RestoreState();
			goto IL_461;
			Block_45:
			base.RestoreState();
			goto IL_461;
			IL_39D:
			base.RestoreState();
			IL_461:
			this.RemoveTrailingSpaces(true);
		}
		private void AddWordAndParse()
		{
			HtmlWord htmlWord = this.AddWord();
			htmlWord.Parse();
			if (htmlWord.m_arrChunks.Count == 0)
			{
				HtmlObject htmlObject = this.m_arrObjects[this.m_arrObjects.Count - 1];
				if (htmlObject == htmlWord)
				{
					this.m_arrObjects.RemoveAt(this.m_arrObjects.Count - 1);
				}
			}
		}
		private HtmlWord AddWord()
		{
			HtmlWord htmlWord = new HtmlWord(this.m_pHtmlManager);
			this.m_arrObjects.Add(htmlWord);
			return htmlWord;
		}
		public HtmlWord AddWord(string pStr, int nLen)
		{
			HtmlWord htmlWord = new HtmlWord(this.m_pHtmlManager);
			HtmlChunk htmlChunk = htmlWord.AddChunk();
			htmlChunk.m_bstrText.Append(pStr);
			htmlChunk.CalculateSize();
			htmlWord.CalculateSize();
			htmlChunk.m_dwFlags &= 4294966259u;
			this.m_arrObjects.Insert(0, htmlWord);
			return htmlWord;
		}
		private HtmlTable AddTable(HtmlTag pTag)
		{
			HtmlTable htmlTable = new HtmlTable(this.m_pHtmlManager, this, pTag);
			htmlTable.m_nParentContainerType = this.m_nType;
			if (htmlTable.m_bFlexibleTopMargin)
			{
				if (this.m_arrObjects.Count == 0)
				{
					htmlTable.m_arrMargins[0] = 0f;
				}
				else
				{
					int num = this.m_arrObjects.Count - 1;
					while (num >= 0 && this.m_arrObjects[num].Type == ObjectType.htmlBreak)
					{
						num--;
					}
					if (num >= 0 && this.m_arrObjects[num].Type == ObjectType.htmlTable && ((HtmlTable)this.m_arrObjects[num]).m_bFlexibleBottomMargin)
					{
						htmlTable.m_arrMargins[0] = 0f;
					}
				}
			}
			if (this.m_arrObjects.Count > 0)
			{
				this.AddBreak(null, true);
				this.m_arrObjects[this.m_arrObjects.Count - 1].Height = 0f;
			}
			this.m_arrObjects.Add(htmlTable);
			if (htmlTable.HorizAlignment == HtmlAttributes.htmlNone)
			{
				this.AddBreak(null, true);
			}
			try
			{
				htmlTable.Parse();
			}
			catch (PdfException)
			{
				this.m_pHtmlManager.LogError(HtmlTag.LookupTag((htmlTable.m_nRepresentsTag == TagType.TAG_NOTATAG) ? TagType.TAG_TABLE : htmlTable.m_nRepresentsTag), "Not applicable.", "Matching closing tag not found.");
			}
			htmlTable.AssignCoordinates();
			return htmlTable;
		}
		private void AddSpace()
		{
			if ((this.m_arrObjects.Count > 0 && (this.m_arrObjects[this.m_arrObjects.Count - 1].m_bIsSpace || this.m_arrObjects[this.m_arrObjects.Count - 1].Type == ObjectType.htmlBreak)) || this.m_arrObjects.Count == 0)
			{
				return;
			}
			HtmlWord htmlWord = this.AddWord();
			HtmlChunk htmlChunk = htmlWord.AddChunk();
			htmlChunk.m_bstrText.Append(" ");
			htmlWord.m_bIsSpace = true;
			htmlWord.m_dwAlignFlags = htmlChunk.m_dwFlags;
			htmlChunk.CalculateSize();
			htmlWord.CalculateSize();
		}
		private void RemoveTrailingSpaces(bool bHandleButtonMargins)
		{
			if (this.m_arrObjects.Count > 0 && this.m_arrObjects[this.m_arrObjects.Count - 1].m_bIsSpace)
			{
				this.m_arrObjects.RemoveAt(this.m_arrObjects.Count - 1);
			}
			if (bHandleButtonMargins && this.m_arrObjects.Count > 0)
			{
				int num = this.m_arrObjects.Count - 1;
				while (num >= 0 && this.m_arrObjects[num].Type == ObjectType.htmlBreak)
				{
					num--;
				}
				if (num >= 0 && this.m_arrObjects[num].Type == ObjectType.htmlTable && ((HtmlTable)this.m_arrObjects[num]).m_bFlexibleBottomMargin)
				{
					((HtmlTable)this.m_arrObjects[num]).m_arrMargins[2] = 0f;
					((HtmlTable)this.m_arrObjects[num]).CalculateSize();
				}
			}
			if (this.m_bDisplayNone && bHandleButtonMargins)
			{
				foreach (HtmlCell current in this.m_pTable.m_arrCells)
				{
					if (current == this)
					{
						this.m_pTable.m_arrCells.Remove(current);
						break;
					}
				}
			}
		}
		private void AddBookmark(HtmlTag pTag)
		{
			string text = "";
			HtmlWord htmlWord = new HtmlWord(this.m_pHtmlManager);
			HtmlChunk htmlChunk = htmlWord.AddChunk();
			htmlChunk.m_bstrText.Append(" ");
			htmlChunk.CalculateSize();
			htmlWord.CalculateSize();
			htmlWord.Width = 0f;
			htmlChunk.Width = 0f;
			htmlChunk.m_rgbBackgroundColor.m_bIsSet = false;
			if (!pTag.GetStringAttribute("Name", ref htmlChunk.m_bstrHrefName))
			{
				pTag.GetStringAttribute("ID", ref htmlChunk.m_bstrHrefName);
			}
			this.m_arrObjects.Add(htmlWord);
			if (!pTag.GetStringAttribute("Href", ref text))
			{
				this.m_pHtmlManager.m_objCSS.AddThisTagToTree(pTag.m_nType);
			}
		}
		private void AddBreak(HtmlTag pTag)
		{
			this.AddBreak(pTag, false);
		}
		private void AddBreak(HtmlTag pTag, bool bDoNotAddIfFirst)
		{
			this.RemoveTrailingSpaces(false);
			if (bDoNotAddIfFirst && this.m_arrObjects.Count == 0)
			{
				return;
			}
			HtmlBreak htmlBreak = new HtmlBreak(this.m_pHtmlManager);
			HtmlChunk htmlChunk = htmlBreak.AddChunk();
			htmlBreak.m_dwAlignFlags = htmlChunk.m_dwFlags;
			htmlChunk.m_bstrText.Append(" ");
			htmlChunk.CalculateSize();
			htmlBreak.CalculateSize();
			htmlBreak.Width = 0f;
			htmlChunk.Width = 0f;
			htmlChunk.m_rgbBackgroundColor.m_bIsSet = false;
			if (this.m_arrObjects.Count > 0 && this.m_arrObjects[this.m_arrObjects.Count - 1].Type != ObjectType.htmlBreak)
			{
				htmlBreak.Height = 0f;
			}
			htmlBreak.m_fMaxDescent = 0f;
			if (pTag != null)
			{
				pTag.GetStandardAttribute("clear", ref htmlBreak.m_nClear);
				htmlBreak.ObtainPageBreakStatus();
			}
			CssProperties cssProperties = CssProperties.NOTFOUND;
			if (htmlBreak.m_pTreeItem.GetEnumProperty(CssProperties.CLEAR, ref cssProperties, false))
			{
				if (cssProperties == CssProperties.V_LEFT)
				{
					htmlBreak.m_nClear = HtmlAttributes.htmlLeft;
				}
				else
				{
					if (cssProperties == CssProperties.V_RIGHT)
					{
						htmlBreak.m_nClear = HtmlAttributes.htmlRight;
					}
					else
					{
						if (cssProperties == CssProperties.V_BOTH)
						{
							htmlBreak.m_nClear = HtmlAttributes.htmlBoth;
						}
					}
				}
			}
			this.m_arrObjects.Add(htmlBreak);
			if (pTag != null && pTag.m_nType == TagType.TAG_BR)
			{
				this.m_pHtmlManager.m_objCSS.AddThisTagToTree(pTag.m_nType);
			}
		}
		private void HandleBase(HtmlTag pTag)
		{
			string text = null;
			if (pTag.GetStringAttribute("href", ref text) && text != null)
			{
				HtmlXmlUrl htmlXmlUrl = new HtmlXmlUrl(this.m_pHtmlManager);
				htmlXmlUrl.MakeUrl(this.m_pHtmlManager.m_pBaseUrl, text);
				htmlXmlUrl.SplitUrl();
				this.m_pHtmlManager.m_pBaseUrl.m_bstrHost = htmlXmlUrl.m_bstrHost;
				this.m_pHtmlManager.m_pBaseUrl.m_bstrPath = htmlXmlUrl.m_bstrPath;
				this.m_pHtmlManager.m_pBaseUrl.m_bstrUrl = htmlXmlUrl.m_bstrUrl;
				this.m_pHtmlManager.m_pBaseUrl.m_bFileUrl = htmlXmlUrl.m_bFileUrl;
			}
			this.m_pHtmlManager.m_objCSS.AddThisTagToTree(pTag.m_nType);
		}
		public HtmlImage AddImage(HtmlTag pTag)
		{
			HtmlImage htmlImage = new HtmlImage(this.m_pHtmlManager, this.m_fWidth);
			htmlImage.m_nParentContainerType = this.m_nType;
			this.m_arrObjects.Add(htmlImage);
			htmlImage.Load(pTag);
			this.m_pHtmlManager.m_objCSS.AddThisTagToTree(pTag.m_nType);
			return htmlImage;
		}
		public HtmlImage AddImageBullet(string bstrPath)
		{
			HtmlTag htmlTag = new HtmlTag(this.m_pHtmlManager);
			htmlTag.m_arrNames.Add("SRC");
			htmlTag.m_arrValues.Add(bstrPath);
			HtmlImage htmlImage = new HtmlImage(this.m_pHtmlManager, 0f);
			htmlImage.m_nParentContainerType = this.m_nType;
			htmlImage.Load(htmlTag, true);
			if (htmlImage.m_bNotFound)
			{
				return null;
			}
			this.m_arrObjects.Insert(0, htmlImage);
			return htmlImage;
		}
		private void AddHR(HtmlTag pTag)
		{
			HtmlHR item = new HtmlHR(this.m_pHtmlManager, this, pTag);
			this.AddBreak(null, true);
			this.m_arrObjects.Add(item);
			this.AddBreak(null, true);
			this.m_pHtmlManager.m_objCSS.AddThisTagToTree(pTag.m_nType);
		}
		public void HandleSelectOption(HtmlTag pTag)
		{
			if (this.m_nType != TagType.TAG_SELECT)
			{
				return;
			}
			this.m_nOptionCount++;
			if (this.m_bFirstOrSelectedOption || this.m_bSelectMultiple)
			{
				if (!this.m_bSelectMultiple)
				{
					this.m_bstrOptionValue = "";
				}
				foreach (HtmlObject current in this.m_arrObjects)
				{
					if (current.Type == ObjectType.htmlWord)
					{
						HtmlWord htmlWord = (HtmlWord)current;
						foreach (HtmlChunk current2 in htmlWord.m_arrChunks)
						{
							this.m_bstrOptionValue += current2.m_bstrText.ToString();
						}
					}
				}
				if (this.m_bSelectMultiple && this.m_bstrOptionValue.Length > 0)
				{
					this.m_bstrOptionValue += "\r\n";
				}
			}
			this.m_arrObjects.Clear();
			if (pTag == null)
			{
				return;
			}
			this.m_bFirstOrSelectedOption = (this.m_nOptionCount == 1 || pTag.GetIntAttribute("Selected"));
		}
		private float ArrangeNonStandardImages(float fMaxHeight, int itFirst, int itLast)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = itFirst; i <= itLast; i++)
			{
				HtmlObject htmlObject = this.m_arrObjects[i];
				if (htmlObject.Type == ObjectType.htmlWord || htmlObject.Type == ObjectType.htmlBreak || htmlObject.Type == ObjectType.htmlHR)
				{
					if (htmlObject.Height > num)
					{
						num = htmlObject.Height;
					}
					if (!htmlObject.m_bIsSpace && htmlObject.Type != ObjectType.htmlHR && ((HtmlWord)htmlObject).m_fMaxDescent < num2)
					{
						num2 = ((HtmlWord)htmlObject).m_fMaxDescent;
					}
				}
				if (htmlObject.Type == ObjectType.htmlImage && ((HtmlImage)htmlObject).m_nAlignment == HtmlAttributes.htmlBottom && ((HtmlImage)htmlObject).Height > num)
				{
					num = ((HtmlImage)htmlObject).Height;
				}
				if (htmlObject.Type == ObjectType.htmlTable && ((HtmlTable)htmlObject).HorizAlignment == HtmlAttributes.htmlNone && ((HtmlTable)htmlObject).Height > num)
				{
					num = ((HtmlTable)htmlObject).Height;
				}
			}
			float num3 = num;
			float num4 = -num2;
			for (int i = itFirst; i <= itLast; i++)
			{
				HtmlObject htmlObject2 = this.m_arrObjects[i];
				if (htmlObject2.Type == ObjectType.htmlImage)
				{
					HtmlImage htmlImage = (HtmlImage)htmlObject2;
					if (htmlImage.m_nAlignment == HtmlAttributes.htmlBottom && htmlImage.Height > num3)
					{
						num3 = htmlImage.Height;
					}
					if (htmlImage.m_nAlignment == HtmlAttributes.htmlMiddle && htmlImage.Height / 2f > num3)
					{
						num3 = htmlImage.Height / 2f;
					}
					if ((htmlImage.m_nAlignment == HtmlAttributes.htmlTop || htmlImage.m_nAlignment == HtmlAttributes.htmlLeft || htmlImage.m_nAlignment == HtmlAttributes.htmlRight) && htmlImage.Height - num > num4)
					{
						num4 = htmlImage.Height - num;
					}
					if (htmlImage.m_nAlignment == HtmlAttributes.htmlMiddle && htmlImage.Height / 2f > num4)
					{
						num4 = htmlImage.Height / 2f;
					}
				}
				if (htmlObject2.Type == ObjectType.htmlTable)
				{
					HtmlTable htmlTable = (HtmlTable)htmlObject2;
					if ((htmlTable.HorizAlignment == HtmlAttributes.htmlLeft || htmlTable.HorizAlignment == HtmlAttributes.htmlRight) && htmlTable.Height - num > num4)
					{
						num4 = htmlTable.Height - num;
					}
					else
					{
						if (htmlTable.HorizAlignment == HtmlAttributes.htmlNone && htmlTable.Height > num3)
						{
							num3 = htmlTable.Height;
						}
					}
				}
			}
			num3 = fMaxHeight - num3;
			for (int i = itFirst; i <= itLast; i++)
			{
				HtmlObject htmlObject3 = this.m_arrObjects[i];
				htmlObject3.chgY(-num3);
				if (htmlObject3.Type == ObjectType.htmlImage)
				{
					HtmlImage htmlImage2 = (HtmlImage)htmlObject3;
					if (htmlImage2.m_nAlignment == HtmlAttributes.htmlMiddle)
					{
						htmlObject3.chgY(htmlObject3.Height / 2f);
					}
					if (htmlImage2.m_nAlignment == HtmlAttributes.htmlTop || htmlImage2.m_nAlignment == HtmlAttributes.htmlLeft || htmlImage2.m_nAlignment == HtmlAttributes.htmlRight)
					{
						htmlObject3.chgY(htmlObject3.Height - num);
					}
				}
				if (htmlObject3.Type == ObjectType.htmlTable && (htmlObject3.HorizAlignment == HtmlAttributes.htmlLeft || htmlObject3.HorizAlignment == HtmlAttributes.htmlRight))
				{
					htmlObject3.chgY(htmlObject3.Height - num);
				}
				htmlObject3.AssignCoordinates();
			}
			for (int i = itFirst; i <= itLast; i++)
			{
				HtmlObject htmlObject4 = this.m_arrObjects[i];
				if (htmlObject4.HorizAlignment == HtmlAttributes.htmlLeft || htmlObject4.HorizAlignment == HtmlAttributes.htmlRight)
				{
					HtmlObject htmlObject5 = htmlObject4;
					this.AddPoint(htmlObject4.HorizAlignment == HtmlAttributes.htmlLeft, htmlObject5.X, htmlObject5.Y, htmlObject5.Width, htmlObject5.Height);
				}
			}
			return num4 - num3;
		}
		private void AddTextarea(HtmlTag pTag)
		{
			int num = this.m_pHtmlManager.m_nPtr - 1;
			int num2 = PdfStream.FindString(this.m_pHtmlManager.m_pBuffer, num, "</TEXTAREA>".ToCharArray());
			if (num2 == -1)
			{
				this.m_pHtmlManager.m_objCSS.AddThisTagToTree(pTag.m_nType);
				return;
			}
			this.m_pHtmlManager.m_nPtr = num2 + 11;
			base.Next();
			HtmlImage htmlImage = this.AddImage(pTag);
			htmlImage.m_bstrValue = new string(this.m_pHtmlManager.m_pBuffer, num, num2 - num);
		}
		private void HandleNonStandardTextAlignment(int itFirst, int itLast, float fCumulativeWidth, float fAvailableWidth, bool bLastLine)
		{
			uint dwAlignFlags = this.m_arrObjects[itFirst].m_dwAlignFlags;
			if ((dwAlignFlags & 896u) == 0u)
			{
				return;
			}
			float num = fAvailableWidth - fCumulativeWidth;
			if ((dwAlignFlags & 256u) != 0u)
			{
				num /= 2f;
			}
			if ((dwAlignFlags & 512u) != 0u)
			{
				num = 0f;
			}
			int num2 = 0;
			for (int i = itFirst; i <= itLast; i++)
			{
				HtmlObject htmlObject = this.m_arrObjects[i];
				if (htmlObject.HorizAlignment != HtmlAttributes.htmlLeft && htmlObject.HorizAlignment != HtmlAttributes.htmlRight)
				{
					htmlObject.chgX(num);
					if (htmlObject.m_bIsSpace && htmlObject.Width > 0f)
					{
						num2++;
					}
				}
			}
			if (bLastLine || (dwAlignFlags & 384u) != 0u)
			{
				return;
			}
			num = (fAvailableWidth - fCumulativeWidth) / (float)num2;
			float num3 = 0f;
			for (int i = itFirst; i <= itLast; i++)
			{
				HtmlObject htmlObject2 = this.m_arrObjects[i];
				if (htmlObject2.HorizAlignment != HtmlAttributes.htmlLeft && htmlObject2.HorizAlignment != HtmlAttributes.htmlRight)
				{
					htmlObject2.chgX(num3);
					if (htmlObject2.m_bIsSpace && htmlObject2.Width > 0f)
					{
						htmlObject2.m_fOldWidth = htmlObject2.Width;
						htmlObject2.chgWidth(num);
						num3 += num;
					}
					if (htmlObject2.X + htmlObject2.Width > this.m_fMaxHorizExtent)
					{
						this.m_fMaxHorizExtent = htmlObject2.X + htmlObject2.Width;
					}
				}
			}
		}
		private float ArrangeLine(int itFirst, int itLast, bool bLastLine, float fTotalAvailableWidth)
		{
			float num = 0f;
			float num2 = this.m_fCurX;
			float num3 = 0f;
			bool flag = false;
			bool flag2 = false;
			float num4 = 0f;
			float num5 = 0f;
			for (int i = itFirst; i <= itLast; i++)
			{
				HtmlObject htmlObject = this.m_arrObjects[i];
				if (htmlObject.HorizAlignment == HtmlAttributes.htmlLeft || htmlObject.HorizAlignment == HtmlAttributes.htmlRight)
				{
					if (htmlObject.HorizAlignment == HtmlAttributes.htmlLeft)
					{
						flag = true;
						num4 += htmlObject.Width;
					}
					if (htmlObject.HorizAlignment == HtmlAttributes.htmlRight)
					{
						flag2 = true;
						num5 += htmlObject.Width;
						this.m_fMaxHorizExtent = fTotalAvailableWidth;
					}
				}
			}
			for (int i = itFirst; i <= itLast; i++)
			{
				HtmlObject htmlObject2 = this.m_arrObjects[i];
				if (htmlObject2.HorizAlignment != HtmlAttributes.htmlLeft && htmlObject2.HorizAlignment != HtmlAttributes.htmlRight)
				{
					if (!htmlObject2.m_bIsSpace)
					{
						break;
					}
					((HtmlWord)htmlObject2).MakeInvisible();
				}
			}
			for (int i = itLast; i >= itFirst; i--)
			{
				HtmlObject htmlObject3 = this.m_arrObjects[i];
				if (htmlObject3.HorizAlignment != HtmlAttributes.htmlLeft && htmlObject3.HorizAlignment != HtmlAttributes.htmlRight)
				{
					if (!htmlObject3.m_bIsSpace)
					{
						break;
					}
					((HtmlWord)htmlObject3).MakeInvisible();
				}
			}
			for (int i = itFirst; i <= itLast; i++)
			{
				HtmlObject htmlObject4 = this.m_arrObjects[i];
				htmlObject4.ExpandIfNecessary(fTotalAvailableWidth - num5 - num4 - 0.001f);
				if (htmlObject4.Height > num)
				{
					num = htmlObject4.Height;
				}
				if (htmlObject4.Type == ObjectType.htmlWord && ((HtmlWord)htmlObject4).m_fMaxDescent < num3)
				{
					num3 = ((HtmlWord)htmlObject4).m_fMaxDescent;
				}
				htmlObject4.X = num2 + num4;
				num2 += htmlObject4.Width;
				if (num2 > this.m_fMaxHorizExtent)
				{
					this.m_fMaxHorizExtent = num2;
				}
				if (htmlObject4.HorizAlignment == HtmlAttributes.htmlLeft || htmlObject4.HorizAlignment == HtmlAttributes.htmlRight)
				{
					num2 -= htmlObject4.Width;
				}
			}
			this.HandleNonStandardTextAlignment(itFirst, itLast, num2 - this.m_fCurX, fTotalAvailableWidth - num5 - num4, bLastLine);
			if (flag)
			{
				float num6 = this.m_fCurX;
				for (int i = itFirst; i <= itLast; i++)
				{
					HtmlObject htmlObject5 = this.m_arrObjects[i];
					if (htmlObject5.HorizAlignment == HtmlAttributes.htmlLeft)
					{
						htmlObject5.X = num6;
						num6 += htmlObject5.Width;
					}
				}
			}
			if (flag2)
			{
				float num6 = this.m_fCurX + fTotalAvailableWidth - num5 - num4;
				for (int i = itLast; i >= itFirst; i--)
				{
					HtmlObject htmlObject6 = this.m_arrObjects[i];
					if (htmlObject6.HorizAlignment == HtmlAttributes.htmlRight)
					{
						htmlObject6.X = num6 + num4;
						num6 += htmlObject6.Width;
					}
				}
			}
			for (int i = itFirst; i <= itLast; i++)
			{
				HtmlObject htmlObject7 = this.m_arrObjects[i];
				htmlObject7.Y = this.m_fCurY + num - htmlObject7.Height;
				htmlObject7.AssignCoordinates();
			}
			float num7 = this.ArrangeNonStandardImages(num, itFirst, itLast);
			if (flag || flag2)
			{
				float num8 = 0f;
				for (int i = itFirst; i <= itLast; i++)
				{
					HtmlObject htmlObject8 = this.m_arrObjects[i];
					if (htmlObject8.HorizAlignment != HtmlAttributes.htmlLeft && htmlObject8.HorizAlignment != HtmlAttributes.htmlRight && htmlObject8.Height > num8)
					{
						num8 = htmlObject8.Height;
					}
				}
				num = num8 - num3;
			}
			if (this.m_arrObjects[itLast].Type == ObjectType.htmlBreak && ((HtmlBreak)this.m_arrObjects[itLast]).m_nClear != HtmlAttributes.htmlNone)
			{
				float clear = this.GetClear(((HtmlBreak)this.m_arrObjects[itLast]).m_nClear);
				if (clear > 0f)
				{
					num = clear;
				}
			}
			return num + num7;
		}
		public override void AssignCoordinates()
		{
			int i = 0;
			this.m_fMaxHorizExtent = 0f;
			while (i < this.m_arrObjects.Count)
			{
				bool flag = false;
				float num = this.GetLeftMargin();
				this.m_fCurX = num;
				float rightMargin = this.GetRightMargin();
				int num2 = i;
				int num3 = i - 1;
				if (num + this.m_arrObjects[i].Width > rightMargin)
				{
					throw new PdfException(PdfErrors._ERROR_HTML_TOONARROW, num + this.m_arrObjects[i].Width - rightMargin);
				}
				while (num + this.m_arrObjects[i].Width <= rightMargin)
				{
					num += this.m_arrObjects[i].Width;
					if (i == num2 && this.m_arrObjects[i].m_bIsSpace)
					{
						num -= this.m_arrObjects[i].Width;
					}
					num3 = i;
					i++;
					if (i >= this.m_arrObjects.Count || this.m_arrObjects[num3].Type == ObjectType.htmlBreak)
					{
						flag = true;
						break;
					}
				}
				if (this.m_bNoWrap && !flag && num + this.m_arrObjects[i].Width > rightMargin)
				{
					throw new PdfException(PdfErrors._ERROR_HTML_TOONARROW, num + this.m_arrObjects[i].Width - rightMargin);
				}
				float num4 = this.ArrangeLine(num2, num3, flag, rightMargin - this.m_fCurX);
				this.m_fCurY += num4;
			}
			for (i = 0; i < this.m_arrObjects.Count; i++)
			{
				if (this.m_arrObjects[i].Type == ObjectType.htmlTable)
				{
					((HtmlTable)this.m_arrObjects[i]).AssignBulletCoordinates();
				}
			}
		}
		private void AddSkippableContainer(HtmlTag pTag)
		{
			HtmlTag htmlTag = new HtmlTag(this.m_pHtmlManager);
			htmlTag.m_nType = pTag.m_nType;
			HtmlTable htmlTable = new HtmlTable(this.m_pHtmlManager, null, htmlTag);
			htmlTable.AddCell(htmlTag);
			if (pTag.m_nType == TagType.TAG_SELECT && pTag.GetStringAttribute("MULTIPLE"))
			{
				htmlTable.m_arrCells[0].m_bSelectMultiple = true;
			}
			try
			{
				htmlTable.m_arrCells[0].Parse();
			}
			catch (PdfException)
			{
			}
			if (htmlTable.m_arrCells.Count > 0)
			{
				this.m_pHtmlManager.HandleSkippableContainer(pTag, htmlTable.m_arrCells[0], this);
			}
		}
		public void ResetInvisibleObjects()
		{
			foreach (HtmlObject current in this.m_arrObjects)
			{
				if (current.Type == ObjectType.htmlWord)
				{
					HtmlWord htmlWord = (HtmlWord)current;
					if (htmlWord.m_bInvisible)
					{
						htmlWord.Width = htmlWord.m_fOldWidth;
						htmlWord.Height = htmlWord.m_fOldHeight;
						htmlWord.m_fMaxDescent = htmlWord.m_fOldDescent;
						htmlWord.m_bInvisible = false;
					}
				}
				if (current.m_bIsSpace && current.m_fOldWidth > 0f)
				{
					current.Width = current.m_fOldWidth;
				}
			}
		}
		public override bool Render(PdfPage pPage, PdfCanvas pCanvas, float fShiftX, float fShiftY)
		{
			foreach (HtmlObject current in this.m_arrObjects)
			{
				current.Render(pPage, pCanvas, fShiftX, fShiftY);
				current.HandlePageBreakAfter(fShiftY);
			}
			return true;
		}
		private bool AllAligned()
		{
			if (this.m_arrObjects.Count == 0)
			{
				return false;
			}
			foreach (HtmlObject current in this.m_arrObjects)
			{
				if (!current.m_bIsSpace && current.HorizAlignment == HtmlAttributes.htmlNone)
				{
					return false;
				}
			}
			return true;
		}
		public void Retrim(float fWidthChange)
		{
			foreach (HtmlObject current in this.m_arrObjects)
			{
				if (current.HorizAlignment != HtmlAttributes.htmlLeft && current.HorizAlignment != HtmlAttributes.htmlRight)
				{
					uint dwAlignFlags = current.m_dwAlignFlags;
					if ((dwAlignFlags & 896u) != 0u)
					{
						if ((dwAlignFlags & 128u) != 0u)
						{
							current.chgX(-fWidthChange);
						}
						if ((dwAlignFlags & 256u) != 0u)
						{
							current.chgX(-fWidthChange / 2f);
						}
						if (current.Type == ObjectType.htmlWord)
						{
							current.AssignCoordinates();
						}
					}
				}
			}
		}
		public void ComputeStats()
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			foreach (HtmlObject current in this.m_arrObjects)
			{
				float width = current.Width;
				num3 += width;
				if (width > num)
				{
					num = width;
				}
				if (current.Type == ObjectType.htmlBreak)
				{
					if (num3 > num2)
					{
						num2 = num3;
					}
					num3 = 0f;
				}
			}
			if (num3 > num2)
			{
				num2 = num3;
			}
			this.m_fMaxItemWidth = num;
			this.m_fMaxLineLength = num2;
		}
		public void AdjustAbsoluteObjects(float fAdjX, float fAdjY)
		{
			if (this.m_bAbsoluteObjectsAdjusted)
			{
				return;
			}
			this.m_bAbsoluteObjectsAdjusted = true;
			foreach (HtmlObject current in this.m_arrObjects)
			{
				if (current.m_bAbsolutePosition)
				{
					current.chgXY(fAdjX, fAdjY);
				}
			}
		}
		public void ClearArrays()
		{
			this.m_arrLeftSide.Clear();
			this.m_arrRightSide.Clear();
		}
		private float GetLeftMargin()
		{
			float num = 0f;
			foreach (FloatRect current in this.m_arrLeftSide)
			{
				if (this.m_fCurY >= current.m_y && this.m_fCurY <= current.m_y1 && current.m_x1 > num)
				{
					num = current.m_x1;
				}
			}
			return num;
		}
		private float GetRightMargin()
		{
			float num = this.m_fWidth - this.m_arrBorders[3] - this.m_arrBorders[1] - this.m_arrPadding[3] - this.m_arrPadding[1];
			foreach (FloatRect current in this.m_arrRightSide)
			{
				if (this.m_fCurY >= current.m_y && this.m_fCurY <= current.m_y1 && current.m_x < num)
				{
					num = current.m_x;
				}
			}
			return num;
		}
		private void AddPoint(bool bLeft, float x, float y, float width, float height)
		{
			if (x == 0f && y == 0f && width == 0f && height == 0f)
			{
				return;
			}
			List<FloatRect> list = bLeft ? this.m_arrLeftSide : this.m_arrRightSide;
			list.Add(new FloatRect
			{
				m_x = x,
				m_y = y - 0.001f,
				m_x1 = x + width,
				m_y1 = y + height
			});
		}
		public float GetClear(HtmlAttributes nSide)
		{
			float num = 0f;
			float num2 = 0f;
			if (nSide == HtmlAttributes.htmlLeft || nSide == HtmlAttributes.htmlBoth)
			{
				foreach (FloatRect current in this.m_arrLeftSide)
				{
					if (this.m_fCurY >= current.m_y && this.m_fCurY <= current.m_y1 && current.m_y1 > num)
					{
						num = current.m_y1 + 0.001f;
					}
				}
			}
			if (nSide == HtmlAttributes.htmlRight || nSide == HtmlAttributes.htmlBoth)
			{
				foreach (FloatRect current2 in this.m_arrRightSide)
				{
					if (this.m_fCurY >= current2.m_y && this.m_fCurY <= current2.m_y1 && current2.m_y1 > num2)
					{
						num2 = current2.m_y1 + 0.001f;
					}
				}
			}
			if (nSide == HtmlAttributes.htmlLeft)
			{
				return num - this.m_fCurY;
			}
			if (nSide == HtmlAttributes.htmlRight)
			{
				return num2 - this.m_fCurY;
			}
			return ((num > num2) ? num : num2) - this.m_fCurY;
		}
		public float GetMaxY()
		{
			float num = this.m_fCurY;
			foreach (FloatRect current in this.m_arrLeftSide)
			{
				if (current.m_y1 > num)
				{
					num = current.m_y1;
				}
			}
			foreach (FloatRect current2 in this.m_arrRightSide)
			{
				if (current2.m_y1 > num)
				{
					num = current2.m_y1;
				}
			}
			return num;
		}
	}
}
