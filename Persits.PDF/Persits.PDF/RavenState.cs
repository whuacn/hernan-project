using System;
namespace Persits.PDF
{
	internal class RavenState
	{
		internal const int ravenLineCapButt = 0;
		internal const int ravenLineCapRound = 1;
		internal const int ravenLineCapProjecting = 2;
		internal const int ravenLineJoinMiter = 0;
		internal const int ravenLineJoinRound = 1;
		internal const int ravenLineJoinBevel = 2;
		internal static int[] ravenColorModeNComps = new int[]
		{
			1,
			1,
			3,
			3
		};
		internal double[] matrix = new double[6];
		internal RavenPattern strokePattern;
		internal RavenPattern fillPattern;
		internal RavenScreen screen;
		internal RavenBlendFunc blendFunc;
		internal double strokeAlpha;
		internal double fillAlpha;
		internal double lineWidth;
		internal int lineCap;
		internal int lineJoin;
		internal double miterLimit;
		internal double flatness;
		internal double[] lineDash;
		internal int lineDashLength;
		internal double lineDashPhase;
		internal bool strokeAdjust;
		internal RavenClip clip;
		internal RavenBitmap softMask;
		internal bool deleteSoftMask;
		internal bool inNonIsolatedGroup;
		internal byte[] rgbTransferR = new byte[256];
		internal byte[] rgbTransferG = new byte[256];
		internal byte[] rgbTransferB = new byte[256];
		internal byte[] grayTransfer = new byte[256];
		internal byte[] cmykTransferC = new byte[256];
		internal byte[] cmykTransferM = new byte[256];
		internal byte[] cmykTransferY = new byte[256];
		internal byte[] cmykTransferK = new byte[256];
		internal uint overprintMask;
		internal RavenState next;
		internal RavenState(int width, int height, bool vectorAntialias, RavenScreenParams screenParams)
		{
			RavenColor colorA = new RavenColor();
			this.matrix[0] = 1.0;
			this.matrix[1] = 0.0;
			this.matrix[2] = 0.0;
			this.matrix[3] = 1.0;
			this.matrix[4] = 0.0;
			this.matrix[5] = 0.0;
			this.strokePattern = new RavenSolidColor(colorA);
			this.fillPattern = new RavenSolidColor(colorA);
			this.screen = new RavenScreen(screenParams);
			this.blendFunc = null;
			this.strokeAlpha = 1.0;
			this.fillAlpha = 1.0;
			this.lineWidth = 0.0;
			this.lineCap = 0;
			this.lineJoin = 0;
			this.miterLimit = 10.0;
			this.flatness = 1.0;
			this.lineDash = null;
			this.lineDashLength = 0;
			this.lineDashPhase = 0.0;
			this.strokeAdjust = false;
			this.clip = new RavenClip(0.0, 0.0, (double)width, (double)height, vectorAntialias);
			this.softMask = null;
			this.deleteSoftMask = false;
			this.inNonIsolatedGroup = false;
			for (int i = 0; i < 256; i++)
			{
				this.rgbTransferR[i] = (byte)i;
				this.rgbTransferG[i] = (byte)i;
				this.rgbTransferB[i] = (byte)i;
				this.grayTransfer[i] = (byte)i;
				this.cmykTransferC[i] = (byte)i;
				this.cmykTransferM[i] = (byte)i;
				this.cmykTransferY[i] = (byte)i;
				this.cmykTransferK[i] = (byte)i;
			}
			this.overprintMask = 4294967295u;
			this.next = null;
		}
		internal RavenState(int width, int height, bool vectorAntialias, RavenScreen screenA)
		{
			RavenColor colorA = new RavenColor();
			this.matrix[0] = 1.0;
			this.matrix[1] = 0.0;
			this.matrix[2] = 0.0;
			this.matrix[3] = 1.0;
			this.matrix[4] = 0.0;
			this.matrix[5] = 0.0;
			this.strokePattern = new RavenSolidColor(colorA);
			this.fillPattern = new RavenSolidColor(colorA);
			this.screen = screenA.copy();
			this.blendFunc = null;
			this.strokeAlpha = 1.0;
			this.fillAlpha = 1.0;
			this.lineWidth = 0.0;
			this.lineCap = 0;
			this.lineJoin = 0;
			this.miterLimit = 10.0;
			this.flatness = 1.0;
			this.lineDash = null;
			this.lineDashLength = 0;
			this.lineDashPhase = 0.0;
			this.strokeAdjust = false;
			this.clip = new RavenClip(0.0, 0.0, (double)width, (double)height, vectorAntialias);
			this.softMask = null;
			this.deleteSoftMask = false;
			this.inNonIsolatedGroup = false;
			for (int i = 0; i < 256; i++)
			{
				this.rgbTransferR[i] = (byte)i;
				this.rgbTransferG[i] = (byte)i;
				this.rgbTransferB[i] = (byte)i;
				this.grayTransfer[i] = (byte)i;
				this.cmykTransferC[i] = (byte)i;
				this.cmykTransferM[i] = (byte)i;
				this.cmykTransferY[i] = (byte)i;
				this.cmykTransferK[i] = (byte)i;
			}
			this.overprintMask = 4294967295u;
			this.next = null;
		}
		private RavenState(RavenState state)
		{
			Array.Copy(state.matrix, this.matrix, 6);
			this.strokePattern = state.strokePattern.copy();
			this.fillPattern = state.fillPattern.copy();
			this.screen = state.screen.copy();
			this.blendFunc = state.blendFunc;
			this.strokeAlpha = state.strokeAlpha;
			this.fillAlpha = state.fillAlpha;
			this.lineWidth = state.lineWidth;
			this.lineCap = state.lineCap;
			this.lineJoin = state.lineJoin;
			this.miterLimit = state.miterLimit;
			this.flatness = state.flatness;
			if (state.lineDash != null)
			{
				this.lineDashLength = state.lineDashLength;
				this.lineDash = new double[this.lineDashLength];
				Array.Copy(state.lineDash, this.lineDash, this.lineDashLength);
			}
			else
			{
				this.lineDash = null;
				this.lineDashLength = 0;
			}
			this.lineDashPhase = state.lineDashPhase;
			this.strokeAdjust = state.strokeAdjust;
			this.clip = state.clip.copy();
			this.softMask = state.softMask;
			this.deleteSoftMask = false;
			this.inNonIsolatedGroup = state.inNonIsolatedGroup;
			Array.Copy(state.rgbTransferR, this.rgbTransferR, 256);
			Array.Copy(state.rgbTransferG, this.rgbTransferG, 256);
			Array.Copy(state.rgbTransferB, this.rgbTransferB, 256);
			Array.Copy(state.grayTransfer, this.grayTransfer, 256);
			Array.Copy(state.cmykTransferC, this.cmykTransferC, 256);
			Array.Copy(state.cmykTransferM, this.cmykTransferM, 256);
			Array.Copy(state.cmykTransferY, this.cmykTransferY, 256);
			Array.Copy(state.cmykTransferK, this.cmykTransferK, 256);
			this.overprintMask = state.overprintMask;
			this.next = null;
		}
		internal RavenState copy()
		{
			return new RavenState(this);
		}
		internal void setStrokePattern(RavenPattern strokePatternA)
		{
			this.strokePattern = strokePatternA;
		}
		internal void setFillPattern(RavenPattern fillPatternA)
		{
			this.fillPattern = fillPatternA;
		}
		internal void setScreen(RavenScreen screenA)
		{
			this.screen = screenA;
		}
		internal void setLineDash(double[] lineDashA, int lineDashLengthA, double lineDashPhaseA)
		{
			this.lineDashLength = lineDashLengthA;
			if (this.lineDashLength > 0)
			{
				this.lineDash = new double[this.lineDashLength];
				Array.Copy(lineDashA, this.lineDash, this.lineDashLength);
			}
			else
			{
				this.lineDash = null;
			}
			this.lineDashPhase = lineDashPhaseA;
		}
		internal void setSoftMask(RavenBitmap softMaskA)
		{
			this.softMask = softMaskA;
			this.deleteSoftMask = true;
		}
	}
}
