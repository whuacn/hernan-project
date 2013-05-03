using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	public class PdfCell
	{
		internal float m_fWidth;
		internal float m_fHeight;
		internal float m_fSpanHeight;
		internal float m_fSpanWidth;
		internal float m_fX;
		internal float m_fY;
		internal float m_fCellPadding;
		internal float m_fBorder;
		internal bool m_bTop;
		internal bool m_bLeft;
		internal bool m_bRight;
		internal bool m_bBottom;
		internal AuxRGB m_rgbTopColor = new AuxRGB();
		internal AuxRGB m_rgbLeftColor = new AuxRGB();
		internal AuxRGB m_rgbRightColor = new AuxRGB();
		internal AuxRGB m_rgbBottomColor = new AuxRGB();
		internal PdfManager m_pManager;
		internal PdfTable m_pTable;
		internal PdfRow m_pRow;
		internal AuxRGB m_rgbBorderColor = new AuxRGB();
		internal AuxRGB m_rgbBgColor = new AuxRGB();
		private object m_varBgColor;
		private object m_varBorderColor;
		internal int m_nColSpan;
		internal int m_nRowSpan;
		internal bool m_bSwallowed;
		internal int m_nIndex;
		internal List<AuxTextChunk> m_ptrTextChunks = new List<AuxTextChunk>();
		internal PdfGraphics m_ptrGraphics;
		public float Width
		{
			get
			{
				this.m_pTable.AdjustCellSizes();
				return this.m_fSpanWidth;
			}
			set
			{
				if (value <= 0f)
				{
					AuxException.Throw("Invalid Width parameter, must be a positive number.", PdfErrors._ERROR_OUTOFRANGE);
				}
				float num = value - this.m_fWidth;
				PdfRow pdfRow = this.m_pTable.FirstRow();
				if (pdfRow == null)
				{
					return;
				}
				for (PdfCell pdfCell = pdfRow.GetCellByIndex(this.m_nIndex); pdfCell != null; pdfCell = pdfCell.m_pRow.CellBelow(pdfCell))
				{
					pdfCell.m_fWidth += num;
					PdfCell pdfCell2 = pdfCell.m_pRow.FirstCell();
					while (pdfCell2 != null && this.m_nIndex > pdfCell2.m_nIndex)
					{
						pdfCell2 = pdfCell.m_pRow.NextCell(pdfCell2);
					}
					pdfCell2 = pdfCell;
					while ((pdfCell2 = pdfCell.m_pRow.NextCell(pdfCell2)) != null)
					{
						pdfCell2.m_fX += num;
					}
				}
				this.m_pTable.m_fWidth += num;
				this.m_pTable.m_bInvalidated = true;
			}
		}
		public int Index
		{
			get
			{
				return this.m_nIndex;
			}
		}
		public float Height
		{
			get
			{
				this.m_pTable.AdjustCellSizes();
				return this.m_fSpanHeight;
			}
			set
			{
				this.m_pRow.Height = value;
			}
		}
		public float X
		{
			get
			{
				return this.m_fX;
			}
		}
		public float Y
		{
			get
			{
				return this.m_fY;
			}
		}
		public int ColSpan
		{
			get
			{
				return this.m_nColSpan;
			}
			set
			{
				if (this.m_bSwallowed)
				{
					AuxException.Throw("ColSpan attribute cannot be changed on this cell.", PdfErrors._ERROR_OUTOFRANGE);
				}
				int nColSpan = this.m_nColSpan;
				if (value <= nColSpan)
				{
					AuxException.Throw("New ColSpan value must be greater than current value.", PdfErrors._ERROR_OUTOFRANGE);
				}
				this.m_nColSpan = value;
				this.m_pTable.m_bInvalidated = true;
			}
		}
		public int RowSpan
		{
			get
			{
				return this.m_nRowSpan;
			}
			set
			{
				if (this.m_bSwallowed)
				{
					AuxException.Throw("RowSpan attribute cannot be changed on this cell.", PdfErrors._ERROR_OUTOFRANGE);
				}
				int nRowSpan = this.m_nRowSpan;
				if (value <= nRowSpan)
				{
					AuxException.Throw("New RowSpan value must be greater than current value.", PdfErrors._ERROR_OUTOFRANGE);
				}
				this.m_nRowSpan = value;
				this.m_pTable.m_bInvalidated = true;
			}
		}
		public object BgColor
		{
			get
			{
				return this.m_varBgColor;
			}
			set
			{
				this.m_varBgColor = value;
				this.m_rgbBgColor = this.FromVarToColor(value);
			}
		}
		public object BorderColor
		{
			get
			{
				return this.m_varBorderColor;
			}
			set
			{
				this.m_varBorderColor = value;
				this.m_rgbBorderColor = this.FromVarToColor(value);
			}
		}
		public float Border
		{
			get
			{
				return this.m_fBorder;
			}
			set
			{
				this.m_fBorder = value;
			}
		}
		public float CellPadding
		{
			get
			{
				return this.m_fCellPadding;
			}
			set
			{
				this.m_fCellPadding = value;
			}
		}
		public PdfRow Row
		{
			get
			{
				return this.m_pRow;
			}
		}
		public PdfCanvas Canvas
		{
			get
			{
				if (this.m_ptrGraphics == null)
				{
					this.m_pTable.AdjustCellSizes();
					PdfParam pdfParam = new PdfParam();
					pdfParam["Left"] = 0f;
					pdfParam["Bottom"] = 0f;
					pdfParam["Right"] = this.m_fSpanWidth;
					pdfParam["Top"] = this.m_fSpanHeight;
					this.m_ptrGraphics = this.m_pTable.m_pDoc.CreateGraphics(pdfParam);
				}
				return this.m_ptrGraphics.Canvas;
			}
		}
		internal PdfCell()
		{
			this.m_fWidth = 0f;
			this.m_nIndex = 0;
			this.m_fHeight = 0f;
			this.m_fSpanHeight = 0f;
			this.m_fSpanWidth = 0f;
			this.m_fX = 0f;
			this.m_fY = 0f;
			this.m_bSwallowed = false;
			this.m_nColSpan = 1;
			this.m_nRowSpan = 1;
			this.m_fCellPadding = 0f;
			this.m_fBorder = 1f;
			this.m_bTop = (this.m_bLeft = (this.m_bRight = (this.m_bBottom = true)));
		}
		internal void Draw(PdfCanvas pCanvas, float X, float Y)
		{
			if (this.m_bSwallowed)
			{
				return;
			}
			if (this.m_rgbBgColor.m_bIsSet)
			{
				pCanvas.SetFillColor(this.m_rgbBgColor.r, this.m_rgbBgColor.g, this.m_rgbBgColor.b);
				pCanvas.FillRect(X + this.m_fX + this.m_fBorder / 2f, Y + this.m_fY - this.m_fSpanHeight + this.m_fBorder / 2f, this.m_fSpanWidth - this.m_fBorder, this.m_fSpanHeight - this.m_fBorder);
			}
			if (this.m_ptrGraphics != null)
			{
				PdfParam pdfParam = new PdfParam();
				pdfParam["x"] = X + this.m_fX;
				pdfParam["y"] = Y + this.m_fY - this.m_fSpanHeight;
				pCanvas.DrawGraphics(this.m_ptrGraphics, pdfParam);
			}
			foreach (AuxTextChunk current in this.m_ptrTextChunks)
			{
				PdfParam pParam = current.m_pParam;
				float num = pParam.Number("IndentX");
				float num2 = pParam.Number("IndentY");
				pParam.AssignValue("X", X + this.m_fX + this.m_fCellPadding + this.m_fBorder + num);
				pParam.AssignValue("Y", Y + this.m_fY - this.m_fCellPadding - this.m_fBorder - num2);
				pParam.AssignValue("Width", this.m_fSpanWidth - 2f * this.m_fCellPadding - num - this.m_fBorder * 2f);
				pParam.AssignValue("Height", this.m_fSpanHeight - 2f * this.m_fCellPadding - num2 - this.m_fBorder * 2f);
				if ((this.m_rgbBgColor.m_bIsSet || this.m_pTable.m_rgbBgColor.m_bIsSet) && !pParam.IsSet("Color"))
				{
					pParam.AssignValue("Color", 0f);
				}
				pCanvas.DrawText(current.m_bstrText, current.m_pParam, current.m_pFont);
			}
			if (!this.m_bTop || !this.m_bLeft || !this.m_bRight || !this.m_bBottom || this.m_rgbTopColor.m_bIsSet || this.m_rgbLeftColor.m_bIsSet || this.m_rgbBottomColor.m_bIsSet || this.m_rgbRightColor.m_bIsSet)
			{
				if (!this.m_rgbTopColor.m_bIsSet)
				{
					this.m_rgbTopColor.Set(ref this.m_rgbBorderColor);
				}
				if (!this.m_rgbRightColor.m_bIsSet)
				{
					this.m_rgbRightColor.Set(ref this.m_rgbBorderColor);
				}
				if (!this.m_rgbLeftColor.m_bIsSet)
				{
					this.m_rgbLeftColor.Set(ref this.m_rgbBorderColor);
				}
				if (!this.m_rgbBottomColor.m_bIsSet)
				{
					this.m_rgbBottomColor.Set(ref this.m_rgbBorderColor);
				}
				if (this.m_fBorder > 0f)
				{
					pCanvas.LineWidth = this.m_fBorder;
				}
				pCanvas.LineCap = 2;
				if (this.m_bTop)
				{
					pCanvas.SetColor(this.m_rgbTopColor.r, this.m_rgbTopColor.g, this.m_rgbTopColor.b);
					pCanvas.DrawLine(X + this.m_fX + this.m_fBorder / 2f, Y + this.m_fY - this.m_fSpanHeight - this.m_fBorder / 2f + this.m_fSpanHeight, X + this.m_fX - this.m_fBorder / 2f + this.m_fSpanWidth, Y + this.m_fY - this.m_fSpanHeight - this.m_fBorder / 2f + this.m_fSpanHeight);
				}
				if (this.m_bBottom)
				{
					pCanvas.SetColor(this.m_rgbBottomColor.r, this.m_rgbBottomColor.g, this.m_rgbBottomColor.b);
					pCanvas.DrawLine(X + this.m_fX + this.m_fBorder / 2f, Y + this.m_fY - this.m_fSpanHeight + this.m_fBorder / 2f, X + this.m_fX - this.m_fBorder / 2f + this.m_fSpanWidth, Y + this.m_fY - this.m_fSpanHeight + this.m_fBorder / 2f);
				}
				if (this.m_bLeft)
				{
					pCanvas.SetColor(this.m_rgbLeftColor.r, this.m_rgbLeftColor.g, this.m_rgbLeftColor.b);
					pCanvas.DrawLine(X + this.m_fX + this.m_fBorder / 2f, Y + this.m_fY - this.m_fSpanHeight + this.m_fBorder / 2f, X + this.m_fX + this.m_fBorder / 2f, Y + this.m_fY - this.m_fSpanHeight - this.m_fBorder / 2f + this.m_fSpanHeight);
				}
				if (this.m_bRight)
				{
					pCanvas.SetColor(this.m_rgbRightColor.r, this.m_rgbRightColor.g, this.m_rgbRightColor.b);
					pCanvas.DrawLine(X + this.m_fX - this.m_fBorder / 2f + this.m_fSpanWidth, Y + this.m_fY - this.m_fSpanHeight + this.m_fBorder / 2f, X + this.m_fX - this.m_fBorder / 2f + this.m_fSpanWidth, Y + this.m_fY - this.m_fSpanHeight - this.m_fBorder / 2f + this.m_fSpanHeight);
					return;
				}
			}
			else
			{
				if (this.m_rgbBorderColor.m_bIsSet)
				{
					pCanvas.SetColor(this.m_rgbBorderColor.r, this.m_rgbBorderColor.g, this.m_rgbBorderColor.b);
				}
				if (this.m_fBorder > 0f)
				{
					pCanvas.LineWidth = this.m_fBorder;
					pCanvas.DrawRect(X + this.m_fX + this.m_fBorder / 2f, Y + this.m_fY - this.m_fSpanHeight + this.m_fBorder / 2f, this.m_fSpanWidth - this.m_fBorder, this.m_fSpanHeight - this.m_fBorder);
				}
			}
		}
		internal bool CannotBeSwallowed()
		{
			return this.m_nRowSpan > 1 || this.m_nColSpan > 1 || this.m_bSwallowed;
		}
		public int AddText(string Text, object Param)
		{
			return this.AddText(Text, Param, null);
		}
		public int AddText(string Text, object Param, PdfFont Font)
		{
			PdfFont pdfFont = Font;
			if (pdfFont == null)
			{
				pdfFont = this.m_pTable.m_ptrFont;
			}
			if (pdfFont == null)
			{
				AuxException.Throw("Font argument must be specified if Table.Font is not specified.", PdfErrors._ERROR_INVALIDFONTARG);
			}
			PdfParam pParam = this.m_pManager.VariantToParam(Param);
			this.m_pTable.AdjustCellSizes();
			AuxTextChunk auxTextChunk = new AuxTextChunk();
			bool flag = false;
			int num = 0;
			PdfParam pdfParam = new PdfParam();
			pdfParam.Copy(pParam);
			float num2 = pdfParam.Number("IndentX");
			float num3 = pdfParam.Number("IndentY");
			if (pdfParam.IsSet("Expand"))
			{
				flag = pdfParam.Bool("Expand");
			}
			if (pdfParam.IsSet("VAlignment"))
			{
				num = pdfParam.Long("VAlignment");
				if (num != 0 && num != 1 && num != 2)
				{
					AuxException.Throw("Valid VAlignment values are 0 (top, default), 1 (bottom) or 2 (middle).", PdfErrors._ERROR_INVALIDFONTARG);
				}
			}
			pdfParam.AssignValue("X", this.m_fCellPadding + num2 + this.m_fBorder);
			pdfParam.AssignValue("Y", -this.m_fCellPadding - num3 - this.m_fBorder);
			pdfParam.AssignValue("Width", this.m_fSpanWidth - 2f * this.m_fCellPadding - num2 - 2f * this.m_fBorder);
			pdfParam.AssignValue("Height", flag ? -1f : (this.m_fSpanHeight - 2f * this.m_fCellPadding - num3 - 2f * this.m_fBorder));
			auxTextChunk.m_pParam = pdfParam;
			auxTextChunk.m_bstrText = Text;
			auxTextChunk.m_pFont = pdfFont;
			this.m_ptrTextChunks.Add(auxTextChunk);
			PdfCanvas pdfCanvas = new PdfCanvas();
			pdfCanvas.m_pManager = this.m_pManager;
			pdfCanvas.m_pDoc = this.m_pTable.m_pDoc;
			int result = pdfCanvas.DrawText(Text, pdfParam, pdfFont);
			if (flag && pdfCanvas.m_fVertialExtent + this.m_fCellPadding * 2f + this.m_fBorder * 2f > this.m_fSpanHeight)
			{
				this.Height = pdfCanvas.m_fVertialExtent + num3 + this.m_fCellPadding * 2f + this.m_fBorder * 2f;
			}
			if (num == 1 || num == 2)
			{
				auxTextChunk.m_nVAlignment = num;
				auxTextChunk.m_fVerticalExtent = pdfCanvas.m_fVertialExtent;
			}
			return result;
		}
		public void ClearText()
		{
			this.ClearChunks();
		}
		internal void ClearChunks()
		{
			this.m_ptrTextChunks.Clear();
		}
		internal AuxRGB FromVarToColor(object color)
		{
			AuxRGB auxRGB = new AuxRGB();
			string fullName = color.GetType().FullName;
			if (fullName == "System.UInt16")
			{
				auxRGB.Set((ushort)color);
				return auxRGB;
			}
			if (fullName == "System.Int16")
			{
				short num = (short)color;
				auxRGB.Set((ushort)num);
				return auxRGB;
			}
			if (fullName == "System.Int32")
			{
				int rgb = (int)color;
				auxRGB.Set((uint)rgb);
				return auxRGB;
			}
			if (fullName == "System.UInt32")
			{
				auxRGB.Set((uint)color);
				return auxRGB;
			}
			if (fullName == "System.String")
			{
				float num2 = 0f;
				if (PdfParam.NameToValue((string)color, ref num2))
				{
					auxRGB.Set((uint)num2);
					return auxRGB;
				}
				AuxException.Throw("Color not found.", PdfErrors._ERROR_OUTOFRANGE);
			}
			AuxException.Throw("Invalid color value.", PdfErrors._ERROR_OUTOFRANGE);
			return null;
		}
		internal void MarkAsSwallowed(int nHowMany, bool bHorizontal)
		{
			PdfCell pdfCell = this;
			if (bHorizontal)
			{
				while (nHowMany > 0)
				{
					if ((pdfCell = this.m_pRow.NextCell(pdfCell)) == null)
					{
						return;
					}
					pdfCell.m_bSwallowed = true;
					nHowMany--;
				}
			}
			else
			{
				while (nHowMany > 0 && (pdfCell = this.m_pRow.CellBelow(pdfCell)) != null)
				{
					pdfCell.m_bSwallowed = true;
					nHowMany--;
				}
			}
		}
		public void SetBorderParams(object Param)
		{
			PdfParam pdfParam = this.m_pManager.VariantToParam(Param);
			if (pdfParam.IsSet("Top"))
			{
				this.m_bTop = pdfParam.Bool("Top");
			}
			if (pdfParam.IsSet("Left"))
			{
				this.m_bLeft = pdfParam.Bool("Left");
			}
			if (pdfParam.IsSet("Bottom"))
			{
				this.m_bBottom = pdfParam.Bool("Bottom");
			}
			if (pdfParam.IsSet("Right"))
			{
				this.m_bRight = pdfParam.Bool("Right");
			}
			if (pdfParam.IsSet("TopColor"))
			{
				pdfParam.Color("TopColor", ref this.m_rgbTopColor);
			}
			if (pdfParam.IsSet("LeftColor"))
			{
				pdfParam.Color("LeftColor", ref this.m_rgbLeftColor);
			}
			if (pdfParam.IsSet("BottomColor"))
			{
				pdfParam.Color("BottomColor", ref this.m_rgbBottomColor);
			}
			if (pdfParam.IsSet("RightColor"))
			{
				pdfParam.Color("RightColor", ref this.m_rgbRightColor);
			}
		}
	}
}
