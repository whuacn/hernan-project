using System;
using System.Collections.Generic;
using System.Text;
namespace Persits.PDF
{
	internal class HtmlTable : HtmlBox
	{
		public List<HtmlCell> m_arrCells = new List<HtmlCell>();
		public TagType m_nRepresentsTag;
		public TagType m_nParentContainerType;
		public int m_nRowCount;
		public int m_nColCount;
		public int m_nRows;
		public int m_nCols;
		public bool m_bCoordinatesAssigned;
		public float m_fCellBorder;
		public float m_fCellSpacing;
		public float m_fCellPadding;
		public bool m_bFlexibleTopMargin;
		public bool m_bFlexibleBottomMargin;
		public bool m_bFlexibleWidth;
		public float m_fPercentWidth;
		public bool m_bTrimmable;
		private CssProperties m_nBulletType;
		private CssProperties m_nBulletPosition;
		private int m_nBulletCounter;
		private int m_nBulletNumber;
		private string m_bstrBulletPath;
		private HtmlWord m_pBullet;
		private HtmlImage m_pBulletImage;
		private bool m_bBorderCollapse;
		private List<int> m_arrSwallowDiagram = new List<int>();
		private bool m_bGridLinesChanged;
		private HtmlGridLine[] m_pVertLines;
		private HtmlGridLine[] m_pHorizLines;
		private AuxSparce2DArray m_pHeightMatrix;
		private Aux2DArray m_pWidthMatrix;
		private HtmlTable m_pCaption;
		private float m_fCaptionHeight;
		private float m_fCaptionWidth;
		private HtmlImage m_pBkImage;
		private CssProperties m_nRepeatStyle;
		private CssProperties m_nRepeatPositionX;
		private CssProperties m_nRepeatPositionY;
		public float m_fSpecifiedWidth;
		public float m_fSpecifiedHeight;
		public float m_fMaxWidth;
		private AuxRGB m_rgbBgColor = new AuxRGB();
		private Aux2DArray m_arrCellIndex;
		private float m_fBulletX;
		public override ObjectType Type
		{
			get
			{
				return ObjectType.htmlTable;
			}
		}
		public override HtmlAttributes HorizAlignment
		{
			get
			{
				return this.m_nAlignment;
			}
		}
		public HtmlTable(HtmlManager pMgr, HtmlCell pParentCell, HtmlTag pTag) : base(pMgr)
		{
			CssProperties cssProperties = CssProperties.NOTFOUND;
			this.m_fWidth = (this.m_fHeight = 0f);
			this.m_bFlexibleWidth = false;
			this.m_fPercentWidth = 100f;
			this.m_bTrimmable = false;
			this.m_nRowCount = 0;
			this.m_nColCount = 0;
			this.m_fCellBorder = 0f;
			this.m_bBorderCollapse = false;
			this.m_bGridLinesChanged = false;
			this.m_bFlexibleTopMargin = (this.m_bFlexibleBottomMargin = false);
			int num = 1;
			this.m_fCellPadding = (float)num;
			this.m_pBullet = null;
			this.m_pBulletImage = null;
			this.m_nBulletCounter = 0;
			this.m_nBulletNumber = 0;
			this.m_nBulletType = CssProperties.V_0;
			this.m_pHtmlManager = pMgr;
			this.m_bCoordinatesAssigned = false;
			this.m_pCaption = null;
			this.m_fCaptionHeight = 0f;
			this.m_fCaptionWidth = 0f;
			this.m_pHorizLines = null;
			this.m_pVertLines = null;
			this.m_pHeightMatrix = null;
			this.m_pWidthMatrix = null;
			this.m_pBkImage = null;
			if (pParentCell == null)
			{
				return;
			}
			this.m_fCellSpacing = 2f;
			this.m_pTreeItem.GetNumericProperty(CssProperties.BORDER_SPACING, ref this.m_fCellSpacing);
			pTag.GetIntAttribute("CellPadding", ref num);
			this.m_fCellPadding = (float)num;
			this.m_nAlignment = HtmlAttributes.htmlNone;
			if (pTag.m_nType == TagType.TAG_TABLE)
			{
				pTag.GetStandardAttribute("Align", ref this.m_nAlignment);
			}
			float num2 = 12f;
			this.m_pTreeItem.GetNumericProperty(CssProperties.FONT_SIZE, ref num2);
			if (pTag.IsOneOf(11u))
			{
				this.m_arrMargins[0] = (this.m_arrMargins[2] = num2);
				this.m_arrMargins[1] = (this.m_arrMargins[3] = (float)HtmlManager.TAB_SHIFT);
				if (pTag.m_nType == TagType.TAG_UL || pTag.m_nType == TagType.TAG_OL)
				{
					this.m_arrMargins[1] = 0f;
					if (this.m_pTreeItem.m_pParent != null && (this.m_pTreeItem.m_pParent.m_dwTagFlag & 31u) != 0u)
					{
						this.m_arrMargins[0] = (this.m_arrMargins[2] = 0f);
					}
				}
			}
			if (pTag.IsOneOf(768u))
			{
				this.m_arrMargins[0] = (this.m_arrMargins[2] = num2);
			}
			if (pTag.m_nType == TagType.TAG_DD)
			{
				this.m_arrMargins[3] = (float)HtmlManager.TAB_SHIFT;
			}
			if (pTag.m_nType == TagType.TAG_LI)
			{
				if (pParentCell != null && (pParentCell.m_nType == TagType.TAG_UL || pParentCell.m_nType == TagType.TAG_OL))
				{
					if (pTag.GetIntAttribute("value", ref this.m_nBulletNumber))
					{
						if (this.m_nBulletNumber <= 0)
						{
							this.m_nBulletNumber = 1;
						}
						pParentCell.m_pTable.m_nBulletCounter = this.m_nBulletNumber;
					}
					else
					{
						pParentCell.m_pTable.m_nBulletCounter++;
						this.m_nBulletNumber = pParentCell.m_pTable.m_nBulletCounter;
					}
				}
				this.m_pTreeItem.GetEnumProperty(CssProperties.LIST_STYLE_TYPE, ref this.m_nBulletType);
				this.m_pTreeItem.GetEnumProperty(CssProperties.LIST_STYLE_POSITION, ref this.m_nBulletPosition);
				this.m_pTreeItem.GetStringProperty(CssProperties.LIST_STYLE_IMAGE, ref this.m_bstrBulletPath);
			}
			if (this.m_pTreeItem.GetEnumProperty(CssProperties.FLOAT, ref cssProperties, false))
			{
				if (cssProperties == CssProperties.V_RIGHT)
				{
					this.m_nAlignment = HtmlAttributes.htmlRight;
				}
				else
				{
					if (cssProperties == CssProperties.V_LEFT)
					{
						this.m_nAlignment = HtmlAttributes.htmlLeft;
					}
				}
			}
			if (this.m_nAlignment == HtmlAttributes.htmlCenter)
			{
				this.m_dwAlignFlags |= 256u;
			}
			if (this.m_nAlignment != HtmlAttributes.htmlLeft && this.m_nAlignment != HtmlAttributes.htmlRight)
			{
				this.m_nAlignment = HtmlAttributes.htmlNone;
			}
			if (this.m_dwAlignFlags == 0u && this.m_pTreeItem.m_pParent != null && this.m_pTreeItem.m_pParent.GetEnumProperty(CssProperties.TEXT_ALIGN, ref cssProperties))
			{
				if (cssProperties == CssProperties.V_RIGHT)
				{
					this.m_dwAlignFlags = 128u;
				}
				else
				{
					if (cssProperties == CssProperties.V_CENTER)
					{
						this.m_dwAlignFlags = 256u;
					}
					else
					{
						if (cssProperties == CssProperties.V_LEFT)
						{
							this.m_dwAlignFlags = 0u;
						}
					}
				}
			}
			bool flag = false;
			float num3 = 0f;
			if (this.m_pTreeItem.GetNumericProperty(CssProperties.WIDTH, ref num3, ref flag, false) && num3 > 0f)
			{
				if (flag && num3 <= 100f && num3 > 0f)
				{
					if (pParentCell.Width == 0f && pParentCell.m_nType == TagType.TAG_TD && pParentCell.m_pTable.m_fSpecifiedWidth == 0f)
					{
						this.m_bTrimmable = true;
					}
					if (this.m_bAbsolutePosition)
					{
						this.m_bTrimmable = true;
					}
					this.m_fSpecifiedWidth = 0f;
					this.m_fPercentWidth = num3;
					this.m_bFlexibleWidth = true;
					if (pParentCell.m_nType == TagType.TAG_UNKNOWN)
					{
					}
				}
				else
				{
					this.m_fSpecifiedWidth = num3;
				}
				this.m_fMaxWidth = this.m_fSpecifiedWidth;
			}
			else
			{
				this.m_fMaxWidth = pParentCell.Width;
				this.m_bFlexibleWidth = true;
				this.m_bTrimmable = true;
				this.m_fSpecifiedWidth = 0f;
			}
			flag = false;
			if (this.m_pTreeItem.GetNumericProperty(CssProperties.HEIGHT, ref num3, ref flag, false))
			{
				if (flag)
				{
					float num4;
					if (pParentCell.m_nType == TagType.TAG_TD || pParentCell.m_nType == TagType.TAG_TH)
					{
						num4 = 0f;
					}
					else
					{
						num4 = this.m_pHtmlManager.m_fDefaultPageHeight;
					}
					this.m_fSpecifiedHeight = num3 * num4 / 100f;
				}
				else
				{
					this.m_fSpecifiedHeight = num3;
				}
			}
			else
			{
				this.m_fSpecifiedHeight = 0f;
			}
			this.m_fCellPadding *= HtmlManager.fEmpiricalScaleFactor;
			this.m_pTreeItem.GetColorProperty(CssProperties.BACKGROUND_COLOR, ref this.m_rgbBgColor, false);
			string item = null;
			if (this.m_pTreeItem.GetStringProperty(CssProperties.BACKGROUND_IMAGE, ref item, false))
			{
				this.m_pBkImage = new HtmlImage(this.m_pHtmlManager, 0f);
				this.m_pBkImage.m_nParentContainerType = this.m_nRepresentsTag;
				HtmlTag htmlTag = new HtmlTag(this.m_pHtmlManager);
				htmlTag.m_arrNames.Add("SRC");
				htmlTag.m_arrValues.Add(item);
				this.m_pBkImage.Load(htmlTag, true);
				if (this.m_pBkImage.m_bNotFound)
				{
					this.m_pBkImage = null;
				}
				else
				{
					this.m_rgbBgColor.Reset();
				}
				this.m_nRepeatStyle = CssProperties.BACKGROUND_REPEAT;
				this.m_pTreeItem.GetEnumProperty(CssProperties.BACKGROUND_REPEAT, ref this.m_nRepeatStyle, false);
				this.m_nRepeatPositionX = CssProperties.V_LEFT;
				this.m_nRepeatPositionY = CssProperties.V_TOP;
				this.m_pTreeItem.GetBackgroundPositionProperties(CssProperties.BACKGROUND_POSITION, ref this.m_nRepeatPositionX, ref this.m_nRepeatPositionY, false);
			}
			double num5 = (double)this.m_arrMargins[0];
			double num6 = (double)this.m_arrMargins[2];
			this.m_pTreeItem.GetMarginProperties(ref this.m_arrMargins);
			if (pTag.IsOneOf(779u))
			{
				if (num5 == (double)this.m_arrMargins[0])
				{
					this.m_bFlexibleTopMargin = true;
				}
				if (num6 == (double)this.m_arrMargins[2])
				{
					this.m_bFlexibleBottomMargin = true;
				}
			}
			this.m_arrBorderColors[0].Set(15000804u);
			this.m_arrBorderColors[3].Set(15000804u);
			this.m_arrBorderColors[1].Set(6579300u);
			this.m_arrBorderColors[2].Set(6579300u);
			this.m_arrBorders[0] = (this.m_arrBorders[1] = (this.m_arrBorders[2] = (this.m_arrBorders[3] = 0f)));
			this.m_arrBorderStyles[0] = (this.m_arrBorderStyles[1] = (this.m_arrBorderStyles[2] = (this.m_arrBorderStyles[3] = CssProperties.V_NONE)));
			this.m_pTreeItem.GetBorderProperties(this.m_arrBorderStyles, ref this.m_arrBorders, ref this.m_arrBorderColors);
			int num7 = -999;
			if (pTag.GetIntAttribute("Border", ref num7) && (num7 > 0 || num7 == -999))
			{
				this.m_fCellBorder = 1f * HtmlManager.fEmpiricalScaleFactor;
			}
			if (this.m_pTreeItem.GetEnumProperty(CssProperties.BORDER_COLLAPSE, ref cssProperties) && cssProperties == CssProperties.V_COLLAPSE)
			{
				this.m_fCellSpacing = -(this.m_arrBorders[0] + this.m_arrBorders[1] + this.m_arrBorders[2] + this.m_arrBorders[3]) / 4f;
				this.m_bBorderCollapse = true;
			}
			this.m_nRepresentsTag = TagType.TAG_NOTATAG;
			if (pTag.m_nType != TagType.TAG_TABLE)
			{
				this.m_nRepresentsTag = pTag.m_nType;
				this.m_arrBorders[0] = (this.m_arrBorders[1] = (this.m_arrBorders[2] = (this.m_arrBorders[3] = 0f)));
				this.m_arrBorderStyles[0] = (this.m_arrBorderStyles[1] = (this.m_arrBorderStyles[2] = (this.m_arrBorderStyles[3] = CssProperties.V_NONE)));
				this.m_fCellSpacing = 0f;
				this.m_fCellPadding = 0f;
				if (this.m_fSpecifiedWidth - this.m_arrMargins[1] - this.m_arrMargins[3] > 0f)
				{
					this.m_fSpecifiedWidth -= this.m_arrMargins[1] + this.m_arrMargins[3];
				}
			}
			base.ObtainPageBreakStatus();
		}
		private bool IsSwallowed(int nRow, int nCol, int nRowSpan, int nColSpan)
		{
			if (nRowSpan == 0 && nColSpan == 0)
			{
				return false;
			}
			while (this.m_arrSwallowDiagram.Count < nCol + nColSpan - 1)
			{
				this.m_arrSwallowDiagram.Add(0);
			}
			if (this.m_arrSwallowDiagram[nCol - 1] < nRow)
			{
				for (int i = 0; i < nColSpan; i++)
				{
					this.m_arrSwallowDiagram[nCol - 1 + i] = nRow + nRowSpan - 1;
				}
				return false;
			}
			return true;
		}
		private void GoToNext(bool bNewRow, int nRowSpan, int nColSpan)
		{
			if (bNewRow)
			{
				this.m_nRowCount++;
				CssProperties cssProperties = CssProperties.NOTFOUND;
				if (this.m_pHtmlManager.m_objCSS.m_pCurrentLeaf.GetEnumProperty(CssProperties.DISPLAY, ref cssProperties, false) && cssProperties == CssProperties.V_NONE)
				{
					this.m_nRowCount--;
				}
			}
			if (this.m_nRowCount == 0)
			{
				this.m_nColCount = 1;
				return;
			}
			int num = 1;
			while (this.IsSwallowed(this.m_nRowCount, num, nRowSpan, nColSpan))
			{
				num++;
				if (bNewRow && num > this.m_nColCount)
				{
					num = 1;
					this.m_nRowCount++;
				}
			}
			this.m_nColCount = num;
		}
		public override void Parse()
		{
			HtmlTag htmlTag = new HtmlTag(this.m_pHtmlManager);
			if (this.m_nRepresentsTag != TagType.TAG_NOTATAG)
			{
				HtmlCell htmlCell = this.AddCell(htmlTag);
				htmlCell.m_nType = this.m_nRepresentsTag;
				htmlCell.m_nVAlign = HtmlAttributes.htmlTop;
				htmlCell.Parse();
				this.AddBullet();
				return;
			}
			while (true)
			{
				if (base.m_c == '<' && base.ParseTag(ref htmlTag))
				{
					this.m_pHtmlManager.m_objCSS.AddToTree(htmlTag);
					if (htmlTag.m_nType == TagType.TAG_TR && !htmlTag.m_bClosing)
					{
						this.GoToNext(true, 0, 0);
					}
					else
					{
						if ((htmlTag.m_nType == TagType.TAG_TD || htmlTag.m_nType == TagType.TAG_TH) && !htmlTag.m_bClosing)
						{
							HtmlCell htmlCell2 = this.AddCell(htmlTag);
							htmlCell2.Parse();
						}
						else
						{
							if (htmlTag.m_nType == TagType.TAG_CAPTION && !htmlTag.m_bClosing)
							{
								this.AddCaption(htmlTag);
							}
							else
							{
								if (htmlTag.m_nType == TagType.TAG_TABLE && htmlTag.m_bClosing)
								{
									break;
								}
							}
						}
					}
				}
				else
				{
					if (HtmlObject.IsSpace(base.m_c))
					{
						base.Next();
					}
					else
					{
						base.Next();
					}
				}
			}
		}
		public HtmlCell AddCell(HtmlTag pTag)
		{
			HtmlCell htmlCell = new HtmlCell(this);
			htmlCell.m_pTag = new HtmlTag(this.m_pHtmlManager);
			pTag.CopyTo(ref htmlCell.m_pTag);
			if (this.m_nColCount == 0 && this.m_nRowCount == 0)
			{
				this.m_nColCount = (this.m_nRowCount = 1);
			}
			htmlCell.m_pTreeItem = this.m_pHtmlManager.m_objCSS.m_pCurrentLeaf;
			htmlCell.m_nType = pTag.m_nType;
			this.m_arrCells.Add(htmlCell);
			pTag.GetIntAttribute("ColSpan", ref htmlCell.m_nColSpan);
			if (htmlCell.m_nColSpan <= 0)
			{
				htmlCell.m_nColSpan = 1;
			}
			pTag.GetIntAttribute("RowSpan", ref htmlCell.m_nRowSpan);
			if (htmlCell.m_nRowSpan <= 0)
			{
				htmlCell.m_nRowSpan = 1;
			}
			this.GoToNext(false, htmlCell.m_nRowSpan, htmlCell.m_nColSpan);
			htmlCell.m_nColNum = this.m_nColCount;
			htmlCell.m_nRowNum = this.m_nRowCount;
			float num = 0f;
			bool flag = false;
			if (htmlCell.m_pTreeItem.GetNumericProperty(CssProperties.WIDTH, ref num, ref flag, false))
			{
				if (flag)
				{
					htmlCell.Width = this.m_fMaxWidth * num / 100f;
					htmlCell.m_fSpecifiedWidth = htmlCell.Width;
					htmlCell.m_bFlexibleWidth = true;
					htmlCell.m_fPercentWidth = num;
					if (num == 100f)
					{
						htmlCell.m_fSpecifiedWidth = 0f;
						htmlCell.Width = 0f;
					}
				}
				else
				{
					htmlCell.Width = num;
					htmlCell.m_fSpecifiedWidth = htmlCell.Width;
					htmlCell.m_bFlexibleWidth = false;
					if (htmlCell.m_fSpecifiedWidth > this.m_pHtmlManager.m_pMasterTable.m_arrCells[0].Width)
					{
						htmlCell.m_fSpecifiedWidth = 0f;
						htmlCell.Width = 0f;
						htmlCell.m_bFlexibleWidth = true;
					}
				}
			}
			else
			{
				htmlCell.m_bFlexibleWidth = true;
			}
			if (!htmlCell.m_pTreeItem.GetNumericProperty(CssProperties.HEIGHT, ref htmlCell.m_fSpecifiedHeight, ref flag, false) && htmlCell.m_pTreeItem.m_pParent != null && htmlCell.m_pTreeItem.m_pParent.m_nType == TagType.TAG_TR)
			{
				htmlCell.m_pTreeItem.m_pParent.GetNumericProperty(CssProperties.HEIGHT, ref htmlCell.m_fSpecifiedHeight, ref flag, false);
			}
			CssProperties cssProperties = CssProperties.NOTFOUND;
			if (htmlCell.m_pTreeItem.GetEnumProperty(CssProperties.VERTICAL_ALIGN, ref cssProperties, false))
			{
				htmlCell.m_nVAlign = ((cssProperties == CssProperties.V_TOP) ? HtmlAttributes.htmlTop : ((cssProperties == CssProperties.V_BOTTOM) ? HtmlAttributes.htmlBottom : HtmlAttributes.htmlMiddle));
			}
			else
			{
				if (htmlCell.m_pTreeItem.m_pParent != null && htmlCell.m_pTreeItem.m_pParent.m_nType == TagType.TAG_TR && htmlCell.m_pTreeItem.m_pParent.GetEnumProperty(CssProperties.VERTICAL_ALIGN, ref cssProperties, false))
				{
					htmlCell.m_nVAlign = ((cssProperties == CssProperties.V_TOP) ? HtmlAttributes.htmlTop : ((cssProperties == CssProperties.V_BOTTOM) ? HtmlAttributes.htmlBottom : HtmlAttributes.htmlMiddle));
				}
			}
			if (pTag.GetStringAttribute("NoWrap"))
			{
				htmlCell.m_bNoWrap = true;
			}
			if (htmlCell.m_pTreeItem.GetEnumProperty(CssProperties.WHITE_SPACE, ref cssProperties) && cssProperties == CssProperties.V_NOWRAP)
			{
				htmlCell.m_bNoWrap = true;
			}
			if (!htmlCell.m_pTreeItem.GetColorProperty(CssProperties.BACKGROUND_COLOR, ref htmlCell.m_rgbBgColor, false) && htmlCell.m_pTreeItem.m_pParent != null && htmlCell.m_pTreeItem.m_pParent.m_nType == TagType.TAG_TR)
			{
				htmlCell.m_pTreeItem.m_pParent.GetColorProperty(CssProperties.BACKGROUND_COLOR, ref htmlCell.m_rgbBgColor, false);
			}
			string text = null;
			htmlCell.m_pBkImage = null;
			if (!htmlCell.m_pTreeItem.GetStringProperty(CssProperties.BACKGROUND_IMAGE, ref text, false) && htmlCell.m_pTreeItem.m_pParent != null && htmlCell.m_pTreeItem.m_pParent.m_nType == TagType.TAG_TR)
			{
				htmlCell.m_pTreeItem.m_pParent.GetStringProperty(CssProperties.BACKGROUND_IMAGE, ref text, false);
			}
			if (htmlCell.m_pTreeItem.m_pParent != null && htmlCell.m_pTreeItem.m_pParent.m_nType == TagType.TAG_TR && !htmlCell.m_bDisplayNone)
			{
				htmlCell.m_bDisplayNone = (htmlCell.m_pTreeItem.m_pParent.GetEnumProperty(CssProperties.DISPLAY, ref cssProperties, false) && cssProperties == CssProperties.V_NONE);
			}
			if (text != null && text.Length > 0)
			{
				htmlCell.m_pBkImage = new HtmlImage(this.m_pHtmlManager, 0f);
				htmlCell.m_pBkImage.m_nParentContainerType = TagType.TAG_TD;
				HtmlTag htmlTag = new HtmlTag(this.m_pHtmlManager);
				htmlTag.m_arrNames.Add("SRC");
				htmlTag.m_arrValues.Add(text);
				htmlCell.m_pBkImage.Load(htmlTag, true);
				if (htmlCell.m_pBkImage.m_bNotFound)
				{
					htmlCell.m_pBkImage = null;
				}
				else
				{
					htmlCell.m_rgbBgColor.Reset();
				}
				htmlCell.m_nRepeatStyle = CssProperties.BACKGROUND_REPEAT;
				htmlCell.m_pTreeItem.GetEnumProperty(CssProperties.BACKGROUND_REPEAT, ref htmlCell.m_nRepeatStyle, false);
				htmlCell.m_nRepeatPositionX = CssProperties.V_LEFT;
				htmlCell.m_nRepeatPositionY = CssProperties.V_TOP;
				htmlCell.m_pTreeItem.GetBackgroundPositionProperties(CssProperties.BACKGROUND_POSITION, ref htmlCell.m_nRepeatPositionX, ref htmlCell.m_nRepeatPositionY, false);
			}
			htmlCell.m_arrPadding[0] = (htmlCell.m_arrPadding[1] = (htmlCell.m_arrPadding[2] = (htmlCell.m_arrPadding[3] = this.m_fCellPadding)));
			htmlCell.m_pTreeItem.GetMarginProperties(ref htmlCell.m_arrPadding, true);
			htmlCell.m_arrBorderColors[1].Set(15000804u);
			htmlCell.m_arrBorderColors[2].Set(15000804u);
			htmlCell.m_arrBorderColors[0].Set(6579300u);
			htmlCell.m_arrBorderColors[3].Set(6579300u);
			htmlCell.m_arrBorders[0] = (htmlCell.m_arrBorders[1] = (htmlCell.m_arrBorders[2] = (htmlCell.m_arrBorders[3] = this.m_fCellBorder)));
			htmlCell.m_arrBorderStyles[0] = (htmlCell.m_arrBorderStyles[1] = (htmlCell.m_arrBorderStyles[2] = (htmlCell.m_arrBorderStyles[3] = ((this.m_fCellBorder > 0f) ? CssProperties.V_SOLID : CssProperties.V_NONE))));
			htmlCell.m_pTreeItem.GetBorderProperties(htmlCell.m_arrBorderStyles, ref htmlCell.m_arrBorders, ref htmlCell.m_arrBorderColors);
			if (this.m_bBorderCollapse)
			{
				for (int i = 0; i < 4; i++)
				{
					if (htmlCell.m_arrBorders[i] < Math.Abs(this.m_fCellSpacing))
					{
						this.m_fCellSpacing = -htmlCell.m_arrBorders[i];
					}
				}
			}
			return htmlCell;
		}
		private void AddCaption(HtmlTag pTag)
		{
			if (this.m_pCaption == null)
			{
				this.m_pCaption = new HtmlTable(this.m_pHtmlManager, this.m_pHtmlManager.m_pMasterTable.m_arrCells[0], pTag);
				this.m_pCaption.m_bTrimmable = false;
				this.m_pCaption.AddCell(pTag);
				this.m_pCaption.m_fMaxWidth = 0f;
				this.m_pCaption.m_nParentContainerType = pTag.m_nType;
			}
			this.m_pCaption.m_arrCells[0].Parse();
			this.m_pCaption.AssignCoordinates();
			if (this.m_pCaption.m_bDisplayNone)
			{
				this.m_pCaption = null;
			}
		}
		private void AdjustCellSizes()
		{
			if (!this.m_bGridLinesChanged)
			{
				return;
			}
			foreach (HtmlCell current in this.m_arrCells)
			{
				current.Width = this.ColWidth(current.m_nColNum);
				for (int i = 1; i < current.m_nColSpan; i++)
				{
					current.chgWidth(this.m_fCellSpacing + this.ColWidth(current.m_nColNum + i));
				}
				current.Height = this.RowHeight(current.m_nRowNum);
				for (int i = 1; i < current.m_nRowSpan; i++)
				{
					current.chgHeight(this.m_fCellSpacing + this.RowHeight(current.m_nRowNum + i));
				}
			}
			this.m_bGridLinesChanged = false;
		}
		private void AdjustCellWidths(int nRow, int nCol, float fWidthIncrease)
		{
			HtmlCell cell = this.GetCell(nRow, nCol);
			int num = cell.m_nColNum + cell.m_nColSpan - 1;
			this.m_pVertLines[num].m_fCurPos += fWidthIncrease + 0.001f;
			for (int i = num + 1; i <= this.m_nCols; i++)
			{
				if (this.m_pVertLines[i].m_fCurPos < this.m_pVertLines[num].m_fCurPos)
				{
					this.m_pVertLines[i].m_fCurPos = this.m_pVertLines[num].m_fCurPos;
				}
			}
			this.m_bGridLinesChanged = true;
			this.AdjustCellSizes();
		}
		private void AdjustCellHeights(int nRow, int nCol, float fNewHeight)
		{
			HtmlCell cell = this.GetCell(nRow, nCol);
			int num = cell.m_nRowNum + cell.m_nRowSpan - 1;
			if (fNewHeight > this.RowHeight(num))
			{
				float fCurPos = this.m_pHorizLines[num].m_fCurPos;
				this.m_pHorizLines[num].m_fCurPos = this.m_pHorizLines[num - cell.m_nRowSpan].m_fCurPos + fNewHeight;
				if (this.m_pHorizLines[num].m_fCurPos != fCurPos)
				{
					this.m_bGridLinesChanged = true;
				}
				for (int i = num + 1; i <= this.m_nRows; i++)
				{
					if (this.m_pHorizLines[i].m_fCurPos < this.m_pHorizLines[num].m_fCurPos)
					{
						this.m_pHorizLines[i].m_fCurPos = this.m_pHorizLines[num].m_fCurPos;
						this.m_bGridLinesChanged = true;
					}
				}
			}
			if (fNewHeight > this.m_pHeightMatrix[cell.m_nRowNum, cell.m_nRowNum + cell.m_nRowSpan - 1])
			{
				this.m_pHeightMatrix[cell.m_nRowNum, cell.m_nRowNum + cell.m_nRowSpan - 1] = fNewHeight;
			}
			this.AdjustCellSizes();
		}
		private bool PopulateTable()
		{
			this.ResetHeightMatrix();
			for (int i = 1; i <= this.m_nRows; i++)
			{
				for (int j = 1; j <= this.m_nCols; j++)
				{
					HtmlCell cell = this.GetCell(i, j);
					if (cell != null)
					{
						try
						{
							cell.X = 0f;
							cell.Y = 0f;
							cell.m_fCurX = cell.X;
							cell.m_fCurY = cell.Y;
							cell.AssignCoordinates();
							cell.chgWidth(cell.m_arrBorders[3] + cell.m_arrBorders[1] + cell.m_arrPadding[3] + cell.m_arrPadding[1]);
							cell.Height = cell.GetMaxY() + cell.m_arrBorders[2] + cell.m_arrBorders[0] + cell.m_arrPadding[2] + cell.m_arrPadding[0];
							this.AdjustCellHeights(i, j, cell.Height);
						}
						catch (PdfException ex)
						{
							int arg_C7_0 = ex.m_nCode;
							float fWidth = ex.m_fWidth;
							this.AdjustCellWidths(i, j, fWidth);
							this.ResetInvisibleObjects();
							this.ClearArrays();
							return false;
						}
					}
				}
			}
			this.Trim();
			if (this.m_pCaption != null)
			{
				this.CalculateSize();
				while (!this.m_pCaption.PopulateTable())
				{
				}
				if (this.m_pCaption.m_fWidth < this.m_fWidth - this.m_arrMargins[1] - this.m_arrMargins[3])
				{
					this.m_pCaption.ExpandIfNecessary(this.m_fWidth - this.m_arrMargins[1] - this.m_arrMargins[3]);
				}
				this.m_fCaptionHeight = this.m_pCaption.m_fHeight;
				this.m_fCaptionWidth = this.m_pCaption.m_fWidth;
			}
			this.ResolveHeightMatrix();
			this.CalculateSize();
			return true;
		}
		public override void AssignCoordinates()
		{
			if (this.m_bCoordinatesAssigned)
			{
				return;
			}
			if (this.m_arrCells.Count == 0)
			{
				return;
			}
			this.DetermineTableSize();
			this.ResetWidthMatrix();
			this.ResolveWidthMatrix();
			while (!this.PopulateTable())
			{
			}
			this.m_bCoordinatesAssigned = true;
		}
		public override void CalculateSize()
		{
			float num = this.m_arrBorders[1] + this.m_arrBorders[3] + (float)(this.m_nCols + 1) * this.m_fCellSpacing;
			float num2 = this.m_arrBorders[0] + this.m_arrBorders[2] + (float)(this.m_nRows + 1) * this.m_fCellSpacing;
			for (int i = 1; i <= this.m_nCols; i++)
			{
				num += this.ColWidth(i);
			}
			for (int i = 1; i <= this.m_nRows; i++)
			{
				num2 += this.RowHeight(i);
			}
			this.m_fWidth = ((this.m_fCaptionWidth > num) ? this.m_fCaptionWidth : num);
			this.m_fHeight = num2 + this.m_fCaptionHeight;
			this.m_fWidth += this.m_arrMargins[1] + this.m_arrMargins[3];
			this.m_fHeight += this.m_arrMargins[0] + this.m_arrMargins[2];
		}
		public override bool Render(PdfPage pPage, PdfCanvas pCanvas, float fShiftX, float fShiftY)
		{
			if (this.m_arrCells.Count == 0 || this.m_bDisplayNone)
			{
				return true;
			}
			base.HandlePageBreakBefore(fShiftY);
			float num = this.m_fCaptionHeight;
			if (this.m_pCaption != null)
			{
				float num2;
				if (this.m_pCaption.m_arrCells[0].m_nVAlign == HtmlAttributes.htmlBottom)
				{
					num = 0f;
					num2 = this.m_fY + this.m_fHeight - this.m_fCaptionHeight - this.m_arrMargins[2];
				}
				else
				{
					num2 = this.m_fY + this.m_arrMargins[0];
				}
				this.m_pCaption.Render(pPage, pCanvas, this.m_fX + this.m_arrMargins[3] + fShiftX, num2 + fShiftY);
			}
			float num3 = this.m_arrBorders[1] + this.m_arrBorders[3] + (float)(this.m_nCols + 1) * this.m_fCellSpacing;
			float num4 = this.m_arrBorders[0] + this.m_arrBorders[2] + (float)(this.m_nRows + 1) * this.m_fCellSpacing;
			for (int i = 1; i <= this.m_nCols; i++)
			{
				num3 += this.ColWidth(i);
			}
			for (int i = 1; i <= this.m_nRows; i++)
			{
				num4 += this.RowHeight(i);
			}
			this.FillBackground(this.m_fX + fShiftX + this.m_arrBorders[3] + this.m_arrMargins[3], this.m_pHtmlManager.m_fShiftY - (this.m_fY + fShiftY + num) - this.m_arrBorders[0] - this.m_arrMargins[0], num3 - this.m_arrBorders[1] - this.m_arrBorders[3], -num4 + this.m_arrBorders[0] + this.m_arrBorders[2], pPage, pCanvas, this.m_rgbBgColor);
			if (this.m_pBkImage != null)
			{
				this.TileImage(this.m_fX + fShiftX + this.m_arrMargins[3] + this.m_arrBorders[3], this.m_fY + fShiftY + num + this.m_arrMargins[0] + this.m_arrBorders[0], num3 - this.m_arrBorders[1] - this.m_arrBorders[3], num4 - this.m_arrBorders[0] - this.m_arrBorders[2], this.m_pBkImage, pPage, pCanvas, this.m_nRepeatStyle, this.m_nRepeatPositionX, this.m_nRepeatPositionY);
			}
			base.DrawBorder(pCanvas, fShiftX, fShiftY, 0f, this.m_fCaptionHeight - num, this.m_arrMargins[3] + this.m_arrMargins[1] + num3 - this.m_fWidth, -this.m_fCaptionHeight);
			foreach (HtmlCell current in this.m_arrCells)
			{
				float num5 = this.m_arrBorders[3] + this.m_fCellSpacing;
				float num6 = this.m_arrBorders[0] + this.m_fCellSpacing;
				for (int i = 1; i < current.m_nColNum; i++)
				{
					num5 += this.ColWidth(i) + this.m_fCellSpacing;
				}
				for (int i = 1; i < current.m_nRowNum; i++)
				{
					num6 += this.RowHeight(i) + this.m_fCellSpacing;
				}
				float num7 = (current.Height - current.GetMaxY() - current.m_arrPadding[0] - current.m_arrBorders[0] - current.m_arrPadding[2] - current.m_arrBorders[2]) / 2f;
				if (current.m_nVAlign != HtmlAttributes.htmlMiddle)
				{
					num7 = ((current.m_nVAlign == HtmlAttributes.htmlBottom) ? (num7 * 2f) : 0f);
				}
				current.AdjustAbsoluteObjects(-num5 - (this.m_bAbsolutePosition ? 0f : (this.m_fX + this.m_arrMargins[3] + current.m_arrPadding[3] + current.m_arrBorders[3])), -num6 - num7 - (this.m_bAbsolutePosition ? 0f : (fShiftY + this.m_fY + num + this.m_arrMargins[0] + current.m_arrPadding[0] + current.m_arrBorders[0])));
				current.X = -(current.m_arrPadding[3] + current.m_arrBorders[3]);
				current.Y = -(current.m_arrPadding[0] + current.m_arrBorders[0]);
				this.FillBackground(this.m_fX + fShiftX + num5 + this.m_arrMargins[3] + current.m_arrBorders[3], this.m_pHtmlManager.m_fShiftY - this.m_fY - fShiftY - num6 - num - this.m_arrMargins[0] - current.m_arrBorders[0], current.Width - current.m_arrBorders[3] - current.m_arrBorders[1], -(current.Height - current.m_arrBorders[0] - current.m_arrBorders[2]), pPage, pCanvas, current.m_rgbBgColor);
				if (current.m_pBkImage != null)
				{
					this.TileImage(this.m_fX + fShiftX + num5 + current.m_arrBorders[3] + this.m_arrMargins[3], this.m_fY + fShiftY + num6 + current.m_arrBorders[0] + num + this.m_arrMargins[0], current.Width - current.m_arrBorders[3] - current.m_arrBorders[1], current.Height - current.m_arrBorders[0] - current.m_arrBorders[2], current.m_pBkImage, pPage, pCanvas, current.m_nRepeatStyle, current.m_nRepeatPositionX, current.m_nRepeatPositionY);
				}
				if (current.m_arrObjects.Count > 0 || this.m_nRepresentsTag != TagType.TAG_NOTATAG)
				{
					current.DrawBorder(pCanvas, fShiftX + this.m_fX + num5 + this.m_arrMargins[3] + current.m_arrPadding[3] + current.m_arrBorders[3], fShiftY + this.m_fY + num6 + num + this.m_arrMargins[0] + current.m_arrPadding[0] + current.m_arrBorders[0]);
				}
				current.Render(pPage, pCanvas, fShiftX + this.m_fX + num5 + this.m_arrMargins[3] + current.m_arrPadding[3] + current.m_arrBorders[3], fShiftY + this.m_fY + num6 + num + num7 + this.m_arrMargins[0] + current.m_arrPadding[0] + current.m_arrBorders[0]);
			}
			return true;
		}
		private void FillBackground(float x, float y, float width, float height, PdfPage pPage, PdfCanvas pCanvas, AuxRGB pRGB)
		{
			if (pRGB == null || !pRGB.m_bIsSet)
			{
				return;
			}
			if (y < this.m_pHtmlManager.m_fLowerMargin || y + height > this.m_pHtmlManager.m_fLowerMargin + this.m_pHtmlManager.m_fFrameHeight)
			{
				return;
			}
			pCanvas.SaveState();
			pCanvas.SetFillColor(pRGB.r, pRGB.g, pRGB.b);
			pCanvas.FillRect(x, y, width, height);
			pCanvas.RestoreState();
		}
		public void TileImage(float x, float y, float width, float height, HtmlImage pImage, PdfPage pPage, PdfCanvas pCanvas, CssProperties nRepeatStyle, CssProperties nRepeatPositionX, CssProperties nRepeatPositionY)
		{
			if (y + height < this.m_pHtmlManager.m_fFrameY || y > this.m_pHtmlManager.m_fFrameY + this.m_pHtmlManager.m_fFrameHeight)
			{
				return;
			}
			float fScale = this.m_pHtmlManager.m_fScale;
			float num = 0f;
			float num2 = 0f;
			if (nRepeatPositionX == CssProperties.V_CENTER)
			{
				num = (width - pImage.Width) / 2f;
			}
			else
			{
				if (nRepeatPositionX == CssProperties.V_RIGHT)
				{
					num = width - pImage.Width;
				}
			}
			if (nRepeatPositionY == CssProperties.V_CENTER)
			{
				num2 = (height - pImage.Height) / 2f;
			}
			else
			{
				if (nRepeatPositionY == CssProperties.V_BOTTOM)
				{
					num2 = height - pImage.Height;
				}
			}
			PdfParam pdfParam = new PdfParam();
			pdfParam["left"] = 0f;
			pdfParam["bottom"] = 0f;
			pdfParam["pattern"] = 1f;
			pdfParam["right"] = pImage.Width * fScale;
			pdfParam["top"] = pImage.Height * fScale;
			pdfParam["e"] = 0f;
			pdfParam["f"] = 0f;
			if (this.m_pHtmlManager.m_bLandscape)
			{
				pdfParam["a"] = 0f;
				pdfParam["b"] = 1f;
				pdfParam["c"] = -1f;
				pdfParam["d"] = 0f;
				pdfParam["e"] = pPage.Width - (this.m_pHtmlManager.m_fShiftY - y - num2) * fScale;
				pdfParam["f"] = (x + num) * fScale;
			}
			else
			{
				pdfParam["a"] = 1f;
				pdfParam["b"] = 0f;
				pdfParam["c"] = 0f;
				pdfParam["d"] = 1f;
				pdfParam["e"] = (x + num) * fScale;
				pdfParam["f"] = (this.m_pHtmlManager.m_fShiftY - y - num2) * fScale;
			}
			PdfGraphics pdfGraphics = this.m_pHtmlManager.m_pDocument.CreateGraphics(pdfParam);
			pdfParam.Clear();
			pdfParam["x"] = 0f;
			pdfParam["y"] = 0f;
			pdfParam["scalex"] = pImage.m_fScaleX * fScale;
			pdfParam["scaley"] = pImage.m_fScaleY * fScale;
			pdfGraphics.Canvas.DrawImage(pImage.m_ptrImage, pdfParam);
			float num3 = width;
			float num4 = height;
			if ((nRepeatStyle == CssProperties.V_REPEAT_Y || nRepeatStyle == CssProperties.V_NO_REPEAT) && pImage.Width < num3)
			{
				num3 = pImage.Width;
			}
			else
			{
				num = 0f;
			}
			if ((nRepeatStyle == CssProperties.V_REPEAT_X || nRepeatStyle == CssProperties.V_NO_REPEAT) && pImage.Height < num4)
			{
				num4 = pImage.Height;
			}
			else
			{
				num2 = 0f;
			}
			pCanvas.SaveState();
			pCanvas.AddRect(x + num, this.m_pHtmlManager.m_fShiftY - y - num2, num3, -num4);
			pCanvas.FillWithPattern(pdfGraphics, false);
			pCanvas.RestoreState();
		}
		private HtmlCell GetCell(int nRow, int nCol)
		{
			int num = (int)this.m_arrCellIndex[nRow, nCol];
			if (num == 0)
			{
				return null;
			}
			return this.m_arrCells[num - 1];
		}
		private void DetermineTableSize()
		{
			this.m_nRows = 0;
			this.m_nCols = 0;
			foreach (HtmlCell current in this.m_arrCells)
			{
				if (current.m_nColNum + current.m_nColSpan - 1 > this.m_nCols)
				{
					this.m_nCols = current.m_nColNum + current.m_nColSpan - 1;
				}
				if (current.m_nRowNum + current.m_nRowSpan - 1 > this.m_nRows)
				{
					this.m_nRows = current.m_nRowNum + current.m_nRowSpan - 1;
				}
				current.ComputeStats();
			}
			this.m_fSpecifiedWidth -= this.m_arrBorders[1] + this.m_arrBorders[3] + (float)(this.m_nCols + 1) * this.m_fCellSpacing;
			this.m_fSpecifiedHeight -= this.m_arrBorders[0] + this.m_arrBorders[2] + (float)(this.m_nRows + 1) * this.m_fCellSpacing;
			this.m_fMaxWidth -= this.m_arrBorders[1] + this.m_arrBorders[3] + (float)(this.m_nCols + 1) * this.m_fCellSpacing + this.m_arrMargins[1] + this.m_arrMargins[3];
			if (this.m_fSpecifiedWidth < 0f)
			{
				this.m_fSpecifiedWidth = 0f;
			}
			if (this.m_fSpecifiedHeight < 0f)
			{
				this.m_fSpecifiedHeight = 0f;
			}
			if (this.m_fMaxWidth < 0f)
			{
				this.m_fMaxWidth = 0f;
			}
			this.m_pVertLines = new HtmlGridLine[this.m_nCols + 1];
			for (int i = 0; i < this.m_pVertLines.Length; i++)
			{
				this.m_pVertLines[i] = new HtmlGridLine();
			}
			for (int i = 0; i <= this.m_nCols; i++)
			{
				this.m_pVertLines[i].m_fCurPos = (this.m_pVertLines[i].m_fMaxPos = this.m_fSpecifiedWidth);
				this.m_pVertLines[i].m_fMinPos = (this.m_pVertLines[i].m_fMinWidth = 0f);
				this.m_pVertLines[i].m_fSpecified = 0f;
				this.m_pVertLines[i].m_bFlexibleWidth = true;
				this.m_pVertLines[i].m_bPhantom = true;
			}
			this.m_pVertLines[0].m_fMinPos = (this.m_pVertLines[0].m_fCurPos = 0f);
			this.m_pHorizLines = new HtmlGridLine[this.m_nRows + 1];
			for (int i = 0; i < this.m_pHorizLines.Length; i++)
			{
				this.m_pHorizLines[i] = new HtmlGridLine();
			}
			for (int i = 0; i <= this.m_nRows; i++)
			{
				this.m_pHorizLines[i].m_fCurPos = (this.m_pHorizLines[i].m_fMaxPos = this.m_fSpecifiedHeight);
				this.m_pHorizLines[i].m_fMinPos = 0f;
				this.m_pHorizLines[i].m_fSpecified = 0f;
			}
			this.m_pHorizLines[0].m_fMinPos = (this.m_pHorizLines[0].m_fCurPos = 0f);
			foreach (HtmlCell current2 in this.m_arrCells)
			{
				float fSpecifiedHeight = current2.m_fSpecifiedHeight;
				if (fSpecifiedHeight > 0f && fSpecifiedHeight > this.m_pHorizLines[current2.m_nRowNum + current2.m_nRowSpan - 1 - 1].m_fSpecified)
				{
					this.m_pHorizLines[current2.m_nRowNum + current2.m_nRowSpan - 1].m_fSpecified = fSpecifiedHeight;
				}
				float fSpecifiedWidth = current2.m_fSpecifiedWidth;
				if (fSpecifiedWidth > 0f && fSpecifiedWidth > this.m_pVertLines[current2.m_nColNum + current2.m_nColSpan - 1].m_fSpecified)
				{
					this.m_pVertLines[current2.m_nColNum + current2.m_nColSpan - 1].m_fSpecified = fSpecifiedWidth;
				}
				this.m_pVertLines[current2.m_nColNum + current2.m_nColSpan - 1].m_bFlexibleWidth = current2.m_bFlexibleWidth;
				this.m_pVertLines[current2.m_nColNum + current2.m_nColSpan - 1].m_bPhantom = false;
			}
			int num = 0;
			for (int i = 1; i <= this.m_nCols; i++)
			{
				if (this.m_pVertLines[i].m_bPhantom)
				{
					num++;
				}
			}
			if (num > 0)
			{
				foreach (HtmlCell current3 in this.m_arrCells)
				{
					for (int i = this.m_nCols; i >= 1; i--)
					{
						if (this.m_pVertLines[i].m_bPhantom)
						{
							if (current3.m_nColNum <= i && current3.m_nColNum + current3.m_nColSpan - 1 > i)
							{
								current3.m_nColSpan--;
							}
							if (current3.m_nColNum > i)
							{
								current3.m_nColNum--;
							}
						}
					}
				}
				for (int i = 1; i <= this.m_nCols; i++)
				{
					if (this.m_pVertLines[i].m_bPhantom)
					{
						for (int j = i; j <= this.m_nCols; j++)
						{
							this.m_pVertLines[j - 1] = this.m_pVertLines[j];
						}
					}
				}
				this.m_nCols -= num;
			}
			if (this.m_fSpecifiedWidth == 0f)
			{
				bool flag = true;
				for (int i = 1; i <= this.m_nCols; i++)
				{
					this.m_fSpecifiedWidth += this.m_pVertLines[i].m_fSpecified;
					if (this.m_pVertLines[i].m_fSpecified == 0f)
					{
						flag = false;
					}
				}
				if (this.m_fSpecifiedWidth > this.m_fMaxWidth)
				{
					this.m_fSpecifiedWidth = this.m_fMaxWidth;
				}
				if (flag)
				{
					this.m_bFlexibleWidth = false;
				}
			}
			this.m_pHeightMatrix = new AuxSparce2DArray(this.m_nRows, this.m_nRows);
			this.ResetHeightMatrix();
			this.m_pWidthMatrix = new Aux2DArray(this.m_nCols, this.m_nCols);
			this.m_arrCellIndex = new Aux2DArray(this.m_nCols + 1, this.m_nRows + 1);
			int num2 = 1;
			foreach (HtmlCell current4 in this.m_arrCells)
			{
				this.m_arrCellIndex[current4.m_nRowNum, current4.m_nColNum] = (float)num2;
				num2++;
			}
			this.m_bGridLinesChanged = true;
		}
		private void ResetWidthMatrix()
		{
			if (this.m_pWidthMatrix.IsEmpty())
			{
				return;
			}
			this.m_pWidthMatrix.Reset();
			float num = 0f;
			foreach (HtmlCell current in this.m_arrCells)
			{
				if (current.m_fSpecifiedWidth > this.m_pWidthMatrix[current.m_nColNum, current.m_nColNum + current.m_nColSpan - 1])
				{
					this.m_pWidthMatrix[current.m_nColNum, current.m_nColNum + current.m_nColSpan - 1] = current.m_fSpecifiedWidth;
				}
				num += current.m_fSpecifiedWidth;
			}
			for (int i = 1; i <= this.m_nCols; i++)
			{
				float num2 = 0f;
				for (int j = 1; j <= this.m_nRows; j++)
				{
					HtmlCell cell = this.GetCell(j, i);
					if (cell != null && cell.m_nColNum + cell.m_nColSpan - 1 == i)
					{
						float num3 = cell.m_arrBorders[3] + cell.m_arrBorders[1] + cell.m_arrPadding[3] + cell.m_arrPadding[1];
						if (cell.m_fMaxItemWidth + num3 > this.m_pWidthMatrix[cell.m_nColNum, cell.m_nColNum])
						{
							this.m_pWidthMatrix[cell.m_nColNum, cell.m_nColNum] = cell.m_fMaxItemWidth + num3;
						}
						if (cell.m_nColSpan == 1 && cell.m_fMaxItemWidth + num3 > this.m_pVertLines[cell.m_nColNum].m_fMinPos)
						{
							this.m_pVertLines[cell.m_nColNum].m_fMinPos = cell.m_fMaxItemWidth + num3;
						}
						if (cell.m_fMaxLineLength > num2)
						{
							num2 = cell.m_fMaxLineLength;
						}
					}
				}
				this.m_pVertLines[i].m_fRelativeWidth = num2;
			}
			if (this.m_fSpecifiedWidth > 0f)
			{
				this.m_pWidthMatrix[1, this.m_nCols] = this.m_fSpecifiedWidth;
			}
			else
			{
				if (this.m_fMaxWidth > this.m_pWidthMatrix[1, this.m_nCols])
				{
					this.m_pWidthMatrix[1, this.m_nCols] = this.m_fMaxWidth;
				}
			}
			this.m_bGridLinesChanged = true;
		}
		private void ResolveWidthMatrix()
		{
			Aux2DArray aux2DArray = new Aux2DArray(this.m_nCols, 1);
			float num = this.m_pWidthMatrix[1, this.m_nCols];
			if (num > 0f)
			{
				float num2 = 0f;
				float num3 = 0f;
				float num4 = num;
				bool flag = false;
				for (int i = 1; i <= this.m_nCols; i++)
				{
					num2 += this.m_pWidthMatrix[i, i];
					if (this.m_pVertLines[i].m_bFlexibleWidth)
					{
						flag = true;
						num3 += this.m_pWidthMatrix[i, i] - this.m_pVertLines[i].m_fMinPos;
						num4 -= this.m_pVertLines[i].m_fMinPos;
					}
					else
					{
						num4 -= this.m_pWidthMatrix[i, i];
					}
				}
				if (num2 > num && flag && num3 > 0f)
				{
					for (int j = 1; j <= this.m_nCols; j++)
					{
						if (this.m_pVertLines[j].m_bFlexibleWidth)
						{
							float value = this.m_pVertLines[j].m_fMinPos + (this.m_pWidthMatrix[j, j] - this.m_pVertLines[j].m_fMinPos) * num4 / num3;
							this.m_pWidthMatrix[j, j] = value;
						}
					}
				}
			}
			for (int k = 1; k <= this.m_nCols; k++)
			{
				for (int l = 1; l <= this.m_nCols - k + 1; l++)
				{
					float num5 = this.m_pWidthMatrix[l, k + l - 1];
					if (num5 != 0f)
					{
						float num6 = 0f;
						float num7 = 0f;
						for (int m = l; m <= k + l - 1; m++)
						{
							num6 += aux2DArray[1, m];
							if (this.m_pVertLines[m].m_fSpecified == 0f)
							{
								num7 += this.m_pVertLines[m].m_fRelativeWidth;
							}
						}
						if (num6 < num5)
						{
							if (num7 == 0f)
							{
								float num8 = (num5 - num6) / (float)k;
								for (int n = l; n <= k + l - 1; n++)
								{
									aux2DArray[1, n] = aux2DArray[1, n] + num8;
								}
							}
							else
							{
								for (int num9 = l; num9 <= k + l - 1; num9++)
								{
									if (this.m_pVertLines[num9].m_fSpecified == 0f)
									{
										aux2DArray[1, num9] = aux2DArray[1, num9] + (num5 - num6) * this.m_pVertLines[num9].m_fRelativeWidth / num7;
									}
								}
							}
						}
					}
				}
			}
			for (int num10 = 1; num10 <= this.m_nCols; num10++)
			{
				this.m_pVertLines[num10].m_fCurPos = this.m_pVertLines[num10 - 1].m_fCurPos + aux2DArray[1, num10];
			}
			this.m_bGridLinesChanged = true;
			this.AdjustCellSizes();
		}
		private void ResetHeightMatrix()
		{
			this.m_pHeightMatrix.Reset();
			foreach (HtmlCell current in this.m_arrCells)
			{
				if (current.m_fSpecifiedHeight > this.m_pHeightMatrix[current.m_nRowNum, current.m_nRowNum + current.m_nRowSpan - 1])
				{
					this.m_pHeightMatrix[current.m_nRowNum, current.m_nRowNum + current.m_nRowSpan - 1] = current.m_fSpecifiedHeight;
				}
			}
			if (this.m_fSpecifiedHeight > 0f)
			{
				this.m_pHeightMatrix[1, this.m_nRows] = this.m_fSpecifiedHeight;
			}
		}
		private void ResolveHeightMatrix()
		{
			Aux2DArray aux2DArray = new Aux2DArray(this.m_nRows, 1);
			for (int i = 1; i <= this.m_nRows; i++)
			{
				for (int j = 1; j <= this.m_nRows - i + 1; j++)
				{
					float num = this.m_pHeightMatrix[j, i + j - 1];
					if (num != 0f)
					{
						float num2 = 0f;
						int num3 = 0;
						for (int k = j; k <= i + j - 1; k++)
						{
							if (this.m_pHorizLines[k].m_fSpecified == 0f)
							{
								num3++;
							}
							num2 += aux2DArray[1, k];
						}
						if (num2 < num)
						{
							if (num3 == 0)
							{
								float num4 = (num - num2) / (float)i;
								for (int k = j; k <= i + j - 1; k++)
								{
									aux2DArray[1, k] = aux2DArray[1, k] + num4;
								}
							}
							else
							{
								float num5 = (num - num2) / (float)num3;
								for (int k = j; k <= i + j - 1; k++)
								{
									if (this.m_pHorizLines[k].m_fSpecified == 0f)
									{
										aux2DArray[1, k] = aux2DArray[1, k] + num5;
									}
								}
							}
						}
					}
				}
			}
			for (int i = 1; i <= this.m_nRows; i++)
			{
				this.m_pHorizLines[i].m_fCurPos = this.m_pHorizLines[i - 1].m_fCurPos + aux2DArray[1, i];
			}
			this.m_bGridLinesChanged = true;
			this.AdjustCellSizes();
		}
		public override void ExpandIfNecessary(float fAvailWidth)
		{
			if (fAvailWidth <= this.m_fWidth || !this.m_bFlexibleWidth || this.m_pWidthMatrix == null || this.HorizAlignment != HtmlAttributes.htmlNone)
			{
				return;
			}
			this.m_fSpecifiedWidth = 0f;
			this.m_fMaxWidth = fAvailWidth * this.m_fPercentWidth / 100f;
			this.m_fMaxWidth -= this.m_arrBorders[1] + this.m_arrBorders[3] + (float)(this.m_nCols + 1) * this.m_fCellSpacing + this.m_arrMargins[1] + this.m_arrMargins[3];
			if (this.m_fMaxWidth < 0f)
			{
				this.m_fMaxWidth = 0f;
			}
			foreach (HtmlCell current in this.m_arrCells)
			{
				current.Width = 0f;
				current.m_fSpecifiedWidth = current.Width;
				float num = 0f;
				bool flag = false;
				if (current.m_nType == TagType.TAG_TD && current.m_pTreeItem.GetNumericProperty(CssProperties.WIDTH, ref num, ref flag, false))
				{
					if (flag)
					{
						current.Width = this.m_fMaxWidth * num / 100f;
						current.m_fSpecifiedWidth = current.Width;
					}
					else
					{
						current.Width = num;
						current.m_fSpecifiedWidth = current.Width;
					}
				}
			}
			this.ResetInvisibleObjects();
			this.ClearArrays();
			this.ResetWidthMatrix();
			this.ResolveWidthMatrix();
			this.ResetHeightMatrix();
			this.ResolveHeightMatrix();
			while (!this.PopulateTable())
			{
			}
		}
		private void CascadeFreezeColumns(int nCol, Aux2DArray arrAdj, ref List<float> arrVSpeeds)
		{
			for (int i = 0; i <= this.m_nCols; i++)
			{
				if (nCol < i && arrAdj[nCol + 1, i + 1] > 0f)
				{
					arrVSpeeds[i] = arrVSpeeds[nCol];
					this.CascadeFreezeColumns(i, arrAdj, ref arrVSpeeds);
				}
			}
		}
		private void Trim()
		{
			if (!this.m_bTrimmable)
			{
				return;
			}
			this.AdjustCellSizes();
			Aux2DArray aux2DArray = new Aux2DArray(this.m_nCols + 1, this.m_nCols + 1);
			foreach (HtmlCell current in this.m_arrCells)
			{
				if (current.m_fSpecifiedWidth > 0f)
				{
					aux2DArray[current.m_nColNum, current.m_nColNum + current.m_nColSpan] = 1f;
					aux2DArray[current.m_nColNum + current.m_nColSpan, current.m_nColNum] = 1f;
				}
			}
			List<float> list = new List<float>();
			List<float> list2 = new List<float>();
			List<float> list3 = new List<float>();
			list2.Add(0f);
			list3.Add(0f);
			list.Add(0f);
			float num = 0f;
			for (int i = 1; i <= this.m_nCols; i++)
			{
				list2.Add(this.ColWidth(i));
				list.Add(this.ColWidth(i));
				num += this.ColWidth(i);
				list3.Add(num);
			}
			for (int j = 0; j <= this.m_nCols; j++)
			{
				this.CascadeFreezeColumns(j, aux2DArray, ref list3);
			}
			while (true)
			{
				HtmlCell htmlCell = null;
				float num2 = 0f;
				float num3 = 999999.9f;
				foreach (HtmlCell current2 in this.m_arrCells)
				{
					float num4 = list3[current2.m_nColNum + current2.m_nColSpan - 1] - list3[current2.m_nColNum - 1];
					if (num4 > 0f)
					{
						num2 += num4;
						float num5 = this.m_fCellSpacing * (float)(current2.m_nColSpan - 1);
						for (int k = current2.m_nColNum; k < current2.m_nColNum + current2.m_nColSpan; k++)
						{
							num5 += list2[k];
						}
						num5 -= current2.m_fMaxHorizExtent + current2.m_arrBorders[3] + current2.m_arrBorders[1] + current2.m_arrPadding[3] + current2.m_arrPadding[1];
						float num6 = num5 / num4;
						if (num6 < num3)
						{
							htmlCell = current2;
							num3 = num6;
						}
					}
				}
				if (num2 > 0f && htmlCell != null)
				{
					for (int l = 1; l <= this.m_nCols; l++)
					{
						if (list3[l] > 0f)
						{
							List<float> list4;
							int index;
							(list4 = list2)[index = l] = list4[index] - num3 * (list3[l] - list3[l - 1]);
						}
					}
					list3[htmlCell.m_nColNum + htmlCell.m_nColSpan - 1] = 0f;
					this.CascadeFreezeColumns(htmlCell.m_nColNum + htmlCell.m_nColSpan - 1, aux2DArray, ref list3);
					using (List<HtmlCell>.Enumerator enumerator3 = this.m_arrCells.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							HtmlCell current3 = enumerator3.Current;
							if (current3.m_nColNum + current3.m_nColSpan - 1 >= htmlCell.m_nColNum && current3.m_nColNum + current3.m_nColSpan - 1 <= htmlCell.m_nColNum + htmlCell.m_nColSpan - 1)
							{
								list3[current3.m_nColNum + current3.m_nColSpan - 1] = 0f;
								this.CascadeFreezeColumns(current3.m_nColNum + current3.m_nColSpan - 1, aux2DArray, ref list3);
							}
						}
						continue;
					}
					break;
				}
				break;
			}
			for (int m = 1; m <= this.m_nCols; m++)
			{
				this.m_pVertLines[m].m_fCurPos = this.m_pVertLines[m - 1].m_fCurPos + list2[m];
				foreach (HtmlCell current4 in this.m_arrCells)
				{
					if (current4.m_nColNum + current4.m_nColSpan - 1 == m)
					{
						float num7 = 0f;
						float num8 = 0f;
						for (int n = current4.m_nColNum; n < current4.m_nColNum + current4.m_nColSpan; n++)
						{
							num7 += list[n];
							num8 += list2[n];
						}
						current4.Retrim(num7 - num8);
					}
				}
			}
			this.m_bGridLinesChanged = true;
			this.AdjustCellSizes();
			this.CalculateSize();
		}
		private float ColWidth(int nCol)
		{
			return this.m_pVertLines[nCol].m_fCurPos - this.m_pVertLines[nCol - 1].m_fCurPos;
		}
		private float RowHeight(int nRow)
		{
			return this.m_pHorizLines[nRow].m_fCurPos - this.m_pHorizLines[nRow - 1].m_fCurPos;
		}
		private void ResetInvisibleObjects()
		{
			foreach (HtmlCell current in this.m_arrCells)
			{
				current.ResetInvisibleObjects();
			}
		}
		private void ClearArrays()
		{
			foreach (HtmlCell current in this.m_arrCells)
			{
				current.ClearArrays();
			}
		}
		private void AddBullet()
		{
			if (this.m_nRepresentsTag != TagType.TAG_LI || this.m_nBulletType == CssProperties.V_NONE)
			{
				return;
			}
			if (this.m_arrCells.Count == 0)
			{
				return;
			}
			if (this.m_nParentContainerType != TagType.TAG_UL && this.m_nParentContainerType != TagType.TAG_OL)
			{
				this.m_nBulletPosition = CssProperties.V_INSIDE;
				if (this.m_nBulletNumber == 0)
				{
					this.m_nBulletNumber = 1;
				}
				if (this.m_nBulletType == CssProperties.V_0)
				{
					this.m_nBulletType = CssProperties.V_DISC;
				}
			}
			HtmlCell htmlCell = this.m_arrCells[0];
			if (this.m_bstrBulletPath != null && this.m_bstrBulletPath.Length > 0)
			{
				this.m_pBulletImage = htmlCell.AddImageBullet(this.m_bstrBulletPath);
				if (this.m_pBulletImage != null)
				{
					this.m_fBulletX = -this.m_pBulletImage.Width - (float)(HtmlManager.TAB_SHIFT / 5);
					if (this.m_nBulletPosition == CssProperties.V_INSIDE)
					{
						this.m_pBulletImage = null;
						return;
					}
					this.m_pBulletImage.Width = 0f;
					return;
				}
			}
			if (this.m_nBulletPosition == CssProperties.V_INSIDE)
			{
				htmlCell.AddWord(" ", 1);
			}
			char c = '\0';
			if (this.m_nBulletType == CssProperties.V_DISC)
			{
				c = '•';
			}
			else
			{
				if (this.m_nBulletType == CssProperties.V_CIRCLE)
				{
					c = '◦';
				}
				else
				{
					if (this.m_nBulletType == CssProperties.V_SQUARE)
					{
						c = '▪';
					}
				}
			}
			if (c != '\0')
			{
				this.m_pBullet = htmlCell.AddWord(c.ToString(), 1);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				int i = this.m_nBulletNumber;
				if (this.m_nBulletType == CssProperties.V_LOWER_ALPHA || this.m_nBulletType == CssProperties.V_UPPER_ALPHA)
				{
					if (this.m_nBulletNumber > 18278)
					{
						stringBuilder.Append("...");
					}
					else
					{
						int num = (i <= 26) ? 0 : ((i > 702) ? 2 : 1);
						stringBuilder.Append(new string(' ', num + 1));
						while (i > 0)
						{
							stringBuilder[num--] = (char)(97 + (i - 1) % 26);
							i = (i - 1) / 26;
						}
						stringBuilder.Append(".");
					}
				}
				else
				{
					if (i < 4000 && (this.m_nBulletType == CssProperties.V_LOWER_ROMAN || this.m_nBulletType == CssProperties.V_UPPER_ROMAN))
					{
						string[] array = new string[]
						{
							"m",
							"cm",
							"d",
							"cd",
							"c",
							"xc",
							"l",
							"xl",
							"x",
							"ix",
							"v",
							"iv",
							"i"
						};
						int[] array2 = new int[]
						{
							1000,
							900,
							500,
							400,
							100,
							90,
							50,
							40,
							10,
							9,
							5,
							4,
							1
						};
						int num2 = 0;
						while (i > 0)
						{
							while (i >= array2[num2])
							{
								stringBuilder.Append(array[num2]);
								i -= array2[num2];
							}
							num2++;
						}
						stringBuilder.Append(".");
					}
					else
					{
						stringBuilder.Append(string.Format("{0}.", this.m_nBulletNumber));
					}
				}
				if (this.m_nBulletType == CssProperties.V_UPPER_ALPHA || this.m_nBulletType == CssProperties.V_UPPER_ROMAN)
				{
					stringBuilder = new StringBuilder(stringBuilder.ToString().ToUpper());
				}
				this.m_pBullet = htmlCell.AddWord(stringBuilder.ToString(), stringBuilder.ToString().Length);
			}
			this.m_pBullet.CalculateSize();
			this.m_fBulletX = -this.m_pBullet.Width - (float)(HtmlManager.TAB_SHIFT / 5) - htmlCell.m_arrPadding[3] - htmlCell.m_arrBorders[3];
			if (this.m_nBulletPosition == CssProperties.V_INSIDE)
			{
				this.m_pBullet = null;
				return;
			}
			this.m_pBullet.Width = 0f;
		}
		public void AssignBulletCoordinates()
		{
			if (this.m_pBulletImage != null)
			{
				this.m_pBulletImage.X = this.m_fBulletX;
				this.m_pBulletImage.Y = 0f;
				this.m_pBulletImage.AssignCoordinates();
				return;
			}
			if (this.m_pBullet == null)
			{
				return;
			}
			this.m_pBullet.X = this.m_fBulletX;
			this.m_pBullet.Y = 0f;
			this.m_pBullet.AssignCoordinates();
		}
	}
}
