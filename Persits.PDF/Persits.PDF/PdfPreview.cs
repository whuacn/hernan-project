using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
namespace Persits.PDF
{
	public class PdfPreview
	{
		internal class NameToUnicode
		{
			public uint u;
			public string name;
			public NameToUnicode(uint uu, string nn)
			{
				this.u = uu;
				this.name = nn;
			}
		}
		internal delegate void PreviewOperatorFunction(PdfObject[] args, int numArgs);
		internal class PreviewOperator
		{
			internal string name;
			internal int numArgs;
			internal enumType[] tchk = new enumType[8];
			internal PdfPreview.PreviewOperatorFunction func;
			internal PreviewOperator(string nm, int na, enumType[] arr, PdfPreview.PreviewOperatorFunction f)
			{
				this.name = nm;
				this.numArgs = na;
				this.func = f;
				for (int i = 0; i < arr.Length; i++)
				{
					this.tchk[i] = arr[i];
				}
			}
		}
		internal const int maxArgs = 8;
		private const double functionColorDelta = 0.00390625;
		private const int axialMaxSplits = 256;
		private const int functionMaxDepth = 6;
		private const int radialMaxSplits = 256;
		private NameToCharCode macRomanReverseMap;
		private NameToCharCode m_nameToUnicode;
		private CMapCache m_pCMapCache;
		private BuiltinFont[] builtinFonts = new BuiltinFont[14];
		private BuiltinFont[] builtinFontSubst = new BuiltinFont[12];
		private static BuiltinFontWidth[] courierWidthsTab = new BuiltinFontWidth[]
		{
			new BuiltinFontWidth("Ntilde", 600, null),
			new BuiltinFontWidth("rcaron", 600, null),
			new BuiltinFontWidth("kcommaaccent", 600, null),
			new BuiltinFontWidth("Ncommaaccent", 600, null),
			new BuiltinFontWidth("Zacute", 600, null),
			new BuiltinFontWidth("comma", 600, null),
			new BuiltinFontWidth("cedilla", 600, null),
			new BuiltinFontWidth("plusminus", 600, null),
			new BuiltinFontWidth("circumflex", 600, null),
			new BuiltinFontWidth("dotaccent", 600, null),
			new BuiltinFontWidth("edotaccent", 600, null),
			new BuiltinFontWidth("asciitilde", 600, null),
			new BuiltinFontWidth("colon", 600, null),
			new BuiltinFontWidth("onehalf", 600, null),
			new BuiltinFontWidth("dollar", 600, null),
			new BuiltinFontWidth("Lcaron", 600, null),
			new BuiltinFontWidth("ntilde", 600, null),
			new BuiltinFontWidth("Aogonek", 600, null),
			new BuiltinFontWidth("ncommaaccent", 600, null),
			new BuiltinFontWidth("minus", 600, null),
			new BuiltinFontWidth("Iogonek", 600, null),
			new BuiltinFontWidth("zacute", 600, null),
			new BuiltinFontWidth("yen", 600, null),
			new BuiltinFontWidth("space", 600, null),
			new BuiltinFontWidth("Omacron", 600, null),
			new BuiltinFontWidth("questiondown", 600, null),
			new BuiltinFontWidth("emdash", 600, null),
			new BuiltinFontWidth("Agrave", 600, null),
			new BuiltinFontWidth("three", 600, null),
			new BuiltinFontWidth("numbersign", 600, null),
			new BuiltinFontWidth("lcaron", 600, null),
			new BuiltinFontWidth("A", 600, null),
			new BuiltinFontWidth("B", 600, null),
			new BuiltinFontWidth("C", 600, null),
			new BuiltinFontWidth("aogonek", 600, null),
			new BuiltinFontWidth("D", 600, null),
			new BuiltinFontWidth("E", 600, null),
			new BuiltinFontWidth("onequarter", 600, null),
			new BuiltinFontWidth("F", 600, null),
			new BuiltinFontWidth("G", 600, null),
			new BuiltinFontWidth("H", 600, null),
			new BuiltinFontWidth("I", 600, null),
			new BuiltinFontWidth("J", 600, null),
			new BuiltinFontWidth("K", 600, null),
			new BuiltinFontWidth("iogonek", 600, null),
			new BuiltinFontWidth("L", 600, null),
			new BuiltinFontWidth("backslash", 600, null),
			new BuiltinFontWidth("periodcentered", 600, null),
			new BuiltinFontWidth("M", 600, null),
			new BuiltinFontWidth("N", 600, null),
			new BuiltinFontWidth("omacron", 600, null),
			new BuiltinFontWidth("Tcommaaccent", 600, null),
			new BuiltinFontWidth("O", 600, null),
			new BuiltinFontWidth("P", 600, null),
			new BuiltinFontWidth("Q", 600, null),
			new BuiltinFontWidth("Uhungarumlaut", 600, null),
			new BuiltinFontWidth("R", 600, null),
			new BuiltinFontWidth("Aacute", 600, null),
			new BuiltinFontWidth("caron", 600, null),
			new BuiltinFontWidth("S", 600, null),
			new BuiltinFontWidth("T", 600, null),
			new BuiltinFontWidth("U", 600, null),
			new BuiltinFontWidth("agrave", 600, null),
			new BuiltinFontWidth("V", 600, null),
			new BuiltinFontWidth("W", 600, null),
			new BuiltinFontWidth("equal", 600, null),
			new BuiltinFontWidth("question", 600, null),
			new BuiltinFontWidth("X", 600, null),
			new BuiltinFontWidth("Y", 600, null),
			new BuiltinFontWidth("Z", 600, null),
			new BuiltinFontWidth("four", 600, null),
			new BuiltinFontWidth("a", 600, null),
			new BuiltinFontWidth("Gcommaaccent", 600, null),
			new BuiltinFontWidth("b", 600, null),
			new BuiltinFontWidth("c", 600, null),
			new BuiltinFontWidth("d", 600, null),
			new BuiltinFontWidth("e", 600, null),
			new BuiltinFontWidth("f", 600, null),
			new BuiltinFontWidth("g", 600, null),
			new BuiltinFontWidth("bullet", 600, null),
			new BuiltinFontWidth("h", 600, null),
			new BuiltinFontWidth("i", 600, null),
			new BuiltinFontWidth("Oslash", 600, null),
			new BuiltinFontWidth("dagger", 600, null),
			new BuiltinFontWidth("j", 600, null),
			new BuiltinFontWidth("k", 600, null),
			new BuiltinFontWidth("l", 600, null),
			new BuiltinFontWidth("m", 600, null),
			new BuiltinFontWidth("n", 600, null),
			new BuiltinFontWidth("tcommaaccent", 600, null),
			new BuiltinFontWidth("o", 600, null),
			new BuiltinFontWidth("ordfeminine", 600, null),
			new BuiltinFontWidth("ring", 600, null),
			new BuiltinFontWidth("p", 600, null),
			new BuiltinFontWidth("q", 600, null),
			new BuiltinFontWidth("uhungarumlaut", 600, null),
			new BuiltinFontWidth("r", 600, null),
			new BuiltinFontWidth("twosuperior", 600, null),
			new BuiltinFontWidth("aacute", 600, null),
			new BuiltinFontWidth("s", 600, null),
			new BuiltinFontWidth("OE", 600, null),
			new BuiltinFontWidth("t", 600, null),
			new BuiltinFontWidth("divide", 600, null),
			new BuiltinFontWidth("u", 600, null),
			new BuiltinFontWidth("Ccaron", 600, null),
			new BuiltinFontWidth("v", 600, null),
			new BuiltinFontWidth("w", 600, null),
			new BuiltinFontWidth("x", 600, null),
			new BuiltinFontWidth("y", 600, null),
			new BuiltinFontWidth("z", 600, null),
			new BuiltinFontWidth("Gbreve", 600, null),
			new BuiltinFontWidth("commaaccent", 600, null),
			new BuiltinFontWidth("hungarumlaut", 600, null),
			new BuiltinFontWidth("Idotaccent", 600, null),
			new BuiltinFontWidth("Nacute", 600, null),
			new BuiltinFontWidth("quotedbl", 600, null),
			new BuiltinFontWidth("gcommaaccent", 600, null),
			new BuiltinFontWidth("mu", 600, null),
			new BuiltinFontWidth("greaterequal", 600, null),
			new BuiltinFontWidth("Scaron", 600, null),
			new BuiltinFontWidth("Lslash", 600, null),
			new BuiltinFontWidth("semicolon", 600, null),
			new BuiltinFontWidth("oslash", 600, null),
			new BuiltinFontWidth("lessequal", 600, null),
			new BuiltinFontWidth("lozenge", 600, null),
			new BuiltinFontWidth("parenright", 600, null),
			new BuiltinFontWidth("ccaron", 600, null),
			new BuiltinFontWidth("Ecircumflex", 600, null),
			new BuiltinFontWidth("gbreve", 600, null),
			new BuiltinFontWidth("trademark", 600, null),
			new BuiltinFontWidth("daggerdbl", 600, null),
			new BuiltinFontWidth("nacute", 600, null),
			new BuiltinFontWidth("macron", 600, null),
			new BuiltinFontWidth("Otilde", 600, null),
			new BuiltinFontWidth("Emacron", 600, null),
			new BuiltinFontWidth("ellipsis", 600, null),
			new BuiltinFontWidth("scaron", 600, null),
			new BuiltinFontWidth("AE", 600, null),
			new BuiltinFontWidth("Ucircumflex", 600, null),
			new BuiltinFontWidth("lslash", 600, null),
			new BuiltinFontWidth("quotedblleft", 600, null),
			new BuiltinFontWidth("hyphen", 600, null),
			new BuiltinFontWidth("guilsinglright", 600, null),
			new BuiltinFontWidth("quotesingle", 600, null),
			new BuiltinFontWidth("eight", 600, null),
			new BuiltinFontWidth("exclamdown", 600, null),
			new BuiltinFontWidth("endash", 600, null),
			new BuiltinFontWidth("oe", 600, null),
			new BuiltinFontWidth("Abreve", 600, null),
			new BuiltinFontWidth("Umacron", 600, null),
			new BuiltinFontWidth("ecircumflex", 600, null),
			new BuiltinFontWidth("Adieresis", 600, null),
			new BuiltinFontWidth("copyright", 600, null),
			new BuiltinFontWidth("Egrave", 600, null),
			new BuiltinFontWidth("slash", 600, null),
			new BuiltinFontWidth("Edieresis", 600, null),
			new BuiltinFontWidth("otilde", 600, null),
			new BuiltinFontWidth("Idieresis", 600, null),
			new BuiltinFontWidth("parenleft", 600, null),
			new BuiltinFontWidth("one", 600, null),
			new BuiltinFontWidth("emacron", 600, null),
			new BuiltinFontWidth("Odieresis", 600, null),
			new BuiltinFontWidth("ucircumflex", 600, null),
			new BuiltinFontWidth("bracketleft", 600, null),
			new BuiltinFontWidth("Ugrave", 600, null),
			new BuiltinFontWidth("quoteright", 600, null),
			new BuiltinFontWidth("Udieresis", 600, null),
			new BuiltinFontWidth("perthousand", 600, null),
			new BuiltinFontWidth("Ydieresis", 600, null),
			new BuiltinFontWidth("umacron", 600, null),
			new BuiltinFontWidth("abreve", 600, null),
			new BuiltinFontWidth("Eacute", 600, null),
			new BuiltinFontWidth("adieresis", 600, null),
			new BuiltinFontWidth("egrave", 600, null),
			new BuiltinFontWidth("edieresis", 600, null),
			new BuiltinFontWidth("idieresis", 600, null),
			new BuiltinFontWidth("Eth", 600, null),
			new BuiltinFontWidth("ae", 600, null),
			new BuiltinFontWidth("asterisk", 600, null),
			new BuiltinFontWidth("odieresis", 600, null),
			new BuiltinFontWidth("Uacute", 600, null),
			new BuiltinFontWidth("ugrave", 600, null),
			new BuiltinFontWidth("five", 600, null),
			new BuiltinFontWidth("nine", 600, null),
			new BuiltinFontWidth("udieresis", 600, null),
			new BuiltinFontWidth("Zcaron", 600, null),
			new BuiltinFontWidth("Scommaaccent", 600, null),
			new BuiltinFontWidth("threequarters", 600, null),
			new BuiltinFontWidth("guillemotright", 600, null),
			new BuiltinFontWidth("Ccedilla", 600, null),
			new BuiltinFontWidth("ydieresis", 600, null),
			new BuiltinFontWidth("tilde", 600, null),
			new BuiltinFontWidth("at", 600, null),
			new BuiltinFontWidth("eacute", 600, null),
			new BuiltinFontWidth("underscore", 600, null),
			new BuiltinFontWidth("Euro", 600, null),
			new BuiltinFontWidth("Dcroat", 600, null),
			new BuiltinFontWidth("zero", 600, null),
			new BuiltinFontWidth("multiply", 600, null),
			new BuiltinFontWidth("eth", 600, null),
			new BuiltinFontWidth("Scedilla", 600, null),
			new BuiltinFontWidth("Racute", 600, null),
			new BuiltinFontWidth("Ograve", 600, null),
			new BuiltinFontWidth("partialdiff", 600, null),
			new BuiltinFontWidth("uacute", 600, null),
			new BuiltinFontWidth("braceleft", 600, null),
			new BuiltinFontWidth("Thorn", 600, null),
			new BuiltinFontWidth("zcaron", 600, null),
			new BuiltinFontWidth("scommaaccent", 600, null),
			new BuiltinFontWidth("ccedilla", 600, null),
			new BuiltinFontWidth("Dcaron", 600, null),
			new BuiltinFontWidth("dcroat", 600, null),
			new BuiltinFontWidth("scedilla", 600, null),
			new BuiltinFontWidth("Oacute", 600, null),
			new BuiltinFontWidth("Ocircumflex", 600, null),
			new BuiltinFontWidth("ogonek", 600, null),
			new BuiltinFontWidth("ograve", 600, null),
			new BuiltinFontWidth("racute", 600, null),
			new BuiltinFontWidth("Tcaron", 600, null),
			new BuiltinFontWidth("Eogonek", 600, null),
			new BuiltinFontWidth("thorn", 600, null),
			new BuiltinFontWidth("degree", 600, null),
			new BuiltinFontWidth("registered", 600, null),
			new BuiltinFontWidth("radical", 600, null),
			new BuiltinFontWidth("Aring", 600, null),
			new BuiltinFontWidth("percent", 600, null),
			new BuiltinFontWidth("six", 600, null),
			new BuiltinFontWidth("paragraph", 600, null),
			new BuiltinFontWidth("dcaron", 600, null),
			new BuiltinFontWidth("Uogonek", 600, null),
			new BuiltinFontWidth("two", 600, null),
			new BuiltinFontWidth("summation", 600, null),
			new BuiltinFontWidth("Igrave", 600, null),
			new BuiltinFontWidth("Lacute", 600, null),
			new BuiltinFontWidth("ocircumflex", 600, null),
			new BuiltinFontWidth("oacute", 600, null),
			new BuiltinFontWidth("Uring", 600, null),
			new BuiltinFontWidth("Lcommaaccent", 600, null),
			new BuiltinFontWidth("tcaron", 600, null),
			new BuiltinFontWidth("eogonek", 600, null),
			new BuiltinFontWidth("Delta", 600, null),
			new BuiltinFontWidth("Ohungarumlaut", 600, null),
			new BuiltinFontWidth("asciicircum", 600, null),
			new BuiltinFontWidth("aring", 600, null),
			new BuiltinFontWidth("grave", 600, null),
			new BuiltinFontWidth("uogonek", 600, null),
			new BuiltinFontWidth("bracketright", 600, null),
			new BuiltinFontWidth("ampersand", 600, null),
			new BuiltinFontWidth("Iacute", 600, null),
			new BuiltinFontWidth("lacute", 600, null),
			new BuiltinFontWidth("igrave", 600, null),
			new BuiltinFontWidth("Ncaron", 600, null),
			new BuiltinFontWidth("plus", 600, null),
			new BuiltinFontWidth("uring", 600, null),
			new BuiltinFontWidth("quotesinglbase", 600, null),
			new BuiltinFontWidth("lcommaaccent", 600, null),
			new BuiltinFontWidth("Yacute", 600, null),
			new BuiltinFontWidth("ohungarumlaut", 600, null),
			new BuiltinFontWidth("threesuperior", 600, null),
			new BuiltinFontWidth("acute", 600, null),
			new BuiltinFontWidth("section", 600, null),
			new BuiltinFontWidth("dieresis", 600, null),
			new BuiltinFontWidth("quotedblbase", 600, null),
			new BuiltinFontWidth("iacute", 600, null),
			new BuiltinFontWidth("ncaron", 600, null),
			new BuiltinFontWidth("florin", 600, null),
			new BuiltinFontWidth("yacute", 600, null),
			new BuiltinFontWidth("Rcommaaccent", 600, null),
			new BuiltinFontWidth("fi", 600, null),
			new BuiltinFontWidth("fl", 600, null),
			new BuiltinFontWidth("Acircumflex", 600, null),
			new BuiltinFontWidth("Cacute", 600, null),
			new BuiltinFontWidth("Icircumflex", 600, null),
			new BuiltinFontWidth("guillemotleft", 600, null),
			new BuiltinFontWidth("germandbls", 600, null),
			new BuiltinFontWidth("seven", 600, null),
			new BuiltinFontWidth("Amacron", 600, null),
			new BuiltinFontWidth("Sacute", 600, null),
			new BuiltinFontWidth("ordmasculine", 600, null),
			new BuiltinFontWidth("dotlessi", 600, null),
			new BuiltinFontWidth("sterling", 600, null),
			new BuiltinFontWidth("notequal", 600, null),
			new BuiltinFontWidth("Imacron", 600, null),
			new BuiltinFontWidth("rcommaaccent", 600, null),
			new BuiltinFontWidth("Zdotaccent", 600, null),
			new BuiltinFontWidth("acircumflex", 600, null),
			new BuiltinFontWidth("cacute", 600, null),
			new BuiltinFontWidth("Ecaron", 600, null),
			new BuiltinFontWidth("braceright", 600, null),
			new BuiltinFontWidth("icircumflex", 600, null),
			new BuiltinFontWidth("quotedblright", 600, null),
			new BuiltinFontWidth("amacron", 600, null),
			new BuiltinFontWidth("sacute", 600, null),
			new BuiltinFontWidth("imacron", 600, null),
			new BuiltinFontWidth("cent", 600, null),
			new BuiltinFontWidth("currency", 600, null),
			new BuiltinFontWidth("logicalnot", 600, null),
			new BuiltinFontWidth("zdotaccent", 600, null),
			new BuiltinFontWidth("Atilde", 600, null),
			new BuiltinFontWidth("breve", 600, null),
			new BuiltinFontWidth("bar", 600, null),
			new BuiltinFontWidth("fraction", 600, null),
			new BuiltinFontWidth("less", 600, null),
			new BuiltinFontWidth("ecaron", 600, null),
			new BuiltinFontWidth("guilsinglleft", 600, null),
			new BuiltinFontWidth("exclam", 600, null),
			new BuiltinFontWidth("period", 600, null),
			new BuiltinFontWidth("Rcaron", 600, null),
			new BuiltinFontWidth("Kcommaaccent", 600, null),
			new BuiltinFontWidth("greater", 600, null),
			new BuiltinFontWidth("atilde", 600, null),
			new BuiltinFontWidth("brokenbar", 600, null),
			new BuiltinFontWidth("quoteleft", 600, null),
			new BuiltinFontWidth("Edotaccent", 600, null),
			new BuiltinFontWidth("onesuperior", 600, null)
		};
		private static BuiltinFontWidth[] courierBoldWidthsTab = new BuiltinFontWidth[]
		{
			new BuiltinFontWidth("Ntilde", 600, null),
			new BuiltinFontWidth("rcaron", 600, null),
			new BuiltinFontWidth("kcommaaccent", 600, null),
			new BuiltinFontWidth("Ncommaaccent", 600, null),
			new BuiltinFontWidth("Zacute", 600, null),
			new BuiltinFontWidth("comma", 600, null),
			new BuiltinFontWidth("cedilla", 600, null),
			new BuiltinFontWidth("plusminus", 600, null),
			new BuiltinFontWidth("circumflex", 600, null),
			new BuiltinFontWidth("dotaccent", 600, null),
			new BuiltinFontWidth("edotaccent", 600, null),
			new BuiltinFontWidth("asciitilde", 600, null),
			new BuiltinFontWidth("colon", 600, null),
			new BuiltinFontWidth("onehalf", 600, null),
			new BuiltinFontWidth("dollar", 600, null),
			new BuiltinFontWidth("Lcaron", 600, null),
			new BuiltinFontWidth("ntilde", 600, null),
			new BuiltinFontWidth("Aogonek", 600, null),
			new BuiltinFontWidth("ncommaaccent", 600, null),
			new BuiltinFontWidth("minus", 600, null),
			new BuiltinFontWidth("Iogonek", 600, null),
			new BuiltinFontWidth("zacute", 600, null),
			new BuiltinFontWidth("yen", 600, null),
			new BuiltinFontWidth("space", 600, null),
			new BuiltinFontWidth("Omacron", 600, null),
			new BuiltinFontWidth("questiondown", 600, null),
			new BuiltinFontWidth("emdash", 600, null),
			new BuiltinFontWidth("Agrave", 600, null),
			new BuiltinFontWidth("three", 600, null),
			new BuiltinFontWidth("numbersign", 600, null),
			new BuiltinFontWidth("lcaron", 600, null),
			new BuiltinFontWidth("A", 600, null),
			new BuiltinFontWidth("B", 600, null),
			new BuiltinFontWidth("C", 600, null),
			new BuiltinFontWidth("aogonek", 600, null),
			new BuiltinFontWidth("D", 600, null),
			new BuiltinFontWidth("E", 600, null),
			new BuiltinFontWidth("onequarter", 600, null),
			new BuiltinFontWidth("F", 600, null),
			new BuiltinFontWidth("G", 600, null),
			new BuiltinFontWidth("H", 600, null),
			new BuiltinFontWidth("I", 600, null),
			new BuiltinFontWidth("J", 600, null),
			new BuiltinFontWidth("K", 600, null),
			new BuiltinFontWidth("iogonek", 600, null),
			new BuiltinFontWidth("backslash", 600, null),
			new BuiltinFontWidth("L", 600, null),
			new BuiltinFontWidth("periodcentered", 600, null),
			new BuiltinFontWidth("M", 600, null),
			new BuiltinFontWidth("N", 600, null),
			new BuiltinFontWidth("omacron", 600, null),
			new BuiltinFontWidth("Tcommaaccent", 600, null),
			new BuiltinFontWidth("O", 600, null),
			new BuiltinFontWidth("P", 600, null),
			new BuiltinFontWidth("Q", 600, null),
			new BuiltinFontWidth("Uhungarumlaut", 600, null),
			new BuiltinFontWidth("R", 600, null),
			new BuiltinFontWidth("Aacute", 600, null),
			new BuiltinFontWidth("caron", 600, null),
			new BuiltinFontWidth("S", 600, null),
			new BuiltinFontWidth("T", 600, null),
			new BuiltinFontWidth("U", 600, null),
			new BuiltinFontWidth("agrave", 600, null),
			new BuiltinFontWidth("V", 600, null),
			new BuiltinFontWidth("W", 600, null),
			new BuiltinFontWidth("X", 600, null),
			new BuiltinFontWidth("question", 600, null),
			new BuiltinFontWidth("equal", 600, null),
			new BuiltinFontWidth("Y", 600, null),
			new BuiltinFontWidth("Z", 600, null),
			new BuiltinFontWidth("four", 600, null),
			new BuiltinFontWidth("a", 600, null),
			new BuiltinFontWidth("Gcommaaccent", 600, null),
			new BuiltinFontWidth("b", 600, null),
			new BuiltinFontWidth("c", 600, null),
			new BuiltinFontWidth("d", 600, null),
			new BuiltinFontWidth("e", 600, null),
			new BuiltinFontWidth("f", 600, null),
			new BuiltinFontWidth("g", 600, null),
			new BuiltinFontWidth("bullet", 600, null),
			new BuiltinFontWidth("h", 600, null),
			new BuiltinFontWidth("i", 600, null),
			new BuiltinFontWidth("Oslash", 600, null),
			new BuiltinFontWidth("dagger", 600, null),
			new BuiltinFontWidth("j", 600, null),
			new BuiltinFontWidth("k", 600, null),
			new BuiltinFontWidth("l", 600, null),
			new BuiltinFontWidth("m", 600, null),
			new BuiltinFontWidth("n", 600, null),
			new BuiltinFontWidth("tcommaaccent", 600, null),
			new BuiltinFontWidth("o", 600, null),
			new BuiltinFontWidth("ordfeminine", 600, null),
			new BuiltinFontWidth("ring", 600, null),
			new BuiltinFontWidth("p", 600, null),
			new BuiltinFontWidth("q", 600, null),
			new BuiltinFontWidth("uhungarumlaut", 600, null),
			new BuiltinFontWidth("r", 600, null),
			new BuiltinFontWidth("twosuperior", 600, null),
			new BuiltinFontWidth("aacute", 600, null),
			new BuiltinFontWidth("s", 600, null),
			new BuiltinFontWidth("OE", 600, null),
			new BuiltinFontWidth("t", 600, null),
			new BuiltinFontWidth("divide", 600, null),
			new BuiltinFontWidth("u", 600, null),
			new BuiltinFontWidth("Ccaron", 600, null),
			new BuiltinFontWidth("v", 600, null),
			new BuiltinFontWidth("w", 600, null),
			new BuiltinFontWidth("x", 600, null),
			new BuiltinFontWidth("y", 600, null),
			new BuiltinFontWidth("z", 600, null),
			new BuiltinFontWidth("Gbreve", 600, null),
			new BuiltinFontWidth("commaaccent", 600, null),
			new BuiltinFontWidth("hungarumlaut", 600, null),
			new BuiltinFontWidth("Idotaccent", 600, null),
			new BuiltinFontWidth("Nacute", 600, null),
			new BuiltinFontWidth("quotedbl", 600, null),
			new BuiltinFontWidth("gcommaaccent", 600, null),
			new BuiltinFontWidth("mu", 600, null),
			new BuiltinFontWidth("greaterequal", 600, null),
			new BuiltinFontWidth("Scaron", 600, null),
			new BuiltinFontWidth("Lslash", 600, null),
			new BuiltinFontWidth("semicolon", 600, null),
			new BuiltinFontWidth("oslash", 600, null),
			new BuiltinFontWidth("lessequal", 600, null),
			new BuiltinFontWidth("lozenge", 600, null),
			new BuiltinFontWidth("parenright", 600, null),
			new BuiltinFontWidth("ccaron", 600, null),
			new BuiltinFontWidth("Ecircumflex", 600, null),
			new BuiltinFontWidth("gbreve", 600, null),
			new BuiltinFontWidth("trademark", 600, null),
			new BuiltinFontWidth("daggerdbl", 600, null),
			new BuiltinFontWidth("nacute", 600, null),
			new BuiltinFontWidth("macron", 600, null),
			new BuiltinFontWidth("Otilde", 600, null),
			new BuiltinFontWidth("Emacron", 600, null),
			new BuiltinFontWidth("ellipsis", 600, null),
			new BuiltinFontWidth("scaron", 600, null),
			new BuiltinFontWidth("AE", 600, null),
			new BuiltinFontWidth("Ucircumflex", 600, null),
			new BuiltinFontWidth("lslash", 600, null),
			new BuiltinFontWidth("quotedblleft", 600, null),
			new BuiltinFontWidth("guilsinglright", 600, null),
			new BuiltinFontWidth("hyphen", 600, null),
			new BuiltinFontWidth("quotesingle", 600, null),
			new BuiltinFontWidth("eight", 600, null),
			new BuiltinFontWidth("exclamdown", 600, null),
			new BuiltinFontWidth("endash", 600, null),
			new BuiltinFontWidth("oe", 600, null),
			new BuiltinFontWidth("Abreve", 600, null),
			new BuiltinFontWidth("Umacron", 600, null),
			new BuiltinFontWidth("ecircumflex", 600, null),
			new BuiltinFontWidth("Adieresis", 600, null),
			new BuiltinFontWidth("copyright", 600, null),
			new BuiltinFontWidth("Egrave", 600, null),
			new BuiltinFontWidth("slash", 600, null),
			new BuiltinFontWidth("Edieresis", 600, null),
			new BuiltinFontWidth("otilde", 600, null),
			new BuiltinFontWidth("Idieresis", 600, null),
			new BuiltinFontWidth("parenleft", 600, null),
			new BuiltinFontWidth("one", 600, null),
			new BuiltinFontWidth("emacron", 600, null),
			new BuiltinFontWidth("Odieresis", 600, null),
			new BuiltinFontWidth("ucircumflex", 600, null),
			new BuiltinFontWidth("bracketleft", 600, null),
			new BuiltinFontWidth("Ugrave", 600, null),
			new BuiltinFontWidth("quoteright", 600, null),
			new BuiltinFontWidth("Udieresis", 600, null),
			new BuiltinFontWidth("perthousand", 600, null),
			new BuiltinFontWidth("Ydieresis", 600, null),
			new BuiltinFontWidth("umacron", 600, null),
			new BuiltinFontWidth("abreve", 600, null),
			new BuiltinFontWidth("Eacute", 600, null),
			new BuiltinFontWidth("adieresis", 600, null),
			new BuiltinFontWidth("egrave", 600, null),
			new BuiltinFontWidth("edieresis", 600, null),
			new BuiltinFontWidth("idieresis", 600, null),
			new BuiltinFontWidth("Eth", 600, null),
			new BuiltinFontWidth("ae", 600, null),
			new BuiltinFontWidth("asterisk", 600, null),
			new BuiltinFontWidth("odieresis", 600, null),
			new BuiltinFontWidth("Uacute", 600, null),
			new BuiltinFontWidth("ugrave", 600, null),
			new BuiltinFontWidth("nine", 600, null),
			new BuiltinFontWidth("five", 600, null),
			new BuiltinFontWidth("udieresis", 600, null),
			new BuiltinFontWidth("Zcaron", 600, null),
			new BuiltinFontWidth("Scommaaccent", 600, null),
			new BuiltinFontWidth("threequarters", 600, null),
			new BuiltinFontWidth("guillemotright", 600, null),
			new BuiltinFontWidth("Ccedilla", 600, null),
			new BuiltinFontWidth("ydieresis", 600, null),
			new BuiltinFontWidth("tilde", 600, null),
			new BuiltinFontWidth("at", 600, null),
			new BuiltinFontWidth("eacute", 600, null),
			new BuiltinFontWidth("underscore", 600, null),
			new BuiltinFontWidth("Euro", 600, null),
			new BuiltinFontWidth("Dcroat", 600, null),
			new BuiltinFontWidth("multiply", 600, null),
			new BuiltinFontWidth("zero", 600, null),
			new BuiltinFontWidth("eth", 600, null),
			new BuiltinFontWidth("Scedilla", 600, null),
			new BuiltinFontWidth("Ograve", 600, null),
			new BuiltinFontWidth("Racute", 600, null),
			new BuiltinFontWidth("partialdiff", 600, null),
			new BuiltinFontWidth("uacute", 600, null),
			new BuiltinFontWidth("braceleft", 600, null),
			new BuiltinFontWidth("Thorn", 600, null),
			new BuiltinFontWidth("zcaron", 600, null),
			new BuiltinFontWidth("scommaaccent", 600, null),
			new BuiltinFontWidth("ccedilla", 600, null),
			new BuiltinFontWidth("Dcaron", 600, null),
			new BuiltinFontWidth("dcroat", 600, null),
			new BuiltinFontWidth("Ocircumflex", 600, null),
			new BuiltinFontWidth("Oacute", 600, null),
			new BuiltinFontWidth("scedilla", 600, null),
			new BuiltinFontWidth("ogonek", 600, null),
			new BuiltinFontWidth("ograve", 600, null),
			new BuiltinFontWidth("racute", 600, null),
			new BuiltinFontWidth("Tcaron", 600, null),
			new BuiltinFontWidth("Eogonek", 600, null),
			new BuiltinFontWidth("thorn", 600, null),
			new BuiltinFontWidth("degree", 600, null),
			new BuiltinFontWidth("registered", 600, null),
			new BuiltinFontWidth("radical", 600, null),
			new BuiltinFontWidth("Aring", 600, null),
			new BuiltinFontWidth("percent", 600, null),
			new BuiltinFontWidth("six", 600, null),
			new BuiltinFontWidth("paragraph", 600, null),
			new BuiltinFontWidth("dcaron", 600, null),
			new BuiltinFontWidth("Uogonek", 600, null),
			new BuiltinFontWidth("two", 600, null),
			new BuiltinFontWidth("summation", 600, null),
			new BuiltinFontWidth("Igrave", 600, null),
			new BuiltinFontWidth("Lacute", 600, null),
			new BuiltinFontWidth("ocircumflex", 600, null),
			new BuiltinFontWidth("oacute", 600, null),
			new BuiltinFontWidth("Uring", 600, null),
			new BuiltinFontWidth("Lcommaaccent", 600, null),
			new BuiltinFontWidth("tcaron", 600, null),
			new BuiltinFontWidth("eogonek", 600, null),
			new BuiltinFontWidth("Delta", 600, null),
			new BuiltinFontWidth("Ohungarumlaut", 600, null),
			new BuiltinFontWidth("asciicircum", 600, null),
			new BuiltinFontWidth("aring", 600, null),
			new BuiltinFontWidth("grave", 600, null),
			new BuiltinFontWidth("uogonek", 600, null),
			new BuiltinFontWidth("bracketright", 600, null),
			new BuiltinFontWidth("Iacute", 600, null),
			new BuiltinFontWidth("ampersand", 600, null),
			new BuiltinFontWidth("igrave", 600, null),
			new BuiltinFontWidth("lacute", 600, null),
			new BuiltinFontWidth("Ncaron", 600, null),
			new BuiltinFontWidth("plus", 600, null),
			new BuiltinFontWidth("uring", 600, null),
			new BuiltinFontWidth("quotesinglbase", 600, null),
			new BuiltinFontWidth("lcommaaccent", 600, null),
			new BuiltinFontWidth("Yacute", 600, null),
			new BuiltinFontWidth("ohungarumlaut", 600, null),
			new BuiltinFontWidth("threesuperior", 600, null),
			new BuiltinFontWidth("acute", 600, null),
			new BuiltinFontWidth("section", 600, null),
			new BuiltinFontWidth("dieresis", 600, null),
			new BuiltinFontWidth("iacute", 600, null),
			new BuiltinFontWidth("quotedblbase", 600, null),
			new BuiltinFontWidth("ncaron", 600, null),
			new BuiltinFontWidth("florin", 600, null),
			new BuiltinFontWidth("yacute", 600, null),
			new BuiltinFontWidth("Rcommaaccent", 600, null),
			new BuiltinFontWidth("fi", 600, null),
			new BuiltinFontWidth("fl", 600, null),
			new BuiltinFontWidth("Acircumflex", 600, null),
			new BuiltinFontWidth("Cacute", 600, null),
			new BuiltinFontWidth("Icircumflex", 600, null),
			new BuiltinFontWidth("guillemotleft", 600, null),
			new BuiltinFontWidth("germandbls", 600, null),
			new BuiltinFontWidth("Amacron", 600, null),
			new BuiltinFontWidth("seven", 600, null),
			new BuiltinFontWidth("Sacute", 600, null),
			new BuiltinFontWidth("ordmasculine", 600, null),
			new BuiltinFontWidth("dotlessi", 600, null),
			new BuiltinFontWidth("sterling", 600, null),
			new BuiltinFontWidth("notequal", 600, null),
			new BuiltinFontWidth("Imacron", 600, null),
			new BuiltinFontWidth("rcommaaccent", 600, null),
			new BuiltinFontWidth("Zdotaccent", 600, null),
			new BuiltinFontWidth("acircumflex", 600, null),
			new BuiltinFontWidth("cacute", 600, null),
			new BuiltinFontWidth("Ecaron", 600, null),
			new BuiltinFontWidth("icircumflex", 600, null),
			new BuiltinFontWidth("braceright", 600, null),
			new BuiltinFontWidth("quotedblright", 600, null),
			new BuiltinFontWidth("amacron", 600, null),
			new BuiltinFontWidth("sacute", 600, null),
			new BuiltinFontWidth("imacron", 600, null),
			new BuiltinFontWidth("cent", 600, null),
			new BuiltinFontWidth("currency", 600, null),
			new BuiltinFontWidth("logicalnot", 600, null),
			new BuiltinFontWidth("zdotaccent", 600, null),
			new BuiltinFontWidth("Atilde", 600, null),
			new BuiltinFontWidth("breve", 600, null),
			new BuiltinFontWidth("bar", 600, null),
			new BuiltinFontWidth("fraction", 600, null),
			new BuiltinFontWidth("less", 600, null),
			new BuiltinFontWidth("ecaron", 600, null),
			new BuiltinFontWidth("guilsinglleft", 600, null),
			new BuiltinFontWidth("exclam", 600, null),
			new BuiltinFontWidth("period", 600, null),
			new BuiltinFontWidth("Rcaron", 600, null),
			new BuiltinFontWidth("Kcommaaccent", 600, null),
			new BuiltinFontWidth("greater", 600, null),
			new BuiltinFontWidth("atilde", 600, null),
			new BuiltinFontWidth("brokenbar", 600, null),
			new BuiltinFontWidth("quoteleft", 600, null),
			new BuiltinFontWidth("Edotaccent", 600, null),
			new BuiltinFontWidth("onesuperior", 600, null)
		};
		private static BuiltinFontWidth[] courierBoldObliqueWidthsTab = new BuiltinFontWidth[]
		{
			new BuiltinFontWidth("Ntilde", 600, null),
			new BuiltinFontWidth("rcaron", 600, null),
			new BuiltinFontWidth("kcommaaccent", 600, null),
			new BuiltinFontWidth("Ncommaaccent", 600, null),
			new BuiltinFontWidth("Zacute", 600, null),
			new BuiltinFontWidth("comma", 600, null),
			new BuiltinFontWidth("cedilla", 600, null),
			new BuiltinFontWidth("plusminus", 600, null),
			new BuiltinFontWidth("circumflex", 600, null),
			new BuiltinFontWidth("dotaccent", 600, null),
			new BuiltinFontWidth("edotaccent", 600, null),
			new BuiltinFontWidth("asciitilde", 600, null),
			new BuiltinFontWidth("colon", 600, null),
			new BuiltinFontWidth("onehalf", 600, null),
			new BuiltinFontWidth("dollar", 600, null),
			new BuiltinFontWidth("Lcaron", 600, null),
			new BuiltinFontWidth("ntilde", 600, null),
			new BuiltinFontWidth("Aogonek", 600, null),
			new BuiltinFontWidth("ncommaaccent", 600, null),
			new BuiltinFontWidth("minus", 600, null),
			new BuiltinFontWidth("Iogonek", 600, null),
			new BuiltinFontWidth("zacute", 600, null),
			new BuiltinFontWidth("yen", 600, null),
			new BuiltinFontWidth("space", 600, null),
			new BuiltinFontWidth("Omacron", 600, null),
			new BuiltinFontWidth("questiondown", 600, null),
			new BuiltinFontWidth("emdash", 600, null),
			new BuiltinFontWidth("Agrave", 600, null),
			new BuiltinFontWidth("three", 600, null),
			new BuiltinFontWidth("numbersign", 600, null),
			new BuiltinFontWidth("lcaron", 600, null),
			new BuiltinFontWidth("A", 600, null),
			new BuiltinFontWidth("B", 600, null),
			new BuiltinFontWidth("C", 600, null),
			new BuiltinFontWidth("aogonek", 600, null),
			new BuiltinFontWidth("D", 600, null),
			new BuiltinFontWidth("E", 600, null),
			new BuiltinFontWidth("onequarter", 600, null),
			new BuiltinFontWidth("F", 600, null),
			new BuiltinFontWidth("G", 600, null),
			new BuiltinFontWidth("H", 600, null),
			new BuiltinFontWidth("I", 600, null),
			new BuiltinFontWidth("J", 600, null),
			new BuiltinFontWidth("K", 600, null),
			new BuiltinFontWidth("iogonek", 600, null),
			new BuiltinFontWidth("backslash", 600, null),
			new BuiltinFontWidth("L", 600, null),
			new BuiltinFontWidth("periodcentered", 600, null),
			new BuiltinFontWidth("M", 600, null),
			new BuiltinFontWidth("N", 600, null),
			new BuiltinFontWidth("omacron", 600, null),
			new BuiltinFontWidth("Tcommaaccent", 600, null),
			new BuiltinFontWidth("O", 600, null),
			new BuiltinFontWidth("P", 600, null),
			new BuiltinFontWidth("Q", 600, null),
			new BuiltinFontWidth("Uhungarumlaut", 600, null),
			new BuiltinFontWidth("R", 600, null),
			new BuiltinFontWidth("Aacute", 600, null),
			new BuiltinFontWidth("caron", 600, null),
			new BuiltinFontWidth("S", 600, null),
			new BuiltinFontWidth("T", 600, null),
			new BuiltinFontWidth("U", 600, null),
			new BuiltinFontWidth("agrave", 600, null),
			new BuiltinFontWidth("V", 600, null),
			new BuiltinFontWidth("W", 600, null),
			new BuiltinFontWidth("X", 600, null),
			new BuiltinFontWidth("question", 600, null),
			new BuiltinFontWidth("equal", 600, null),
			new BuiltinFontWidth("Y", 600, null),
			new BuiltinFontWidth("Z", 600, null),
			new BuiltinFontWidth("four", 600, null),
			new BuiltinFontWidth("a", 600, null),
			new BuiltinFontWidth("Gcommaaccent", 600, null),
			new BuiltinFontWidth("b", 600, null),
			new BuiltinFontWidth("c", 600, null),
			new BuiltinFontWidth("d", 600, null),
			new BuiltinFontWidth("e", 600, null),
			new BuiltinFontWidth("f", 600, null),
			new BuiltinFontWidth("g", 600, null),
			new BuiltinFontWidth("bullet", 600, null),
			new BuiltinFontWidth("h", 600, null),
			new BuiltinFontWidth("i", 600, null),
			new BuiltinFontWidth("Oslash", 600, null),
			new BuiltinFontWidth("dagger", 600, null),
			new BuiltinFontWidth("j", 600, null),
			new BuiltinFontWidth("k", 600, null),
			new BuiltinFontWidth("l", 600, null),
			new BuiltinFontWidth("m", 600, null),
			new BuiltinFontWidth("n", 600, null),
			new BuiltinFontWidth("tcommaaccent", 600, null),
			new BuiltinFontWidth("o", 600, null),
			new BuiltinFontWidth("ordfeminine", 600, null),
			new BuiltinFontWidth("ring", 600, null),
			new BuiltinFontWidth("p", 600, null),
			new BuiltinFontWidth("q", 600, null),
			new BuiltinFontWidth("uhungarumlaut", 600, null),
			new BuiltinFontWidth("r", 600, null),
			new BuiltinFontWidth("twosuperior", 600, null),
			new BuiltinFontWidth("aacute", 600, null),
			new BuiltinFontWidth("s", 600, null),
			new BuiltinFontWidth("OE", 600, null),
			new BuiltinFontWidth("t", 600, null),
			new BuiltinFontWidth("divide", 600, null),
			new BuiltinFontWidth("u", 600, null),
			new BuiltinFontWidth("Ccaron", 600, null),
			new BuiltinFontWidth("v", 600, null),
			new BuiltinFontWidth("w", 600, null),
			new BuiltinFontWidth("x", 600, null),
			new BuiltinFontWidth("y", 600, null),
			new BuiltinFontWidth("z", 600, null),
			new BuiltinFontWidth("Gbreve", 600, null),
			new BuiltinFontWidth("commaaccent", 600, null),
			new BuiltinFontWidth("hungarumlaut", 600, null),
			new BuiltinFontWidth("Idotaccent", 600, null),
			new BuiltinFontWidth("Nacute", 600, null),
			new BuiltinFontWidth("quotedbl", 600, null),
			new BuiltinFontWidth("gcommaaccent", 600, null),
			new BuiltinFontWidth("mu", 600, null),
			new BuiltinFontWidth("greaterequal", 600, null),
			new BuiltinFontWidth("Scaron", 600, null),
			new BuiltinFontWidth("Lslash", 600, null),
			new BuiltinFontWidth("semicolon", 600, null),
			new BuiltinFontWidth("oslash", 600, null),
			new BuiltinFontWidth("lessequal", 600, null),
			new BuiltinFontWidth("lozenge", 600, null),
			new BuiltinFontWidth("parenright", 600, null),
			new BuiltinFontWidth("ccaron", 600, null),
			new BuiltinFontWidth("Ecircumflex", 600, null),
			new BuiltinFontWidth("gbreve", 600, null),
			new BuiltinFontWidth("trademark", 600, null),
			new BuiltinFontWidth("daggerdbl", 600, null),
			new BuiltinFontWidth("nacute", 600, null),
			new BuiltinFontWidth("macron", 600, null),
			new BuiltinFontWidth("Otilde", 600, null),
			new BuiltinFontWidth("Emacron", 600, null),
			new BuiltinFontWidth("ellipsis", 600, null),
			new BuiltinFontWidth("scaron", 600, null),
			new BuiltinFontWidth("AE", 600, null),
			new BuiltinFontWidth("Ucircumflex", 600, null),
			new BuiltinFontWidth("lslash", 600, null),
			new BuiltinFontWidth("quotedblleft", 600, null),
			new BuiltinFontWidth("guilsinglright", 600, null),
			new BuiltinFontWidth("hyphen", 600, null),
			new BuiltinFontWidth("quotesingle", 600, null),
			new BuiltinFontWidth("eight", 600, null),
			new BuiltinFontWidth("exclamdown", 600, null),
			new BuiltinFontWidth("endash", 600, null),
			new BuiltinFontWidth("oe", 600, null),
			new BuiltinFontWidth("Abreve", 600, null),
			new BuiltinFontWidth("Umacron", 600, null),
			new BuiltinFontWidth("ecircumflex", 600, null),
			new BuiltinFontWidth("Adieresis", 600, null),
			new BuiltinFontWidth("copyright", 600, null),
			new BuiltinFontWidth("Egrave", 600, null),
			new BuiltinFontWidth("slash", 600, null),
			new BuiltinFontWidth("Edieresis", 600, null),
			new BuiltinFontWidth("otilde", 600, null),
			new BuiltinFontWidth("Idieresis", 600, null),
			new BuiltinFontWidth("parenleft", 600, null),
			new BuiltinFontWidth("one", 600, null),
			new BuiltinFontWidth("emacron", 600, null),
			new BuiltinFontWidth("Odieresis", 600, null),
			new BuiltinFontWidth("ucircumflex", 600, null),
			new BuiltinFontWidth("bracketleft", 600, null),
			new BuiltinFontWidth("Ugrave", 600, null),
			new BuiltinFontWidth("quoteright", 600, null),
			new BuiltinFontWidth("Udieresis", 600, null),
			new BuiltinFontWidth("perthousand", 600, null),
			new BuiltinFontWidth("Ydieresis", 600, null),
			new BuiltinFontWidth("umacron", 600, null),
			new BuiltinFontWidth("abreve", 600, null),
			new BuiltinFontWidth("Eacute", 600, null),
			new BuiltinFontWidth("adieresis", 600, null),
			new BuiltinFontWidth("egrave", 600, null),
			new BuiltinFontWidth("edieresis", 600, null),
			new BuiltinFontWidth("idieresis", 600, null),
			new BuiltinFontWidth("Eth", 600, null),
			new BuiltinFontWidth("ae", 600, null),
			new BuiltinFontWidth("asterisk", 600, null),
			new BuiltinFontWidth("odieresis", 600, null),
			new BuiltinFontWidth("Uacute", 600, null),
			new BuiltinFontWidth("ugrave", 600, null),
			new BuiltinFontWidth("nine", 600, null),
			new BuiltinFontWidth("five", 600, null),
			new BuiltinFontWidth("udieresis", 600, null),
			new BuiltinFontWidth("Zcaron", 600, null),
			new BuiltinFontWidth("Scommaaccent", 600, null),
			new BuiltinFontWidth("threequarters", 600, null),
			new BuiltinFontWidth("guillemotright", 600, null),
			new BuiltinFontWidth("Ccedilla", 600, null),
			new BuiltinFontWidth("ydieresis", 600, null),
			new BuiltinFontWidth("tilde", 600, null),
			new BuiltinFontWidth("at", 600, null),
			new BuiltinFontWidth("eacute", 600, null),
			new BuiltinFontWidth("underscore", 600, null),
			new BuiltinFontWidth("Euro", 600, null),
			new BuiltinFontWidth("Dcroat", 600, null),
			new BuiltinFontWidth("multiply", 600, null),
			new BuiltinFontWidth("zero", 600, null),
			new BuiltinFontWidth("eth", 600, null),
			new BuiltinFontWidth("Scedilla", 600, null),
			new BuiltinFontWidth("Ograve", 600, null),
			new BuiltinFontWidth("Racute", 600, null),
			new BuiltinFontWidth("partialdiff", 600, null),
			new BuiltinFontWidth("uacute", 600, null),
			new BuiltinFontWidth("braceleft", 600, null),
			new BuiltinFontWidth("Thorn", 600, null),
			new BuiltinFontWidth("zcaron", 600, null),
			new BuiltinFontWidth("scommaaccent", 600, null),
			new BuiltinFontWidth("ccedilla", 600, null),
			new BuiltinFontWidth("Dcaron", 600, null),
			new BuiltinFontWidth("dcroat", 600, null),
			new BuiltinFontWidth("Ocircumflex", 600, null),
			new BuiltinFontWidth("Oacute", 600, null),
			new BuiltinFontWidth("scedilla", 600, null),
			new BuiltinFontWidth("ogonek", 600, null),
			new BuiltinFontWidth("ograve", 600, null),
			new BuiltinFontWidth("racute", 600, null),
			new BuiltinFontWidth("Tcaron", 600, null),
			new BuiltinFontWidth("Eogonek", 600, null),
			new BuiltinFontWidth("thorn", 600, null),
			new BuiltinFontWidth("degree", 600, null),
			new BuiltinFontWidth("registered", 600, null),
			new BuiltinFontWidth("radical", 600, null),
			new BuiltinFontWidth("Aring", 600, null),
			new BuiltinFontWidth("percent", 600, null),
			new BuiltinFontWidth("six", 600, null),
			new BuiltinFontWidth("paragraph", 600, null),
			new BuiltinFontWidth("dcaron", 600, null),
			new BuiltinFontWidth("Uogonek", 600, null),
			new BuiltinFontWidth("two", 600, null),
			new BuiltinFontWidth("summation", 600, null),
			new BuiltinFontWidth("Igrave", 600, null),
			new BuiltinFontWidth("Lacute", 600, null),
			new BuiltinFontWidth("ocircumflex", 600, null),
			new BuiltinFontWidth("oacute", 600, null),
			new BuiltinFontWidth("Uring", 600, null),
			new BuiltinFontWidth("Lcommaaccent", 600, null),
			new BuiltinFontWidth("tcaron", 600, null),
			new BuiltinFontWidth("eogonek", 600, null),
			new BuiltinFontWidth("Delta", 600, null),
			new BuiltinFontWidth("Ohungarumlaut", 600, null),
			new BuiltinFontWidth("asciicircum", 600, null),
			new BuiltinFontWidth("aring", 600, null),
			new BuiltinFontWidth("grave", 600, null),
			new BuiltinFontWidth("uogonek", 600, null),
			new BuiltinFontWidth("bracketright", 600, null),
			new BuiltinFontWidth("Iacute", 600, null),
			new BuiltinFontWidth("ampersand", 600, null),
			new BuiltinFontWidth("igrave", 600, null),
			new BuiltinFontWidth("lacute", 600, null),
			new BuiltinFontWidth("Ncaron", 600, null),
			new BuiltinFontWidth("plus", 600, null),
			new BuiltinFontWidth("uring", 600, null),
			new BuiltinFontWidth("quotesinglbase", 600, null),
			new BuiltinFontWidth("lcommaaccent", 600, null),
			new BuiltinFontWidth("Yacute", 600, null),
			new BuiltinFontWidth("ohungarumlaut", 600, null),
			new BuiltinFontWidth("threesuperior", 600, null),
			new BuiltinFontWidth("acute", 600, null),
			new BuiltinFontWidth("section", 600, null),
			new BuiltinFontWidth("dieresis", 600, null),
			new BuiltinFontWidth("iacute", 600, null),
			new BuiltinFontWidth("quotedblbase", 600, null),
			new BuiltinFontWidth("ncaron", 600, null),
			new BuiltinFontWidth("florin", 600, null),
			new BuiltinFontWidth("yacute", 600, null),
			new BuiltinFontWidth("Rcommaaccent", 600, null),
			new BuiltinFontWidth("fi", 600, null),
			new BuiltinFontWidth("fl", 600, null),
			new BuiltinFontWidth("Acircumflex", 600, null),
			new BuiltinFontWidth("Cacute", 600, null),
			new BuiltinFontWidth("Icircumflex", 600, null),
			new BuiltinFontWidth("guillemotleft", 600, null),
			new BuiltinFontWidth("germandbls", 600, null),
			new BuiltinFontWidth("Amacron", 600, null),
			new BuiltinFontWidth("seven", 600, null),
			new BuiltinFontWidth("Sacute", 600, null),
			new BuiltinFontWidth("ordmasculine", 600, null),
			new BuiltinFontWidth("dotlessi", 600, null),
			new BuiltinFontWidth("sterling", 600, null),
			new BuiltinFontWidth("notequal", 600, null),
			new BuiltinFontWidth("Imacron", 600, null),
			new BuiltinFontWidth("rcommaaccent", 600, null),
			new BuiltinFontWidth("Zdotaccent", 600, null),
			new BuiltinFontWidth("acircumflex", 600, null),
			new BuiltinFontWidth("cacute", 600, null),
			new BuiltinFontWidth("Ecaron", 600, null),
			new BuiltinFontWidth("icircumflex", 600, null),
			new BuiltinFontWidth("braceright", 600, null),
			new BuiltinFontWidth("quotedblright", 600, null),
			new BuiltinFontWidth("amacron", 600, null),
			new BuiltinFontWidth("sacute", 600, null),
			new BuiltinFontWidth("imacron", 600, null),
			new BuiltinFontWidth("cent", 600, null),
			new BuiltinFontWidth("currency", 600, null),
			new BuiltinFontWidth("logicalnot", 600, null),
			new BuiltinFontWidth("zdotaccent", 600, null),
			new BuiltinFontWidth("Atilde", 600, null),
			new BuiltinFontWidth("breve", 600, null),
			new BuiltinFontWidth("bar", 600, null),
			new BuiltinFontWidth("fraction", 600, null),
			new BuiltinFontWidth("less", 600, null),
			new BuiltinFontWidth("ecaron", 600, null),
			new BuiltinFontWidth("guilsinglleft", 600, null),
			new BuiltinFontWidth("exclam", 600, null),
			new BuiltinFontWidth("period", 600, null),
			new BuiltinFontWidth("Rcaron", 600, null),
			new BuiltinFontWidth("Kcommaaccent", 600, null),
			new BuiltinFontWidth("greater", 600, null),
			new BuiltinFontWidth("atilde", 600, null),
			new BuiltinFontWidth("brokenbar", 600, null),
			new BuiltinFontWidth("quoteleft", 600, null),
			new BuiltinFontWidth("Edotaccent", 600, null),
			new BuiltinFontWidth("onesuperior", 600, null)
		};
		private static BuiltinFontWidth[] courierObliqueWidthsTab = new BuiltinFontWidth[]
		{
			new BuiltinFontWidth("Ntilde", 600, null),
			new BuiltinFontWidth("rcaron", 600, null),
			new BuiltinFontWidth("kcommaaccent", 600, null),
			new BuiltinFontWidth("Ncommaaccent", 600, null),
			new BuiltinFontWidth("Zacute", 600, null),
			new BuiltinFontWidth("comma", 600, null),
			new BuiltinFontWidth("cedilla", 600, null),
			new BuiltinFontWidth("plusminus", 600, null),
			new BuiltinFontWidth("circumflex", 600, null),
			new BuiltinFontWidth("dotaccent", 600, null),
			new BuiltinFontWidth("edotaccent", 600, null),
			new BuiltinFontWidth("asciitilde", 600, null),
			new BuiltinFontWidth("colon", 600, null),
			new BuiltinFontWidth("onehalf", 600, null),
			new BuiltinFontWidth("dollar", 600, null),
			new BuiltinFontWidth("Lcaron", 600, null),
			new BuiltinFontWidth("ntilde", 600, null),
			new BuiltinFontWidth("Aogonek", 600, null),
			new BuiltinFontWidth("ncommaaccent", 600, null),
			new BuiltinFontWidth("minus", 600, null),
			new BuiltinFontWidth("Iogonek", 600, null),
			new BuiltinFontWidth("zacute", 600, null),
			new BuiltinFontWidth("yen", 600, null),
			new BuiltinFontWidth("space", 600, null),
			new BuiltinFontWidth("Omacron", 600, null),
			new BuiltinFontWidth("questiondown", 600, null),
			new BuiltinFontWidth("emdash", 600, null),
			new BuiltinFontWidth("Agrave", 600, null),
			new BuiltinFontWidth("three", 600, null),
			new BuiltinFontWidth("numbersign", 600, null),
			new BuiltinFontWidth("lcaron", 600, null),
			new BuiltinFontWidth("A", 600, null),
			new BuiltinFontWidth("B", 600, null),
			new BuiltinFontWidth("C", 600, null),
			new BuiltinFontWidth("aogonek", 600, null),
			new BuiltinFontWidth("D", 600, null),
			new BuiltinFontWidth("E", 600, null),
			new BuiltinFontWidth("onequarter", 600, null),
			new BuiltinFontWidth("F", 600, null),
			new BuiltinFontWidth("G", 600, null),
			new BuiltinFontWidth("H", 600, null),
			new BuiltinFontWidth("I", 600, null),
			new BuiltinFontWidth("J", 600, null),
			new BuiltinFontWidth("K", 600, null),
			new BuiltinFontWidth("iogonek", 600, null),
			new BuiltinFontWidth("backslash", 600, null),
			new BuiltinFontWidth("L", 600, null),
			new BuiltinFontWidth("periodcentered", 600, null),
			new BuiltinFontWidth("M", 600, null),
			new BuiltinFontWidth("N", 600, null),
			new BuiltinFontWidth("omacron", 600, null),
			new BuiltinFontWidth("Tcommaaccent", 600, null),
			new BuiltinFontWidth("O", 600, null),
			new BuiltinFontWidth("P", 600, null),
			new BuiltinFontWidth("Q", 600, null),
			new BuiltinFontWidth("Uhungarumlaut", 600, null),
			new BuiltinFontWidth("R", 600, null),
			new BuiltinFontWidth("Aacute", 600, null),
			new BuiltinFontWidth("caron", 600, null),
			new BuiltinFontWidth("S", 600, null),
			new BuiltinFontWidth("T", 600, null),
			new BuiltinFontWidth("U", 600, null),
			new BuiltinFontWidth("agrave", 600, null),
			new BuiltinFontWidth("V", 600, null),
			new BuiltinFontWidth("W", 600, null),
			new BuiltinFontWidth("X", 600, null),
			new BuiltinFontWidth("question", 600, null),
			new BuiltinFontWidth("equal", 600, null),
			new BuiltinFontWidth("Y", 600, null),
			new BuiltinFontWidth("Z", 600, null),
			new BuiltinFontWidth("four", 600, null),
			new BuiltinFontWidth("a", 600, null),
			new BuiltinFontWidth("Gcommaaccent", 600, null),
			new BuiltinFontWidth("b", 600, null),
			new BuiltinFontWidth("c", 600, null),
			new BuiltinFontWidth("d", 600, null),
			new BuiltinFontWidth("e", 600, null),
			new BuiltinFontWidth("f", 600, null),
			new BuiltinFontWidth("g", 600, null),
			new BuiltinFontWidth("bullet", 600, null),
			new BuiltinFontWidth("h", 600, null),
			new BuiltinFontWidth("i", 600, null),
			new BuiltinFontWidth("Oslash", 600, null),
			new BuiltinFontWidth("dagger", 600, null),
			new BuiltinFontWidth("j", 600, null),
			new BuiltinFontWidth("k", 600, null),
			new BuiltinFontWidth("l", 600, null),
			new BuiltinFontWidth("m", 600, null),
			new BuiltinFontWidth("n", 600, null),
			new BuiltinFontWidth("tcommaaccent", 600, null),
			new BuiltinFontWidth("o", 600, null),
			new BuiltinFontWidth("ordfeminine", 600, null),
			new BuiltinFontWidth("ring", 600, null),
			new BuiltinFontWidth("p", 600, null),
			new BuiltinFontWidth("q", 600, null),
			new BuiltinFontWidth("uhungarumlaut", 600, null),
			new BuiltinFontWidth("r", 600, null),
			new BuiltinFontWidth("twosuperior", 600, null),
			new BuiltinFontWidth("aacute", 600, null),
			new BuiltinFontWidth("s", 600, null),
			new BuiltinFontWidth("OE", 600, null),
			new BuiltinFontWidth("t", 600, null),
			new BuiltinFontWidth("divide", 600, null),
			new BuiltinFontWidth("u", 600, null),
			new BuiltinFontWidth("Ccaron", 600, null),
			new BuiltinFontWidth("v", 600, null),
			new BuiltinFontWidth("w", 600, null),
			new BuiltinFontWidth("x", 600, null),
			new BuiltinFontWidth("y", 600, null),
			new BuiltinFontWidth("z", 600, null),
			new BuiltinFontWidth("Gbreve", 600, null),
			new BuiltinFontWidth("commaaccent", 600, null),
			new BuiltinFontWidth("hungarumlaut", 600, null),
			new BuiltinFontWidth("Idotaccent", 600, null),
			new BuiltinFontWidth("Nacute", 600, null),
			new BuiltinFontWidth("quotedbl", 600, null),
			new BuiltinFontWidth("gcommaaccent", 600, null),
			new BuiltinFontWidth("mu", 600, null),
			new BuiltinFontWidth("greaterequal", 600, null),
			new BuiltinFontWidth("Scaron", 600, null),
			new BuiltinFontWidth("Lslash", 600, null),
			new BuiltinFontWidth("semicolon", 600, null),
			new BuiltinFontWidth("oslash", 600, null),
			new BuiltinFontWidth("lessequal", 600, null),
			new BuiltinFontWidth("lozenge", 600, null),
			new BuiltinFontWidth("parenright", 600, null),
			new BuiltinFontWidth("ccaron", 600, null),
			new BuiltinFontWidth("Ecircumflex", 600, null),
			new BuiltinFontWidth("gbreve", 600, null),
			new BuiltinFontWidth("trademark", 600, null),
			new BuiltinFontWidth("daggerdbl", 600, null),
			new BuiltinFontWidth("nacute", 600, null),
			new BuiltinFontWidth("macron", 600, null),
			new BuiltinFontWidth("Otilde", 600, null),
			new BuiltinFontWidth("Emacron", 600, null),
			new BuiltinFontWidth("ellipsis", 600, null),
			new BuiltinFontWidth("scaron", 600, null),
			new BuiltinFontWidth("AE", 600, null),
			new BuiltinFontWidth("Ucircumflex", 600, null),
			new BuiltinFontWidth("lslash", 600, null),
			new BuiltinFontWidth("quotedblleft", 600, null),
			new BuiltinFontWidth("guilsinglright", 600, null),
			new BuiltinFontWidth("hyphen", 600, null),
			new BuiltinFontWidth("quotesingle", 600, null),
			new BuiltinFontWidth("eight", 600, null),
			new BuiltinFontWidth("exclamdown", 600, null),
			new BuiltinFontWidth("endash", 600, null),
			new BuiltinFontWidth("oe", 600, null),
			new BuiltinFontWidth("Abreve", 600, null),
			new BuiltinFontWidth("Umacron", 600, null),
			new BuiltinFontWidth("ecircumflex", 600, null),
			new BuiltinFontWidth("Adieresis", 600, null),
			new BuiltinFontWidth("copyright", 600, null),
			new BuiltinFontWidth("Egrave", 600, null),
			new BuiltinFontWidth("slash", 600, null),
			new BuiltinFontWidth("Edieresis", 600, null),
			new BuiltinFontWidth("otilde", 600, null),
			new BuiltinFontWidth("Idieresis", 600, null),
			new BuiltinFontWidth("parenleft", 600, null),
			new BuiltinFontWidth("one", 600, null),
			new BuiltinFontWidth("emacron", 600, null),
			new BuiltinFontWidth("Odieresis", 600, null),
			new BuiltinFontWidth("ucircumflex", 600, null),
			new BuiltinFontWidth("bracketleft", 600, null),
			new BuiltinFontWidth("Ugrave", 600, null),
			new BuiltinFontWidth("quoteright", 600, null),
			new BuiltinFontWidth("Udieresis", 600, null),
			new BuiltinFontWidth("perthousand", 600, null),
			new BuiltinFontWidth("Ydieresis", 600, null),
			new BuiltinFontWidth("umacron", 600, null),
			new BuiltinFontWidth("abreve", 600, null),
			new BuiltinFontWidth("Eacute", 600, null),
			new BuiltinFontWidth("adieresis", 600, null),
			new BuiltinFontWidth("egrave", 600, null),
			new BuiltinFontWidth("edieresis", 600, null),
			new BuiltinFontWidth("idieresis", 600, null),
			new BuiltinFontWidth("Eth", 600, null),
			new BuiltinFontWidth("ae", 600, null),
			new BuiltinFontWidth("asterisk", 600, null),
			new BuiltinFontWidth("odieresis", 600, null),
			new BuiltinFontWidth("Uacute", 600, null),
			new BuiltinFontWidth("ugrave", 600, null),
			new BuiltinFontWidth("nine", 600, null),
			new BuiltinFontWidth("five", 600, null),
			new BuiltinFontWidth("udieresis", 600, null),
			new BuiltinFontWidth("Zcaron", 600, null),
			new BuiltinFontWidth("Scommaaccent", 600, null),
			new BuiltinFontWidth("threequarters", 600, null),
			new BuiltinFontWidth("guillemotright", 600, null),
			new BuiltinFontWidth("Ccedilla", 600, null),
			new BuiltinFontWidth("ydieresis", 600, null),
			new BuiltinFontWidth("tilde", 600, null),
			new BuiltinFontWidth("at", 600, null),
			new BuiltinFontWidth("eacute", 600, null),
			new BuiltinFontWidth("underscore", 600, null),
			new BuiltinFontWidth("Euro", 600, null),
			new BuiltinFontWidth("Dcroat", 600, null),
			new BuiltinFontWidth("multiply", 600, null),
			new BuiltinFontWidth("zero", 600, null),
			new BuiltinFontWidth("eth", 600, null),
			new BuiltinFontWidth("Scedilla", 600, null),
			new BuiltinFontWidth("Ograve", 600, null),
			new BuiltinFontWidth("Racute", 600, null),
			new BuiltinFontWidth("partialdiff", 600, null),
			new BuiltinFontWidth("uacute", 600, null),
			new BuiltinFontWidth("braceleft", 600, null),
			new BuiltinFontWidth("Thorn", 600, null),
			new BuiltinFontWidth("zcaron", 600, null),
			new BuiltinFontWidth("scommaaccent", 600, null),
			new BuiltinFontWidth("ccedilla", 600, null),
			new BuiltinFontWidth("Dcaron", 600, null),
			new BuiltinFontWidth("dcroat", 600, null),
			new BuiltinFontWidth("Ocircumflex", 600, null),
			new BuiltinFontWidth("Oacute", 600, null),
			new BuiltinFontWidth("scedilla", 600, null),
			new BuiltinFontWidth("ogonek", 600, null),
			new BuiltinFontWidth("ograve", 600, null),
			new BuiltinFontWidth("racute", 600, null),
			new BuiltinFontWidth("Tcaron", 600, null),
			new BuiltinFontWidth("Eogonek", 600, null),
			new BuiltinFontWidth("thorn", 600, null),
			new BuiltinFontWidth("degree", 600, null),
			new BuiltinFontWidth("registered", 600, null),
			new BuiltinFontWidth("radical", 600, null),
			new BuiltinFontWidth("Aring", 600, null),
			new BuiltinFontWidth("percent", 600, null),
			new BuiltinFontWidth("six", 600, null),
			new BuiltinFontWidth("paragraph", 600, null),
			new BuiltinFontWidth("dcaron", 600, null),
			new BuiltinFontWidth("Uogonek", 600, null),
			new BuiltinFontWidth("two", 600, null),
			new BuiltinFontWidth("summation", 600, null),
			new BuiltinFontWidth("Igrave", 600, null),
			new BuiltinFontWidth("Lacute", 600, null),
			new BuiltinFontWidth("ocircumflex", 600, null),
			new BuiltinFontWidth("oacute", 600, null),
			new BuiltinFontWidth("Uring", 600, null),
			new BuiltinFontWidth("Lcommaaccent", 600, null),
			new BuiltinFontWidth("tcaron", 600, null),
			new BuiltinFontWidth("eogonek", 600, null),
			new BuiltinFontWidth("Delta", 600, null),
			new BuiltinFontWidth("Ohungarumlaut", 600, null),
			new BuiltinFontWidth("asciicircum", 600, null),
			new BuiltinFontWidth("aring", 600, null),
			new BuiltinFontWidth("grave", 600, null),
			new BuiltinFontWidth("uogonek", 600, null),
			new BuiltinFontWidth("bracketright", 600, null),
			new BuiltinFontWidth("Iacute", 600, null),
			new BuiltinFontWidth("ampersand", 600, null),
			new BuiltinFontWidth("igrave", 600, null),
			new BuiltinFontWidth("lacute", 600, null),
			new BuiltinFontWidth("Ncaron", 600, null),
			new BuiltinFontWidth("plus", 600, null),
			new BuiltinFontWidth("uring", 600, null),
			new BuiltinFontWidth("quotesinglbase", 600, null),
			new BuiltinFontWidth("lcommaaccent", 600, null),
			new BuiltinFontWidth("Yacute", 600, null),
			new BuiltinFontWidth("ohungarumlaut", 600, null),
			new BuiltinFontWidth("threesuperior", 600, null),
			new BuiltinFontWidth("acute", 600, null),
			new BuiltinFontWidth("section", 600, null),
			new BuiltinFontWidth("dieresis", 600, null),
			new BuiltinFontWidth("iacute", 600, null),
			new BuiltinFontWidth("quotedblbase", 600, null),
			new BuiltinFontWidth("ncaron", 600, null),
			new BuiltinFontWidth("florin", 600, null),
			new BuiltinFontWidth("yacute", 600, null),
			new BuiltinFontWidth("Rcommaaccent", 600, null),
			new BuiltinFontWidth("fi", 600, null),
			new BuiltinFontWidth("fl", 600, null),
			new BuiltinFontWidth("Acircumflex", 600, null),
			new BuiltinFontWidth("Cacute", 600, null),
			new BuiltinFontWidth("Icircumflex", 600, null),
			new BuiltinFontWidth("guillemotleft", 600, null),
			new BuiltinFontWidth("germandbls", 600, null),
			new BuiltinFontWidth("Amacron", 600, null),
			new BuiltinFontWidth("seven", 600, null),
			new BuiltinFontWidth("Sacute", 600, null),
			new BuiltinFontWidth("ordmasculine", 600, null),
			new BuiltinFontWidth("dotlessi", 600, null),
			new BuiltinFontWidth("sterling", 600, null),
			new BuiltinFontWidth("notequal", 600, null),
			new BuiltinFontWidth("Imacron", 600, null),
			new BuiltinFontWidth("rcommaaccent", 600, null),
			new BuiltinFontWidth("Zdotaccent", 600, null),
			new BuiltinFontWidth("acircumflex", 600, null),
			new BuiltinFontWidth("cacute", 600, null),
			new BuiltinFontWidth("Ecaron", 600, null),
			new BuiltinFontWidth("icircumflex", 600, null),
			new BuiltinFontWidth("braceright", 600, null),
			new BuiltinFontWidth("quotedblright", 600, null),
			new BuiltinFontWidth("amacron", 600, null),
			new BuiltinFontWidth("sacute", 600, null),
			new BuiltinFontWidth("imacron", 600, null),
			new BuiltinFontWidth("cent", 600, null),
			new BuiltinFontWidth("currency", 600, null),
			new BuiltinFontWidth("logicalnot", 600, null),
			new BuiltinFontWidth("zdotaccent", 600, null),
			new BuiltinFontWidth("Atilde", 600, null),
			new BuiltinFontWidth("breve", 600, null),
			new BuiltinFontWidth("bar", 600, null),
			new BuiltinFontWidth("fraction", 600, null),
			new BuiltinFontWidth("less", 600, null),
			new BuiltinFontWidth("ecaron", 600, null),
			new BuiltinFontWidth("guilsinglleft", 600, null),
			new BuiltinFontWidth("exclam", 600, null),
			new BuiltinFontWidth("period", 600, null),
			new BuiltinFontWidth("Rcaron", 600, null),
			new BuiltinFontWidth("Kcommaaccent", 600, null),
			new BuiltinFontWidth("greater", 600, null),
			new BuiltinFontWidth("atilde", 600, null),
			new BuiltinFontWidth("brokenbar", 600, null),
			new BuiltinFontWidth("quoteleft", 600, null),
			new BuiltinFontWidth("Edotaccent", 600, null),
			new BuiltinFontWidth("onesuperior", 600, null)
		};
		private static BuiltinFontWidth[] helveticaWidthsTab = new BuiltinFontWidth[]
		{
			new BuiltinFontWidth("Ntilde", 722, null),
			new BuiltinFontWidth("rcaron", 333, null),
			new BuiltinFontWidth("kcommaaccent", 500, null),
			new BuiltinFontWidth("Ncommaaccent", 722, null),
			new BuiltinFontWidth("Zacute", 611, null),
			new BuiltinFontWidth("comma", 278, null),
			new BuiltinFontWidth("cedilla", 333, null),
			new BuiltinFontWidth("plusminus", 584, null),
			new BuiltinFontWidth("circumflex", 333, null),
			new BuiltinFontWidth("dotaccent", 333, null),
			new BuiltinFontWidth("edotaccent", 556, null),
			new BuiltinFontWidth("asciitilde", 584, null),
			new BuiltinFontWidth("colon", 278, null),
			new BuiltinFontWidth("onehalf", 834, null),
			new BuiltinFontWidth("dollar", 556, null),
			new BuiltinFontWidth("Lcaron", 556, null),
			new BuiltinFontWidth("ntilde", 556, null),
			new BuiltinFontWidth("Aogonek", 667, null),
			new BuiltinFontWidth("ncommaaccent", 556, null),
			new BuiltinFontWidth("minus", 584, null),
			new BuiltinFontWidth("Iogonek", 278, null),
			new BuiltinFontWidth("zacute", 500, null),
			new BuiltinFontWidth("yen", 556, null),
			new BuiltinFontWidth("space", 278, null),
			new BuiltinFontWidth("Omacron", 778, null),
			new BuiltinFontWidth("questiondown", 611, null),
			new BuiltinFontWidth("emdash", 1000, null),
			new BuiltinFontWidth("Agrave", 667, null),
			new BuiltinFontWidth("three", 556, null),
			new BuiltinFontWidth("numbersign", 556, null),
			new BuiltinFontWidth("lcaron", 299, null),
			new BuiltinFontWidth("A", 667, null),
			new BuiltinFontWidth("B", 667, null),
			new BuiltinFontWidth("C", 722, null),
			new BuiltinFontWidth("aogonek", 556, null),
			new BuiltinFontWidth("D", 722, null),
			new BuiltinFontWidth("E", 667, null),
			new BuiltinFontWidth("onequarter", 834, null),
			new BuiltinFontWidth("F", 611, null),
			new BuiltinFontWidth("G", 778, null),
			new BuiltinFontWidth("H", 722, null),
			new BuiltinFontWidth("I", 278, null),
			new BuiltinFontWidth("J", 500, null),
			new BuiltinFontWidth("K", 667, null),
			new BuiltinFontWidth("iogonek", 222, null),
			new BuiltinFontWidth("backslash", 278, null),
			new BuiltinFontWidth("L", 556, null),
			new BuiltinFontWidth("periodcentered", 278, null),
			new BuiltinFontWidth("M", 833, null),
			new BuiltinFontWidth("N", 722, null),
			new BuiltinFontWidth("omacron", 556, null),
			new BuiltinFontWidth("Tcommaaccent", 611, null),
			new BuiltinFontWidth("O", 778, null),
			new BuiltinFontWidth("P", 667, null),
			new BuiltinFontWidth("Q", 778, null),
			new BuiltinFontWidth("Uhungarumlaut", 722, null),
			new BuiltinFontWidth("R", 722, null),
			new BuiltinFontWidth("Aacute", 667, null),
			new BuiltinFontWidth("caron", 333, null),
			new BuiltinFontWidth("S", 667, null),
			new BuiltinFontWidth("T", 611, null),
			new BuiltinFontWidth("U", 722, null),
			new BuiltinFontWidth("agrave", 556, null),
			new BuiltinFontWidth("V", 667, null),
			new BuiltinFontWidth("W", 944, null),
			new BuiltinFontWidth("X", 667, null),
			new BuiltinFontWidth("question", 556, null),
			new BuiltinFontWidth("equal", 584, null),
			new BuiltinFontWidth("Y", 667, null),
			new BuiltinFontWidth("Z", 611, null),
			new BuiltinFontWidth("four", 556, null),
			new BuiltinFontWidth("a", 556, null),
			new BuiltinFontWidth("Gcommaaccent", 778, null),
			new BuiltinFontWidth("b", 556, null),
			new BuiltinFontWidth("c", 500, null),
			new BuiltinFontWidth("d", 556, null),
			new BuiltinFontWidth("e", 556, null),
			new BuiltinFontWidth("f", 278, null),
			new BuiltinFontWidth("g", 556, null),
			new BuiltinFontWidth("bullet", 350, null),
			new BuiltinFontWidth("h", 556, null),
			new BuiltinFontWidth("i", 222, null),
			new BuiltinFontWidth("Oslash", 778, null),
			new BuiltinFontWidth("dagger", 556, null),
			new BuiltinFontWidth("j", 222, null),
			new BuiltinFontWidth("k", 500, null),
			new BuiltinFontWidth("l", 222, null),
			new BuiltinFontWidth("m", 833, null),
			new BuiltinFontWidth("n", 556, null),
			new BuiltinFontWidth("tcommaaccent", 278, null),
			new BuiltinFontWidth("o", 556, null),
			new BuiltinFontWidth("ordfeminine", 370, null),
			new BuiltinFontWidth("ring", 333, null),
			new BuiltinFontWidth("p", 556, null),
			new BuiltinFontWidth("q", 556, null),
			new BuiltinFontWidth("uhungarumlaut", 556, null),
			new BuiltinFontWidth("r", 333, null),
			new BuiltinFontWidth("twosuperior", 333, null),
			new BuiltinFontWidth("aacute", 556, null),
			new BuiltinFontWidth("s", 500, null),
			new BuiltinFontWidth("OE", 1000, null),
			new BuiltinFontWidth("t", 278, null),
			new BuiltinFontWidth("divide", 584, null),
			new BuiltinFontWidth("u", 556, null),
			new BuiltinFontWidth("Ccaron", 722, null),
			new BuiltinFontWidth("v", 500, null),
			new BuiltinFontWidth("w", 722, null),
			new BuiltinFontWidth("x", 500, null),
			new BuiltinFontWidth("y", 500, null),
			new BuiltinFontWidth("z", 500, null),
			new BuiltinFontWidth("Gbreve", 778, null),
			new BuiltinFontWidth("commaaccent", 250, null),
			new BuiltinFontWidth("hungarumlaut", 333, null),
			new BuiltinFontWidth("Idotaccent", 278, null),
			new BuiltinFontWidth("Nacute", 722, null),
			new BuiltinFontWidth("quotedbl", 355, null),
			new BuiltinFontWidth("gcommaaccent", 556, null),
			new BuiltinFontWidth("mu", 556, null),
			new BuiltinFontWidth("greaterequal", 549, null),
			new BuiltinFontWidth("Scaron", 667, null),
			new BuiltinFontWidth("Lslash", 556, null),
			new BuiltinFontWidth("semicolon", 278, null),
			new BuiltinFontWidth("oslash", 611, null),
			new BuiltinFontWidth("lessequal", 549, null),
			new BuiltinFontWidth("lozenge", 471, null),
			new BuiltinFontWidth("parenright", 333, null),
			new BuiltinFontWidth("ccaron", 500, null),
			new BuiltinFontWidth("Ecircumflex", 667, null),
			new BuiltinFontWidth("gbreve", 556, null),
			new BuiltinFontWidth("trademark", 1000, null),
			new BuiltinFontWidth("daggerdbl", 556, null),
			new BuiltinFontWidth("nacute", 556, null),
			new BuiltinFontWidth("macron", 333, null),
			new BuiltinFontWidth("Otilde", 778, null),
			new BuiltinFontWidth("Emacron", 667, null),
			new BuiltinFontWidth("ellipsis", 1000, null),
			new BuiltinFontWidth("scaron", 500, null),
			new BuiltinFontWidth("AE", 1000, null),
			new BuiltinFontWidth("Ucircumflex", 722, null),
			new BuiltinFontWidth("lslash", 222, null),
			new BuiltinFontWidth("quotedblleft", 333, null),
			new BuiltinFontWidth("guilsinglright", 333, null),
			new BuiltinFontWidth("hyphen", 333, null),
			new BuiltinFontWidth("quotesingle", 191, null),
			new BuiltinFontWidth("eight", 556, null),
			new BuiltinFontWidth("exclamdown", 333, null),
			new BuiltinFontWidth("endash", 556, null),
			new BuiltinFontWidth("oe", 944, null),
			new BuiltinFontWidth("Abreve", 667, null),
			new BuiltinFontWidth("Umacron", 722, null),
			new BuiltinFontWidth("ecircumflex", 556, null),
			new BuiltinFontWidth("Adieresis", 667, null),
			new BuiltinFontWidth("copyright", 737, null),
			new BuiltinFontWidth("Egrave", 667, null),
			new BuiltinFontWidth("slash", 278, null),
			new BuiltinFontWidth("Edieresis", 667, null),
			new BuiltinFontWidth("otilde", 556, null),
			new BuiltinFontWidth("Idieresis", 278, null),
			new BuiltinFontWidth("parenleft", 333, null),
			new BuiltinFontWidth("one", 556, null),
			new BuiltinFontWidth("emacron", 556, null),
			new BuiltinFontWidth("Odieresis", 778, null),
			new BuiltinFontWidth("ucircumflex", 556, null),
			new BuiltinFontWidth("bracketleft", 278, null),
			new BuiltinFontWidth("Ugrave", 722, null),
			new BuiltinFontWidth("quoteright", 222, null),
			new BuiltinFontWidth("Udieresis", 722, null),
			new BuiltinFontWidth("perthousand", 1000, null),
			new BuiltinFontWidth("Ydieresis", 667, null),
			new BuiltinFontWidth("umacron", 556, null),
			new BuiltinFontWidth("abreve", 556, null),
			new BuiltinFontWidth("Eacute", 667, null),
			new BuiltinFontWidth("adieresis", 556, null),
			new BuiltinFontWidth("egrave", 556, null),
			new BuiltinFontWidth("edieresis", 556, null),
			new BuiltinFontWidth("idieresis", 278, null),
			new BuiltinFontWidth("Eth", 722, null),
			new BuiltinFontWidth("ae", 889, null),
			new BuiltinFontWidth("asterisk", 389, null),
			new BuiltinFontWidth("odieresis", 556, null),
			new BuiltinFontWidth("Uacute", 722, null),
			new BuiltinFontWidth("ugrave", 556, null),
			new BuiltinFontWidth("nine", 556, null),
			new BuiltinFontWidth("five", 556, null),
			new BuiltinFontWidth("udieresis", 556, null),
			new BuiltinFontWidth("Zcaron", 611, null),
			new BuiltinFontWidth("Scommaaccent", 667, null),
			new BuiltinFontWidth("threequarters", 834, null),
			new BuiltinFontWidth("guillemotright", 556, null),
			new BuiltinFontWidth("Ccedilla", 722, null),
			new BuiltinFontWidth("ydieresis", 500, null),
			new BuiltinFontWidth("tilde", 333, null),
			new BuiltinFontWidth("at", 1015, null),
			new BuiltinFontWidth("eacute", 556, null),
			new BuiltinFontWidth("underscore", 556, null),
			new BuiltinFontWidth("Euro", 556, null),
			new BuiltinFontWidth("Dcroat", 722, null),
			new BuiltinFontWidth("multiply", 584, null),
			new BuiltinFontWidth("zero", 556, null),
			new BuiltinFontWidth("eth", 556, null),
			new BuiltinFontWidth("Scedilla", 667, null),
			new BuiltinFontWidth("Ograve", 778, null),
			new BuiltinFontWidth("Racute", 722, null),
			new BuiltinFontWidth("partialdiff", 476, null),
			new BuiltinFontWidth("uacute", 556, null),
			new BuiltinFontWidth("braceleft", 334, null),
			new BuiltinFontWidth("Thorn", 667, null),
			new BuiltinFontWidth("zcaron", 500, null),
			new BuiltinFontWidth("scommaaccent", 500, null),
			new BuiltinFontWidth("ccedilla", 500, null),
			new BuiltinFontWidth("Dcaron", 722, null),
			new BuiltinFontWidth("dcroat", 556, null),
			new BuiltinFontWidth("Ocircumflex", 778, null),
			new BuiltinFontWidth("Oacute", 778, null),
			new BuiltinFontWidth("scedilla", 500, null),
			new BuiltinFontWidth("ogonek", 333, null),
			new BuiltinFontWidth("ograve", 556, null),
			new BuiltinFontWidth("racute", 333, null),
			new BuiltinFontWidth("Tcaron", 611, null),
			new BuiltinFontWidth("Eogonek", 667, null),
			new BuiltinFontWidth("thorn", 556, null),
			new BuiltinFontWidth("degree", 400, null),
			new BuiltinFontWidth("registered", 737, null),
			new BuiltinFontWidth("radical", 453, null),
			new BuiltinFontWidth("Aring", 667, null),
			new BuiltinFontWidth("percent", 889, null),
			new BuiltinFontWidth("six", 556, null),
			new BuiltinFontWidth("paragraph", 537, null),
			new BuiltinFontWidth("dcaron", 643, null),
			new BuiltinFontWidth("Uogonek", 722, null),
			new BuiltinFontWidth("two", 556, null),
			new BuiltinFontWidth("summation", 600, null),
			new BuiltinFontWidth("Igrave", 278, null),
			new BuiltinFontWidth("Lacute", 556, null),
			new BuiltinFontWidth("ocircumflex", 556, null),
			new BuiltinFontWidth("oacute", 556, null),
			new BuiltinFontWidth("Uring", 722, null),
			new BuiltinFontWidth("Lcommaaccent", 556, null),
			new BuiltinFontWidth("tcaron", 317, null),
			new BuiltinFontWidth("eogonek", 556, null),
			new BuiltinFontWidth("Delta", 612, null),
			new BuiltinFontWidth("Ohungarumlaut", 778, null),
			new BuiltinFontWidth("asciicircum", 469, null),
			new BuiltinFontWidth("aring", 556, null),
			new BuiltinFontWidth("grave", 333, null),
			new BuiltinFontWidth("uogonek", 556, null),
			new BuiltinFontWidth("bracketright", 278, null),
			new BuiltinFontWidth("Iacute", 278, null),
			new BuiltinFontWidth("ampersand", 667, null),
			new BuiltinFontWidth("igrave", 278, null),
			new BuiltinFontWidth("lacute", 222, null),
			new BuiltinFontWidth("Ncaron", 722, null),
			new BuiltinFontWidth("plus", 584, null),
			new BuiltinFontWidth("uring", 556, null),
			new BuiltinFontWidth("quotesinglbase", 222, null),
			new BuiltinFontWidth("lcommaaccent", 222, null),
			new BuiltinFontWidth("Yacute", 667, null),
			new BuiltinFontWidth("ohungarumlaut", 556, null),
			new BuiltinFontWidth("threesuperior", 333, null),
			new BuiltinFontWidth("acute", 333, null),
			new BuiltinFontWidth("section", 556, null),
			new BuiltinFontWidth("dieresis", 333, null),
			new BuiltinFontWidth("iacute", 278, null),
			new BuiltinFontWidth("quotedblbase", 333, null),
			new BuiltinFontWidth("ncaron", 556, null),
			new BuiltinFontWidth("florin", 556, null),
			new BuiltinFontWidth("yacute", 500, null),
			new BuiltinFontWidth("Rcommaaccent", 722, null),
			new BuiltinFontWidth("fi", 500, null),
			new BuiltinFontWidth("fl", 500, null),
			new BuiltinFontWidth("Acircumflex", 667, null),
			new BuiltinFontWidth("Cacute", 722, null),
			new BuiltinFontWidth("Icircumflex", 278, null),
			new BuiltinFontWidth("guillemotleft", 556, null),
			new BuiltinFontWidth("germandbls", 611, null),
			new BuiltinFontWidth("Amacron", 667, null),
			new BuiltinFontWidth("seven", 556, null),
			new BuiltinFontWidth("Sacute", 667, null),
			new BuiltinFontWidth("ordmasculine", 365, null),
			new BuiltinFontWidth("dotlessi", 278, null),
			new BuiltinFontWidth("sterling", 556, null),
			new BuiltinFontWidth("notequal", 549, null),
			new BuiltinFontWidth("Imacron", 278, null),
			new BuiltinFontWidth("rcommaaccent", 333, null),
			new BuiltinFontWidth("Zdotaccent", 611, null),
			new BuiltinFontWidth("acircumflex", 556, null),
			new BuiltinFontWidth("cacute", 500, null),
			new BuiltinFontWidth("Ecaron", 667, null),
			new BuiltinFontWidth("icircumflex", 278, null),
			new BuiltinFontWidth("braceright", 334, null),
			new BuiltinFontWidth("quotedblright", 333, null),
			new BuiltinFontWidth("amacron", 556, null),
			new BuiltinFontWidth("sacute", 500, null),
			new BuiltinFontWidth("imacron", 278, null),
			new BuiltinFontWidth("cent", 556, null),
			new BuiltinFontWidth("currency", 556, null),
			new BuiltinFontWidth("logicalnot", 584, null),
			new BuiltinFontWidth("zdotaccent", 500, null),
			new BuiltinFontWidth("Atilde", 667, null),
			new BuiltinFontWidth("breve", 333, null),
			new BuiltinFontWidth("bar", 260, null),
			new BuiltinFontWidth("fraction", 167, null),
			new BuiltinFontWidth("less", 584, null),
			new BuiltinFontWidth("ecaron", 556, null),
			new BuiltinFontWidth("guilsinglleft", 333, null),
			new BuiltinFontWidth("exclam", 278, null),
			new BuiltinFontWidth("period", 278, null),
			new BuiltinFontWidth("Rcaron", 722, null),
			new BuiltinFontWidth("Kcommaaccent", 667, null),
			new BuiltinFontWidth("greater", 584, null),
			new BuiltinFontWidth("atilde", 556, null),
			new BuiltinFontWidth("brokenbar", 260, null),
			new BuiltinFontWidth("quoteleft", 222, null),
			new BuiltinFontWidth("Edotaccent", 667, null),
			new BuiltinFontWidth("onesuperior", 333, null)
		};
		private static BuiltinFontWidth[] helveticaBoldWidthsTab = new BuiltinFontWidth[]
		{
			new BuiltinFontWidth("Ntilde", 722, null),
			new BuiltinFontWidth("rcaron", 389, null),
			new BuiltinFontWidth("kcommaaccent", 556, null),
			new BuiltinFontWidth("Ncommaaccent", 722, null),
			new BuiltinFontWidth("Zacute", 611, null),
			new BuiltinFontWidth("comma", 278, null),
			new BuiltinFontWidth("cedilla", 333, null),
			new BuiltinFontWidth("plusminus", 584, null),
			new BuiltinFontWidth("circumflex", 333, null),
			new BuiltinFontWidth("dotaccent", 333, null),
			new BuiltinFontWidth("edotaccent", 556, null),
			new BuiltinFontWidth("asciitilde", 584, null),
			new BuiltinFontWidth("colon", 333, null),
			new BuiltinFontWidth("onehalf", 834, null),
			new BuiltinFontWidth("dollar", 556, null),
			new BuiltinFontWidth("Lcaron", 611, null),
			new BuiltinFontWidth("ntilde", 611, null),
			new BuiltinFontWidth("Aogonek", 722, null),
			new BuiltinFontWidth("ncommaaccent", 611, null),
			new BuiltinFontWidth("minus", 584, null),
			new BuiltinFontWidth("Iogonek", 278, null),
			new BuiltinFontWidth("zacute", 500, null),
			new BuiltinFontWidth("yen", 556, null),
			new BuiltinFontWidth("space", 278, null),
			new BuiltinFontWidth("Omacron", 778, null),
			new BuiltinFontWidth("questiondown", 611, null),
			new BuiltinFontWidth("emdash", 1000, null),
			new BuiltinFontWidth("Agrave", 722, null),
			new BuiltinFontWidth("three", 556, null),
			new BuiltinFontWidth("numbersign", 556, null),
			new BuiltinFontWidth("lcaron", 400, null),
			new BuiltinFontWidth("A", 722, null),
			new BuiltinFontWidth("B", 722, null),
			new BuiltinFontWidth("C", 722, null),
			new BuiltinFontWidth("aogonek", 556, null),
			new BuiltinFontWidth("D", 722, null),
			new BuiltinFontWidth("E", 667, null),
			new BuiltinFontWidth("onequarter", 834, null),
			new BuiltinFontWidth("F", 611, null),
			new BuiltinFontWidth("G", 778, null),
			new BuiltinFontWidth("H", 722, null),
			new BuiltinFontWidth("I", 278, null),
			new BuiltinFontWidth("J", 556, null),
			new BuiltinFontWidth("K", 722, null),
			new BuiltinFontWidth("iogonek", 278, null),
			new BuiltinFontWidth("backslash", 278, null),
			new BuiltinFontWidth("L", 611, null),
			new BuiltinFontWidth("periodcentered", 278, null),
			new BuiltinFontWidth("M", 833, null),
			new BuiltinFontWidth("N", 722, null),
			new BuiltinFontWidth("omacron", 611, null),
			new BuiltinFontWidth("Tcommaaccent", 611, null),
			new BuiltinFontWidth("O", 778, null),
			new BuiltinFontWidth("P", 667, null),
			new BuiltinFontWidth("Q", 778, null),
			new BuiltinFontWidth("Uhungarumlaut", 722, null),
			new BuiltinFontWidth("R", 722, null),
			new BuiltinFontWidth("Aacute", 722, null),
			new BuiltinFontWidth("caron", 333, null),
			new BuiltinFontWidth("S", 667, null),
			new BuiltinFontWidth("T", 611, null),
			new BuiltinFontWidth("U", 722, null),
			new BuiltinFontWidth("agrave", 556, null),
			new BuiltinFontWidth("V", 667, null),
			new BuiltinFontWidth("W", 944, null),
			new BuiltinFontWidth("X", 667, null),
			new BuiltinFontWidth("question", 611, null),
			new BuiltinFontWidth("equal", 584, null),
			new BuiltinFontWidth("Y", 667, null),
			new BuiltinFontWidth("Z", 611, null),
			new BuiltinFontWidth("four", 556, null),
			new BuiltinFontWidth("a", 556, null),
			new BuiltinFontWidth("Gcommaaccent", 778, null),
			new BuiltinFontWidth("b", 611, null),
			new BuiltinFontWidth("c", 556, null),
			new BuiltinFontWidth("d", 611, null),
			new BuiltinFontWidth("e", 556, null),
			new BuiltinFontWidth("f", 333, null),
			new BuiltinFontWidth("g", 611, null),
			new BuiltinFontWidth("bullet", 350, null),
			new BuiltinFontWidth("h", 611, null),
			new BuiltinFontWidth("i", 278, null),
			new BuiltinFontWidth("Oslash", 778, null),
			new BuiltinFontWidth("dagger", 556, null),
			new BuiltinFontWidth("j", 278, null),
			new BuiltinFontWidth("k", 556, null),
			new BuiltinFontWidth("l", 278, null),
			new BuiltinFontWidth("m", 889, null),
			new BuiltinFontWidth("n", 611, null),
			new BuiltinFontWidth("tcommaaccent", 333, null),
			new BuiltinFontWidth("o", 611, null),
			new BuiltinFontWidth("ordfeminine", 370, null),
			new BuiltinFontWidth("ring", 333, null),
			new BuiltinFontWidth("p", 611, null),
			new BuiltinFontWidth("q", 611, null),
			new BuiltinFontWidth("uhungarumlaut", 611, null),
			new BuiltinFontWidth("r", 389, null),
			new BuiltinFontWidth("twosuperior", 333, null),
			new BuiltinFontWidth("aacute", 556, null),
			new BuiltinFontWidth("s", 556, null),
			new BuiltinFontWidth("OE", 1000, null),
			new BuiltinFontWidth("t", 333, null),
			new BuiltinFontWidth("divide", 584, null),
			new BuiltinFontWidth("u", 611, null),
			new BuiltinFontWidth("Ccaron", 722, null),
			new BuiltinFontWidth("v", 556, null),
			new BuiltinFontWidth("w", 778, null),
			new BuiltinFontWidth("x", 556, null),
			new BuiltinFontWidth("y", 556, null),
			new BuiltinFontWidth("z", 500, null),
			new BuiltinFontWidth("Gbreve", 778, null),
			new BuiltinFontWidth("commaaccent", 250, null),
			new BuiltinFontWidth("hungarumlaut", 333, null),
			new BuiltinFontWidth("Idotaccent", 278, null),
			new BuiltinFontWidth("Nacute", 722, null),
			new BuiltinFontWidth("quotedbl", 474, null),
			new BuiltinFontWidth("gcommaaccent", 611, null),
			new BuiltinFontWidth("mu", 611, null),
			new BuiltinFontWidth("greaterequal", 549, null),
			new BuiltinFontWidth("Scaron", 667, null),
			new BuiltinFontWidth("Lslash", 611, null),
			new BuiltinFontWidth("semicolon", 333, null),
			new BuiltinFontWidth("oslash", 611, null),
			new BuiltinFontWidth("lessequal", 549, null),
			new BuiltinFontWidth("lozenge", 494, null),
			new BuiltinFontWidth("parenright", 333, null),
			new BuiltinFontWidth("ccaron", 556, null),
			new BuiltinFontWidth("Ecircumflex", 667, null),
			new BuiltinFontWidth("gbreve", 611, null),
			new BuiltinFontWidth("trademark", 1000, null),
			new BuiltinFontWidth("daggerdbl", 556, null),
			new BuiltinFontWidth("nacute", 611, null),
			new BuiltinFontWidth("macron", 333, null),
			new BuiltinFontWidth("Otilde", 778, null),
			new BuiltinFontWidth("Emacron", 667, null),
			new BuiltinFontWidth("ellipsis", 1000, null),
			new BuiltinFontWidth("scaron", 556, null),
			new BuiltinFontWidth("AE", 1000, null),
			new BuiltinFontWidth("Ucircumflex", 722, null),
			new BuiltinFontWidth("lslash", 278, null),
			new BuiltinFontWidth("quotedblleft", 500, null),
			new BuiltinFontWidth("guilsinglright", 333, null),
			new BuiltinFontWidth("hyphen", 333, null),
			new BuiltinFontWidth("quotesingle", 238, null),
			new BuiltinFontWidth("eight", 556, null),
			new BuiltinFontWidth("exclamdown", 333, null),
			new BuiltinFontWidth("endash", 556, null),
			new BuiltinFontWidth("oe", 944, null),
			new BuiltinFontWidth("Abreve", 722, null),
			new BuiltinFontWidth("Umacron", 722, null),
			new BuiltinFontWidth("ecircumflex", 556, null),
			new BuiltinFontWidth("Adieresis", 722, null),
			new BuiltinFontWidth("copyright", 737, null),
			new BuiltinFontWidth("Egrave", 667, null),
			new BuiltinFontWidth("slash", 278, null),
			new BuiltinFontWidth("Edieresis", 667, null),
			new BuiltinFontWidth("otilde", 611, null),
			new BuiltinFontWidth("Idieresis", 278, null),
			new BuiltinFontWidth("parenleft", 333, null),
			new BuiltinFontWidth("one", 556, null),
			new BuiltinFontWidth("emacron", 556, null),
			new BuiltinFontWidth("Odieresis", 778, null),
			new BuiltinFontWidth("ucircumflex", 611, null),
			new BuiltinFontWidth("bracketleft", 333, null),
			new BuiltinFontWidth("Ugrave", 722, null),
			new BuiltinFontWidth("quoteright", 278, null),
			new BuiltinFontWidth("Udieresis", 722, null),
			new BuiltinFontWidth("perthousand", 1000, null),
			new BuiltinFontWidth("Ydieresis", 667, null),
			new BuiltinFontWidth("umacron", 611, null),
			new BuiltinFontWidth("abreve", 556, null),
			new BuiltinFontWidth("Eacute", 667, null),
			new BuiltinFontWidth("adieresis", 556, null),
			new BuiltinFontWidth("egrave", 556, null),
			new BuiltinFontWidth("edieresis", 556, null),
			new BuiltinFontWidth("idieresis", 278, null),
			new BuiltinFontWidth("Eth", 722, null),
			new BuiltinFontWidth("ae", 889, null),
			new BuiltinFontWidth("asterisk", 389, null),
			new BuiltinFontWidth("odieresis", 611, null),
			new BuiltinFontWidth("Uacute", 722, null),
			new BuiltinFontWidth("ugrave", 611, null),
			new BuiltinFontWidth("nine", 556, null),
			new BuiltinFontWidth("five", 556, null),
			new BuiltinFontWidth("udieresis", 611, null),
			new BuiltinFontWidth("Zcaron", 611, null),
			new BuiltinFontWidth("Scommaaccent", 667, null),
			new BuiltinFontWidth("threequarters", 834, null),
			new BuiltinFontWidth("guillemotright", 556, null),
			new BuiltinFontWidth("Ccedilla", 722, null),
			new BuiltinFontWidth("ydieresis", 556, null),
			new BuiltinFontWidth("tilde", 333, null),
			new BuiltinFontWidth("dbldaggerumlaut", 556, null),
			new BuiltinFontWidth("at", 975, null),
			new BuiltinFontWidth("eacute", 556, null),
			new BuiltinFontWidth("underscore", 556, null),
			new BuiltinFontWidth("Euro", 556, null),
			new BuiltinFontWidth("Dcroat", 722, null),
			new BuiltinFontWidth("multiply", 584, null),
			new BuiltinFontWidth("zero", 556, null),
			new BuiltinFontWidth("eth", 611, null),
			new BuiltinFontWidth("Scedilla", 667, null),
			new BuiltinFontWidth("Ograve", 778, null),
			new BuiltinFontWidth("Racute", 722, null),
			new BuiltinFontWidth("partialdiff", 494, null),
			new BuiltinFontWidth("uacute", 611, null),
			new BuiltinFontWidth("braceleft", 389, null),
			new BuiltinFontWidth("Thorn", 667, null),
			new BuiltinFontWidth("zcaron", 500, null),
			new BuiltinFontWidth("scommaaccent", 556, null),
			new BuiltinFontWidth("ccedilla", 556, null),
			new BuiltinFontWidth("Dcaron", 722, null),
			new BuiltinFontWidth("dcroat", 611, null),
			new BuiltinFontWidth("Ocircumflex", 778, null),
			new BuiltinFontWidth("Oacute", 778, null),
			new BuiltinFontWidth("scedilla", 556, null),
			new BuiltinFontWidth("ogonek", 333, null),
			new BuiltinFontWidth("ograve", 611, null),
			new BuiltinFontWidth("racute", 389, null),
			new BuiltinFontWidth("Tcaron", 611, null),
			new BuiltinFontWidth("Eogonek", 667, null),
			new BuiltinFontWidth("thorn", 611, null),
			new BuiltinFontWidth("degree", 400, null),
			new BuiltinFontWidth("registered", 737, null),
			new BuiltinFontWidth("radical", 549, null),
			new BuiltinFontWidth("Aring", 722, null),
			new BuiltinFontWidth("percent", 889, null),
			new BuiltinFontWidth("six", 556, null),
			new BuiltinFontWidth("paragraph", 556, null),
			new BuiltinFontWidth("dcaron", 743, null),
			new BuiltinFontWidth("Uogonek", 722, null),
			new BuiltinFontWidth("two", 556, null),
			new BuiltinFontWidth("summation", 600, null),
			new BuiltinFontWidth("Igrave", 278, null),
			new BuiltinFontWidth("Lacute", 611, null),
			new BuiltinFontWidth("ocircumflex", 611, null),
			new BuiltinFontWidth("oacute", 611, null),
			new BuiltinFontWidth("Uring", 722, null),
			new BuiltinFontWidth("Lcommaaccent", 611, null),
			new BuiltinFontWidth("tcaron", 389, null),
			new BuiltinFontWidth("eogonek", 556, null),
			new BuiltinFontWidth("Delta", 612, null),
			new BuiltinFontWidth("Ohungarumlaut", 778, null),
			new BuiltinFontWidth("asciicircum", 584, null),
			new BuiltinFontWidth("aring", 556, null),
			new BuiltinFontWidth("grave", 333, null),
			new BuiltinFontWidth("uogonek", 611, null),
			new BuiltinFontWidth("bracketright", 333, null),
			new BuiltinFontWidth("Iacute", 278, null),
			new BuiltinFontWidth("ampersand", 722, null),
			new BuiltinFontWidth("igrave", 278, null),
			new BuiltinFontWidth("lacute", 278, null),
			new BuiltinFontWidth("Ncaron", 722, null),
			new BuiltinFontWidth("plus", 584, null),
			new BuiltinFontWidth("uring", 611, null),
			new BuiltinFontWidth("quotesinglbase", 278, null),
			new BuiltinFontWidth("lcommaaccent", 278, null),
			new BuiltinFontWidth("Yacute", 667, null),
			new BuiltinFontWidth("ohungarumlaut", 611, null),
			new BuiltinFontWidth("threesuperior", 333, null),
			new BuiltinFontWidth("acute", 333, null),
			new BuiltinFontWidth("section", 556, null),
			new BuiltinFontWidth("dieresis", 333, null),
			new BuiltinFontWidth("iacute", 278, null),
			new BuiltinFontWidth("quotedblbase", 500, null),
			new BuiltinFontWidth("ncaron", 611, null),
			new BuiltinFontWidth("florin", 556, null),
			new BuiltinFontWidth("yacute", 556, null),
			new BuiltinFontWidth("Rcommaaccent", 722, null),
			new BuiltinFontWidth("fi", 611, null),
			new BuiltinFontWidth("fl", 611, null),
			new BuiltinFontWidth("Acircumflex", 722, null),
			new BuiltinFontWidth("Cacute", 722, null),
			new BuiltinFontWidth("Icircumflex", 278, null),
			new BuiltinFontWidth("guillemotleft", 556, null),
			new BuiltinFontWidth("germandbls", 611, null),
			new BuiltinFontWidth("Amacron", 722, null),
			new BuiltinFontWidth("seven", 556, null),
			new BuiltinFontWidth("Sacute", 667, null),
			new BuiltinFontWidth("ordmasculine", 365, null),
			new BuiltinFontWidth("dotlessi", 278, null),
			new BuiltinFontWidth("sterling", 556, null),
			new BuiltinFontWidth("notequal", 549, null),
			new BuiltinFontWidth("Imacron", 278, null),
			new BuiltinFontWidth("rcommaaccent", 389, null),
			new BuiltinFontWidth("Zdotaccent", 611, null),
			new BuiltinFontWidth("acircumflex", 556, null),
			new BuiltinFontWidth("cacute", 556, null),
			new BuiltinFontWidth("Ecaron", 667, null),
			new BuiltinFontWidth("icircumflex", 278, null),
			new BuiltinFontWidth("braceright", 389, null),
			new BuiltinFontWidth("quotedblright", 500, null),
			new BuiltinFontWidth("amacron", 556, null),
			new BuiltinFontWidth("sacute", 556, null),
			new BuiltinFontWidth("imacron", 278, null),
			new BuiltinFontWidth("cent", 556, null),
			new BuiltinFontWidth("currency", 556, null),
			new BuiltinFontWidth("logicalnot", 584, null),
			new BuiltinFontWidth("zdotaccent", 500, null),
			new BuiltinFontWidth("Atilde", 722, null),
			new BuiltinFontWidth("breve", 333, null),
			new BuiltinFontWidth("bar", 280, null),
			new BuiltinFontWidth("fraction", 167, null),
			new BuiltinFontWidth("less", 584, null),
			new BuiltinFontWidth("ecaron", 556, null),
			new BuiltinFontWidth("guilsinglleft", 333, null),
			new BuiltinFontWidth("exclam", 333, null),
			new BuiltinFontWidth("period", 278, null),
			new BuiltinFontWidth("Rcaron", 722, null),
			new BuiltinFontWidth("Kcommaaccent", 722, null),
			new BuiltinFontWidth("greater", 584, null),
			new BuiltinFontWidth("atilde", 556, null),
			new BuiltinFontWidth("brokenbar", 280, null),
			new BuiltinFontWidth("quoteleft", 278, null),
			new BuiltinFontWidth("Edotaccent", 667, null),
			new BuiltinFontWidth("onesuperior", 333, null)
		};
		private static BuiltinFontWidth[] helveticaBoldObliqueWidthsTab = new BuiltinFontWidth[]
		{
			new BuiltinFontWidth("Ntilde", 722, null),
			new BuiltinFontWidth("rcaron", 389, null),
			new BuiltinFontWidth("kcommaaccent", 556, null),
			new BuiltinFontWidth("Ncommaaccent", 722, null),
			new BuiltinFontWidth("Zacute", 611, null),
			new BuiltinFontWidth("comma", 278, null),
			new BuiltinFontWidth("cedilla", 333, null),
			new BuiltinFontWidth("plusminus", 584, null),
			new BuiltinFontWidth("circumflex", 333, null),
			new BuiltinFontWidth("dotaccent", 333, null),
			new BuiltinFontWidth("edotaccent", 556, null),
			new BuiltinFontWidth("asciitilde", 584, null),
			new BuiltinFontWidth("colon", 333, null),
			new BuiltinFontWidth("onehalf", 834, null),
			new BuiltinFontWidth("dollar", 556, null),
			new BuiltinFontWidth("Lcaron", 611, null),
			new BuiltinFontWidth("ntilde", 611, null),
			new BuiltinFontWidth("Aogonek", 722, null),
			new BuiltinFontWidth("ncommaaccent", 611, null),
			new BuiltinFontWidth("minus", 584, null),
			new BuiltinFontWidth("Iogonek", 278, null),
			new BuiltinFontWidth("zacute", 500, null),
			new BuiltinFontWidth("yen", 556, null),
			new BuiltinFontWidth("space", 278, null),
			new BuiltinFontWidth("Omacron", 778, null),
			new BuiltinFontWidth("questiondown", 611, null),
			new BuiltinFontWidth("emdash", 1000, null),
			new BuiltinFontWidth("Agrave", 722, null),
			new BuiltinFontWidth("three", 556, null),
			new BuiltinFontWidth("numbersign", 556, null),
			new BuiltinFontWidth("lcaron", 400, null),
			new BuiltinFontWidth("A", 722, null),
			new BuiltinFontWidth("B", 722, null),
			new BuiltinFontWidth("C", 722, null),
			new BuiltinFontWidth("aogonek", 556, null),
			new BuiltinFontWidth("D", 722, null),
			new BuiltinFontWidth("E", 667, null),
			new BuiltinFontWidth("onequarter", 834, null),
			new BuiltinFontWidth("F", 611, null),
			new BuiltinFontWidth("G", 778, null),
			new BuiltinFontWidth("H", 722, null),
			new BuiltinFontWidth("I", 278, null),
			new BuiltinFontWidth("J", 556, null),
			new BuiltinFontWidth("K", 722, null),
			new BuiltinFontWidth("iogonek", 278, null),
			new BuiltinFontWidth("backslash", 278, null),
			new BuiltinFontWidth("L", 611, null),
			new BuiltinFontWidth("periodcentered", 278, null),
			new BuiltinFontWidth("M", 833, null),
			new BuiltinFontWidth("N", 722, null),
			new BuiltinFontWidth("omacron", 611, null),
			new BuiltinFontWidth("Tcommaaccent", 611, null),
			new BuiltinFontWidth("O", 778, null),
			new BuiltinFontWidth("P", 667, null),
			new BuiltinFontWidth("Q", 778, null),
			new BuiltinFontWidth("Uhungarumlaut", 722, null),
			new BuiltinFontWidth("R", 722, null),
			new BuiltinFontWidth("Aacute", 722, null),
			new BuiltinFontWidth("caron", 333, null),
			new BuiltinFontWidth("S", 667, null),
			new BuiltinFontWidth("T", 611, null),
			new BuiltinFontWidth("U", 722, null),
			new BuiltinFontWidth("agrave", 556, null),
			new BuiltinFontWidth("V", 667, null),
			new BuiltinFontWidth("W", 944, null),
			new BuiltinFontWidth("X", 667, null),
			new BuiltinFontWidth("question", 611, null),
			new BuiltinFontWidth("equal", 584, null),
			new BuiltinFontWidth("Y", 667, null),
			new BuiltinFontWidth("Z", 611, null),
			new BuiltinFontWidth("four", 556, null),
			new BuiltinFontWidth("a", 556, null),
			new BuiltinFontWidth("Gcommaaccent", 778, null),
			new BuiltinFontWidth("b", 611, null),
			new BuiltinFontWidth("c", 556, null),
			new BuiltinFontWidth("d", 611, null),
			new BuiltinFontWidth("e", 556, null),
			new BuiltinFontWidth("f", 333, null),
			new BuiltinFontWidth("g", 611, null),
			new BuiltinFontWidth("bullet", 350, null),
			new BuiltinFontWidth("h", 611, null),
			new BuiltinFontWidth("i", 278, null),
			new BuiltinFontWidth("Oslash", 778, null),
			new BuiltinFontWidth("dagger", 556, null),
			new BuiltinFontWidth("j", 278, null),
			new BuiltinFontWidth("k", 556, null),
			new BuiltinFontWidth("l", 278, null),
			new BuiltinFontWidth("m", 889, null),
			new BuiltinFontWidth("n", 611, null),
			new BuiltinFontWidth("tcommaaccent", 333, null),
			new BuiltinFontWidth("o", 611, null),
			new BuiltinFontWidth("ordfeminine", 370, null),
			new BuiltinFontWidth("ring", 333, null),
			new BuiltinFontWidth("p", 611, null),
			new BuiltinFontWidth("q", 611, null),
			new BuiltinFontWidth("uhungarumlaut", 611, null),
			new BuiltinFontWidth("r", 389, null),
			new BuiltinFontWidth("twosuperior", 333, null),
			new BuiltinFontWidth("aacute", 556, null),
			new BuiltinFontWidth("s", 556, null),
			new BuiltinFontWidth("OE", 1000, null),
			new BuiltinFontWidth("t", 333, null),
			new BuiltinFontWidth("divide", 584, null),
			new BuiltinFontWidth("u", 611, null),
			new BuiltinFontWidth("Ccaron", 722, null),
			new BuiltinFontWidth("v", 556, null),
			new BuiltinFontWidth("w", 778, null),
			new BuiltinFontWidth("x", 556, null),
			new BuiltinFontWidth("y", 556, null),
			new BuiltinFontWidth("z", 500, null),
			new BuiltinFontWidth("Gbreve", 778, null),
			new BuiltinFontWidth("commaaccent", 250, null),
			new BuiltinFontWidth("hungarumlaut", 333, null),
			new BuiltinFontWidth("Idotaccent", 278, null),
			new BuiltinFontWidth("Nacute", 722, null),
			new BuiltinFontWidth("quotedbl", 474, null),
			new BuiltinFontWidth("gcommaaccent", 611, null),
			new BuiltinFontWidth("mu", 611, null),
			new BuiltinFontWidth("greaterequal", 549, null),
			new BuiltinFontWidth("Scaron", 667, null),
			new BuiltinFontWidth("Lslash", 611, null),
			new BuiltinFontWidth("semicolon", 333, null),
			new BuiltinFontWidth("oslash", 611, null),
			new BuiltinFontWidth("lessequal", 549, null),
			new BuiltinFontWidth("lozenge", 494, null),
			new BuiltinFontWidth("parenright", 333, null),
			new BuiltinFontWidth("ccaron", 556, null),
			new BuiltinFontWidth("Ecircumflex", 667, null),
			new BuiltinFontWidth("gbreve", 611, null),
			new BuiltinFontWidth("trademark", 1000, null),
			new BuiltinFontWidth("daggerdbl", 556, null),
			new BuiltinFontWidth("nacute", 611, null),
			new BuiltinFontWidth("macron", 333, null),
			new BuiltinFontWidth("Otilde", 778, null),
			new BuiltinFontWidth("Emacron", 667, null),
			new BuiltinFontWidth("ellipsis", 1000, null),
			new BuiltinFontWidth("scaron", 556, null),
			new BuiltinFontWidth("AE", 1000, null),
			new BuiltinFontWidth("Ucircumflex", 722, null),
			new BuiltinFontWidth("lslash", 278, null),
			new BuiltinFontWidth("quotedblleft", 500, null),
			new BuiltinFontWidth("guilsinglright", 333, null),
			new BuiltinFontWidth("hyphen", 333, null),
			new BuiltinFontWidth("quotesingle", 238, null),
			new BuiltinFontWidth("eight", 556, null),
			new BuiltinFontWidth("exclamdown", 333, null),
			new BuiltinFontWidth("endash", 556, null),
			new BuiltinFontWidth("oe", 944, null),
			new BuiltinFontWidth("Abreve", 722, null),
			new BuiltinFontWidth("Umacron", 722, null),
			new BuiltinFontWidth("ecircumflex", 556, null),
			new BuiltinFontWidth("Adieresis", 722, null),
			new BuiltinFontWidth("copyright", 737, null),
			new BuiltinFontWidth("Egrave", 667, null),
			new BuiltinFontWidth("slash", 278, null),
			new BuiltinFontWidth("Edieresis", 667, null),
			new BuiltinFontWidth("otilde", 611, null),
			new BuiltinFontWidth("Idieresis", 278, null),
			new BuiltinFontWidth("parenleft", 333, null),
			new BuiltinFontWidth("one", 556, null),
			new BuiltinFontWidth("emacron", 556, null),
			new BuiltinFontWidth("Odieresis", 778, null),
			new BuiltinFontWidth("ucircumflex", 611, null),
			new BuiltinFontWidth("bracketleft", 333, null),
			new BuiltinFontWidth("Ugrave", 722, null),
			new BuiltinFontWidth("quoteright", 278, null),
			new BuiltinFontWidth("Udieresis", 722, null),
			new BuiltinFontWidth("perthousand", 1000, null),
			new BuiltinFontWidth("Ydieresis", 667, null),
			new BuiltinFontWidth("umacron", 611, null),
			new BuiltinFontWidth("abreve", 556, null),
			new BuiltinFontWidth("Eacute", 667, null),
			new BuiltinFontWidth("adieresis", 556, null),
			new BuiltinFontWidth("egrave", 556, null),
			new BuiltinFontWidth("edieresis", 556, null),
			new BuiltinFontWidth("idieresis", 278, null),
			new BuiltinFontWidth("Eth", 722, null),
			new BuiltinFontWidth("ae", 889, null),
			new BuiltinFontWidth("asterisk", 389, null),
			new BuiltinFontWidth("odieresis", 611, null),
			new BuiltinFontWidth("Uacute", 722, null),
			new BuiltinFontWidth("ugrave", 611, null),
			new BuiltinFontWidth("nine", 556, null),
			new BuiltinFontWidth("five", 556, null),
			new BuiltinFontWidth("udieresis", 611, null),
			new BuiltinFontWidth("Zcaron", 611, null),
			new BuiltinFontWidth("Scommaaccent", 667, null),
			new BuiltinFontWidth("threequarters", 834, null),
			new BuiltinFontWidth("guillemotright", 556, null),
			new BuiltinFontWidth("Ccedilla", 722, null),
			new BuiltinFontWidth("ydieresis", 556, null),
			new BuiltinFontWidth("tilde", 333, null),
			new BuiltinFontWidth("at", 975, null),
			new BuiltinFontWidth("eacute", 556, null),
			new BuiltinFontWidth("underscore", 556, null),
			new BuiltinFontWidth("Euro", 556, null),
			new BuiltinFontWidth("Dcroat", 722, null),
			new BuiltinFontWidth("multiply", 584, null),
			new BuiltinFontWidth("zero", 556, null),
			new BuiltinFontWidth("eth", 611, null),
			new BuiltinFontWidth("Scedilla", 667, null),
			new BuiltinFontWidth("Ograve", 778, null),
			new BuiltinFontWidth("Racute", 722, null),
			new BuiltinFontWidth("partialdiff", 494, null),
			new BuiltinFontWidth("uacute", 611, null),
			new BuiltinFontWidth("braceleft", 389, null),
			new BuiltinFontWidth("Thorn", 667, null),
			new BuiltinFontWidth("zcaron", 500, null),
			new BuiltinFontWidth("scommaaccent", 556, null),
			new BuiltinFontWidth("ccedilla", 556, null),
			new BuiltinFontWidth("Dcaron", 722, null),
			new BuiltinFontWidth("dcroat", 611, null),
			new BuiltinFontWidth("Ocircumflex", 778, null),
			new BuiltinFontWidth("Oacute", 778, null),
			new BuiltinFontWidth("scedilla", 556, null),
			new BuiltinFontWidth("ogonek", 333, null),
			new BuiltinFontWidth("ograve", 611, null),
			new BuiltinFontWidth("racute", 389, null),
			new BuiltinFontWidth("Tcaron", 611, null),
			new BuiltinFontWidth("Eogonek", 667, null),
			new BuiltinFontWidth("thorn", 611, null),
			new BuiltinFontWidth("degree", 400, null),
			new BuiltinFontWidth("registered", 737, null),
			new BuiltinFontWidth("radical", 549, null),
			new BuiltinFontWidth("Aring", 722, null),
			new BuiltinFontWidth("percent", 889, null),
			new BuiltinFontWidth("six", 556, null),
			new BuiltinFontWidth("paragraph", 556, null),
			new BuiltinFontWidth("dcaron", 743, null),
			new BuiltinFontWidth("Uogonek", 722, null),
			new BuiltinFontWidth("two", 556, null),
			new BuiltinFontWidth("summation", 600, null),
			new BuiltinFontWidth("Igrave", 278, null),
			new BuiltinFontWidth("Lacute", 611, null),
			new BuiltinFontWidth("ocircumflex", 611, null),
			new BuiltinFontWidth("oacute", 611, null),
			new BuiltinFontWidth("Uring", 722, null),
			new BuiltinFontWidth("Lcommaaccent", 611, null),
			new BuiltinFontWidth("tcaron", 389, null),
			new BuiltinFontWidth("eogonek", 556, null),
			new BuiltinFontWidth("Delta", 612, null),
			new BuiltinFontWidth("Ohungarumlaut", 778, null),
			new BuiltinFontWidth("asciicircum", 584, null),
			new BuiltinFontWidth("aring", 556, null),
			new BuiltinFontWidth("grave", 333, null),
			new BuiltinFontWidth("uogonek", 611, null),
			new BuiltinFontWidth("bracketright", 333, null),
			new BuiltinFontWidth("Iacute", 278, null),
			new BuiltinFontWidth("ampersand", 722, null),
			new BuiltinFontWidth("igrave", 278, null),
			new BuiltinFontWidth("lacute", 278, null),
			new BuiltinFontWidth("Ncaron", 722, null),
			new BuiltinFontWidth("plus", 584, null),
			new BuiltinFontWidth("uring", 611, null),
			new BuiltinFontWidth("quotesinglbase", 278, null),
			new BuiltinFontWidth("lcommaaccent", 278, null),
			new BuiltinFontWidth("Yacute", 667, null),
			new BuiltinFontWidth("ohungarumlaut", 611, null),
			new BuiltinFontWidth("threesuperior", 333, null),
			new BuiltinFontWidth("acute", 333, null),
			new BuiltinFontWidth("section", 556, null),
			new BuiltinFontWidth("dieresis", 333, null),
			new BuiltinFontWidth("iacute", 278, null),
			new BuiltinFontWidth("quotedblbase", 500, null),
			new BuiltinFontWidth("ncaron", 611, null),
			new BuiltinFontWidth("florin", 556, null),
			new BuiltinFontWidth("yacute", 556, null),
			new BuiltinFontWidth("Rcommaaccent", 722, null),
			new BuiltinFontWidth("fi", 611, null),
			new BuiltinFontWidth("fl", 611, null),
			new BuiltinFontWidth("Acircumflex", 722, null),
			new BuiltinFontWidth("Cacute", 722, null),
			new BuiltinFontWidth("Icircumflex", 278, null),
			new BuiltinFontWidth("guillemotleft", 556, null),
			new BuiltinFontWidth("germandbls", 611, null),
			new BuiltinFontWidth("Amacron", 722, null),
			new BuiltinFontWidth("seven", 556, null),
			new BuiltinFontWidth("Sacute", 667, null),
			new BuiltinFontWidth("ordmasculine", 365, null),
			new BuiltinFontWidth("dotlessi", 278, null),
			new BuiltinFontWidth("sterling", 556, null),
			new BuiltinFontWidth("notequal", 549, null),
			new BuiltinFontWidth("Imacron", 278, null),
			new BuiltinFontWidth("rcommaaccent", 389, null),
			new BuiltinFontWidth("Zdotaccent", 611, null),
			new BuiltinFontWidth("acircumflex", 556, null),
			new BuiltinFontWidth("cacute", 556, null),
			new BuiltinFontWidth("Ecaron", 667, null),
			new BuiltinFontWidth("icircumflex", 278, null),
			new BuiltinFontWidth("braceright", 389, null),
			new BuiltinFontWidth("quotedblright", 500, null),
			new BuiltinFontWidth("amacron", 556, null),
			new BuiltinFontWidth("sacute", 556, null),
			new BuiltinFontWidth("imacron", 278, null),
			new BuiltinFontWidth("cent", 556, null),
			new BuiltinFontWidth("currency", 556, null),
			new BuiltinFontWidth("logicalnot", 584, null),
			new BuiltinFontWidth("zdotaccent", 500, null),
			new BuiltinFontWidth("Atilde", 722, null),
			new BuiltinFontWidth("breve", 333, null),
			new BuiltinFontWidth("bar", 280, null),
			new BuiltinFontWidth("fraction", 167, null),
			new BuiltinFontWidth("less", 584, null),
			new BuiltinFontWidth("ecaron", 556, null),
			new BuiltinFontWidth("guilsinglleft", 333, null),
			new BuiltinFontWidth("exclam", 333, null),
			new BuiltinFontWidth("period", 278, null),
			new BuiltinFontWidth("Rcaron", 722, null),
			new BuiltinFontWidth("Kcommaaccent", 722, null),
			new BuiltinFontWidth("greater", 584, null),
			new BuiltinFontWidth("atilde", 556, null),
			new BuiltinFontWidth("brokenbar", 280, null),
			new BuiltinFontWidth("quoteleft", 278, null),
			new BuiltinFontWidth("Edotaccent", 667, null),
			new BuiltinFontWidth("onesuperior", 333, null)
		};
		private static BuiltinFontWidth[] helveticaObliqueWidthsTab = new BuiltinFontWidth[]
		{
			new BuiltinFontWidth("Ntilde", 722, null),
			new BuiltinFontWidth("rcaron", 333, null),
			new BuiltinFontWidth("kcommaaccent", 500, null),
			new BuiltinFontWidth("Ncommaaccent", 722, null),
			new BuiltinFontWidth("Zacute", 611, null),
			new BuiltinFontWidth("comma", 278, null),
			new BuiltinFontWidth("cedilla", 333, null),
			new BuiltinFontWidth("plusminus", 584, null),
			new BuiltinFontWidth("circumflex", 333, null),
			new BuiltinFontWidth("dotaccent", 333, null),
			new BuiltinFontWidth("edotaccent", 556, null),
			new BuiltinFontWidth("asciitilde", 584, null),
			new BuiltinFontWidth("colon", 278, null),
			new BuiltinFontWidth("onehalf", 834, null),
			new BuiltinFontWidth("dollar", 556, null),
			new BuiltinFontWidth("Lcaron", 556, null),
			new BuiltinFontWidth("ntilde", 556, null),
			new BuiltinFontWidth("Aogonek", 667, null),
			new BuiltinFontWidth("ncommaaccent", 556, null),
			new BuiltinFontWidth("minus", 584, null),
			new BuiltinFontWidth("Iogonek", 278, null),
			new BuiltinFontWidth("zacute", 500, null),
			new BuiltinFontWidth("yen", 556, null),
			new BuiltinFontWidth("space", 278, null),
			new BuiltinFontWidth("Omacron", 778, null),
			new BuiltinFontWidth("questiondown", 611, null),
			new BuiltinFontWidth("emdash", 1000, null),
			new BuiltinFontWidth("Agrave", 667, null),
			new BuiltinFontWidth("three", 556, null),
			new BuiltinFontWidth("numbersign", 556, null),
			new BuiltinFontWidth("lcaron", 299, null),
			new BuiltinFontWidth("A", 667, null),
			new BuiltinFontWidth("B", 667, null),
			new BuiltinFontWidth("C", 722, null),
			new BuiltinFontWidth("aogonek", 556, null),
			new BuiltinFontWidth("D", 722, null),
			new BuiltinFontWidth("E", 667, null),
			new BuiltinFontWidth("onequarter", 834, null),
			new BuiltinFontWidth("F", 611, null),
			new BuiltinFontWidth("G", 778, null),
			new BuiltinFontWidth("H", 722, null),
			new BuiltinFontWidth("I", 278, null),
			new BuiltinFontWidth("J", 500, null),
			new BuiltinFontWidth("K", 667, null),
			new BuiltinFontWidth("iogonek", 222, null),
			new BuiltinFontWidth("backslash", 278, null),
			new BuiltinFontWidth("L", 556, null),
			new BuiltinFontWidth("periodcentered", 278, null),
			new BuiltinFontWidth("M", 833, null),
			new BuiltinFontWidth("N", 722, null),
			new BuiltinFontWidth("omacron", 556, null),
			new BuiltinFontWidth("Tcommaaccent", 611, null),
			new BuiltinFontWidth("O", 778, null),
			new BuiltinFontWidth("P", 667, null),
			new BuiltinFontWidth("Q", 778, null),
			new BuiltinFontWidth("Uhungarumlaut", 722, null),
			new BuiltinFontWidth("R", 722, null),
			new BuiltinFontWidth("Aacute", 667, null),
			new BuiltinFontWidth("caron", 333, null),
			new BuiltinFontWidth("S", 667, null),
			new BuiltinFontWidth("T", 611, null),
			new BuiltinFontWidth("U", 722, null),
			new BuiltinFontWidth("agrave", 556, null),
			new BuiltinFontWidth("V", 667, null),
			new BuiltinFontWidth("W", 944, null),
			new BuiltinFontWidth("X", 667, null),
			new BuiltinFontWidth("question", 556, null),
			new BuiltinFontWidth("equal", 584, null),
			new BuiltinFontWidth("Y", 667, null),
			new BuiltinFontWidth("Z", 611, null),
			new BuiltinFontWidth("four", 556, null),
			new BuiltinFontWidth("a", 556, null),
			new BuiltinFontWidth("Gcommaaccent", 778, null),
			new BuiltinFontWidth("b", 556, null),
			new BuiltinFontWidth("c", 500, null),
			new BuiltinFontWidth("d", 556, null),
			new BuiltinFontWidth("e", 556, null),
			new BuiltinFontWidth("f", 278, null),
			new BuiltinFontWidth("g", 556, null),
			new BuiltinFontWidth("bullet", 350, null),
			new BuiltinFontWidth("h", 556, null),
			new BuiltinFontWidth("i", 222, null),
			new BuiltinFontWidth("Oslash", 778, null),
			new BuiltinFontWidth("dagger", 556, null),
			new BuiltinFontWidth("j", 222, null),
			new BuiltinFontWidth("k", 500, null),
			new BuiltinFontWidth("l", 222, null),
			new BuiltinFontWidth("m", 833, null),
			new BuiltinFontWidth("n", 556, null),
			new BuiltinFontWidth("tcommaaccent", 278, null),
			new BuiltinFontWidth("o", 556, null),
			new BuiltinFontWidth("ordfeminine", 370, null),
			new BuiltinFontWidth("ring", 333, null),
			new BuiltinFontWidth("p", 556, null),
			new BuiltinFontWidth("q", 556, null),
			new BuiltinFontWidth("uhungarumlaut", 556, null),
			new BuiltinFontWidth("r", 333, null),
			new BuiltinFontWidth("twosuperior", 333, null),
			new BuiltinFontWidth("aacute", 556, null),
			new BuiltinFontWidth("s", 500, null),
			new BuiltinFontWidth("OE", 1000, null),
			new BuiltinFontWidth("t", 278, null),
			new BuiltinFontWidth("divide", 584, null),
			new BuiltinFontWidth("u", 556, null),
			new BuiltinFontWidth("Ccaron", 722, null),
			new BuiltinFontWidth("v", 500, null),
			new BuiltinFontWidth("w", 722, null),
			new BuiltinFontWidth("x", 500, null),
			new BuiltinFontWidth("y", 500, null),
			new BuiltinFontWidth("z", 500, null),
			new BuiltinFontWidth("Gbreve", 778, null),
			new BuiltinFontWidth("commaaccent", 250, null),
			new BuiltinFontWidth("hungarumlaut", 333, null),
			new BuiltinFontWidth("Idotaccent", 278, null),
			new BuiltinFontWidth("Nacute", 722, null),
			new BuiltinFontWidth("quotedbl", 355, null),
			new BuiltinFontWidth("gcommaaccent", 556, null),
			new BuiltinFontWidth("mu", 556, null),
			new BuiltinFontWidth("greaterequal", 549, null),
			new BuiltinFontWidth("Scaron", 667, null),
			new BuiltinFontWidth("Lslash", 556, null),
			new BuiltinFontWidth("semicolon", 278, null),
			new BuiltinFontWidth("oslash", 611, null),
			new BuiltinFontWidth("lessequal", 549, null),
			new BuiltinFontWidth("lozenge", 471, null),
			new BuiltinFontWidth("parenright", 333, null),
			new BuiltinFontWidth("ccaron", 500, null),
			new BuiltinFontWidth("Ecircumflex", 667, null),
			new BuiltinFontWidth("gbreve", 556, null),
			new BuiltinFontWidth("trademark", 1000, null),
			new BuiltinFontWidth("daggerdbl", 556, null),
			new BuiltinFontWidth("nacute", 556, null),
			new BuiltinFontWidth("macron", 333, null),
			new BuiltinFontWidth("Otilde", 778, null),
			new BuiltinFontWidth("Emacron", 667, null),
			new BuiltinFontWidth("ellipsis", 1000, null),
			new BuiltinFontWidth("scaron", 500, null),
			new BuiltinFontWidth("AE", 1000, null),
			new BuiltinFontWidth("Ucircumflex", 722, null),
			new BuiltinFontWidth("lslash", 222, null),
			new BuiltinFontWidth("quotedblleft", 333, null),
			new BuiltinFontWidth("guilsinglright", 333, null),
			new BuiltinFontWidth("hyphen", 333, null),
			new BuiltinFontWidth("quotesingle", 191, null),
			new BuiltinFontWidth("eight", 556, null),
			new BuiltinFontWidth("exclamdown", 333, null),
			new BuiltinFontWidth("endash", 556, null),
			new BuiltinFontWidth("oe", 944, null),
			new BuiltinFontWidth("Abreve", 667, null),
			new BuiltinFontWidth("Umacron", 722, null),
			new BuiltinFontWidth("ecircumflex", 556, null),
			new BuiltinFontWidth("Adieresis", 667, null),
			new BuiltinFontWidth("copyright", 737, null),
			new BuiltinFontWidth("Egrave", 667, null),
			new BuiltinFontWidth("slash", 278, null),
			new BuiltinFontWidth("Edieresis", 667, null),
			new BuiltinFontWidth("otilde", 556, null),
			new BuiltinFontWidth("Idieresis", 278, null),
			new BuiltinFontWidth("parenleft", 333, null),
			new BuiltinFontWidth("one", 556, null),
			new BuiltinFontWidth("emacron", 556, null),
			new BuiltinFontWidth("Odieresis", 778, null),
			new BuiltinFontWidth("ucircumflex", 556, null),
			new BuiltinFontWidth("bracketleft", 278, null),
			new BuiltinFontWidth("Ugrave", 722, null),
			new BuiltinFontWidth("quoteright", 222, null),
			new BuiltinFontWidth("Udieresis", 722, null),
			new BuiltinFontWidth("perthousand", 1000, null),
			new BuiltinFontWidth("Ydieresis", 667, null),
			new BuiltinFontWidth("umacron", 556, null),
			new BuiltinFontWidth("abreve", 556, null),
			new BuiltinFontWidth("Eacute", 667, null),
			new BuiltinFontWidth("adieresis", 556, null),
			new BuiltinFontWidth("egrave", 556, null),
			new BuiltinFontWidth("edieresis", 556, null),
			new BuiltinFontWidth("idieresis", 278, null),
			new BuiltinFontWidth("Eth", 722, null),
			new BuiltinFontWidth("ae", 889, null),
			new BuiltinFontWidth("asterisk", 389, null),
			new BuiltinFontWidth("odieresis", 556, null),
			new BuiltinFontWidth("Uacute", 722, null),
			new BuiltinFontWidth("ugrave", 556, null),
			new BuiltinFontWidth("nine", 556, null),
			new BuiltinFontWidth("five", 556, null),
			new BuiltinFontWidth("udieresis", 556, null),
			new BuiltinFontWidth("Zcaron", 611, null),
			new BuiltinFontWidth("Scommaaccent", 667, null),
			new BuiltinFontWidth("threequarters", 834, null),
			new BuiltinFontWidth("guillemotright", 556, null),
			new BuiltinFontWidth("Ccedilla", 722, null),
			new BuiltinFontWidth("ydieresis", 500, null),
			new BuiltinFontWidth("tilde", 333, null),
			new BuiltinFontWidth("at", 1015, null),
			new BuiltinFontWidth("eacute", 556, null),
			new BuiltinFontWidth("underscore", 556, null),
			new BuiltinFontWidth("Euro", 556, null),
			new BuiltinFontWidth("Dcroat", 722, null),
			new BuiltinFontWidth("multiply", 584, null),
			new BuiltinFontWidth("zero", 556, null),
			new BuiltinFontWidth("eth", 556, null),
			new BuiltinFontWidth("Scedilla", 667, null),
			new BuiltinFontWidth("Ograve", 778, null),
			new BuiltinFontWidth("Racute", 722, null),
			new BuiltinFontWidth("partialdiff", 476, null),
			new BuiltinFontWidth("uacute", 556, null),
			new BuiltinFontWidth("braceleft", 334, null),
			new BuiltinFontWidth("Thorn", 667, null),
			new BuiltinFontWidth("zcaron", 500, null),
			new BuiltinFontWidth("scommaaccent", 500, null),
			new BuiltinFontWidth("ccedilla", 500, null),
			new BuiltinFontWidth("Dcaron", 722, null),
			new BuiltinFontWidth("dcroat", 556, null),
			new BuiltinFontWidth("Ocircumflex", 778, null),
			new BuiltinFontWidth("Oacute", 778, null),
			new BuiltinFontWidth("scedilla", 500, null),
			new BuiltinFontWidth("ogonek", 333, null),
			new BuiltinFontWidth("ograve", 556, null),
			new BuiltinFontWidth("racute", 333, null),
			new BuiltinFontWidth("Tcaron", 611, null),
			new BuiltinFontWidth("Eogonek", 667, null),
			new BuiltinFontWidth("thorn", 556, null),
			new BuiltinFontWidth("degree", 400, null),
			new BuiltinFontWidth("registered", 737, null),
			new BuiltinFontWidth("radical", 453, null),
			new BuiltinFontWidth("Aring", 667, null),
			new BuiltinFontWidth("percent", 889, null),
			new BuiltinFontWidth("six", 556, null),
			new BuiltinFontWidth("paragraph", 537, null),
			new BuiltinFontWidth("dcaron", 643, null),
			new BuiltinFontWidth("Uogonek", 722, null),
			new BuiltinFontWidth("two", 556, null),
			new BuiltinFontWidth("summation", 600, null),
			new BuiltinFontWidth("Igrave", 278, null),
			new BuiltinFontWidth("Lacute", 556, null),
			new BuiltinFontWidth("ocircumflex", 556, null),
			new BuiltinFontWidth("oacute", 556, null),
			new BuiltinFontWidth("Uring", 722, null),
			new BuiltinFontWidth("Lcommaaccent", 556, null),
			new BuiltinFontWidth("tcaron", 317, null),
			new BuiltinFontWidth("eogonek", 556, null),
			new BuiltinFontWidth("Delta", 612, null),
			new BuiltinFontWidth("Ohungarumlaut", 778, null),
			new BuiltinFontWidth("asciicircum", 469, null),
			new BuiltinFontWidth("aring", 556, null),
			new BuiltinFontWidth("grave", 333, null),
			new BuiltinFontWidth("uogonek", 556, null),
			new BuiltinFontWidth("bracketright", 278, null),
			new BuiltinFontWidth("Iacute", 278, null),
			new BuiltinFontWidth("ampersand", 667, null),
			new BuiltinFontWidth("igrave", 278, null),
			new BuiltinFontWidth("lacute", 222, null),
			new BuiltinFontWidth("Ncaron", 722, null),
			new BuiltinFontWidth("plus", 584, null),
			new BuiltinFontWidth("uring", 556, null),
			new BuiltinFontWidth("quotesinglbase", 222, null),
			new BuiltinFontWidth("lcommaaccent", 222, null),
			new BuiltinFontWidth("Yacute", 667, null),
			new BuiltinFontWidth("ohungarumlaut", 556, null),
			new BuiltinFontWidth("threesuperior", 333, null),
			new BuiltinFontWidth("acute", 333, null),
			new BuiltinFontWidth("section", 556, null),
			new BuiltinFontWidth("dieresis", 333, null),
			new BuiltinFontWidth("iacute", 278, null),
			new BuiltinFontWidth("quotedblbase", 333, null),
			new BuiltinFontWidth("ncaron", 556, null),
			new BuiltinFontWidth("florin", 556, null),
			new BuiltinFontWidth("yacute", 500, null),
			new BuiltinFontWidth("Rcommaaccent", 722, null),
			new BuiltinFontWidth("fi", 500, null),
			new BuiltinFontWidth("fl", 500, null),
			new BuiltinFontWidth("Acircumflex", 667, null),
			new BuiltinFontWidth("Cacute", 722, null),
			new BuiltinFontWidth("Icircumflex", 278, null),
			new BuiltinFontWidth("guillemotleft", 556, null),
			new BuiltinFontWidth("germandbls", 611, null),
			new BuiltinFontWidth("Amacron", 667, null),
			new BuiltinFontWidth("seven", 556, null),
			new BuiltinFontWidth("Sacute", 667, null),
			new BuiltinFontWidth("ordmasculine", 365, null),
			new BuiltinFontWidth("dotlessi", 278, null),
			new BuiltinFontWidth("sterling", 556, null),
			new BuiltinFontWidth("notequal", 549, null),
			new BuiltinFontWidth("Imacron", 278, null),
			new BuiltinFontWidth("rcommaaccent", 333, null),
			new BuiltinFontWidth("Zdotaccent", 611, null),
			new BuiltinFontWidth("acircumflex", 556, null),
			new BuiltinFontWidth("cacute", 500, null),
			new BuiltinFontWidth("Ecaron", 667, null),
			new BuiltinFontWidth("icircumflex", 278, null),
			new BuiltinFontWidth("braceright", 334, null),
			new BuiltinFontWidth("quotedblright", 333, null),
			new BuiltinFontWidth("amacron", 556, null),
			new BuiltinFontWidth("sacute", 500, null),
			new BuiltinFontWidth("imacron", 278, null),
			new BuiltinFontWidth("cent", 556, null),
			new BuiltinFontWidth("currency", 556, null),
			new BuiltinFontWidth("logicalnot", 584, null),
			new BuiltinFontWidth("zdotaccent", 500, null),
			new BuiltinFontWidth("Atilde", 667, null),
			new BuiltinFontWidth("breve", 333, null),
			new BuiltinFontWidth("bar", 260, null),
			new BuiltinFontWidth("fraction", 167, null),
			new BuiltinFontWidth("less", 584, null),
			new BuiltinFontWidth("ecaron", 556, null),
			new BuiltinFontWidth("guilsinglleft", 333, null),
			new BuiltinFontWidth("exclam", 278, null),
			new BuiltinFontWidth("period", 278, null),
			new BuiltinFontWidth("Rcaron", 722, null),
			new BuiltinFontWidth("Kcommaaccent", 667, null),
			new BuiltinFontWidth("greater", 584, null),
			new BuiltinFontWidth("atilde", 556, null),
			new BuiltinFontWidth("brokenbar", 260, null),
			new BuiltinFontWidth("quoteleft", 222, null),
			new BuiltinFontWidth("Edotaccent", 667, null),
			new BuiltinFontWidth("onesuperior", 333, null)
		};
		private static BuiltinFontWidth[] symbolWidthsTab = new BuiltinFontWidth[]
		{
			new BuiltinFontWidth("bracketleftex", 384, null),
			new BuiltinFontWidth("alpha", 631, null),
			new BuiltinFontWidth("union", 768, null),
			new BuiltinFontWidth("infinity", 713, null),
			new BuiltinFontWidth("comma", 250, null),
			new BuiltinFontWidth("copyrightsans", 790, null),
			new BuiltinFontWidth("plusminus", 549, null),
			new BuiltinFontWidth("arrowup", 603, null),
			new BuiltinFontWidth("apple", 790, null),
			new BuiltinFontWidth("parenleftbt", 384, null),
			new BuiltinFontWidth("notelement", 713, null),
			new BuiltinFontWidth("colon", 278, null),
			new BuiltinFontWidth("beta", 549, null),
			new BuiltinFontWidth("braceleftbt", 494, null),
			new BuiltinFontWidth("Lambda", 686, null),
			new BuiltinFontWidth("Phi", 763, null),
			new BuiltinFontWidth("minus", 549, null),
			new BuiltinFontWidth("space", 250, null),
			new BuiltinFontWidth("Sigma", 592, null),
			new BuiltinFontWidth("approxequal", 549, null),
			new BuiltinFontWidth("minute", 247, null),
			new BuiltinFontWidth("circleplus", 768, null),
			new BuiltinFontWidth("Omicron", 722, null),
			new BuiltinFontWidth("three", 500, null),
			new BuiltinFontWidth("numbersign", 500, null),
			new BuiltinFontWidth("lambda", 549, null),
			new BuiltinFontWidth("phi", 521, null),
			new BuiltinFontWidth("aleph", 823, null),
			new BuiltinFontWidth("Tau", 611, null),
			new BuiltinFontWidth("spade", 753, null),
			new BuiltinFontWidth("logicaland", 603, null),
			new BuiltinFontWidth("sigma", 603, null),
			new BuiltinFontWidth("propersuperset", 713, null),
			new BuiltinFontWidth("omicron", 549, null),
			new BuiltinFontWidth("question", 444, null),
			new BuiltinFontWidth("equal", 549, null),
			new BuiltinFontWidth("Epsilon", 611, null),
			new BuiltinFontWidth("emptyset", 823, null),
			new BuiltinFontWidth("diamond", 753, null),
			new BuiltinFontWidth("four", 500, null),
			new BuiltinFontWidth("Mu", 889, null),
			new BuiltinFontWidth("parenlefttp", 384, null),
			new BuiltinFontWidth("club", 753, null),
			new BuiltinFontWidth("bullet", 460, null),
			new BuiltinFontWidth("Omega", 768, null),
			new BuiltinFontWidth("tau", 439, null),
			new BuiltinFontWidth("Upsilon", 690, null),
			new BuiltinFontWidth("bracelefttp", 494, null),
			new BuiltinFontWidth("heart", 753, null),
			new BuiltinFontWidth("divide", 549, null),
			new BuiltinFontWidth("epsilon", 439, null),
			new BuiltinFontWidth("logicalor", 603, null),
			new BuiltinFontWidth("parenleftex", 384, null),
			new BuiltinFontWidth("greaterequal", 549, null),
			new BuiltinFontWidth("mu", 576, null),
			new BuiltinFontWidth("Nu", 722, null),
			new BuiltinFontWidth("therefore", 863, null),
			new BuiltinFontWidth("notsubset", 713, null),
			new BuiltinFontWidth("omega", 686, null),
			new BuiltinFontWidth("semicolon", 278, null),
			new BuiltinFontWidth("element", 713, null),
			new BuiltinFontWidth("upsilon", 576, null),
			new BuiltinFontWidth("existential", 549, null),
			new BuiltinFontWidth("integralbt", 686, null),
			new BuiltinFontWidth("lessequal", 549, null),
			new BuiltinFontWidth("phi1", 603, null),
			new BuiltinFontWidth("lozenge", 494, null),
			new BuiltinFontWidth("trademarkserif", 890, null),
			new BuiltinFontWidth("parenright", 333, null),
			new BuiltinFontWidth("reflexsuperset", 713, null),
			new BuiltinFontWidth("sigma1", 439, null),
			new BuiltinFontWidth("nu", 521, null),
			new BuiltinFontWidth("Gamma", 603, null),
			new BuiltinFontWidth("angleright", 329, null),
			new BuiltinFontWidth("ellipsis", 1000, null),
			new BuiltinFontWidth("Rho", 556, null),
			new BuiltinFontWidth("parenrightbt", 384, null),
			new BuiltinFontWidth("radicalex", 500, null),
			new BuiltinFontWidth("eight", 500, null),
			new BuiltinFontWidth("angleleft", 329, null),
			new BuiltinFontWidth("arrowdbldown", 603, null),
			new BuiltinFontWidth("congruent", 549, null),
			new BuiltinFontWidth("Theta", 741, null),
			new BuiltinFontWidth("intersection", 768, null),
			new BuiltinFontWidth("Pi", 768, null),
			new BuiltinFontWidth("slash", 278, null),
			new BuiltinFontWidth("registerserif", 790, null),
			new BuiltinFontWidth("parenleft", 333, null),
			new BuiltinFontWidth("one", 500, null),
			new BuiltinFontWidth("gamma", 411, null),
			new BuiltinFontWidth("bracketleft", 333, null),
			new BuiltinFontWidth("rho", 549, null),
			new BuiltinFontWidth("circlemultiply", 768, null),
			new BuiltinFontWidth("Chi", 722, null),
			new BuiltinFontWidth("theta", 521, null),
			new BuiltinFontWidth("pi", 549, null),
			new BuiltinFontWidth("integraltp", 686, null),
			new BuiltinFontWidth("Eta", 722, null),
			new BuiltinFontWidth("product", 823, null),
			new BuiltinFontWidth("nine", 500, null),
			new BuiltinFontWidth("five", 500, null),
			new BuiltinFontWidth("propersubset", 713, null),
			new BuiltinFontWidth("bracketrightbt", 384, null),
			new BuiltinFontWidth("trademarksans", 786, null),
			new BuiltinFontWidth("dotmath", 250, null),
			new BuiltinFontWidth("integralex", 686, null),
			new BuiltinFontWidth("chi", 549, null),
			new BuiltinFontWidth("parenrighttp", 384, null),
			new BuiltinFontWidth("eta", 603, null),
			new BuiltinFontWidth("underscore", 500, null),
			new BuiltinFontWidth("Euro", 750, null),
			new BuiltinFontWidth("multiply", 549, null),
			new BuiltinFontWidth("zero", 500, null),
			new BuiltinFontWidth("partialdiff", 494, null),
			new BuiltinFontWidth("angle", 768, null),
			new BuiltinFontWidth("arrowdblleft", 987, null),
			new BuiltinFontWidth("braceleft", 480, null),
			new BuiltinFontWidth("parenrightex", 384, null),
			new BuiltinFontWidth("Rfraktur", 795, null),
			new BuiltinFontWidth("Zeta", 611, null),
			new BuiltinFontWidth("braceex", 494, null),
			new BuiltinFontWidth("arrowdblup", 603, null),
			new BuiltinFontWidth("arrowdown", 603, null),
			new BuiltinFontWidth("Ifraktur", 686, null),
			new BuiltinFontWidth("degree", 400, null),
			new BuiltinFontWidth("Iota", 333, null),
			new BuiltinFontWidth("perpendicular", 658, null),
			new BuiltinFontWidth("radical", 549, null),
			new BuiltinFontWidth("asteriskmath", 500, null),
			new BuiltinFontWidth("percent", 833, null),
			new BuiltinFontWidth("zeta", 494, null),
			new BuiltinFontWidth("six", 500, null),
			new BuiltinFontWidth("two", 500, null),
			new BuiltinFontWidth("weierstrass", 987, null),
			new BuiltinFontWidth("summation", 713, null),
			new BuiltinFontWidth("bracketrighttp", 384, null),
			new BuiltinFontWidth("carriagereturn", 658, null),
			new BuiltinFontWidth("suchthat", 439, null),
			new BuiltinFontWidth("arrowvertex", 603, null),
			new BuiltinFontWidth("Delta", 612, null),
			new BuiltinFontWidth("iota", 329, null),
			new BuiltinFontWidth("arrowhorizex", 1000, null),
			new BuiltinFontWidth("bracketrightex", 384, null),
			new BuiltinFontWidth("bracketright", 333, null),
			new BuiltinFontWidth("ampersand", 778, null),
			new BuiltinFontWidth("plus", 549, null),
			new BuiltinFontWidth("proportional", 713, null),
			new BuiltinFontWidth("delta", 494, null),
			new BuiltinFontWidth("copyrightserif", 790, null),
			new BuiltinFontWidth("bracerightmid", 494, null),
			new BuiltinFontWidth("arrowleft", 987, null),
			new BuiltinFontWidth("second", 411, null),
			new BuiltinFontWidth("arrowdblboth", 1042, null),
			new BuiltinFontWidth("florin", 500, null),
			new BuiltinFontWidth("Psi", 795, null),
			new BuiltinFontWidth("bracerightbt", 494, null),
			new BuiltinFontWidth("bracketleftbt", 384, null),
			new BuiltinFontWidth("seven", 500, null),
			new BuiltinFontWidth("braceleftmid", 494, null),
			new BuiltinFontWidth("notequal", 549, null),
			new BuiltinFontWidth("psi", 686, null),
			new BuiltinFontWidth("equivalence", 549, null),
			new BuiltinFontWidth("universal", 713, null),
			new BuiltinFontWidth("arrowdblright", 987, null),
			new BuiltinFontWidth("braceright", 480, null),
			new BuiltinFontWidth("reflexsubset", 713, null),
			new BuiltinFontWidth("Xi", 645, null),
			new BuiltinFontWidth("theta1", 631, null),
			new BuiltinFontWidth("logicalnot", 713, null),
			new BuiltinFontWidth("Kappa", 722, null),
			new BuiltinFontWidth("similar", 549, null),
			new BuiltinFontWidth("bar", 200, null),
			new BuiltinFontWidth("fraction", 167, null),
			new BuiltinFontWidth("less", 549, null),
			new BuiltinFontWidth("registersans", 790, null),
			new BuiltinFontWidth("omega1", 713, null),
			new BuiltinFontWidth("exclam", 333, null),
			new BuiltinFontWidth("Upsilon1", 620, null),
			new BuiltinFontWidth("bracerighttp", 494, null),
			new BuiltinFontWidth("xi", 493, null),
			new BuiltinFontWidth("period", 250, null),
			new BuiltinFontWidth("Alpha", 722, null),
			new BuiltinFontWidth("arrowright", 987, null),
			new BuiltinFontWidth("greater", 549, null),
			new BuiltinFontWidth("bracketlefttp", 384, null),
			new BuiltinFontWidth("kappa", 549, null),
			new BuiltinFontWidth("gradient", 713, null),
			new BuiltinFontWidth("integral", 274, null),
			new BuiltinFontWidth("arrowboth", 1042, null),
			new BuiltinFontWidth("Beta", 667, null)
		};
		private static BuiltinFontWidth[] timesBoldWidthsTab = new BuiltinFontWidth[]
		{
			new BuiltinFontWidth("Ntilde", 722, null),
			new BuiltinFontWidth("rcaron", 444, null),
			new BuiltinFontWidth("kcommaaccent", 556, null),
			new BuiltinFontWidth("Ncommaaccent", 722, null),
			new BuiltinFontWidth("Zacute", 667, null),
			new BuiltinFontWidth("comma", 250, null),
			new BuiltinFontWidth("cedilla", 333, null),
			new BuiltinFontWidth("plusminus", 570, null),
			new BuiltinFontWidth("circumflex", 333, null),
			new BuiltinFontWidth("dotaccent", 333, null),
			new BuiltinFontWidth("edotaccent", 444, null),
			new BuiltinFontWidth("asciitilde", 520, null),
			new BuiltinFontWidth("colon", 333, null),
			new BuiltinFontWidth("onehalf", 750, null),
			new BuiltinFontWidth("dollar", 500, null),
			new BuiltinFontWidth("Lcaron", 667, null),
			new BuiltinFontWidth("ntilde", 556, null),
			new BuiltinFontWidth("Aogonek", 722, null),
			new BuiltinFontWidth("ncommaaccent", 556, null),
			new BuiltinFontWidth("minus", 570, null),
			new BuiltinFontWidth("Iogonek", 389, null),
			new BuiltinFontWidth("zacute", 444, null),
			new BuiltinFontWidth("yen", 500, null),
			new BuiltinFontWidth("space", 250, null),
			new BuiltinFontWidth("Omacron", 778, null),
			new BuiltinFontWidth("questiondown", 500, null),
			new BuiltinFontWidth("emdash", 1000, null),
			new BuiltinFontWidth("Agrave", 722, null),
			new BuiltinFontWidth("three", 500, null),
			new BuiltinFontWidth("numbersign", 500, null),
			new BuiltinFontWidth("lcaron", 394, null),
			new BuiltinFontWidth("A", 722, null),
			new BuiltinFontWidth("B", 667, null),
			new BuiltinFontWidth("C", 722, null),
			new BuiltinFontWidth("aogonek", 500, null),
			new BuiltinFontWidth("D", 722, null),
			new BuiltinFontWidth("E", 667, null),
			new BuiltinFontWidth("onequarter", 750, null),
			new BuiltinFontWidth("F", 611, null),
			new BuiltinFontWidth("G", 778, null),
			new BuiltinFontWidth("H", 778, null),
			new BuiltinFontWidth("I", 389, null),
			new BuiltinFontWidth("J", 500, null),
			new BuiltinFontWidth("K", 778, null),
			new BuiltinFontWidth("iogonek", 278, null),
			new BuiltinFontWidth("backslash", 278, null),
			new BuiltinFontWidth("L", 667, null),
			new BuiltinFontWidth("periodcentered", 250, null),
			new BuiltinFontWidth("M", 944, null),
			new BuiltinFontWidth("N", 722, null),
			new BuiltinFontWidth("omacron", 500, null),
			new BuiltinFontWidth("Tcommaaccent", 667, null),
			new BuiltinFontWidth("O", 778, null),
			new BuiltinFontWidth("P", 611, null),
			new BuiltinFontWidth("Q", 778, null),
			new BuiltinFontWidth("Uhungarumlaut", 722, null),
			new BuiltinFontWidth("R", 722, null),
			new BuiltinFontWidth("Aacute", 722, null),
			new BuiltinFontWidth("caron", 333, null),
			new BuiltinFontWidth("S", 556, null),
			new BuiltinFontWidth("T", 667, null),
			new BuiltinFontWidth("U", 722, null),
			new BuiltinFontWidth("agrave", 500, null),
			new BuiltinFontWidth("V", 722, null),
			new BuiltinFontWidth("W", 1000, null),
			new BuiltinFontWidth("X", 722, null),
			new BuiltinFontWidth("question", 500, null),
			new BuiltinFontWidth("equal", 570, null),
			new BuiltinFontWidth("Y", 722, null),
			new BuiltinFontWidth("Z", 667, null),
			new BuiltinFontWidth("four", 500, null),
			new BuiltinFontWidth("a", 500, null),
			new BuiltinFontWidth("Gcommaaccent", 778, null),
			new BuiltinFontWidth("b", 556, null),
			new BuiltinFontWidth("c", 444, null),
			new BuiltinFontWidth("d", 556, null),
			new BuiltinFontWidth("e", 444, null),
			new BuiltinFontWidth("f", 333, null),
			new BuiltinFontWidth("g", 500, null),
			new BuiltinFontWidth("bullet", 350, null),
			new BuiltinFontWidth("h", 556, null),
			new BuiltinFontWidth("i", 278, null),
			new BuiltinFontWidth("Oslash", 778, null),
			new BuiltinFontWidth("dagger", 500, null),
			new BuiltinFontWidth("j", 333, null),
			new BuiltinFontWidth("k", 556, null),
			new BuiltinFontWidth("l", 278, null),
			new BuiltinFontWidth("m", 833, null),
			new BuiltinFontWidth("n", 556, null),
			new BuiltinFontWidth("tcommaaccent", 333, null),
			new BuiltinFontWidth("o", 500, null),
			new BuiltinFontWidth("ordfeminine", 300, null),
			new BuiltinFontWidth("ring", 333, null),
			new BuiltinFontWidth("p", 556, null),
			new BuiltinFontWidth("q", 556, null),
			new BuiltinFontWidth("uhungarumlaut", 556, null),
			new BuiltinFontWidth("r", 444, null),
			new BuiltinFontWidth("twosuperior", 300, null),
			new BuiltinFontWidth("aacute", 500, null),
			new BuiltinFontWidth("s", 389, null),
			new BuiltinFontWidth("OE", 1000, null),
			new BuiltinFontWidth("t", 333, null),
			new BuiltinFontWidth("divide", 570, null),
			new BuiltinFontWidth("u", 556, null),
			new BuiltinFontWidth("Ccaron", 722, null),
			new BuiltinFontWidth("v", 500, null),
			new BuiltinFontWidth("w", 722, null),
			new BuiltinFontWidth("x", 500, null),
			new BuiltinFontWidth("y", 500, null),
			new BuiltinFontWidth("z", 444, null),
			new BuiltinFontWidth("Gbreve", 778, null),
			new BuiltinFontWidth("commaaccent", 250, null),
			new BuiltinFontWidth("hungarumlaut", 333, null),
			new BuiltinFontWidth("Idotaccent", 389, null),
			new BuiltinFontWidth("Nacute", 722, null),
			new BuiltinFontWidth("quotedbl", 555, null),
			new BuiltinFontWidth("gcommaaccent", 500, null),
			new BuiltinFontWidth("mu", 556, null),
			new BuiltinFontWidth("greaterequal", 549, null),
			new BuiltinFontWidth("Scaron", 556, null),
			new BuiltinFontWidth("Lslash", 667, null),
			new BuiltinFontWidth("semicolon", 333, null),
			new BuiltinFontWidth("oslash", 500, null),
			new BuiltinFontWidth("lessequal", 549, null),
			new BuiltinFontWidth("lozenge", 494, null),
			new BuiltinFontWidth("parenright", 333, null),
			new BuiltinFontWidth("ccaron", 444, null),
			new BuiltinFontWidth("Ecircumflex", 667, null),
			new BuiltinFontWidth("gbreve", 500, null),
			new BuiltinFontWidth("trademark", 1000, null),
			new BuiltinFontWidth("daggerdbl", 500, null),
			new BuiltinFontWidth("nacute", 556, null),
			new BuiltinFontWidth("macron", 333, null),
			new BuiltinFontWidth("Otilde", 778, null),
			new BuiltinFontWidth("Emacron", 667, null),
			new BuiltinFontWidth("ellipsis", 1000, null),
			new BuiltinFontWidth("scaron", 389, null),
			new BuiltinFontWidth("AE", 1000, null),
			new BuiltinFontWidth("Ucircumflex", 722, null),
			new BuiltinFontWidth("lslash", 278, null),
			new BuiltinFontWidth("quotedblleft", 500, null),
			new BuiltinFontWidth("guilsinglright", 333, null),
			new BuiltinFontWidth("hyphen", 333, null),
			new BuiltinFontWidth("quotesingle", 278, null),
			new BuiltinFontWidth("eight", 500, null),
			new BuiltinFontWidth("exclamdown", 333, null),
			new BuiltinFontWidth("endash", 500, null),
			new BuiltinFontWidth("oe", 722, null),
			new BuiltinFontWidth("Abreve", 722, null),
			new BuiltinFontWidth("Umacron", 722, null),
			new BuiltinFontWidth("ecircumflex", 444, null),
			new BuiltinFontWidth("Adieresis", 722, null),
			new BuiltinFontWidth("copyright", 747, null),
			new BuiltinFontWidth("Egrave", 667, null),
			new BuiltinFontWidth("slash", 278, null),
			new BuiltinFontWidth("Edieresis", 667, null),
			new BuiltinFontWidth("otilde", 500, null),
			new BuiltinFontWidth("Idieresis", 389, null),
			new BuiltinFontWidth("parenleft", 333, null),
			new BuiltinFontWidth("one", 500, null),
			new BuiltinFontWidth("emacron", 444, null),
			new BuiltinFontWidth("Odieresis", 778, null),
			new BuiltinFontWidth("ucircumflex", 556, null),
			new BuiltinFontWidth("bracketleft", 333, null),
			new BuiltinFontWidth("Ugrave", 722, null),
			new BuiltinFontWidth("quoteright", 333, null),
			new BuiltinFontWidth("Udieresis", 722, null),
			new BuiltinFontWidth("perthousand", 1000, null),
			new BuiltinFontWidth("Ydieresis", 722, null),
			new BuiltinFontWidth("umacron", 556, null),
			new BuiltinFontWidth("abreve", 500, null),
			new BuiltinFontWidth("Eacute", 667, null),
			new BuiltinFontWidth("adieresis", 500, null),
			new BuiltinFontWidth("egrave", 444, null),
			new BuiltinFontWidth("edieresis", 444, null),
			new BuiltinFontWidth("idieresis", 278, null),
			new BuiltinFontWidth("Eth", 722, null),
			new BuiltinFontWidth("ae", 722, null),
			new BuiltinFontWidth("asterisk", 500, null),
			new BuiltinFontWidth("odieresis", 500, null),
			new BuiltinFontWidth("Uacute", 722, null),
			new BuiltinFontWidth("ugrave", 556, null),
			new BuiltinFontWidth("nine", 500, null),
			new BuiltinFontWidth("five", 500, null),
			new BuiltinFontWidth("udieresis", 556, null),
			new BuiltinFontWidth("Zcaron", 667, null),
			new BuiltinFontWidth("Scommaaccent", 556, null),
			new BuiltinFontWidth("threequarters", 750, null),
			new BuiltinFontWidth("guillemotright", 500, null),
			new BuiltinFontWidth("Ccedilla", 722, null),
			new BuiltinFontWidth("ydieresis", 500, null),
			new BuiltinFontWidth("tilde", 333, null),
			new BuiltinFontWidth("at", 930, null),
			new BuiltinFontWidth("eacute", 444, null),
			new BuiltinFontWidth("underscore", 500, null),
			new BuiltinFontWidth("Euro", 500, null),
			new BuiltinFontWidth("Dcroat", 722, null),
			new BuiltinFontWidth("multiply", 570, null),
			new BuiltinFontWidth("zero", 500, null),
			new BuiltinFontWidth("eth", 500, null),
			new BuiltinFontWidth("Scedilla", 556, null),
			new BuiltinFontWidth("Ograve", 778, null),
			new BuiltinFontWidth("Racute", 722, null),
			new BuiltinFontWidth("partialdiff", 494, null),
			new BuiltinFontWidth("uacute", 556, null),
			new BuiltinFontWidth("braceleft", 394, null),
			new BuiltinFontWidth("Thorn", 611, null),
			new BuiltinFontWidth("zcaron", 444, null),
			new BuiltinFontWidth("scommaaccent", 389, null),
			new BuiltinFontWidth("ccedilla", 444, null),
			new BuiltinFontWidth("Dcaron", 722, null),
			new BuiltinFontWidth("dcroat", 556, null),
			new BuiltinFontWidth("Ocircumflex", 778, null),
			new BuiltinFontWidth("Oacute", 778, null),
			new BuiltinFontWidth("scedilla", 389, null),
			new BuiltinFontWidth("ogonek", 333, null),
			new BuiltinFontWidth("ograve", 500, null),
			new BuiltinFontWidth("racute", 444, null),
			new BuiltinFontWidth("Tcaron", 667, null),
			new BuiltinFontWidth("Eogonek", 667, null),
			new BuiltinFontWidth("thorn", 556, null),
			new BuiltinFontWidth("degree", 400, null),
			new BuiltinFontWidth("registered", 747, null),
			new BuiltinFontWidth("radical", 549, null),
			new BuiltinFontWidth("Aring", 722, null),
			new BuiltinFontWidth("percent", 1000, null),
			new BuiltinFontWidth("six", 500, null),
			new BuiltinFontWidth("paragraph", 540, null),
			new BuiltinFontWidth("dcaron", 672, null),
			new BuiltinFontWidth("Uogonek", 722, null),
			new BuiltinFontWidth("two", 500, null),
			new BuiltinFontWidth("summation", 600, null),
			new BuiltinFontWidth("Igrave", 389, null),
			new BuiltinFontWidth("Lacute", 667, null),
			new BuiltinFontWidth("ocircumflex", 500, null),
			new BuiltinFontWidth("oacute", 500, null),
			new BuiltinFontWidth("Uring", 722, null),
			new BuiltinFontWidth("Lcommaaccent", 667, null),
			new BuiltinFontWidth("tcaron", 416, null),
			new BuiltinFontWidth("eogonek", 444, null),
			new BuiltinFontWidth("Delta", 612, null),
			new BuiltinFontWidth("Ohungarumlaut", 778, null),
			new BuiltinFontWidth("asciicircum", 581, null),
			new BuiltinFontWidth("aring", 500, null),
			new BuiltinFontWidth("grave", 333, null),
			new BuiltinFontWidth("uogonek", 556, null),
			new BuiltinFontWidth("bracketright", 333, null),
			new BuiltinFontWidth("Iacute", 389, null),
			new BuiltinFontWidth("ampersand", 833, null),
			new BuiltinFontWidth("igrave", 278, null),
			new BuiltinFontWidth("lacute", 278, null),
			new BuiltinFontWidth("Ncaron", 722, null),
			new BuiltinFontWidth("plus", 570, null),
			new BuiltinFontWidth("uring", 556, null),
			new BuiltinFontWidth("quotesinglbase", 333, null),
			new BuiltinFontWidth("lcommaaccent", 278, null),
			new BuiltinFontWidth("Yacute", 722, null),
			new BuiltinFontWidth("ohungarumlaut", 500, null),
			new BuiltinFontWidth("threesuperior", 300, null),
			new BuiltinFontWidth("acute", 333, null),
			new BuiltinFontWidth("section", 500, null),
			new BuiltinFontWidth("dieresis", 333, null),
			new BuiltinFontWidth("iacute", 278, null),
			new BuiltinFontWidth("quotedblbase", 500, null),
			new BuiltinFontWidth("ncaron", 556, null),
			new BuiltinFontWidth("florin", 500, null),
			new BuiltinFontWidth("yacute", 500, null),
			new BuiltinFontWidth("Rcommaaccent", 722, null),
			new BuiltinFontWidth("fi", 556, null),
			new BuiltinFontWidth("fl", 556, null),
			new BuiltinFontWidth("Acircumflex", 722, null),
			new BuiltinFontWidth("Cacute", 722, null),
			new BuiltinFontWidth("Icircumflex", 389, null),
			new BuiltinFontWidth("guillemotleft", 500, null),
			new BuiltinFontWidth("germandbls", 556, null),
			new BuiltinFontWidth("Amacron", 722, null),
			new BuiltinFontWidth("seven", 500, null),
			new BuiltinFontWidth("Sacute", 556, null),
			new BuiltinFontWidth("ordmasculine", 330, null),
			new BuiltinFontWidth("dotlessi", 278, null),
			new BuiltinFontWidth("sterling", 500, null),
			new BuiltinFontWidth("notequal", 549, null),
			new BuiltinFontWidth("Imacron", 389, null),
			new BuiltinFontWidth("rcommaaccent", 444, null),
			new BuiltinFontWidth("Zdotaccent", 667, null),
			new BuiltinFontWidth("acircumflex", 500, null),
			new BuiltinFontWidth("cacute", 444, null),
			new BuiltinFontWidth("Ecaron", 667, null),
			new BuiltinFontWidth("icircumflex", 278, null),
			new BuiltinFontWidth("braceright", 394, null),
			new BuiltinFontWidth("quotedblright", 500, null),
			new BuiltinFontWidth("amacron", 500, null),
			new BuiltinFontWidth("sacute", 389, null),
			new BuiltinFontWidth("imacron", 278, null),
			new BuiltinFontWidth("cent", 500, null),
			new BuiltinFontWidth("currency", 500, null),
			new BuiltinFontWidth("logicalnot", 570, null),
			new BuiltinFontWidth("zdotaccent", 444, null),
			new BuiltinFontWidth("Atilde", 722, null),
			new BuiltinFontWidth("breve", 333, null),
			new BuiltinFontWidth("bar", 220, null),
			new BuiltinFontWidth("fraction", 167, null),
			new BuiltinFontWidth("less", 570, null),
			new BuiltinFontWidth("ecaron", 444, null),
			new BuiltinFontWidth("guilsinglleft", 333, null),
			new BuiltinFontWidth("exclam", 333, null),
			new BuiltinFontWidth("period", 250, null),
			new BuiltinFontWidth("Rcaron", 722, null),
			new BuiltinFontWidth("Kcommaaccent", 778, null),
			new BuiltinFontWidth("greater", 570, null),
			new BuiltinFontWidth("atilde", 500, null),
			new BuiltinFontWidth("brokenbar", 220, null),
			new BuiltinFontWidth("quoteleft", 333, null),
			new BuiltinFontWidth("Edotaccent", 667, null),
			new BuiltinFontWidth("onesuperior", 300, null)
		};
		private static BuiltinFontWidth[] timesBoldItalicWidthsTab = new BuiltinFontWidth[]
		{
			new BuiltinFontWidth("Ntilde", 722, null),
			new BuiltinFontWidth("rcaron", 389, null),
			new BuiltinFontWidth("kcommaaccent", 500, null),
			new BuiltinFontWidth("Ncommaaccent", 722, null),
			new BuiltinFontWidth("Zacute", 611, null),
			new BuiltinFontWidth("comma", 250, null),
			new BuiltinFontWidth("cedilla", 333, null),
			new BuiltinFontWidth("plusminus", 570, null),
			new BuiltinFontWidth("circumflex", 333, null),
			new BuiltinFontWidth("dotaccent", 333, null),
			new BuiltinFontWidth("edotaccent", 444, null),
			new BuiltinFontWidth("asciitilde", 570, null),
			new BuiltinFontWidth("colon", 333, null),
			new BuiltinFontWidth("onehalf", 750, null),
			new BuiltinFontWidth("dollar", 500, null),
			new BuiltinFontWidth("Lcaron", 611, null),
			new BuiltinFontWidth("ntilde", 556, null),
			new BuiltinFontWidth("Aogonek", 667, null),
			new BuiltinFontWidth("ncommaaccent", 556, null),
			new BuiltinFontWidth("minus", 606, null),
			new BuiltinFontWidth("Iogonek", 389, null),
			new BuiltinFontWidth("zacute", 389, null),
			new BuiltinFontWidth("yen", 500, null),
			new BuiltinFontWidth("space", 250, null),
			new BuiltinFontWidth("Omacron", 722, null),
			new BuiltinFontWidth("questiondown", 500, null),
			new BuiltinFontWidth("emdash", 1000, null),
			new BuiltinFontWidth("Agrave", 667, null),
			new BuiltinFontWidth("three", 500, null),
			new BuiltinFontWidth("numbersign", 500, null),
			new BuiltinFontWidth("lcaron", 382, null),
			new BuiltinFontWidth("A", 667, null),
			new BuiltinFontWidth("B", 667, null),
			new BuiltinFontWidth("C", 667, null),
			new BuiltinFontWidth("aogonek", 500, null),
			new BuiltinFontWidth("D", 722, null),
			new BuiltinFontWidth("E", 667, null),
			new BuiltinFontWidth("onequarter", 750, null),
			new BuiltinFontWidth("F", 667, null),
			new BuiltinFontWidth("G", 722, null),
			new BuiltinFontWidth("H", 778, null),
			new BuiltinFontWidth("I", 389, null),
			new BuiltinFontWidth("J", 500, null),
			new BuiltinFontWidth("K", 667, null),
			new BuiltinFontWidth("iogonek", 278, null),
			new BuiltinFontWidth("backslash", 278, null),
			new BuiltinFontWidth("L", 611, null),
			new BuiltinFontWidth("periodcentered", 250, null),
			new BuiltinFontWidth("M", 889, null),
			new BuiltinFontWidth("N", 722, null),
			new BuiltinFontWidth("omacron", 500, null),
			new BuiltinFontWidth("Tcommaaccent", 611, null),
			new BuiltinFontWidth("O", 722, null),
			new BuiltinFontWidth("P", 611, null),
			new BuiltinFontWidth("Q", 722, null),
			new BuiltinFontWidth("Uhungarumlaut", 722, null),
			new BuiltinFontWidth("R", 667, null),
			new BuiltinFontWidth("Aacute", 667, null),
			new BuiltinFontWidth("caron", 333, null),
			new BuiltinFontWidth("S", 556, null),
			new BuiltinFontWidth("T", 611, null),
			new BuiltinFontWidth("U", 722, null),
			new BuiltinFontWidth("agrave", 500, null),
			new BuiltinFontWidth("V", 667, null),
			new BuiltinFontWidth("W", 889, null),
			new BuiltinFontWidth("X", 667, null),
			new BuiltinFontWidth("question", 500, null),
			new BuiltinFontWidth("equal", 570, null),
			new BuiltinFontWidth("Y", 611, null),
			new BuiltinFontWidth("Z", 611, null),
			new BuiltinFontWidth("four", 500, null),
			new BuiltinFontWidth("a", 500, null),
			new BuiltinFontWidth("Gcommaaccent", 722, null),
			new BuiltinFontWidth("b", 500, null),
			new BuiltinFontWidth("c", 444, null),
			new BuiltinFontWidth("d", 500, null),
			new BuiltinFontWidth("e", 444, null),
			new BuiltinFontWidth("f", 333, null),
			new BuiltinFontWidth("g", 500, null),
			new BuiltinFontWidth("bullet", 350, null),
			new BuiltinFontWidth("h", 556, null),
			new BuiltinFontWidth("i", 278, null),
			new BuiltinFontWidth("Oslash", 722, null),
			new BuiltinFontWidth("dagger", 500, null),
			new BuiltinFontWidth("j", 278, null),
			new BuiltinFontWidth("k", 500, null),
			new BuiltinFontWidth("l", 278, null),
			new BuiltinFontWidth("m", 778, null),
			new BuiltinFontWidth("n", 556, null),
			new BuiltinFontWidth("tcommaaccent", 278, null),
			new BuiltinFontWidth("o", 500, null),
			new BuiltinFontWidth("ordfeminine", 266, null),
			new BuiltinFontWidth("ring", 333, null),
			new BuiltinFontWidth("p", 500, null),
			new BuiltinFontWidth("q", 500, null),
			new BuiltinFontWidth("uhungarumlaut", 556, null),
			new BuiltinFontWidth("r", 389, null),
			new BuiltinFontWidth("twosuperior", 300, null),
			new BuiltinFontWidth("aacute", 500, null),
			new BuiltinFontWidth("s", 389, null),
			new BuiltinFontWidth("OE", 944, null),
			new BuiltinFontWidth("t", 278, null),
			new BuiltinFontWidth("divide", 570, null),
			new BuiltinFontWidth("u", 556, null),
			new BuiltinFontWidth("Ccaron", 667, null),
			new BuiltinFontWidth("v", 444, null),
			new BuiltinFontWidth("w", 667, null),
			new BuiltinFontWidth("x", 500, null),
			new BuiltinFontWidth("y", 444, null),
			new BuiltinFontWidth("z", 389, null),
			new BuiltinFontWidth("Gbreve", 722, null),
			new BuiltinFontWidth("commaaccent", 250, null),
			new BuiltinFontWidth("hungarumlaut", 333, null),
			new BuiltinFontWidth("Idotaccent", 389, null),
			new BuiltinFontWidth("Nacute", 722, null),
			new BuiltinFontWidth("quotedbl", 555, null),
			new BuiltinFontWidth("gcommaaccent", 500, null),
			new BuiltinFontWidth("mu", 576, null),
			new BuiltinFontWidth("greaterequal", 549, null),
			new BuiltinFontWidth("Scaron", 556, null),
			new BuiltinFontWidth("Lslash", 611, null),
			new BuiltinFontWidth("semicolon", 333, null),
			new BuiltinFontWidth("oslash", 500, null),
			new BuiltinFontWidth("lessequal", 549, null),
			new BuiltinFontWidth("lozenge", 494, null),
			new BuiltinFontWidth("parenright", 333, null),
			new BuiltinFontWidth("ccaron", 444, null),
			new BuiltinFontWidth("Ecircumflex", 667, null),
			new BuiltinFontWidth("gbreve", 500, null),
			new BuiltinFontWidth("trademark", 1000, null),
			new BuiltinFontWidth("daggerdbl", 500, null),
			new BuiltinFontWidth("nacute", 556, null),
			new BuiltinFontWidth("macron", 333, null),
			new BuiltinFontWidth("Otilde", 722, null),
			new BuiltinFontWidth("Emacron", 667, null),
			new BuiltinFontWidth("ellipsis", 1000, null),
			new BuiltinFontWidth("scaron", 389, null),
			new BuiltinFontWidth("AE", 944, null),
			new BuiltinFontWidth("Ucircumflex", 722, null),
			new BuiltinFontWidth("lslash", 278, null),
			new BuiltinFontWidth("quotedblleft", 500, null),
			new BuiltinFontWidth("guilsinglright", 333, null),
			new BuiltinFontWidth("hyphen", 333, null),
			new BuiltinFontWidth("quotesingle", 278, null),
			new BuiltinFontWidth("eight", 500, null),
			new BuiltinFontWidth("exclamdown", 389, null),
			new BuiltinFontWidth("endash", 500, null),
			new BuiltinFontWidth("oe", 722, null),
			new BuiltinFontWidth("Abreve", 667, null),
			new BuiltinFontWidth("Umacron", 722, null),
			new BuiltinFontWidth("ecircumflex", 444, null),
			new BuiltinFontWidth("Adieresis", 667, null),
			new BuiltinFontWidth("copyright", 747, null),
			new BuiltinFontWidth("Egrave", 667, null),
			new BuiltinFontWidth("slash", 278, null),
			new BuiltinFontWidth("Edieresis", 667, null),
			new BuiltinFontWidth("otilde", 500, null),
			new BuiltinFontWidth("Idieresis", 389, null),
			new BuiltinFontWidth("parenleft", 333, null),
			new BuiltinFontWidth("one", 500, null),
			new BuiltinFontWidth("emacron", 444, null),
			new BuiltinFontWidth("Odieresis", 722, null),
			new BuiltinFontWidth("ucircumflex", 556, null),
			new BuiltinFontWidth("bracketleft", 333, null),
			new BuiltinFontWidth("Ugrave", 722, null),
			new BuiltinFontWidth("quoteright", 333, null),
			new BuiltinFontWidth("Udieresis", 722, null),
			new BuiltinFontWidth("perthousand", 1000, null),
			new BuiltinFontWidth("Ydieresis", 611, null),
			new BuiltinFontWidth("umacron", 556, null),
			new BuiltinFontWidth("abreve", 500, null),
			new BuiltinFontWidth("Eacute", 667, null),
			new BuiltinFontWidth("adieresis", 500, null),
			new BuiltinFontWidth("egrave", 444, null),
			new BuiltinFontWidth("edieresis", 444, null),
			new BuiltinFontWidth("idieresis", 278, null),
			new BuiltinFontWidth("Eth", 722, null),
			new BuiltinFontWidth("ae", 722, null),
			new BuiltinFontWidth("asterisk", 500, null),
			new BuiltinFontWidth("odieresis", 500, null),
			new BuiltinFontWidth("Uacute", 722, null),
			new BuiltinFontWidth("ugrave", 556, null),
			new BuiltinFontWidth("nine", 500, null),
			new BuiltinFontWidth("five", 500, null),
			new BuiltinFontWidth("udieresis", 556, null),
			new BuiltinFontWidth("Zcaron", 611, null),
			new BuiltinFontWidth("Scommaaccent", 556, null),
			new BuiltinFontWidth("threequarters", 750, null),
			new BuiltinFontWidth("guillemotright", 500, null),
			new BuiltinFontWidth("Ccedilla", 667, null),
			new BuiltinFontWidth("ydieresis", 444, null),
			new BuiltinFontWidth("tilde", 333, null),
			new BuiltinFontWidth("at", 832, null),
			new BuiltinFontWidth("eacute", 444, null),
			new BuiltinFontWidth("underscore", 500, null),
			new BuiltinFontWidth("Euro", 500, null),
			new BuiltinFontWidth("Dcroat", 722, null),
			new BuiltinFontWidth("multiply", 570, null),
			new BuiltinFontWidth("zero", 500, null),
			new BuiltinFontWidth("eth", 500, null),
			new BuiltinFontWidth("Scedilla", 556, null),
			new BuiltinFontWidth("Ograve", 722, null),
			new BuiltinFontWidth("Racute", 667, null),
			new BuiltinFontWidth("partialdiff", 494, null),
			new BuiltinFontWidth("uacute", 556, null),
			new BuiltinFontWidth("braceleft", 348, null),
			new BuiltinFontWidth("Thorn", 611, null),
			new BuiltinFontWidth("zcaron", 389, null),
			new BuiltinFontWidth("scommaaccent", 389, null),
			new BuiltinFontWidth("ccedilla", 444, null),
			new BuiltinFontWidth("Dcaron", 722, null),
			new BuiltinFontWidth("dcroat", 500, null),
			new BuiltinFontWidth("Ocircumflex", 722, null),
			new BuiltinFontWidth("Oacute", 722, null),
			new BuiltinFontWidth("scedilla", 389, null),
			new BuiltinFontWidth("ogonek", 333, null),
			new BuiltinFontWidth("ograve", 500, null),
			new BuiltinFontWidth("racute", 389, null),
			new BuiltinFontWidth("Tcaron", 611, null),
			new BuiltinFontWidth("Eogonek", 667, null),
			new BuiltinFontWidth("thorn", 500, null),
			new BuiltinFontWidth("degree", 400, null),
			new BuiltinFontWidth("registered", 747, null),
			new BuiltinFontWidth("radical", 549, null),
			new BuiltinFontWidth("Aring", 667, null),
			new BuiltinFontWidth("percent", 833, null),
			new BuiltinFontWidth("six", 500, null),
			new BuiltinFontWidth("paragraph", 500, null),
			new BuiltinFontWidth("dcaron", 608, null),
			new BuiltinFontWidth("Uogonek", 722, null),
			new BuiltinFontWidth("two", 500, null),
			new BuiltinFontWidth("summation", 600, null),
			new BuiltinFontWidth("Igrave", 389, null),
			new BuiltinFontWidth("Lacute", 611, null),
			new BuiltinFontWidth("ocircumflex", 500, null),
			new BuiltinFontWidth("oacute", 500, null),
			new BuiltinFontWidth("Uring", 722, null),
			new BuiltinFontWidth("Lcommaaccent", 611, null),
			new BuiltinFontWidth("tcaron", 366, null),
			new BuiltinFontWidth("eogonek", 444, null),
			new BuiltinFontWidth("Delta", 612, null),
			new BuiltinFontWidth("Ohungarumlaut", 722, null),
			new BuiltinFontWidth("asciicircum", 570, null),
			new BuiltinFontWidth("aring", 500, null),
			new BuiltinFontWidth("grave", 333, null),
			new BuiltinFontWidth("uogonek", 556, null),
			new BuiltinFontWidth("bracketright", 333, null),
			new BuiltinFontWidth("Iacute", 389, null),
			new BuiltinFontWidth("ampersand", 778, null),
			new BuiltinFontWidth("igrave", 278, null),
			new BuiltinFontWidth("lacute", 278, null),
			new BuiltinFontWidth("Ncaron", 722, null),
			new BuiltinFontWidth("plus", 570, null),
			new BuiltinFontWidth("uring", 556, null),
			new BuiltinFontWidth("quotesinglbase", 333, null),
			new BuiltinFontWidth("lcommaaccent", 278, null),
			new BuiltinFontWidth("Yacute", 611, null),
			new BuiltinFontWidth("ohungarumlaut", 500, null),
			new BuiltinFontWidth("threesuperior", 300, null),
			new BuiltinFontWidth("acute", 333, null),
			new BuiltinFontWidth("section", 500, null),
			new BuiltinFontWidth("dieresis", 333, null),
			new BuiltinFontWidth("iacute", 278, null),
			new BuiltinFontWidth("quotedblbase", 500, null),
			new BuiltinFontWidth("ncaron", 556, null),
			new BuiltinFontWidth("florin", 500, null),
			new BuiltinFontWidth("yacute", 444, null),
			new BuiltinFontWidth("Rcommaaccent", 667, null),
			new BuiltinFontWidth("fi", 556, null),
			new BuiltinFontWidth("fl", 556, null),
			new BuiltinFontWidth("Acircumflex", 667, null),
			new BuiltinFontWidth("Cacute", 667, null),
			new BuiltinFontWidth("Icircumflex", 389, null),
			new BuiltinFontWidth("guillemotleft", 500, null),
			new BuiltinFontWidth("germandbls", 500, null),
			new BuiltinFontWidth("Amacron", 667, null),
			new BuiltinFontWidth("seven", 500, null),
			new BuiltinFontWidth("Sacute", 556, null),
			new BuiltinFontWidth("ordmasculine", 300, null),
			new BuiltinFontWidth("dotlessi", 278, null),
			new BuiltinFontWidth("sterling", 500, null),
			new BuiltinFontWidth("notequal", 549, null),
			new BuiltinFontWidth("Imacron", 389, null),
			new BuiltinFontWidth("rcommaaccent", 389, null),
			new BuiltinFontWidth("Zdotaccent", 611, null),
			new BuiltinFontWidth("acircumflex", 500, null),
			new BuiltinFontWidth("cacute", 444, null),
			new BuiltinFontWidth("Ecaron", 667, null),
			new BuiltinFontWidth("icircumflex", 278, null),
			new BuiltinFontWidth("braceright", 348, null),
			new BuiltinFontWidth("quotedblright", 500, null),
			new BuiltinFontWidth("amacron", 500, null),
			new BuiltinFontWidth("sacute", 389, null),
			new BuiltinFontWidth("imacron", 278, null),
			new BuiltinFontWidth("cent", 500, null),
			new BuiltinFontWidth("currency", 500, null),
			new BuiltinFontWidth("logicalnot", 606, null),
			new BuiltinFontWidth("zdotaccent", 389, null),
			new BuiltinFontWidth("Atilde", 667, null),
			new BuiltinFontWidth("breve", 333, null),
			new BuiltinFontWidth("bar", 220, null),
			new BuiltinFontWidth("fraction", 167, null),
			new BuiltinFontWidth("less", 570, null),
			new BuiltinFontWidth("ecaron", 444, null),
			new BuiltinFontWidth("guilsinglleft", 333, null),
			new BuiltinFontWidth("exclam", 389, null),
			new BuiltinFontWidth("period", 250, null),
			new BuiltinFontWidth("Rcaron", 667, null),
			new BuiltinFontWidth("Kcommaaccent", 667, null),
			new BuiltinFontWidth("greater", 570, null),
			new BuiltinFontWidth("atilde", 500, null),
			new BuiltinFontWidth("brokenbar", 220, null),
			new BuiltinFontWidth("quoteleft", 333, null),
			new BuiltinFontWidth("Edotaccent", 667, null),
			new BuiltinFontWidth("onesuperior", 300, null)
		};
		private static BuiltinFontWidth[] timesItalicWidthsTab = new BuiltinFontWidth[]
		{
			new BuiltinFontWidth("Ntilde", 667, null),
			new BuiltinFontWidth("rcaron", 389, null),
			new BuiltinFontWidth("kcommaaccent", 444, null),
			new BuiltinFontWidth("Ncommaaccent", 667, null),
			new BuiltinFontWidth("Zacute", 556, null),
			new BuiltinFontWidth("comma", 250, null),
			new BuiltinFontWidth("cedilla", 333, null),
			new BuiltinFontWidth("plusminus", 675, null),
			new BuiltinFontWidth("circumflex", 333, null),
			new BuiltinFontWidth("dotaccent", 333, null),
			new BuiltinFontWidth("edotaccent", 444, null),
			new BuiltinFontWidth("asciitilde", 541, null),
			new BuiltinFontWidth("colon", 333, null),
			new BuiltinFontWidth("onehalf", 750, null),
			new BuiltinFontWidth("dollar", 500, null),
			new BuiltinFontWidth("Lcaron", 611, null),
			new BuiltinFontWidth("ntilde", 500, null),
			new BuiltinFontWidth("Aogonek", 611, null),
			new BuiltinFontWidth("ncommaaccent", 500, null),
			new BuiltinFontWidth("minus", 675, null),
			new BuiltinFontWidth("Iogonek", 333, null),
			new BuiltinFontWidth("zacute", 389, null),
			new BuiltinFontWidth("yen", 500, null),
			new BuiltinFontWidth("space", 250, null),
			new BuiltinFontWidth("Omacron", 722, null),
			new BuiltinFontWidth("questiondown", 500, null),
			new BuiltinFontWidth("emdash", 889, null),
			new BuiltinFontWidth("Agrave", 611, null),
			new BuiltinFontWidth("three", 500, null),
			new BuiltinFontWidth("numbersign", 500, null),
			new BuiltinFontWidth("lcaron", 300, null),
			new BuiltinFontWidth("A", 611, null),
			new BuiltinFontWidth("B", 611, null),
			new BuiltinFontWidth("C", 667, null),
			new BuiltinFontWidth("aogonek", 500, null),
			new BuiltinFontWidth("D", 722, null),
			new BuiltinFontWidth("E", 611, null),
			new BuiltinFontWidth("onequarter", 750, null),
			new BuiltinFontWidth("F", 611, null),
			new BuiltinFontWidth("G", 722, null),
			new BuiltinFontWidth("H", 722, null),
			new BuiltinFontWidth("I", 333, null),
			new BuiltinFontWidth("J", 444, null),
			new BuiltinFontWidth("K", 667, null),
			new BuiltinFontWidth("iogonek", 278, null),
			new BuiltinFontWidth("backslash", 278, null),
			new BuiltinFontWidth("L", 556, null),
			new BuiltinFontWidth("periodcentered", 250, null),
			new BuiltinFontWidth("M", 833, null),
			new BuiltinFontWidth("N", 667, null),
			new BuiltinFontWidth("omacron", 500, null),
			new BuiltinFontWidth("Tcommaaccent", 556, null),
			new BuiltinFontWidth("O", 722, null),
			new BuiltinFontWidth("P", 611, null),
			new BuiltinFontWidth("Q", 722, null),
			new BuiltinFontWidth("Uhungarumlaut", 722, null),
			new BuiltinFontWidth("R", 611, null),
			new BuiltinFontWidth("Aacute", 611, null),
			new BuiltinFontWidth("caron", 333, null),
			new BuiltinFontWidth("S", 500, null),
			new BuiltinFontWidth("T", 556, null),
			new BuiltinFontWidth("U", 722, null),
			new BuiltinFontWidth("agrave", 500, null),
			new BuiltinFontWidth("V", 611, null),
			new BuiltinFontWidth("W", 833, null),
			new BuiltinFontWidth("X", 611, null),
			new BuiltinFontWidth("question", 500, null),
			new BuiltinFontWidth("equal", 675, null),
			new BuiltinFontWidth("Y", 556, null),
			new BuiltinFontWidth("Z", 556, null),
			new BuiltinFontWidth("four", 500, null),
			new BuiltinFontWidth("a", 500, null),
			new BuiltinFontWidth("Gcommaaccent", 722, null),
			new BuiltinFontWidth("b", 500, null),
			new BuiltinFontWidth("c", 444, null),
			new BuiltinFontWidth("d", 500, null),
			new BuiltinFontWidth("e", 444, null),
			new BuiltinFontWidth("f", 278, null),
			new BuiltinFontWidth("g", 500, null),
			new BuiltinFontWidth("bullet", 350, null),
			new BuiltinFontWidth("h", 500, null),
			new BuiltinFontWidth("i", 278, null),
			new BuiltinFontWidth("Oslash", 722, null),
			new BuiltinFontWidth("dagger", 500, null),
			new BuiltinFontWidth("j", 278, null),
			new BuiltinFontWidth("k", 444, null),
			new BuiltinFontWidth("l", 278, null),
			new BuiltinFontWidth("m", 722, null),
			new BuiltinFontWidth("n", 500, null),
			new BuiltinFontWidth("tcommaaccent", 278, null),
			new BuiltinFontWidth("o", 500, null),
			new BuiltinFontWidth("ordfeminine", 276, null),
			new BuiltinFontWidth("ring", 333, null),
			new BuiltinFontWidth("p", 500, null),
			new BuiltinFontWidth("q", 500, null),
			new BuiltinFontWidth("uhungarumlaut", 500, null),
			new BuiltinFontWidth("r", 389, null),
			new BuiltinFontWidth("twosuperior", 300, null),
			new BuiltinFontWidth("aacute", 500, null),
			new BuiltinFontWidth("s", 389, null),
			new BuiltinFontWidth("OE", 944, null),
			new BuiltinFontWidth("t", 278, null),
			new BuiltinFontWidth("divide", 675, null),
			new BuiltinFontWidth("u", 500, null),
			new BuiltinFontWidth("Ccaron", 667, null),
			new BuiltinFontWidth("v", 444, null),
			new BuiltinFontWidth("w", 667, null),
			new BuiltinFontWidth("x", 444, null),
			new BuiltinFontWidth("y", 444, null),
			new BuiltinFontWidth("z", 389, null),
			new BuiltinFontWidth("Gbreve", 722, null),
			new BuiltinFontWidth("commaaccent", 250, null),
			new BuiltinFontWidth("hungarumlaut", 333, null),
			new BuiltinFontWidth("Idotaccent", 333, null),
			new BuiltinFontWidth("Nacute", 667, null),
			new BuiltinFontWidth("quotedbl", 420, null),
			new BuiltinFontWidth("gcommaaccent", 500, null),
			new BuiltinFontWidth("mu", 500, null),
			new BuiltinFontWidth("greaterequal", 549, null),
			new BuiltinFontWidth("Scaron", 500, null),
			new BuiltinFontWidth("Lslash", 556, null),
			new BuiltinFontWidth("semicolon", 333, null),
			new BuiltinFontWidth("oslash", 500, null),
			new BuiltinFontWidth("lessequal", 549, null),
			new BuiltinFontWidth("lozenge", 471, null),
			new BuiltinFontWidth("parenright", 333, null),
			new BuiltinFontWidth("ccaron", 444, null),
			new BuiltinFontWidth("Ecircumflex", 611, null),
			new BuiltinFontWidth("gbreve", 500, null),
			new BuiltinFontWidth("trademark", 980, null),
			new BuiltinFontWidth("daggerdbl", 500, null),
			new BuiltinFontWidth("nacute", 500, null),
			new BuiltinFontWidth("macron", 333, null),
			new BuiltinFontWidth("Otilde", 722, null),
			new BuiltinFontWidth("Emacron", 611, null),
			new BuiltinFontWidth("ellipsis", 889, null),
			new BuiltinFontWidth("scaron", 389, null),
			new BuiltinFontWidth("AE", 889, null),
			new BuiltinFontWidth("Ucircumflex", 722, null),
			new BuiltinFontWidth("lslash", 278, null),
			new BuiltinFontWidth("quotedblleft", 556, null),
			new BuiltinFontWidth("guilsinglright", 333, null),
			new BuiltinFontWidth("hyphen", 333, null),
			new BuiltinFontWidth("quotesingle", 214, null),
			new BuiltinFontWidth("eight", 500, null),
			new BuiltinFontWidth("exclamdown", 389, null),
			new BuiltinFontWidth("endash", 500, null),
			new BuiltinFontWidth("oe", 667, null),
			new BuiltinFontWidth("Abreve", 611, null),
			new BuiltinFontWidth("Umacron", 722, null),
			new BuiltinFontWidth("ecircumflex", 444, null),
			new BuiltinFontWidth("Adieresis", 611, null),
			new BuiltinFontWidth("copyright", 760, null),
			new BuiltinFontWidth("Egrave", 611, null),
			new BuiltinFontWidth("slash", 278, null),
			new BuiltinFontWidth("Edieresis", 611, null),
			new BuiltinFontWidth("otilde", 500, null),
			new BuiltinFontWidth("Idieresis", 333, null),
			new BuiltinFontWidth("parenleft", 333, null),
			new BuiltinFontWidth("one", 500, null),
			new BuiltinFontWidth("emacron", 444, null),
			new BuiltinFontWidth("Odieresis", 722, null),
			new BuiltinFontWidth("ucircumflex", 500, null),
			new BuiltinFontWidth("bracketleft", 389, null),
			new BuiltinFontWidth("Ugrave", 722, null),
			new BuiltinFontWidth("quoteright", 333, null),
			new BuiltinFontWidth("Udieresis", 722, null),
			new BuiltinFontWidth("perthousand", 1000, null),
			new BuiltinFontWidth("Ydieresis", 556, null),
			new BuiltinFontWidth("umacron", 500, null),
			new BuiltinFontWidth("abreve", 500, null),
			new BuiltinFontWidth("Eacute", 611, null),
			new BuiltinFontWidth("adieresis", 500, null),
			new BuiltinFontWidth("egrave", 444, null),
			new BuiltinFontWidth("edieresis", 444, null),
			new BuiltinFontWidth("idieresis", 278, null),
			new BuiltinFontWidth("Eth", 722, null),
			new BuiltinFontWidth("ae", 667, null),
			new BuiltinFontWidth("asterisk", 500, null),
			new BuiltinFontWidth("odieresis", 500, null),
			new BuiltinFontWidth("Uacute", 722, null),
			new BuiltinFontWidth("ugrave", 500, null),
			new BuiltinFontWidth("nine", 500, null),
			new BuiltinFontWidth("five", 500, null),
			new BuiltinFontWidth("udieresis", 500, null),
			new BuiltinFontWidth("Zcaron", 556, null),
			new BuiltinFontWidth("Scommaaccent", 500, null),
			new BuiltinFontWidth("threequarters", 750, null),
			new BuiltinFontWidth("guillemotright", 500, null),
			new BuiltinFontWidth("Ccedilla", 667, null),
			new BuiltinFontWidth("ydieresis", 444, null),
			new BuiltinFontWidth("tilde", 333, null),
			new BuiltinFontWidth("at", 920, null),
			new BuiltinFontWidth("eacute", 444, null),
			new BuiltinFontWidth("underscore", 500, null),
			new BuiltinFontWidth("Euro", 500, null),
			new BuiltinFontWidth("Dcroat", 722, null),
			new BuiltinFontWidth("multiply", 675, null),
			new BuiltinFontWidth("zero", 500, null),
			new BuiltinFontWidth("eth", 500, null),
			new BuiltinFontWidth("Scedilla", 500, null),
			new BuiltinFontWidth("Ograve", 722, null),
			new BuiltinFontWidth("Racute", 611, null),
			new BuiltinFontWidth("partialdiff", 476, null),
			new BuiltinFontWidth("uacute", 500, null),
			new BuiltinFontWidth("braceleft", 400, null),
			new BuiltinFontWidth("Thorn", 611, null),
			new BuiltinFontWidth("zcaron", 389, null),
			new BuiltinFontWidth("scommaaccent", 389, null),
			new BuiltinFontWidth("ccedilla", 444, null),
			new BuiltinFontWidth("Dcaron", 722, null),
			new BuiltinFontWidth("dcroat", 500, null),
			new BuiltinFontWidth("Ocircumflex", 722, null),
			new BuiltinFontWidth("Oacute", 722, null),
			new BuiltinFontWidth("scedilla", 389, null),
			new BuiltinFontWidth("ogonek", 333, null),
			new BuiltinFontWidth("ograve", 500, null),
			new BuiltinFontWidth("racute", 389, null),
			new BuiltinFontWidth("Tcaron", 556, null),
			new BuiltinFontWidth("Eogonek", 611, null),
			new BuiltinFontWidth("thorn", 500, null),
			new BuiltinFontWidth("degree", 400, null),
			new BuiltinFontWidth("registered", 760, null),
			new BuiltinFontWidth("radical", 453, null),
			new BuiltinFontWidth("Aring", 611, null),
			new BuiltinFontWidth("percent", 833, null),
			new BuiltinFontWidth("six", 500, null),
			new BuiltinFontWidth("paragraph", 523, null),
			new BuiltinFontWidth("dcaron", 544, null),
			new BuiltinFontWidth("Uogonek", 722, null),
			new BuiltinFontWidth("two", 500, null),
			new BuiltinFontWidth("summation", 600, null),
			new BuiltinFontWidth("Igrave", 333, null),
			new BuiltinFontWidth("Lacute", 556, null),
			new BuiltinFontWidth("ocircumflex", 500, null),
			new BuiltinFontWidth("oacute", 500, null),
			new BuiltinFontWidth("Uring", 722, null),
			new BuiltinFontWidth("Lcommaaccent", 556, null),
			new BuiltinFontWidth("tcaron", 300, null),
			new BuiltinFontWidth("eogonek", 444, null),
			new BuiltinFontWidth("Delta", 612, null),
			new BuiltinFontWidth("Ohungarumlaut", 722, null),
			new BuiltinFontWidth("asciicircum", 422, null),
			new BuiltinFontWidth("aring", 500, null),
			new BuiltinFontWidth("grave", 333, null),
			new BuiltinFontWidth("uogonek", 500, null),
			new BuiltinFontWidth("bracketright", 389, null),
			new BuiltinFontWidth("Iacute", 333, null),
			new BuiltinFontWidth("ampersand", 778, null),
			new BuiltinFontWidth("igrave", 278, null),
			new BuiltinFontWidth("lacute", 278, null),
			new BuiltinFontWidth("Ncaron", 667, null),
			new BuiltinFontWidth("plus", 675, null),
			new BuiltinFontWidth("uring", 500, null),
			new BuiltinFontWidth("quotesinglbase", 333, null),
			new BuiltinFontWidth("lcommaaccent", 278, null),
			new BuiltinFontWidth("Yacute", 556, null),
			new BuiltinFontWidth("ohungarumlaut", 500, null),
			new BuiltinFontWidth("threesuperior", 300, null),
			new BuiltinFontWidth("acute", 333, null),
			new BuiltinFontWidth("section", 500, null),
			new BuiltinFontWidth("dieresis", 333, null),
			new BuiltinFontWidth("iacute", 278, null),
			new BuiltinFontWidth("quotedblbase", 556, null),
			new BuiltinFontWidth("ncaron", 500, null),
			new BuiltinFontWidth("florin", 500, null),
			new BuiltinFontWidth("yacute", 444, null),
			new BuiltinFontWidth("Rcommaaccent", 611, null),
			new BuiltinFontWidth("fi", 500, null),
			new BuiltinFontWidth("fl", 500, null),
			new BuiltinFontWidth("Acircumflex", 611, null),
			new BuiltinFontWidth("Cacute", 667, null),
			new BuiltinFontWidth("Icircumflex", 333, null),
			new BuiltinFontWidth("guillemotleft", 500, null),
			new BuiltinFontWidth("germandbls", 500, null),
			new BuiltinFontWidth("Amacron", 611, null),
			new BuiltinFontWidth("seven", 500, null),
			new BuiltinFontWidth("Sacute", 500, null),
			new BuiltinFontWidth("ordmasculine", 310, null),
			new BuiltinFontWidth("dotlessi", 278, null),
			new BuiltinFontWidth("sterling", 500, null),
			new BuiltinFontWidth("notequal", 549, null),
			new BuiltinFontWidth("Imacron", 333, null),
			new BuiltinFontWidth("rcommaaccent", 389, null),
			new BuiltinFontWidth("Zdotaccent", 556, null),
			new BuiltinFontWidth("acircumflex", 500, null),
			new BuiltinFontWidth("cacute", 444, null),
			new BuiltinFontWidth("Ecaron", 611, null),
			new BuiltinFontWidth("icircumflex", 278, null),
			new BuiltinFontWidth("braceright", 400, null),
			new BuiltinFontWidth("quotedblright", 556, null),
			new BuiltinFontWidth("amacron", 500, null),
			new BuiltinFontWidth("sacute", 389, null),
			new BuiltinFontWidth("imacron", 278, null),
			new BuiltinFontWidth("cent", 500, null),
			new BuiltinFontWidth("currency", 500, null),
			new BuiltinFontWidth("logicalnot", 675, null),
			new BuiltinFontWidth("zdotaccent", 389, null),
			new BuiltinFontWidth("Atilde", 611, null),
			new BuiltinFontWidth("breve", 333, null),
			new BuiltinFontWidth("bar", 275, null),
			new BuiltinFontWidth("fraction", 167, null),
			new BuiltinFontWidth("less", 675, null),
			new BuiltinFontWidth("ecaron", 444, null),
			new BuiltinFontWidth("guilsinglleft", 333, null),
			new BuiltinFontWidth("exclam", 333, null),
			new BuiltinFontWidth("period", 250, null),
			new BuiltinFontWidth("Rcaron", 611, null),
			new BuiltinFontWidth("Kcommaaccent", 667, null),
			new BuiltinFontWidth("greater", 675, null),
			new BuiltinFontWidth("atilde", 500, null),
			new BuiltinFontWidth("brokenbar", 275, null),
			new BuiltinFontWidth("quoteleft", 333, null),
			new BuiltinFontWidth("Edotaccent", 611, null),
			new BuiltinFontWidth("onesuperior", 300, null)
		};
		private static BuiltinFontWidth[] timesRomanWidthsTab = new BuiltinFontWidth[]
		{
			new BuiltinFontWidth("Ntilde", 722, null),
			new BuiltinFontWidth("rcaron", 333, null),
			new BuiltinFontWidth("kcommaaccent", 500, null),
			new BuiltinFontWidth("Ncommaaccent", 722, null),
			new BuiltinFontWidth("Zacute", 611, null),
			new BuiltinFontWidth("comma", 250, null),
			new BuiltinFontWidth("cedilla", 333, null),
			new BuiltinFontWidth("plusminus", 564, null),
			new BuiltinFontWidth("circumflex", 333, null),
			new BuiltinFontWidth("dotaccent", 333, null),
			new BuiltinFontWidth("edotaccent", 444, null),
			new BuiltinFontWidth("asciitilde", 541, null),
			new BuiltinFontWidth("colon", 278, null),
			new BuiltinFontWidth("onehalf", 750, null),
			new BuiltinFontWidth("dollar", 500, null),
			new BuiltinFontWidth("Lcaron", 611, null),
			new BuiltinFontWidth("ntilde", 500, null),
			new BuiltinFontWidth("Aogonek", 722, null),
			new BuiltinFontWidth("ncommaaccent", 500, null),
			new BuiltinFontWidth("minus", 564, null),
			new BuiltinFontWidth("Iogonek", 333, null),
			new BuiltinFontWidth("zacute", 444, null),
			new BuiltinFontWidth("yen", 500, null),
			new BuiltinFontWidth("space", 250, null),
			new BuiltinFontWidth("Omacron", 722, null),
			new BuiltinFontWidth("questiondown", 444, null),
			new BuiltinFontWidth("emdash", 1000, null),
			new BuiltinFontWidth("Agrave", 722, null),
			new BuiltinFontWidth("three", 500, null),
			new BuiltinFontWidth("numbersign", 500, null),
			new BuiltinFontWidth("lcaron", 344, null),
			new BuiltinFontWidth("A", 722, null),
			new BuiltinFontWidth("B", 667, null),
			new BuiltinFontWidth("C", 667, null),
			new BuiltinFontWidth("aogonek", 444, null),
			new BuiltinFontWidth("D", 722, null),
			new BuiltinFontWidth("E", 611, null),
			new BuiltinFontWidth("onequarter", 750, null),
			new BuiltinFontWidth("F", 556, null),
			new BuiltinFontWidth("G", 722, null),
			new BuiltinFontWidth("H", 722, null),
			new BuiltinFontWidth("I", 333, null),
			new BuiltinFontWidth("J", 389, null),
			new BuiltinFontWidth("K", 722, null),
			new BuiltinFontWidth("iogonek", 278, null),
			new BuiltinFontWidth("backslash", 278, null),
			new BuiltinFontWidth("L", 611, null),
			new BuiltinFontWidth("periodcentered", 250, null),
			new BuiltinFontWidth("M", 889, null),
			new BuiltinFontWidth("N", 722, null),
			new BuiltinFontWidth("omacron", 500, null),
			new BuiltinFontWidth("Tcommaaccent", 611, null),
			new BuiltinFontWidth("O", 722, null),
			new BuiltinFontWidth("P", 556, null),
			new BuiltinFontWidth("Q", 722, null),
			new BuiltinFontWidth("Uhungarumlaut", 722, null),
			new BuiltinFontWidth("R", 667, null),
			new BuiltinFontWidth("Aacute", 722, null),
			new BuiltinFontWidth("caron", 333, null),
			new BuiltinFontWidth("S", 556, null),
			new BuiltinFontWidth("T", 611, null),
			new BuiltinFontWidth("U", 722, null),
			new BuiltinFontWidth("agrave", 444, null),
			new BuiltinFontWidth("V", 722, null),
			new BuiltinFontWidth("W", 944, null),
			new BuiltinFontWidth("X", 722, null),
			new BuiltinFontWidth("question", 444, null),
			new BuiltinFontWidth("equal", 564, null),
			new BuiltinFontWidth("Y", 722, null),
			new BuiltinFontWidth("Z", 611, null),
			new BuiltinFontWidth("four", 500, null),
			new BuiltinFontWidth("a", 444, null),
			new BuiltinFontWidth("Gcommaaccent", 722, null),
			new BuiltinFontWidth("b", 500, null),
			new BuiltinFontWidth("c", 444, null),
			new BuiltinFontWidth("d", 500, null),
			new BuiltinFontWidth("e", 444, null),
			new BuiltinFontWidth("f", 333, null),
			new BuiltinFontWidth("g", 500, null),
			new BuiltinFontWidth("bullet", 350, null),
			new BuiltinFontWidth("h", 500, null),
			new BuiltinFontWidth("i", 278, null),
			new BuiltinFontWidth("Oslash", 722, null),
			new BuiltinFontWidth("dagger", 500, null),
			new BuiltinFontWidth("j", 278, null),
			new BuiltinFontWidth("k", 500, null),
			new BuiltinFontWidth("l", 278, null),
			new BuiltinFontWidth("m", 778, null),
			new BuiltinFontWidth("n", 500, null),
			new BuiltinFontWidth("tcommaaccent", 278, null),
			new BuiltinFontWidth("o", 500, null),
			new BuiltinFontWidth("ordfeminine", 276, null),
			new BuiltinFontWidth("ring", 333, null),
			new BuiltinFontWidth("p", 500, null),
			new BuiltinFontWidth("q", 500, null),
			new BuiltinFontWidth("uhungarumlaut", 500, null),
			new BuiltinFontWidth("r", 333, null),
			new BuiltinFontWidth("twosuperior", 300, null),
			new BuiltinFontWidth("aacute", 444, null),
			new BuiltinFontWidth("s", 389, null),
			new BuiltinFontWidth("OE", 889, null),
			new BuiltinFontWidth("t", 278, null),
			new BuiltinFontWidth("divide", 564, null),
			new BuiltinFontWidth("u", 500, null),
			new BuiltinFontWidth("Ccaron", 667, null),
			new BuiltinFontWidth("v", 500, null),
			new BuiltinFontWidth("w", 722, null),
			new BuiltinFontWidth("x", 500, null),
			new BuiltinFontWidth("y", 500, null),
			new BuiltinFontWidth("z", 444, null),
			new BuiltinFontWidth("Gbreve", 722, null),
			new BuiltinFontWidth("commaaccent", 250, null),
			new BuiltinFontWidth("hungarumlaut", 333, null),
			new BuiltinFontWidth("Idotaccent", 333, null),
			new BuiltinFontWidth("Nacute", 722, null),
			new BuiltinFontWidth("quotedbl", 408, null),
			new BuiltinFontWidth("gcommaaccent", 500, null),
			new BuiltinFontWidth("mu", 500, null),
			new BuiltinFontWidth("greaterequal", 549, null),
			new BuiltinFontWidth("Scaron", 556, null),
			new BuiltinFontWidth("Lslash", 611, null),
			new BuiltinFontWidth("semicolon", 278, null),
			new BuiltinFontWidth("oslash", 500, null),
			new BuiltinFontWidth("lessequal", 549, null),
			new BuiltinFontWidth("lozenge", 471, null),
			new BuiltinFontWidth("parenright", 333, null),
			new BuiltinFontWidth("ccaron", 444, null),
			new BuiltinFontWidth("Ecircumflex", 611, null),
			new BuiltinFontWidth("gbreve", 500, null),
			new BuiltinFontWidth("trademark", 980, null),
			new BuiltinFontWidth("daggerdbl", 500, null),
			new BuiltinFontWidth("nacute", 500, null),
			new BuiltinFontWidth("macron", 333, null),
			new BuiltinFontWidth("Otilde", 722, null),
			new BuiltinFontWidth("Emacron", 611, null),
			new BuiltinFontWidth("ellipsis", 1000, null),
			new BuiltinFontWidth("scaron", 389, null),
			new BuiltinFontWidth("AE", 889, null),
			new BuiltinFontWidth("Ucircumflex", 722, null),
			new BuiltinFontWidth("lslash", 278, null),
			new BuiltinFontWidth("quotedblleft", 444, null),
			new BuiltinFontWidth("guilsinglright", 333, null),
			new BuiltinFontWidth("hyphen", 333, null),
			new BuiltinFontWidth("quotesingle", 180, null),
			new BuiltinFontWidth("eight", 500, null),
			new BuiltinFontWidth("exclamdown", 333, null),
			new BuiltinFontWidth("endash", 500, null),
			new BuiltinFontWidth("oe", 722, null),
			new BuiltinFontWidth("Abreve", 722, null),
			new BuiltinFontWidth("Umacron", 722, null),
			new BuiltinFontWidth("ecircumflex", 444, null),
			new BuiltinFontWidth("Adieresis", 722, null),
			new BuiltinFontWidth("copyright", 760, null),
			new BuiltinFontWidth("Egrave", 611, null),
			new BuiltinFontWidth("slash", 278, null),
			new BuiltinFontWidth("Edieresis", 611, null),
			new BuiltinFontWidth("otilde", 500, null),
			new BuiltinFontWidth("Idieresis", 333, null),
			new BuiltinFontWidth("parenleft", 333, null),
			new BuiltinFontWidth("one", 500, null),
			new BuiltinFontWidth("emacron", 444, null),
			new BuiltinFontWidth("Odieresis", 722, null),
			new BuiltinFontWidth("ucircumflex", 500, null),
			new BuiltinFontWidth("bracketleft", 333, null),
			new BuiltinFontWidth("Ugrave", 722, null),
			new BuiltinFontWidth("quoteright", 333, null),
			new BuiltinFontWidth("Udieresis", 722, null),
			new BuiltinFontWidth("perthousand", 1000, null),
			new BuiltinFontWidth("Ydieresis", 722, null),
			new BuiltinFontWidth("umacron", 500, null),
			new BuiltinFontWidth("abreve", 444, null),
			new BuiltinFontWidth("Eacute", 611, null),
			new BuiltinFontWidth("adieresis", 444, null),
			new BuiltinFontWidth("egrave", 444, null),
			new BuiltinFontWidth("edieresis", 444, null),
			new BuiltinFontWidth("idieresis", 278, null),
			new BuiltinFontWidth("Eth", 722, null),
			new BuiltinFontWidth("ae", 667, null),
			new BuiltinFontWidth("asterisk", 500, null),
			new BuiltinFontWidth("odieresis", 500, null),
			new BuiltinFontWidth("Uacute", 722, null),
			new BuiltinFontWidth("ugrave", 500, null),
			new BuiltinFontWidth("nine", 500, null),
			new BuiltinFontWidth("five", 500, null),
			new BuiltinFontWidth("udieresis", 500, null),
			new BuiltinFontWidth("Zcaron", 611, null),
			new BuiltinFontWidth("Scommaaccent", 556, null),
			new BuiltinFontWidth("threequarters", 750, null),
			new BuiltinFontWidth("guillemotright", 500, null),
			new BuiltinFontWidth("Ccedilla", 667, null),
			new BuiltinFontWidth("ydieresis", 500, null),
			new BuiltinFontWidth("tilde", 333, null),
			new BuiltinFontWidth("at", 921, null),
			new BuiltinFontWidth("eacute", 444, null),
			new BuiltinFontWidth("underscore", 500, null),
			new BuiltinFontWidth("Euro", 500, null),
			new BuiltinFontWidth("Dcroat", 722, null),
			new BuiltinFontWidth("multiply", 564, null),
			new BuiltinFontWidth("zero", 500, null),
			new BuiltinFontWidth("eth", 500, null),
			new BuiltinFontWidth("Scedilla", 556, null),
			new BuiltinFontWidth("Ograve", 722, null),
			new BuiltinFontWidth("Racute", 667, null),
			new BuiltinFontWidth("partialdiff", 476, null),
			new BuiltinFontWidth("uacute", 500, null),
			new BuiltinFontWidth("braceleft", 480, null),
			new BuiltinFontWidth("Thorn", 556, null),
			new BuiltinFontWidth("zcaron", 444, null),
			new BuiltinFontWidth("scommaaccent", 389, null),
			new BuiltinFontWidth("ccedilla", 444, null),
			new BuiltinFontWidth("Dcaron", 722, null),
			new BuiltinFontWidth("dcroat", 500, null),
			new BuiltinFontWidth("Ocircumflex", 722, null),
			new BuiltinFontWidth("Oacute", 722, null),
			new BuiltinFontWidth("scedilla", 389, null),
			new BuiltinFontWidth("ogonek", 333, null),
			new BuiltinFontWidth("ograve", 500, null),
			new BuiltinFontWidth("racute", 333, null),
			new BuiltinFontWidth("Tcaron", 611, null),
			new BuiltinFontWidth("Eogonek", 611, null),
			new BuiltinFontWidth("thorn", 500, null),
			new BuiltinFontWidth("degree", 400, null),
			new BuiltinFontWidth("registered", 760, null),
			new BuiltinFontWidth("radical", 453, null),
			new BuiltinFontWidth("Aring", 722, null),
			new BuiltinFontWidth("percent", 833, null),
			new BuiltinFontWidth("six", 500, null),
			new BuiltinFontWidth("paragraph", 453, null),
			new BuiltinFontWidth("dcaron", 588, null),
			new BuiltinFontWidth("Uogonek", 722, null),
			new BuiltinFontWidth("two", 500, null),
			new BuiltinFontWidth("summation", 600, null),
			new BuiltinFontWidth("Igrave", 333, null),
			new BuiltinFontWidth("Lacute", 611, null),
			new BuiltinFontWidth("ocircumflex", 500, null),
			new BuiltinFontWidth("oacute", 500, null),
			new BuiltinFontWidth("Uring", 722, null),
			new BuiltinFontWidth("Lcommaaccent", 611, null),
			new BuiltinFontWidth("tcaron", 326, null),
			new BuiltinFontWidth("eogonek", 444, null),
			new BuiltinFontWidth("Delta", 612, null),
			new BuiltinFontWidth("Ohungarumlaut", 722, null),
			new BuiltinFontWidth("asciicircum", 469, null),
			new BuiltinFontWidth("aring", 444, null),
			new BuiltinFontWidth("grave", 333, null),
			new BuiltinFontWidth("uogonek", 500, null),
			new BuiltinFontWidth("bracketright", 333, null),
			new BuiltinFontWidth("Iacute", 333, null),
			new BuiltinFontWidth("ampersand", 778, null),
			new BuiltinFontWidth("igrave", 278, null),
			new BuiltinFontWidth("lacute", 278, null),
			new BuiltinFontWidth("Ncaron", 722, null),
			new BuiltinFontWidth("plus", 564, null),
			new BuiltinFontWidth("uring", 500, null),
			new BuiltinFontWidth("quotesinglbase", 333, null),
			new BuiltinFontWidth("lcommaaccent", 278, null),
			new BuiltinFontWidth("Yacute", 722, null),
			new BuiltinFontWidth("ohungarumlaut", 500, null),
			new BuiltinFontWidth("threesuperior", 300, null),
			new BuiltinFontWidth("acute", 333, null),
			new BuiltinFontWidth("section", 500, null),
			new BuiltinFontWidth("dieresis", 333, null),
			new BuiltinFontWidth("iacute", 278, null),
			new BuiltinFontWidth("quotedblbase", 444, null),
			new BuiltinFontWidth("ncaron", 500, null),
			new BuiltinFontWidth("florin", 500, null),
			new BuiltinFontWidth("yacute", 500, null),
			new BuiltinFontWidth("Rcommaaccent", 667, null),
			new BuiltinFontWidth("fi", 556, null),
			new BuiltinFontWidth("fl", 556, null),
			new BuiltinFontWidth("Acircumflex", 722, null),
			new BuiltinFontWidth("Cacute", 667, null),
			new BuiltinFontWidth("Icircumflex", 333, null),
			new BuiltinFontWidth("guillemotleft", 500, null),
			new BuiltinFontWidth("germandbls", 500, null),
			new BuiltinFontWidth("Amacron", 722, null),
			new BuiltinFontWidth("seven", 500, null),
			new BuiltinFontWidth("Sacute", 556, null),
			new BuiltinFontWidth("ordmasculine", 310, null),
			new BuiltinFontWidth("dotlessi", 278, null),
			new BuiltinFontWidth("sterling", 500, null),
			new BuiltinFontWidth("notequal", 549, null),
			new BuiltinFontWidth("Imacron", 333, null),
			new BuiltinFontWidth("rcommaaccent", 333, null),
			new BuiltinFontWidth("Zdotaccent", 611, null),
			new BuiltinFontWidth("acircumflex", 444, null),
			new BuiltinFontWidth("cacute", 444, null),
			new BuiltinFontWidth("Ecaron", 611, null),
			new BuiltinFontWidth("icircumflex", 278, null),
			new BuiltinFontWidth("braceright", 480, null),
			new BuiltinFontWidth("quotedblright", 444, null),
			new BuiltinFontWidth("amacron", 444, null),
			new BuiltinFontWidth("sacute", 389, null),
			new BuiltinFontWidth("imacron", 278, null),
			new BuiltinFontWidth("cent", 500, null),
			new BuiltinFontWidth("currency", 500, null),
			new BuiltinFontWidth("logicalnot", 564, null),
			new BuiltinFontWidth("zdotaccent", 444, null),
			new BuiltinFontWidth("Atilde", 722, null),
			new BuiltinFontWidth("breve", 333, null),
			new BuiltinFontWidth("bar", 200, null),
			new BuiltinFontWidth("fraction", 167, null),
			new BuiltinFontWidth("less", 564, null),
			new BuiltinFontWidth("ecaron", 444, null),
			new BuiltinFontWidth("guilsinglleft", 333, null),
			new BuiltinFontWidth("exclam", 333, null),
			new BuiltinFontWidth("period", 250, null),
			new BuiltinFontWidth("Rcaron", 667, null),
			new BuiltinFontWidth("Kcommaaccent", 722, null),
			new BuiltinFontWidth("greater", 564, null),
			new BuiltinFontWidth("atilde", 444, null),
			new BuiltinFontWidth("brokenbar", 200, null),
			new BuiltinFontWidth("quoteleft", 333, null),
			new BuiltinFontWidth("Edotaccent", 611, null),
			new BuiltinFontWidth("onesuperior", 300, null)
		};
		private static BuiltinFontWidth[] zapfDingbatsWidthsTab = new BuiltinFontWidth[]
		{
			new BuiltinFontWidth("a81", 438, null),
			new BuiltinFontWidth("a82", 138, null),
			new BuiltinFontWidth("a83", 277, null),
			new BuiltinFontWidth("a84", 415, null),
			new BuiltinFontWidth("a85", 509, null),
			new BuiltinFontWidth("a86", 410, null),
			new BuiltinFontWidth("a87", 234, null),
			new BuiltinFontWidth("a88", 234, null),
			new BuiltinFontWidth("a89", 390, null),
			new BuiltinFontWidth("a140", 788, null),
			new BuiltinFontWidth("a141", 788, null),
			new BuiltinFontWidth("a142", 788, null),
			new BuiltinFontWidth("a143", 788, null),
			new BuiltinFontWidth("a144", 788, null),
			new BuiltinFontWidth("a145", 788, null),
			new BuiltinFontWidth("a146", 788, null),
			new BuiltinFontWidth("a147", 788, null),
			new BuiltinFontWidth("a148", 788, null),
			new BuiltinFontWidth("a149", 788, null),
			new BuiltinFontWidth("a90", 390, null),
			new BuiltinFontWidth("a91", 276, null),
			new BuiltinFontWidth("a92", 276, null),
			new BuiltinFontWidth("space", 278, null),
			new BuiltinFontWidth("a93", 317, null),
			new BuiltinFontWidth("a94", 317, null),
			new BuiltinFontWidth("a95", 334, null),
			new BuiltinFontWidth("a96", 334, null),
			new BuiltinFontWidth("a97", 392, null),
			new BuiltinFontWidth("a98", 392, null),
			new BuiltinFontWidth("a99", 668, null),
			new BuiltinFontWidth("a150", 788, null),
			new BuiltinFontWidth("a151", 788, null),
			new BuiltinFontWidth("a152", 788, null),
			new BuiltinFontWidth("a153", 788, null),
			new BuiltinFontWidth("a154", 788, null),
			new BuiltinFontWidth("a155", 788, null),
			new BuiltinFontWidth("a156", 788, null),
			new BuiltinFontWidth("a157", 788, null),
			new BuiltinFontWidth("a158", 788, null),
			new BuiltinFontWidth("a159", 788, null),
			new BuiltinFontWidth("a160", 894, null),
			new BuiltinFontWidth("a161", 838, null),
			new BuiltinFontWidth("a162", 924, null),
			new BuiltinFontWidth("a163", 1016, null),
			new BuiltinFontWidth("a164", 458, null),
			new BuiltinFontWidth("a165", 924, null),
			new BuiltinFontWidth("a166", 918, null),
			new BuiltinFontWidth("a167", 927, null),
			new BuiltinFontWidth("a168", 928, null),
			new BuiltinFontWidth("a169", 928, null),
			new BuiltinFontWidth("a170", 834, null),
			new BuiltinFontWidth("a171", 873, null),
			new BuiltinFontWidth("a172", 828, null),
			new BuiltinFontWidth("a173", 924, null),
			new BuiltinFontWidth("a174", 917, null),
			new BuiltinFontWidth("a175", 930, null),
			new BuiltinFontWidth("a176", 931, null),
			new BuiltinFontWidth("a177", 463, null),
			new BuiltinFontWidth("a178", 883, null),
			new BuiltinFontWidth("a179", 836, null),
			new BuiltinFontWidth("a180", 867, null),
			new BuiltinFontWidth("a181", 696, null),
			new BuiltinFontWidth("a182", 874, null),
			new BuiltinFontWidth("a183", 760, null),
			new BuiltinFontWidth("a184", 946, null),
			new BuiltinFontWidth("a185", 865, null),
			new BuiltinFontWidth("a186", 967, null),
			new BuiltinFontWidth("a187", 831, null),
			new BuiltinFontWidth("a188", 873, null),
			new BuiltinFontWidth("a189", 927, null),
			new BuiltinFontWidth("a1", 974, null),
			new BuiltinFontWidth("a2", 961, null),
			new BuiltinFontWidth("a3", 980, null),
			new BuiltinFontWidth("a4", 719, null),
			new BuiltinFontWidth("a5", 789, null),
			new BuiltinFontWidth("a6", 494, null),
			new BuiltinFontWidth("a7", 552, null),
			new BuiltinFontWidth("a8", 537, null),
			new BuiltinFontWidth("a9", 577, null),
			new BuiltinFontWidth("a190", 970, null),
			new BuiltinFontWidth("a191", 918, null),
			new BuiltinFontWidth("a192", 748, null),
			new BuiltinFontWidth("a193", 836, null),
			new BuiltinFontWidth("a194", 771, null),
			new BuiltinFontWidth("a195", 888, null),
			new BuiltinFontWidth("a196", 748, null),
			new BuiltinFontWidth("a197", 771, null),
			new BuiltinFontWidth("a198", 888, null),
			new BuiltinFontWidth("a199", 867, null),
			new BuiltinFontWidth("a10", 692, null),
			new BuiltinFontWidth("a11", 960, null),
			new BuiltinFontWidth("a12", 939, null),
			new BuiltinFontWidth("a13", 549, null),
			new BuiltinFontWidth("a14", 855, null),
			new BuiltinFontWidth("a15", 911, null),
			new BuiltinFontWidth("a16", 933, null),
			new BuiltinFontWidth("a17", 945, null),
			new BuiltinFontWidth("a18", 974, null),
			new BuiltinFontWidth("a19", 755, null),
			new BuiltinFontWidth("a20", 846, null),
			new BuiltinFontWidth("a21", 762, null),
			new BuiltinFontWidth("a22", 761, null),
			new BuiltinFontWidth("a23", 571, null),
			new BuiltinFontWidth("a24", 677, null),
			new BuiltinFontWidth("a25", 763, null),
			new BuiltinFontWidth("a26", 760, null),
			new BuiltinFontWidth("a27", 759, null),
			new BuiltinFontWidth("a28", 754, null),
			new BuiltinFontWidth("a29", 786, null),
			new BuiltinFontWidth("a30", 788, null),
			new BuiltinFontWidth("a31", 788, null),
			new BuiltinFontWidth("a32", 790, null),
			new BuiltinFontWidth("a33", 793, null),
			new BuiltinFontWidth("a34", 794, null),
			new BuiltinFontWidth("a35", 816, null),
			new BuiltinFontWidth("a36", 823, null),
			new BuiltinFontWidth("a37", 789, null),
			new BuiltinFontWidth("a38", 841, null),
			new BuiltinFontWidth("a39", 823, null),
			new BuiltinFontWidth("a40", 833, null),
			new BuiltinFontWidth("a41", 816, null),
			new BuiltinFontWidth("a42", 831, null),
			new BuiltinFontWidth("a43", 923, null),
			new BuiltinFontWidth("a44", 744, null),
			new BuiltinFontWidth("a45", 723, null),
			new BuiltinFontWidth("a46", 749, null),
			new BuiltinFontWidth("a47", 790, null),
			new BuiltinFontWidth("a48", 792, null),
			new BuiltinFontWidth("a49", 695, null),
			new BuiltinFontWidth("a100", 668, null),
			new BuiltinFontWidth("a101", 732, null),
			new BuiltinFontWidth("a102", 544, null),
			new BuiltinFontWidth("a103", 544, null),
			new BuiltinFontWidth("a104", 910, null),
			new BuiltinFontWidth("a105", 911, null),
			new BuiltinFontWidth("a106", 667, null),
			new BuiltinFontWidth("a107", 760, null),
			new BuiltinFontWidth("a108", 760, null),
			new BuiltinFontWidth("a109", 626, null),
			new BuiltinFontWidth("a50", 776, null),
			new BuiltinFontWidth("a51", 768, null),
			new BuiltinFontWidth("a52", 792, null),
			new BuiltinFontWidth("a53", 759, null),
			new BuiltinFontWidth("a54", 707, null),
			new BuiltinFontWidth("a55", 708, null),
			new BuiltinFontWidth("a56", 682, null),
			new BuiltinFontWidth("a57", 701, null),
			new BuiltinFontWidth("a58", 826, null),
			new BuiltinFontWidth("a59", 815, null),
			new BuiltinFontWidth("a110", 694, null),
			new BuiltinFontWidth("a111", 595, null),
			new BuiltinFontWidth("a112", 776, null),
			new BuiltinFontWidth("a117", 690, null),
			new BuiltinFontWidth("a118", 791, null),
			new BuiltinFontWidth("a119", 790, null),
			new BuiltinFontWidth("a60", 789, null),
			new BuiltinFontWidth("a61", 789, null),
			new BuiltinFontWidth("a62", 707, null),
			new BuiltinFontWidth("a63", 687, null),
			new BuiltinFontWidth("a64", 696, null),
			new BuiltinFontWidth("a65", 689, null),
			new BuiltinFontWidth("a66", 786, null),
			new BuiltinFontWidth("a67", 787, null),
			new BuiltinFontWidth("a68", 713, null),
			new BuiltinFontWidth("a69", 791, null),
			new BuiltinFontWidth("a200", 696, null),
			new BuiltinFontWidth("a201", 874, null),
			new BuiltinFontWidth("a120", 788, null),
			new BuiltinFontWidth("a121", 788, null),
			new BuiltinFontWidth("a202", 974, null),
			new BuiltinFontWidth("a122", 788, null),
			new BuiltinFontWidth("a203", 762, null),
			new BuiltinFontWidth("a123", 788, null),
			new BuiltinFontWidth("a204", 759, null),
			new BuiltinFontWidth("a124", 788, null),
			new BuiltinFontWidth("a205", 509, null),
			new BuiltinFontWidth("a125", 788, null),
			new BuiltinFontWidth("a206", 410, null),
			new BuiltinFontWidth("a126", 788, null),
			new BuiltinFontWidth("a127", 788, null),
			new BuiltinFontWidth("a128", 788, null),
			new BuiltinFontWidth("a129", 788, null),
			new BuiltinFontWidth("a70", 785, null),
			new BuiltinFontWidth("a71", 791, null),
			new BuiltinFontWidth("a72", 873, null),
			new BuiltinFontWidth("a73", 761, null),
			new BuiltinFontWidth("a74", 762, null),
			new BuiltinFontWidth("a75", 759, null),
			new BuiltinFontWidth("a76", 892, null),
			new BuiltinFontWidth("a77", 892, null),
			new BuiltinFontWidth("a78", 788, null),
			new BuiltinFontWidth("a79", 784, null),
			new BuiltinFontWidth("a130", 788, null),
			new BuiltinFontWidth("a131", 788, null),
			new BuiltinFontWidth("a132", 788, null),
			new BuiltinFontWidth("a133", 788, null),
			new BuiltinFontWidth("a134", 788, null),
			new BuiltinFontWidth("a135", 788, null),
			new BuiltinFontWidth("a136", 788, null),
			new BuiltinFontWidth("a137", 788, null),
			new BuiltinFontWidth("a138", 788, null),
			new BuiltinFontWidth("a139", 788, null)
		};
		private static string[] standardEncoding;
		private string[] symbolEncoding;
		private string[] zapfDingbatsEncoding;
		private string[] macRomanEncoding;
		private static PdfPreview.NameToUnicode[] nameToUnicodeTab;
		internal PdfManager m_pManager;
		internal PdfDocument m_pDoc;
		internal PdfPage m_pPage;
		internal PdfResources m_pResources;
		internal PdfRavenOutputDev m_pRavenOut;
		internal PdfState m_pState;
		internal bool m_bDebug;
		internal bool m_bFontChanged;
		internal int m_nFormDepth;
		internal int m_nIgnoreUndef;
		internal IccProfile m_pIccProfile;
		internal ClipType m_clip;
		internal double[] baseMatrix;
		internal int m_nPixelWidth;
		internal int m_nPixelHeight;
		internal PdfInput m_pInput;
		private StringBuilder m_bstrLog;
		private double axialColorDelta;
		private double radialColorDelta;
		private static PdfPreview.PreviewOperator[] m_arrOperators;
		public string Log
		{
			get
			{
				return this.m_bstrLog.ToString();
			}
		}
		internal void initBuiltinFontTables()
		{
			this.builtinFonts = new BuiltinFont[14];
			for (int i = 0; i < this.builtinFonts.Length; i++)
			{
				this.builtinFonts[i] = new BuiltinFont();
			}
			this.builtinFonts[0].name = "Courier";
			this.builtinFonts[0].defaultBaseEnc = PdfPreview.standardEncoding;
			this.builtinFonts[0].ascent = 629;
			this.builtinFonts[0].descent = -157;
			this.builtinFonts[0].bbox[0] = -23;
			this.builtinFonts[0].bbox[1] = -250;
			this.builtinFonts[0].bbox[2] = 715;
			this.builtinFonts[0].bbox[3] = 805;
			this.builtinFonts[0].widths = new BuiltinFontWidths(PdfPreview.courierWidthsTab, 315);
			this.builtinFonts[1].name = "Courier-Bold";
			this.builtinFonts[1].defaultBaseEnc = PdfPreview.standardEncoding;
			this.builtinFonts[1].ascent = 629;
			this.builtinFonts[1].descent = -113;
			this.builtinFonts[1].bbox[0] = -23;
			this.builtinFonts[1].bbox[1] = -250;
			this.builtinFonts[1].bbox[2] = 749;
			this.builtinFonts[1].bbox[3] = 801;
			this.builtinFonts[1].widths = new BuiltinFontWidths(PdfPreview.courierBoldWidthsTab, 315);
			this.builtinFonts[2].name = "Courier-BoldOblique";
			this.builtinFonts[2].defaultBaseEnc = PdfPreview.standardEncoding;
			this.builtinFonts[2].ascent = 629;
			this.builtinFonts[2].descent = -157;
			this.builtinFonts[2].bbox[0] = -57;
			this.builtinFonts[2].bbox[1] = -250;
			this.builtinFonts[2].bbox[2] = 869;
			this.builtinFonts[2].bbox[3] = 801;
			this.builtinFonts[2].widths = new BuiltinFontWidths(PdfPreview.courierBoldObliqueWidthsTab, 315);
			this.builtinFonts[3].name = "Courier-Oblique";
			this.builtinFonts[3].defaultBaseEnc = PdfPreview.standardEncoding;
			this.builtinFonts[3].ascent = 629;
			this.builtinFonts[3].descent = -157;
			this.builtinFonts[3].bbox[0] = -27;
			this.builtinFonts[3].bbox[1] = -250;
			this.builtinFonts[3].bbox[2] = 849;
			this.builtinFonts[3].bbox[3] = 805;
			this.builtinFonts[3].widths = new BuiltinFontWidths(PdfPreview.courierObliqueWidthsTab, 315);
			this.builtinFonts[4].name = "Helvetica";
			this.builtinFonts[4].defaultBaseEnc = PdfPreview.standardEncoding;
			this.builtinFonts[4].ascent = 718;
			this.builtinFonts[4].descent = -207;
			this.builtinFonts[4].bbox[0] = -166;
			this.builtinFonts[4].bbox[1] = -225;
			this.builtinFonts[4].bbox[2] = 1000;
			this.builtinFonts[4].bbox[3] = 931;
			this.builtinFonts[4].widths = new BuiltinFontWidths(PdfPreview.helveticaWidthsTab, 315);
			this.builtinFonts[5].name = "Helvetica-Bold";
			this.builtinFonts[5].defaultBaseEnc = PdfPreview.standardEncoding;
			this.builtinFonts[5].ascent = 718;
			this.builtinFonts[5].descent = -207;
			this.builtinFonts[5].bbox[0] = -170;
			this.builtinFonts[5].bbox[1] = -228;
			this.builtinFonts[5].bbox[2] = 1003;
			this.builtinFonts[5].bbox[3] = 962;
			this.builtinFonts[5].widths = new BuiltinFontWidths(PdfPreview.helveticaBoldWidthsTab, 316);
			this.builtinFonts[6].name = "Helvetica-BoldOblique";
			this.builtinFonts[6].defaultBaseEnc = PdfPreview.standardEncoding;
			this.builtinFonts[6].ascent = 718;
			this.builtinFonts[6].descent = -207;
			this.builtinFonts[6].bbox[0] = -174;
			this.builtinFonts[6].bbox[1] = -228;
			this.builtinFonts[6].bbox[2] = 1114;
			this.builtinFonts[6].bbox[3] = 962;
			this.builtinFonts[6].widths = new BuiltinFontWidths(PdfPreview.helveticaBoldObliqueWidthsTab, 315);
			this.builtinFonts[7].name = "Helvetica-Oblique";
			this.builtinFonts[7].defaultBaseEnc = PdfPreview.standardEncoding;
			this.builtinFonts[7].ascent = 718;
			this.builtinFonts[7].descent = -207;
			this.builtinFonts[7].bbox[0] = -170;
			this.builtinFonts[7].bbox[1] = -225;
			this.builtinFonts[7].bbox[2] = 1116;
			this.builtinFonts[7].bbox[3] = 931;
			this.builtinFonts[7].widths = new BuiltinFontWidths(PdfPreview.helveticaObliqueWidthsTab, 315);
			this.builtinFonts[8].name = "Symbol";
			this.builtinFonts[8].defaultBaseEnc = this.symbolEncoding;
			this.builtinFonts[8].ascent = 1010;
			this.builtinFonts[8].descent = -293;
			this.builtinFonts[8].bbox[0] = -180;
			this.builtinFonts[8].bbox[1] = -293;
			this.builtinFonts[8].bbox[2] = 1090;
			this.builtinFonts[8].bbox[3] = 1010;
			this.builtinFonts[8].widths = new BuiltinFontWidths(PdfPreview.symbolWidthsTab, 190);
			this.builtinFonts[9].name = "Times-Bold";
			this.builtinFonts[9].defaultBaseEnc = PdfPreview.standardEncoding;
			this.builtinFonts[9].ascent = 683;
			this.builtinFonts[9].descent = -217;
			this.builtinFonts[9].bbox[0] = -168;
			this.builtinFonts[9].bbox[1] = -218;
			this.builtinFonts[9].bbox[2] = 1000;
			this.builtinFonts[9].bbox[3] = 935;
			this.builtinFonts[9].widths = new BuiltinFontWidths(PdfPreview.timesBoldWidthsTab, 315);
			this.builtinFonts[10].name = "Times-BoldItalic";
			this.builtinFonts[10].defaultBaseEnc = PdfPreview.standardEncoding;
			this.builtinFonts[10].ascent = 683;
			this.builtinFonts[10].descent = -217;
			this.builtinFonts[10].bbox[0] = -200;
			this.builtinFonts[10].bbox[1] = -218;
			this.builtinFonts[10].bbox[2] = 996;
			this.builtinFonts[10].bbox[3] = 921;
			this.builtinFonts[10].widths = new BuiltinFontWidths(PdfPreview.timesBoldItalicWidthsTab, 315);
			this.builtinFonts[11].name = "Times-Italic";
			this.builtinFonts[11].defaultBaseEnc = PdfPreview.standardEncoding;
			this.builtinFonts[11].ascent = 683;
			this.builtinFonts[11].descent = -217;
			this.builtinFonts[11].bbox[0] = -169;
			this.builtinFonts[11].bbox[1] = -217;
			this.builtinFonts[11].bbox[2] = 1010;
			this.builtinFonts[11].bbox[3] = 883;
			this.builtinFonts[11].widths = new BuiltinFontWidths(PdfPreview.timesItalicWidthsTab, 315);
			this.builtinFonts[12].name = "Times-Roman";
			this.builtinFonts[12].defaultBaseEnc = PdfPreview.standardEncoding;
			this.builtinFonts[12].ascent = 683;
			this.builtinFonts[12].descent = -217;
			this.builtinFonts[12].bbox[0] = -168;
			this.builtinFonts[12].bbox[1] = -218;
			this.builtinFonts[12].bbox[2] = 1000;
			this.builtinFonts[12].bbox[3] = 898;
			this.builtinFonts[12].widths = new BuiltinFontWidths(PdfPreview.timesRomanWidthsTab, 315);
			this.builtinFonts[13].name = "ZapfDingbats";
			this.builtinFonts[13].defaultBaseEnc = this.zapfDingbatsEncoding;
			this.builtinFonts[13].ascent = 820;
			this.builtinFonts[13].descent = -143;
			this.builtinFonts[13].bbox[0] = -1;
			this.builtinFonts[13].bbox[1] = -143;
			this.builtinFonts[13].bbox[2] = 981;
			this.builtinFonts[13].bbox[3] = 820;
			this.builtinFonts[13].widths = new BuiltinFontWidths(PdfPreview.zapfDingbatsWidthsTab, 202);
			this.macRomanReverseMap = new NameToCharCode();
			for (int j = 255; j >= 0; j--)
			{
				if (this.macRomanEncoding[j] != null)
				{
					this.macRomanReverseMap.add(this.macRomanEncoding[j], (uint)j);
				}
			}
			this.builtinFontSubst[0] = this.builtinFonts[0];
			this.builtinFontSubst[1] = this.builtinFonts[3];
			this.builtinFontSubst[2] = this.builtinFonts[1];
			this.builtinFontSubst[3] = this.builtinFonts[2];
			this.builtinFontSubst[4] = this.builtinFonts[4];
			this.builtinFontSubst[5] = this.builtinFonts[7];
			this.builtinFontSubst[6] = this.builtinFonts[5];
			this.builtinFontSubst[7] = this.builtinFonts[6];
			this.builtinFontSubst[8] = this.builtinFonts[12];
			this.builtinFontSubst[9] = this.builtinFonts[11];
			this.builtinFontSubst[10] = this.builtinFonts[9];
			this.builtinFontSubst[11] = this.builtinFonts[10];
			this.m_nameToUnicode = new NameToCharCode();
			int num = 0;
			while (PdfPreview.nameToUnicodeTab[num].name != null)
			{
				this.m_nameToUnicode.add(PdfPreview.nameToUnicodeTab[num].name, PdfPreview.nameToUnicodeTab[num].u);
				num++;
			}
			this.m_pCMapCache = new CMapCache();
		}
		internal PdfPreview()
		{
			string[] array = new string[256];
			array[32] = "space";
			array[33] = "exclam";
			array[34] = "universal";
			array[35] = "numbersign";
			array[36] = "existential";
			array[37] = "percent";
			array[38] = "ampersand";
			array[39] = "suchthat";
			array[40] = "parenleft";
			array[41] = "parenright";
			array[42] = "asteriskmath";
			array[43] = "plus";
			array[44] = "comma";
			array[45] = "minus";
			array[46] = "period";
			array[47] = "slash";
			array[48] = "zero";
			array[49] = "one";
			array[50] = "two";
			array[51] = "three";
			array[52] = "four";
			array[53] = "five";
			array[54] = "six";
			array[55] = "seven";
			array[56] = "eight";
			array[57] = "nine";
			array[58] = "colon";
			array[59] = "semicolon";
			array[60] = "less";
			array[61] = "equal";
			array[62] = "greater";
			array[63] = "question";
			array[64] = "congruent";
			array[65] = "Alpha";
			array[66] = "Beta";
			array[67] = "Chi";
			array[68] = "Delta";
			array[69] = "Epsilon";
			array[70] = "Phi";
			array[71] = "Gamma";
			array[72] = "Eta";
			array[73] = "Iota";
			array[74] = "theta1";
			array[75] = "Kappa";
			array[76] = "Lambda";
			array[77] = "Mu";
			array[78] = "Nu";
			array[79] = "Omicron";
			array[80] = "Pi";
			array[81] = "Theta";
			array[82] = "Rho";
			array[83] = "Sigma";
			array[84] = "Tau";
			array[85] = "Upsilon";
			array[86] = "sigma1";
			array[87] = "Omega";
			array[88] = "Xi";
			array[89] = "Psi";
			array[90] = "Zeta";
			array[91] = "bracketleft";
			array[92] = "therefore";
			array[93] = "bracketright";
			array[94] = "perpendicular";
			array[95] = "underscore";
			array[96] = "radicalex";
			array[97] = "alpha";
			array[98] = "beta";
			array[99] = "chi";
			array[100] = "delta";
			array[101] = "epsilon";
			array[102] = "phi";
			array[103] = "gamma";
			array[104] = "eta";
			array[105] = "iota";
			array[106] = "phi1";
			array[107] = "kappa";
			array[108] = "lambda";
			array[109] = "mu";
			array[110] = "nu";
			array[111] = "omicron";
			array[112] = "pi";
			array[113] = "theta";
			array[114] = "rho";
			array[115] = "sigma";
			array[116] = "tau";
			array[117] = "upsilon";
			array[118] = "omega1";
			array[119] = "omega";
			array[120] = "xi";
			array[121] = "psi";
			array[122] = "zeta";
			array[123] = "braceleft";
			array[124] = "bar";
			array[125] = "braceright";
			array[126] = "similar";
			array[161] = "Upsilon1";
			array[162] = "minute";
			array[163] = "lessequal";
			array[164] = "fraction";
			array[165] = "infinity";
			array[166] = "florin";
			array[167] = "club";
			array[168] = "diamond";
			array[169] = "heart";
			array[170] = "spade";
			array[171] = "arrowboth";
			array[172] = "arrowleft";
			array[173] = "arrowup";
			array[174] = "arrowright";
			array[175] = "arrowdown";
			array[176] = "degree";
			array[177] = "plusminus";
			array[178] = "second";
			array[179] = "greaterequal";
			array[180] = "multiply";
			array[181] = "proportional";
			array[182] = "partialdiff";
			array[183] = "bullet";
			array[184] = "divide";
			array[185] = "notequal";
			array[186] = "equivalence";
			array[187] = "approxequal";
			array[188] = "ellipsis";
			array[189] = "arrowvertex";
			array[190] = "arrowhorizex";
			array[191] = "carriagereturn";
			array[192] = "aleph";
			array[193] = "Ifraktur";
			array[194] = "Rfraktur";
			array[195] = "weierstrass";
			array[196] = "circlemultiply";
			array[197] = "circleplus";
			array[198] = "emptyset";
			array[199] = "intersection";
			array[200] = "union";
			array[201] = "propersuperset";
			array[202] = "reflexsuperset";
			array[203] = "notsubset";
			array[204] = "propersubset";
			array[205] = "reflexsubset";
			array[206] = "element";
			array[207] = "notelement";
			array[208] = "angle";
			array[209] = "gradient";
			array[210] = "registerserif";
			array[211] = "copyrightserif";
			array[212] = "trademarkserif";
			array[213] = "product";
			array[214] = "radical";
			array[215] = "dotmath";
			array[216] = "logicalnot";
			array[217] = "logicaland";
			array[218] = "logicalor";
			array[219] = "arrowdblboth";
			array[220] = "arrowdblleft";
			array[221] = "arrowdblup";
			array[222] = "arrowdblright";
			array[223] = "arrowdbldown";
			array[224] = "lozenge";
			array[225] = "angleleft";
			array[226] = "registersans";
			array[227] = "copyrightsans";
			array[228] = "trademarksans";
			array[229] = "summation";
			array[230] = "parenlefttp";
			array[231] = "parenleftex";
			array[232] = "parenleftbt";
			array[233] = "bracketlefttp";
			array[234] = "bracketleftex";
			array[235] = "bracketleftbt";
			array[236] = "bracelefttp";
			array[237] = "braceleftmid";
			array[238] = "braceleftbt";
			array[239] = "braceex";
			array[241] = "angleright";
			array[242] = "integral";
			array[243] = "integraltp";
			array[244] = "integralex";
			array[245] = "integralbt";
			array[246] = "parenrighttp";
			array[247] = "parenrightex";
			array[248] = "parenrightbt";
			array[249] = "bracketrighttp";
			array[250] = "bracketrightex";
			array[251] = "bracketrightbt";
			array[252] = "bracerighttp";
			array[253] = "bracerightmid";
			array[254] = "bracerightbt";
			this.symbolEncoding = array;
			string[] array2 = new string[256];
			array2[32] = "space";
			array2[33] = "a1";
			array2[34] = "a2";
			array2[35] = "a202";
			array2[36] = "a3";
			array2[37] = "a4";
			array2[38] = "a5";
			array2[39] = "a119";
			array2[40] = "a118";
			array2[41] = "a117";
			array2[42] = "a11";
			array2[43] = "a12";
			array2[44] = "a13";
			array2[45] = "a14";
			array2[46] = "a15";
			array2[47] = "a16";
			array2[48] = "a105";
			array2[49] = "a17";
			array2[50] = "a18";
			array2[51] = "a19";
			array2[52] = "a20";
			array2[53] = "a21";
			array2[54] = "a22";
			array2[55] = "a23";
			array2[56] = "a24";
			array2[57] = "a25";
			array2[58] = "a26";
			array2[59] = "a27";
			array2[60] = "a28";
			array2[61] = "a6";
			array2[62] = "a7";
			array2[63] = "a8";
			array2[64] = "a9";
			array2[65] = "a10";
			array2[66] = "a29";
			array2[67] = "a30";
			array2[68] = "a31";
			array2[69] = "a32";
			array2[70] = "a33";
			array2[71] = "a34";
			array2[72] = "a35";
			array2[73] = "a36";
			array2[74] = "a37";
			array2[75] = "a38";
			array2[76] = "a39";
			array2[77] = "a40";
			array2[78] = "a41";
			array2[79] = "a42";
			array2[80] = "a43";
			array2[81] = "a44";
			array2[82] = "a45";
			array2[83] = "a46";
			array2[84] = "a47";
			array2[85] = "a48";
			array2[86] = "a49";
			array2[87] = "a50";
			array2[88] = "a51";
			array2[89] = "a52";
			array2[90] = "a53";
			array2[91] = "a54";
			array2[92] = "a55";
			array2[93] = "a56";
			array2[94] = "a57";
			array2[95] = "a58";
			array2[96] = "a59";
			array2[97] = "a60";
			array2[98] = "a61";
			array2[99] = "a62";
			array2[100] = "a63";
			array2[101] = "a64";
			array2[102] = "a65";
			array2[103] = "a66";
			array2[104] = "a67";
			array2[105] = "a68";
			array2[106] = "a69";
			array2[107] = "a70";
			array2[108] = "a71";
			array2[109] = "a72";
			array2[110] = "a73";
			array2[111] = "a74";
			array2[112] = "a203";
			array2[113] = "a75";
			array2[114] = "a204";
			array2[115] = "a76";
			array2[116] = "a77";
			array2[117] = "a78";
			array2[118] = "a79";
			array2[119] = "a81";
			array2[120] = "a82";
			array2[121] = "a83";
			array2[122] = "a84";
			array2[123] = "a97";
			array2[124] = "a98";
			array2[125] = "a99";
			array2[126] = "a100";
			array2[161] = "a101";
			array2[162] = "a102";
			array2[163] = "a103";
			array2[164] = "a104";
			array2[165] = "a106";
			array2[166] = "a107";
			array2[167] = "a108";
			array2[168] = "a112";
			array2[169] = "a111";
			array2[170] = "a110";
			array2[171] = "a109";
			array2[172] = "a120";
			array2[173] = "a121";
			array2[174] = "a122";
			array2[175] = "a123";
			array2[176] = "a124";
			array2[177] = "a125";
			array2[178] = "a126";
			array2[179] = "a127";
			array2[180] = "a128";
			array2[181] = "a129";
			array2[182] = "a130";
			array2[183] = "a131";
			array2[184] = "a132";
			array2[185] = "a133";
			array2[186] = "a134";
			array2[187] = "a135";
			array2[188] = "a136";
			array2[189] = "a137";
			array2[190] = "a138";
			array2[191] = "a139";
			array2[192] = "a140";
			array2[193] = "a141";
			array2[194] = "a142";
			array2[195] = "a143";
			array2[196] = "a144";
			array2[197] = "a145";
			array2[198] = "a146";
			array2[199] = "a147";
			array2[200] = "a148";
			array2[201] = "a149";
			array2[202] = "a150";
			array2[203] = "a151";
			array2[204] = "a152";
			array2[205] = "a153";
			array2[206] = "a154";
			array2[207] = "a155";
			array2[208] = "a156";
			array2[209] = "a157";
			array2[210] = "a158";
			array2[211] = "a159";
			array2[212] = "a160";
			array2[213] = "a161";
			array2[214] = "a163";
			array2[215] = "a164";
			array2[216] = "a196";
			array2[217] = "a165";
			array2[218] = "a192";
			array2[219] = "a166";
			array2[220] = "a167";
			array2[221] = "a168";
			array2[222] = "a169";
			array2[223] = "a170";
			array2[224] = "a171";
			array2[225] = "a172";
			array2[226] = "a173";
			array2[227] = "a162";
			array2[228] = "a174";
			array2[229] = "a175";
			array2[230] = "a176";
			array2[231] = "a177";
			array2[232] = "a178";
			array2[233] = "a179";
			array2[234] = "a193";
			array2[235] = "a180";
			array2[236] = "a199";
			array2[237] = "a181";
			array2[238] = "a200";
			array2[239] = "a182";
			array2[241] = "a201";
			array2[242] = "a183";
			array2[243] = "a184";
			array2[244] = "a197";
			array2[245] = "a185";
			array2[246] = "a194";
			array2[247] = "a198";
			array2[248] = "a186";
			array2[249] = "a195";
			array2[250] = "a187";
			array2[251] = "a188";
			array2[252] = "a189";
			array2[253] = "a190";
			array2[254] = "a191";
			this.zapfDingbatsEncoding = array2;
			this.macRomanEncoding = new string[]
			{
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				"space",
				"exclam",
				"quotedbl",
				"numbersign",
				"dollar",
				"percent",
				"ampersand",
				"quotesingle",
				"parenleft",
				"parenright",
				"asterisk",
				"plus",
				"comma",
				"hyphen",
				"period",
				"slash",
				"zero",
				"one",
				"two",
				"three",
				"four",
				"five",
				"six",
				"seven",
				"eight",
				"nine",
				"colon",
				"semicolon",
				"less",
				"equal",
				"greater",
				"question",
				"at",
				"A",
				"B",
				"C",
				"D",
				"E",
				"F",
				"G",
				"H",
				"I",
				"J",
				"K",
				"L",
				"M",
				"N",
				"O",
				"P",
				"Q",
				"R",
				"S",
				"T",
				"U",
				"V",
				"W",
				"X",
				"Y",
				"Z",
				"bracketleft",
				"backslash",
				"bracketright",
				"asciicircum",
				"underscore",
				"grave",
				"a",
				"b",
				"c",
				"d",
				"e",
				"f",
				"g",
				"h",
				"i",
				"j",
				"k",
				"l",
				"m",
				"n",
				"o",
				"p",
				"q",
				"r",
				"s",
				"t",
				"u",
				"v",
				"w",
				"x",
				"y",
				"z",
				"braceleft",
				"bar",
				"braceright",
				"asciitilde",
				null,
				"Adieresis",
				"Aring",
				"Ccedilla",
				"Eacute",
				"Ntilde",
				"Odieresis",
				"Udieresis",
				"aacute",
				"agrave",
				"acircumflex",
				"adieresis",
				"atilde",
				"aring",
				"ccedilla",
				"eacute",
				"egrave",
				"ecircumflex",
				"edieresis",
				"iacute",
				"igrave",
				"icircumflex",
				"idieresis",
				"ntilde",
				"oacute",
				"ograve",
				"ocircumflex",
				"odieresis",
				"otilde",
				"uacute",
				"ugrave",
				"ucircumflex",
				"udieresis",
				"dagger",
				"degree",
				"cent",
				"sterling",
				"section",
				"bullet",
				"paragraph",
				"germandbls",
				"registered",
				"copyright",
				"trademark",
				"acute",
				"dieresis",
				"notequal",
				"AE",
				"Oslash",
				"infinity",
				"plusminus",
				"lessequal",
				"greaterequal",
				"yen",
				"mu",
				"partialdiff",
				"summation",
				"product",
				"pi",
				"integral",
				"ordfeminine",
				"ordmasculine",
				"Omega",
				"ae",
				"oslash",
				"questiondown",
				"exclamdown",
				"logicalnot",
				"radical",
				"florin",
				"approxequal",
				"Delta",
				"guillemotleft",
				"guillemotright",
				"ellipsis",
				"space",
				"Agrave",
				"Atilde",
				"Otilde",
				"OE",
				"oe",
				"endash",
				"emdash",
				"quotedblleft",
				"quotedblright",
				"quoteleft",
				"quoteright",
				"divide",
				"lozenge",
				"ydieresis",
				"Ydieresis",
				"fraction",
				"currency",
				"guilsinglleft",
				"guilsinglright",
				"fi",
				"fl",
				"daggerdbl",
				"periodcentered",
				"quotesinglbase",
				"quotedblbase",
				"perthousand",
				"Acircumflex",
				"Ecircumflex",
				"Aacute",
				"Edieresis",
				"Egrave",
				"Iacute",
				"Icircumflex",
				"Idieresis",
				"Igrave",
				"Oacute",
				"Ocircumflex",
				"apple",
				"Ograve",
				"Uacute",
				"Ucircumflex",
				"Ugrave",
				"dotlessi",
				"circumflex",
				"tilde",
				"macron",
				"breve",
				"dotaccent",
				"ring",
				"cedilla",
				"hungarumlaut",
				"ogonek",
				"caron"
			};
			this.baseMatrix = new double[6];
			this.m_bstrLog = new StringBuilder();
			//base..tor();
			this.m_pManager = null;
			this.m_pDoc = null;
			this.m_pPage = null;
			this.m_pResources = null;
			this.m_pRavenOut = null;
			this.m_pState = null;
			this.m_bDebug = false;
			this.m_bFontChanged = false;
			this.m_nFormDepth = 0;
			this.m_nIgnoreUndef = 0;
			this.m_pIccProfile = null;
			this.initBuiltinFontTables();
			this.InitializeOperatorArray();
			this.axialColorDelta = (double)PdfState.dblToCol(0.00390625);
			this.radialColorDelta = (double)PdfState.dblToCol(0.00390625);
		}
		internal void Create(PdfParam pParam)
		{
			float num = 72f;
			float num2 = 72f;
			if (pParam.IsSet("ResolutionX"))
			{
				num = pParam.Number("ResolutionX");
			}
			if (pParam.IsSet("ResolutionY"))
			{
				num2 = pParam.Number("ResolutionY");
			}
			if (num <= 0f || num2 <= 0f)
			{
				AuxException.Throw("ResolutuionX/ResolutionY must be a positive value.", PdfErrors._ERROR_PREVIEW1);
			}
			bool flag = pParam.IsTrue("IgnoreRotate");
			bool flag2 = pParam.IsTrue("IgnoreCropBox");
			this.m_bDebug = pParam.IsTrue("Debug");
			bool flag3 = pParam.IsTrue("FastCMYK");
			int i;
			for (i = (flag ? 0 : this.m_pPage.m_nRotate); i < 0; i += 360)
			{
			}
			while (i > 360)
			{
				i -= 360;
			}
			PdfRect ptrCropBox = this.m_pPage.m_ptrCropBox;
			PdfRect ptrMediaBox = this.m_pPage.m_ptrMediaBox;
			int num3 = (int)this.m_pPage.Width;
			int num4 = (int)this.m_pPage.Height;
			int num5;
			int num6;
			if (flag2)
			{
				num3 = (int)ptrMediaBox.m_fRight;
				num4 = (int)ptrMediaBox.m_fTop;
				num5 = (int)ptrMediaBox.m_fLeft;
				num6 = (int)ptrMediaBox.m_fBottom;
			}
			else
			{
				num3 = (int)ptrCropBox.m_fRight;
				num4 = (int)ptrCropBox.m_fTop;
				num5 = (int)ptrCropBox.m_fLeft;
				num6 = (int)ptrCropBox.m_fBottom;
			}
			if ((num3 - num5) * (num4 - num6) <= 0)
			{
				AuxException.Throw("Invalid page width and/or height.", PdfErrors._ERROR_PREVIEW1);
			}
			if (!flag3)
			{
				this.m_pIccProfile = new IccProfile();
				this.m_pIccProfile.LoadProfileFromResource();
			}
			RavenColor ravenColor = new RavenColor();
			ravenColor[0] = (ravenColor[1] = (ravenColor[2] = 255));
			this.m_pRavenOut = new PdfRavenOutputDev(RavenColorMode.ravenModeBGR8, 1, false, ravenColor, true, true);
			this.m_pRavenOut.m_pDoc = this.m_pDoc;
			this.m_pRavenOut.m_pFontEngine = new RavenFontEngine(true, 1, true);
			this.m_pState = new PdfState((double)num, (double)num2, (double)num5, (double)num6, (double)num3, (double)num4, i, this.m_pRavenOut.upsideDown(), this);
			this.m_clip = ClipType.clipNone;
			this.m_pRavenOut.startPage(1, this.m_pState);
			if (this.m_pRavenOut.getBitmap().getDataPtr() == null)
			{
				AuxException.Throw("Out of memory when attempting to allocate the image bitmap.", PdfErrors._ERROR_OUTOFMEMORY);
			}
			for (int j = 0; j < 6; j++)
			{
				this.baseMatrix[j] = this.m_pState.getCTM()[j];
			}
			if (!flag2 && (ptrCropBox.m_fBottom != ptrMediaBox.m_fBottom || ptrCropBox.m_fLeft != ptrMediaBox.m_fLeft || ptrCropBox.m_fRight != ptrMediaBox.m_fRight || ptrCropBox.m_fTop != ptrMediaBox.m_fTop))
			{
				this.m_pState.moveTo((double)ptrCropBox.m_fLeft, (double)ptrCropBox.m_fBottom);
				this.m_pState.lineTo((double)ptrCropBox.m_fRight, (double)ptrCropBox.m_fBottom);
				this.m_pState.lineTo((double)ptrCropBox.m_fRight, (double)ptrCropBox.m_fTop);
				this.m_pState.lineTo((double)ptrCropBox.m_fLeft, (double)ptrCropBox.m_fTop);
				this.m_pState.closePath();
				this.m_pState.clip();
				this.m_pRavenOut.clip(this.m_pState);
				this.m_pState.clearPath();
			}
			this.m_pResources = new PdfResources(this.m_pPage.m_pPageObj.m_objAttributes.GetObjectByName("Resources"), this.m_pPage.m_pPageObj.m_objAttributes, this.m_pDoc, this.m_pPage, this);
			PdfStream pCode = new PdfStream();
			this.m_pDoc.m_pInput.ParseContentStreamsForPreview(this.m_pPage, ref pCode);
			this.RenderImage(pCode);
			this.DrawAnnotations();
			RavenBitmap bitmap = this.m_pRavenOut.getBitmap();
			this.m_nPixelWidth = bitmap.Width;
			this.m_nPixelHeight = bitmap.Height;
		}
		private void RenderImage(PdfStream pCode)
		{
			PdfObject[] array = new PdfObject[20];
			int nNumArg = 0;
			PdfInput pdfInput = new PdfInput(ref pCode, this.m_pDoc, "");
			pdfInput.Next();
			pdfInput.SkipWhite();
			this.m_pInput = pdfInput;
			while (pdfInput.m_dwPtr < pdfInput.m_dwBytesRead)
			{
				PdfObject pdfObject = pdfInput.ParseObject(null, 0, 0);
				enumType nType = pdfObject.m_nType;
				if (nType == enumType.pdfOther)
				{
					string bstrOriginalValue = ((PdfOther)pdfObject).m_bstrOriginalValue;
					this.HandlePostScriptCommand(bstrOriginalValue, array, nNumArg);
					nNumArg = 0;
				}
				else
				{
					array[nNumArg++] = pdfObject;
				}
			}
		}
		internal void HandlePostScriptCommand(string command, PdfObject[] args, int nNumArg)
		{
			PdfPreview.PreviewOperator previewOperator = this.FindOp(command);
			if (previewOperator == null)
			{
				this.LogError("Unknown operator '{0}'.", command);
				return;
			}
			if (previewOperator.numArgs >= 0)
			{
				if (nNumArg < previewOperator.numArgs)
				{
					string msg = string.Format("Too few arguments ({0}) to operator '{1}', should be {2}.", nNumArg, command, previewOperator.numArgs);
					this.LogError(msg, null);
					return;
				}
				if (nNumArg > previewOperator.numArgs)
				{
					string msg = string.Format("Too many arguments ({0}) to operator '{1}', should be {2}. Ignoring extra ones.", nNumArg, command, previewOperator.numArgs);
					this.LogError(msg, null);
				}
			}
			else
			{
				if (nNumArg > -previewOperator.numArgs)
				{
					string msg = string.Format("Too many arguments ({0}) to operator '{1}', should be no more than {2}. Ignoring extra ones.", nNumArg, command, -previewOperator.numArgs);
					this.LogError(msg, null);
				}
			}
			for (int i = 0; i < nNumArg; i++)
			{
				if (!this.CheckArg(args[i], previewOperator.tchk[i]))
				{
					string msg = string.Format("Argument {0} to operator '{1}' is of wrong type ({2}), should be {3}.", new object[]
					{
						i,
						command,
						(int)args[i].m_nType,
						(int)previewOperator.tchk[i]
					});
					this.LogError(msg, null);
					return;
				}
			}
			try
			{
				previewOperator.func(args, nNumArg);
			}
			catch (PdfException ex)
			{
				this.LogError(ex.Message, null);
			}
		}
		private bool CheckArg(PdfObject arg, enumType type)
		{
			switch (type)
			{
			case enumType.pdfNull:
				return false;
			case enumType.pdfName:
			case enumType.pdfNumber:
			case enumType.pdfArray:
			case enumType.pdfBool:
			case enumType.pdfString:
				return arg.m_nType == type;
			case enumType.pdfProps:
				return arg.m_nType == enumType.pdfDictionary || arg.m_nType == enumType.pdfName;
			case enumType.pdfSCN:
				return arg.m_nType == enumType.pdfNumber || arg.m_nType == enumType.pdfName;
			}
			return false;
		}
		private PdfPreview.PreviewOperator FindOp(string command)
		{
			int num = 0;
			int num2 = -1;
			int num3 = PdfPreview.m_arrOperators.Length;
			while (num3 - num2 > 1)
			{
				int num4 = (num2 + num3) / 2;
				num = string.Compare(PdfPreview.m_arrOperators[num4].name, command, StringComparison.Ordinal);
				if (num < 0)
				{
					num2 = num4;
				}
				else
				{
					if (num > 0)
					{
						num3 = num4;
					}
					else
					{
						num3 = (num2 = num4);
					}
				}
			}
			if (num != 0)
			{
				return null;
			}
			return PdfPreview.m_arrOperators[num2];
		}
		internal void LogError(string msg1)
		{
			this.LogError(msg1, null);
		}
		internal void LogError(string msg1, string msg2)
		{
			if (!this.m_bDebug)
			{
				return;
			}
			if (msg2 == null)
			{
				this.m_bstrLog.Append(msg1);
			}
			else
			{
				this.m_bstrLog.Append(string.Format(msg1, msg2));
			}
			this.m_bstrLog.Append("\r\n\r\n");
		}
		internal void getRGB(PdfColor color, PdfRGB rgb)
		{
			double[] array = new double[4];
			double[] array2 = new double[3];
			array[0] = PdfState.colToDbl(color.c[0]);
			array[1] = PdfState.colToDbl(color.c[1]);
			array[2] = PdfState.colToDbl(color.c[2]);
			array[3] = PdfState.colToDbl(color.c[3]);
			this.m_pIccProfile.CMYK2RGB(array, array2);
			rgb.r = PdfState.dblToCol(array2[0]);
			rgb.g = PdfState.dblToCol(array2[1]);
			rgb.b = PdfState.dblToCol(array2[2]);
		}
		internal Bitmap SaveHelper()
		{
			RavenBitmap bitmap = this.m_pRavenOut.getBitmap();
			Bitmap bitmap2 = new Bitmap(bitmap.width, bitmap.height);
			Rectangle rect = new Rectangle(0, 0, bitmap2.Width, bitmap2.Height);
			BitmapData bitmapData = bitmap2.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
			IntPtr scan = bitmapData.Scan0;
			for (int i = 0; i < bitmap2.Height; i++)
			{
				Marshal.Copy(bitmap.getDataPtr(), bitmap2.Width * 3 * i, new IntPtr((int)scan + bitmapData.Stride * i), bitmap2.Width * 3);
			}
			bitmap2.UnlockBits(bitmapData);
			return bitmap2;
		}
		public string Save(string Path)
		{
			return this.Save(Path, true);
		}
		public string Save(string Path, bool Overwrite)
		{
			AuxFile auxFile = new AuxFile(Path);
			if (!Overwrite)
			{
				auxFile.CreateUniqueFileName();
			}
			bool flag = auxFile.m_bstrPath.Length > 3 && string.Compare(auxFile.m_bstrPath.Substring(auxFile.m_bstrPath.Length - 3, 3), "jpg", true) == 0;
			using (Bitmap bitmap = this.SaveHelper())
			{
				bitmap.Save(auxFile.m_bstrPath, flag ? ImageFormat.Jpeg : ImageFormat.Png);
			}
			return auxFile.m_bstrFileName;
		}
		public byte[] Save()
		{
			return this.Save(ImageFormat.Png);
		}
		public byte[] Save(ImageFormat format)
		{
			byte[] result;
			using (Bitmap bitmap = this.SaveHelper())
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					bitmap.Save(memoryStream, format);
					result = memoryStream.ToArray();
				}
			}
			return result;
		}
		private void opMoveSetShowText(PdfObject[] args, int numArgs)
		{
		}
		private void opMoveShowText(PdfObject[] args, int numArgs)
		{
		}
		private void opFillStroke(PdfObject[] args, int numArgs)
		{
			if (!this.m_pState.isCurPt())
			{
				return;
			}
			if (this.m_pState.isPath())
			{
				if (this.m_pState.getFillColorSpace().getMode() == enumColorSpaceMode.csPattern)
				{
					this.doPatternFill(false);
				}
				else
				{
					this.m_pRavenOut.fill(this.m_pState);
				}
				this.m_pRavenOut.stroke(this.m_pState);
			}
			this.doEndPath();
		}
		private void opEOFillStroke(PdfObject[] args, int numArgs)
		{
			if (!this.m_pState.isCurPt())
			{
				return;
			}
			if (this.m_pState.isPath())
			{
				if (this.m_pState.getFillColorSpace().getMode() == enumColorSpaceMode.csPattern)
				{
					this.doPatternFill(true);
				}
				else
				{
					this.m_pRavenOut.eoFill(this.m_pState);
				}
				this.m_pRavenOut.stroke(this.m_pState);
			}
			this.doEndPath();
		}
		private void opBeginMarkedContent(PdfObject[] args, int numArgs)
		{
		}
		private void opBeginImage(PdfObject[] args, int numArgs)
		{
			PdfDictWithStream pdfDictWithStream = new PdfDictWithStream(null);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			string[] array = new string[15];
			int num5 = 0;
			while (this.m_pInput.m_dwPtr < this.m_pInput.m_dwBytesRead)
			{
				PdfObject pdfObject = this.m_pInput.ParseObject(null, 0, 0);
				if (pdfObject.m_nType == enumType.pdfOther && ((PdfOther)pdfObject).m_bstrOriginalValue == "ID")
				{
					break;
				}
				if (pdfObject.m_nType != enumType.pdfName)
				{
					this.LogError("Inline image dictionary key is not a Name object.");
				}
				string bstrName = ((PdfName)pdfObject).m_bstrName;
				pdfObject = this.m_pInput.ParseObject(null, 0, 0);
				pdfObject.m_bstrType = bstrName;
				if ((bstrName == "Width" || bstrName == "W") && pdfObject.m_nType == enumType.pdfNumber)
				{
					num = (int)((PdfNumber)pdfObject).m_fValue;
				}
				else
				{
					if ((bstrName == "Height" || bstrName == "H") && pdfObject.m_nType == enumType.pdfNumber)
					{
						num2 = (int)((PdfNumber)pdfObject).m_fValue;
					}
					else
					{
						if ((bstrName == "BitsPerComponent" || bstrName == "BPC") && pdfObject.m_nType == enumType.pdfNumber)
						{
							num3 = (int)((PdfNumber)pdfObject).m_fValue;
						}
						else
						{
							if ((bstrName == "ImageMask" || bstrName == "IM") && pdfObject.m_nType == enumType.pdfBool)
							{
								num3 = 1;
								num4 = 1;
							}
							else
							{
								if ((bstrName == "ColorSpace" || bstrName == "CS") && pdfObject.m_nType == enumType.pdfName)
								{
									string bstrName2 = ((PdfName)pdfObject).m_bstrName;
									if (bstrName2 == "DeviceRGB" || bstrName2 == "RGB")
									{
										num4 = 3;
									}
									else
									{
										if (bstrName2 == "DeviceGray" || bstrName2 == "G")
										{
											num4 = 1;
										}
										else
										{
											if (bstrName2 == "DeviceCMYK" || bstrName2 == "CMYK")
											{
												num4 = 4;
											}
											else
											{
												if (bstrName2 == "Indexed" || bstrName2 == "I")
												{
													num4 = 1;
												}
											}
										}
									}
								}
								else
								{
									if ((bstrName == "Filter" || bstrName == "F") && (pdfObject.m_nType == enumType.pdfName || pdfObject.m_nType == enumType.pdfArray))
									{
										if (pdfObject.m_nType == enumType.pdfName)
										{
											array[num5++] = ((PdfName)pdfObject).m_bstrName;
										}
										else
										{
											PdfArray pdfArray = (PdfArray)pdfObject;
											for (int i = 0; i < pdfArray.Size; i++)
											{
												PdfObject @object = pdfArray.GetObject(i);
												if (@object != null && @object.m_nType == enumType.pdfName)
												{
													array[num5++] = ((PdfName)@object).m_bstrName;
												}
											}
										}
									}
								}
							}
						}
					}
				}
				pdfDictWithStream.Add(pdfObject);
			}
			int num6 = (num * num3 * num4 + 7) / 8 * num2;
			while (this.m_pInput.m_dwPtr > 1 && (this.m_pInput.m_szBuffer[this.m_pInput.m_dwPtr] != 73 || this.m_pInput.m_szBuffer[this.m_pInput.m_dwPtr + 1] != 68))
			{
				this.m_pInput.m_dwPtr--;
			}
			this.m_pInput.m_dwPtr += 3;
			int dwPtr = this.m_pInput.m_dwPtr;
			int num7 = dwPtr;
			if (num5 == 0 && num6 > 0)
			{
				num7 = dwPtr + num6;
			}
			else
			{
				while (num7 < this.m_pInput.m_dwBytesRead - 3 && (!char.IsWhiteSpace((char)this.m_pInput.m_szBuffer[num7]) || this.m_pInput.m_szBuffer[num7 + 1] != 69 || this.m_pInput.m_szBuffer[num7 + 2] != 73 || !char.IsWhiteSpace((char)this.m_pInput.m_szBuffer[num7 + 3])))
				{
					num7++;
				}
			}
			byte[] array2 = new byte[num7 - dwPtr];
			this.m_pInput.ReadBytes(this.m_pInput.m_dwPtr, array2, num7 - dwPtr);
			pdfDictWithStream.m_objStream.Set(array2);
			this.m_pInput.m_dwPtr += num7 - dwPtr;
			this.m_pInput.Next();
			this.m_pInput.ApplyFilters(pdfDictWithStream.m_objStream, array, num5, pdfDictWithStream);
			this.doImage(null, pdfDictWithStream, true);
		}
		private void opBeginText(PdfObject[] args, int numArgs)
		{
		}
		private void opBeginIgnoreUndef(PdfObject[] args, int numArgs)
		{
			this.m_nIgnoreUndef++;
		}
		private void opSetStrokeColorSpace(PdfObject[] args, int numArgs)
		{
			PdfColor pdfColor = new PdfColor();
			this.m_pState.setStrokePattern(null);
			PdfObject pdfObject = this.m_pResources.LookupColorSpace(((PdfName)args[0]).m_bstrName);
			PdfColorSpaceTop pdfColorSpaceTop;
			if (pdfObject == null)
			{
				pdfColorSpaceTop = PdfColorSpaceTop.parse(args[0], this);
			}
			else
			{
				pdfColorSpaceTop = PdfColorSpaceTop.parse(pdfObject, this);
			}
			if (pdfColorSpaceTop != null)
			{
				this.m_pState.setStrokeColorSpace(pdfColorSpaceTop);
			}
			else
			{
				this.LogError("CS operator handler: bad color space.");
			}
			for (int i = 0; i < 32; i++)
			{
				pdfColor.c[i] = 0;
			}
			this.m_pState.setStrokeColor(pdfColor);
			this.m_pRavenOut.updateStrokeColor(this.m_pState);
		}
		private void opMarkPoint(PdfObject[] args, int numArgs)
		{
		}
		private void opXObject(PdfObject[] args, int numArgs)
		{
			PdfObject pdfObject = this.m_pResources.LookupXObject(((PdfName)args[0]).m_bstrName);
			if (pdfObject == null)
			{
				this.LogError("XObject name '{0}' not found.", ((PdfName)args[0]).m_bstrName);
				return;
			}
			if (pdfObject.m_nType != enumType.pdfDictWithStream)
			{
				this.LogError("XObject name '{0}' not found.", ((PdfName)args[0]).m_bstrName);
				return;
			}
			PdfObject objectByName = ((PdfDictWithStream)pdfObject).GetObjectByName("Subtype");
			if (objectByName == null || objectByName.m_nType != enumType.pdfName)
			{
				this.LogError("XObject Subtype is of wrong type or missing.");
				return;
			}
			string bstrName = ((PdfName)objectByName).m_bstrName;
			if (string.Compare(bstrName, "Image") == 0)
			{
				PdfObject reff = this.m_pResources.LookupXObject(((PdfName)args[0]).m_bstrName);
				this.doImage(reff, (PdfDictWithStream)pdfObject, false);
				return;
			}
			if (string.Compare(bstrName, "Form") == 0)
			{
				this.doForm(pdfObject);
				return;
			}
			if (string.Compare(bstrName, "PS") == 0)
			{
				this.LogError("opXObject, PS part, not yet implemented.");
				return;
			}
			this.LogError("XObject subtype '{0}' is unknown.", bstrName);
		}
		private void opEndImage(PdfObject[] args, int numArgs)
		{
		}
		private void opEndMarkedContent(PdfObject[] args, int numArgs)
		{
		}
		private void opEndText(PdfObject[] args, int numArgs)
		{
		}
		private void opEndIgnoreUndef(PdfObject[] args, int numArgs)
		{
			if (this.m_nIgnoreUndef > 0)
			{
				this.m_nIgnoreUndef--;
			}
		}
		private void opFill(PdfObject[] args, int numArgs)
		{
			if (!this.m_pState.isCurPt())
			{
				return;
			}
			if (this.m_pState.isPath())
			{
				if (this.m_pState.getFillColorSpace().getMode() == enumColorSpaceMode.csPattern)
				{
					this.doPatternFill(false);
				}
				else
				{
					this.m_pRavenOut.fill(this.m_pState);
				}
			}
			this.doEndPath();
		}
		private void opSetStrokeGray(PdfObject[] args, int numArgs)
		{
			PdfColor pdfColor = new PdfColor();
			this.m_pState.setStrokePattern(null);
			this.m_pState.setStrokeColorSpace(new PdfDeviceGrayColorSpace());
			pdfColor.c[0] = PdfState.dblToCol(((PdfNumber)args[0]).m_fValue);
			this.m_pState.setStrokeColor(pdfColor);
			this.m_pRavenOut.updateStrokeColor(this.m_pState);
		}
		private void opImageData(PdfObject[] args, int numArgs)
		{
		}
		private void opSetLineCap(PdfObject[] args, int numArgs)
		{
			this.m_pState.setLineCap((int)((PdfNumber)args[0]).m_fValue);
			this.m_pRavenOut.updateLineCap(this.m_pState);
		}
		private void opSetStrokeCMYKColor(PdfObject[] args, int numArgs)
		{
			PdfColor pdfColor = new PdfColor();
			this.m_pState.setStrokePattern(null);
			this.m_pState.setStrokeColorSpace(new PdfDeviceCMYKColorSpace(this));
			for (int i = 0; i < 4; i++)
			{
				pdfColor.c[i] = PdfState.dblToCol(((PdfNumber)args[i]).m_fValue);
			}
			this.m_pState.setStrokeColor(pdfColor);
			this.m_pRavenOut.updateStrokeColor(this.m_pState);
		}
		private void opSetMiterLimit(PdfObject[] args, int numArgs)
		{
			this.m_pState.setMiterLimit(((PdfNumber)args[0]).m_fValue);
			this.m_pRavenOut.updateMiterLimit(this.m_pState);
		}
		private void opRestore(PdfObject[] args, int numArgs)
		{
			this.restoreState();
		}
		private void opSetStrokeRGBColor(PdfObject[] args, int numArgs)
		{
			PdfColor pdfColor = new PdfColor();
			this.m_pState.setStrokePattern(null);
			this.m_pState.setStrokeColorSpace(new PdfDeviceRGBColorSpace());
			for (int i = 0; i < 3; i++)
			{
				pdfColor.c[i] = PdfState.dblToCol(((PdfNumber)args[i]).m_fValue);
			}
			this.m_pState.setStrokeColor(pdfColor);
			this.m_pRavenOut.updateStrokeColor(this.m_pState);
		}
		private void opStroke(PdfObject[] args, int numArgs)
		{
			if (!this.m_pState.isCurPt())
			{
				return;
			}
			if (this.m_pState.isPath())
			{
				this.m_pRavenOut.stroke(this.m_pState);
			}
			this.doEndPath();
		}
		private void opSetStrokeColor(PdfObject[] args, int numArgs)
		{
			PdfColor pdfColor = new PdfColor();
			this.m_pState.setStrokePattern(null);
			for (int i = 0; i < numArgs; i++)
			{
				pdfColor.c[i] = PdfState.dblToCol(((PdfNumber)args[i]).m_fValue);
			}
			this.m_pState.setStrokeColor(pdfColor);
			this.m_pRavenOut.updateStrokeColor(this.m_pState);
		}
		private void opSetStrokeColorN(PdfObject[] args, int numArgs)
		{
			PdfColor pdfColor = new PdfColor();
			if (this.m_pState.getStrokeColorSpace().getMode() == enumColorSpaceMode.csPattern)
			{
				if (numArgs > 1)
				{
					int num = 0;
					while (num < numArgs && num < 4)
					{
						if (args[num].m_nType == enumType.pdfNumber)
						{
							pdfColor.c[num] = PdfState.dblToCol(((PdfNumber)args[num]).m_fValue);
						}
						num++;
					}
					this.m_pState.setStrokeColor(pdfColor);
					this.m_pRavenOut.updateStrokeColor(this.m_pState);
				}
				PdfPattern strokePattern;
				if (args[numArgs - 1].m_nType == enumType.pdfName && (strokePattern = this.m_pResources.LookupPattern(((PdfName)args[numArgs - 1]).m_bstrName)) != null)
				{
					this.m_pState.setStrokePattern(strokePattern);
					return;
				}
			}
			else
			{
				this.m_pState.setStrokePattern(null);
				int num = 0;
				while (num < numArgs && num < 4)
				{
					if (args[num].m_nType == enumType.pdfNumber)
					{
						pdfColor.c[num] = PdfState.dblToCol(((PdfNumber)args[num]).m_fValue);
					}
					num++;
				}
				this.m_pState.setStrokeColor(pdfColor);
				this.m_pRavenOut.updateStrokeColor(this.m_pState);
			}
		}
		private void opTextNextLine(PdfObject[] args, int numArgs)
		{
		}
		private void opTextMoveSet(PdfObject[] args, int numArgs)
		{
		}
		private void opShowSpaceText(PdfObject[] args, int numArgs)
		{
		}
		private void opSetTextLeading(PdfObject[] args, int numArgs)
		{
		}
		private void opSetCharSpacing(PdfObject[] args, int numArgs)
		{
		}
		private void opTextMove(PdfObject[] args, int numArgs)
		{
		}
		private void opSetFont(PdfObject[] args, int numArgs)
		{
		}
		private void opShowText(PdfObject[] args, int numArgs)
		{
		}
		private void opSetTextMatrix(PdfObject[] args, int numArgs)
		{
		}
		private void opSetTextRender(PdfObject[] args, int numArgs)
		{
		}
		private void opSetTextRise(PdfObject[] args, int numArgs)
		{
		}
		private void opSetWordSpacing(PdfObject[] args, int numArgs)
		{
		}
		private void opSetHorizScaling(PdfObject[] args, int numArgs)
		{
		}
		private void opClip(PdfObject[] args, int numArgs)
		{
			this.m_clip = ClipType.clipNormal;
		}
		private void opEOClip(PdfObject[] args, int numArgs)
		{
			this.m_clip = ClipType.clipEO;
		}
		private void opCloseFillStroke(PdfObject[] args, int numArgs)
		{
			if (!this.m_pState.isCurPt())
			{
				return;
			}
			if (this.m_pState.isPath())
			{
				this.m_pState.closePath();
				if (this.m_pState.getFillColorSpace().getMode() == enumColorSpaceMode.csPattern)
				{
					this.doPatternFill(false);
				}
				else
				{
					this.m_pRavenOut.fill(this.m_pState);
				}
				this.m_pRavenOut.stroke(this.m_pState);
			}
			this.doEndPath();
		}
		private void opCloseEOFillStroke(PdfObject[] args, int numArgs)
		{
			if (!this.m_pState.isCurPt())
			{
				return;
			}
			if (this.m_pState.isPath())
			{
				this.m_pState.closePath();
				if (this.m_pState.getFillColorSpace().getMode() == enumColorSpaceMode.csPattern)
				{
					this.doPatternFill(true);
				}
				else
				{
					this.m_pRavenOut.eoFill(this.m_pState);
				}
				this.m_pRavenOut.stroke(this.m_pState);
			}
			this.doEndPath();
		}
		private void opCurveTo(PdfObject[] args, int numArgs)
		{
			if (!this.m_pState.isCurPt())
			{
				this.LogError("c operator handler: no current point.");
				return;
			}
			double fValue = ((PdfNumber)args[0]).m_fValue;
			double fValue2 = ((PdfNumber)args[1]).m_fValue;
			double fValue3 = ((PdfNumber)args[2]).m_fValue;
			double fValue4 = ((PdfNumber)args[3]).m_fValue;
			double fValue5 = ((PdfNumber)args[4]).m_fValue;
			double fValue6 = ((PdfNumber)args[5]).m_fValue;
			this.m_pState.curveTo(fValue, fValue2, fValue3, fValue4, fValue5, fValue6);
		}
		private void opConcat(PdfObject[] args, int numArgs)
		{
			this.m_pState.concatCTM(((PdfNumber)args[0]).m_fValue, ((PdfNumber)args[1]).m_fValue, ((PdfNumber)args[2]).m_fValue, ((PdfNumber)args[3]).m_fValue, ((PdfNumber)args[4]).m_fValue, ((PdfNumber)args[5]).m_fValue);
			this.m_pRavenOut.updateCTM(this.m_pState, ((PdfNumber)args[0]).m_fValue, ((PdfNumber)args[1]).m_fValue, ((PdfNumber)args[2]).m_fValue, ((PdfNumber)args[3]).m_fValue, ((PdfNumber)args[4]).m_fValue, ((PdfNumber)args[5]).m_fValue);
			this.m_bFontChanged = true;
		}
		private void opSetFillColorSpace(PdfObject[] args, int numArgs)
		{
			PdfColor pdfColor = new PdfColor();
			this.m_pState.setFillPattern(null);
			PdfObject pdfObject = this.m_pResources.LookupColorSpace(((PdfName)args[0]).m_bstrName);
			PdfColorSpaceTop pdfColorSpaceTop;
			if (pdfObject == null)
			{
				pdfColorSpaceTop = PdfColorSpaceTop.parse(args[0], this);
			}
			else
			{
				pdfColorSpaceTop = PdfColorSpaceTop.parse(pdfObject, this);
			}
			if (pdfColorSpaceTop != null)
			{
				this.m_pState.setFillColorSpace(pdfColorSpaceTop);
				pdfColorSpaceTop.getDefaultColor(pdfColor);
				this.m_pState.setFillColor(pdfColor);
				this.m_pRavenOut.updateFillColor(this.m_pState);
				return;
			}
			this.LogError("cs operator handler: bad color space.");
		}
		private void opSetDash(PdfObject[] args, int numArgs)
		{
			PdfArray pdfArray = (PdfArray)args[0];
			int size = pdfArray.Size;
			double[] array;
			if (size == 0)
			{
				array = null;
			}
			else
			{
				array = new double[size];
				for (int i = 0; i < size; i++)
				{
					PdfObject @object = pdfArray.GetObject(i);
					array[i] = ((PdfNumber)@object).m_fValue;
				}
			}
			this.m_pState.setLineDash(array, size, ((PdfNumber)args[1]).m_fValue);
			this.m_pRavenOut.updateLineDash(this.m_pState);
		}
		private void opSetCharWidth(PdfObject[] args, int numArgs)
		{
		}
		private void opSetCacheDevice(PdfObject[] args, int numArgs)
		{
		}
		private void opEOFill(PdfObject[] args, int numArgs)
		{
			if (!this.m_pState.isCurPt())
			{
				return;
			}
			if (this.m_pState.isPath())
			{
				if (this.m_pState.getFillColorSpace().getMode() == enumColorSpaceMode.csPattern)
				{
					this.doPatternFill(true);
				}
				else
				{
					this.m_pRavenOut.eoFill(this.m_pState);
				}
			}
			this.doEndPath();
		}
		private void opSetFillGray(PdfObject[] args, int numArgs)
		{
			PdfColor pdfColor = new PdfColor();
			this.m_pState.setFillPattern(null);
			this.m_pState.setFillColorSpace(new PdfDeviceGrayColorSpace());
			pdfColor.c[0] = PdfState.dblToCol(((PdfNumber)args[0]).m_fValue);
			this.m_pState.setFillColor(pdfColor);
			this.m_pRavenOut.updateFillColor(this.m_pState);
		}
		private void opSetExtGState(PdfObject[] args, int numArgs)
		{
			PdfFunctionTop[] array = new PdfFunctionTop[4];
			enumBlendMode blendMode = enumBlendMode.bmBlendNormal;
			PdfColor pdfColor = new PdfColor();
			PdfObject[] array2 = new PdfObject[2];
			PdfObject pdfObject = this.m_pResources.LookupExtGState(((PdfName)args[0]).m_bstrName);
			if (pdfObject == null)
			{
				return;
			}
			if (pdfObject.m_nType != enumType.pdfDictionary)
			{
				this.LogError("ExtGState '{0}' must be a dictionary.", ((PdfName)args[0]).m_bstrName);
				return;
			}
			PdfObject objectByName = ((PdfDict)pdfObject).GetObjectByName("LW");
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				this.opSetLineWidth(new PdfObject[]
				{
					objectByName
				}, 1);
			}
			objectByName = ((PdfDict)pdfObject).GetObjectByName("LC");
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				this.opSetLineCap(new PdfObject[]
				{
					objectByName
				}, 1);
			}
			objectByName = ((PdfDict)pdfObject).GetObjectByName("LJ");
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				this.opSetLineJoin(new PdfObject[]
				{
					objectByName
				}, 1);
			}
			objectByName = ((PdfDict)pdfObject).GetObjectByName("M");
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				this.opSetMiterLimit(new PdfObject[]
				{
					objectByName
				}, 1);
			}
			objectByName = ((PdfDict)pdfObject).GetObjectByName("D");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 2)
			{
				array2[0] = ((PdfArray)objectByName).GetObject(0);
				array2[1] = ((PdfArray)objectByName).GetObject(1);
				if (array2[0].m_nType == enumType.pdfArray && array2[1].m_nType == enumType.pdfNumber)
				{
					this.opSetDash(array2, 2);
				}
			}
			objectByName = ((PdfDict)pdfObject).GetObjectByName("BM");
			if (objectByName != null)
			{
				if (this.m_pState.parseBlendMode(objectByName, ref blendMode))
				{
					this.m_pState.setBlendMode(blendMode);
					this.m_pRavenOut.updateBlendMode(this.m_pState);
				}
				else
				{
					this.LogError("Blend mode in ExtGState in invalid.");
				}
			}
			objectByName = ((PdfDict)pdfObject).GetObjectByName("ca");
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				this.m_pState.setFillOpacity(((PdfNumber)objectByName).m_fValue);
				this.m_pRavenOut.updateFillOpacity(this.m_pState);
			}
			objectByName = ((PdfDict)pdfObject).GetObjectByName("CA");
			if (objectByName != null && objectByName.m_nType == enumType.pdfNumber)
			{
				this.m_pState.setStrokeOpacity(((PdfNumber)objectByName).m_fValue);
				this.m_pRavenOut.updateStrokeOpacity(this.m_pState);
			}
			objectByName = ((PdfDict)pdfObject).GetObjectByName("TR2");
			if (objectByName == null)
			{
				objectByName = ((PdfDict)pdfObject).GetObjectByName("TR");
			}
			if (objectByName != null && objectByName.m_nType == enumType.pdfName && (((PdfName)objectByName).m_bstrName == "Default" || ((PdfName)objectByName).m_bstrName == "Identity"))
			{
				array[0] = (array[1] = (array[2] = (array[3] = null)));
				this.m_pState.setTransfer(array);
				this.m_pRavenOut.updateTransfer(this.m_pState);
			}
			else
			{
				if (objectByName != null && objectByName.m_nType == enumType.pdfArray && ((PdfArray)objectByName).Size == 4)
				{
					int i;
					for (i = 0; i < 4; i++)
					{
						PdfObject pdfObject2 = ((PdfArray)objectByName).GetObject(i);
						array[i] = PdfFunctionTop.parse(pdfObject2);
						if (array[i] == null)
						{
							break;
						}
					}
					if (i == 4)
					{
						this.m_pState.setTransfer(array);
						this.m_pRavenOut.updateTransfer(this.m_pState);
					}
				}
				else
				{
					if (objectByName != null && (objectByName.m_nType == enumType.pdfName || objectByName.m_nType == enumType.pdfDictionary || objectByName.m_nType == enumType.pdfDictWithStream))
					{
						if ((array[0] = PdfFunctionTop.parse(objectByName)) != null)
						{
							array[1] = (array[2] = (array[3] = null));
							this.m_pState.setTransfer(array);
							this.m_pRavenOut.updateTransfer(this.m_pState);
						}
					}
					else
					{
						if (objectByName != null)
						{
							this.LogError("Transfer function in ExtGState in invalid.");
						}
					}
				}
			}
			objectByName = ((PdfDict)pdfObject).GetObjectByName("SMask");
			if (objectByName != null)
			{
				if (objectByName.m_nType == enumType.pdfName && ((PdfName)objectByName).m_bstrName == "None")
				{
					this.m_pRavenOut.clearSoftMask(this.m_pState);
					return;
				}
				if (objectByName.m_nType == enumType.pdfDictionary)
				{
					PdfObject pdfObject2 = ((PdfDict)objectByName).GetObjectByName("S");
					bool alpha = pdfObject2 != null && pdfObject2.m_nType == enumType.pdfName && ((PdfName)pdfObject2).m_bstrName == "Alpha";
					array[0] = null;
					pdfObject2 = ((PdfDict)objectByName).GetObjectByName("TR");
					if (pdfObject2 != null)
					{
						if (pdfObject2.m_nType == enumType.pdfName && (((PdfName)pdfObject2).m_bstrName == "Default" || ((PdfName)pdfObject2).m_bstrName == "Identity"))
						{
							array[0] = null;
						}
						else
						{
							array[0] = PdfFunctionTop.parse(pdfObject2);
							if (array[0].getInputSize() != 1 || array[0].getOutputSize() != 1)
							{
								this.LogError("Soft mask's Transfer function in ExtGState in invalid.");
								array[0] = null;
							}
						}
					}
					pdfObject2 = ((PdfDict)objectByName).GetObjectByName("BC");
					bool flag = pdfObject2 != null && pdfObject2.m_nType == enumType.pdfArray;
					PdfObject pdfObject3;
					if (flag)
					{
						int i;
						for (i = 0; i < 32; i++)
						{
							pdfColor.c[i] = 0;
						}
						i = 0;
						while (i < ((PdfArray)pdfObject2).Size && i < 32)
						{
							pdfObject3 = ((PdfArray)pdfObject2).GetObject(i);
							if (pdfObject3 != null && pdfObject3.m_nType == enumType.pdfNumber)
							{
								pdfColor.c[i] = PdfState.dblToCol(((PdfNumber)pdfObject3).m_fValue);
							}
							i++;
						}
					}
					pdfObject2 = ((PdfDict)objectByName).GetObjectByName("G");
					if (pdfObject2 == null || pdfObject2.m_nType != enumType.pdfDictWithStream)
					{
						this.LogError("Soft mask in ExtGState is invalid (missing group.)");
						return;
					}
					pdfObject3 = ((PdfDict)pdfObject2).GetObjectByName("Group");
					if (pdfObject3 == null || pdfObject3.m_nType != enumType.pdfDictionary)
					{
						this.LogError("Soft mask in ExtGState is invalid (missing group.)");
						return;
					}
					PdfColorSpaceTop pdfColorSpaceTop = null;
					bool isolated;
					bool knockout = isolated = false;
					PdfObject objectByName2 = ((PdfDict)pdfObject3).GetObjectByName("CS");
					if (objectByName2 != null)
					{
						pdfColorSpaceTop = PdfColorSpaceTop.parse(objectByName2, this);
					}
					objectByName2 = ((PdfDict)pdfObject3).GetObjectByName("I");
					if (objectByName2 != null && objectByName2.m_nType == enumType.pdfBool)
					{
						isolated = ((PdfBool)objectByName2).m_bValue;
					}
					objectByName2 = ((PdfDict)pdfObject3).GetObjectByName("K");
					if (objectByName2 != null && objectByName2.m_nType == enumType.pdfBool)
					{
						knockout = ((PdfBool)objectByName2).m_bValue;
					}
					if (!flag)
					{
						if (pdfColorSpaceTop != null)
						{
							pdfColorSpaceTop.getDefaultColor(pdfColor);
						}
						else
						{
							for (int i = 0; i < 32; i++)
							{
								pdfColor.c[i] = 0;
							}
						}
					}
					this.doSoftMask(pdfObject2, alpha, pdfColorSpaceTop, isolated, knockout, array[0], pdfColor);
					if (array[0] != null)
					{
						return;
					}
				}
				else
				{
					if (objectByName != null)
					{
						this.LogError("Soft mask in ExtGState is invalid.");
					}
				}
			}
		}
		private void opClosePath(PdfObject[] args, int numArgs)
		{
			if (!this.m_pState.isCurPt())
			{
				this.LogError("h operator handler: no current point.");
				return;
			}
			this.m_pState.closePath();
		}
		private void opSetFlat(PdfObject[] args, int numArgs)
		{
			this.m_pState.setFlatness((int)((PdfNumber)args[0]).m_fValue);
			this.m_pRavenOut.updateFlatness(this.m_pState);
		}
		private void opSetLineJoin(PdfObject[] args, int numArgs)
		{
			this.m_pState.setLineJoin((int)((PdfNumber)args[0]).m_fValue);
			this.m_pRavenOut.updateLineJoin(this.m_pState);
		}
		private void opSetFillCMYKColor(PdfObject[] args, int numArgs)
		{
			PdfColor pdfColor = new PdfColor();
			this.m_pState.setFillPattern(null);
			this.m_pState.setFillColorSpace(new PdfDeviceCMYKColorSpace(this));
			for (int i = 0; i < 4; i++)
			{
				pdfColor.c[i] = PdfState.dblToCol(((PdfNumber)args[i]).m_fValue);
			}
			this.m_pState.setFillColor(pdfColor);
			this.m_pRavenOut.updateFillColor(this.m_pState);
		}
		private void opLineTo(PdfObject[] args, int numArgs)
		{
			if (!this.m_pState.isCurPt())
			{
				this.LogError("l operator handler: no current point.");
				return;
			}
			this.m_pState.lineTo(((PdfNumber)args[0]).m_fValue, ((PdfNumber)args[1]).m_fValue);
		}
		private void opMoveTo(PdfObject[] args, int numArgs)
		{
			this.m_pState.moveTo(((PdfNumber)args[0]).m_fValue, ((PdfNumber)args[1]).m_fValue);
		}
		private void opEndPath(PdfObject[] args, int numArgs)
		{
			this.doEndPath();
		}
		private void opSave(PdfObject[] args, int numArgs)
		{
			this.saveState();
		}
		private void opRectangle(PdfObject[] args, int numArgs)
		{
			double fValue = ((PdfNumber)args[0]).m_fValue;
			double fValue2 = ((PdfNumber)args[1]).m_fValue;
			double fValue3 = ((PdfNumber)args[2]).m_fValue;
			double fValue4 = ((PdfNumber)args[3]).m_fValue;
			this.m_pState.moveTo(fValue, fValue2);
			this.m_pState.lineTo(fValue + fValue3, fValue2);
			this.m_pState.lineTo(fValue + fValue3, fValue2 + fValue4);
			this.m_pState.lineTo(fValue, fValue2 + fValue4);
			this.m_pState.closePath();
		}
		private void opSetFillRGBColor(PdfObject[] args, int numArgs)
		{
			PdfColor pdfColor = new PdfColor();
			this.m_pState.setFillPattern(null);
			this.m_pState.setFillColorSpace(new PdfDeviceRGBColorSpace());
			for (int i = 0; i < 3; i++)
			{
				pdfColor.c[i] = PdfState.dblToCol(((PdfNumber)args[i]).m_fValue);
			}
			this.m_pState.setFillColor(pdfColor);
			this.m_pRavenOut.updateFillColor(this.m_pState);
		}
		private void opSetRenderingIntent(PdfObject[] args, int numArgs)
		{
		}
		private void opCloseStroke(PdfObject[] args, int numArgs)
		{
			if (!this.m_pState.isCurPt())
			{
				return;
			}
			if (this.m_pState.isPath())
			{
				this.m_pState.closePath();
				this.m_pRavenOut.stroke(this.m_pState);
			}
			this.doEndPath();
		}
		private void opSetFillColor(PdfObject[] args, int numArgs)
		{
			PdfColor pdfColor = new PdfColor();
			this.m_pState.setFillPattern(null);
			for (int i = 0; i < numArgs; i++)
			{
				pdfColor.c[i] = PdfState.dblToCol(((PdfNumber)args[i]).m_fValue);
			}
			this.m_pState.setFillColor(pdfColor);
			this.m_pRavenOut.updateFillColor(this.m_pState);
		}
		private void opSetFillColorN(PdfObject[] args, int numArgs)
		{
			PdfColor pdfColor = new PdfColor();
			if (this.m_pState.getFillColorSpace().getMode() == enumColorSpaceMode.csPattern)
			{
				if (numArgs > 1)
				{
					int num = 0;
					while (num < numArgs && num < 4)
					{
						if (args[num].m_nType == enumType.pdfNumber)
						{
							pdfColor.c[num] = PdfState.dblToCol(((PdfNumber)args[num]).m_fValue);
						}
						num++;
					}
					this.m_pState.setFillColor(pdfColor);
					this.m_pRavenOut.updateFillColor(this.m_pState);
				}
				PdfPattern fillPattern;
				if (args[numArgs - 1].m_nType == enumType.pdfName && (fillPattern = this.m_pResources.LookupPattern(((PdfName)args[numArgs - 1]).m_bstrName)) != null)
				{
					this.m_pState.setFillPattern(fillPattern);
					return;
				}
			}
			else
			{
				this.m_pState.setFillPattern(null);
				int num = 0;
				while (num < numArgs && num < 4)
				{
					if (args[num].m_nType == enumType.pdfNumber)
					{
						pdfColor.c[num] = PdfState.dblToCol(((PdfNumber)args[num]).m_fValue);
					}
					num++;
				}
				this.m_pState.setFillColor(pdfColor);
				this.m_pRavenOut.updateFillColor(this.m_pState);
			}
		}
		private void opShFill(PdfObject[] args, int numArgs)
		{
			double x = 0.0;
			double y = 0.0;
			double x2 = 0.0;
			double y2 = 0.0;
			PdfShading pdfShading = this.m_pResources.LookupShading(((PdfName)args[0]).m_bstrName);
			if (pdfShading == null)
			{
				return;
			}
			PdfState oldState = this.saveStateStack();
			if (pdfShading.getHasBBox())
			{
				pdfShading.getBBox(ref x, ref y, ref x2, ref y2);
				this.m_pState.moveTo(x, y);
				this.m_pState.lineTo(x2, y);
				this.m_pState.lineTo(x2, y2);
				this.m_pState.lineTo(x, y2);
				this.m_pState.closePath();
				this.m_pState.clip();
				this.m_pRavenOut.clip(this.m_pState);
				this.m_pState.clearPath();
			}
			this.m_pState.setFillColorSpace(pdfShading.getColorSpace().copy());
			switch (pdfShading.getType())
			{
			case 1:
				this.doFunctionShFill((PdfFunctionShading)pdfShading);
				break;
			case 2:
				this.doAxialShFill((PdfAxialShading)pdfShading);
				break;
			case 3:
				this.doRadialShFill((PdfRadialShading)pdfShading);
				break;
			}
			this.restoreStateStack(oldState);
		}
		private void doFunctionShFill(PdfFunctionShading shading)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			PdfColor[] array = new PdfColor[4];
			shading.getDomain(ref num, ref num2, ref num3, ref num4);
			shading.getColor(num, num2, array[0]);
			shading.getColor(num, num4, array[1]);
			shading.getColor(num3, num2, array[2]);
			shading.getColor(num3, num4, array[3]);
			this.doFunctionShFill1(shading, num, num2, num3, num4, array, 0);
		}
		private void doFunctionShFill1(PdfFunctionShading shading, double x0, double y0, double x1, double y1, PdfColor[] colors, int depth)
		{
			PdfColor pdfColor = new PdfColor();
			PdfColor pdfColor2 = new PdfColor();
			PdfColor pdfColor3 = new PdfColor();
			PdfColor pdfColor4 = new PdfColor();
			PdfColor pdfColor5 = new PdfColor();
			PdfColor pdfColor6 = new PdfColor();
			PdfColor[] array = new PdfColor[4];
			int nComps = shading.getColorSpace().getNComps();
			double[] matrix = shading.getMatrix();
			int i;
			for (i = 0; i < 4; i++)
			{
				int num = 0;
				while (num < nComps && (double)Math.Abs(colors[i].c[num] - colors[i + 1 & 3].c[num]) <= 0.00390625)
				{
					num++;
				}
				if (num < nComps)
				{
					break;
				}
			}
			double num2 = 0.5 * (x0 + x1);
			double num3 = 0.5 * (y0 + y1);
			if ((i == 4 && depth > 0) || depth == 6)
			{
				shading.getColor(num2, num3, pdfColor);
				this.m_pState.setFillColor(pdfColor);
				this.m_pRavenOut.updateFillColor(this.m_pState);
				this.m_pState.moveTo(x0 * matrix[0] + y0 * matrix[2] + matrix[4], x0 * matrix[1] + y0 * matrix[3] + matrix[5]);
				this.m_pState.lineTo(x1 * matrix[0] + y0 * matrix[2] + matrix[4], x1 * matrix[1] + y0 * matrix[3] + matrix[5]);
				this.m_pState.lineTo(x1 * matrix[0] + y1 * matrix[2] + matrix[4], x1 * matrix[1] + y1 * matrix[3] + matrix[5]);
				this.m_pState.lineTo(x0 * matrix[0] + y1 * matrix[2] + matrix[4], x0 * matrix[1] + y1 * matrix[3] + matrix[5]);
				this.m_pState.closePath();
				this.m_pRavenOut.fill(this.m_pState);
				this.m_pState.clearPath();
				return;
			}
			shading.getColor(x0, num3, pdfColor2);
			shading.getColor(x1, num3, pdfColor3);
			shading.getColor(num2, y0, pdfColor4);
			shading.getColor(num2, y1, pdfColor5);
			shading.getColor(num2, num3, pdfColor6);
			array[0] = colors[0];
			array[1] = pdfColor2;
			array[2] = pdfColor4;
			array[3] = pdfColor6;
			this.doFunctionShFill1(shading, x0, y0, num2, num3, array, depth + 1);
			array[0] = pdfColor2;
			array[1] = colors[1];
			array[2] = pdfColor6;
			array[3] = pdfColor5;
			this.doFunctionShFill1(shading, x0, num3, num2, y1, array, depth + 1);
			array[0] = pdfColor4;
			array[1] = pdfColor6;
			array[2] = colors[2];
			array[3] = pdfColor3;
			this.doFunctionShFill1(shading, num2, y0, x1, num3, array, depth + 1);
			array[0] = pdfColor6;
			array[1] = pdfColor5;
			array[2] = pdfColor3;
			array[3] = colors[3];
			this.doFunctionShFill1(shading, num2, num3, x1, y1, array, depth + 1);
		}
		private void doAxialShFill(PdfAxialShading shading)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 0.0;
			double num6 = 0.0;
			double num7 = 0.0;
			double num8 = 0.0;
			double[] array = new double[257];
			int[] array2 = new int[257];
			PdfColor pdfColor = new PdfColor();
			PdfColor pdfColor2 = new PdfColor();
			this.m_pState.getUserClipBBox(ref num, ref num2, ref num3, ref num4);
			shading.getCoords(ref num5, ref num6, ref num7, ref num8);
			double num9 = num7 - num5;
			double num10 = num8 - num6;
			bool flag = Math.Abs(num9) < 0.01 && Math.Abs(num10) < 0.01;
			bool flag2 = Math.Abs(num10) < Math.Abs(num9);
			double num12;
			double num11;
			if (flag)
			{
				num11 = (num12 = 0.0);
			}
			else
			{
				double num13 = 1.0 / (num9 * num9 + num10 * num10);
				num11 = (num12 = ((num - num5) * num9 + (num2 - num6) * num10) * num13);
				double num14 = ((num - num5) * num9 + (num4 - num6) * num10) * num13;
				if (num14 < num12)
				{
					num12 = num14;
				}
				else
				{
					if (num14 > num11)
					{
						num11 = num14;
					}
				}
				num14 = ((num3 - num5) * num9 + (num2 - num6) * num10) * num13;
				if (num14 < num12)
				{
					num12 = num14;
				}
				else
				{
					if (num14 > num11)
					{
						num11 = num14;
					}
				}
				num14 = ((num3 - num5) * num9 + (num4 - num6) * num10) * num13;
				if (num14 < num12)
				{
					num12 = num14;
				}
				else
				{
					if (num14 > num11)
					{
						num11 = num14;
					}
				}
				if (num12 < 0.0 && !shading.getExtend0())
				{
					num12 = 0.0;
				}
				if (num11 > 1.0 && !shading.getExtend1())
				{
					num11 = 1.0;
				}
			}
			double domain = shading.getDomain0();
			double domain2 = shading.getDomain1();
			int nComps = shading.getColorSpace().getNComps();
			array[0] = num12;
			array2[0] = 256;
			array[256] = num11;
			double t;
			if (num12 < 0.0)
			{
				t = domain;
			}
			else
			{
				if (num12 > 1.0)
				{
					t = domain2;
				}
				else
				{
					t = domain + (domain2 - domain) * num12;
				}
			}
			shading.getColor(t, pdfColor);
			double num15 = num5 + num12 * num9;
			double num16 = num6 + num12 * num10;
			double num18;
			double num17;
			if (flag)
			{
				num17 = (num18 = 0.0);
			}
			else
			{
				if (flag2)
				{
					num18 = (num2 - num16) / num9;
					num17 = (num4 - num16) / num9;
				}
				else
				{
					num18 = (num - num15) / -num10;
					num17 = (num3 - num15) / -num10;
				}
				if (num18 > num17)
				{
					double num19 = num18;
					num18 = num17;
					num17 = num19;
				}
			}
			double x = num15 - num18 * num10;
			double y = num16 + num18 * num9;
			double x2 = num15 - num17 * num10;
			double y2 = num16 + num17 * num9;
			for (int i = 0; i < 256; i = array2[i])
			{
				int j;
				int k;
				for (j = array2[i]; j > i + 1; j = k)
				{
					if (array[j] < 0.0)
					{
						t = domain;
					}
					else
					{
						if (array[j] > 1.0)
						{
							t = domain2;
						}
						else
						{
							t = domain + (domain2 - domain) * array[j];
						}
					}
					if (j - i <= 64)
					{
						shading.getColor(t, pdfColor2);
						k = 0;
						while (k < nComps && (double)Math.Abs(pdfColor2.c[k] - pdfColor.c[k]) <= this.axialColorDelta)
						{
							k++;
						}
						if (k == nComps)
						{
							break;
						}
					}
					k = (i + j) / 2;
					array[k] = 0.5 * (array[i] + array[j]);
					array2[i] = k;
					array2[k] = j;
				}
				for (k = 0; k < nComps; k++)
				{
					pdfColor.c[k] = (pdfColor.c[k] + pdfColor2.c[k]) / 2;
				}
				num15 = num5 + array[j] * num9;
				num16 = num6 + array[j] * num10;
				if (flag)
				{
					num17 = (num18 = 0.0);
				}
				else
				{
					if (flag2)
					{
						num18 = (num2 - num16) / num9;
						num17 = (num4 - num16) / num9;
					}
					else
					{
						num18 = (num - num15) / -num10;
						num17 = (num3 - num15) / -num10;
					}
					if (num18 > num17)
					{
						double num19 = num18;
						num18 = num17;
						num17 = num19;
					}
				}
				double num20 = num15 - num18 * num10;
				double num21 = num16 + num18 * num9;
				double num22 = num15 - num17 * num10;
				double num23 = num16 + num17 * num9;
				this.m_pState.setFillColor(pdfColor);
				this.m_pRavenOut.updateFillColor(this.m_pState);
				this.m_pState.moveTo(x, y);
				this.m_pState.lineTo(x2, y2);
				this.m_pState.lineTo(num22, num23);
				this.m_pState.lineTo(num20, num21);
				this.m_pState.closePath();
				this.m_pRavenOut.fill(this.m_pState);
				this.m_pState.clearPath();
				x = num20;
				y = num21;
				x2 = num22;
				y2 = num23;
				Array.Copy(pdfColor2.c, pdfColor.c, pdfColor2.c.Length);
			}
		}
		private void doRadialShFill(PdfRadialShading shading)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 0.0;
			double num6 = 0.0;
			double num7 = 0.0;
			double num8 = 0.0;
			double num9 = 0.0;
			double num10 = 0.0;
			PdfColor pdfColor = new PdfColor();
			PdfColor pdfColor2 = new PdfColor();
			shading.getCoords(ref num5, ref num6, ref num7, ref num8, ref num9, ref num10);
			double domain = shading.getDomain0();
			double domain2 = shading.getDomain1();
			int nComps = shading.getColorSpace().getNComps();
			double num11 = 0.0;
			double num12 = 1.0;
			if (shading.getExtend0())
			{
				if (num7 < num10)
				{
					num11 = -num7 / (num10 - num7);
				}
				else
				{
					this.m_pState.getUserClipBBox(ref num, ref num2, ref num3, ref num4);
					num11 = (Math.Sqrt((num3 - num) * (num3 - num) + (num4 - num2) * (num4 - num2)) - num7) / (num10 - num7);
					if (num11 > 0.0)
					{
						num11 = 0.0;
					}
					else
					{
						if (num11 < -20.0)
						{
							num11 = -20.0;
						}
					}
				}
			}
			if (shading.getExtend1())
			{
				if (num10 < num7)
				{
					num12 = -num7 / (num10 - num7);
				}
				else
				{
					if (num10 > num7)
					{
						this.m_pState.getUserClipBBox(ref num, ref num2, ref num3, ref num4);
						num12 = (Math.Sqrt((num3 - num) * (num3 - num) + (num4 - num2) * (num4 - num2)) - num7) / (num10 - num7);
						if (num12 < 1.0)
						{
							num11 = 1.0;
						}
						else
						{
							if (num12 > 20.0)
							{
								num12 = 20.0;
							}
						}
					}
				}
			}
			double[] cTM = this.m_pState.getCTM();
			double num13 = Math.Abs(cTM[0]);
			if (Math.Abs(cTM[1]) > num13)
			{
				num13 = Math.Abs(cTM[1]);
			}
			if (Math.Abs(cTM[2]) > num13)
			{
				num13 = Math.Abs(cTM[2]);
			}
			if (Math.Abs(cTM[3]) > num13)
			{
				num13 = Math.Abs(cTM[3]);
			}
			if (num7 > num10)
			{
				num13 *= num7;
			}
			else
			{
				num13 *= num10;
			}
			int num14;
			if (num13 < 1.0)
			{
				num14 = 3;
			}
			else
			{
				num14 = (int)(3.1415926535897931 / Math.Acos(1.0 - 0.1 / num13));
				if (num14 < 3)
				{
					num14 = 3;
				}
				else
				{
					if (num14 > 200)
					{
						num14 = 200;
					}
				}
			}
			int i = 0;
			double num15 = num11;
			double num16 = domain + num15 * (domain2 - domain);
			double num17 = num5 + num15 * (num8 - num5);
			double num18 = num6 + num15 * (num9 - num6);
			double num19 = num7 + num15 * (num10 - num7);
			if (num16 < domain)
			{
				shading.getColor(domain, pdfColor);
			}
			else
			{
				if (num16 > domain2)
				{
					shading.getColor(domain2, pdfColor);
				}
				else
				{
					shading.getColor(num16, pdfColor);
				}
			}
			while (i < 256)
			{
				int num20 = 256;
				double num21 = num11 + (double)num20 / 256.0 * (num12 - num11);
				double num22 = domain + num21 * (domain2 - domain);
				if (num22 < domain)
				{
					shading.getColor(domain, pdfColor2);
				}
				else
				{
					if (num22 > domain2)
					{
						shading.getColor(domain2, pdfColor2);
					}
					else
					{
						shading.getColor(num22, pdfColor2);
					}
				}
				while (num20 - i > 1)
				{
					int j = 0;
					while (j < nComps && (double)Math.Abs(pdfColor2.c[j] - pdfColor.c[j]) <= this.radialColorDelta)
					{
						j++;
					}
					if (j == nComps && num20 < 256)
					{
						break;
					}
					num20 = (i + num20) / 2;
					num21 = num11 + (double)num20 / 256.0 * (num12 - num11);
					num22 = domain + num21 * (domain2 - domain);
					if (num22 < domain)
					{
						shading.getColor(domain, pdfColor2);
					}
					else
					{
						if (num22 > domain2)
						{
							shading.getColor(domain2, pdfColor2);
						}
						else
						{
							shading.getColor(num22, pdfColor2);
						}
					}
				}
				double num23 = num5 + num21 * (num8 - num5);
				double num24 = num6 + num21 * (num9 - num6);
				double num25 = num7 + num21 * (num10 - num7);
				for (int j = 0; j < nComps; j++)
				{
					pdfColor.c[j] = (int)(0.5 * (double)(pdfColor.c[j] + pdfColor2.c[j]));
				}
				this.m_pState.setFillColor(pdfColor);
				this.m_pRavenOut.updateFillColor(this.m_pState);
				this.m_pState.moveTo(num17 + num19, num18);
				for (int j = 1; j < num14; j++)
				{
					double num26 = (double)j / (double)num14 * 2.0 * 3.1415926535897931;
					this.m_pState.lineTo(num17 + num19 * Math.Cos(num26), num18 + num19 * Math.Sin(num26));
				}
				this.m_pState.closePath();
				this.m_pState.moveTo(num23 + num25, num24);
				for (int j = 1; j < num14; j++)
				{
					double num26 = (double)j / (double)num14 * 2.0 * 3.1415926535897931;
					this.m_pState.lineTo(num23 + num25 * Math.Cos(num26), num24 + num25 * Math.Sin(num26));
				}
				this.m_pState.closePath();
				this.m_pRavenOut.eoFill(this.m_pState);
				this.m_pState.clearPath();
				i = num20;
				num17 = num23;
				num18 = num24;
				num19 = num25;
				Array.Copy(pdfColor2.c, pdfColor.c, pdfColor.c.Length);
			}
		}
		private void opCurveTo1(PdfObject[] args, int numArgs)
		{
			if (!this.m_pState.isCurPt())
			{
				this.LogError("v operator handler: no current point.");
				return;
			}
			double curX = this.m_pState.getCurX();
			double curY = this.m_pState.getCurY();
			double fValue = ((PdfNumber)args[0]).m_fValue;
			double fValue2 = ((PdfNumber)args[1]).m_fValue;
			double fValue3 = ((PdfNumber)args[2]).m_fValue;
			double fValue4 = ((PdfNumber)args[3]).m_fValue;
			this.m_pState.curveTo(curX, curY, fValue, fValue2, fValue3, fValue4);
		}
		private void opSetLineWidth(PdfObject[] args, int numArgs)
		{
			this.m_pState.setLineWidth(((PdfNumber)args[0]).m_fValue);
			this.m_pRavenOut.updateLineWidth(this.m_pState);
		}
		private void opCurveTo2(PdfObject[] args, int numArgs)
		{
			if (!this.m_pState.isCurPt())
			{
				this.LogError("y operator handler: no current point.");
				return;
			}
			double fValue = ((PdfNumber)args[0]).m_fValue;
			double fValue2 = ((PdfNumber)args[1]).m_fValue;
			double fValue3 = ((PdfNumber)args[2]).m_fValue;
			double fValue4 = ((PdfNumber)args[3]).m_fValue;
			double x = fValue3;
			double y = fValue4;
			this.m_pState.curveTo(fValue, fValue2, fValue3, fValue4, x, y);
		}
		internal void saveState()
		{
			this.m_pRavenOut.saveState(this.m_pState);
			this.m_pState = this.m_pState.save();
		}
		internal void restoreState()
		{
			this.m_pState = this.m_pState.restore();
			this.m_pRavenOut.restoreState(this.m_pState);
		}
		private void doPatternFill(bool eoFill)
		{
			PdfPattern fillPattern;
			if ((fillPattern = this.m_pState.getFillPattern()) == null)
			{
				return;
			}
			switch (fillPattern.getType())
			{
			case 1:
				this.doTilingPatternFill((PdfTilingPattern)fillPattern, false, eoFill, false);
				return;
			case 2:
				this.doShadingPatternFill((PdfShadingPattern)fillPattern, false, eoFill, false);
				return;
			default:
				this.LogError("Pattern type not implemented.");
				return;
			}
		}
		private void doTilingPatternFill(PdfTilingPattern tPat, bool stroke, bool eoFill, bool text)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double[] array = new double[6];
			double[] array2 = new double[6];
			double[] array3 = new double[6];
			double[] array4 = new double[6];
			PdfPatternColorSpace pdfPatternColorSpace = (PdfPatternColorSpace)(stroke ? this.m_pState.getStrokeColorSpace() : this.m_pState.getFillColorSpace());
			double[] cTM = this.m_pState.getCTM();
			double[] array5 = this.baseMatrix;
			double[] matrix = tPat.getMatrix();
			double num5 = 1.0 / (cTM[0] * cTM[3] - cTM[1] * cTM[2]);
			array2[0] = cTM[3] * num5;
			array2[1] = -cTM[1] * num5;
			array2[2] = -cTM[2] * num5;
			array2[3] = cTM[0] * num5;
			array2[4] = (cTM[2] * cTM[5] - cTM[3] * cTM[4]) * num5;
			array2[5] = (cTM[1] * cTM[4] - cTM[0] * cTM[5]) * num5;
			array3[0] = matrix[0] * array5[0] + matrix[1] * array5[2];
			array3[1] = matrix[0] * array5[1] + matrix[1] * array5[3];
			array3[2] = matrix[2] * array5[0] + matrix[3] * array5[2];
			array3[3] = matrix[2] * array5[1] + matrix[3] * array5[3];
			array3[4] = matrix[4] * array5[0] + matrix[5] * array5[2] + array5[4];
			array3[5] = matrix[4] * array5[1] + matrix[5] * array5[3] + array5[5];
			array[0] = array3[0] * array2[0] + array3[1] * array2[2];
			array[1] = array3[0] * array2[1] + array3[1] * array2[3];
			array[2] = array3[2] * array2[0] + array3[3] * array2[2];
			array[3] = array3[2] * array2[1] + array3[3] * array2[3];
			array[4] = array3[4] * array2[0] + array3[5] * array2[2] + array2[4];
			array[5] = array3[4] * array2[1] + array3[5] * array2[3] + array2[5];
			num5 = 1.0 / (array3[0] * array3[3] - array3[1] * array3[2]);
			array4[0] = array3[3] * num5;
			array4[1] = -array3[1] * num5;
			array4[2] = -array3[2] * num5;
			array4[3] = array3[0] * num5;
			array4[4] = (array3[2] * array3[5] - array3[3] * array3[4]) * num5;
			array4[5] = (array3[1] * array3[4] - array3[0] * array3[5]) * num5;
			PdfState oldState = this.saveStateStack();
			this.m_pState.setFillPattern(null);
			this.m_pState.setStrokePattern(null);
			PdfColorSpaceTop under;
			if (tPat.getPaintType() == 2 && (under = pdfPatternColorSpace.getUnder()) != null)
			{
				this.m_pState.setFillColorSpace(under.copy());
				this.m_pState.setStrokeColorSpace(under.copy());
				this.m_pState.setStrokeColor(this.m_pState.getFillColor());
				this.m_pRavenOut.updateFillColor(this.m_pState);
				this.m_pRavenOut.updateStrokeColor(this.m_pState);
			}
			else
			{
				this.m_pState.setFillColorSpace(new PdfDeviceGrayColorSpace());
				this.m_pState.setStrokeColorSpace(new PdfDeviceGrayColorSpace());
			}
			if (!stroke)
			{
				this.m_pState.setLineWidth(0.0);
				this.m_pRavenOut.updateLineWidth(this.m_pState);
			}
			if (stroke)
			{
				this.m_pState.clipToStrokePath();
				this.m_pRavenOut.clipToStrokePath(this.m_pState);
			}
			else
			{
				if (!text)
				{
					this.m_pState.clip();
					if (eoFill)
					{
						this.m_pRavenOut.eoClip(this.m_pState);
					}
					else
					{
						this.m_pRavenOut.clip(this.m_pState);
					}
				}
			}
			this.m_pState.clearPath();
			this.m_pState.getClipBBox(ref num, ref num2, ref num3, ref num4);
			if (num <= num3 && num2 <= num4)
			{
				double num7;
				double num6 = num7 = num * array4[0] + num2 * array4[2] + array4[4];
				double num9;
				double num8 = num9 = num * array4[1] + num2 * array4[3] + array4[5];
				double num10 = num * array4[0] + num4 * array4[2] + array4[4];
				double num11 = num * array4[1] + num4 * array4[3] + array4[5];
				if (num10 < num7)
				{
					num7 = num10;
				}
				else
				{
					if (num10 > num6)
					{
						num6 = num10;
					}
				}
				if (num11 < num9)
				{
					num9 = num11;
				}
				else
				{
					if (num11 > num8)
					{
						num8 = num11;
					}
				}
				num10 = num3 * array4[0] + num2 * array4[2] + array4[4];
				num11 = num3 * array4[1] + num2 * array4[3] + array4[5];
				if (num10 < num7)
				{
					num7 = num10;
				}
				else
				{
					if (num10 > num6)
					{
						num6 = num10;
					}
				}
				if (num11 < num9)
				{
					num9 = num11;
				}
				else
				{
					if (num11 > num8)
					{
						num8 = num11;
					}
				}
				num10 = num3 * array4[0] + num4 * array4[2] + array4[4];
				num11 = num3 * array4[1] + num4 * array4[3] + array4[5];
				if (num10 < num7)
				{
					num7 = num10;
				}
				else
				{
					if (num10 > num6)
					{
						num6 = num10;
					}
				}
				if (num11 < num9)
				{
					num9 = num11;
				}
				else
				{
					if (num11 > num8)
					{
						num8 = num11;
					}
				}
				double num12 = Math.Abs(tPat.getXStep());
				double num13 = Math.Abs(tPat.getYStep());
				int num14 = (int)Math.Ceiling((num7 - tPat.getBBox()[2]) / num12);
				int num15 = (int)Math.Floor((num6 - tPat.getBBox()[0]) / num12) + 1;
				int num16 = (int)Math.Ceiling((num9 - tPat.getBBox()[3]) / num13);
				int num17 = (int)Math.Floor((num8 - tPat.getBBox()[1]) / num13) + 1;
				for (int i = 0; i < 4; i++)
				{
					array3[i] = array[i];
				}
				if (this.m_pRavenOut.useTilingPatternFill())
				{
					array3[4] = array[4];
					array3[5] = array[5];
					this.m_pRavenOut.tilingPatternFill(this.m_pState, this, tPat.getContentStream(), tPat.getPaintType(), tPat.getResDict(), array3, tPat.getBBox(), num14, num16, num15, num17, num12, num13);
				}
				else
				{
					for (int j = num16; j < num17; j++)
					{
						for (int k = num14; k < num15; k++)
						{
							double num18 = (double)k * num12;
							double num19 = (double)j * num13;
							array3[4] = num18 * array[0] + num19 * array[2] + array[4];
							array3[5] = num18 * array[1] + num19 * array[3] + array[5];
							this.drawForm(tPat.getContentStream(), tPat.getResDict(), array3, tPat.getBBox());
						}
					}
				}
			}
			this.restoreStateStack(oldState);
		}
		private void doShadingPatternFill(PdfShadingPattern sPat, bool stroke, bool eoFill, bool text)
		{
			double[] array = new double[6];
			double[] array2 = new double[6];
			double[] array3 = new double[6];
			double x = 0.0;
			double y = 0.0;
			double x2 = 0.0;
			double y2 = 0.0;
			PdfShading shading = sPat.getShading();
			PdfState oldState = this.saveStateStack();
			if (stroke)
			{
				this.m_pState.clipToStrokePath();
				this.m_pRavenOut.clipToStrokePath(this.m_pState);
			}
			else
			{
				if (!text)
				{
					this.m_pState.clip();
					if (eoFill)
					{
						this.m_pRavenOut.eoClip(this.m_pState);
					}
					else
					{
						this.m_pRavenOut.clip(this.m_pState);
					}
				}
			}
			this.m_pState.clearPath();
			double[] cTM = this.m_pState.getCTM();
			double[] array4 = this.baseMatrix;
			double[] matrix = sPat.getMatrix();
			double num = 1.0 / (cTM[0] * cTM[3] - cTM[1] * cTM[2]);
			array2[0] = cTM[3] * num;
			array2[1] = -cTM[1] * num;
			array2[2] = -cTM[2] * num;
			array2[3] = cTM[0] * num;
			array2[4] = (cTM[2] * cTM[5] - cTM[3] * cTM[4]) * num;
			array2[5] = (cTM[1] * cTM[4] - cTM[0] * cTM[5]) * num;
			array3[0] = matrix[0] * array4[0] + matrix[1] * array4[2];
			array3[1] = matrix[0] * array4[1] + matrix[1] * array4[3];
			array3[2] = matrix[2] * array4[0] + matrix[3] * array4[2];
			array3[3] = matrix[2] * array4[1] + matrix[3] * array4[3];
			array3[4] = matrix[4] * array4[0] + matrix[5] * array4[2] + array4[4];
			array3[5] = matrix[4] * array4[1] + matrix[5] * array4[3] + array4[5];
			array[0] = array3[0] * array2[0] + array3[1] * array2[2];
			array[1] = array3[0] * array2[1] + array3[1] * array2[3];
			array[2] = array3[2] * array2[0] + array3[3] * array2[2];
			array[3] = array3[2] * array2[1] + array3[3] * array2[3];
			array[4] = array3[4] * array2[0] + array3[5] * array2[2] + array2[4];
			array[5] = array3[4] * array2[1] + array3[5] * array2[3] + array2[5];
			this.m_pState.concatCTM(array[0], array[1], array[2], array[3], array[4], array[5]);
			this.m_pRavenOut.updateCTM(this.m_pState, array[0], array[1], array[2], array[3], array[4], array[5]);
			if (shading.getHasBBox())
			{
				shading.getBBox(ref x, ref y, ref x2, ref y2);
				this.m_pState.moveTo(x, y);
				this.m_pState.lineTo(x2, y);
				this.m_pState.lineTo(x2, y2);
				this.m_pState.lineTo(x, y2);
				this.m_pState.closePath();
				this.m_pState.clip();
				this.m_pRavenOut.clip(this.m_pState);
				this.m_pState.clearPath();
			}
			this.m_pState.setFillColorSpace(shading.getColorSpace().copy());
			if (shading.getHasBackground())
			{
				this.m_pState.setFillColor(shading.getBackground());
				this.m_pRavenOut.updateFillColor(this.m_pState);
				this.m_pState.getUserClipBBox(ref x, ref y, ref x2, ref y2);
				this.m_pState.moveTo(x, y);
				this.m_pState.lineTo(x2, y);
				this.m_pState.lineTo(x2, y2);
				this.m_pState.lineTo(x, y2);
				this.m_pState.closePath();
				this.m_pRavenOut.fill(this.m_pState);
				this.m_pState.clearPath();
			}
			switch (shading.getType())
			{
			case 1:
				this.doFunctionShFill((PdfFunctionShading)shading);
				break;
			case 2:
				this.doAxialShFill((PdfAxialShading)shading);
				break;
			case 3:
				this.doRadialShFill((PdfRadialShading)shading);
				break;
			case 4:
			case 5:
				this.LogError("Gouraud shading not yet implemented.");
				break;
			case 6:
			case 7:
				this.LogError("Mesh shanding not yet implemented.");
				break;
			}
			this.restoreStateStack(oldState);
		}
		private void doEndPath()
		{
			if (this.m_pState.isCurPt() && this.m_clip != ClipType.clipNone)
			{
				this.m_pState.clip();
				if (this.m_clip == ClipType.clipNormal)
				{
					this.m_pRavenOut.clip(this.m_pState);
				}
				else
				{
					this.m_pRavenOut.eoClip(this.m_pState);
				}
			}
			this.m_clip = ClipType.clipNone;
			this.m_pState.clearPath();
		}
		internal void doImage(PdfObject reff, PdfDictWithStream str, bool inlineImg)
		{
			PdfColorSpaceTop pdfColorSpaceTop = null;
			PdfImageColorMap pdfImageColorMap = null;
			int[] array = new int[64];
			StreamColorSpaceMode streamColorSpaceMode = StreamColorSpaceMode.streamCSNone;
			PdfObject pdfObject = str.GetObjectByName("Width");
			if (pdfObject == null)
			{
				pdfObject = str.GetObjectByName("W");
			}
			if (pdfObject == null || pdfObject.m_nType != enumType.pdfNumber)
			{
				this.LogError("Image width parameter invalid or missing.");
				return;
			}
			int width = (int)((PdfNumber)pdfObject).m_fValue;
			pdfObject = str.GetObjectByName("Height");
			if (pdfObject == null)
			{
				pdfObject = str.GetObjectByName("H");
			}
			if (pdfObject == null || pdfObject.m_nType != enumType.pdfNumber)
			{
				this.LogError("Image height parameter invalid or missing.");
				return;
			}
			int height = (int)((PdfNumber)pdfObject).m_fValue;
			pdfObject = str.GetObjectByName("ImageMask");
			if (pdfObject == null)
			{
				pdfObject = str.GetObjectByName("IM");
			}
			bool flag = false;
			if (pdfObject != null && pdfObject.m_nType == enumType.pdfBool)
			{
				flag = ((PdfBool)pdfObject).m_bValue;
			}
			pdfObject = str.GetObjectByName("BitsPerComponent");
			if (pdfObject == null)
			{
				pdfObject = str.GetObjectByName("BPC");
			}
			int num;
			if (pdfObject != null && pdfObject.m_nType == enumType.pdfNumber)
			{
				num = (int)((PdfNumber)pdfObject).m_fValue;
			}
			else
			{
				if (!flag)
				{
					this.LogError("Image BitsPerComponent parameter invalid or missing.");
					return;
				}
				num = 1;
			}
			if (flag)
			{
				if (num != 1)
				{
					this.LogError("Invalid BitsPerComponent parameter (should be 1 for a mask image.)");
					return;
				}
				bool invert = false;
				pdfObject = str.GetObjectByName("Decode");
				if (pdfObject == null)
				{
					pdfObject = str.GetObjectByName("D");
				}
				if (pdfObject != null && pdfObject.m_nType == enumType.pdfArray)
				{
					PdfObject pdfObject2 = ((PdfArray)pdfObject).GetObject(0);
					if (pdfObject2 != null && pdfObject2.m_nType == enumType.pdfNumber && ((PdfNumber)pdfObject2).m_fValue == 1.0)
					{
						invert = true;
					}
				}
				else
				{
					if (pdfObject != null)
					{
						this.LogError("Image Decode parameter is not an array.");
						return;
					}
				}
				if (this.m_pState.getFillColorSpace().getMode() == enumColorSpaceMode.csPattern)
				{
					this.doPatternImageMask(reff, str, width, height, invert, inlineImg);
					return;
				}
				this.m_pRavenOut.drawImageMask(this.m_pState, reff, str, width, height, invert, inlineImg);
				return;
			}
			else
			{
				pdfObject = str.GetObjectByName("ColorSpace");
				if (pdfObject == null)
				{
					pdfObject = str.GetObjectByName("CS");
				}
				if (pdfObject != null && pdfObject.m_nType == enumType.pdfName)
				{
					PdfObject pdfObject2 = this.m_pResources.LookupColorSpace(((PdfName)pdfObject).m_bstrName);
					if (pdfObject2 != null)
					{
						pdfObject = pdfObject2;
					}
				}
				if (pdfObject != null)
				{
					pdfColorSpaceTop = PdfColorSpaceTop.parse(pdfObject, this);
				}
				else
				{
					if (streamColorSpaceMode == StreamColorSpaceMode.streamCSDeviceGray)
					{
						pdfColorSpaceTop = new PdfDeviceGrayColorSpace();
					}
					else
					{
						if (streamColorSpaceMode == StreamColorSpaceMode.streamCSDeviceRGB)
						{
							pdfColorSpaceTop = new PdfDeviceRGBColorSpace();
						}
						else
						{
							if (streamColorSpaceMode == StreamColorSpaceMode.streamCSDeviceCMYK)
							{
								pdfColorSpaceTop = new PdfDeviceCMYKColorSpace(this);
							}
						}
					}
				}
				if (pdfColorSpaceTop == null)
				{
					this.LogError("Image ColorSpace parameter is invalid or missing.");
					return;
				}
				pdfObject = str.GetObjectByName("Decode");
				if (pdfObject == null)
				{
					pdfObject = str.GetObjectByName("D");
				}
				PdfImageColorMap pdfImageColorMap2 = new PdfImageColorMap(num, pdfObject, pdfColorSpaceTop);
				if (!pdfImageColorMap2.isOk())
				{
					this.LogError("Image color map is invalid.");
					return;
				}
				bool flag4;
				bool flag3;
				bool flag2 = flag3 = (flag4 = false);
				PdfDictWithStream maskStr = null;
				int maskWidth;
				int maskHeight = maskWidth = 0;
				bool maskInvert = false;
				PdfObject objectByName = str.GetObjectByName("Mask");
				PdfObject objectByName2 = str.GetObjectByName("SMask");
				if (objectByName2 != null && objectByName2.m_nType == enumType.pdfDictWithStream)
				{
					if (inlineImg)
					{
						this.LogError("SMask inline image error.");
						return;
					}
					maskStr = (PdfDictWithStream)objectByName2;
					PdfDict pdfDict = (PdfDict)objectByName2;
					pdfObject = pdfDict.GetObjectByName("Width");
					if (pdfObject == null)
					{
						pdfObject = pdfDict.GetObjectByName("W");
					}
					if (pdfObject == null || pdfObject.m_nType != enumType.pdfNumber)
					{
						this.LogError("SMask Width should be a number.");
						return;
					}
					maskWidth = (int)((PdfNumber)pdfObject).m_fValue;
					pdfObject = pdfDict.GetObjectByName("Height");
					if (pdfObject == null)
					{
						pdfObject = pdfDict.GetObjectByName("H");
					}
					if (pdfObject == null || pdfObject.m_nType != enumType.pdfNumber)
					{
						this.LogError("SMask Height should be a number.");
						return;
					}
					maskHeight = (int)((PdfNumber)pdfObject).m_fValue;
					pdfObject = pdfDict.GetObjectByName("BitsPerComponent");
					if (pdfObject == null)
					{
						pdfObject = pdfDict.GetObjectByName("BPC");
					}
					if (pdfObject == null || pdfObject.m_nType != enumType.pdfNumber)
					{
						this.LogError("SMask Height should be a number.");
						return;
					}
					int bitsA = (int)((PdfNumber)pdfObject).m_fValue;
					pdfObject = pdfDict.GetObjectByName("ColorSpace");
					if (pdfObject == null)
					{
						pdfObject = pdfDict.GetObjectByName("CS");
					}
					if (pdfObject != null && pdfObject.m_nType == enumType.pdfName)
					{
						PdfObject pdfObject2 = this.m_pResources.LookupColorSpace(((PdfName)pdfObject).m_bstrName);
						if (pdfObject2 != null)
						{
							pdfObject = pdfObject2;
						}
					}
					PdfColorSpaceTop pdfColorSpaceTop2 = PdfColorSpaceTop.parse(pdfObject, this);
					if (pdfColorSpaceTop2 == null || pdfColorSpaceTop2.getMode() != enumColorSpaceMode.csDeviceGray)
					{
						this.LogError("Invalid image ColorSpace parameter.");
						return;
					}
					pdfObject = pdfDict.GetObjectByName("Decode");
					if (pdfObject == null)
					{
						pdfObject = pdfDict.GetObjectByName("D");
					}
					pdfImageColorMap = new PdfImageColorMap(bitsA, pdfObject, pdfColorSpaceTop2);
					if (!pdfImageColorMap.isOk())
					{
						this.LogError("Invalid image color map.");
						return;
					}
					flag4 = true;
				}
				else
				{
					if (objectByName != null && objectByName.m_nType == enumType.pdfArray)
					{
						flag3 = true;
						int num2 = 0;
						while (num2 + 1 < ((PdfArray)objectByName).Size)
						{
							if (num2 + 1 >= 64)
							{
								break;
							}
							pdfObject = ((PdfArray)objectByName).GetObject(num2);
							if (pdfObject == null || pdfObject.m_nType != enumType.pdfNumber)
							{
								flag3 = false;
								break;
							}
							array[num2] = (int)((PdfNumber)pdfObject).m_fValue;
							if (array[num2] < 0 || array[num2] >= 1 << num)
							{
								flag3 = false;
								break;
							}
							pdfObject = ((PdfArray)objectByName).GetObject(num2 + 1);
							if (pdfObject == null || pdfObject.m_nType != enumType.pdfNumber)
							{
								flag3 = false;
								break;
							}
							array[num2 + 1] = (int)((PdfNumber)pdfObject).m_fValue;
							if (array[num2 + 1] < 0 || array[num2 + 1] >= 1 << num || array[num2] > array[num2 + 1])
							{
								flag3 = false;
								break;
							}
							num2 += 2;
						}
					}
					else
					{
						if (objectByName != null && objectByName.m_nType == enumType.pdfDictWithStream)
						{
							if (inlineImg)
							{
								this.LogError("Mask inline image error.");
								return;
							}
							maskStr = (PdfDictWithStream)objectByName;
							PdfDict pdfDict = (PdfDict)objectByName;
							pdfObject = pdfDict.GetObjectByName("Width");
							if (pdfObject == null)
							{
								pdfObject = pdfDict.GetObjectByName("W");
							}
							if (pdfObject == null || pdfObject.m_nType != enumType.pdfNumber)
							{
								this.LogError("Mask width should be a number.");
								return;
							}
							maskWidth = (int)((PdfNumber)pdfObject).m_fValue;
							pdfObject = pdfDict.GetObjectByName("Height");
							if (pdfObject == null)
							{
								pdfObject = pdfDict.GetObjectByName("H");
							}
							if (pdfObject == null || pdfObject.m_nType != enumType.pdfNumber)
							{
								this.LogError("Mask height should be a number.");
								return;
							}
							maskHeight = (int)((PdfNumber)pdfObject).m_fValue;
							pdfObject = pdfDict.GetObjectByName("ImageMask");
							if (pdfObject == null)
							{
								pdfObject = pdfDict.GetObjectByName("IM");
							}
							if (pdfObject == null || pdfObject.m_nType != enumType.pdfBool)
							{
								this.LogError("ImageMask parameter is invalid or missing.");
								return;
							}
							maskInvert = false;
							pdfObject = pdfDict.GetObjectByName("Decode");
							if (pdfObject == null)
							{
								pdfObject = pdfDict.GetObjectByName("D");
							}
							if (pdfObject != null && pdfObject.m_nType == enumType.pdfArray)
							{
								PdfObject pdfObject2 = ((PdfArray)pdfObject).GetObject(0);
								maskInvert = (pdfObject2 != null && pdfObject2.m_nType == enumType.pdfNumber && ((PdfNumber)pdfObject2).m_fValue == 1.0);
							}
							else
							{
								if (pdfObject != null)
								{
									this.LogError("Decode parameter is invalid or missing.");
									return;
								}
							}
							flag2 = true;
						}
					}
				}
				if (flag4)
				{
					this.m_pRavenOut.drawSoftMaskedImage(this.m_pState, reff, str, width, height, pdfImageColorMap2, maskStr, maskWidth, maskHeight, pdfImageColorMap);
					return;
				}
				if (flag2)
				{
					this.m_pRavenOut.drawMaskedImage(this.m_pState, reff, str, width, height, pdfImageColorMap2, maskStr, maskWidth, maskHeight, maskInvert);
					return;
				}
				this.m_pRavenOut.drawImage(this.m_pState, reff, str, width, height, pdfImageColorMap2, flag3 ? array : null, inlineImg);
				return;
			}
		}
		internal void drawForm(PdfObject str, PdfDict resDict, double[] matrix, double[] bbox)
		{
			this.drawForm(str, resDict, matrix, bbox, false, false, null, false, false, false, null, null);
		}
		private void drawForm(PdfObject str, PdfDict resDict, double[] matrix, double[] bbox, bool transpGroup, bool softMask, PdfColorSpaceTop blendingColorSpace, bool isolated, bool knockout, bool alpha, PdfFunctionTop transferFunc, PdfColor backdropColor)
		{
			double[] array = new double[6];
			this.pushResources(resDict);
			this.saveState();
			this.m_pState.clearPath();
			PdfInput pInput = this.m_pInput;
			this.m_pState.concatCTM(matrix[0], matrix[1], matrix[2], matrix[3], matrix[4], matrix[5]);
			this.m_pRavenOut.updateCTM(this.m_pState, matrix[0], matrix[1], matrix[2], matrix[3], matrix[4], matrix[5]);
			this.m_pState.moveTo(bbox[0], bbox[1]);
			this.m_pState.lineTo(bbox[2], bbox[1]);
			this.m_pState.lineTo(bbox[2], bbox[3]);
			this.m_pState.lineTo(bbox[0], bbox[3]);
			this.m_pState.closePath();
			this.m_pState.clip();
			this.m_pRavenOut.clip(this.m_pState);
			this.m_pState.clearPath();
			if (softMask || transpGroup)
			{
				if (this.m_pState.getBlendMode() != enumBlendMode.bmBlendNormal)
				{
					this.m_pState.setBlendMode(enumBlendMode.bmBlendNormal);
					this.m_pRavenOut.updateBlendMode(this.m_pState);
				}
				if (this.m_pState.getFillOpacity() != 1.0)
				{
					this.m_pState.setFillOpacity(1.0);
					this.m_pRavenOut.updateFillOpacity(this.m_pState);
				}
				if (this.m_pState.getStrokeOpacity() != 1.0)
				{
					this.m_pState.setStrokeOpacity(1.0);
					this.m_pRavenOut.updateStrokeOpacity(this.m_pState);
				}
				this.m_pRavenOut.clearSoftMask(this.m_pState);
				this.m_pRavenOut.beginTransparencyGroup(this.m_pState, bbox, blendingColorSpace, isolated, knockout, softMask);
			}
			for (int i = 0; i < 6; i++)
			{
				array[i] = this.baseMatrix[i];
				this.baseMatrix[i] = this.m_pState.getCTM()[i];
			}
			this.Display(str);
			if (softMask || transpGroup)
			{
				this.m_pRavenOut.endTransparencyGroup(this.m_pState);
			}
			for (int i = 0; i < 6; i++)
			{
				this.baseMatrix[i] = array[i];
			}
			this.m_pInput = pInput;
			this.restoreState();
			this.popResources();
			if (softMask)
			{
				this.m_pRavenOut.setSoftMask(this.m_pState, bbox, alpha, transferFunc, backdropColor);
				return;
			}
			if (transpGroup)
			{
				this.m_pRavenOut.paintTransparencyGroup(this.m_pState, bbox);
			}
		}
		private void doSoftMask(PdfObject str, bool alpha, PdfColorSpaceTop blendingColorSpace, bool isolated, bool knockout, PdfFunctionTop transferFunc, PdfColor backdropColor)
		{
			double[] array = new double[6];
			double[] array2 = new double[4];
			if (this.m_nFormDepth > 20)
			{
				return;
			}
			PdfDict pdfDict = (PdfDict)str;
			PdfObject objectByName = pdfDict.GetObjectByName("FormType");
			if (objectByName != null && (objectByName.m_nType != enumType.pdfNumber || ((PdfNumber)objectByName).m_fValue != 1.0))
			{
				this.LogError("Invalid or missing FormType value.");
			}
			objectByName = pdfDict.GetObjectByName("BBox");
			if (objectByName == null || objectByName.m_nType != enumType.pdfArray)
			{
				this.LogError("Form BBox is missing or invalid.");
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				PdfObject @object = ((PdfArray)objectByName).GetObject(i);
				if (@object != null && @object.m_nType == enumType.pdfNumber)
				{
					array2[i] = ((PdfNumber)@object).m_fValue;
				}
			}
			objectByName = pdfDict.GetObjectByName("Matrix");
			if (objectByName != null && objectByName.m_nType == enumType.pdfArray)
			{
				for (int i = 0; i < 6; i++)
				{
					PdfObject @object = ((PdfArray)objectByName).GetObject(i);
					if (@object != null && @object.m_nType == enumType.pdfNumber)
					{
						array[i] = ((PdfNumber)@object).m_fValue;
					}
				}
			}
			else
			{
				array[0] = 1.0;
				array[1] = 0.0;
				array[2] = 0.0;
				array[3] = 1.0;
				array[4] = 0.0;
				array[5] = 0.0;
			}
			objectByName = pdfDict.GetObjectByName("Resources");
			PdfDict resDict = (objectByName != null && objectByName.m_nType == enumType.pdfDictionary) ? ((PdfDict)objectByName) : null;
			this.m_nFormDepth++;
			this.drawForm(str, resDict, array, array2, true, true, blendingColorSpace, isolated, knockout, alpha, transferFunc, backdropColor);
			this.m_nFormDepth--;
		}
		private void doPatternImageMask(PdfObject rerf, PdfDictWithStream str, int width, int height, bool invert, bool inlineImg)
		{
		}
		private void doForm(PdfObject str)
		{
			double[] array = new double[6];
			double[] array2 = new double[6];
			if (this.m_nFormDepth > 20)
			{
				return;
			}
			PdfDict pdfDict = (PdfDict)str;
			PdfObject pdfObject = pdfDict.GetObjectByName("FormType");
			if (pdfObject != null && pdfObject.m_nType == enumType.pdfNumber)
			{
				double arg_52_0 = ((PdfNumber)pdfObject).m_fValue;
			}
			PdfObject objectByName = pdfDict.GetObjectByName("BBox");
			if (objectByName == null || objectByName.m_nType != enumType.pdfArray)
			{
				this.LogError("Invalid form BBox.");
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				pdfObject = ((PdfArray)objectByName).GetObject(i);
				if (pdfObject.m_nType == enumType.pdfNumber)
				{
					array2[i] = ((PdfNumber)pdfObject).m_fValue;
				}
			}
			PdfObject objectByName2 = pdfDict.GetObjectByName("Matrix");
			if (objectByName2 != null && objectByName2.m_nType == enumType.pdfArray)
			{
				for (int i = 0; i < 6; i++)
				{
					pdfObject = ((PdfArray)objectByName2).GetObject(i);
					if (pdfObject.m_nType == enumType.pdfNumber)
					{
						array[i] = ((PdfNumber)pdfObject).m_fValue;
					}
				}
			}
			else
			{
				array[0] = 1.0;
				array[1] = 0.0;
				array[2] = 0.0;
				array[3] = 1.0;
				array[4] = 0.0;
				array[5] = 0.0;
			}
			PdfObject objectByName3 = pdfDict.GetObjectByName("Resources");
			PdfDict resDict = (objectByName3 != null && objectByName3.m_nType == enumType.pdfDictionary) ? ((PdfDict)objectByName3) : null;
			bool knockout;
			bool transpGroup;
			bool isolated = transpGroup = (knockout = false);
			PdfColorSpaceTop blendingColorSpace = null;
			pdfObject = pdfDict.GetObjectByName("Group");
			if (pdfObject != null && pdfObject.m_nType == enumType.pdfDictionary)
			{
				PdfObject objectByName4 = ((PdfDict)pdfObject).GetObjectByName("S");
				if (objectByName4 != null && objectByName4.m_nType == enumType.pdfName && ((PdfName)objectByName4).m_bstrName == "Transparency")
				{
					transpGroup = true;
					PdfObject objectByName5 = ((PdfDict)pdfObject).GetObjectByName("CS");
					if (objectByName5 != null)
					{
						blendingColorSpace = PdfColorSpaceTop.parse(objectByName5, this);
					}
					objectByName5 = ((PdfDict)pdfObject).GetObjectByName("I");
					if (objectByName5 != null && objectByName5.m_nType == enumType.pdfBool)
					{
						isolated = ((PdfBool)objectByName5).m_bValue;
					}
					objectByName5 = ((PdfDict)pdfObject).GetObjectByName("K");
					if (objectByName5 != null && objectByName5.m_nType == enumType.pdfBool)
					{
						knockout = ((PdfBool)objectByName5).m_bValue;
					}
				}
			}
			this.m_nFormDepth++;
			this.drawForm(str, resDict, array, array2, transpGroup, false, blendingColorSpace, isolated, knockout, false, null, null);
			this.m_nFormDepth--;
		}
		private void Display(PdfObject obj)
		{
			if (obj.m_nType == enumType.pdfArray)
			{
				this.LogError("Display: array for XObject Form not supported.");
			}
			else
			{
				if (obj.m_nType != enumType.pdfDictWithStream)
				{
					this.LogError("Display: invalid content.");
					return;
				}
			}
			this.RenderImage(((PdfDictWithStream)obj).m_objStream);
		}
		private void pushResources(PdfDict resDict)
		{
			PdfResources pResources = this.m_pResources;
			this.m_pResources = new PdfResources(resDict, null, this.m_pDoc, this.m_pPage, this);
			this.m_pResources.m_pNext = pResources;
		}
		private void popResources()
		{
			PdfResources pNext = this.m_pResources.m_pNext;
			this.m_pResources = pNext;
		}
		private void DrawAnnotations()
		{
			foreach (PdfAnnot current in this.m_pPage.m_arrAnnots)
			{
				if (current.m_ptrApperanceDict != null)
				{
					this.m_pDoc.m_pInput.RemoveAllRefsFromObject(current.m_ptrApperanceDict, 0);
					PdfObject pdfObject = null;
					PdfObject objectByName = current.m_pAnnotObj.m_objAttributes.GetObjectByName("AS");
					if (objectByName != null && objectByName.m_nType == enumType.pdfName)
					{
						PdfObject objectByName2 = current.m_ptrApperanceDict.GetObjectByName("N");
						if (objectByName2 != null && objectByName2.m_nType == enumType.pdfDictionary)
						{
							pdfObject = ((PdfDict)objectByName2).GetObjectByName(((PdfName)objectByName).m_bstrName);
						}
					}
					else
					{
						pdfObject = current.m_ptrApperanceDict.GetObjectByName("N");
					}
					if (pdfObject != null && pdfObject.m_nType == enumType.pdfDictWithStream)
					{
						this.doAnnot(pdfObject, (double)current.m_fX1, (double)current.m_fY1, (double)current.m_fX2, (double)current.m_fY2);
					}
				}
			}
		}
		private void doAnnot(PdfObject str, double xMin, double yMin, double xMax, double yMax)
		{
			double[] array = new double[6];
			double[] array2 = new double[6];
			double[] array3 = new double[6];
			PdfDict pdfDict = (PdfDict)str;
			PdfObject objectByName = pdfDict.GetObjectByName("BBox");
			if (objectByName == null || objectByName.m_nType != enumType.pdfArray)
			{
				this.LogError("Annot: invalid form BBox.");
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				PdfObject @object = ((PdfArray)objectByName).GetObject(i);
				if (@object != null && @object.m_nType == enumType.pdfNumber)
				{
					array2[i] = ((PdfNumber)@object).m_fValue;
				}
			}
			PdfObject objectByName2 = pdfDict.GetObjectByName("Matrix");
			if (objectByName2 != null && objectByName2.m_nType == enumType.pdfArray)
			{
				for (int i = 0; i < 6; i++)
				{
					PdfObject @object = ((PdfArray)objectByName2).GetObject(i);
					if (@object != null && @object.m_nType == enumType.pdfNumber)
					{
						array[i] = ((PdfNumber)@object).m_fValue;
					}
				}
			}
			else
			{
				array[0] = 1.0;
				array[1] = 0.0;
				array[2] = 0.0;
				array[3] = 1.0;
				array[4] = 0.0;
				array[5] = 0.0;
			}
			double num = array2[0] * array[0] + array2[1] * array[2] + array[4];
			double num2 = array2[0] * array[1] + array2[1] * array[3] + array[5];
			double num3 = array2[2] * array[0] + array2[3] * array[2] + array[4];
			double num4 = array2[2] * array[1] + array2[3] * array[3] + array[5];
			double[] cTM = this.m_pState.getCTM();
			double num5 = 1.0 / (cTM[0] * cTM[3] - cTM[1] * cTM[2]);
			array3[0] = cTM[3] * num5;
			array3[1] = -cTM[1] * num5;
			array3[2] = -cTM[2] * num5;
			array3[3] = cTM[0] * num5;
			array3[4] = (cTM[2] * cTM[5] - cTM[3] * cTM[4]) * num5;
			array3[5] = (cTM[1] * cTM[4] - cTM[0] * cTM[5]) * num5;
			double num6 = this.baseMatrix[0] * xMin + this.baseMatrix[2] * yMin + this.baseMatrix[4];
			double num7 = this.baseMatrix[1] * xMin + this.baseMatrix[3] * yMin + this.baseMatrix[5];
			double num8 = array3[0] * num6 + array3[2] * num7 + array3[4];
			double num9 = array3[1] * num6 + array3[3] * num7 + array3[5];
			num6 = this.baseMatrix[0] * xMax + this.baseMatrix[2] * yMax + this.baseMatrix[4];
			num7 = this.baseMatrix[1] * xMax + this.baseMatrix[3] * yMax + this.baseMatrix[5];
			double num10 = array3[0] * num6 + array3[2] * num7 + array3[4];
			double num11 = array3[1] * num6 + array3[3] * num7 + array3[5];
			if (num > num3)
			{
				num6 = num;
				num = num3;
				num3 = num6;
			}
			if (num2 > num4)
			{
				num7 = num2;
				num2 = num4;
				num4 = num7;
			}
			if (num8 > num10)
			{
				num6 = num8;
				num8 = num10;
				num10 = num6;
			}
			if (num9 > num11)
			{
				num7 = num9;
				num9 = num11;
				num11 = num7;
			}
			double num12;
			if (num3 == num)
			{
				num12 = 1.0;
			}
			else
			{
				num12 = (num10 - num8) / (num3 - num);
			}
			double num13;
			if (num4 == num2)
			{
				num13 = 1.0;
			}
			else
			{
				num13 = (num11 - num9) / (num4 - num2);
			}
			array[0] *= num12;
			array[2] *= num12;
			array[4] = (array[4] - num) * num12 + num8;
			array[1] *= num13;
			array[3] *= num13;
			array[5] = (array[5] - num2) * num13 + num9;
			PdfObject objectByName3 = pdfDict.GetObjectByName("Resources");
			PdfDict resDict = (objectByName3 != null && objectByName3.m_nType == enumType.pdfDictionary) ? ((PdfDict)objectByName3) : null;
			this.drawForm(str, resDict, array, array2);
		}
		private PdfState saveStateStack()
		{
			this.m_pRavenOut.saveState(this.m_pState);
			PdfState pState = this.m_pState;
			this.m_pState = this.m_pState.copy(true);
			return pState;
		}
		private void restoreStateStack(PdfState oldState)
		{
			while (this.m_pState.hasSaves())
			{
				this.restoreState();
			}
			this.m_pState = oldState;
			this.m_pRavenOut.restoreState(this.m_pState);
		}
		private void InitializeOperatorArray()
		{
			PdfPreview.PreviewOperator[] array = new PdfPreview.PreviewOperator[73];
			array[0] = new PdfPreview.PreviewOperator("\"", 3, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfString
			}, new PdfPreview.PreviewOperatorFunction(this.opMoveSetShowText));
			array[1] = new PdfPreview.PreviewOperator("'", 1, new enumType[]
			{
				enumType.pdfString
			}, new PdfPreview.PreviewOperatorFunction(this.opMoveShowText));
			PdfPreview.PreviewOperator[] arg_7F_0 = array;
			int arg_7F_1 = 2;
			string arg_7A_0 = "B";
			int arg_7A_1 = 0;
			enumType[] arr = new enumType[1];
			arg_7F_0[arg_7F_1] = new PdfPreview.PreviewOperator(arg_7A_0, arg_7A_1, arr, new PdfPreview.PreviewOperatorFunction(this.opFillStroke));
			PdfPreview.PreviewOperator[] arg_A3_0 = array;
			int arg_A3_1 = 3;
			string arg_9E_0 = "B*";
			int arg_9E_1 = 0;
			enumType[] arr2 = new enumType[1];
			arg_A3_0[arg_A3_1] = new PdfPreview.PreviewOperator(arg_9E_0, arg_9E_1, arr2, new PdfPreview.PreviewOperatorFunction(this.opEOFillStroke));
			array[4] = new PdfPreview.PreviewOperator("BDC", 2, new enumType[]
			{
				enumType.pdfName,
				enumType.pdfProps
			}, new PdfPreview.PreviewOperatorFunction(this.opBeginMarkedContent));
			PdfPreview.PreviewOperator[] arg_F6_0 = array;
			int arg_F6_1 = 5;
			string arg_F1_0 = "BI";
			int arg_F1_1 = 0;
			enumType[] arr3 = new enumType[1];
			arg_F6_0[arg_F6_1] = new PdfPreview.PreviewOperator(arg_F1_0, arg_F1_1, arr3, new PdfPreview.PreviewOperatorFunction(this.opBeginImage));
			array[6] = new PdfPreview.PreviewOperator("BMC", 1, new enumType[]
			{
				enumType.pdfName
			}, new PdfPreview.PreviewOperatorFunction(this.opBeginMarkedContent));
			PdfPreview.PreviewOperator[] arg_143_0 = array;
			int arg_143_1 = 7;
			string arg_13E_0 = "BT";
			int arg_13E_1 = 0;
			enumType[] arr4 = new enumType[1];
			arg_143_0[arg_143_1] = new PdfPreview.PreviewOperator(arg_13E_0, arg_13E_1, arr4, new PdfPreview.PreviewOperatorFunction(this.opBeginText));
			PdfPreview.PreviewOperator[] arg_167_0 = array;
			int arg_167_1 = 8;
			string arg_162_0 = "BX";
			int arg_162_1 = 0;
			enumType[] arr5 = new enumType[1];
			arg_167_0[arg_167_1] = new PdfPreview.PreviewOperator(arg_162_0, arg_162_1, arr5, new PdfPreview.PreviewOperatorFunction(this.opBeginIgnoreUndef));
			array[9] = new PdfPreview.PreviewOperator("CS", 1, new enumType[]
			{
				enumType.pdfName
			}, new PdfPreview.PreviewOperatorFunction(this.opSetStrokeColorSpace));
			array[10] = new PdfPreview.PreviewOperator("DP", 2, new enumType[]
			{
				enumType.pdfName,
				enumType.pdfProps
			}, new PdfPreview.PreviewOperatorFunction(this.opMarkPoint));
			array[11] = new PdfPreview.PreviewOperator("Do", 1, new enumType[]
			{
				enumType.pdfName
			}, new PdfPreview.PreviewOperatorFunction(this.opXObject));
			PdfPreview.PreviewOperator[] arg_210_0 = array;
			int arg_210_1 = 12;
			string arg_20B_0 = "EI";
			int arg_20B_1 = 0;
			enumType[] arr6 = new enumType[1];
			arg_210_0[arg_210_1] = new PdfPreview.PreviewOperator(arg_20B_0, arg_20B_1, arr6, new PdfPreview.PreviewOperatorFunction(this.opEndImage));
			PdfPreview.PreviewOperator[] arg_235_0 = array;
			int arg_235_1 = 13;
			string arg_230_0 = "EMC";
			int arg_230_1 = 0;
			enumType[] arr7 = new enumType[1];
			arg_235_0[arg_235_1] = new PdfPreview.PreviewOperator(arg_230_0, arg_230_1, arr7, new PdfPreview.PreviewOperatorFunction(this.opEndMarkedContent));
			PdfPreview.PreviewOperator[] arg_25A_0 = array;
			int arg_25A_1 = 14;
			string arg_255_0 = "ET";
			int arg_255_1 = 0;
			enumType[] arr8 = new enumType[1];
			arg_25A_0[arg_25A_1] = new PdfPreview.PreviewOperator(arg_255_0, arg_255_1, arr8, new PdfPreview.PreviewOperatorFunction(this.opEndText));
			PdfPreview.PreviewOperator[] arg_27F_0 = array;
			int arg_27F_1 = 15;
			string arg_27A_0 = "EX";
			int arg_27A_1 = 0;
			enumType[] arr9 = new enumType[1];
			arg_27F_0[arg_27F_1] = new PdfPreview.PreviewOperator(arg_27A_0, arg_27A_1, arr9, new PdfPreview.PreviewOperatorFunction(this.opEndIgnoreUndef));
			PdfPreview.PreviewOperator[] arg_2A4_0 = array;
			int arg_2A4_1 = 16;
			string arg_29F_0 = "F";
			int arg_29F_1 = 0;
			enumType[] arr10 = new enumType[1];
			arg_2A4_0[arg_2A4_1] = new PdfPreview.PreviewOperator(arg_29F_0, arg_29F_1, arr10, new PdfPreview.PreviewOperatorFunction(this.opFill));
			array[17] = new PdfPreview.PreviewOperator("G", 1, new enumType[]
			{
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetStrokeGray));
			PdfPreview.PreviewOperator[] arg_2F3_0 = array;
			int arg_2F3_1 = 18;
			string arg_2EE_0 = "ID";
			int arg_2EE_1 = 0;
			enumType[] arr11 = new enumType[1];
			arg_2F3_0[arg_2F3_1] = new PdfPreview.PreviewOperator(arg_2EE_0, arg_2EE_1, arr11, new PdfPreview.PreviewOperatorFunction(this.opImageData));
			array[19] = new PdfPreview.PreviewOperator("J", 1, new enumType[]
			{
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetLineCap));
			array[20] = new PdfPreview.PreviewOperator("K", 4, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetStrokeCMYKColor));
			array[21] = new PdfPreview.PreviewOperator("M", 1, new enumType[]
			{
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetMiterLimit));
			array[22] = new PdfPreview.PreviewOperator("MP", 1, new enumType[]
			{
				enumType.pdfName
			}, new PdfPreview.PreviewOperatorFunction(this.opMarkPoint));
			PdfPreview.PreviewOperator[] arg_3CF_0 = array;
			int arg_3CF_1 = 23;
			string arg_3CA_0 = "Q";
			int arg_3CA_1 = 0;
			enumType[] arr12 = new enumType[1];
			arg_3CF_0[arg_3CF_1] = new PdfPreview.PreviewOperator(arg_3CA_0, arg_3CA_1, arr12, new PdfPreview.PreviewOperatorFunction(this.opRestore));
			array[24] = new PdfPreview.PreviewOperator("RG", 3, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetStrokeRGBColor));
			PdfPreview.PreviewOperator[] arg_428_0 = array;
			int arg_428_1 = 25;
			string arg_423_0 = "S";
			int arg_423_1 = 0;
			enumType[] arr13 = new enumType[1];
			arg_428_0[arg_428_1] = new PdfPreview.PreviewOperator(arg_423_0, arg_423_1, arr13, new PdfPreview.PreviewOperatorFunction(this.opStroke));
			array[26] = new PdfPreview.PreviewOperator("SC", -4, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetStrokeColor));
			array[27] = new PdfPreview.PreviewOperator("SCN", -5, new enumType[]
			{
				enumType.pdfSCN,
				enumType.pdfSCN,
				enumType.pdfSCN,
				enumType.pdfSCN,
				enumType.pdfSCN
			}, new PdfPreview.PreviewOperatorFunction(this.opSetStrokeColorN));
			PdfPreview.PreviewOperator[] arg_4CB_0 = array;
			int arg_4CB_1 = 28;
			string arg_4C6_0 = "T*";
			int arg_4C6_1 = 0;
			enumType[] arr14 = new enumType[1];
			arg_4CB_0[arg_4CB_1] = new PdfPreview.PreviewOperator(arg_4C6_0, arg_4C6_1, arr14, new PdfPreview.PreviewOperatorFunction(this.opTextNextLine));
			array[29] = new PdfPreview.PreviewOperator("TD", 2, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opTextMoveSet));
			array[30] = new PdfPreview.PreviewOperator("TJ", 1, new enumType[]
			{
				enumType.pdfArray
			}, new PdfPreview.PreviewOperatorFunction(this.opShowSpaceText));
			array[31] = new PdfPreview.PreviewOperator("TL", 1, new enumType[]
			{
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetTextLeading));
			array[32] = new PdfPreview.PreviewOperator("Tc", 1, new enumType[]
			{
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetCharSpacing));
			array[33] = new PdfPreview.PreviewOperator("Td", 2, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opTextMove));
			array[34] = new PdfPreview.PreviewOperator("Tf", 2, new enumType[]
			{
				enumType.pdfName,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetFont));
			array[35] = new PdfPreview.PreviewOperator("Tj", 1, new enumType[]
			{
				enumType.pdfString
			}, new PdfPreview.PreviewOperatorFunction(this.opShowText));
			array[36] = new PdfPreview.PreviewOperator("Tm", 6, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetTextMatrix));
			array[37] = new PdfPreview.PreviewOperator("Tr", 1, new enumType[]
			{
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetTextRender));
			array[38] = new PdfPreview.PreviewOperator("Ts", 1, new enumType[]
			{
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetTextRise));
			array[39] = new PdfPreview.PreviewOperator("Tw", 1, new enumType[]
			{
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetWordSpacing));
			array[40] = new PdfPreview.PreviewOperator("Tz", 1, new enumType[]
			{
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetHorizScaling));
			PdfPreview.PreviewOperator[] arg_711_0 = array;
			int arg_711_1 = 41;
			string arg_70C_0 = "W";
			int arg_70C_1 = 0;
			enumType[] arr15 = new enumType[1];
			arg_711_0[arg_711_1] = new PdfPreview.PreviewOperator(arg_70C_0, arg_70C_1, arr15, new PdfPreview.PreviewOperatorFunction(this.opClip));
			PdfPreview.PreviewOperator[] arg_736_0 = array;
			int arg_736_1 = 42;
			string arg_731_0 = "W*";
			int arg_731_1 = 0;
			enumType[] arr16 = new enumType[1];
			arg_736_0[arg_736_1] = new PdfPreview.PreviewOperator(arg_731_0, arg_731_1, arr16, new PdfPreview.PreviewOperatorFunction(this.opEOClip));
			PdfPreview.PreviewOperator[] arg_75B_0 = array;
			int arg_75B_1 = 43;
			string arg_756_0 = "b";
			int arg_756_1 = 0;
			enumType[] arr17 = new enumType[1];
			arg_75B_0[arg_75B_1] = new PdfPreview.PreviewOperator(arg_756_0, arg_756_1, arr17, new PdfPreview.PreviewOperatorFunction(this.opCloseFillStroke));
			PdfPreview.PreviewOperator[] arg_780_0 = array;
			int arg_780_1 = 44;
			string arg_77B_0 = "b*";
			int arg_77B_1 = 0;
			enumType[] arr18 = new enumType[1];
			arg_780_0[arg_780_1] = new PdfPreview.PreviewOperator(arg_77B_0, arg_77B_1, arr18, new PdfPreview.PreviewOperatorFunction(this.opCloseEOFillStroke));
			array[45] = new PdfPreview.PreviewOperator("c", 6, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opCurveTo));
			array[46] = new PdfPreview.PreviewOperator("cm", 6, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opConcat));
			array[47] = new PdfPreview.PreviewOperator("cs", 1, new enumType[]
			{
				enumType.pdfName
			}, new PdfPreview.PreviewOperatorFunction(this.opSetFillColorSpace));
			array[48] = new PdfPreview.PreviewOperator("d", 2, new enumType[]
			{
				enumType.pdfArray,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetDash));
			array[49] = new PdfPreview.PreviewOperator("d0", 2, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetCharWidth));
			array[50] = new PdfPreview.PreviewOperator("d1", 6, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetCacheDevice));
			PdfPreview.PreviewOperator[] arg_8F6_0 = array;
			int arg_8F6_1 = 51;
			string arg_8F1_0 = "f";
			int arg_8F1_1 = 0;
			enumType[] arr19 = new enumType[1];
			arg_8F6_0[arg_8F6_1] = new PdfPreview.PreviewOperator(arg_8F1_0, arg_8F1_1, arr19, new PdfPreview.PreviewOperatorFunction(this.opFill));
			PdfPreview.PreviewOperator[] arg_91B_0 = array;
			int arg_91B_1 = 52;
			string arg_916_0 = "f*";
			int arg_916_1 = 0;
			enumType[] arr20 = new enumType[1];
			arg_91B_0[arg_91B_1] = new PdfPreview.PreviewOperator(arg_916_0, arg_916_1, arr20, new PdfPreview.PreviewOperatorFunction(this.opEOFill));
			array[53] = new PdfPreview.PreviewOperator("g", 1, new enumType[]
			{
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetFillGray));
			array[54] = new PdfPreview.PreviewOperator("gs", 1, new enumType[]
			{
				enumType.pdfName
			}, new PdfPreview.PreviewOperatorFunction(this.opSetExtGState));
			PdfPreview.PreviewOperator[] arg_994_0 = array;
			int arg_994_1 = 55;
			string arg_98F_0 = "h";
			int arg_98F_1 = 0;
			enumType[] arr21 = new enumType[1];
			arg_994_0[arg_994_1] = new PdfPreview.PreviewOperator(arg_98F_0, arg_98F_1, arr21, new PdfPreview.PreviewOperatorFunction(this.opClosePath));
			array[56] = new PdfPreview.PreviewOperator("i", 1, new enumType[]
			{
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetFlat));
			array[57] = new PdfPreview.PreviewOperator("j", 1, new enumType[]
			{
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetLineJoin));
			array[58] = new PdfPreview.PreviewOperator("k", 4, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetFillCMYKColor));
			array[59] = new PdfPreview.PreviewOperator("l", 2, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opLineTo));
			array[60] = new PdfPreview.PreviewOperator("m", 2, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opMoveTo));
			PdfPreview.PreviewOperator[] arg_A9E_0 = array;
			int arg_A9E_1 = 61;
			string arg_A99_0 = "n";
			int arg_A99_1 = 0;
			enumType[] arr22 = new enumType[1];
			arg_A9E_0[arg_A9E_1] = new PdfPreview.PreviewOperator(arg_A99_0, arg_A99_1, arr22, new PdfPreview.PreviewOperatorFunction(this.opEndPath));
			PdfPreview.PreviewOperator[] arg_AC1_0 = array;
			int arg_AC1_1 = 62;
			string arg_ABC_0 = "q";
			int arg_ABC_1 = 0;
			arr22 = new enumType[1];
			arg_AC1_0[arg_AC1_1] = new PdfPreview.PreviewOperator(arg_ABC_0, arg_ABC_1, arr22, new PdfPreview.PreviewOperatorFunction(this.opSave));
			array[63] = new PdfPreview.PreviewOperator("re", 4, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opRectangle));
			array[64] = new PdfPreview.PreviewOperator("rg", 3, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetFillRGBColor));
			array[65] = new PdfPreview.PreviewOperator("ri", 1, new enumType[]
			{
				enumType.pdfName
			}, new PdfPreview.PreviewOperatorFunction(this.opSetRenderingIntent));
			PdfPreview.PreviewOperator[] arg_B6D_0 = array;
			int arg_B6D_1 = 66;
			string arg_B68_0 = "s";
			int arg_B68_1 = 0;
			arr22 = new enumType[1];
			arg_B6D_0[arg_B6D_1] = new PdfPreview.PreviewOperator(arg_B68_0, arg_B68_1, arr22, new PdfPreview.PreviewOperatorFunction(this.opCloseStroke));
			array[67] = new PdfPreview.PreviewOperator("sc", -4, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetFillColor));
			array[68] = new PdfPreview.PreviewOperator("scn", -5, new enumType[]
			{
				enumType.pdfSCN,
				enumType.pdfSCN,
				enumType.pdfSCN,
				enumType.pdfSCN,
				enumType.pdfSCN
			}, new PdfPreview.PreviewOperatorFunction(this.opSetFillColorN));
			array[69] = new PdfPreview.PreviewOperator("sh", 1, new enumType[]
			{
				enumType.pdfName
			}, new PdfPreview.PreviewOperatorFunction(this.opShFill));
			array[70] = new PdfPreview.PreviewOperator("v", 4, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opCurveTo1));
			array[71] = new PdfPreview.PreviewOperator("w", 1, new enumType[]
			{
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opSetLineWidth));
			array[72] = new PdfPreview.PreviewOperator("y", 4, new enumType[]
			{
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber,
				enumType.pdfNumber
			}, new PdfPreview.PreviewOperatorFunction(this.opCurveTo2));
			PdfPreview.m_arrOperators = array;
		}
		static PdfPreview()
		{
			// Note: this type is marked as 'beforefieldinit'.
			string[] array = new string[256];
			array[32] = "space";
			array[33] = "exclam";
			array[34] = "quotedbl";
			array[35] = "numbersign";
			array[36] = "dollar";
			array[37] = "percent";
			array[38] = "ampersand";
			array[39] = "quoteright";
			array[40] = "parenleft";
			array[41] = "parenright";
			array[42] = "asterisk";
			array[43] = "plus";
			array[44] = "comma";
			array[45] = "hyphen";
			array[46] = "period";
			array[47] = "slash";
			array[48] = "zero";
			array[49] = "one";
			array[50] = "two";
			array[51] = "three";
			array[52] = "four";
			array[53] = "five";
			array[54] = "six";
			array[55] = "seven";
			array[56] = "eight";
			array[57] = "nine";
			array[58] = "colon";
			array[59] = "semicolon";
			array[60] = "less";
			array[61] = "equal";
			array[62] = "greater";
			array[63] = "question";
			array[64] = "at";
			array[65] = "A";
			array[66] = "B";
			array[67] = "C";
			array[68] = "D";
			array[69] = "E";
			array[70] = "F";
			array[71] = "G";
			array[72] = "H";
			array[73] = "I";
			array[74] = "J";
			array[75] = "K";
			array[76] = "L";
			array[77] = "M";
			array[78] = "N";
			array[79] = "O";
			array[80] = "P";
			array[81] = "Q";
			array[82] = "R";
			array[83] = "S";
			array[84] = "T";
			array[85] = "U";
			array[86] = "V";
			array[87] = "W";
			array[88] = "X";
			array[89] = "Y";
			array[90] = "Z";
			array[91] = "bracketleft";
			array[92] = "backslash";
			array[93] = "bracketright";
			array[94] = "asciicircum";
			array[95] = "underscore";
			array[96] = "quoteleft";
			array[97] = "a";
			array[98] = "b";
			array[99] = "c";
			array[100] = "d";
			array[101] = "e";
			array[102] = "f";
			array[103] = "g";
			array[104] = "h";
			array[105] = "i";
			array[106] = "j";
			array[107] = "k";
			array[108] = "l";
			array[109] = "m";
			array[110] = "n";
			array[111] = "o";
			array[112] = "p";
			array[113] = "q";
			array[114] = "r";
			array[115] = "s";
			array[116] = "t";
			array[117] = "u";
			array[118] = "v";
			array[119] = "w";
			array[120] = "x";
			array[121] = "y";
			array[122] = "z";
			array[123] = "braceleft";
			array[124] = "bar";
			array[125] = "braceright";
			array[126] = "asciitilde";
			array[161] = "exclamdown";
			array[162] = "cent";
			array[163] = "sterling";
			array[164] = "fraction";
			array[165] = "yen";
			array[166] = "florin";
			array[167] = "section";
			array[168] = "currency";
			array[169] = "quotesingle";
			array[170] = "quotedblleft";
			array[171] = "guillemotleft";
			array[172] = "guilsinglleft";
			array[173] = "guilsinglright";
			array[174] = "fi";
			array[175] = "fl";
			array[177] = "endash";
			array[178] = "dagger";
			array[179] = "daggerdbl";
			array[180] = "periodcentered";
			array[182] = "paragraph";
			array[183] = "bullet";
			array[184] = "quotesinglbase";
			array[185] = "quotedblbase";
			array[186] = "quotedblright";
			array[187] = "guillemotright";
			array[188] = "ellipsis";
			array[189] = "perthousand";
			array[191] = "questiondown";
			array[193] = "grave";
			array[194] = "acute";
			array[195] = "circumflex";
			array[196] = "tilde";
			array[197] = "macron";
			array[198] = "breve";
			array[199] = "dotaccent";
			array[200] = "dieresis";
			array[202] = "ring";
			array[203] = "cedilla";
			array[205] = "hungarumlaut";
			array[206] = "ogonek";
			array[207] = "caron";
			array[208] = "emdash";
			array[225] = "AE";
			array[227] = "ordfeminine";
			array[232] = "Lslash";
			array[233] = "Oslash";
			array[234] = "OE";
			array[235] = "ordmasculine";
			array[241] = "ae";
			array[245] = "dotlessi";
			array[248] = "lslash";
			array[249] = "oslash";
			array[250] = "oe";
			array[251] = "germandbls";
			PdfPreview.standardEncoding = array;
			PdfPreview.nameToUnicodeTab = new PdfPreview.NameToUnicode[]
			{
				new PdfPreview.NameToUnicode(33u, "!"),
				new PdfPreview.NameToUnicode(35u, "#"),
				new PdfPreview.NameToUnicode(36u, "$"),
				new PdfPreview.NameToUnicode(37u, "%"),
				new PdfPreview.NameToUnicode(38u, "&"),
				new PdfPreview.NameToUnicode(39u, "'"),
				new PdfPreview.NameToUnicode(40u, "("),
				new PdfPreview.NameToUnicode(41u, ")"),
				new PdfPreview.NameToUnicode(42u, "*"),
				new PdfPreview.NameToUnicode(43u, "+"),
				new PdfPreview.NameToUnicode(44u, ","),
				new PdfPreview.NameToUnicode(45u, "-"),
				new PdfPreview.NameToUnicode(46u, "."),
				new PdfPreview.NameToUnicode(47u, "/"),
				new PdfPreview.NameToUnicode(48u, "0"),
				new PdfPreview.NameToUnicode(49u, "1"),
				new PdfPreview.NameToUnicode(50u, "2"),
				new PdfPreview.NameToUnicode(51u, "3"),
				new PdfPreview.NameToUnicode(52u, "4"),
				new PdfPreview.NameToUnicode(53u, "5"),
				new PdfPreview.NameToUnicode(54u, "6"),
				new PdfPreview.NameToUnicode(55u, "7"),
				new PdfPreview.NameToUnicode(56u, "8"),
				new PdfPreview.NameToUnicode(57u, "9"),
				new PdfPreview.NameToUnicode(58u, ":"),
				new PdfPreview.NameToUnicode(59u, ";"),
				new PdfPreview.NameToUnicode(60u, "<"),
				new PdfPreview.NameToUnicode(61u, "="),
				new PdfPreview.NameToUnicode(62u, ">"),
				new PdfPreview.NameToUnicode(63u, "?"),
				new PdfPreview.NameToUnicode(64u, "@"),
				new PdfPreview.NameToUnicode(65u, "A"),
				new PdfPreview.NameToUnicode(198u, "AE"),
				new PdfPreview.NameToUnicode(508u, "AEacute"),
				new PdfPreview.NameToUnicode(198u, "AEsmall"),
				new PdfPreview.NameToUnicode(193u, "Aacute"),
				new PdfPreview.NameToUnicode(193u, "Aacutesmall"),
				new PdfPreview.NameToUnicode(258u, "Abreve"),
				new PdfPreview.NameToUnicode(194u, "Acircumflex"),
				new PdfPreview.NameToUnicode(194u, "Acircumflexsmall"),
				new PdfPreview.NameToUnicode(63177u, "Acute"),
				new PdfPreview.NameToUnicode(63177u, "Acutesmall"),
				new PdfPreview.NameToUnicode(196u, "Adieresis"),
				new PdfPreview.NameToUnicode(196u, "Adieresissmall"),
				new PdfPreview.NameToUnicode(192u, "Agrave"),
				new PdfPreview.NameToUnicode(192u, "Agravesmall"),
				new PdfPreview.NameToUnicode(913u, "Alpha"),
				new PdfPreview.NameToUnicode(902u, "Alphatonos"),
				new PdfPreview.NameToUnicode(256u, "Amacron"),
				new PdfPreview.NameToUnicode(260u, "Aogonek"),
				new PdfPreview.NameToUnicode(197u, "Aring"),
				new PdfPreview.NameToUnicode(506u, "Aringacute"),
				new PdfPreview.NameToUnicode(197u, "Aringsmall"),
				new PdfPreview.NameToUnicode(65u, "Asmall"),
				new PdfPreview.NameToUnicode(195u, "Atilde"),
				new PdfPreview.NameToUnicode(195u, "Atildesmall"),
				new PdfPreview.NameToUnicode(66u, "B"),
				new PdfPreview.NameToUnicode(914u, "Beta"),
				new PdfPreview.NameToUnicode(63220u, "Brevesmall"),
				new PdfPreview.NameToUnicode(66u, "Bsmall"),
				new PdfPreview.NameToUnicode(67u, "C"),
				new PdfPreview.NameToUnicode(262u, "Cacute"),
				new PdfPreview.NameToUnicode(63178u, "Caron"),
				new PdfPreview.NameToUnicode(63178u, "Caronsmall"),
				new PdfPreview.NameToUnicode(268u, "Ccaron"),
				new PdfPreview.NameToUnicode(199u, "Ccedilla"),
				new PdfPreview.NameToUnicode(199u, "Ccedillasmall"),
				new PdfPreview.NameToUnicode(264u, "Ccircumflex"),
				new PdfPreview.NameToUnicode(266u, "Cdotaccent"),
				new PdfPreview.NameToUnicode(63416u, "Cedillasmall"),
				new PdfPreview.NameToUnicode(935u, "Chi"),
				new PdfPreview.NameToUnicode(63222u, "Circumflexsmall"),
				new PdfPreview.NameToUnicode(67u, "Csmall"),
				new PdfPreview.NameToUnicode(68u, "D"),
				new PdfPreview.NameToUnicode(270u, "Dcaron"),
				new PdfPreview.NameToUnicode(272u, "Dcroat"),
				new PdfPreview.NameToUnicode(8710u, "Delta"),
				new PdfPreview.NameToUnicode(63179u, "Dieresis"),
				new PdfPreview.NameToUnicode(63180u, "DieresisAcute"),
				new PdfPreview.NameToUnicode(63181u, "DieresisGrave"),
				new PdfPreview.NameToUnicode(63179u, "Dieresissmall"),
				new PdfPreview.NameToUnicode(63223u, "Dotaccentsmall"),
				new PdfPreview.NameToUnicode(68u, "Dsmall"),
				new PdfPreview.NameToUnicode(69u, "E"),
				new PdfPreview.NameToUnicode(201u, "Eacute"),
				new PdfPreview.NameToUnicode(201u, "Eacutesmall"),
				new PdfPreview.NameToUnicode(276u, "Ebreve"),
				new PdfPreview.NameToUnicode(282u, "Ecaron"),
				new PdfPreview.NameToUnicode(202u, "Ecircumflex"),
				new PdfPreview.NameToUnicode(202u, "Ecircumflexsmall"),
				new PdfPreview.NameToUnicode(203u, "Edieresis"),
				new PdfPreview.NameToUnicode(203u, "Edieresissmall"),
				new PdfPreview.NameToUnicode(278u, "Edotaccent"),
				new PdfPreview.NameToUnicode(200u, "Egrave"),
				new PdfPreview.NameToUnicode(200u, "Egravesmall"),
				new PdfPreview.NameToUnicode(274u, "Emacron"),
				new PdfPreview.NameToUnicode(330u, "Eng"),
				new PdfPreview.NameToUnicode(280u, "Eogonek"),
				new PdfPreview.NameToUnicode(917u, "Epsilon"),
				new PdfPreview.NameToUnicode(904u, "Epsilontonos"),
				new PdfPreview.NameToUnicode(69u, "Esmall"),
				new PdfPreview.NameToUnicode(919u, "Eta"),
				new PdfPreview.NameToUnicode(905u, "Etatonos"),
				new PdfPreview.NameToUnicode(208u, "Eth"),
				new PdfPreview.NameToUnicode(208u, "Ethsmall"),
				new PdfPreview.NameToUnicode(8364u, "Euro"),
				new PdfPreview.NameToUnicode(70u, "F"),
				new PdfPreview.NameToUnicode(70u, "Fsmall"),
				new PdfPreview.NameToUnicode(71u, "G"),
				new PdfPreview.NameToUnicode(915u, "Gamma"),
				new PdfPreview.NameToUnicode(286u, "Gbreve"),
				new PdfPreview.NameToUnicode(486u, "Gcaron"),
				new PdfPreview.NameToUnicode(284u, "Gcircumflex"),
				new PdfPreview.NameToUnicode(290u, "Gcommaaccent"),
				new PdfPreview.NameToUnicode(288u, "Gdotaccent"),
				new PdfPreview.NameToUnicode(63182u, "Grave"),
				new PdfPreview.NameToUnicode(63182u, "Gravesmall"),
				new PdfPreview.NameToUnicode(71u, "Gsmall"),
				new PdfPreview.NameToUnicode(72u, "H"),
				new PdfPreview.NameToUnicode(9679u, "H18533"),
				new PdfPreview.NameToUnicode(9642u, "H18543"),
				new PdfPreview.NameToUnicode(9643u, "H18551"),
				new PdfPreview.NameToUnicode(9633u, "H22073"),
				new PdfPreview.NameToUnicode(294u, "Hbar"),
				new PdfPreview.NameToUnicode(292u, "Hcircumflex"),
				new PdfPreview.NameToUnicode(72u, "Hsmall"),
				new PdfPreview.NameToUnicode(63183u, "Hungarumlaut"),
				new PdfPreview.NameToUnicode(63183u, "Hungarumlautsmall"),
				new PdfPreview.NameToUnicode(73u, "I"),
				new PdfPreview.NameToUnicode(306u, "IJ"),
				new PdfPreview.NameToUnicode(205u, "Iacute"),
				new PdfPreview.NameToUnicode(205u, "Iacutesmall"),
				new PdfPreview.NameToUnicode(300u, "Ibreve"),
				new PdfPreview.NameToUnicode(206u, "Icircumflex"),
				new PdfPreview.NameToUnicode(206u, "Icircumflexsmall"),
				new PdfPreview.NameToUnicode(207u, "Idieresis"),
				new PdfPreview.NameToUnicode(207u, "Idieresissmall"),
				new PdfPreview.NameToUnicode(304u, "Idotaccent"),
				new PdfPreview.NameToUnicode(8465u, "Ifraktur"),
				new PdfPreview.NameToUnicode(204u, "Igrave"),
				new PdfPreview.NameToUnicode(204u, "Igravesmall"),
				new PdfPreview.NameToUnicode(298u, "Imacron"),
				new PdfPreview.NameToUnicode(302u, "Iogonek"),
				new PdfPreview.NameToUnicode(921u, "Iota"),
				new PdfPreview.NameToUnicode(938u, "Iotadieresis"),
				new PdfPreview.NameToUnicode(906u, "Iotatonos"),
				new PdfPreview.NameToUnicode(73u, "Ismall"),
				new PdfPreview.NameToUnicode(296u, "Itilde"),
				new PdfPreview.NameToUnicode(74u, "J"),
				new PdfPreview.NameToUnicode(308u, "Jcircumflex"),
				new PdfPreview.NameToUnicode(74u, "Jsmall"),
				new PdfPreview.NameToUnicode(75u, "K"),
				new PdfPreview.NameToUnicode(922u, "Kappa"),
				new PdfPreview.NameToUnicode(310u, "Kcommaaccent"),
				new PdfPreview.NameToUnicode(75u, "Ksmall"),
				new PdfPreview.NameToUnicode(76u, "L"),
				new PdfPreview.NameToUnicode(63167u, "LL"),
				new PdfPreview.NameToUnicode(313u, "Lacute"),
				new PdfPreview.NameToUnicode(923u, "Lambda"),
				new PdfPreview.NameToUnicode(317u, "Lcaron"),
				new PdfPreview.NameToUnicode(315u, "Lcommaaccent"),
				new PdfPreview.NameToUnicode(319u, "Ldot"),
				new PdfPreview.NameToUnicode(321u, "Lslash"),
				new PdfPreview.NameToUnicode(321u, "Lslashsmall"),
				new PdfPreview.NameToUnicode(76u, "Lsmall"),
				new PdfPreview.NameToUnicode(77u, "M"),
				new PdfPreview.NameToUnicode(63184u, "Macron"),
				new PdfPreview.NameToUnicode(63184u, "Macronsmall"),
				new PdfPreview.NameToUnicode(77u, "Msmall"),
				new PdfPreview.NameToUnicode(924u, "Mu"),
				new PdfPreview.NameToUnicode(78u, "N"),
				new PdfPreview.NameToUnicode(323u, "Nacute"),
				new PdfPreview.NameToUnicode(327u, "Ncaron"),
				new PdfPreview.NameToUnicode(325u, "Ncommaaccent"),
				new PdfPreview.NameToUnicode(78u, "Nsmall"),
				new PdfPreview.NameToUnicode(209u, "Ntilde"),
				new PdfPreview.NameToUnicode(209u, "Ntildesmall"),
				new PdfPreview.NameToUnicode(925u, "Nu"),
				new PdfPreview.NameToUnicode(79u, "O"),
				new PdfPreview.NameToUnicode(338u, "OE"),
				new PdfPreview.NameToUnicode(338u, "OEsmall"),
				new PdfPreview.NameToUnicode(211u, "Oacute"),
				new PdfPreview.NameToUnicode(211u, "Oacutesmall"),
				new PdfPreview.NameToUnicode(334u, "Obreve"),
				new PdfPreview.NameToUnicode(212u, "Ocircumflex"),
				new PdfPreview.NameToUnicode(212u, "Ocircumflexsmall"),
				new PdfPreview.NameToUnicode(214u, "Odieresis"),
				new PdfPreview.NameToUnicode(214u, "Odieresissmall"),
				new PdfPreview.NameToUnicode(63227u, "Ogoneksmall"),
				new PdfPreview.NameToUnicode(210u, "Ograve"),
				new PdfPreview.NameToUnicode(210u, "Ogravesmall"),
				new PdfPreview.NameToUnicode(416u, "Ohorn"),
				new PdfPreview.NameToUnicode(336u, "Ohungarumlaut"),
				new PdfPreview.NameToUnicode(332u, "Omacron"),
				new PdfPreview.NameToUnicode(8486u, "Omega"),
				new PdfPreview.NameToUnicode(911u, "Omegatonos"),
				new PdfPreview.NameToUnicode(927u, "Omicron"),
				new PdfPreview.NameToUnicode(908u, "Omicrontonos"),
				new PdfPreview.NameToUnicode(216u, "Oslash"),
				new PdfPreview.NameToUnicode(510u, "Oslashacute"),
				new PdfPreview.NameToUnicode(216u, "Oslashsmall"),
				new PdfPreview.NameToUnicode(79u, "Osmall"),
				new PdfPreview.NameToUnicode(213u, "Otilde"),
				new PdfPreview.NameToUnicode(213u, "Otildesmall"),
				new PdfPreview.NameToUnicode(80u, "P"),
				new PdfPreview.NameToUnicode(934u, "Phi"),
				new PdfPreview.NameToUnicode(928u, "Pi"),
				new PdfPreview.NameToUnicode(936u, "Psi"),
				new PdfPreview.NameToUnicode(80u, "Psmall"),
				new PdfPreview.NameToUnicode(81u, "Q"),
				new PdfPreview.NameToUnicode(81u, "Qsmall"),
				new PdfPreview.NameToUnicode(82u, "R"),
				new PdfPreview.NameToUnicode(340u, "Racute"),
				new PdfPreview.NameToUnicode(344u, "Rcaron"),
				new PdfPreview.NameToUnicode(342u, "Rcommaaccent"),
				new PdfPreview.NameToUnicode(8476u, "Rfraktur"),
				new PdfPreview.NameToUnicode(929u, "Rho"),
				new PdfPreview.NameToUnicode(63228u, "Ringsmall"),
				new PdfPreview.NameToUnicode(82u, "Rsmall"),
				new PdfPreview.NameToUnicode(83u, "S"),
				new PdfPreview.NameToUnicode(9484u, "SF010000"),
				new PdfPreview.NameToUnicode(9492u, "SF020000"),
				new PdfPreview.NameToUnicode(9488u, "SF030000"),
				new PdfPreview.NameToUnicode(9496u, "SF040000"),
				new PdfPreview.NameToUnicode(9532u, "SF050000"),
				new PdfPreview.NameToUnicode(9516u, "SF060000"),
				new PdfPreview.NameToUnicode(9524u, "SF070000"),
				new PdfPreview.NameToUnicode(9500u, "SF080000"),
				new PdfPreview.NameToUnicode(9508u, "SF090000"),
				new PdfPreview.NameToUnicode(9472u, "SF100000"),
				new PdfPreview.NameToUnicode(9474u, "SF110000"),
				new PdfPreview.NameToUnicode(9569u, "SF190000"),
				new PdfPreview.NameToUnicode(9570u, "SF200000"),
				new PdfPreview.NameToUnicode(9558u, "SF210000"),
				new PdfPreview.NameToUnicode(9557u, "SF220000"),
				new PdfPreview.NameToUnicode(9571u, "SF230000"),
				new PdfPreview.NameToUnicode(9553u, "SF240000"),
				new PdfPreview.NameToUnicode(9559u, "SF250000"),
				new PdfPreview.NameToUnicode(9565u, "SF260000"),
				new PdfPreview.NameToUnicode(9564u, "SF270000"),
				new PdfPreview.NameToUnicode(9563u, "SF280000"),
				new PdfPreview.NameToUnicode(9566u, "SF360000"),
				new PdfPreview.NameToUnicode(9567u, "SF370000"),
				new PdfPreview.NameToUnicode(9562u, "SF380000"),
				new PdfPreview.NameToUnicode(9556u, "SF390000"),
				new PdfPreview.NameToUnicode(9577u, "SF400000"),
				new PdfPreview.NameToUnicode(9574u, "SF410000"),
				new PdfPreview.NameToUnicode(9568u, "SF420000"),
				new PdfPreview.NameToUnicode(9552u, "SF430000"),
				new PdfPreview.NameToUnicode(9580u, "SF440000"),
				new PdfPreview.NameToUnicode(9575u, "SF450000"),
				new PdfPreview.NameToUnicode(9576u, "SF460000"),
				new PdfPreview.NameToUnicode(9572u, "SF470000"),
				new PdfPreview.NameToUnicode(9573u, "SF480000"),
				new PdfPreview.NameToUnicode(9561u, "SF490000"),
				new PdfPreview.NameToUnicode(9560u, "SF500000"),
				new PdfPreview.NameToUnicode(9554u, "SF510000"),
				new PdfPreview.NameToUnicode(9555u, "SF520000"),
				new PdfPreview.NameToUnicode(9579u, "SF530000"),
				new PdfPreview.NameToUnicode(9578u, "SF540000"),
				new PdfPreview.NameToUnicode(346u, "Sacute"),
				new PdfPreview.NameToUnicode(352u, "Scaron"),
				new PdfPreview.NameToUnicode(352u, "Scaronsmall"),
				new PdfPreview.NameToUnicode(350u, "Scedilla"),
				new PdfPreview.NameToUnicode(348u, "Scircumflex"),
				new PdfPreview.NameToUnicode(536u, "Scommaaccent"),
				new PdfPreview.NameToUnicode(931u, "Sigma"),
				new PdfPreview.NameToUnicode(83u, "Ssmall"),
				new PdfPreview.NameToUnicode(84u, "T"),
				new PdfPreview.NameToUnicode(932u, "Tau"),
				new PdfPreview.NameToUnicode(358u, "Tbar"),
				new PdfPreview.NameToUnicode(356u, "Tcaron"),
				new PdfPreview.NameToUnicode(354u, "Tcommaaccent"),
				new PdfPreview.NameToUnicode(920u, "Theta"),
				new PdfPreview.NameToUnicode(222u, "Thorn"),
				new PdfPreview.NameToUnicode(222u, "Thornsmall"),
				new PdfPreview.NameToUnicode(63230u, "Tildesmall"),
				new PdfPreview.NameToUnicode(84u, "Tsmall"),
				new PdfPreview.NameToUnicode(85u, "U"),
				new PdfPreview.NameToUnicode(218u, "Uacute"),
				new PdfPreview.NameToUnicode(218u, "Uacutesmall"),
				new PdfPreview.NameToUnicode(364u, "Ubreve"),
				new PdfPreview.NameToUnicode(219u, "Ucircumflex"),
				new PdfPreview.NameToUnicode(219u, "Ucircumflexsmall"),
				new PdfPreview.NameToUnicode(220u, "Udieresis"),
				new PdfPreview.NameToUnicode(220u, "Udieresissmall"),
				new PdfPreview.NameToUnicode(217u, "Ugrave"),
				new PdfPreview.NameToUnicode(217u, "Ugravesmall"),
				new PdfPreview.NameToUnicode(431u, "Uhorn"),
				new PdfPreview.NameToUnicode(368u, "Uhungarumlaut"),
				new PdfPreview.NameToUnicode(362u, "Umacron"),
				new PdfPreview.NameToUnicode(370u, "Uogonek"),
				new PdfPreview.NameToUnicode(933u, "Upsilon"),
				new PdfPreview.NameToUnicode(978u, "Upsilon1"),
				new PdfPreview.NameToUnicode(939u, "Upsilondieresis"),
				new PdfPreview.NameToUnicode(910u, "Upsilontonos"),
				new PdfPreview.NameToUnicode(366u, "Uring"),
				new PdfPreview.NameToUnicode(85u, "Usmall"),
				new PdfPreview.NameToUnicode(360u, "Utilde"),
				new PdfPreview.NameToUnicode(86u, "V"),
				new PdfPreview.NameToUnicode(86u, "Vsmall"),
				new PdfPreview.NameToUnicode(87u, "W"),
				new PdfPreview.NameToUnicode(7810u, "Wacute"),
				new PdfPreview.NameToUnicode(372u, "Wcircumflex"),
				new PdfPreview.NameToUnicode(7812u, "Wdieresis"),
				new PdfPreview.NameToUnicode(7808u, "Wgrave"),
				new PdfPreview.NameToUnicode(87u, "Wsmall"),
				new PdfPreview.NameToUnicode(88u, "X"),
				new PdfPreview.NameToUnicode(926u, "Xi"),
				new PdfPreview.NameToUnicode(88u, "Xsmall"),
				new PdfPreview.NameToUnicode(89u, "Y"),
				new PdfPreview.NameToUnicode(221u, "Yacute"),
				new PdfPreview.NameToUnicode(221u, "Yacutesmall"),
				new PdfPreview.NameToUnicode(374u, "Ycircumflex"),
				new PdfPreview.NameToUnicode(376u, "Ydieresis"),
				new PdfPreview.NameToUnicode(376u, "Ydieresissmall"),
				new PdfPreview.NameToUnicode(7922u, "Ygrave"),
				new PdfPreview.NameToUnicode(89u, "Ysmall"),
				new PdfPreview.NameToUnicode(90u, "Z"),
				new PdfPreview.NameToUnicode(377u, "Zacute"),
				new PdfPreview.NameToUnicode(381u, "Zcaron"),
				new PdfPreview.NameToUnicode(381u, "Zcaronsmall"),
				new PdfPreview.NameToUnicode(379u, "Zdotaccent"),
				new PdfPreview.NameToUnicode(918u, "Zeta"),
				new PdfPreview.NameToUnicode(90u, "Zsmall"),
				new PdfPreview.NameToUnicode(34u, "\""),
				new PdfPreview.NameToUnicode(92u, "\\"),
				new PdfPreview.NameToUnicode(93u, "]"),
				new PdfPreview.NameToUnicode(94u, "^"),
				new PdfPreview.NameToUnicode(95u, "_"),
				new PdfPreview.NameToUnicode(96u, "`"),
				new PdfPreview.NameToUnicode(97u, "a"),
				new PdfPreview.NameToUnicode(225u, "aacute"),
				new PdfPreview.NameToUnicode(259u, "abreve"),
				new PdfPreview.NameToUnicode(226u, "acircumflex"),
				new PdfPreview.NameToUnicode(180u, "acute"),
				new PdfPreview.NameToUnicode(769u, "acutecomb"),
				new PdfPreview.NameToUnicode(228u, "adieresis"),
				new PdfPreview.NameToUnicode(230u, "ae"),
				new PdfPreview.NameToUnicode(509u, "aeacute"),
				new PdfPreview.NameToUnicode(8213u, "afii00208"),
				new PdfPreview.NameToUnicode(1040u, "afii10017"),
				new PdfPreview.NameToUnicode(1041u, "afii10018"),
				new PdfPreview.NameToUnicode(1042u, "afii10019"),
				new PdfPreview.NameToUnicode(1043u, "afii10020"),
				new PdfPreview.NameToUnicode(1044u, "afii10021"),
				new PdfPreview.NameToUnicode(1045u, "afii10022"),
				new PdfPreview.NameToUnicode(1025u, "afii10023"),
				new PdfPreview.NameToUnicode(1046u, "afii10024"),
				new PdfPreview.NameToUnicode(1047u, "afii10025"),
				new PdfPreview.NameToUnicode(1048u, "afii10026"),
				new PdfPreview.NameToUnicode(1049u, "afii10027"),
				new PdfPreview.NameToUnicode(1050u, "afii10028"),
				new PdfPreview.NameToUnicode(1051u, "afii10029"),
				new PdfPreview.NameToUnicode(1052u, "afii10030"),
				new PdfPreview.NameToUnicode(1053u, "afii10031"),
				new PdfPreview.NameToUnicode(1054u, "afii10032"),
				new PdfPreview.NameToUnicode(1055u, "afii10033"),
				new PdfPreview.NameToUnicode(1056u, "afii10034"),
				new PdfPreview.NameToUnicode(1057u, "afii10035"),
				new PdfPreview.NameToUnicode(1058u, "afii10036"),
				new PdfPreview.NameToUnicode(1059u, "afii10037"),
				new PdfPreview.NameToUnicode(1060u, "afii10038"),
				new PdfPreview.NameToUnicode(1061u, "afii10039"),
				new PdfPreview.NameToUnicode(1062u, "afii10040"),
				new PdfPreview.NameToUnicode(1063u, "afii10041"),
				new PdfPreview.NameToUnicode(1064u, "afii10042"),
				new PdfPreview.NameToUnicode(1065u, "afii10043"),
				new PdfPreview.NameToUnicode(1066u, "afii10044"),
				new PdfPreview.NameToUnicode(1067u, "afii10045"),
				new PdfPreview.NameToUnicode(1068u, "afii10046"),
				new PdfPreview.NameToUnicode(1069u, "afii10047"),
				new PdfPreview.NameToUnicode(1070u, "afii10048"),
				new PdfPreview.NameToUnicode(1071u, "afii10049"),
				new PdfPreview.NameToUnicode(1168u, "afii10050"),
				new PdfPreview.NameToUnicode(1026u, "afii10051"),
				new PdfPreview.NameToUnicode(1027u, "afii10052"),
				new PdfPreview.NameToUnicode(1028u, "afii10053"),
				new PdfPreview.NameToUnicode(1029u, "afii10054"),
				new PdfPreview.NameToUnicode(1030u, "afii10055"),
				new PdfPreview.NameToUnicode(1031u, "afii10056"),
				new PdfPreview.NameToUnicode(1032u, "afii10057"),
				new PdfPreview.NameToUnicode(1033u, "afii10058"),
				new PdfPreview.NameToUnicode(1034u, "afii10059"),
				new PdfPreview.NameToUnicode(1035u, "afii10060"),
				new PdfPreview.NameToUnicode(1036u, "afii10061"),
				new PdfPreview.NameToUnicode(1038u, "afii10062"),
				new PdfPreview.NameToUnicode(63172u, "afii10063"),
				new PdfPreview.NameToUnicode(63173u, "afii10064"),
				new PdfPreview.NameToUnicode(1072u, "afii10065"),
				new PdfPreview.NameToUnicode(1073u, "afii10066"),
				new PdfPreview.NameToUnicode(1074u, "afii10067"),
				new PdfPreview.NameToUnicode(1075u, "afii10068"),
				new PdfPreview.NameToUnicode(1076u, "afii10069"),
				new PdfPreview.NameToUnicode(1077u, "afii10070"),
				new PdfPreview.NameToUnicode(1105u, "afii10071"),
				new PdfPreview.NameToUnicode(1078u, "afii10072"),
				new PdfPreview.NameToUnicode(1079u, "afii10073"),
				new PdfPreview.NameToUnicode(1080u, "afii10074"),
				new PdfPreview.NameToUnicode(1081u, "afii10075"),
				new PdfPreview.NameToUnicode(1082u, "afii10076"),
				new PdfPreview.NameToUnicode(1083u, "afii10077"),
				new PdfPreview.NameToUnicode(1084u, "afii10078"),
				new PdfPreview.NameToUnicode(1085u, "afii10079"),
				new PdfPreview.NameToUnicode(1086u, "afii10080"),
				new PdfPreview.NameToUnicode(1087u, "afii10081"),
				new PdfPreview.NameToUnicode(1088u, "afii10082"),
				new PdfPreview.NameToUnicode(1089u, "afii10083"),
				new PdfPreview.NameToUnicode(1090u, "afii10084"),
				new PdfPreview.NameToUnicode(1091u, "afii10085"),
				new PdfPreview.NameToUnicode(1092u, "afii10086"),
				new PdfPreview.NameToUnicode(1093u, "afii10087"),
				new PdfPreview.NameToUnicode(1094u, "afii10088"),
				new PdfPreview.NameToUnicode(1095u, "afii10089"),
				new PdfPreview.NameToUnicode(1096u, "afii10090"),
				new PdfPreview.NameToUnicode(1097u, "afii10091"),
				new PdfPreview.NameToUnicode(1098u, "afii10092"),
				new PdfPreview.NameToUnicode(1099u, "afii10093"),
				new PdfPreview.NameToUnicode(1100u, "afii10094"),
				new PdfPreview.NameToUnicode(1101u, "afii10095"),
				new PdfPreview.NameToUnicode(1102u, "afii10096"),
				new PdfPreview.NameToUnicode(1103u, "afii10097"),
				new PdfPreview.NameToUnicode(1169u, "afii10098"),
				new PdfPreview.NameToUnicode(1106u, "afii10099"),
				new PdfPreview.NameToUnicode(1107u, "afii10100"),
				new PdfPreview.NameToUnicode(1108u, "afii10101"),
				new PdfPreview.NameToUnicode(1109u, "afii10102"),
				new PdfPreview.NameToUnicode(1110u, "afii10103"),
				new PdfPreview.NameToUnicode(1111u, "afii10104"),
				new PdfPreview.NameToUnicode(1112u, "afii10105"),
				new PdfPreview.NameToUnicode(1113u, "afii10106"),
				new PdfPreview.NameToUnicode(1114u, "afii10107"),
				new PdfPreview.NameToUnicode(1115u, "afii10108"),
				new PdfPreview.NameToUnicode(1116u, "afii10109"),
				new PdfPreview.NameToUnicode(1118u, "afii10110"),
				new PdfPreview.NameToUnicode(1039u, "afii10145"),
				new PdfPreview.NameToUnicode(1122u, "afii10146"),
				new PdfPreview.NameToUnicode(1138u, "afii10147"),
				new PdfPreview.NameToUnicode(1140u, "afii10148"),
				new PdfPreview.NameToUnicode(63174u, "afii10192"),
				new PdfPreview.NameToUnicode(1119u, "afii10193"),
				new PdfPreview.NameToUnicode(1123u, "afii10194"),
				new PdfPreview.NameToUnicode(1139u, "afii10195"),
				new PdfPreview.NameToUnicode(1141u, "afii10196"),
				new PdfPreview.NameToUnicode(63175u, "afii10831"),
				new PdfPreview.NameToUnicode(63176u, "afii10832"),
				new PdfPreview.NameToUnicode(1241u, "afii10846"),
				new PdfPreview.NameToUnicode(8206u, "afii299"),
				new PdfPreview.NameToUnicode(8207u, "afii300"),
				new PdfPreview.NameToUnicode(8205u, "afii301"),
				new PdfPreview.NameToUnicode(1642u, "afii57381"),
				new PdfPreview.NameToUnicode(1548u, "afii57388"),
				new PdfPreview.NameToUnicode(1632u, "afii57392"),
				new PdfPreview.NameToUnicode(1633u, "afii57393"),
				new PdfPreview.NameToUnicode(1634u, "afii57394"),
				new PdfPreview.NameToUnicode(1635u, "afii57395"),
				new PdfPreview.NameToUnicode(1636u, "afii57396"),
				new PdfPreview.NameToUnicode(1637u, "afii57397"),
				new PdfPreview.NameToUnicode(1638u, "afii57398"),
				new PdfPreview.NameToUnicode(1639u, "afii57399"),
				new PdfPreview.NameToUnicode(1640u, "afii57400"),
				new PdfPreview.NameToUnicode(1641u, "afii57401"),
				new PdfPreview.NameToUnicode(1563u, "afii57403"),
				new PdfPreview.NameToUnicode(1567u, "afii57407"),
				new PdfPreview.NameToUnicode(1569u, "afii57409"),
				new PdfPreview.NameToUnicode(1570u, "afii57410"),
				new PdfPreview.NameToUnicode(1571u, "afii57411"),
				new PdfPreview.NameToUnicode(1572u, "afii57412"),
				new PdfPreview.NameToUnicode(1573u, "afii57413"),
				new PdfPreview.NameToUnicode(1574u, "afii57414"),
				new PdfPreview.NameToUnicode(1575u, "afii57415"),
				new PdfPreview.NameToUnicode(1576u, "afii57416"),
				new PdfPreview.NameToUnicode(1577u, "afii57417"),
				new PdfPreview.NameToUnicode(1578u, "afii57418"),
				new PdfPreview.NameToUnicode(1579u, "afii57419"),
				new PdfPreview.NameToUnicode(1580u, "afii57420"),
				new PdfPreview.NameToUnicode(1581u, "afii57421"),
				new PdfPreview.NameToUnicode(1582u, "afii57422"),
				new PdfPreview.NameToUnicode(1583u, "afii57423"),
				new PdfPreview.NameToUnicode(1584u, "afii57424"),
				new PdfPreview.NameToUnicode(1585u, "afii57425"),
				new PdfPreview.NameToUnicode(1586u, "afii57426"),
				new PdfPreview.NameToUnicode(1587u, "afii57427"),
				new PdfPreview.NameToUnicode(1588u, "afii57428"),
				new PdfPreview.NameToUnicode(1589u, "afii57429"),
				new PdfPreview.NameToUnicode(1590u, "afii57430"),
				new PdfPreview.NameToUnicode(1591u, "afii57431"),
				new PdfPreview.NameToUnicode(1592u, "afii57432"),
				new PdfPreview.NameToUnicode(1593u, "afii57433"),
				new PdfPreview.NameToUnicode(1594u, "afii57434"),
				new PdfPreview.NameToUnicode(1600u, "afii57440"),
				new PdfPreview.NameToUnicode(1601u, "afii57441"),
				new PdfPreview.NameToUnicode(1602u, "afii57442"),
				new PdfPreview.NameToUnicode(1603u, "afii57443"),
				new PdfPreview.NameToUnicode(1604u, "afii57444"),
				new PdfPreview.NameToUnicode(1605u, "afii57445"),
				new PdfPreview.NameToUnicode(1606u, "afii57446"),
				new PdfPreview.NameToUnicode(1608u, "afii57448"),
				new PdfPreview.NameToUnicode(1609u, "afii57449"),
				new PdfPreview.NameToUnicode(1610u, "afii57450"),
				new PdfPreview.NameToUnicode(1611u, "afii57451"),
				new PdfPreview.NameToUnicode(1612u, "afii57452"),
				new PdfPreview.NameToUnicode(1613u, "afii57453"),
				new PdfPreview.NameToUnicode(1614u, "afii57454"),
				new PdfPreview.NameToUnicode(1615u, "afii57455"),
				new PdfPreview.NameToUnicode(1616u, "afii57456"),
				new PdfPreview.NameToUnicode(1617u, "afii57457"),
				new PdfPreview.NameToUnicode(1618u, "afii57458"),
				new PdfPreview.NameToUnicode(1607u, "afii57470"),
				new PdfPreview.NameToUnicode(1700u, "afii57505"),
				new PdfPreview.NameToUnicode(1662u, "afii57506"),
				new PdfPreview.NameToUnicode(1670u, "afii57507"),
				new PdfPreview.NameToUnicode(1688u, "afii57508"),
				new PdfPreview.NameToUnicode(1711u, "afii57509"),
				new PdfPreview.NameToUnicode(1657u, "afii57511"),
				new PdfPreview.NameToUnicode(1672u, "afii57512"),
				new PdfPreview.NameToUnicode(1681u, "afii57513"),
				new PdfPreview.NameToUnicode(1722u, "afii57514"),
				new PdfPreview.NameToUnicode(1746u, "afii57519"),
				new PdfPreview.NameToUnicode(1749u, "afii57534"),
				new PdfPreview.NameToUnicode(8362u, "afii57636"),
				new PdfPreview.NameToUnicode(1470u, "afii57645"),
				new PdfPreview.NameToUnicode(1475u, "afii57658"),
				new PdfPreview.NameToUnicode(1488u, "afii57664"),
				new PdfPreview.NameToUnicode(1489u, "afii57665"),
				new PdfPreview.NameToUnicode(1490u, "afii57666"),
				new PdfPreview.NameToUnicode(1491u, "afii57667"),
				new PdfPreview.NameToUnicode(1492u, "afii57668"),
				new PdfPreview.NameToUnicode(1493u, "afii57669"),
				new PdfPreview.NameToUnicode(1494u, "afii57670"),
				new PdfPreview.NameToUnicode(1495u, "afii57671"),
				new PdfPreview.NameToUnicode(1496u, "afii57672"),
				new PdfPreview.NameToUnicode(1497u, "afii57673"),
				new PdfPreview.NameToUnicode(1498u, "afii57674"),
				new PdfPreview.NameToUnicode(1499u, "afii57675"),
				new PdfPreview.NameToUnicode(1500u, "afii57676"),
				new PdfPreview.NameToUnicode(1501u, "afii57677"),
				new PdfPreview.NameToUnicode(1502u, "afii57678"),
				new PdfPreview.NameToUnicode(1503u, "afii57679"),
				new PdfPreview.NameToUnicode(1504u, "afii57680"),
				new PdfPreview.NameToUnicode(1505u, "afii57681"),
				new PdfPreview.NameToUnicode(1506u, "afii57682"),
				new PdfPreview.NameToUnicode(1507u, "afii57683"),
				new PdfPreview.NameToUnicode(1508u, "afii57684"),
				new PdfPreview.NameToUnicode(1509u, "afii57685"),
				new PdfPreview.NameToUnicode(1510u, "afii57686"),
				new PdfPreview.NameToUnicode(1511u, "afii57687"),
				new PdfPreview.NameToUnicode(1512u, "afii57688"),
				new PdfPreview.NameToUnicode(1513u, "afii57689"),
				new PdfPreview.NameToUnicode(1514u, "afii57690"),
				new PdfPreview.NameToUnicode(64298u, "afii57694"),
				new PdfPreview.NameToUnicode(64299u, "afii57695"),
				new PdfPreview.NameToUnicode(64331u, "afii57700"),
				new PdfPreview.NameToUnicode(64287u, "afii57705"),
				new PdfPreview.NameToUnicode(1520u, "afii57716"),
				new PdfPreview.NameToUnicode(1521u, "afii57717"),
				new PdfPreview.NameToUnicode(1522u, "afii57718"),
				new PdfPreview.NameToUnicode(64309u, "afii57723"),
				new PdfPreview.NameToUnicode(1460u, "afii57793"),
				new PdfPreview.NameToUnicode(1461u, "afii57794"),
				new PdfPreview.NameToUnicode(1462u, "afii57795"),
				new PdfPreview.NameToUnicode(1467u, "afii57796"),
				new PdfPreview.NameToUnicode(1464u, "afii57797"),
				new PdfPreview.NameToUnicode(1463u, "afii57798"),
				new PdfPreview.NameToUnicode(1456u, "afii57799"),
				new PdfPreview.NameToUnicode(1458u, "afii57800"),
				new PdfPreview.NameToUnicode(1457u, "afii57801"),
				new PdfPreview.NameToUnicode(1459u, "afii57802"),
				new PdfPreview.NameToUnicode(1474u, "afii57803"),
				new PdfPreview.NameToUnicode(1473u, "afii57804"),
				new PdfPreview.NameToUnicode(1465u, "afii57806"),
				new PdfPreview.NameToUnicode(1468u, "afii57807"),
				new PdfPreview.NameToUnicode(1469u, "afii57839"),
				new PdfPreview.NameToUnicode(1471u, "afii57841"),
				new PdfPreview.NameToUnicode(1472u, "afii57842"),
				new PdfPreview.NameToUnicode(700u, "afii57929"),
				new PdfPreview.NameToUnicode(8453u, "afii61248"),
				new PdfPreview.NameToUnicode(8467u, "afii61289"),
				new PdfPreview.NameToUnicode(8470u, "afii61352"),
				new PdfPreview.NameToUnicode(8236u, "afii61573"),
				new PdfPreview.NameToUnicode(8237u, "afii61574"),
				new PdfPreview.NameToUnicode(8238u, "afii61575"),
				new PdfPreview.NameToUnicode(8204u, "afii61664"),
				new PdfPreview.NameToUnicode(1645u, "afii63167"),
				new PdfPreview.NameToUnicode(701u, "afii64937"),
				new PdfPreview.NameToUnicode(224u, "agrave"),
				new PdfPreview.NameToUnicode(8501u, "aleph"),
				new PdfPreview.NameToUnicode(945u, "alpha"),
				new PdfPreview.NameToUnicode(940u, "alphatonos"),
				new PdfPreview.NameToUnicode(257u, "amacron"),
				new PdfPreview.NameToUnicode(38u, "ampersand"),
				new PdfPreview.NameToUnicode(38u, "ampersandsmall"),
				new PdfPreview.NameToUnicode(8736u, "angle"),
				new PdfPreview.NameToUnicode(9001u, "angleleft"),
				new PdfPreview.NameToUnicode(9002u, "angleright"),
				new PdfPreview.NameToUnicode(903u, "anoteleia"),
				new PdfPreview.NameToUnicode(261u, "aogonek"),
				new PdfPreview.NameToUnicode(8776u, "approxequal"),
				new PdfPreview.NameToUnicode(229u, "aring"),
				new PdfPreview.NameToUnicode(507u, "aringacute"),
				new PdfPreview.NameToUnicode(8596u, "arrowboth"),
				new PdfPreview.NameToUnicode(8660u, "arrowdblboth"),
				new PdfPreview.NameToUnicode(8659u, "arrowdbldown"),
				new PdfPreview.NameToUnicode(8656u, "arrowdblleft"),
				new PdfPreview.NameToUnicode(8658u, "arrowdblright"),
				new PdfPreview.NameToUnicode(8657u, "arrowdblup"),
				new PdfPreview.NameToUnicode(8595u, "arrowdown"),
				new PdfPreview.NameToUnicode(63719u, "arrowhorizex"),
				new PdfPreview.NameToUnicode(8592u, "arrowleft"),
				new PdfPreview.NameToUnicode(8594u, "arrowright"),
				new PdfPreview.NameToUnicode(8593u, "arrowup"),
				new PdfPreview.NameToUnicode(8597u, "arrowupdn"),
				new PdfPreview.NameToUnicode(8616u, "arrowupdnbse"),
				new PdfPreview.NameToUnicode(63718u, "arrowvertex"),
				new PdfPreview.NameToUnicode(94u, "asciicircum"),
				new PdfPreview.NameToUnicode(126u, "asciitilde"),
				new PdfPreview.NameToUnicode(42u, "asterisk"),
				new PdfPreview.NameToUnicode(8727u, "asteriskmath"),
				new PdfPreview.NameToUnicode(63209u, "asuperior"),
				new PdfPreview.NameToUnicode(64u, "at"),
				new PdfPreview.NameToUnicode(227u, "atilde"),
				new PdfPreview.NameToUnicode(98u, "b"),
				new PdfPreview.NameToUnicode(92u, "backslash"),
				new PdfPreview.NameToUnicode(124u, "bar"),
				new PdfPreview.NameToUnicode(946u, "beta"),
				new PdfPreview.NameToUnicode(9608u, "block"),
				new PdfPreview.NameToUnicode(63732u, "braceex"),
				new PdfPreview.NameToUnicode(123u, "braceleft"),
				new PdfPreview.NameToUnicode(63731u, "braceleftbt"),
				new PdfPreview.NameToUnicode(63730u, "braceleftmid"),
				new PdfPreview.NameToUnicode(63729u, "bracelefttp"),
				new PdfPreview.NameToUnicode(125u, "braceright"),
				new PdfPreview.NameToUnicode(63742u, "bracerightbt"),
				new PdfPreview.NameToUnicode(63741u, "bracerightmid"),
				new PdfPreview.NameToUnicode(63740u, "bracerighttp"),
				new PdfPreview.NameToUnicode(91u, "bracketleft"),
				new PdfPreview.NameToUnicode(63728u, "bracketleftbt"),
				new PdfPreview.NameToUnicode(63727u, "bracketleftex"),
				new PdfPreview.NameToUnicode(63726u, "bracketlefttp"),
				new PdfPreview.NameToUnicode(93u, "bracketright"),
				new PdfPreview.NameToUnicode(63739u, "bracketrightbt"),
				new PdfPreview.NameToUnicode(63738u, "bracketrightex"),
				new PdfPreview.NameToUnicode(63737u, "bracketrighttp"),
				new PdfPreview.NameToUnicode(728u, "breve"),
				new PdfPreview.NameToUnicode(166u, "brokenbar"),
				new PdfPreview.NameToUnicode(63210u, "bsuperior"),
				new PdfPreview.NameToUnicode(8226u, "bullet"),
				new PdfPreview.NameToUnicode(99u, "c"),
				new PdfPreview.NameToUnicode(263u, "cacute"),
				new PdfPreview.NameToUnicode(711u, "caron"),
				new PdfPreview.NameToUnicode(8629u, "carriagereturn"),
				new PdfPreview.NameToUnicode(269u, "ccaron"),
				new PdfPreview.NameToUnicode(231u, "ccedilla"),
				new PdfPreview.NameToUnicode(265u, "ccircumflex"),
				new PdfPreview.NameToUnicode(267u, "cdotaccent"),
				new PdfPreview.NameToUnicode(184u, "cedilla"),
				new PdfPreview.NameToUnicode(162u, "cent"),
				new PdfPreview.NameToUnicode(63199u, "centinferior"),
				new PdfPreview.NameToUnicode(162u, "centoldstyle"),
				new PdfPreview.NameToUnicode(63200u, "centsuperior"),
				new PdfPreview.NameToUnicode(967u, "chi"),
				new PdfPreview.NameToUnicode(9675u, "circle"),
				new PdfPreview.NameToUnicode(8855u, "circlemultiply"),
				new PdfPreview.NameToUnicode(8853u, "circleplus"),
				new PdfPreview.NameToUnicode(710u, "circumflex"),
				new PdfPreview.NameToUnicode(9827u, "club"),
				new PdfPreview.NameToUnicode(58u, "colon"),
				new PdfPreview.NameToUnicode(8353u, "colonmonetary"),
				new PdfPreview.NameToUnicode(44u, "comma"),
				new PdfPreview.NameToUnicode(63171u, "commaaccent"),
				new PdfPreview.NameToUnicode(63201u, "commainferior"),
				new PdfPreview.NameToUnicode(63202u, "commasuperior"),
				new PdfPreview.NameToUnicode(8773u, "congruent"),
				new PdfPreview.NameToUnicode(169u, "copyright"),
				new PdfPreview.NameToUnicode(169u, "copyrightsans"),
				new PdfPreview.NameToUnicode(169u, "copyrightserif"),
				new PdfPreview.NameToUnicode(164u, "currency"),
				new PdfPreview.NameToUnicode(63185u, "cyrBreve"),
				new PdfPreview.NameToUnicode(63186u, "cyrFlex"),
				new PdfPreview.NameToUnicode(63188u, "cyrbreve"),
				new PdfPreview.NameToUnicode(63189u, "cyrflex"),
				new PdfPreview.NameToUnicode(100u, "d"),
				new PdfPreview.NameToUnicode(8224u, "dagger"),
				new PdfPreview.NameToUnicode(8225u, "daggerdbl"),
				new PdfPreview.NameToUnicode(63187u, "dblGrave"),
				new PdfPreview.NameToUnicode(63190u, "dblgrave"),
				new PdfPreview.NameToUnicode(271u, "dcaron"),
				new PdfPreview.NameToUnicode(273u, "dcroat"),
				new PdfPreview.NameToUnicode(176u, "degree"),
				new PdfPreview.NameToUnicode(948u, "delta"),
				new PdfPreview.NameToUnicode(9830u, "diamond"),
				new PdfPreview.NameToUnicode(168u, "dieresis"),
				new PdfPreview.NameToUnicode(63191u, "dieresisacute"),
				new PdfPreview.NameToUnicode(63192u, "dieresisgrave"),
				new PdfPreview.NameToUnicode(901u, "dieresistonos"),
				new PdfPreview.NameToUnicode(247u, "divide"),
				new PdfPreview.NameToUnicode(9619u, "dkshade"),
				new PdfPreview.NameToUnicode(9604u, "dnblock"),
				new PdfPreview.NameToUnicode(36u, "dollar"),
				new PdfPreview.NameToUnicode(63203u, "dollarinferior"),
				new PdfPreview.NameToUnicode(36u, "dollaroldstyle"),
				new PdfPreview.NameToUnicode(63204u, "dollarsuperior"),
				new PdfPreview.NameToUnicode(8363u, "dong"),
				new PdfPreview.NameToUnicode(729u, "dotaccent"),
				new PdfPreview.NameToUnicode(803u, "dotbelowcomb"),
				new PdfPreview.NameToUnicode(305u, "dotlessi"),
				new PdfPreview.NameToUnicode(63166u, "dotlessj"),
				new PdfPreview.NameToUnicode(8901u, "dotmath"),
				new PdfPreview.NameToUnicode(63211u, "dsuperior"),
				new PdfPreview.NameToUnicode(101u, "e"),
				new PdfPreview.NameToUnicode(233u, "eacute"),
				new PdfPreview.NameToUnicode(277u, "ebreve"),
				new PdfPreview.NameToUnicode(283u, "ecaron"),
				new PdfPreview.NameToUnicode(234u, "ecircumflex"),
				new PdfPreview.NameToUnicode(235u, "edieresis"),
				new PdfPreview.NameToUnicode(279u, "edotaccent"),
				new PdfPreview.NameToUnicode(232u, "egrave"),
				new PdfPreview.NameToUnicode(56u, "eight"),
				new PdfPreview.NameToUnicode(8328u, "eightinferior"),
				new PdfPreview.NameToUnicode(56u, "eightoldstyle"),
				new PdfPreview.NameToUnicode(8312u, "eightsuperior"),
				new PdfPreview.NameToUnicode(8712u, "element"),
				new PdfPreview.NameToUnicode(8230u, "ellipsis"),
				new PdfPreview.NameToUnicode(275u, "emacron"),
				new PdfPreview.NameToUnicode(8212u, "emdash"),
				new PdfPreview.NameToUnicode(8709u, "emptyset"),
				new PdfPreview.NameToUnicode(8211u, "endash"),
				new PdfPreview.NameToUnicode(331u, "eng"),
				new PdfPreview.NameToUnicode(281u, "eogonek"),
				new PdfPreview.NameToUnicode(949u, "epsilon"),
				new PdfPreview.NameToUnicode(941u, "epsilontonos"),
				new PdfPreview.NameToUnicode(61u, "equal"),
				new PdfPreview.NameToUnicode(8801u, "equivalence"),
				new PdfPreview.NameToUnicode(8494u, "estimated"),
				new PdfPreview.NameToUnicode(63212u, "esuperior"),
				new PdfPreview.NameToUnicode(951u, "eta"),
				new PdfPreview.NameToUnicode(942u, "etatonos"),
				new PdfPreview.NameToUnicode(240u, "eth"),
				new PdfPreview.NameToUnicode(33u, "exclam"),
				new PdfPreview.NameToUnicode(8252u, "exclamdbl"),
				new PdfPreview.NameToUnicode(161u, "exclamdown"),
				new PdfPreview.NameToUnicode(161u, "exclamdownsmall"),
				new PdfPreview.NameToUnicode(33u, "exclamleft"),
				new PdfPreview.NameToUnicode(33u, "exclamsmall"),
				new PdfPreview.NameToUnicode(8707u, "existential"),
				new PdfPreview.NameToUnicode(102u, "f"),
				new PdfPreview.NameToUnicode(9792u, "female"),
				new PdfPreview.NameToUnicode(64256u, "ff"),
				new PdfPreview.NameToUnicode(64259u, "ffi"),
				new PdfPreview.NameToUnicode(64260u, "ffl"),
				new PdfPreview.NameToUnicode(64257u, "fi"),
				new PdfPreview.NameToUnicode(8210u, "figuredash"),
				new PdfPreview.NameToUnicode(9632u, "filledbox"),
				new PdfPreview.NameToUnicode(9644u, "filledrect"),
				new PdfPreview.NameToUnicode(53u, "five"),
				new PdfPreview.NameToUnicode(8541u, "fiveeighths"),
				new PdfPreview.NameToUnicode(8325u, "fiveinferior"),
				new PdfPreview.NameToUnicode(53u, "fiveoldstyle"),
				new PdfPreview.NameToUnicode(8309u, "fivesuperior"),
				new PdfPreview.NameToUnicode(64258u, "fl"),
				new PdfPreview.NameToUnicode(402u, "florin"),
				new PdfPreview.NameToUnicode(52u, "four"),
				new PdfPreview.NameToUnicode(8324u, "fourinferior"),
				new PdfPreview.NameToUnicode(52u, "fouroldstyle"),
				new PdfPreview.NameToUnicode(8308u, "foursuperior"),
				new PdfPreview.NameToUnicode(8260u, "fraction"),
				new PdfPreview.NameToUnicode(8355u, "franc"),
				new PdfPreview.NameToUnicode(103u, "g"),
				new PdfPreview.NameToUnicode(947u, "gamma"),
				new PdfPreview.NameToUnicode(287u, "gbreve"),
				new PdfPreview.NameToUnicode(487u, "gcaron"),
				new PdfPreview.NameToUnicode(285u, "gcircumflex"),
				new PdfPreview.NameToUnicode(291u, "gcommaaccent"),
				new PdfPreview.NameToUnicode(289u, "gdotaccent"),
				new PdfPreview.NameToUnicode(223u, "germandbls"),
				new PdfPreview.NameToUnicode(8711u, "gradient"),
				new PdfPreview.NameToUnicode(96u, "grave"),
				new PdfPreview.NameToUnicode(768u, "gravecomb"),
				new PdfPreview.NameToUnicode(62u, "greater"),
				new PdfPreview.NameToUnicode(8805u, "greaterequal"),
				new PdfPreview.NameToUnicode(171u, "guillemotleft"),
				new PdfPreview.NameToUnicode(187u, "guillemotright"),
				new PdfPreview.NameToUnicode(8249u, "guilsinglleft"),
				new PdfPreview.NameToUnicode(8250u, "guilsinglright"),
				new PdfPreview.NameToUnicode(104u, "h"),
				new PdfPreview.NameToUnicode(295u, "hbar"),
				new PdfPreview.NameToUnicode(293u, "hcircumflex"),
				new PdfPreview.NameToUnicode(9829u, "heart"),
				new PdfPreview.NameToUnicode(777u, "hookabovecomb"),
				new PdfPreview.NameToUnicode(8962u, "house"),
				new PdfPreview.NameToUnicode(733u, "hungarumlaut"),
				new PdfPreview.NameToUnicode(45u, "hyphen"),
				new PdfPreview.NameToUnicode(63205u, "hypheninferior"),
				new PdfPreview.NameToUnicode(63206u, "hyphensuperior"),
				new PdfPreview.NameToUnicode(105u, "i"),
				new PdfPreview.NameToUnicode(237u, "iacute"),
				new PdfPreview.NameToUnicode(301u, "ibreve"),
				new PdfPreview.NameToUnicode(238u, "icircumflex"),
				new PdfPreview.NameToUnicode(239u, "idieresis"),
				new PdfPreview.NameToUnicode(236u, "igrave"),
				new PdfPreview.NameToUnicode(307u, "ij"),
				new PdfPreview.NameToUnicode(299u, "imacron"),
				new PdfPreview.NameToUnicode(8734u, "infinity"),
				new PdfPreview.NameToUnicode(8747u, "integral"),
				new PdfPreview.NameToUnicode(8993u, "integralbt"),
				new PdfPreview.NameToUnicode(63733u, "integralex"),
				new PdfPreview.NameToUnicode(8992u, "integraltp"),
				new PdfPreview.NameToUnicode(8745u, "intersection"),
				new PdfPreview.NameToUnicode(9688u, "invbullet"),
				new PdfPreview.NameToUnicode(9689u, "invcircle"),
				new PdfPreview.NameToUnicode(9787u, "invsmileface"),
				new PdfPreview.NameToUnicode(303u, "iogonek"),
				new PdfPreview.NameToUnicode(953u, "iota"),
				new PdfPreview.NameToUnicode(970u, "iotadieresis"),
				new PdfPreview.NameToUnicode(912u, "iotadieresistonos"),
				new PdfPreview.NameToUnicode(943u, "iotatonos"),
				new PdfPreview.NameToUnicode(63213u, "isuperior"),
				new PdfPreview.NameToUnicode(297u, "itilde"),
				new PdfPreview.NameToUnicode(106u, "j"),
				new PdfPreview.NameToUnicode(309u, "jcircumflex"),
				new PdfPreview.NameToUnicode(107u, "k"),
				new PdfPreview.NameToUnicode(954u, "kappa"),
				new PdfPreview.NameToUnicode(311u, "kcommaaccent"),
				new PdfPreview.NameToUnicode(312u, "kgreenlandic"),
				new PdfPreview.NameToUnicode(108u, "l"),
				new PdfPreview.NameToUnicode(314u, "lacute"),
				new PdfPreview.NameToUnicode(955u, "lambda"),
				new PdfPreview.NameToUnicode(318u, "lcaron"),
				new PdfPreview.NameToUnicode(316u, "lcommaaccent"),
				new PdfPreview.NameToUnicode(320u, "ldot"),
				new PdfPreview.NameToUnicode(60u, "less"),
				new PdfPreview.NameToUnicode(8804u, "lessequal"),
				new PdfPreview.NameToUnicode(9612u, "lfblock"),
				new PdfPreview.NameToUnicode(8356u, "lira"),
				new PdfPreview.NameToUnicode(63168u, "ll"),
				new PdfPreview.NameToUnicode(8743u, "logicaland"),
				new PdfPreview.NameToUnicode(172u, "logicalnot"),
				new PdfPreview.NameToUnicode(8744u, "logicalor"),
				new PdfPreview.NameToUnicode(383u, "longs"),
				new PdfPreview.NameToUnicode(9674u, "lozenge"),
				new PdfPreview.NameToUnicode(322u, "lslash"),
				new PdfPreview.NameToUnicode(63214u, "lsuperior"),
				new PdfPreview.NameToUnicode(9617u, "ltshade"),
				new PdfPreview.NameToUnicode(109u, "m"),
				new PdfPreview.NameToUnicode(175u, "macron"),
				new PdfPreview.NameToUnicode(9794u, "male"),
				new PdfPreview.NameToUnicode(8722u, "minus"),
				new PdfPreview.NameToUnicode(8242u, "minute"),
				new PdfPreview.NameToUnicode(63215u, "msuperior"),
				new PdfPreview.NameToUnicode(181u, "mu"),
				new PdfPreview.NameToUnicode(215u, "multiply"),
				new PdfPreview.NameToUnicode(9834u, "musicalnote"),
				new PdfPreview.NameToUnicode(9835u, "musicalnotedbl"),
				new PdfPreview.NameToUnicode(110u, "n"),
				new PdfPreview.NameToUnicode(324u, "nacute"),
				new PdfPreview.NameToUnicode(329u, "napostrophe"),
				new PdfPreview.NameToUnicode(160u, "nbspace"),
				new PdfPreview.NameToUnicode(328u, "ncaron"),
				new PdfPreview.NameToUnicode(326u, "ncommaaccent"),
				new PdfPreview.NameToUnicode(57u, "nine"),
				new PdfPreview.NameToUnicode(8329u, "nineinferior"),
				new PdfPreview.NameToUnicode(57u, "nineoldstyle"),
				new PdfPreview.NameToUnicode(8313u, "ninesuperior"),
				new PdfPreview.NameToUnicode(160u, "nonbreakingspace"),
				new PdfPreview.NameToUnicode(8713u, "notelement"),
				new PdfPreview.NameToUnicode(8800u, "notequal"),
				new PdfPreview.NameToUnicode(8836u, "notsubset"),
				new PdfPreview.NameToUnicode(8319u, "nsuperior"),
				new PdfPreview.NameToUnicode(241u, "ntilde"),
				new PdfPreview.NameToUnicode(957u, "nu"),
				new PdfPreview.NameToUnicode(35u, "numbersign"),
				new PdfPreview.NameToUnicode(111u, "o"),
				new PdfPreview.NameToUnicode(243u, "oacute"),
				new PdfPreview.NameToUnicode(335u, "obreve"),
				new PdfPreview.NameToUnicode(244u, "ocircumflex"),
				new PdfPreview.NameToUnicode(246u, "odieresis"),
				new PdfPreview.NameToUnicode(339u, "oe"),
				new PdfPreview.NameToUnicode(731u, "ogonek"),
				new PdfPreview.NameToUnicode(242u, "ograve"),
				new PdfPreview.NameToUnicode(417u, "ohorn"),
				new PdfPreview.NameToUnicode(337u, "ohungarumlaut"),
				new PdfPreview.NameToUnicode(333u, "omacron"),
				new PdfPreview.NameToUnicode(969u, "omega"),
				new PdfPreview.NameToUnicode(982u, "omega1"),
				new PdfPreview.NameToUnicode(974u, "omegatonos"),
				new PdfPreview.NameToUnicode(959u, "omicron"),
				new PdfPreview.NameToUnicode(972u, "omicrontonos"),
				new PdfPreview.NameToUnicode(49u, "one"),
				new PdfPreview.NameToUnicode(8228u, "onedotenleader"),
				new PdfPreview.NameToUnicode(8539u, "oneeighth"),
				new PdfPreview.NameToUnicode(63196u, "onefitted"),
				new PdfPreview.NameToUnicode(189u, "onehalf"),
				new PdfPreview.NameToUnicode(8321u, "oneinferior"),
				new PdfPreview.NameToUnicode(49u, "oneoldstyle"),
				new PdfPreview.NameToUnicode(188u, "onequarter"),
				new PdfPreview.NameToUnicode(185u, "onesuperior"),
				new PdfPreview.NameToUnicode(8531u, "onethird"),
				new PdfPreview.NameToUnicode(9702u, "openbullet"),
				new PdfPreview.NameToUnicode(170u, "ordfeminine"),
				new PdfPreview.NameToUnicode(186u, "ordmasculine"),
				new PdfPreview.NameToUnicode(8735u, "orthogonal"),
				new PdfPreview.NameToUnicode(248u, "oslash"),
				new PdfPreview.NameToUnicode(511u, "oslashacute"),
				new PdfPreview.NameToUnicode(63216u, "osuperior"),
				new PdfPreview.NameToUnicode(245u, "otilde"),
				new PdfPreview.NameToUnicode(112u, "p"),
				new PdfPreview.NameToUnicode(182u, "paragraph"),
				new PdfPreview.NameToUnicode(40u, "parenleft"),
				new PdfPreview.NameToUnicode(63725u, "parenleftbt"),
				new PdfPreview.NameToUnicode(63724u, "parenleftex"),
				new PdfPreview.NameToUnicode(8333u, "parenleftinferior"),
				new PdfPreview.NameToUnicode(8317u, "parenleftsuperior"),
				new PdfPreview.NameToUnicode(63723u, "parenlefttp"),
				new PdfPreview.NameToUnicode(41u, "parenright"),
				new PdfPreview.NameToUnicode(63736u, "parenrightbt"),
				new PdfPreview.NameToUnicode(63735u, "parenrightex"),
				new PdfPreview.NameToUnicode(8334u, "parenrightinferior"),
				new PdfPreview.NameToUnicode(8318u, "parenrightsuperior"),
				new PdfPreview.NameToUnicode(63734u, "parenrighttp"),
				new PdfPreview.NameToUnicode(8706u, "partialdiff"),
				new PdfPreview.NameToUnicode(37u, "percent"),
				new PdfPreview.NameToUnicode(46u, "period"),
				new PdfPreview.NameToUnicode(183u, "periodcentered"),
				new PdfPreview.NameToUnicode(63207u, "periodinferior"),
				new PdfPreview.NameToUnicode(63208u, "periodsuperior"),
				new PdfPreview.NameToUnicode(8869u, "perpendicular"),
				new PdfPreview.NameToUnicode(8240u, "perthousand"),
				new PdfPreview.NameToUnicode(8359u, "peseta"),
				new PdfPreview.NameToUnicode(966u, "phi"),
				new PdfPreview.NameToUnicode(981u, "phi1"),
				new PdfPreview.NameToUnicode(960u, "pi"),
				new PdfPreview.NameToUnicode(43u, "plus"),
				new PdfPreview.NameToUnicode(177u, "plusminus"),
				new PdfPreview.NameToUnicode(8478u, "prescription"),
				new PdfPreview.NameToUnicode(8719u, "product"),
				new PdfPreview.NameToUnicode(8834u, "propersubset"),
				new PdfPreview.NameToUnicode(8835u, "propersuperset"),
				new PdfPreview.NameToUnicode(8733u, "proportional"),
				new PdfPreview.NameToUnicode(968u, "psi"),
				new PdfPreview.NameToUnicode(113u, "q"),
				new PdfPreview.NameToUnicode(63u, "question"),
				new PdfPreview.NameToUnicode(191u, "questiondown"),
				new PdfPreview.NameToUnicode(191u, "questiondownsmall"),
				new PdfPreview.NameToUnicode(63u, "questionsmall"),
				new PdfPreview.NameToUnicode(34u, "quotedbl"),
				new PdfPreview.NameToUnicode(8222u, "quotedblbase"),
				new PdfPreview.NameToUnicode(8220u, "quotedblleft"),
				new PdfPreview.NameToUnicode(8221u, "quotedblright"),
				new PdfPreview.NameToUnicode(8216u, "quoteleft"),
				new PdfPreview.NameToUnicode(8219u, "quotereversed"),
				new PdfPreview.NameToUnicode(8217u, "quoteright"),
				new PdfPreview.NameToUnicode(8218u, "quotesinglbase"),
				new PdfPreview.NameToUnicode(39u, "quotesingle"),
				new PdfPreview.NameToUnicode(114u, "r"),
				new PdfPreview.NameToUnicode(341u, "racute"),
				new PdfPreview.NameToUnicode(8730u, "radical"),
				new PdfPreview.NameToUnicode(63717u, "radicalex"),
				new PdfPreview.NameToUnicode(345u, "rcaron"),
				new PdfPreview.NameToUnicode(343u, "rcommaaccent"),
				new PdfPreview.NameToUnicode(8838u, "reflexsubset"),
				new PdfPreview.NameToUnicode(8839u, "reflexsuperset"),
				new PdfPreview.NameToUnicode(174u, "registered"),
				new PdfPreview.NameToUnicode(174u, "registersans"),
				new PdfPreview.NameToUnicode(174u, "registerserif"),
				new PdfPreview.NameToUnicode(8976u, "revlogicalnot"),
				new PdfPreview.NameToUnicode(961u, "rho"),
				new PdfPreview.NameToUnicode(730u, "ring"),
				new PdfPreview.NameToUnicode(63217u, "rsuperior"),
				new PdfPreview.NameToUnicode(9616u, "rtblock"),
				new PdfPreview.NameToUnicode(63197u, "rupiah"),
				new PdfPreview.NameToUnicode(115u, "s"),
				new PdfPreview.NameToUnicode(347u, "sacute"),
				new PdfPreview.NameToUnicode(353u, "scaron"),
				new PdfPreview.NameToUnicode(351u, "scedilla"),
				new PdfPreview.NameToUnicode(349u, "scircumflex"),
				new PdfPreview.NameToUnicode(537u, "scommaaccent"),
				new PdfPreview.NameToUnicode(8243u, "second"),
				new PdfPreview.NameToUnicode(167u, "section"),
				new PdfPreview.NameToUnicode(59u, "semicolon"),
				new PdfPreview.NameToUnicode(55u, "seven"),
				new PdfPreview.NameToUnicode(8542u, "seveneighths"),
				new PdfPreview.NameToUnicode(8327u, "seveninferior"),
				new PdfPreview.NameToUnicode(55u, "sevenoldstyle"),
				new PdfPreview.NameToUnicode(8311u, "sevensuperior"),
				new PdfPreview.NameToUnicode(9618u, "shade"),
				new PdfPreview.NameToUnicode(963u, "sigma"),
				new PdfPreview.NameToUnicode(962u, "sigma1"),
				new PdfPreview.NameToUnicode(8764u, "similar"),
				new PdfPreview.NameToUnicode(54u, "six"),
				new PdfPreview.NameToUnicode(8326u, "sixinferior"),
				new PdfPreview.NameToUnicode(54u, "sixoldstyle"),
				new PdfPreview.NameToUnicode(8310u, "sixsuperior"),
				new PdfPreview.NameToUnicode(47u, "slash"),
				new PdfPreview.NameToUnicode(9786u, "smileface"),
				new PdfPreview.NameToUnicode(32u, "space"),
				new PdfPreview.NameToUnicode(9824u, "spade"),
				new PdfPreview.NameToUnicode(63218u, "ssuperior"),
				new PdfPreview.NameToUnicode(163u, "sterling"),
				new PdfPreview.NameToUnicode(8715u, "suchthat"),
				new PdfPreview.NameToUnicode(8721u, "summation"),
				new PdfPreview.NameToUnicode(9788u, "sun"),
				new PdfPreview.NameToUnicode(116u, "t"),
				new PdfPreview.NameToUnicode(964u, "tau"),
				new PdfPreview.NameToUnicode(359u, "tbar"),
				new PdfPreview.NameToUnicode(357u, "tcaron"),
				new PdfPreview.NameToUnicode(355u, "tcommaaccent"),
				new PdfPreview.NameToUnicode(8756u, "therefore"),
				new PdfPreview.NameToUnicode(952u, "theta"),
				new PdfPreview.NameToUnicode(977u, "theta1"),
				new PdfPreview.NameToUnicode(254u, "thorn"),
				new PdfPreview.NameToUnicode(51u, "three"),
				new PdfPreview.NameToUnicode(8540u, "threeeighths"),
				new PdfPreview.NameToUnicode(8323u, "threeinferior"),
				new PdfPreview.NameToUnicode(51u, "threeoldstyle"),
				new PdfPreview.NameToUnicode(190u, "threequarters"),
				new PdfPreview.NameToUnicode(63198u, "threequartersemdash"),
				new PdfPreview.NameToUnicode(179u, "threesuperior"),
				new PdfPreview.NameToUnicode(732u, "tilde"),
				new PdfPreview.NameToUnicode(771u, "tildecomb"),
				new PdfPreview.NameToUnicode(900u, "tonos"),
				new PdfPreview.NameToUnicode(8482u, "trademark"),
				new PdfPreview.NameToUnicode(8482u, "trademarksans"),
				new PdfPreview.NameToUnicode(8482u, "trademarkserif"),
				new PdfPreview.NameToUnicode(9660u, "triagdn"),
				new PdfPreview.NameToUnicode(9668u, "triaglf"),
				new PdfPreview.NameToUnicode(9658u, "triagrt"),
				new PdfPreview.NameToUnicode(9650u, "triagup"),
				new PdfPreview.NameToUnicode(63219u, "tsuperior"),
				new PdfPreview.NameToUnicode(50u, "two"),
				new PdfPreview.NameToUnicode(8229u, "twodotenleader"),
				new PdfPreview.NameToUnicode(8322u, "twoinferior"),
				new PdfPreview.NameToUnicode(50u, "twooldstyle"),
				new PdfPreview.NameToUnicode(178u, "twosuperior"),
				new PdfPreview.NameToUnicode(8532u, "twothirds"),
				new PdfPreview.NameToUnicode(117u, "u"),
				new PdfPreview.NameToUnicode(250u, "uacute"),
				new PdfPreview.NameToUnicode(365u, "ubreve"),
				new PdfPreview.NameToUnicode(251u, "ucircumflex"),
				new PdfPreview.NameToUnicode(252u, "udieresis"),
				new PdfPreview.NameToUnicode(249u, "ugrave"),
				new PdfPreview.NameToUnicode(432u, "uhorn"),
				new PdfPreview.NameToUnicode(369u, "uhungarumlaut"),
				new PdfPreview.NameToUnicode(363u, "umacron"),
				new PdfPreview.NameToUnicode(95u, "underscore"),
				new PdfPreview.NameToUnicode(8215u, "underscoredbl"),
				new PdfPreview.NameToUnicode(8746u, "union"),
				new PdfPreview.NameToUnicode(8704u, "universal"),
				new PdfPreview.NameToUnicode(371u, "uogonek"),
				new PdfPreview.NameToUnicode(9600u, "upblock"),
				new PdfPreview.NameToUnicode(965u, "upsilon"),
				new PdfPreview.NameToUnicode(971u, "upsilondieresis"),
				new PdfPreview.NameToUnicode(944u, "upsilondieresistonos"),
				new PdfPreview.NameToUnicode(973u, "upsilontonos"),
				new PdfPreview.NameToUnicode(367u, "uring"),
				new PdfPreview.NameToUnicode(361u, "utilde"),
				new PdfPreview.NameToUnicode(118u, "v"),
				new PdfPreview.NameToUnicode(119u, "w"),
				new PdfPreview.NameToUnicode(7811u, "wacute"),
				new PdfPreview.NameToUnicode(373u, "wcircumflex"),
				new PdfPreview.NameToUnicode(7813u, "wdieresis"),
				new PdfPreview.NameToUnicode(8472u, "weierstrass"),
				new PdfPreview.NameToUnicode(7809u, "wgrave"),
				new PdfPreview.NameToUnicode(120u, "x"),
				new PdfPreview.NameToUnicode(958u, "xi"),
				new PdfPreview.NameToUnicode(121u, "y"),
				new PdfPreview.NameToUnicode(253u, "yacute"),
				new PdfPreview.NameToUnicode(375u, "ycircumflex"),
				new PdfPreview.NameToUnicode(255u, "ydieresis"),
				new PdfPreview.NameToUnicode(165u, "yen"),
				new PdfPreview.NameToUnicode(7923u, "ygrave"),
				new PdfPreview.NameToUnicode(122u, "z"),
				new PdfPreview.NameToUnicode(378u, "zacute"),
				new PdfPreview.NameToUnicode(382u, "zcaron"),
				new PdfPreview.NameToUnicode(380u, "zdotaccent"),
				new PdfPreview.NameToUnicode(48u, "zero"),
				new PdfPreview.NameToUnicode(8320u, "zeroinferior"),
				new PdfPreview.NameToUnicode(48u, "zerooldstyle"),
				new PdfPreview.NameToUnicode(8304u, "zerosuperior"),
				new PdfPreview.NameToUnicode(950u, "zeta"),
				new PdfPreview.NameToUnicode(123u, "new NameToUnicode( "),
				new PdfPreview.NameToUnicode(124u, "|"),
				new PdfPreview.NameToUnicode(125u, ")"),
				new PdfPreview.NameToUnicode(126u, "~"),
				new PdfPreview.NameToUnicode(0u, null)
			};
		}
	}
}
