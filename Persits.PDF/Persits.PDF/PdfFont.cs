// Type: Persits.PDF.PdfFont
// Assembly: Persits.PDF, Version=2.5.0.19989, Culture=neutral, PublicKeyToken=b8d8c63f1ff142a8
// Assembly location: C:\Users\hernan\Desktop\asppdf_net\Samples\Bin\Persits.PDF.dll

using System;
using System.Collections.Generic;
using System.Text;

namespace Persits.PDF
{
  public class PdfFont
  {
    private const int CP_MACCP = 2;
    private static string[] m_arrGlyphNames;
    private static ushort[] m_arrGlyphCodes;
    private static ushort[] m_arrPDFEncodingArray;
    internal string m_bstrFace;
    internal string m_bstrFamily;
    internal string m_bstrFaceAsObtainedFromTTFFile;
    internal string m_bstrFontName;
    internal pdfCharSets m_nCharSet;
    internal string m_bstrType;
    internal ushort[] m_pWidths;
    internal int m_nFirstChar;
    internal int m_nLastChar;
    internal int m_nUnitsPerEm;
    internal int m_nItalicAngle;
    internal uint m_nEMSquare;
    internal int m_nAscent;
    internal int m_nDescent;
    internal ushort m_nDefaultWidth;
    internal int m_nLineSpacing;
    internal uint m_nCapHeight;
    internal PdfRect m_rectFontBox;
    internal bool m_bParsed;
    internal int m_nVerticalExtent;
    internal bool m_bDingbatOrSymbol;
    internal bool m_bSymbolFont;
    internal PdfManager m_pManager;
    internal PdfDocument m_pDoc;
    internal int m_nEmbedding;
    internal int m_nID;
    internal string m_szID;
    internal ushort[] m_glyphIdArray;
    internal ushort[] m_endCount;
    internal ushort[] m_startCount;
    internal ushort[] m_idRangeOffset;
    internal ushort[] m_idDelta;
    internal List<GlyphInfo> m_arrGlyphs;
    internal int m_nCmapCount;
    internal int m_nHorMetricCount;
    internal HorMetric[] m_HorMetrics;
    internal PdfIndirectObj m_pFontObj;
    internal PdfIndirectObj m_pToUnicodeObj;
    internal PdfIndirectObj m_pFontDescrObj;
    internal PdfIndirectObj m_pFontFileObj;
    internal string m_bstrPath;
    internal string m_bstrEncoding;
    internal string m_bstrNameInResource;
    internal int m_nObjNum;
    internal int m_nGenNum;
    internal int m_nToUnicodeObjNum;
    internal int m_nToUnicodeGenNum;
    internal int m_nEncodingObjNum;
    internal int m_nEncodingGenNum;
    internal List<MapEntry> m_arrCmap;
    internal List<MapEntry> m_arrDifferences;
    internal bool m_bMacRomanEncoding;
    internal Dictionary<string, int> m_mapTableLength;
    internal Dictionary<string, int> m_mapTableOffset;
    internal Dictionary<string, int> m_mapTableChecksum;
    internal PdfStream[] m_arrTables;
    internal static string[] m_arrTableNames;
    internal int m_nLocaFormat;
    internal uint[] m_locaOffsets;
    internal int m_nLocaLength;
    internal uint[] m_locaNew;
    internal int m_dwGlyphTableRealSize;
    internal int m_dwLocaTableRealSize;
    internal bool m_bBarcodeFont;
    internal PdfStream m_streamBarcodeFont;
    internal List<int> m_arrGlyphIDs;
    internal ushort m_nMacStyle;
    internal static ushort[] m_arrVerticalExtents;
    internal static ushort[][] m_arrFontWidths;
    internal static short[] m_arrDescents;
    internal static ushort[] m_arrCapHeights;

    public string Face
    {
      get
      {
        return this.m_bstrFace;
      }
    }

    public string Family
    {
      get
      {
        return this.m_bstrFamily;
      }
    }

    internal pdfCharSets CharSet
    {
      get
      {
        return this.m_nCharSet;
      }
    }

    public int UnitsPerEm
    {
      get
      {
        return this.m_nUnitsPerEm;
      }
    }

    public int CapHeight
    {
      get
      {
        return (int) this.m_nCapHeight;
      }
    }

    public int Embedding
    {
      get
      {
        return this.m_nEmbedding;
      }
    }

    public bool SymbolFont
    {
      get
      {
        return this.m_bSymbolFont;
      }
    }

    static PdfFont()
    {
      PdfFont.m_arrGlyphNames = new string[1051]
      {
        "A",
        "AE",
        "AEacute",
        "AEsmall",
        "Aacute",
        "Aacutesmall",
        "Abreve",
        "Acircumflex",
        "Acircumflexsmall",
        "Acute",
        "Acutesmall",
        "Adieresis",
        "Adieresissmall",
        "Agrave",
        "Agravesmall",
        "Alpha",
        "Alphatonos",
        "Amacron",
        "Aogonek",
        "Aring",
        "Aringacute",
        "Aringsmall",
        "Asmall",
        "Atilde",
        "Atildesmall",
        "B",
        "Beta",
        "Brevesmall",
        "Bsmall",
        "C",
        "Cacute",
        "Caron",
        "Caronsmall",
        "Ccaron",
        "Ccedilla",
        "Ccedillasmall",
        "Ccircumflex",
        "Cdotaccent",
        "Cedillasmall",
        "Chi",
        "Circumflexsmall",
        "Csmall",
        "D",
        "Dcaron",
        "Dcroat",
        "Delta",
        "Delta",
        "Dieresis",
        "DieresisAcute",
        "DieresisGrave",
        "Dieresissmall",
        "Dotaccentsmall",
        "Dsmall",
        "E",
        "Eacute",
        "Eacutesmall",
        "Ebreve",
        "Ecaron",
        "Ecircumflex",
        "Ecircumflexsmall",
        "Edieresis",
        "Edieresissmall",
        "Edotaccent",
        "Egrave",
        "Egravesmall",
        "Emacron",
        "Eng",
        "Eogonek",
        "Epsilon",
        "Epsilontonos",
        "Esmall",
        "Eta",
        "Etatonos",
        "Eth",
        "Ethsmall",
        "Euro",
        "F",
        "Fsmall",
        "G",
        "Gamma",
        "Gbreve",
        "Gcaron",
        "Gcircumflex",
        "Gcommaaccent",
        "Gdotaccent",
        "Grave",
        "Gravesmall",
        "Gsmall",
        "H",
        "H18533",
        "H18543",
        "H18551",
        "H22073",
        "Hbar",
        "Hcircumflex",
        "Hsmall",
        "Hungarumlaut",
        "Hungarumlautsmall",
        "I",
        "IJ",
        "Iacute",
        "Iacutesmall",
        "Ibreve",
        "Icircumflex",
        "Icircumflexsmall",
        "Idieresis",
        "Idieresissmall",
        "Idotaccent",
        "Ifraktur",
        "Igrave",
        "Igravesmall",
        "Imacron",
        "Iogonek",
        "Iota",
        "Iotadieresis",
        "Iotatonos",
        "Ismall",
        "Itilde",
        "J",
        "Jcircumflex",
        "Jsmall",
        "K",
        "Kappa",
        "Kcommaaccent",
        "Ksmall",
        "L",
        "LL",
        "Lacute",
        "Lambda",
        "Lcaron",
        "Lcommaaccent",
        "Ldot",
        "Lslash",
        "Lslashsmall",
        "Lsmall",
        "M",
        "Macron",
        "Macronsmall",
        "Msmall",
        "Mu",
        "N",
        "Nacute",
        "Ncaron",
        "Ncommaaccent",
        "Nsmall",
        "Ntilde",
        "Ntildesmall",
        "Nu",
        "O",
        "OE",
        "OEsmall",
        "Oacute",
        "Oacutesmall",
        "Obreve",
        "Ocircumflex",
        "Ocircumflexsmall",
        "Odieresis",
        "Odieresissmall",
        "Ogoneksmall",
        "Ograve",
        "Ogravesmall",
        "Ohorn",
        "Ohungarumlaut",
        "Omacron",
        "Omega",
        "Omega",
        "Omegatonos",
        "Omicron",
        "Omicrontonos",
        "Oslash",
        "Oslashacute",
        "Oslashsmall",
        "Osmall",
        "Otilde",
        "Otildesmall",
        "P",
        "Phi",
        "Pi",
        "Psi",
        "Psmall",
        "Q",
        "Qsmall",
        "R",
        "Racute",
        "Rcaron",
        "Rcommaaccent",
        "Rfraktur",
        "Rho",
        "Ringsmall",
        "Rsmall",
        "S",
        "SF010000",
        "SF020000",
        "SF030000",
        "SF040000",
        "SF050000",
        "SF060000",
        "SF070000",
        "SF080000",
        "SF090000",
        "SF100000",
        "SF110000",
        "SF190000",
        "SF200000",
        "SF210000",
        "SF220000",
        "SF230000",
        "SF240000",
        "SF250000",
        "SF260000",
        "SF270000",
        "SF280000",
        "SF360000",
        "SF370000",
        "SF380000",
        "SF390000",
        "SF400000",
        "SF410000",
        "SF420000",
        "SF430000",
        "SF440000",
        "SF450000",
        "SF460000",
        "SF470000",
        "SF480000",
        "SF490000",
        "SF500000",
        "SF510000",
        "SF520000",
        "SF530000",
        "SF540000",
        "Sacute",
        "Scaron",
        "Scaronsmall",
        "Scedilla",
        "Scedilla",
        "Scircumflex",
        "Scommaaccent",
        "Sigma",
        "Ssmall",
        "T",
        "Tau",
        "Tbar",
        "Tcaron",
        "Tcommaaccent",
        "Tcommaaccent",
        "Theta",
        "Thorn",
        "Thornsmall",
        "Tildesmall",
        "Tsmall",
        "U",
        "Uacute",
        "Uacutesmall",
        "Ubreve",
        "Ucircumflex",
        "Ucircumflexsmall",
        "Udieresis",
        "Udieresissmall",
        "Ugrave",
        "Ugravesmall",
        "Uhorn",
        "Uhungarumlaut",
        "Umacron",
        "Uogonek",
        "Upsilon",
        "Upsilon1",
        "Upsilondieresis",
        "Upsilontonos",
        "Uring",
        "Usmall",
        "Utilde",
        "V",
        "Vsmall",
        "W",
        "Wacute",
        "Wcircumflex",
        "Wdieresis",
        "Wgrave",
        "Wsmall",
        "X",
        "Xi",
        "Xsmall",
        "Y",
        "Yacute",
        "Yacutesmall",
        "Ycircumflex",
        "Ydieresis",
        "Ydieresissmall",
        "Ygrave",
        "Ysmall",
        "Z",
        "Zacute",
        "Zcaron",
        "Zcaronsmall",
        "Zdotaccent",
        "Zeta",
        "Zsmall",
        "a",
        "aacute",
        "abreve",
        "acircumflex",
        "acute",
        "acutecomb",
        "adieresis",
        "ae",
        "aeacute",
        "afii00208",
        "afii10017",
        "afii10018",
        "afii10019",
        "afii10020",
        "afii10021",
        "afii10022",
        "afii10023",
        "afii10024",
        "afii10025",
        "afii10026",
        "afii10027",
        "afii10028",
        "afii10029",
        "afii10030",
        "afii10031",
        "afii10032",
        "afii10033",
        "afii10034",
        "afii10035",
        "afii10036",
        "afii10037",
        "afii10038",
        "afii10039",
        "afii10040",
        "afii10041",
        "afii10042",
        "afii10043",
        "afii10044",
        "afii10045",
        "afii10046",
        "afii10047",
        "afii10048",
        "afii10049",
        "afii10050",
        "afii10051",
        "afii10052",
        "afii10053",
        "afii10054",
        "afii10055",
        "afii10056",
        "afii10057",
        "afii10058",
        "afii10059",
        "afii10060",
        "afii10061",
        "afii10062",
        "afii10063",
        "afii10064",
        "afii10065",
        "afii10066",
        "afii10067",
        "afii10068",
        "afii10069",
        "afii10070",
        "afii10071",
        "afii10072",
        "afii10073",
        "afii10074",
        "afii10075",
        "afii10076",
        "afii10077",
        "afii10078",
        "afii10079",
        "afii10080",
        "afii10081",
        "afii10082",
        "afii10083",
        "afii10084",
        "afii10085",
        "afii10086",
        "afii10087",
        "afii10088",
        "afii10089",
        "afii10090",
        "afii10091",
        "afii10092",
        "afii10093",
        "afii10094",
        "afii10095",
        "afii10096",
        "afii10097",
        "afii10098",
        "afii10099",
        "afii10100",
        "afii10101",
        "afii10102",
        "afii10103",
        "afii10104",
        "afii10105",
        "afii10106",
        "afii10107",
        "afii10108",
        "afii10109",
        "afii10110",
        "afii10145",
        "afii10146",
        "afii10147",
        "afii10148",
        "afii10192",
        "afii10193",
        "afii10194",
        "afii10195",
        "afii10196",
        "afii10831",
        "afii10832",
        "afii10846",
        "afii299",
        "afii300",
        "afii301",
        "afii57381",
        "afii57388",
        "afii57392",
        "afii57393",
        "afii57394",
        "afii57395",
        "afii57396",
        "afii57397",
        "afii57398",
        "afii57399",
        "afii57400",
        "afii57401",
        "afii57403",
        "afii57407",
        "afii57409",
        "afii57410",
        "afii57411",
        "afii57412",
        "afii57413",
        "afii57414",
        "afii57415",
        "afii57416",
        "afii57417",
        "afii57418",
        "afii57419",
        "afii57420",
        "afii57421",
        "afii57422",
        "afii57423",
        "afii57424",
        "afii57425",
        "afii57426",
        "afii57427",
        "afii57428",
        "afii57429",
        "afii57430",
        "afii57431",
        "afii57432",
        "afii57433",
        "afii57434",
        "afii57440",
        "afii57441",
        "afii57442",
        "afii57443",
        "afii57444",
        "afii57445",
        "afii57446",
        "afii57448",
        "afii57449",
        "afii57450",
        "afii57451",
        "afii57452",
        "afii57453",
        "afii57454",
        "afii57455",
        "afii57456",
        "afii57457",
        "afii57458",
        "afii57470",
        "afii57505",
        "afii57506",
        "afii57507",
        "afii57508",
        "afii57509",
        "afii57511",
        "afii57512",
        "afii57513",
        "afii57514",
        "afii57519",
        "afii57534",
        "afii57636",
        "afii57645",
        "afii57658",
        "afii57664",
        "afii57665",
        "afii57666",
        "afii57667",
        "afii57668",
        "afii57669",
        "afii57670",
        "afii57671",
        "afii57672",
        "afii57673",
        "afii57674",
        "afii57675",
        "afii57676",
        "afii57677",
        "afii57678",
        "afii57679",
        "afii57680",
        "afii57681",
        "afii57682",
        "afii57683",
        "afii57684",
        "afii57685",
        "afii57686",
        "afii57687",
        "afii57688",
        "afii57689",
        "afii57690",
        "afii57694",
        "afii57695",
        "afii57700",
        "afii57705",
        "afii57716",
        "afii57717",
        "afii57718",
        "afii57723",
        "afii57793",
        "afii57794",
        "afii57795",
        "afii57796",
        "afii57797",
        "afii57798",
        "afii57799",
        "afii57800",
        "afii57801",
        "afii57802",
        "afii57803",
        "afii57804",
        "afii57806",
        "afii57807",
        "afii57839",
        "afii57841",
        "afii57842",
        "afii57929",
        "afii61248",
        "afii61289",
        "afii61352",
        "afii61573",
        "afii61574",
        "afii61575",
        "afii61664",
        "afii63167",
        "afii64937",
        "agrave",
        "aleph",
        "alpha",
        "alphatonos",
        "amacron",
        "ampersand",
        "ampersandsmall",
        "angle",
        "angleleft",
        "angleright",
        "anoteleia",
        "aogonek",
        "approxequal",
        "aring",
        "aringacute",
        "arrowboth",
        "arrowdblboth",
        "arrowdbldown",
        "arrowdblleft",
        "arrowdblright",
        "arrowdblup",
        "arrowdown",
        "arrowhorizex",
        "arrowleft",
        "arrowright",
        "arrowup",
        "arrowupdn",
        "arrowupdnbse",
        "arrowvertex",
        "asciicircum",
        "asciitilde",
        "asterisk",
        "asteriskmath",
        "asuperior",
        "at",
        "atilde",
        "b",
        "backslash",
        "bar",
        "beta",
        "block",
        "braceex",
        "braceleft",
        "braceleftbt",
        "braceleftmid",
        "bracelefttp",
        "braceright",
        "bracerightbt",
        "bracerightmid",
        "bracerighttp",
        "bracketleft",
        "bracketleftbt",
        "bracketleftex",
        "bracketlefttp",
        "bracketright",
        "bracketrightbt",
        "bracketrightex",
        "bracketrighttp",
        "breve",
        "brokenbar",
        "bsuperior",
        "bullet",
        "c",
        "cacute",
        "caron",
        "carriagereturn",
        "ccaron",
        "ccedilla",
        "ccircumflex",
        "cdotaccent",
        "cedilla",
        "cent",
        "centinferior",
        "centoldstyle",
        "centsuperior",
        "chi",
        "circle",
        "circlemultiply",
        "circleplus",
        "circumflex",
        "club",
        "colon",
        "colonmonetary",
        "comma",
        "commaaccent",
        "commainferior",
        "commasuperior",
        "congruent",
        "copyright",
        "copyrightsans",
        "copyrightserif",
        "currency",
        "cyrBreve",
        "cyrFlex",
        "cyrbreve",
        "cyrflex",
        "d",
        "dagger",
        "daggerdbl",
        "dblGrave",
        "dblgrave",
        "dcaron",
        "dcroat",
        "degree",
        "delta",
        "diamond",
        "dieresis",
        "dieresisacute",
        "dieresisgrave",
        "dieresistonos",
        "divide",
        "dkshade",
        "dnblock",
        "dollar",
        "dollarinferior",
        "dollaroldstyle",
        "dollarsuperior",
        "dong",
        "dotaccent",
        "dotbelowcomb",
        "dotlessi",
        "dotlessj",
        "dotmath",
        "dsuperior",
        "e",
        "eacute",
        "ebreve",
        "ecaron",
        "ecircumflex",
        "edieresis",
        "edotaccent",
        "egrave",
        "eight",
        "eightinferior",
        "eightoldstyle",
        "eightsuperior",
        "element",
        "ellipsis",
        "emacron",
        "emdash",
        "emptyset",
        "endash",
        "eng",
        "eogonek",
        "epsilon",
        "epsilontonos",
        "equal",
        "equivalence",
        "estimated",
        "esuperior",
        "eta",
        "etatonos",
        "eth",
        "exclam",
        "exclamdbl",
        "exclamdown",
        "exclamdownsmall",
        "exclamsmall",
        "existential",
        "f",
        "female",
        "ff",
        "ffi",
        "ffl",
        "fi",
        "figuredash",
        "filledbox",
        "filledrect",
        "five",
        "fiveeighths",
        "fiveinferior",
        "fiveoldstyle",
        "fivesuperior",
        "fl",
        "florin",
        "four",
        "fourinferior",
        "fouroldstyle",
        "foursuperior",
        "fraction",
        "fraction",
        "franc",
        "g",
        "gamma",
        "gbreve",
        "gcaron",
        "gcircumflex",
        "gcommaaccent",
        "gdotaccent",
        "germandbls",
        "gradient",
        "grave",
        "gravecomb",
        "greater",
        "greaterequal",
        "guillemotleft",
        "guillemotright",
        "guilsinglleft",
        "guilsinglright",
        "h",
        "hbar",
        "hcircumflex",
        "heart",
        "hookabovecomb",
        "house",
        "hungarumlaut",
        "hyphen",
        "hyphen",
        "hypheninferior",
        "hyphensuperior",
        "i",
        "iacute",
        "ibreve",
        "icircumflex",
        "idieresis",
        "igrave",
        "ij",
        "imacron",
        "infinity",
        "integral",
        "integralbt",
        "integralex",
        "integraltp",
        "intersection",
        "invbullet",
        "invcircle",
        "invsmileface",
        "iogonek",
        "iota",
        "iotadieresis",
        "iotadieresistonos",
        "iotatonos",
        "isuperior",
        "itilde",
        "j",
        "jcircumflex",
        "k",
        "kappa",
        "kcommaaccent",
        "kgreenlandic",
        "l",
        "lacute",
        "lambda",
        "lcaron",
        "lcommaaccent",
        "ldot",
        "less",
        "lessequal",
        "lfblock",
        "lira",
        "ll",
        "logicaland",
        "logicalnot",
        "logicalor",
        "longs",
        "lozenge",
        "lslash",
        "lsuperior",
        "ltshade",
        "m",
        "macron",
        "macron",
        "male",
        "minus",
        "minute",
        "msuperior",
        "mu",
        "mu",
        "multiply",
        "musicalnote",
        "musicalnotedbl",
        "n",
        "nacute",
        "napostrophe",
        "ncaron",
        "ncommaaccent",
        "nine",
        "nineinferior",
        "nineoldstyle",
        "ninesuperior",
        "notelement",
        "notequal",
        "notsubset",
        "nsuperior",
        "ntilde",
        "nu",
        "numbersign",
        "o",
        "oacute",
        "obreve",
        "ocircumflex",
        "odieresis",
        "oe",
        "ogonek",
        "ograve",
        "ohorn",
        "ohungarumlaut",
        "omacron",
        "omega",
        "omega1",
        "omegatonos",
        "omicron",
        "omicrontonos",
        "one",
        "onedotenleader",
        "oneeighth",
        "onefitted",
        "onehalf",
        "oneinferior",
        "oneoldstyle",
        "onequarter",
        "onesuperior",
        "onethird",
        "openbullet",
        "ordfeminine",
        "ordmasculine",
        "orthogonal",
        "oslash",
        "oslashacute",
        "osuperior",
        "otilde",
        "p",
        "paragraph",
        "parenleft",
        "parenleftbt",
        "parenleftex",
        "parenleftinferior",
        "parenleftsuperior",
        "parenlefttp",
        "parenright",
        "parenrightbt",
        "parenrightex",
        "parenrightinferior",
        "parenrightsuperior",
        "parenrighttp",
        "partialdiff",
        "percent",
        "period",
        "periodcentered",
        "periodcentered",
        "periodinferior",
        "periodsuperior",
        "perpendicular",
        "perthousand",
        "peseta",
        "phi",
        "phi1",
        "pi",
        "plus",
        "plusminus",
        "prescription",
        "product",
        "propersubset",
        "propersuperset",
        "proportional",
        "psi",
        "q",
        "question",
        "questiondown",
        "questiondownsmall",
        "questionsmall",
        "quotedbl",
        "quotedblbase",
        "quotedblleft",
        "quotedblright",
        "quoteleft",
        "quotereversed",
        "quoteright",
        "quotesinglbase",
        "quotesingle",
        "r",
        "racute",
        "radical",
        "radicalex",
        "rcaron",
        "rcommaaccent",
        "reflexsubset",
        "reflexsuperset",
        "registered",
        "registersans",
        "registerserif",
        "revlogicalnot",
        "rho",
        "ring",
        "rsuperior",
        "rtblock",
        "rupiah",
        "s",
        "sacute",
        "scaron",
        "scedilla",
        "scedilla",
        "scircumflex",
        "scommaaccent",
        "second",
        "section",
        "semicolon",
        "seven",
        "seveneighths",
        "seveninferior",
        "sevenoldstyle",
        "sevensuperior",
        "shade",
        "sigma",
        "sigma1",
        "similar",
        "six",
        "sixinferior",
        "sixoldstyle",
        "sixsuperior",
        "slash",
        "smileface",
        "space",
        "space",
        "spade",
        "ssuperior",
        "sterling",
        "suchthat",
        "summation",
        "sun",
        "t",
        "tau",
        "tbar",
        "tcaron",
        "tcommaaccent",
        "tcommaaccent",
        "therefore",
        "theta",
        "theta1",
        "thorn",
        "three",
        "threeeighths",
        "threeinferior",
        "threeoldstyle",
        "threequarters",
        "threequartersemdash",
        "threesuperior",
        "tilde",
        "tildecomb",
        "tonos",
        "trademark",
        "trademarksans",
        "trademarkserif",
        "triagdn",
        "triaglf",
        "triagrt",
        "triagup",
        "tsuperior",
        "two",
        "twodotenleader",
        "twoinferior",
        "twooldstyle",
        "twosuperior",
        "twothirds",
        "u",
        "uacute",
        "ubreve",
        "ucircumflex",
        "udieresis",
        "ugrave",
        "uhorn",
        "uhungarumlaut",
        "umacron",
        "underscore",
        "underscoredbl",
        "union",
        "universal",
        "uogonek",
        "upblock",
        "upsilon",
        "upsilondieresis",
        "upsilondieresistonos",
        "upsilontonos",
        "uring",
        "utilde",
        "v",
        "w",
        "wacute",
        "wcircumflex",
        "wdieresis",
        "weierstrass",
        "wgrave",
        "x",
        "xi",
        "y",
        "yacute",
        "ycircumflex",
        "ydieresis",
        "yen",
        "ygrave",
        "z",
        "zacute",
        "zcaron",
        "zdotaccent",
        "zero",
        "zeroinferior",
        "zerooldstyle",
        "zerosuperior",
        "zeta"
      };
      PdfFont.m_arrGlyphCodes = new ushort[1051]
      {
        (ushort) 65,
        (ushort) 198,
        (ushort) 508,
        (ushort) 63462,
        (ushort) 193,
        (ushort) 63457,
        (ushort) 258,
        (ushort) 194,
        (ushort) 63458,
        (ushort) 63177,
        (ushort) 63412,
        (ushort) 196,
        (ushort) 63460,
        (ushort) 192,
        (ushort) 63456,
        (ushort) 913,
        (ushort) 902,
        (ushort) 256,
        (ushort) 260,
        (ushort) 197,
        (ushort) 506,
        (ushort) 63461,
        (ushort) 63329,
        (ushort) 195,
        (ushort) 63459,
        (ushort) 66,
        (ushort) 914,
        (ushort) 63220,
        (ushort) 63330,
        (ushort) 67,
        (ushort) 262,
        (ushort) 63178,
        (ushort) 63221,
        (ushort) 268,
        (ushort) 199,
        (ushort) 63463,
        (ushort) 264,
        (ushort) 266,
        (ushort) 63416,
        (ushort) 935,
        (ushort) 63222,
        (ushort) 63331,
        (ushort) 68,
        (ushort) 270,
        (ushort) 272,
        (ushort) 8710,
        (ushort) 916,
        (ushort) 63179,
        (ushort) 63180,
        (ushort) 63181,
        (ushort) 63400,
        (ushort) 63223,
        (ushort) 63332,
        (ushort) 69,
        (ushort) 201,
        (ushort) 63465,
        (ushort) 276,
        (ushort) 282,
        (ushort) 202,
        (ushort) 63466,
        (ushort) 203,
        (ushort) 63467,
        (ushort) 278,
        (ushort) 200,
        (ushort) 63464,
        (ushort) 274,
        (ushort) 330,
        (ushort) 280,
        (ushort) 917,
        (ushort) 904,
        (ushort) 63333,
        (ushort) 919,
        (ushort) 905,
        (ushort) 208,
        (ushort) 63472,
        (ushort) 8364,
        (ushort) 70,
        (ushort) 63334,
        (ushort) 71,
        (ushort) 915,
        (ushort) 286,
        (ushort) 486,
        (ushort) 284,
        (ushort) 290,
        (ushort) 288,
        (ushort) 63182,
        (ushort) 63328,
        (ushort) 63335,
        (ushort) 72,
        (ushort) 9679,
        (ushort) 9642,
        (ushort) 9643,
        (ushort) 9633,
        (ushort) 294,
        (ushort) 292,
        (ushort) 63336,
        (ushort) 63183,
        (ushort) 63224,
        (ushort) 73,
        (ushort) 306,
        (ushort) 205,
        (ushort) 63469,
        (ushort) 300,
        (ushort) 206,
        (ushort) 63470,
        (ushort) 207,
        (ushort) 63471,
        (ushort) 304,
        (ushort) 8465,
        (ushort) 204,
        (ushort) 63468,
        (ushort) 298,
        (ushort) 302,
        (ushort) 921,
        (ushort) 938,
        (ushort) 906,
        (ushort) 63337,
        (ushort) 296,
        (ushort) 74,
        (ushort) 308,
        (ushort) 63338,
        (ushort) 75,
        (ushort) 922,
        (ushort) 310,
        (ushort) 63339,
        (ushort) 76,
        (ushort) 63167,
        (ushort) 313,
        (ushort) 923,
        (ushort) 317,
        (ushort) 315,
        (ushort) 319,
        (ushort) 321,
        (ushort) 63225,
        (ushort) 63340,
        (ushort) 77,
        (ushort) 63184,
        (ushort) 63407,
        (ushort) 63341,
        (ushort) 924,
        (ushort) 78,
        (ushort) 323,
        (ushort) 327,
        (ushort) 325,
        (ushort) 63342,
        (ushort) 209,
        (ushort) 63473,
        (ushort) 925,
        (ushort) 79,
        (ushort) 338,
        (ushort) 63226,
        (ushort) 211,
        (ushort) 63475,
        (ushort) 334,
        (ushort) 212,
        (ushort) 63476,
        (ushort) 214,
        (ushort) 63478,
        (ushort) 63227,
        (ushort) 210,
        (ushort) 63474,
        (ushort) 416,
        (ushort) 336,
        (ushort) 332,
        (ushort) 8486,
        (ushort) 937,
        (ushort) 911,
        (ushort) 927,
        (ushort) 908,
        (ushort) 216,
        (ushort) 510,
        (ushort) 63480,
        (ushort) 63343,
        (ushort) 213,
        (ushort) 63477,
        (ushort) 80,
        (ushort) 934,
        (ushort) 928,
        (ushort) 936,
        (ushort) 63344,
        (ushort) 81,
        (ushort) 63345,
        (ushort) 82,
        (ushort) 340,
        (ushort) 344,
        (ushort) 342,
        (ushort) 8476,
        (ushort) 929,
        (ushort) 63228,
        (ushort) 63346,
        (ushort) 83,
        (ushort) 9484,
        (ushort) 9492,
        (ushort) 9488,
        (ushort) 9496,
        (ushort) 9532,
        (ushort) 9516,
        (ushort) 9524,
        (ushort) 9500,
        (ushort) 9508,
        (ushort) 9472,
        (ushort) 9474,
        (ushort) 9569,
        (ushort) 9570,
        (ushort) 9558,
        (ushort) 9557,
        (ushort) 9571,
        (ushort) 9553,
        (ushort) 9559,
        (ushort) 9565,
        (ushort) 9564,
        (ushort) 9563,
        (ushort) 9566,
        (ushort) 9567,
        (ushort) 9562,
        (ushort) 9556,
        (ushort) 9577,
        (ushort) 9574,
        (ushort) 9568,
        (ushort) 9552,
        (ushort) 9580,
        (ushort) 9575,
        (ushort) 9576,
        (ushort) 9572,
        (ushort) 9573,
        (ushort) 9561,
        (ushort) 9560,
        (ushort) 9554,
        (ushort) 9555,
        (ushort) 9579,
        (ushort) 9578,
        (ushort) 346,
        (ushort) 352,
        (ushort) 63229,
        (ushort) 350,
        (ushort) 63169,
        (ushort) 348,
        (ushort) 536,
        (ushort) 931,
        (ushort) 63347,
        (ushort) 84,
        (ushort) 932,
        (ushort) 358,
        (ushort) 356,
        (ushort) 354,
        (ushort) 538,
        (ushort) 920,
        (ushort) 222,
        (ushort) 63486,
        (ushort) 63230,
        (ushort) 63348,
        (ushort) 85,
        (ushort) 218,
        (ushort) 63482,
        (ushort) 364,
        (ushort) 219,
        (ushort) 63483,
        (ushort) 220,
        (ushort) 63484,
        (ushort) 217,
        (ushort) 63481,
        (ushort) 431,
        (ushort) 368,
        (ushort) 362,
        (ushort) 370,
        (ushort) 933,
        (ushort) 978,
        (ushort) 939,
        (ushort) 910,
        (ushort) 366,
        (ushort) 63349,
        (ushort) 360,
        (ushort) 86,
        (ushort) 63350,
        (ushort) 87,
        (ushort) 7810,
        (ushort) 372,
        (ushort) 7812,
        (ushort) 7808,
        (ushort) 63351,
        (ushort) 88,
        (ushort) 926,
        (ushort) 63352,
        (ushort) 89,
        (ushort) 221,
        (ushort) 63485,
        (ushort) 374,
        (ushort) 376,
        (ushort) 63487,
        (ushort) 7922,
        (ushort) 63353,
        (ushort) 90,
        (ushort) 377,
        (ushort) 381,
        (ushort) 63231,
        (ushort) 379,
        (ushort) 918,
        (ushort) 63354,
        (ushort) 97,
        (ushort) 225,
        (ushort) 259,
        (ushort) 226,
        (ushort) 180,
        (ushort) 769,
        (ushort) 228,
        (ushort) 230,
        (ushort) 509,
        (ushort) 8213,
        (ushort) 1040,
        (ushort) 1041,
        (ushort) 1042,
        (ushort) 1043,
        (ushort) 1044,
        (ushort) 1045,
        (ushort) 1025,
        (ushort) 1046,
        (ushort) 1047,
        (ushort) 1048,
        (ushort) 1049,
        (ushort) 1050,
        (ushort) 1051,
        (ushort) 1052,
        (ushort) 1053,
        (ushort) 1054,
        (ushort) 1055,
        (ushort) 1056,
        (ushort) 1057,
        (ushort) 1058,
        (ushort) 1059,
        (ushort) 1060,
        (ushort) 1061,
        (ushort) 1062,
        (ushort) 1063,
        (ushort) 1064,
        (ushort) 1065,
        (ushort) 1066,
        (ushort) 1067,
        (ushort) 1068,
        (ushort) 1069,
        (ushort) 1070,
        (ushort) 1071,
        (ushort) 1168,
        (ushort) 1026,
        (ushort) 1027,
        (ushort) 1028,
        (ushort) 1029,
        (ushort) 1030,
        (ushort) 1031,
        (ushort) 1032,
        (ushort) 1033,
        (ushort) 1034,
        (ushort) 1035,
        (ushort) 1036,
        (ushort) 1038,
        (ushort) 63172,
        (ushort) 63173,
        (ushort) 1072,
        (ushort) 1073,
        (ushort) 1074,
        (ushort) 1075,
        (ushort) 1076,
        (ushort) 1077,
        (ushort) 1105,
        (ushort) 1078,
        (ushort) 1079,
        (ushort) 1080,
        (ushort) 1081,
        (ushort) 1082,
        (ushort) 1083,
        (ushort) 1084,
        (ushort) 1085,
        (ushort) 1086,
        (ushort) 1087,
        (ushort) 1088,
        (ushort) 1089,
        (ushort) 1090,
        (ushort) 1091,
        (ushort) 1092,
        (ushort) 1093,
        (ushort) 1094,
        (ushort) 1095,
        (ushort) 1096,
        (ushort) 1097,
        (ushort) 1098,
        (ushort) 1099,
        (ushort) 1100,
        (ushort) 1101,
        (ushort) 1102,
        (ushort) 1103,
        (ushort) 1169,
        (ushort) 1106,
        (ushort) 1107,
        (ushort) 1108,
        (ushort) 1109,
        (ushort) 1110,
        (ushort) 1111,
        (ushort) 1112,
        (ushort) 1113,
        (ushort) 1114,
        (ushort) 1115,
        (ushort) 1116,
        (ushort) 1118,
        (ushort) 1039,
        (ushort) 1122,
        (ushort) 1138,
        (ushort) 1140,
        (ushort) 63174,
        (ushort) 1119,
        (ushort) 1123,
        (ushort) 1139,
        (ushort) 1141,
        (ushort) 63175,
        (ushort) 63176,
        (ushort) 1241,
        (ushort) 8206,
        (ushort) 8207,
        (ushort) 8205,
        (ushort) 1642,
        (ushort) 1548,
        (ushort) 1632,
        (ushort) 1633,
        (ushort) 1634,
        (ushort) 1635,
        (ushort) 1636,
        (ushort) 1637,
        (ushort) 1638,
        (ushort) 1639,
        (ushort) 1640,
        (ushort) 1641,
        (ushort) 1563,
        (ushort) 1567,
        (ushort) 1569,
        (ushort) 1570,
        (ushort) 1571,
        (ushort) 1572,
        (ushort) 1573,
        (ushort) 1574,
        (ushort) 1575,
        (ushort) 1576,
        (ushort) 1577,
        (ushort) 1578,
        (ushort) 1579,
        (ushort) 1580,
        (ushort) 1581,
        (ushort) 1582,
        (ushort) 1583,
        (ushort) 1584,
        (ushort) 1585,
        (ushort) 1586,
        (ushort) 1587,
        (ushort) 1588,
        (ushort) 1589,
        (ushort) 1590,
        (ushort) 1591,
        (ushort) 1592,
        (ushort) 1593,
        (ushort) 1594,
        (ushort) 1600,
        (ushort) 1601,
        (ushort) 1602,
        (ushort) 1603,
        (ushort) 1604,
        (ushort) 1605,
        (ushort) 1606,
        (ushort) 1608,
        (ushort) 1609,
        (ushort) 1610,
        (ushort) 1611,
        (ushort) 1612,
        (ushort) 1613,
        (ushort) 1614,
        (ushort) 1615,
        (ushort) 1616,
        (ushort) 1617,
        (ushort) 1618,
        (ushort) 1607,
        (ushort) 1700,
        (ushort) 1662,
        (ushort) 1670,
        (ushort) 1688,
        (ushort) 1711,
        (ushort) 1657,
        (ushort) 1672,
        (ushort) 1681,
        (ushort) 1722,
        (ushort) 1746,
        (ushort) 1749,
        (ushort) 8362,
        (ushort) 1470,
        (ushort) 1475,
        (ushort) 1488,
        (ushort) 1489,
        (ushort) 1490,
        (ushort) 1491,
        (ushort) 1492,
        (ushort) 1493,
        (ushort) 1494,
        (ushort) 1495,
        (ushort) 1496,
        (ushort) 1497,
        (ushort) 1498,
        (ushort) 1499,
        (ushort) 1500,
        (ushort) 1501,
        (ushort) 1502,
        (ushort) 1503,
        (ushort) 1504,
        (ushort) 1505,
        (ushort) 1506,
        (ushort) 1507,
        (ushort) 1508,
        (ushort) 1509,
        (ushort) 1510,
        (ushort) 1511,
        (ushort) 1512,
        (ushort) 1513,
        (ushort) 1514,
        (ushort) 64298,
        (ushort) 64299,
        (ushort) 64331,
        (ushort) 64287,
        (ushort) 1520,
        (ushort) 1521,
        (ushort) 1522,
        (ushort) 64309,
        (ushort) 1460,
        (ushort) 1461,
        (ushort) 1462,
        (ushort) 1467,
        (ushort) 1464,
        (ushort) 1463,
        (ushort) 1456,
        (ushort) 1458,
        (ushort) 1457,
        (ushort) 1459,
        (ushort) 1474,
        (ushort) 1473,
        (ushort) 1465,
        (ushort) 1468,
        (ushort) 1469,
        (ushort) 1471,
        (ushort) 1472,
        (ushort) 700,
        (ushort) 8453,
        (ushort) 8467,
        (ushort) 8470,
        (ushort) 8236,
        (ushort) 8237,
        (ushort) 8238,
        (ushort) 8204,
        (ushort) 1645,
        (ushort) 701,
        (ushort) 224,
        (ushort) 8501,
        (ushort) 945,
        (ushort) 940,
        (ushort) 257,
        (ushort) 38,
        (ushort) 63270,
        (ushort) 8736,
        (ushort) 9001,
        (ushort) 9002,
        (ushort) 903,
        (ushort) 261,
        (ushort) 8776,
        (ushort) 229,
        (ushort) 507,
        (ushort) 8596,
        (ushort) 8660,
        (ushort) 8659,
        (ushort) 8656,
        (ushort) 8658,
        (ushort) 8657,
        (ushort) 8595,
        (ushort) 63719,
        (ushort) 8592,
        (ushort) 8594,
        (ushort) 8593,
        (ushort) 8597,
        (ushort) 8616,
        (ushort) 63718,
        (ushort) 94,
        (ushort) 126,
        (ushort) 42,
        (ushort) 8727,
        (ushort) 63209,
        (ushort) 64,
        (ushort) 227,
        (ushort) 98,
        (ushort) 92,
        (ushort) 124,
        (ushort) 946,
        (ushort) 9608,
        (ushort) 63732,
        (ushort) 123,
        (ushort) 63731,
        (ushort) 63730,
        (ushort) 63729,
        (ushort) 125,
        (ushort) 63742,
        (ushort) 63741,
        (ushort) 63740,
        (ushort) 91,
        (ushort) 63728,
        (ushort) 63727,
        (ushort) 63726,
        (ushort) 93,
        (ushort) 63739,
        (ushort) 63738,
        (ushort) 63737,
        (ushort) 728,
        (ushort) 166,
        (ushort) 63210,
        (ushort) 8226,
        (ushort) 99,
        (ushort) 263,
        (ushort) 711,
        (ushort) 8629,
        (ushort) 269,
        (ushort) 231,
        (ushort) 265,
        (ushort) 267,
        (ushort) 184,
        (ushort) 162,
        (ushort) 63199,
        (ushort) 63394,
        (ushort) 63200,
        (ushort) 967,
        (ushort) 9675,
        (ushort) 8855,
        (ushort) 8853,
        (ushort) 710,
        (ushort) 9827,
        (ushort) 58,
        (ushort) 8353,
        (ushort) 44,
        (ushort) 63171,
        (ushort) 63201,
        (ushort) 63202,
        (ushort) 8773,
        (ushort) 169,
        (ushort) 63721,
        (ushort) 63193,
        (ushort) 164,
        (ushort) 63185,
        (ushort) 63186,
        (ushort) 63188,
        (ushort) 63189,
        (ushort) 100,
        (ushort) 8224,
        (ushort) 8225,
        (ushort) 63187,
        (ushort) 63190,
        (ushort) 271,
        (ushort) 273,
        (ushort) 176,
        (ushort) 948,
        (ushort) 9830,
        (ushort) 168,
        (ushort) 63191,
        (ushort) 63192,
        (ushort) 901,
        (ushort) 247,
        (ushort) 9619,
        (ushort) 9604,
        (ushort) 36,
        (ushort) 63203,
        (ushort) 63268,
        (ushort) 63204,
        (ushort) 8363,
        (ushort) 729,
        (ushort) 803,
        (ushort) 305,
        (ushort) 63166,
        (ushort) 8901,
        (ushort) 63211,
        (ushort) 101,
        (ushort) 233,
        (ushort) 277,
        (ushort) 283,
        (ushort) 234,
        (ushort) 235,
        (ushort) 279,
        (ushort) 232,
        (ushort) 56,
        (ushort) 8328,
        (ushort) 63288,
        (ushort) 8312,
        (ushort) 8712,
        (ushort) 8230,
        (ushort) 275,
        (ushort) 8212,
        (ushort) 8709,
        (ushort) 8211,
        (ushort) 331,
        (ushort) 281,
        (ushort) 949,
        (ushort) 941,
        (ushort) 61,
        (ushort) 8801,
        (ushort) 8494,
        (ushort) 63212,
        (ushort) 951,
        (ushort) 942,
        (ushort) 240,
        (ushort) 33,
        (ushort) 8252,
        (ushort) 161,
        (ushort) 63393,
        (ushort) 63265,
        (ushort) 8707,
        (ushort) 102,
        (ushort) 9792,
        (ushort) 64256,
        (ushort) 64259,
        (ushort) 64260,
        (ushort) 64257,
        (ushort) 8210,
        (ushort) 9632,
        (ushort) 9644,
        (ushort) 53,
        (ushort) 8541,
        (ushort) 8325,
        (ushort) 63285,
        (ushort) 8309,
        (ushort) 64258,
        (ushort) 402,
        (ushort) 52,
        (ushort) 8324,
        (ushort) 63284,
        (ushort) 8308,
        (ushort) 8260,
        (ushort) 8725,
        (ushort) 8355,
        (ushort) 103,
        (ushort) 947,
        (ushort) 287,
        (ushort) 487,
        (ushort) 285,
        (ushort) 291,
        (ushort) 289,
        (ushort) 223,
        (ushort) 8711,
        (ushort) 96,
        (ushort) 768,
        (ushort) 62,
        (ushort) 8805,
        (ushort) 171,
        (ushort) 187,
        (ushort) 8249,
        (ushort) 8250,
        (ushort) 104,
        (ushort) 295,
        (ushort) 293,
        (ushort) 9829,
        (ushort) 777,
        (ushort) 8962,
        (ushort) 733,
        (ushort) 45,
        (ushort) 173,
        (ushort) 63205,
        (ushort) 63206,
        (ushort) 105,
        (ushort) 237,
        (ushort) 301,
        (ushort) 238,
        (ushort) 239,
        (ushort) 236,
        (ushort) 307,
        (ushort) 299,
        (ushort) 8734,
        (ushort) 8747,
        (ushort) 8993,
        (ushort) 63733,
        (ushort) 8992,
        (ushort) 8745,
        (ushort) 9688,
        (ushort) 9689,
        (ushort) 9787,
        (ushort) 303,
        (ushort) 953,
        (ushort) 970,
        (ushort) 912,
        (ushort) 943,
        (ushort) 63213,
        (ushort) 297,
        (ushort) 106,
        (ushort) 309,
        (ushort) 107,
        (ushort) 954,
        (ushort) 311,
        (ushort) 312,
        (ushort) 108,
        (ushort) 314,
        (ushort) 955,
        (ushort) 318,
        (ushort) 316,
        (ushort) 320,
        (ushort) 60,
        (ushort) 8804,
        (ushort) 9612,
        (ushort) 8356,
        (ushort) 63168,
        (ushort) 8743,
        (ushort) 172,
        (ushort) 8744,
        (ushort) 383,
        (ushort) 9674,
        (ushort) 322,
        (ushort) 63214,
        (ushort) 9617,
        (ushort) 109,
        (ushort) 175,
        (ushort) 713,
        (ushort) 9794,
        (ushort) 8722,
        (ushort) 8242,
        (ushort) 63215,
        (ushort) 181,
        (ushort) 956,
        (ushort) 215,
        (ushort) 9834,
        (ushort) 9835,
        (ushort) 110,
        (ushort) 324,
        (ushort) 329,
        (ushort) 328,
        (ushort) 326,
        (ushort) 57,
        (ushort) 8329,
        (ushort) 63289,
        (ushort) 8313,
        (ushort) 8713,
        (ushort) 8800,
        (ushort) 8836,
        (ushort) 8319,
        (ushort) 241,
        (ushort) 957,
        (ushort) 35,
        (ushort) 111,
        (ushort) 243,
        (ushort) 335,
        (ushort) 244,
        (ushort) 246,
        (ushort) 339,
        (ushort) 731,
        (ushort) 242,
        (ushort) 417,
        (ushort) 337,
        (ushort) 333,
        (ushort) 969,
        (ushort) 982,
        (ushort) 974,
        (ushort) 959,
        (ushort) 972,
        (ushort) 49,
        (ushort) 8228,
        (ushort) 8539,
        (ushort) 63196,
        (ushort) 189,
        (ushort) 8321,
        (ushort) 63281,
        (ushort) 188,
        (ushort) 185,
        (ushort) 8531,
        (ushort) 9702,
        (ushort) 170,
        (ushort) 186,
        (ushort) 8735,
        (ushort) 248,
        (ushort) 511,
        (ushort) 63216,
        (ushort) 245,
        (ushort) 112,
        (ushort) 182,
        (ushort) 40,
        (ushort) 63725,
        (ushort) 63724,
        (ushort) 8333,
        (ushort) 8317,
        (ushort) 63723,
        (ushort) 41,
        (ushort) 63736,
        (ushort) 63735,
        (ushort) 8334,
        (ushort) 8318,
        (ushort) 63734,
        (ushort) 8706,
        (ushort) 37,
        (ushort) 46,
        (ushort) 183,
        (ushort) 8729,
        (ushort) 63207,
        (ushort) 63208,
        (ushort) 8869,
        (ushort) 8240,
        (ushort) 8359,
        (ushort) 966,
        (ushort) 981,
        (ushort) 960,
        (ushort) 43,
        (ushort) 177,
        (ushort) 8478,
        (ushort) 8719,
        (ushort) 8834,
        (ushort) 8835,
        (ushort) 8733,
        (ushort) 968,
        (ushort) 113,
        (ushort) 63,
        (ushort) 191,
        (ushort) 63423,
        (ushort) 63295,
        (ushort) 34,
        (ushort) 8222,
        (ushort) 8220,
        (ushort) 8221,
        (ushort) 8216,
        (ushort) 8219,
        (ushort) 8217,
        (ushort) 8218,
        (ushort) 39,
        (ushort) 114,
        (ushort) 341,
        (ushort) 8730,
        (ushort) 63717,
        (ushort) 345,
        (ushort) 343,
        (ushort) 8838,
        (ushort) 8839,
        (ushort) 174,
        (ushort) 63720,
        (ushort) 63194,
        (ushort) 8976,
        (ushort) 961,
        (ushort) 730,
        (ushort) 63217,
        (ushort) 9616,
        (ushort) 63197,
        (ushort) 115,
        (ushort) 347,
        (ushort) 353,
        (ushort) 351,
        (ushort) 63170,
        (ushort) 349,
        (ushort) 537,
        (ushort) 8243,
        (ushort) 167,
        (ushort) 59,
        (ushort) 55,
        (ushort) 8542,
        (ushort) 8327,
        (ushort) 63287,
        (ushort) 8311,
        (ushort) 9618,
        (ushort) 963,
        (ushort) 962,
        (ushort) 8764,
        (ushort) 54,
        (ushort) 8326,
        (ushort) 63286,
        (ushort) 8310,
        (ushort) 47,
        (ushort) 9786,
        (ushort) 32,
        (ushort) 160,
        (ushort) 9824,
        (ushort) 63218,
        (ushort) 163,
        (ushort) 8715,
        (ushort) 8721,
        (ushort) 9788,
        (ushort) 116,
        (ushort) 964,
        (ushort) 359,
        (ushort) 357,
        (ushort) 355,
        (ushort) 539,
        (ushort) 8756,
        (ushort) 952,
        (ushort) 977,
        (ushort) 254,
        (ushort) 51,
        (ushort) 8540,
        (ushort) 8323,
        (ushort) 63283,
        (ushort) 190,
        (ushort) 63198,
        (ushort) 179,
        (ushort) 732,
        (ushort) 771,
        (ushort) 900,
        (ushort) 8482,
        (ushort) 63722,
        (ushort) 63195,
        (ushort) 9660,
        (ushort) 9668,
        (ushort) 9658,
        (ushort) 9650,
        (ushort) 63219,
        (ushort) 50,
        (ushort) 8229,
        (ushort) 8322,
        (ushort) 63282,
        (ushort) 178,
        (ushort) 8532,
        (ushort) 117,
        (ushort) 250,
        (ushort) 365,
        (ushort) 251,
        (ushort) 252,
        (ushort) 249,
        (ushort) 432,
        (ushort) 369,
        (ushort) 363,
        (ushort) 95,
        (ushort) 8215,
        (ushort) 8746,
        (ushort) 8704,
        (ushort) 371,
        (ushort) 9600,
        (ushort) 965,
        (ushort) 971,
        (ushort) 944,
        (ushort) 973,
        (ushort) 367,
        (ushort) 361,
        (ushort) 118,
        (ushort) 119,
        (ushort) 7811,
        (ushort) 373,
        (ushort) 7813,
        (ushort) 8472,
        (ushort) 7809,
        (ushort) 120,
        (ushort) 958,
        (ushort) 121,
        (ushort) 253,
        (ushort) 375,
        (ushort) byte.MaxValue,
        (ushort) 165,
        (ushort) 7923,
        (ushort) 122,
        (ushort) 378,
        (ushort) 382,
        (ushort) 380,
        (ushort) 48,
        (ushort) 8320,
        (ushort) 63280,
        (ushort) 8304,
        (ushort) 950
      };
      PdfFont.m_arrPDFEncodingArray = new ushort[256]
      {
        (ushort) 0,
        (ushort) 65533,
        (ushort) 65533,
        (ushort) 65533,
        (ushort) 65533,
        (ushort) 65533,
        (ushort) 65533,
        (ushort) 65533,
        (ushort) 65533,
        (ushort) 9,
        (ushort) 10,
        (ushort) 65533,
        (ushort) 12,
        (ushort) 13,
        (ushort) 65533,
        (ushort) 65533,
        (ushort) 65533,
        (ushort) 65533,
        (ushort) 65533,
        (ushort) 65533,
        (ushort) 65533,
        (ushort) 65533,
        (ushort) 65533,
        (ushort) 65533,
        (ushort) 728,
        (ushort) 711,
        (ushort) 710,
        (ushort) 729,
        (ushort) 733,
        (ushort) 731,
        (ushort) 730,
        (ushort) 732,
        (ushort) 32,
        (ushort) 33,
        (ushort) 34,
        (ushort) 35,
        (ushort) 36,
        (ushort) 37,
        (ushort) 38,
        (ushort) 39,
        (ushort) 40,
        (ushort) 41,
        (ushort) 42,
        (ushort) 43,
        (ushort) 44,
        (ushort) 45,
        (ushort) 46,
        (ushort) 47,
        (ushort) 48,
        (ushort) 49,
        (ushort) 50,
        (ushort) 51,
        (ushort) 52,
        (ushort) 53,
        (ushort) 54,
        (ushort) 55,
        (ushort) 56,
        (ushort) 57,
        (ushort) 58,
        (ushort) 59,
        (ushort) 60,
        (ushort) 61,
        (ushort) 62,
        (ushort) 63,
        (ushort) 64,
        (ushort) 65,
        (ushort) 66,
        (ushort) 67,
        (ushort) 68,
        (ushort) 69,
        (ushort) 70,
        (ushort) 71,
        (ushort) 72,
        (ushort) 73,
        (ushort) 74,
        (ushort) 75,
        (ushort) 76,
        (ushort) 77,
        (ushort) 78,
        (ushort) 79,
        (ushort) 80,
        (ushort) 81,
        (ushort) 82,
        (ushort) 83,
        (ushort) 84,
        (ushort) 85,
        (ushort) 86,
        (ushort) 87,
        (ushort) 88,
        (ushort) 89,
        (ushort) 90,
        (ushort) 91,
        (ushort) 92,
        (ushort) 93,
        (ushort) 94,
        (ushort) 95,
        (ushort) 96,
        (ushort) 97,
        (ushort) 98,
        (ushort) 99,
        (ushort) 100,
        (ushort) 101,
        (ushort) 102,
        (ushort) 103,
        (ushort) 104,
        (ushort) 105,
        (ushort) 106,
        (ushort) 107,
        (ushort) 108,
        (ushort) 109,
        (ushort) 110,
        (ushort) 111,
        (ushort) 112,
        (ushort) 113,
        (ushort) 114,
        (ushort) 115,
        (ushort) 116,
        (ushort) 117,
        (ushort) 118,
        (ushort) 119,
        (ushort) 120,
        (ushort) 121,
        (ushort) 122,
        (ushort) 123,
        (ushort) 124,
        (ushort) 125,
        (ushort) 126,
        (ushort) 65533,
        (ushort) 8226,
        (ushort) 8224,
        (ushort) 8225,
        (ushort) 8230,
        (ushort) 8212,
        (ushort) 8211,
        (ushort) 402,
        (ushort) 8260,
        (ushort) 8249,
        (ushort) 8250,
        (ushort) 8722,
        (ushort) 8240,
        (ushort) 8222,
        (ushort) 8220,
        (ushort) 8221,
        (ushort) 8216,
        (ushort) 8217,
        (ushort) 8218,
        (ushort) 8482,
        (ushort) 64257,
        (ushort) 64258,
        (ushort) 321,
        (ushort) 338,
        (ushort) 352,
        (ushort) 376,
        (ushort) 381,
        (ushort) 305,
        (ushort) 322,
        (ushort) 339,
        (ushort) 353,
        (ushort) 382,
        (ushort) 65533,
        (ushort) 8364,
        (ushort) 161,
        (ushort) 162,
        (ushort) 163,
        (ushort) 164,
        (ushort) 165,
        (ushort) 166,
        (ushort) 167,
        (ushort) 168,
        (ushort) 169,
        (ushort) 170,
        (ushort) 171,
        (ushort) 172,
        (ushort) 65533,
        (ushort) 174,
        (ushort) 175,
        (ushort) 176,
        (ushort) 177,
        (ushort) 178,
        (ushort) 179,
        (ushort) 180,
        (ushort) 181,
        (ushort) 182,
        (ushort) 183,
        (ushort) 184,
        (ushort) 185,
        (ushort) 186,
        (ushort) 187,
        (ushort) 188,
        (ushort) 189,
        (ushort) 190,
        (ushort) 191,
        (ushort) 192,
        (ushort) 193,
        (ushort) 194,
        (ushort) 195,
        (ushort) 196,
        (ushort) 197,
        (ushort) 198,
        (ushort) 199,
        (ushort) 200,
        (ushort) 201,
        (ushort) 202,
        (ushort) 203,
        (ushort) 204,
        (ushort) 205,
        (ushort) 206,
        (ushort) 207,
        (ushort) 208,
        (ushort) 209,
        (ushort) 210,
        (ushort) 211,
        (ushort) 212,
        (ushort) 213,
        (ushort) 214,
        (ushort) 215,
        (ushort) 216,
        (ushort) 217,
        (ushort) 218,
        (ushort) 219,
        (ushort) 220,
        (ushort) 221,
        (ushort) 222,
        (ushort) 223,
        (ushort) 224,
        (ushort) 225,
        (ushort) 226,
        (ushort) 227,
        (ushort) 228,
        (ushort) 229,
        (ushort) 230,
        (ushort) 231,
        (ushort) 232,
        (ushort) 233,
        (ushort) 234,
        (ushort) 235,
        (ushort) 236,
        (ushort) 237,
        (ushort) 238,
        (ushort) 239,
        (ushort) 240,
        (ushort) 241,
        (ushort) 242,
        (ushort) 243,
        (ushort) 244,
        (ushort) 245,
        (ushort) 246,
        (ushort) 247,
        (ushort) 248,
        (ushort) 249,
        (ushort) 250,
        (ushort) 251,
        (ushort) 252,
        (ushort) 253,
        (ushort) 254,
        (ushort) byte.MaxValue
      };
      PdfFont.m_arrTableNames = new string[9]
      {
        "cvt ",
        "fpgm",
        "glyf",
        "head",
        "hhea",
        "hmtx",
        "loca",
        "maxp",
        "prep"
      };
      PdfFont.m_arrVerticalExtents = new ushort[14]
      {
        (ushort) 978,
        (ushort) 1022,
        (ushort) 978,
        (ushort) 1022,
        (ushort) 1156,
        (ushort) 1190,
        (ushort) 1156,
        (ushort) 1190,
        (ushort) 1116,
        (ushort) 1153,
        (ushort) 1100,
        (ushort) 1139,
        (ushort) 1131,
        (ushort) 1091
      };
      PdfFont.m_arrFontWidths = new ushort[14][]
      {
        new ushort[256]
        {
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600
        },
        new ushort[256]
        {
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600
        },
        new ushort[256]
        {
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600
        },
        new ushort[256]
        {
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600,
          (ushort) 600
        },
        new ushort[256]
        {
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 278,
          (ushort) 278,
          (ushort) 355,
          (ushort) 556,
          (ushort) 556,
          (ushort) 889,
          (ushort) 667,
          (ushort) 191,
          (ushort) 333,
          (ushort) 333,
          (ushort) 389,
          (ushort) 584,
          (ushort) 278,
          (ushort) 333,
          (ushort) 278,
          (ushort) 278,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 278,
          (ushort) 278,
          (ushort) 584,
          (ushort) 584,
          (ushort) 584,
          (ushort) 556,
          (ushort) 1015,
          (ushort) 667,
          (ushort) 667,
          (ushort) 722,
          (ushort) 722,
          (ushort) 667,
          (ushort) 611,
          (ushort) 778,
          (ushort) 722,
          (ushort) 278,
          (ushort) 500,
          (ushort) 667,
          (ushort) 556,
          (ushort) 833,
          (ushort) 722,
          (ushort) 778,
          (ushort) 667,
          (ushort) 778,
          (ushort) 722,
          (ushort) 667,
          (ushort) 611,
          (ushort) 722,
          (ushort) 667,
          (ushort) 944,
          (ushort) 667,
          (ushort) 667,
          (ushort) 611,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 469,
          (ushort) 556,
          (ushort) 333,
          (ushort) 556,
          (ushort) 556,
          (ushort) 500,
          (ushort) 556,
          (ushort) 556,
          (ushort) 278,
          (ushort) 556,
          (ushort) 556,
          (ushort) 222,
          (ushort) 222,
          (ushort) 500,
          (ushort) 222,
          (ushort) 833,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 333,
          (ushort) 500,
          (ushort) 278,
          (ushort) 556,
          (ushort) 500,
          (ushort) 722,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 334,
          (ushort) 260,
          (ushort) 334,
          (ushort) 584,
          (ushort) 278,
          (ushort) 350,
          (ushort) 556,
          (ushort) 556,
          (ushort) 1000,
          (ushort) 1000,
          (ushort) 556,
          (ushort) 556,
          (ushort) 167,
          (ushort) 333,
          (ushort) 333,
          (ushort) 584,
          (ushort) 1000,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 222,
          (ushort) 222,
          (ushort) 222,
          (ushort) 1000,
          (ushort) 500,
          (ushort) 500,
          (ushort) 556,
          (ushort) 1000,
          (ushort) 667,
          (ushort) 667,
          (ushort) 611,
          (ushort) 278,
          (ushort) 222,
          (ushort) 944,
          (ushort) 500,
          (ushort) 500,
          (ushort) 278,
          (ushort) 278,
          (ushort) 333,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 260,
          (ushort) 556,
          (ushort) 333,
          (ushort) 737,
          (ushort) 370,
          (ushort) 556,
          (ushort) 584,
          (ushort) 278,
          (ushort) 737,
          (ushort) 333,
          (ushort) 400,
          (ushort) 584,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 556,
          (ushort) 537,
          (ushort) 278,
          (ushort) 333,
          (ushort) 333,
          (ushort) 365,
          (ushort) 556,
          (ushort) 834,
          (ushort) 834,
          (ushort) 834,
          (ushort) 611,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 1000,
          (ushort) 722,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 722,
          (ushort) 722,
          (ushort) 778,
          (ushort) 778,
          (ushort) 778,
          (ushort) 778,
          (ushort) 778,
          (ushort) 584,
          (ushort) 778,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 667,
          (ushort) 667,
          (ushort) 611,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 889,
          (ushort) 500,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 584,
          (ushort) 611,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 500,
          (ushort) 556,
          (ushort) 500
        },
        new ushort[256]
        {
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 278,
          (ushort) 333,
          (ushort) 474,
          (ushort) 556,
          (ushort) 556,
          (ushort) 889,
          (ushort) 722,
          (ushort) 238,
          (ushort) 333,
          (ushort) 333,
          (ushort) 389,
          (ushort) 584,
          (ushort) 278,
          (ushort) 333,
          (ushort) 278,
          (ushort) 278,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 333,
          (ushort) 333,
          (ushort) 584,
          (ushort) 584,
          (ushort) 584,
          (ushort) 611,
          (ushort) 975,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 667,
          (ushort) 611,
          (ushort) 778,
          (ushort) 722,
          (ushort) 278,
          (ushort) 556,
          (ushort) 722,
          (ushort) 611,
          (ushort) 833,
          (ushort) 722,
          (ushort) 778,
          (ushort) 667,
          (ushort) 778,
          (ushort) 722,
          (ushort) 667,
          (ushort) 611,
          (ushort) 722,
          (ushort) 667,
          (ushort) 944,
          (ushort) 667,
          (ushort) 667,
          (ushort) 611,
          (ushort) 333,
          (ushort) 278,
          (ushort) 333,
          (ushort) 584,
          (ushort) 556,
          (ushort) 333,
          (ushort) 556,
          (ushort) 611,
          (ushort) 556,
          (ushort) 611,
          (ushort) 556,
          (ushort) 333,
          (ushort) 611,
          (ushort) 611,
          (ushort) 278,
          (ushort) 278,
          (ushort) 556,
          (ushort) 278,
          (ushort) 889,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 389,
          (ushort) 556,
          (ushort) 333,
          (ushort) 611,
          (ushort) 556,
          (ushort) 778,
          (ushort) 556,
          (ushort) 556,
          (ushort) 500,
          (ushort) 389,
          (ushort) 280,
          (ushort) 389,
          (ushort) 584,
          (ushort) 278,
          (ushort) 350,
          (ushort) 556,
          (ushort) 556,
          (ushort) 1000,
          (ushort) 1000,
          (ushort) 556,
          (ushort) 556,
          (ushort) 167,
          (ushort) 333,
          (ushort) 333,
          (ushort) 584,
          (ushort) 1000,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 1000,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 1000,
          (ushort) 667,
          (ushort) 667,
          (ushort) 611,
          (ushort) 278,
          (ushort) 278,
          (ushort) 944,
          (ushort) 556,
          (ushort) 500,
          (ushort) 278,
          (ushort) 278,
          (ushort) 333,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 280,
          (ushort) 556,
          (ushort) 333,
          (ushort) 737,
          (ushort) 370,
          (ushort) 556,
          (ushort) 584,
          (ushort) 278,
          (ushort) 737,
          (ushort) 333,
          (ushort) 400,
          (ushort) 584,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 611,
          (ushort) 556,
          (ushort) 278,
          (ushort) 333,
          (ushort) 333,
          (ushort) 365,
          (ushort) 556,
          (ushort) 834,
          (ushort) 834,
          (ushort) 834,
          (ushort) 611,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 1000,
          (ushort) 722,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 722,
          (ushort) 722,
          (ushort) 778,
          (ushort) 778,
          (ushort) 778,
          (ushort) 778,
          (ushort) 778,
          (ushort) 584,
          (ushort) 778,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 667,
          (ushort) 667,
          (ushort) 611,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 889,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 584,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 556,
          (ushort) 611,
          (ushort) 556
        },
        new ushort[256]
        {
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 278,
          (ushort) 278,
          (ushort) 355,
          (ushort) 556,
          (ushort) 556,
          (ushort) 889,
          (ushort) 667,
          (ushort) 191,
          (ushort) 333,
          (ushort) 333,
          (ushort) 389,
          (ushort) 584,
          (ushort) 278,
          (ushort) 333,
          (ushort) 278,
          (ushort) 278,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 278,
          (ushort) 278,
          (ushort) 584,
          (ushort) 584,
          (ushort) 584,
          (ushort) 556,
          (ushort) 1015,
          (ushort) 667,
          (ushort) 667,
          (ushort) 722,
          (ushort) 722,
          (ushort) 667,
          (ushort) 611,
          (ushort) 778,
          (ushort) 722,
          (ushort) 278,
          (ushort) 500,
          (ushort) 667,
          (ushort) 556,
          (ushort) 833,
          (ushort) 722,
          (ushort) 778,
          (ushort) 667,
          (ushort) 778,
          (ushort) 722,
          (ushort) 667,
          (ushort) 611,
          (ushort) 722,
          (ushort) 667,
          (ushort) 944,
          (ushort) 667,
          (ushort) 667,
          (ushort) 611,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 469,
          (ushort) 556,
          (ushort) 333,
          (ushort) 556,
          (ushort) 556,
          (ushort) 500,
          (ushort) 556,
          (ushort) 556,
          (ushort) 278,
          (ushort) 556,
          (ushort) 556,
          (ushort) 222,
          (ushort) 222,
          (ushort) 500,
          (ushort) 222,
          (ushort) 833,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 333,
          (ushort) 500,
          (ushort) 278,
          (ushort) 556,
          (ushort) 500,
          (ushort) 722,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 334,
          (ushort) 260,
          (ushort) 334,
          (ushort) 584,
          (ushort) 278,
          (ushort) 350,
          (ushort) 556,
          (ushort) 556,
          (ushort) 1000,
          (ushort) 1000,
          (ushort) 556,
          (ushort) 556,
          (ushort) 167,
          (ushort) 333,
          (ushort) 333,
          (ushort) 584,
          (ushort) 1000,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 222,
          (ushort) 222,
          (ushort) 222,
          (ushort) 1000,
          (ushort) 500,
          (ushort) 500,
          (ushort) 556,
          (ushort) 1000,
          (ushort) 667,
          (ushort) 667,
          (ushort) 611,
          (ushort) 278,
          (ushort) 222,
          (ushort) 944,
          (ushort) 500,
          (ushort) 500,
          (ushort) 278,
          (ushort) 278,
          (ushort) 333,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 260,
          (ushort) 556,
          (ushort) 333,
          (ushort) 737,
          (ushort) 370,
          (ushort) 556,
          (ushort) 584,
          (ushort) 278,
          (ushort) 737,
          (ushort) 333,
          (ushort) 400,
          (ushort) 584,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 556,
          (ushort) 537,
          (ushort) 278,
          (ushort) 333,
          (ushort) 333,
          (ushort) 365,
          (ushort) 556,
          (ushort) 834,
          (ushort) 834,
          (ushort) 834,
          (ushort) 611,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 1000,
          (ushort) 722,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 722,
          (ushort) 722,
          (ushort) 778,
          (ushort) 778,
          (ushort) 778,
          (ushort) 778,
          (ushort) 778,
          (ushort) 584,
          (ushort) 778,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 667,
          (ushort) 667,
          (ushort) 611,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 889,
          (ushort) 500,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 584,
          (ushort) 611,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 500,
          (ushort) 556,
          (ushort) 500
        },
        new ushort[256]
        {
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 278,
          (ushort) 333,
          (ushort) 474,
          (ushort) 556,
          (ushort) 556,
          (ushort) 889,
          (ushort) 722,
          (ushort) 238,
          (ushort) 333,
          (ushort) 333,
          (ushort) 389,
          (ushort) 584,
          (ushort) 278,
          (ushort) 333,
          (ushort) 278,
          (ushort) 278,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 333,
          (ushort) 333,
          (ushort) 584,
          (ushort) 584,
          (ushort) 584,
          (ushort) 611,
          (ushort) 975,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 667,
          (ushort) 611,
          (ushort) 778,
          (ushort) 722,
          (ushort) 278,
          (ushort) 556,
          (ushort) 722,
          (ushort) 611,
          (ushort) 833,
          (ushort) 722,
          (ushort) 778,
          (ushort) 667,
          (ushort) 778,
          (ushort) 722,
          (ushort) 667,
          (ushort) 611,
          (ushort) 722,
          (ushort) 667,
          (ushort) 944,
          (ushort) 667,
          (ushort) 667,
          (ushort) 611,
          (ushort) 333,
          (ushort) 278,
          (ushort) 333,
          (ushort) 584,
          (ushort) 556,
          (ushort) 333,
          (ushort) 556,
          (ushort) 611,
          (ushort) 556,
          (ushort) 611,
          (ushort) 556,
          (ushort) 333,
          (ushort) 611,
          (ushort) 611,
          (ushort) 278,
          (ushort) 278,
          (ushort) 556,
          (ushort) 278,
          (ushort) 889,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 389,
          (ushort) 556,
          (ushort) 333,
          (ushort) 611,
          (ushort) 556,
          (ushort) 778,
          (ushort) 556,
          (ushort) 556,
          (ushort) 500,
          (ushort) 389,
          (ushort) 280,
          (ushort) 389,
          (ushort) 584,
          (ushort) 278,
          (ushort) 350,
          (ushort) 556,
          (ushort) 556,
          (ushort) 1000,
          (ushort) 1000,
          (ushort) 556,
          (ushort) 556,
          (ushort) 167,
          (ushort) 333,
          (ushort) 333,
          (ushort) 584,
          (ushort) 1000,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 1000,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 1000,
          (ushort) 667,
          (ushort) 667,
          (ushort) 611,
          (ushort) 278,
          (ushort) 278,
          (ushort) 944,
          (ushort) 556,
          (ushort) 500,
          (ushort) 278,
          (ushort) 278,
          (ushort) 333,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 280,
          (ushort) 556,
          (ushort) 333,
          (ushort) 737,
          (ushort) 370,
          (ushort) 556,
          (ushort) 584,
          (ushort) 278,
          (ushort) 737,
          (ushort) 333,
          (ushort) 400,
          (ushort) 584,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 611,
          (ushort) 556,
          (ushort) 278,
          (ushort) 333,
          (ushort) 333,
          (ushort) 365,
          (ushort) 556,
          (ushort) 834,
          (ushort) 834,
          (ushort) 834,
          (ushort) 611,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 1000,
          (ushort) 722,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 722,
          (ushort) 722,
          (ushort) 778,
          (ushort) 778,
          (ushort) 778,
          (ushort) 778,
          (ushort) 778,
          (ushort) 584,
          (ushort) 778,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 667,
          (ushort) 667,
          (ushort) 611,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 889,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 584,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 556,
          (ushort) 611,
          (ushort) 556
        },
        new ushort[256]
        {
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 250,
          (ushort) 333,
          (ushort) 408,
          (ushort) 500,
          (ushort) 500,
          (ushort) 833,
          (ushort) 778,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 500,
          (ushort) 564,
          (ushort) 250,
          (ushort) 333,
          (ushort) 250,
          (ushort) 278,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 278,
          (ushort) 278,
          (ushort) 564,
          (ushort) 564,
          (ushort) 564,
          (ushort) 444,
          (ushort) 921,
          (ushort) 722,
          (ushort) 667,
          (ushort) 667,
          (ushort) 722,
          (ushort) 611,
          (ushort) 556,
          (ushort) 722,
          (ushort) 722,
          (ushort) 333,
          (ushort) 389,
          (ushort) 722,
          (ushort) 611,
          (ushort) 889,
          (ushort) 722,
          (ushort) 722,
          (ushort) 556,
          (ushort) 722,
          (ushort) 667,
          (ushort) 556,
          (ushort) 611,
          (ushort) 722,
          (ushort) 722,
          (ushort) 944,
          (ushort) 722,
          (ushort) 722,
          (ushort) 611,
          (ushort) 333,
          (ushort) 278,
          (ushort) 333,
          (ushort) 469,
          (ushort) 500,
          (ushort) 333,
          (ushort) 444,
          (ushort) 500,
          (ushort) 444,
          (ushort) 500,
          (ushort) 444,
          (ushort) 333,
          (ushort) 500,
          (ushort) 500,
          (ushort) 278,
          (ushort) 278,
          (ushort) 500,
          (ushort) 278,
          (ushort) 778,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 333,
          (ushort) 389,
          (ushort) 278,
          (ushort) 500,
          (ushort) 500,
          (ushort) 722,
          (ushort) 500,
          (ushort) 500,
          (ushort) 444,
          (ushort) 480,
          (ushort) 200,
          (ushort) 480,
          (ushort) 541,
          (ushort) 250,
          (ushort) 350,
          (ushort) 500,
          (ushort) 500,
          (ushort) 1000,
          (ushort) 1000,
          (ushort) 500,
          (ushort) 500,
          (ushort) 167,
          (ushort) 333,
          (ushort) 333,
          (ushort) 564,
          (ushort) 1000,
          (ushort) 444,
          (ushort) 444,
          (ushort) 444,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 980,
          (ushort) 556,
          (ushort) 556,
          (ushort) 611,
          (ushort) 889,
          (ushort) 556,
          (ushort) 722,
          (ushort) 611,
          (ushort) 278,
          (ushort) 278,
          (ushort) 722,
          (ushort) 389,
          (ushort) 444,
          (ushort) 250,
          (ushort) 250,
          (ushort) 333,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 200,
          (ushort) 500,
          (ushort) 333,
          (ushort) 760,
          (ushort) 276,
          (ushort) 500,
          (ushort) 564,
          (ushort) 250,
          (ushort) 760,
          (ushort) 333,
          (ushort) 400,
          (ushort) 564,
          (ushort) 300,
          (ushort) 300,
          (ushort) 333,
          (ushort) 500,
          (ushort) 453,
          (ushort) 250,
          (ushort) 333,
          (ushort) 300,
          (ushort) 310,
          (ushort) 500,
          (ushort) 750,
          (ushort) 750,
          (ushort) 750,
          (ushort) 444,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 889,
          (ushort) 667,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 564,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 556,
          (ushort) 500,
          (ushort) 444,
          (ushort) 444,
          (ushort) 444,
          (ushort) 444,
          (ushort) 444,
          (ushort) 444,
          (ushort) 667,
          (ushort) 444,
          (ushort) 444,
          (ushort) 444,
          (ushort) 444,
          (ushort) 444,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 564,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500
        },
        new ushort[256]
        {
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 250,
          (ushort) 333,
          (ushort) 555,
          (ushort) 500,
          (ushort) 500,
          (ushort) 1000,
          (ushort) 833,
          (ushort) 278,
          (ushort) 333,
          (ushort) 333,
          (ushort) 500,
          (ushort) 570,
          (ushort) 250,
          (ushort) 333,
          (ushort) 250,
          (ushort) 278,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 333,
          (ushort) 333,
          (ushort) 570,
          (ushort) 570,
          (ushort) 570,
          (ushort) 500,
          (ushort) 930,
          (ushort) 722,
          (ushort) 667,
          (ushort) 722,
          (ushort) 722,
          (ushort) 667,
          (ushort) 611,
          (ushort) 778,
          (ushort) 778,
          (ushort) 389,
          (ushort) 500,
          (ushort) 778,
          (ushort) 667,
          (ushort) 944,
          (ushort) 722,
          (ushort) 778,
          (ushort) 611,
          (ushort) 778,
          (ushort) 722,
          (ushort) 556,
          (ushort) 667,
          (ushort) 722,
          (ushort) 722,
          (ushort) 1000,
          (ushort) 722,
          (ushort) 722,
          (ushort) 667,
          (ushort) 333,
          (ushort) 278,
          (ushort) 333,
          (ushort) 581,
          (ushort) 500,
          (ushort) 333,
          (ushort) 500,
          (ushort) 556,
          (ushort) 444,
          (ushort) 556,
          (ushort) 444,
          (ushort) 333,
          (ushort) 500,
          (ushort) 556,
          (ushort) 278,
          (ushort) 333,
          (ushort) 556,
          (ushort) 278,
          (ushort) 833,
          (ushort) 556,
          (ushort) 500,
          (ushort) 556,
          (ushort) 556,
          (ushort) 444,
          (ushort) 389,
          (ushort) 333,
          (ushort) 556,
          (ushort) 500,
          (ushort) 722,
          (ushort) 500,
          (ushort) 500,
          (ushort) 444,
          (ushort) 394,
          (ushort) 220,
          (ushort) 394,
          (ushort) 520,
          (ushort) 250,
          (ushort) 350,
          (ushort) 500,
          (ushort) 500,
          (ushort) 1000,
          (ushort) 1000,
          (ushort) 500,
          (ushort) 500,
          (ushort) 167,
          (ushort) 333,
          (ushort) 333,
          (ushort) 570,
          (ushort) 1000,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 1000,
          (ushort) 556,
          (ushort) 556,
          (ushort) 667,
          (ushort) 1000,
          (ushort) 556,
          (ushort) 722,
          (ushort) 667,
          (ushort) 278,
          (ushort) 278,
          (ushort) 722,
          (ushort) 389,
          (ushort) 444,
          (ushort) 250,
          (ushort) 250,
          (ushort) 333,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 220,
          (ushort) 500,
          (ushort) 333,
          (ushort) 747,
          (ushort) 300,
          (ushort) 500,
          (ushort) 570,
          (ushort) 250,
          (ushort) 747,
          (ushort) 333,
          (ushort) 400,
          (ushort) 570,
          (ushort) 300,
          (ushort) 300,
          (ushort) 333,
          (ushort) 556,
          (ushort) 540,
          (ushort) 250,
          (ushort) 333,
          (ushort) 300,
          (ushort) 330,
          (ushort) 500,
          (ushort) 750,
          (ushort) 750,
          (ushort) 750,
          (ushort) 500,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 1000,
          (ushort) 722,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 389,
          (ushort) 389,
          (ushort) 389,
          (ushort) 389,
          (ushort) 722,
          (ushort) 722,
          (ushort) 778,
          (ushort) 778,
          (ushort) 778,
          (ushort) 778,
          (ushort) 778,
          (ushort) 570,
          (ushort) 778,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 611,
          (ushort) 556,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 722,
          (ushort) 444,
          (ushort) 444,
          (ushort) 444,
          (ushort) 444,
          (ushort) 444,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 500,
          (ushort) 556,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 570,
          (ushort) 500,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 500,
          (ushort) 556,
          (ushort) 500
        },
        new ushort[256]
        {
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 250,
          (ushort) 333,
          (ushort) 420,
          (ushort) 500,
          (ushort) 500,
          (ushort) 833,
          (ushort) 778,
          (ushort) 214,
          (ushort) 333,
          (ushort) 333,
          (ushort) 500,
          (ushort) 675,
          (ushort) 250,
          (ushort) 333,
          (ushort) 250,
          (ushort) 278,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 333,
          (ushort) 333,
          (ushort) 675,
          (ushort) 675,
          (ushort) 675,
          (ushort) 500,
          (ushort) 920,
          (ushort) 611,
          (ushort) 611,
          (ushort) 667,
          (ushort) 722,
          (ushort) 611,
          (ushort) 611,
          (ushort) 722,
          (ushort) 722,
          (ushort) 333,
          (ushort) 444,
          (ushort) 667,
          (ushort) 556,
          (ushort) 833,
          (ushort) 667,
          (ushort) 722,
          (ushort) 611,
          (ushort) 722,
          (ushort) 611,
          (ushort) 500,
          (ushort) 556,
          (ushort) 722,
          (ushort) 611,
          (ushort) 833,
          (ushort) 611,
          (ushort) 556,
          (ushort) 556,
          (ushort) 389,
          (ushort) 278,
          (ushort) 389,
          (ushort) 422,
          (ushort) 500,
          (ushort) 333,
          (ushort) 500,
          (ushort) 500,
          (ushort) 444,
          (ushort) 500,
          (ushort) 444,
          (ushort) 278,
          (ushort) 500,
          (ushort) 500,
          (ushort) 278,
          (ushort) 278,
          (ushort) 444,
          (ushort) 278,
          (ushort) 722,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 389,
          (ushort) 389,
          (ushort) 278,
          (ushort) 500,
          (ushort) 444,
          (ushort) 667,
          (ushort) 444,
          (ushort) 444,
          (ushort) 389,
          (ushort) 400,
          (ushort) 275,
          (ushort) 400,
          (ushort) 541,
          (ushort) 250,
          (ushort) 350,
          (ushort) 500,
          (ushort) 500,
          (ushort) 889,
          (ushort) 889,
          (ushort) 500,
          (ushort) 500,
          (ushort) 167,
          (ushort) 333,
          (ushort) 333,
          (ushort) 675,
          (ushort) 1000,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 980,
          (ushort) 500,
          (ushort) 500,
          (ushort) 556,
          (ushort) 944,
          (ushort) 500,
          (ushort) 556,
          (ushort) 556,
          (ushort) 278,
          (ushort) 278,
          (ushort) 667,
          (ushort) 389,
          (ushort) 389,
          (ushort) 250,
          (ushort) 250,
          (ushort) 389,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 275,
          (ushort) 500,
          (ushort) 333,
          (ushort) 760,
          (ushort) 276,
          (ushort) 500,
          (ushort) 675,
          (ushort) 250,
          (ushort) 760,
          (ushort) 333,
          (ushort) 400,
          (ushort) 675,
          (ushort) 300,
          (ushort) 300,
          (ushort) 333,
          (ushort) 500,
          (ushort) 523,
          (ushort) 250,
          (ushort) 333,
          (ushort) 300,
          (ushort) 310,
          (ushort) 500,
          (ushort) 750,
          (ushort) 750,
          (ushort) 750,
          (ushort) 500,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 889,
          (ushort) 667,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 611,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 722,
          (ushort) 667,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 675,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 556,
          (ushort) 611,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 667,
          (ushort) 444,
          (ushort) 444,
          (ushort) 444,
          (ushort) 444,
          (ushort) 444,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 675,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 444,
          (ushort) 500,
          (ushort) 444
        },
        new ushort[256]
        {
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 250,
          (ushort) 389,
          (ushort) 555,
          (ushort) 500,
          (ushort) 500,
          (ushort) 833,
          (ushort) 778,
          (ushort) 278,
          (ushort) 333,
          (ushort) 333,
          (ushort) 500,
          (ushort) 570,
          (ushort) 250,
          (ushort) 333,
          (ushort) 250,
          (ushort) 278,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 333,
          (ushort) 333,
          (ushort) 570,
          (ushort) 570,
          (ushort) 570,
          (ushort) 500,
          (ushort) 832,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 722,
          (ushort) 667,
          (ushort) 667,
          (ushort) 722,
          (ushort) 778,
          (ushort) 389,
          (ushort) 500,
          (ushort) 667,
          (ushort) 611,
          (ushort) 889,
          (ushort) 722,
          (ushort) 722,
          (ushort) 611,
          (ushort) 722,
          (ushort) 667,
          (ushort) 556,
          (ushort) 611,
          (ushort) 722,
          (ushort) 667,
          (ushort) 889,
          (ushort) 667,
          (ushort) 611,
          (ushort) 611,
          (ushort) 333,
          (ushort) 278,
          (ushort) 333,
          (ushort) 570,
          (ushort) 500,
          (ushort) 333,
          (ushort) 500,
          (ushort) 500,
          (ushort) 444,
          (ushort) 500,
          (ushort) 444,
          (ushort) 333,
          (ushort) 500,
          (ushort) 556,
          (ushort) 278,
          (ushort) 278,
          (ushort) 500,
          (ushort) 278,
          (ushort) 778,
          (ushort) 556,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 389,
          (ushort) 389,
          (ushort) 278,
          (ushort) 556,
          (ushort) 444,
          (ushort) 667,
          (ushort) 500,
          (ushort) 444,
          (ushort) 389,
          (ushort) 348,
          (ushort) 220,
          (ushort) 348,
          (ushort) 570,
          (ushort) 250,
          (ushort) 350,
          (ushort) 500,
          (ushort) 500,
          (ushort) 1000,
          (ushort) 1000,
          (ushort) 500,
          (ushort) 500,
          (ushort) 167,
          (ushort) 333,
          (ushort) 333,
          (ushort) 606,
          (ushort) 1000,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 333,
          (ushort) 333,
          (ushort) 333,
          (ushort) 1000,
          (ushort) 556,
          (ushort) 556,
          (ushort) 611,
          (ushort) 944,
          (ushort) 556,
          (ushort) 611,
          (ushort) 611,
          (ushort) 278,
          (ushort) 278,
          (ushort) 722,
          (ushort) 389,
          (ushort) 389,
          (ushort) 250,
          (ushort) 250,
          (ushort) 389,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 220,
          (ushort) 500,
          (ushort) 333,
          (ushort) 747,
          (ushort) 266,
          (ushort) 500,
          (ushort) 606,
          (ushort) 250,
          (ushort) 747,
          (ushort) 333,
          (ushort) 400,
          (ushort) 570,
          (ushort) 300,
          (ushort) 300,
          (ushort) 333,
          (ushort) 576,
          (ushort) 500,
          (ushort) 250,
          (ushort) 333,
          (ushort) 300,
          (ushort) 300,
          (ushort) 500,
          (ushort) 750,
          (ushort) 750,
          (ushort) 750,
          (ushort) 500,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 944,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 667,
          (ushort) 389,
          (ushort) 389,
          (ushort) 389,
          (ushort) 389,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 570,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 722,
          (ushort) 611,
          (ushort) 611,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 722,
          (ushort) 444,
          (ushort) 444,
          (ushort) 444,
          (ushort) 444,
          (ushort) 444,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 500,
          (ushort) 556,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 570,
          (ushort) 500,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 556,
          (ushort) 444,
          (ushort) 500,
          (ushort) 444
        },
        new ushort[256]
        {
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 333,
          (ushort) 713,
          (ushort) 500,
          (ushort) 549,
          (ushort) 833,
          (ushort) 778,
          (ushort) 439,
          (ushort) 333,
          (ushort) 333,
          (ushort) 500,
          (ushort) 549,
          (ushort) 250,
          (ushort) 549,
          (ushort) 250,
          (ushort) 278,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 500,
          (ushort) 278,
          (ushort) 278,
          (ushort) 549,
          (ushort) 549,
          (ushort) 549,
          (ushort) 444,
          (ushort) 549,
          (ushort) 722,
          (ushort) 667,
          (ushort) 722,
          (ushort) 612,
          (ushort) 611,
          (ushort) 763,
          (ushort) 603,
          (ushort) 722,
          (ushort) 333,
          (ushort) 631,
          (ushort) 722,
          (ushort) 686,
          (ushort) 889,
          (ushort) 722,
          (ushort) 722,
          (ushort) 768,
          (ushort) 741,
          (ushort) 556,
          (ushort) 592,
          (ushort) 611,
          (ushort) 690,
          (ushort) 439,
          (ushort) 768,
          (ushort) 645,
          (ushort) 795,
          (ushort) 611,
          (ushort) 333,
          (ushort) 863,
          (ushort) 333,
          (ushort) 658,
          (ushort) 500,
          (ushort) 500,
          (ushort) 631,
          (ushort) 549,
          (ushort) 549,
          (ushort) 494,
          (ushort) 439,
          (ushort) 521,
          (ushort) 411,
          (ushort) 603,
          (ushort) 329,
          (ushort) 603,
          (ushort) 549,
          (ushort) 549,
          (ushort) 576,
          (ushort) 521,
          (ushort) 549,
          (ushort) 549,
          (ushort) 521,
          (ushort) 549,
          (ushort) 603,
          (ushort) 439,
          (ushort) 576,
          (ushort) 713,
          (ushort) 686,
          (ushort) 493,
          (ushort) 686,
          (ushort) 494,
          (ushort) 480,
          (ushort) 200,
          (ushort) 480,
          (ushort) 549,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 250,
          (ushort) 620,
          (ushort) 247,
          (ushort) 549,
          (ushort) 167,
          (ushort) 713,
          (ushort) 500,
          (ushort) 753,
          (ushort) 753,
          (ushort) 753,
          (ushort) 753,
          (ushort) 1042,
          (ushort) 987,
          (ushort) 603,
          (ushort) 987,
          (ushort) 603,
          (ushort) 400,
          (ushort) 549,
          (ushort) 411,
          (ushort) 549,
          (ushort) 549,
          (ushort) 713,
          (ushort) 494,
          (ushort) 460,
          (ushort) 549,
          (ushort) 549,
          (ushort) 549,
          (ushort) 549,
          (ushort) 1000,
          (ushort) 603,
          (ushort) 1000,
          (ushort) 658,
          (ushort) 823,
          (ushort) 686,
          (ushort) 795,
          (ushort) 987,
          (ushort) 768,
          (ushort) 768,
          (ushort) 823,
          (ushort) 768,
          (ushort) 768,
          (ushort) 713,
          (ushort) 713,
          (ushort) 713,
          (ushort) 713,
          (ushort) 713,
          (ushort) 713,
          (ushort) 713,
          (ushort) 768,
          (ushort) 713,
          (ushort) 790,
          (ushort) 790,
          (ushort) 890,
          (ushort) 823,
          (ushort) 549,
          (ushort) 250,
          (ushort) 713,
          (ushort) 603,
          (ushort) 603,
          (ushort) 1042,
          (ushort) 987,
          (ushort) 603,
          (ushort) 987,
          (ushort) 603,
          (ushort) 494,
          (ushort) 329,
          (ushort) 790,
          (ushort) 790,
          (ushort) 786,
          (ushort) 713,
          (ushort) 384,
          (ushort) 384,
          (ushort) 384,
          (ushort) 384,
          (ushort) 384,
          (ushort) 384,
          (ushort) 494,
          (ushort) 494,
          (ushort) 494,
          (ushort) 494,
          (ushort) 250,
          (ushort) 329,
          (ushort) 274,
          (ushort) 686,
          (ushort) 686,
          (ushort) 686,
          (ushort) 384,
          (ushort) 384,
          (ushort) 384,
          (ushort) 384,
          (ushort) 384,
          (ushort) 384,
          (ushort) 494,
          (ushort) 494,
          (ushort) 494,
          (ushort) 250
        },
        new ushort[256]
        {
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 974,
          (ushort) 961,
          (ushort) 974,
          (ushort) 980,
          (ushort) 719,
          (ushort) 789,
          (ushort) 790,
          (ushort) 791,
          (ushort) 690,
          (ushort) 960,
          (ushort) 939,
          (ushort) 549,
          (ushort) 855,
          (ushort) 911,
          (ushort) 933,
          (ushort) 911,
          (ushort) 945,
          (ushort) 974,
          (ushort) 755,
          (ushort) 846,
          (ushort) 762,
          (ushort) 761,
          (ushort) 571,
          (ushort) 677,
          (ushort) 763,
          (ushort) 760,
          (ushort) 759,
          (ushort) 754,
          (ushort) 494,
          (ushort) 552,
          (ushort) 537,
          (ushort) 577,
          (ushort) 692,
          (ushort) 786,
          (ushort) 788,
          (ushort) 788,
          (ushort) 790,
          (ushort) 793,
          (ushort) 794,
          (ushort) 816,
          (ushort) 823,
          (ushort) 789,
          (ushort) 841,
          (ushort) 823,
          (ushort) 833,
          (ushort) 816,
          (ushort) 831,
          (ushort) 923,
          (ushort) 744,
          (ushort) 723,
          (ushort) 749,
          (ushort) 790,
          (ushort) 792,
          (ushort) 695,
          (ushort) 776,
          (ushort) 768,
          (ushort) 792,
          (ushort) 759,
          (ushort) 707,
          (ushort) 708,
          (ushort) 682,
          (ushort) 701,
          (ushort) 826,
          (ushort) 815,
          (ushort) 789,
          (ushort) 789,
          (ushort) 707,
          (ushort) 687,
          (ushort) 696,
          (ushort) 689,
          (ushort) 786,
          (ushort) 787,
          (ushort) 713,
          (ushort) 791,
          (ushort) 785,
          (ushort) 791,
          (ushort) 873,
          (ushort) 761,
          (ushort) 762,
          (ushort) 762,
          (ushort) 759,
          (ushort) 759,
          (ushort) 892,
          (ushort) 892,
          (ushort) 788,
          (ushort) 784,
          (ushort) 438,
          (ushort) 138,
          (ushort) 277,
          (ushort) 415,
          (ushort) 392,
          (ushort) 392,
          (ushort) 668,
          (ushort) 668,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 278,
          (ushort) 732,
          (ushort) 544,
          (ushort) 544,
          (ushort) 910,
          (ushort) 667,
          (ushort) 760,
          (ushort) 760,
          (ushort) 776,
          (ushort) 595,
          (ushort) 694,
          (ushort) 626,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 788,
          (ushort) 894,
          (ushort) 838,
          (ushort) 1016,
          (ushort) 458,
          (ushort) 748,
          (ushort) 924,
          (ushort) 748,
          (ushort) 918,
          (ushort) 927,
          (ushort) 928,
          (ushort) 928,
          (ushort) 834,
          (ushort) 873,
          (ushort) 828,
          (ushort) 924,
          (ushort) 924,
          (ushort) 917,
          (ushort) 930,
          (ushort) 931,
          (ushort) 463,
          (ushort) 883,
          (ushort) 836,
          (ushort) 836,
          (ushort) 867,
          (ushort) 867,
          (ushort) 696,
          (ushort) 696,
          (ushort) 874,
          (ushort) 278,
          (ushort) 874,
          (ushort) 760,
          (ushort) 946,
          (ushort) 771,
          (ushort) 865,
          (ushort) 771,
          (ushort) 888,
          (ushort) 967,
          (ushort) 888,
          (ushort) 831,
          (ushort) 873,
          (ushort) 927,
          (ushort) 970,
          (ushort) 918,
          (ushort) 278
        }
      };
      PdfFont.m_arrDescents = new short[14]
      {
        (short) -157,
        (short) -157,
        (short) -157,
        (short) -157,
        (short) -207,
        (short) -207,
        (short) -207,
        (short) -207,
        (short) -217,
        (short) -217,
        (short) -217,
        (short) -217,
        (short) -207,
        (short) -207
      };
      PdfFont.m_arrCapHeights = new ushort[14]
      {
        (ushort) 562,
        (ushort) 562,
        (ushort) 562,
        (ushort) 562,
        (ushort) 718,
        (ushort) 718,
        (ushort) 718,
        (ushort) 718,
        (ushort) 662,
        (ushort) 676,
        (ushort) 653,
        (ushort) 669,
        (ushort) 718,
        (ushort) 718
      };
    }

    internal PdfFont()
    {
      this.m_bstrType = "Type1";
      this.m_nLastChar = (int) byte.MaxValue;
      this.m_nUnitsPerEm = 1000;
      this.m_nDefaultWidth = (ushort) 1000;
      this.m_rectFontBox = new PdfRect();
      this.m_arrGlyphs = new List<GlyphInfo>();
      this.m_bstrEncoding = "WinAnsiEncoding";
      this.m_arrCmap = new List<MapEntry>();
      this.m_arrDifferences = new List<MapEntry>();
      this.m_mapTableLength = new Dictionary<string, int>();
      this.m_mapTableOffset = new Dictionary<string, int>();
      this.m_mapTableChecksum = new Dictionary<string, int>();
      this.m_arrTables = new PdfStream[9];
      this.m_arrGlyphIDs = new List<int>();
      foreach (string index in PdfFont.m_arrTableNames)
      {
        this.m_mapTableOffset[index] = 0;
        this.m_mapTableLength[index] = 0;
      }
    }

    internal GlyphInfo FindGlyph(char Code)
    {
      int count = this.m_arrGlyphs.Count;
      if (count == 0)
        return (GlyphInfo) null;
      int num1 = 0;
      int num2 = count - 1;
      while (num1 <= num2)
      {
        int index = (num1 + num2) / 2;
        if ((int) this.m_arrGlyphs[index].m_wcCode == (int) Code)
          return this.m_arrGlyphs[index];
        if ((int) Code < (int) this.m_arrGlyphs[index].m_wcCode)
          num2 = index - 1;
        else
          num1 = index + 1;
      }
      return (GlyphInfo) null;
    }

    internal void AddGlyph(char Code, ushort GlyphID, ushort Width)
    {
      GlyphInfo glyphInfo1 = new GlyphInfo();
      glyphInfo1.m_wcCode = Code;
      glyphInfo1.m_nGlyph = GlyphID;
      glyphInfo1.m_nWidth = Width;
      int index = 0;
      foreach (GlyphInfo glyphInfo2 in this.m_arrGlyphs)
      {
        if ((int) Code == (int) glyphInfo2.m_wcCode)
          return;
        if ((int) Code < (int) glyphInfo2.m_wcCode)
        {
          this.m_arrGlyphs.Insert(index, glyphInfo1);
          return;
        }
        else
          ++index;
      }
      this.m_arrGlyphs.Add(glyphInfo1);
    }

    internal void PopulateToUnicodeCmap()
    {
      if (this.m_pToUnicodeObj == null)
        return;
      List<GlyphInfo> list = new List<GlyphInfo>();
      foreach (GlyphInfo glyphInfo in this.m_arrGlyphs)
        list.Add(glyphInfo);
      for (int index1 = 0; index1 < list.Count; ++index1)
      {
        for (int index2 = list.Count - 1; index2 > index1; --index2)
        {
          if ((int) list[index2 - 1].m_wcCode > (int) list[index2].m_wcCode)
          {
            GlyphInfo glyphInfo = list[index2 - 1];
            list[index2 - 1] = list[index2];
            list[index2] = glyphInfo;
          }
        }
      }
      PdfOutput pdfOutput = new PdfOutput(1000);
      int NewSize = 0;
      pdfOutput.Write("begincmap\n", ref NewSize);
      pdfOutput.Write("/CIDSystemInfo\n<< /Registry (Adobe)\n /Ordering (UCS)\n /Supplement 0\n>> def\n", ref NewSize);
      pdfOutput.Write("/CMapName /Adobe-Identity-UCS def\n", ref NewSize);
      pdfOutput.Write("/CMapType 2 def\n", ref NewSize);
      if (this.m_arrGlyphs.Count > 0)
      {
        pdfOutput.Write("1 begincodespacerange\n", ref NewSize);
        string objString1 = string.Format("<{0:x04}> <{1:x04}>\n", (object) this.m_arrGlyphs[0].m_nGlyph, (object) this.m_arrGlyphs[this.m_arrGlyphs.Count - 1].m_nGlyph);
        pdfOutput.Write(objString1, ref NewSize);
        pdfOutput.Write("endcodespacerange\n", ref NewSize);
        int num1 = 1;
        for (int index = 0; index < list.Count - 1; ++index)
        {
          if ((int) list[index].m_wcCode != (int) list[index + 1].m_wcCode - 1 || (int) list[index].m_nGlyph != (int) list[index + 1].m_nGlyph - 1)
            ++num1;
        }
        string objString2 = string.Format("{0} beginbfrange\n", (object) num1);
        pdfOutput.Write(objString2, ref NewSize);
        int index1 = 0;
        int num2 = (int) list[index1].m_nGlyph;
        int num3 = (int) list[index1].m_nGlyph;
        ushort num4 = (ushort) list[index1].m_wcCode;
        ushort num5 = (ushort) list[index1].m_wcCode;
        for (int index2 = 1; index2 < list.Count; ++index2)
        {
          if ((int) num5 != (int) list[index2].m_wcCode - 1 || num3 != (int) list[index2].m_nGlyph - 1)
          {
            string objString3 = string.Format("<{0:x04}> <{1:x04}> <{2:x04}>\n", (object) num2, (object) num3, (object) num4);
            pdfOutput.Write(objString3, ref NewSize);
            num2 = (int) list[index2].m_nGlyph;
            num4 = (ushort) list[index2].m_wcCode;
          }
          num3 = (int) list[index2].m_nGlyph;
          num5 = (ushort) list[index2].m_wcCode;
        }
        string objString4 = string.Format("<{0:x04}> <{1:x04}> <{2:x04}>\n", (object) num2, (object) num3, (object) num4);
        pdfOutput.Write(objString4, ref NewSize);
        pdfOutput.Write("endbfrange\n", ref NewSize);
      }
      pdfOutput.Write("endcmap\n", ref NewSize);
      pdfOutput.Write("CMapName currentdict /CMap defineresource pop\n", ref NewSize);
      this.m_pToUnicodeObj.m_objStream.Set((PdfStream) pdfOutput);
      this.m_pToUnicodeObj.AddInt("Length", pdfOutput.Length);
    }

    internal void GetCharInfo(char wcCode, out ushort pGlyph, out ushort pWidth)
    {
      pGlyph = (ushort) 0;
      if (this.m_glyphIdArray == null)
      {
        if (this.m_pWidths == null)
          AuxException.Throw("Text extent cannot be determined.", PdfErrors._ERROR_TEXTEXTENT);
        if ((int) wcCode < this.m_nFirstChar || (int) wcCode > this.m_nLastChar)
          AuxException.Throw(string.Format("Character {0} out of range (font \"{1}\").", (object) wcCode, (object) this.m_bstrFace), PdfErrors._ERROR_TEXTEXTENT);
        pWidth = this.m_pWidths[(int) wcCode];
      }
      else
      {
        GlyphInfo glyph = this.FindGlyph(wcCode);
        if (glyph != null)
        {
          pGlyph = glyph.m_nGlyph;
          pWidth = glyph.m_nWidth;
        }
        else
        {
          int index = 0;
          bool flag = false;
          while ((int) wcCode < (int) this.m_startCount[index] || (int) wcCode > (int) this.m_endCount[index])
          {
            if ((int) this.m_startCount[index] != (int) ushort.MaxValue)
            {
              ++index;
              if (index >= this.m_nCmapCount)
                AuxException.Throw("Fatal error during cmap lookup. The cmap table maybe corrupt.", PdfErrors._ERROR_BADCMAPTABLE);
            }
            else
              goto label_14;
          }
          flag = true;
label_14:
          ushort GlyphID = (ushort) 0;
          if (flag)
            GlyphID = (int) this.m_idRangeOffset[index] != 0 ? this.m_glyphIdArray[(int) this.m_idRangeOffset[index] / 2 + (int) wcCode - (int) this.m_startCount[index] - this.m_nCmapCount + index] : (ushort) (((int) this.m_idDelta[index] + (int) wcCode) % 65536);
          ushort Width = this.m_nDefaultWidth;
          if ((int) GlyphID >= 0 && (int) GlyphID < this.m_nHorMetricCount)
            Width = this.m_HorMetrics[(int) GlyphID].advanceWidth;
          pWidth = Width;
          pGlyph = GlyphID;
          this.AddGlyph(wcCode, GlyphID, Width);
        }
      }
    }

    internal void SetIndex()
    {
      int num = 0;
      string str;
      do
      {
        ++num;
        str = string.Format("F{0}", (object) num);
      }
      while (this.m_pDoc.m_listFontNames.GetObjectByName(str) != null);
      this.m_nID = num;
      this.m_szID = string.Format("F{0}", (object) this.m_nID);
      this.m_pDoc.m_listFontNames.Add((PdfObject) new PdfName(str, (string) null));
    }

    internal void CreateIndirectObject(PdfIndirectObj pResourceObj, PdfIndirectObj pFontDictObj)
    {
      PdfIndirectObj pdfIndirectObj1 = (PdfIndirectObj) null;
      bool flag = false;
      if (this.m_pFontObj != null)
        flag = true;
      else
        this.m_pFontObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectFont);
      if (pResourceObj != null)
      {
        PdfDict pdfDict1;
        if (pFontDictObj == null)
        {
          pdfDict1 = pResourceObj.m_nType != enumIndirectType.pdfIndirectResource ? pResourceObj.AddDict("Resources") : pResourceObj.m_objAttributes;
          pResourceObj.m_bModified = true;
        }
        else
        {
          pdfDict1 = pFontDictObj.m_objAttributes;
          pFontDictObj.m_bModified = true;
        }
        PdfDict pdfDict2 = pFontDictObj != null ? pFontDictObj.m_objAttributes : (PdfDict) pdfDict1.GetObjectByName("Font");
        if (pdfDict2 == null)
        {
          pdfDict2 = new PdfDict("Font");
          pdfDict1.Add((PdfObject) pdfDict2);
        }
        pdfDict2.Add(new PdfReference(this.m_szID, (PdfObject) this.m_pFontObj));
        this.m_pDoc.m_szFontID = this.m_szID;
      }
      if (flag)
        return;
      this.m_pFontObj.AddName("Type", "Font");
      this.m_pFontObj.AddName("Subtype", this.m_bstrType);
      this.m_pFontObj.AddName("BaseFont", this.m_bstrFontName);
      this.m_pFontObj.AddName("Name", this.m_szID);
      if (!this.m_bDingbatOrSymbol)
        this.m_pFontObj.AddName("Encoding", this.m_bstrEncoding);
      if (this.m_bstrPath == null)
        return;
      if (this.m_bstrType == "Type0")
      {
        pdfIndirectObj1 = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectFont);
        pdfIndirectObj1.AddName("Type", "Font");
        pdfIndirectObj1.AddName("Subtype", "CIDFontType2");
        pdfIndirectObj1.AddName("BaseFont", this.m_bstrFontName);
        this.m_pFontObj.AddArray("DescendantFonts").Add(new PdfReference((string) null, (PdfObject) pdfIndirectObj1));
        PdfIndirectObj pdfIndirectObj2 = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectCIDSystemInfo);
        pdfIndirectObj2.AddString("Registry", "Adobe");
        pdfIndirectObj2.AddString("Ordering", "Identity");
        pdfIndirectObj2.AddInt("Supplement", 0);
        pdfIndirectObj1.AddReference("CIDSystemInfo", (PdfObject) pdfIndirectObj2);
      }
      this.m_pFontDescrObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectFontDescriptor);
      if (this.m_bstrType == "Type0" && pdfIndirectObj1 != null)
        pdfIndirectObj1.AddReference("FontDescriptor", (PdfObject) this.m_pFontDescrObj);
      else
        this.m_pFontObj.AddReference("FontDescriptor", (PdfObject) this.m_pFontDescrObj);
      this.m_pFontDescrObj.AddName("Type", "FontDescriptor");
      this.m_pFontDescrObj.AddName("FontName", this.m_bstrFontName);
      PdfArray pdfArray1 = this.m_pFontDescrObj.AddArray("FontBBox");
      pdfArray1.Add((PdfObject) new PdfNumber((string) null, (double) this.m_rectFontBox.m_fLeft));
      pdfArray1.Add((PdfObject) new PdfNumber((string) null, (double) this.m_rectFontBox.m_fBottom));
      pdfArray1.Add((PdfObject) new PdfNumber((string) null, (double) this.m_rectFontBox.m_fRight));
      pdfArray1.Add((PdfObject) new PdfNumber((string) null, (double) this.m_rectFontBox.m_fTop));
      this.m_pFontDescrObj.AddInt("Ascent", this.m_nAscent);
      this.m_pFontDescrObj.AddInt("Descent", this.m_nDescent);
      this.m_pFontDescrObj.AddInt("CapHeight", (int) this.m_nCapHeight);
      this.m_pFontDescrObj.AddInt("ItalicAngle", this.m_nItalicAngle);
      this.m_pFontDescrObj.AddInt("StemV", 0);
      this.m_pFontDescrObj.AddInt("Flags", 262176);
      this.m_pFontFileObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectFontFile);
      this.m_pFontDescrObj.AddReference("FontFile2", (PdfObject) this.m_pFontFileObj);
      if (pdfIndirectObj1 == null)
        return;
      PdfArray pdfArray2 = new PdfArray((string) null);
      for (int index = 0; index < this.m_nHorMetricCount; ++index)
      {
        int num = (int) this.m_HorMetrics[index].advanceWidth * 1000 / this.m_nUnitsPerEm;
        pdfArray2.Add((PdfObject) new PdfNumber((string) null, (double) num));
      }
      PdfArray pdfArray3 = pdfIndirectObj1.AddArray("W");
      pdfArray2.m_nItemsPerLine = 20;
      pdfArray3.Add((PdfObject) new PdfNumber((string) null, 0.0));
      pdfArray3.Add((PdfObject) pdfArray2);
      if (this.m_nHorMetricCount >= 1 && this.m_nHorMetricCount <= 3)
        pdfIndirectObj1.AddInt("DW", (int) this.m_HorMetrics[0].advanceWidth * 1000 / this.m_nUnitsPerEm);
      this.m_pToUnicodeObj = this.m_pDoc.AddNewIndirectObject(enumIndirectType.pdfIndirectToUnicode);
      this.m_pFontObj.AddReference("ToUnicode", (PdfObject) this.m_pToUnicodeObj);
    }

    internal void UnicodeToGlyph(string Text, ref PdfStream stream, out float pWidth)
    {
      PdfString pdfString1 = new PdfString();
      uint num = 0U;
      int length = Text.Length;
      if (this.m_endCount == null)
      {
        PdfString pdfString2 = new PdfString((string) null, Text);
        PdfOutput pOutput = new PdfOutput(length + 10);
        pdfString2.WriteOut(pOutput);
        stream.Set((PdfStream) pOutput);
        for (int index = 0; index < length; ++index)
        {
          ushort pGlyph;
          ushort pWidth1;
          this.GetCharInfo(Text[index], out pGlyph, out pWidth1);
          num += (uint) pWidth1;
        }
        pWidth = (float) num / (float) this.m_nUnitsPerEm;
      }
      else
      {
        pdfString1.Alloc(2 * length);
        pdfString1.m_objMemStream.Position = 0L;
        for (int index = 0; index < length; ++index)
        {
          ushort pGlyph;
          ushort pWidth1;
          this.GetCharInfo(Text[index], out pGlyph, out pWidth1);
          pdfString1.m_objMemStream.WriteByte((byte) ((uint) pGlyph >> 8));
          pdfString1.m_objMemStream.WriteByte((byte) ((uint) pGlyph & (uint) byte.MaxValue));
          num += (uint) pWidth1;
        }
        PdfOutput pOutput = new PdfOutput(2 * length + 10);
        pdfString1.WriteOut(pOutput);
        stream.Set((PdfStream) pOutput);
        pWidth = (float) num / (float) this.m_nUnitsPerEm;
      }
    }

    internal void SetFace(string Face)
    {
      this.m_bstrFace = Face;
      int num1 = 0;
      int length = Face.Length;
      for (int index = 0; index < length; ++index)
      {
        if ((int) Face[index] == 32)
          ++num1;
      }
      char[] chArray1 = Face.ToCharArray();
      if (num1 > 0)
      {
        if (length > 12 && string.Compare(Face, length - 12, " Bold Italic", 0, 12, true) == 0)
        {
          chArray1[length - 12] = ',';
          --num1;
        }
        else if (length > 7 && string.Compare(Face, length - 7, " Italic", 0, 7, true) == 0)
        {
          chArray1[length - 7] = ',';
          --num1;
        }
        else if (length > 5 && string.Compare(Face, length - 5, " Bold", 0, 5, true) == 0)
        {
          chArray1[length - 5] = ',';
          --num1;
        }
        char[] chArray2 = new char[length - num1];
        int num2 = 0;
        for (int index = 0; index < chArray1.Length; ++index)
        {
          if ((int) chArray1[index] != 32)
            chArray2[num2++] = chArray1[index];
        }
        this.m_bstrFace = new string(chArray2);
      }
      StringBuilder stringBuilder = new StringBuilder("abcdef");
      Random random = new Random();
      for (int index = 0; index < 6; ++index)
        stringBuilder[index] = (char) (65 + random.Next(0, 9999) % 26);
      stringBuilder.Append("+");
      stringBuilder.Append(this.m_bstrFace);
      this.m_bstrFontName = ((object) stringBuilder).ToString();
    }

    public PdfRect GetTextExtent(string Text, float Size)
    {
      PdfStream stream = new PdfStream();
      float pWidth1 = 0.0f;
      this.CreateIndirectObject((PdfIndirectObj) null, (PdfIndirectObj) null);
      if (this.m_glyphIdArray != null)
      {
        this.UnicodeToGlyph(Text, ref stream, out pWidth1);
      }
      else
      {
        int length = Text.Length;
        for (int index = 0; index < length; ++index)
        {
          ushort pGlyph;
          ushort pWidth2;
          this.GetCharInfo(Text[index], out pGlyph, out pWidth2);
          pWidth1 += (float) pWidth2;
        }
        pWidth1 /= (float) this.m_nUnitsPerEm;
      }
      PdfRect pdfRect = new PdfRect();
      pdfRect.m_fRight = pWidth1 * Size;
      pdfRect.m_fTop = (float) this.m_nCapHeight / (float) this.m_nUnitsPerEm * Size;
      return pdfRect;
    }

    public float GetParagraphHeight(string Text, object Param)
    {
      PdfParam pParam = this.m_pManager.VariantToParam(Param);
      if (!pParam.IsSet("Width"))
        AuxException.Throw("Width parameter expected.", PdfErrors._ERROR_PARAMNOTSPECIFIED);
      float num = pParam.Number("Width");
      PdfParam pdfParam1 = new PdfParam();
      pdfParam1.Copy(pParam);
      pdfParam1.Add("expand=true");
      PdfParam pdfParam2 = this.m_pManager.CreateParam("rows=1; cols=1; height=5; cellpadding=0; cellspacing=0; border=0; cellborder=0");
      pdfParam2["Width"] = num;
      PdfTable table = this.m_pDoc.CreateTable((object) pdfParam2);
      table.Font = this;
      PdfCell pdfCell = table[1, 1];
      pdfCell.AddText(Text, (object) pdfParam1);
      return pdfCell.Height;
    }

    internal static void EncodeText(string Text, string strEncoding, out string pOut)
    {
      int codepage = 1252;
      if (strEncoding != null && strEncoding == "MacRomanEncoding")
        codepage = 10000;
      else if (strEncoding != null && strEncoding == "PDFDocEncoding")
      {
        byte[] bytes = new byte[Text.Length];
        for (int index1 = 0; index1 < bytes.Length; ++index1)
        {
          for (int index2 = 0; index2 < PdfFont.m_arrPDFEncodingArray.Length; ++index2)
          {
            if ((int) Text[index1] == (int) PdfFont.m_arrPDFEncodingArray[index2])
            {
              bytes[index1] = (byte) index2;
              break;
            }
          }
        }
        pOut = Encoding.UTF7.GetString(bytes);
        return;
      }
      byte[] bytes1 = Encoding.GetEncoding(codepage).GetBytes(Text);
      pOut = Encoding.UTF7.GetString(bytes1);
    }

    internal void AddCmapRangeEntry(PdfObject pBegin, PdfObject pEnd, PdfObject pCode)
    {
      if (pBegin.m_nType != enumType.pdfString || pEnd.m_nType != enumType.pdfString)
        return;
      int nFromCode = ((PdfString) pBegin).ToLong();
      int nToCode = ((PdfString) pEnd).ToLong();
      if (pCode.m_nType == enumType.pdfString)
      {
        int num = ((PdfString) pCode).ToLong();
        this.AddEntry(nFromCode, nToCode, (char) num, (string) null);
      }
      else
      {
        if (pCode.m_nType != enumType.pdfArray)
          return;
        PdfArray pdfArray = (PdfArray) pCode;
        for (int nIndex = 0; nIndex < pdfArray.Size; ++nIndex)
        {
          PdfObject @object = pdfArray.GetObject(nIndex);
          if (@object.m_nType == enumType.pdfString)
          {
            string @string = Encoding.BigEndianUnicode.GetString(((PdfStream) @object).ToBytes());
            this.AddEntry(nFromCode + nIndex, nFromCode + nIndex, char.MinValue, @string);
          }
        }
      }
    }

    internal void AddCmapCharEntry(PdfObject pBegin, PdfObject pCode)
    {
      if (pBegin.m_nType != enumType.pdfString || pCode.m_nType != enumType.pdfString)
        return;
      int num = ((PdfString) pBegin).ToLong();
      PdfString pdfString = (PdfString) pCode;
      int index = 0;
      while (index < pdfString.Length / 2)
      {
        this.AddEntry(num + index, num + index, (char) (((uint) pdfString[index] << 8) + (uint) pdfString[index + 1]), (string) null);
        index += 2;
      }
    }

    internal void AddEntry(int nFromCode, int nToCode, char nUnicode, string bstrValue)
    {
      MapEntry mapEntry = new MapEntry();
      mapEntry.m_nFromCode = nFromCode;
      mapEntry.m_nToCode = nToCode;
      mapEntry.m_wcUnicode = nUnicode;
      if (bstrValue != null)
        mapEntry.m_bstrValue = bstrValue;
      this.m_arrCmap.Add(mapEntry);
    }

    internal string ConvertToUnicode(PdfString objString, int nCodePage)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.m_arrCmap.Count == 0)
      {
        int nCodePage1 = nCodePage;
        if (nCodePage == 0 && this.m_bMacRomanEncoding)
          nCodePage1 = 10000;
        string str = objString.ToUnicode(nCodePage1);
        stringBuilder.Append(str);
        if (this.m_arrDifferences.Count > 0)
        {
          foreach (MapEntry mapEntry in this.m_arrDifferences)
          {
            for (int index = 0; index < objString.Length; ++index)
            {
              if ((int) objString[index] == mapEntry.m_nFromCode && index < stringBuilder.Length)
                stringBuilder[index] = mapEntry.m_wcUnicode;
            }
          }
        }
        return ((object) stringBuilder).ToString();
      }
      else
      {
        bool flag = false;
        int length;
        int num1;
        if (this.m_bstrType == "Type0")
        {
          length = objString.Length;
          num1 = 2;
          flag = true;
        }
        else
        {
          length = objString.Length;
          num1 = 1;
        }
        int index = 0;
        while (index < length)
        {
          int num2 = (int) objString[index];
          if (flag)
            num2 = (num2 << 8) + (int) objString[index + 1];
          foreach (MapEntry mapEntry in this.m_arrCmap)
          {
            if (num2 >= mapEntry.m_nFromCode && num2 <= mapEntry.m_nToCode)
            {
              if (mapEntry.m_bstrValue == null)
              {
                char ch = (char) ((int) mapEntry.m_wcUnicode + num2 - mapEntry.m_nFromCode);
                stringBuilder.Append(ch);
                break;
              }
              else
                stringBuilder.Append(mapEntry.m_bstrValue);
            }
          }
          index += num1;
        }
        return ((object) stringBuilder).ToString();
      }
    }

    internal void CopyEncodingInfo(PdfFont pAlreadyParsedFont)
    {
      this.m_bMacRomanEncoding = pAlreadyParsedFont.m_bMacRomanEncoding;
      foreach (MapEntry mapEntry1 in pAlreadyParsedFont.m_arrDifferences)
      {
        MapEntry mapEntry2 = new MapEntry();
        mapEntry2.m_nFromCode = mapEntry1.m_nFromCode;
        mapEntry2.m_wcUnicode = mapEntry1.m_wcUnicode;
        this.m_arrDifferences.Add(mapEntry2);
      }
    }

    internal void HandleDifferences(PdfArray pArray)
    {
      if (pArray == null)
        return;
      char ch = char.MinValue;
      int num1 = 0;
      for (int nIndex = 0; nIndex < pArray.Size; ++nIndex)
      {
        PdfObject @object = pArray.GetObject(nIndex);
        if (@object.m_nType == enumType.pdfNumber)
        {
          ch = (char) ((PdfNumber) @object).m_fValue;
          num1 = 0;
        }
        else if (@object.m_nType == enumType.pdfName)
        {
          ushort num2 = this.LookupGlyphName(((PdfName) @object).m_bstrName);
          if ((int) num2 != 0 && (int) num2 != (int) ch + num1)
          {
            MapEntry mapEntry = new MapEntry();
            mapEntry.m_nFromCode = (int) ch + num1;
            mapEntry.m_wcUnicode = (char) num2;
            this.m_arrDifferences.Add(mapEntry);
          }
          ++num1;
        }
      }
    }

    internal bool LookupDifferenceItem(char UnicodeChar, out byte pAnsiChar)
    {
      pAnsiChar = (byte) 0;
      foreach (MapEntry mapEntry in this.m_arrDifferences)
      {
        if ((int) UnicodeChar == (int) mapEntry.m_wcUnicode)
        {
          pAnsiChar = (byte) mapEntry.m_nFromCode;
          return true;
        }
      }
      return false;
    }

    internal ushort LookupGlyphName(string Name)
    {
      if (string.Compare(Name, ".notdef") == 0)
        return (ushort) 0;
      int num1 = 0;
      int num2 = PdfFont.m_arrGlyphNames.Length - 1;
      while (num1 <= num2)
      {
        int index = (num1 + num2) / 2;
        int num3 = string.Compare(Name, PdfFont.m_arrGlyphNames[index], StringComparison.Ordinal);
        if (num3 == 0)
          return PdfFont.m_arrGlyphCodes[index];
        if (num3 < 0)
          num2 = index - 1;
        else
          num1 = index + 1;
      }
      return (ushort) 0;
    }

    internal void CreateSubset()
    {
      if (this.m_pFontDescrObj == null || this.m_pFontFileObj == null)
        return;
      this.PopulateToUnicodeCmap();
      if (this.m_bBarcodeFont)
        this.m_pFontFileObj.m_objStream.Set(this.m_streamBarcodeFont);
      else if (this.m_mapTableOffset["loca"] == 0)
      {
        this.m_pFontFileObj.m_objStream.LoadFromFile(this.m_bstrPath);
      }
      else
      {
        this.AppendGlyphList();
        this.PopulateNewGlyphTable();
        this.PopulateNewLocaTable();
        this.AssembleFontSubset();
      }
      int length = this.m_pFontFileObj.m_objStream.Length;
      this.m_pFontFileObj.m_objStream.Encode(enumEncoding.PdfEncFlate);
      this.m_pFontFileObj.AddName("Filter", "FlateDecode");
      this.m_pFontFileObj.AddInt("Length", this.m_pFontFileObj.m_objStream.Length);
      this.m_pFontFileObj.AddInt("Length1", length);
    }

    internal void AssembleFontSubset()
    {
      int[] numArray = new int[21]
      {
        0,
        0,
        1,
        1,
        2,
        2,
        2,
        2,
        3,
        3,
        3,
        3,
        3,
        3,
        3,
        3,
        4,
        4,
        4,
        4,
        4
      };
      PdfStream pStream = this.m_pFontFileObj.m_objStream;
      int num1 = 0;
      int index1 = 2;
      for (int index2 = 0; index2 < 9; ++index2)
      {
        if (string.Compare(PdfFont.m_arrTableNames[index2], "glyf") != 0 && string.Compare(PdfFont.m_arrTableNames[index2], "loca") != 0)
        {
          int num2 = this.m_mapTableLength[PdfFont.m_arrTableNames[index2]];
          if (num2 != 0)
          {
            ++index1;
            num1 += num2 + 3 & -4;
          }
        }
      }
      int Size = num1 + this.m_mapTableLength["glyf"] + this.m_mapTableLength["loca"] + (16 * index1 + 12);
      pStream.Alloc(Size);
      this.WriteLong(pStream, 65536U);
      this.WriteShort(pStream, (ushort) index1);
      int num3 = numArray[index1];
      this.WriteShort(pStream, (ushort) ((1 << num3) * 16));
      this.WriteShort(pStream, (ushort) num3);
      this.WriteShort(pStream, (ushort) ((index1 - (1 << num3)) * 16));
      int num4 = pStream.Length + 16 * index1;
      for (int index2 = 0; index2 < 9; ++index2)
      {
        int num2 = this.m_mapTableLength[PdfFont.m_arrTableNames[index2]];
        if (num2 != 0)
        {
          pStream.Append(Encoding.UTF8.GetBytes(PdfFont.m_arrTableNames[index2]));
          this.WriteLong(pStream, (uint) this.m_mapTableChecksum[PdfFont.m_arrTableNames[index2]]);
          this.WriteLong(pStream, (uint) num4);
          if (string.Compare(PdfFont.m_arrTableNames[index2], "glyf") == 0)
          {
            this.WriteLong(pStream, (uint) this.m_dwGlyphTableRealSize);
            num4 += this.m_dwGlyphTableRealSize;
          }
          else if (string.Compare(PdfFont.m_arrTableNames[index2], "loca") == 0)
          {
            this.WriteLong(pStream, (uint) this.m_dwLocaTableRealSize);
            num4 += this.m_dwLocaTableRealSize;
          }
          else
          {
            this.WriteLong(pStream, (uint) num2);
            num4 += num2;
          }
        }
      }
      for (int index2 = 0; index2 < 9; ++index2)
      {
        if (this.m_mapTableLength[PdfFont.m_arrTableNames[index2]] != 0)
          pStream.Append(this.m_arrTables[index2]);
      }
    }

    internal void PopulateNewLocaTable()
    {
      this.m_arrTables[6] = new PdfStream();
      PdfStream pStream = this.m_arrTables[6];
      this.m_dwLocaTableRealSize = this.m_nLocaFormat != 0 ? this.m_nLocaLength * 4 : this.m_nLocaLength * 2;
      pStream.Alloc(this.m_dwLocaTableRealSize + 3 & -4);
      for (int index = 0; index < this.m_nLocaLength; ++index)
      {
        if (this.m_nLocaFormat == 0)
          this.WriteShort(pStream, (ushort) (this.m_locaNew[index] / 2U));
        else
          this.WriteLong(pStream, this.m_locaNew[index]);
      }
      this.m_mapTableChecksum["loca"] = pStream.Checksum();
    }

    internal void AppendGlyphList()
    {
      foreach (GlyphInfo glyphInfo in this.m_arrGlyphs)
        this.AddGlyphIDToList((int) glyphInfo.m_nGlyph);
      this.AddGlyphIDToList(0);
      foreach (GlyphInfo glyphInfo in this.m_arrGlyphs)
        this.CheckGlyphComposite((int) glyphInfo.m_nGlyph);
    }

    internal void CheckGlyphComposite(int nGlyph)
    {
      if (nGlyph < 0 || nGlyph > this.m_nLocaLength || (int) this.m_locaOffsets[nGlyph] == (int) this.m_locaOffsets[nGlyph + 1])
        return;
      int num1 = this.m_mapTableOffset["glyf"];
      PdfInput pdfInput = new PdfInput(this.m_bstrPath, this.m_pDoc, (string) null);
      int nPtr1 = num1 + (int) this.m_locaOffsets[nGlyph];
      short num2;
      pdfInput.ReadAndSwapShort(nPtr1, out num2);
      if ((int) num2 >= 0)
        return;
      int nPtr2 = nPtr1 + 10;
      while (true)
      {
        short num3;
        int nPtr3 = nPtr2 + pdfInput.ReadAndSwapShort(nPtr2, out num3);
        short num4;
        int num5 = nPtr3 + pdfInput.ReadAndSwapShort(nPtr3, out num4);
        this.AddGlyphIDToList((int) num4);
        if (((int) num3 & 32) != 0)
        {
          int num6 = ((int) num3 & 1) == 0 ? 2 : 4;
          if (((int) num3 & 8) != 0)
            num6 += 2;
          else if (((int) num3 & 64) != 0)
            num6 += 4;
          if (((int) num3 & 128) != 0)
            num6 += 8;
          nPtr2 = num5 + num6;
        }
        else
          break;
      }
    }

    internal void PopulateNewGlyphTable()
    {
      if (this.m_mapTableOffset["glyf"] == 0)
        AuxException.Throw("glyf table not found in the font.", PdfErrors._ERROR_GLYPH_FONT);
      this.m_arrTables[2] = new PdfStream();
      PdfStream pdfStream = this.m_arrTables[2];
      uint num1 = 0U;
      foreach (int index in this.m_arrGlyphIDs)
      {
        if (index >= 0 && index <= this.m_nLocaLength)
          num1 += this.m_locaOffsets[index + 1] - this.m_locaOffsets[index];
      }
      this.m_dwGlyphTableRealSize = (int) num1;
      uint num2 = (uint) ((ulong) (num1 + 3U) & 18446744073709551612UL);
      pdfStream.Alloc((int) num2);
      pdfStream.m_objMemStream.Position = 0L;
      this.m_locaNew = new uint[this.m_nLocaLength];
      int num3 = this.m_mapTableOffset["glyf"];
      PdfInput pdfInput = new PdfInput(this.m_bstrPath, this.m_pDoc, (string) null);
      uint num4 = 0U;
      int index1 = 0;
      for (int index2 = 0; index2 < this.m_nLocaLength; ++index2)
      {
        this.m_locaNew[index2] = num4;
        if (index1 < this.m_arrGlyphIDs.Count && this.m_arrGlyphIDs[index1] == index2)
        {
          ++index1;
          this.m_locaNew[index2] = num4;
          uint num5 = this.m_locaOffsets[index2 + 1] - this.m_locaOffsets[index2];
          byte[] numArray = new byte[num5];
          pdfInput.ReadBytes((int) ((long) num3 + (long) this.m_locaOffsets[index2]), numArray, (int) num5);
          num4 += num5;
          pdfStream.m_objMemStream.Write(numArray, 0, (int) num5);
        }
      }
      this.m_mapTableChecksum["glyf"] = pdfStream.Checksum();
    }

    internal void Parse(string Path, byte[] Stream)
    {
      if (this.m_bParsed)
        return;
      PdfInput Input = (PdfInput) null;
      try
      {
        Input = Stream != null ? new PdfInput(Stream, this.m_pDoc, (string) null) : new PdfInput(Path, this.m_pDoc, (string) null);
        this.ParseFontDirectory(Input);
        this.ParseHead(Input, this.m_mapTableOffset["head"], this.m_mapTableLength["head"]);
        this.ParseCmap(Input, this.m_mapTableOffset["cmap"], this.m_mapTableLength["cmap"]);
        this.ParseHhea(Input, this.m_mapTableOffset["hhea"], this.m_mapTableLength["hhea"]);
        this.ParseHmtx(Input, this.m_mapTableOffset["hmtx"], this.m_mapTableLength["hmtx"]);
        this.ParseOs2(Input, this.m_mapTableOffset["OS/2"], this.m_mapTableLength["OS/2"]);
        this.ParseCvt(Input, this.m_mapTableOffset["cvt "], this.m_mapTableLength["cvt "]);
        this.ParseFpgm(Input, this.m_mapTableOffset["fpgm"], this.m_mapTableLength["fpgm"]);
        this.ParseMaxp(Input, this.m_mapTableOffset["maxp"], this.m_mapTableLength["maxp"]);
        this.ParseLoca(Input, this.m_mapTableOffset["loca"], this.m_mapTableLength["loca"]);
        this.ParsePrep(Input, this.m_mapTableOffset["prep"], this.m_mapTableLength["prep"]);
        this.ParseName(Input, this.m_mapTableOffset["name"], this.m_mapTableLength["name"]);
        Input.Close();
        if (this.m_nHorMetricCount > 0 && this.m_HorMetrics == null)
          Input.Throw("Horizontal metrics data not found in this font.");
        if (Path != null)
          this.m_bstrPath = Path;
      }
      catch (Exception ex)
      {
        if (Input != null)
          Input.Close();
        throw ex;
      }
      this.m_bParsed = true;
    }

    internal void ParseName(PdfInput Input, int dwOffset, int dwLength)
    {
      if (dwOffset == 0)
        return;
      string str = (string) null;
      int nPtr1 = dwOffset;
      ushort num1;
      int nPtr2 = nPtr1 + Input.ReadAndSwapUshort(nPtr1, out num1);
      ushort num2;
      int nPtr3 = nPtr2 + Input.ReadAndSwapUshort(nPtr2, out num2);
      ushort num3;
      int nPtr4 = nPtr3 + Input.ReadAndSwapUshort(nPtr3, out num3);
      for (int index = 0; index < (int) num2; ++index)
      {
        ushort num4;
        int nPtr5 = nPtr4 + Input.ReadAndSwapUshort(nPtr4, out num4);
        ushort num5;
        int nPtr6 = nPtr5 + Input.ReadAndSwapUshort(nPtr5, out num5);
        ushort num6;
        int nPtr7 = nPtr6 + Input.ReadAndSwapUshort(nPtr6, out num6);
        ushort num7;
        int nPtr8 = nPtr7 + Input.ReadAndSwapUshort(nPtr7, out num7);
        ushort num8;
        int nPtr9 = nPtr8 + Input.ReadAndSwapUshort(nPtr8, out num8);
        ushort num9;
        nPtr4 = nPtr9 + Input.ReadAndSwapUshort(nPtr9, out num9);
        if (((int) num4 == 0 || (int) num4 == 3) && ((int) num7 == 1 || (int) num7 == 4))
        {
          PdfString pdfString = new PdfString();
          pdfString.m_bAnsi = false;
          pdfString.Append(new byte[2]
          {
            (byte) 254,
            byte.MaxValue
          });
          byte[] numArray = new byte[(int) num8];
          Input.ReadBytes(dwOffset + (int) num3 + (int) num9, numArray, (int) num8);
          pdfString.Append(numArray);
          if ((int) num7 == 1)
            this.m_bstrFamily = pdfString.ToString();
          else if ((int) num6 == 1033)
            str = pdfString.ToString();
          if (this.m_bstrFamily != null && str != null && (int) num6 == 1033)
            break;
        }
      }
      if (str == null)
        return;
      this.m_bstrFace = str;
      this.m_bstrFaceAsObtainedFromTTFFile = str;
    }

    internal void ParsePrep(PdfInput Input, int dwOffset, int dwLength)
    {
      if (dwOffset == 0)
        return;
      byte[] numArray = new byte[dwLength];
      Input.ReadBytes(dwOffset, numArray, dwLength);
      this.m_arrTables[8] = new PdfStream();
      this.m_arrTables[8].Set(numArray);
    }

    internal void ParseLoca(PdfInput Input, int dwOffset, int dwLength)
    {
      if (dwOffset == 0)
        return;
      byte[] pBuffer = new byte[dwLength];
      Input.ReadBytes(dwOffset, pBuffer, dwLength);
      if (this.m_nLocaFormat == 0)
      {
        this.m_locaOffsets = new uint[dwLength / 2];
        this.m_nLocaLength = dwLength / 2;
        for (int index = 0; index < dwLength / 2; ++index)
          this.m_locaOffsets[index] = (uint) (2 * (256 * (int) pBuffer[2 * index] + (int) pBuffer[2 * index + 1]));
      }
      else
      {
        this.m_locaOffsets = new uint[dwLength / 4];
        this.m_nLocaLength = dwLength / 4;
        for (int index = 0; index < dwLength / 4; ++index)
          this.m_locaOffsets[index] = (uint) (16777216 * (int) pBuffer[4 * index] + 65536 * (int) pBuffer[4 * index + 1] + 256 * (int) pBuffer[4 * index + 2]) + (uint) pBuffer[4 * index + 3];
      }
    }

    internal void ParseMaxp(PdfInput Input, int dwOffset, int dwLength)
    {
      if (dwOffset == 0)
        return;
      byte[] numArray = new byte[dwLength];
      Input.ReadBytes(dwOffset, numArray, dwLength);
      this.m_arrTables[7] = new PdfStream();
      this.m_arrTables[7].Set(numArray);
    }

    internal void ParseFpgm(PdfInput Input, int dwOffset, int dwLength)
    {
      if (dwOffset == 0)
        return;
      byte[] numArray = new byte[dwLength];
      Input.ReadBytes(dwOffset, numArray, dwLength);
      this.m_arrTables[1] = new PdfStream();
      this.m_arrTables[1].Set(numArray);
    }

    internal void ParseCvt(PdfInput Input, int dwOffset, int dwLength)
    {
      if (dwOffset == 0)
        return;
      byte[] numArray = new byte[dwLength];
      Input.ReadBytes(dwOffset, numArray, dwLength);
      this.m_arrTables[0] = new PdfStream();
      this.m_arrTables[0].Set(numArray);
    }

    internal void ParseOs2(PdfInput Input, int dwOffset, int dwLength)
    {
      if (dwOffset == 0)
        return;
      int nPtr1 = dwOffset;
      ushort num1;
      int nPtr2 = nPtr1 + Input.ReadAndSwapUshort(nPtr1, out num1);
      short num2;
      int nPtr3 = nPtr2 + Input.ReadAndSwapShort(nPtr2, out num2);
      ushort num3;
      int nPtr4 = nPtr3 + Input.ReadAndSwapUshort(nPtr3, out num3);
      ushort num4;
      int nPtr5 = nPtr4 + Input.ReadAndSwapUshort(nPtr4, out num4);
      ushort num5;
      int nPtr6 = nPtr5 + Input.ReadAndSwapUshort(nPtr5, out num5) + 74;
      short num6;
      int num7 = nPtr6 + Input.ReadAndSwapShort(nPtr6, out num6);
      this.m_nCapHeight = (uint) (((int) num1 > 1 ? (double) num6 : 0.7 * (double) this.m_nUnitsPerEm) * 1000.0 / (double) this.m_nUnitsPerEm);
      this.m_nEmbedding = (int) num5;
    }

    internal void ParseHmtx(PdfInput Input, int dwOffset, int dwLength)
    {
      if (dwOffset == 0)
        return;
      this.m_HorMetrics = new HorMetric[this.m_nHorMetricCount];
      for (int index = 0; index < this.m_nHorMetricCount; ++index)
        this.m_HorMetrics[index] = new HorMetric();
      int nPtr1 = dwOffset;
      for (int index = 0; index < this.m_nHorMetricCount; ++index)
      {
        int nPtr2 = nPtr1 + Input.ReadAndSwapUshort(nPtr1, out this.m_HorMetrics[index].advanceWidth);
        nPtr1 = nPtr2 + Input.ReadAndSwapShort(nPtr2, out this.m_HorMetrics[index].lsb);
      }
      int dwFrom = dwOffset;
      int num = 4;
      int length = dwLength > this.m_nHorMetricCount * num ? this.m_nHorMetricCount * num : dwLength;
      byte[] numArray = new byte[length];
      Input.ReadBytes(dwFrom, numArray, length);
      this.m_arrTables[5] = new PdfStream();
      this.m_arrTables[5].Set(new byte[dwLength]);
      this.m_arrTables[5].m_objMemStream.Position = 0L;
      this.m_arrTables[5].m_objMemStream.Write(numArray, 0, length);
    }

    internal void ParseHhea(PdfInput Input, int dwOffset, int dwLength)
    {
      if (dwOffset == 0)
        return;
      int nPtr1 = dwOffset;
      int num1;
      int nPtr2 = nPtr1 + Input.ReadAndSwapInt(nPtr1, out num1);
      short num2;
      int nPtr3 = nPtr2 + Input.ReadAndSwapShort(nPtr2, out num2);
      short num3;
      int nPtr4 = nPtr3 + Input.ReadAndSwapShort(nPtr3, out num3);
      short num4;
      int nPtr5 = nPtr4 + Input.ReadAndSwapShort(nPtr4, out num4);
      ushort num5;
      int nPtr6 = nPtr5 + Input.ReadAndSwapUshort(nPtr5, out num5);
      short num6;
      int nPtr7 = nPtr6 + Input.ReadAndSwapShort(nPtr6, out num6);
      short num7;
      int nPtr8 = nPtr7 + Input.ReadAndSwapShort(nPtr7, out num7);
      short num8;
      int nPtr9 = nPtr8 + Input.ReadAndSwapShort(nPtr8, out num8);
      short num9;
      int nPtr10 = nPtr9 + Input.ReadAndSwapShort(nPtr9, out num9);
      short num10;
      int nPtr11 = nPtr10 + Input.ReadAndSwapShort(nPtr10, out num10);
      short num11;
      int nPtr12 = nPtr11 + Input.ReadAndSwapShort(nPtr11, out num11);
      short num12;
      int nPtr13 = nPtr12 + Input.ReadAndSwapShort(nPtr12, out num12);
      short num13;
      int nPtr14 = nPtr13 + Input.ReadAndSwapShort(nPtr13, out num13);
      short num14;
      int nPtr15 = nPtr14 + Input.ReadAndSwapShort(nPtr14, out num14);
      short num15;
      int nPtr16 = nPtr15 + Input.ReadAndSwapShort(nPtr15, out num15);
      short num16;
      int nPtr17 = nPtr16 + Input.ReadAndSwapShort(nPtr16, out num16);
      ushort num17;
      int num18 = nPtr17 + Input.ReadAndSwapUshort(nPtr17, out num17);
      int dwFrom = dwOffset;
      byte[] numArray = new byte[dwLength];
      Input.ReadBytes(dwFrom, numArray, dwLength);
      this.m_arrTables[4] = new PdfStream();
      this.m_arrTables[4].Set(numArray);
      this.m_nHorMetricCount = (int) num17;
      this.m_nAscent = (int) ((double) num2 * 1000.0 / (double) this.m_nUnitsPerEm);
      this.m_nDescent = (int) ((double) num3 * 1000.0 / (double) this.m_nUnitsPerEm);
      this.m_nDefaultWidth = num5;
      this.m_nLineSpacing = (int) ((double) (this.m_nAscent - this.m_nDescent) + (double) num4 * 1000.0 / (double) this.m_nUnitsPerEm);
    }

    private void ParseUnicodeTable(PdfInput Input, int dwOffset)
    {
      this.m_bSymbolFont = false;
      int nPtr1 = dwOffset;
      ushort num1;
      int nPtr2 = nPtr1 + Input.ReadAndSwapUshort(nPtr1, out num1);
      ushort num2;
      int nPtr3 = nPtr2 + Input.ReadAndSwapUshort(nPtr2, out num2);
      ushort num3;
      int nPtr4 = nPtr3 + Input.ReadAndSwapUshort(nPtr3, out num3);
      ushort num4;
      int nPtr5 = nPtr4 + Input.ReadAndSwapUshort(nPtr4, out num4);
      ushort num5;
      int nPtr6 = nPtr5 + Input.ReadAndSwapUshort(nPtr5, out num5);
      ushort num6;
      int nPtr7 = nPtr6 + Input.ReadAndSwapUshort(nPtr6, out num6);
      ushort num7;
      int nPtr8 = nPtr7 + Input.ReadAndSwapUshort(nPtr7, out num7);
      int num8 = (int) num4 / 2;
      this.m_endCount = new ushort[(int) num4];
      this.m_startCount = new ushort[(int) num4];
      this.m_idDelta = new ushort[(int) num4];
      this.m_idRangeOffset = new ushort[(int) num4];
      this.m_nCmapCount = (int) num4 / 2;
      for (int index = 0; index < num8; ++index)
        nPtr8 += Input.ReadAndSwapUshort(nPtr8, out this.m_endCount[index]);
      int nPtr9 = nPtr8 + 2;
      for (int index = 0; index < num8; ++index)
        nPtr9 += Input.ReadAndSwapUshort(nPtr9, out this.m_startCount[index]);
      for (int index = 0; index < num8; ++index)
        nPtr9 += Input.ReadAndSwapUshort(nPtr9, out this.m_idDelta[index]);
      for (int index = 0; index < num8; ++index)
        nPtr9 += Input.ReadAndSwapUshort(nPtr9, out this.m_idRangeOffset[index]);
      int length = ((int) num2 - 8 * num8 - 16) / 2;
      this.m_glyphIdArray = new ushort[length];
      for (int index = 0; index < length; ++index)
        nPtr9 += Input.ReadAndSwapUshort(nPtr9, out this.m_glyphIdArray[index]);
    }

    private void ParseSymbolTable(PdfInput Input, int dwOffset)
    {
      ushort num;
      Input.ReadAndSwapUshort(dwOffset, out num);
      if ((int) num != 4 || this.m_endCount != null)
        return;
      this.ParseUnicodeTable(Input, dwOffset);
      this.m_bSymbolFont = true;
    }

    private void ParseCmap(PdfInput Input, int dwOffset, int dwLength)
    {
      if (dwOffset == 0 || dwLength == 0)
        return;
      int nPtr1 = dwOffset;
      ushort num1;
      int nPtr2 = nPtr1 + Input.ReadAndSwapUshort(nPtr1, out num1);
      ushort num2;
      int nPtr3 = nPtr2 + Input.ReadAndSwapUshort(nPtr2, out num2);
      for (int index = 0; index < (int) num2; ++index)
      {
        ushort num3;
        int nPtr4 = nPtr3 + Input.ReadAndSwapUshort(nPtr3, out num3);
        ushort num4;
        int nPtr5 = nPtr4 + Input.ReadAndSwapUshort(nPtr4, out num4);
        uint num5;
        nPtr3 = nPtr5 + Input.ReadAndSwapUint(nPtr5, out num5);
        if ((int) num3 == 3 && (int) num4 == 1)
          this.ParseUnicodeTable(Input, (int) ((long) dwOffset + (long) num5));
        if ((int) num3 == 3 && (int) num4 == 0)
          this.ParseSymbolTable(Input, (int) ((long) dwOffset + (long) num5));
      }
    }

    internal void ParseHead(PdfInput Input, int dwOffset, int dwLength)
    {
      if (dwOffset == 0)
        return;
      int dwFrom = dwOffset;
      byte[] numArray = new byte[dwLength];
      Input.ReadBytes(dwFrom, numArray, dwLength);
      this.m_arrTables[3] = new PdfStream();
      this.m_arrTables[3].Set(numArray);
      int nPtr1 = dwOffset;
      int num1;
      int nPtr2 = nPtr1 + Input.ReadAndSwapInt(nPtr1, out num1);
      int num2;
      int nPtr3 = nPtr2 + Input.ReadAndSwapInt(nPtr2, out num2);
      uint num3;
      int nPtr4 = nPtr3 + Input.ReadAndSwapUint(nPtr3, out num3);
      uint num4;
      int nPtr5 = nPtr4 + Input.ReadAndSwapUint(nPtr4, out num4);
      ushort num5;
      int nPtr6 = nPtr5 + Input.ReadAndSwapUshort(nPtr5, out num5);
      ushort num6;
      int nPtr7 = nPtr6 + Input.ReadAndSwapUshort(nPtr6, out num6);
      int num7;
      int nPtr8 = nPtr7 + Input.ReadAndSwapInt(nPtr7, out num7);
      int num8;
      int nPtr9 = nPtr8 + Input.ReadAndSwapInt(nPtr8, out num8);
      int num9;
      int nPtr10 = nPtr9 + Input.ReadAndSwapInt(nPtr9, out num9);
      int num10;
      int nPtr11 = nPtr10 + Input.ReadAndSwapInt(nPtr10, out num10);
      short num11;
      int nPtr12 = nPtr11 + Input.ReadAndSwapShort(nPtr11, out num11);
      short num12;
      int nPtr13 = nPtr12 + Input.ReadAndSwapShort(nPtr12, out num12);
      short num13;
      int nPtr14 = nPtr13 + Input.ReadAndSwapShort(nPtr13, out num13);
      short num14;
      int nPtr15 = nPtr14 + Input.ReadAndSwapShort(nPtr14, out num14);
      ushort num15;
      int nPtr16 = nPtr15 + Input.ReadAndSwapUshort(nPtr15, out num15);
      ushort num16;
      int nPtr17 = nPtr16 + Input.ReadAndSwapUshort(nPtr16, out num16);
      short num17;
      int nPtr18 = nPtr17 + Input.ReadAndSwapShort(nPtr17, out num17);
      short num18;
      int nPtr19 = nPtr18 + Input.ReadAndSwapShort(nPtr18, out num18);
      short num19;
      int num20 = nPtr19 + Input.ReadAndSwapShort(nPtr19, out num19);
      this.m_nUnitsPerEm = (int) num6;
      this.m_nLocaFormat = (int) num18;
      this.m_nMacStyle = num15;
      if (this.m_nUnitsPerEm <= 0)
        return;
      this.m_rectFontBox.m_fLeft = (float) ((int) num11 * 1000 / this.m_nUnitsPerEm);
      this.m_rectFontBox.m_fRight = (float) ((int) num13 * 1000 / this.m_nUnitsPerEm);
      this.m_rectFontBox.m_fBottom = (float) ((int) num12 * 1000 / this.m_nUnitsPerEm);
      this.m_rectFontBox.m_fTop = (float) ((int) num14 * 1000 / this.m_nUnitsPerEm);
      this.m_nVerticalExtent = (int) ((double) this.m_rectFontBox.m_fTop - (double) this.m_rectFontBox.m_fBottom);
    }

    internal void ParseFontDirectory(PdfInput Input)
    {
      int nPtr1 = 0;
      uint num1;
      int nPtr2 = nPtr1 + Input.ReadAndSwapUint(nPtr1, out num1);
      ushort num2;
      int nPtr3 = nPtr2 + Input.ReadAndSwapUshort(nPtr2, out num2);
      ushort num3;
      int nPtr4 = nPtr3 + Input.ReadAndSwapUshort(nPtr3, out num3);
      ushort num4;
      int nPtr5 = nPtr4 + Input.ReadAndSwapUshort(nPtr4, out num4);
      ushort num5;
      int dwFrom = nPtr5 + Input.ReadAndSwapUshort(nPtr5, out num5);
      if ((int) num1 != 65536 && (int) num1 != 1330926671)
        Input.Throw("This is not a valid TrueType or OpenType font file.");
      if ((int) num2 > 100)
        Input.Throw("The table number is invalid.");
      byte[] numArray = new byte[4];
      for (int index = 0; index < (int) num2; ++index)
      {
        int nPtr6 = dwFrom + Input.ReadBytes(dwFrom, numArray, 4);
        uint num6;
        int nPtr7 = nPtr6 + Input.ReadAndSwapUint(nPtr6, out num6);
        uint num7;
        int nPtr8 = nPtr7 + Input.ReadAndSwapUint(nPtr7, out num7);
        uint num8;
        dwFrom = nPtr8 + Input.ReadAndSwapUint(nPtr8, out num8);
        string @string = Encoding.UTF8.GetString(numArray);
        this.m_mapTableLength[@string] = (int) num8;
        this.m_mapTableOffset[@string] = (int) num7;
        this.m_mapTableChecksum[@string] = (int) num6;
      }
    }

    private void WriteShort(PdfStream pStream, ushort shortValue)
    {
      byte[] bytes = BitConverter.GetBytes(shortValue);
      byte[] Buffer = new byte[2]
      {
        bytes[1],
        bytes[0]
      };
      pStream.Append(Buffer);
    }

    private void WriteLong(PdfStream pStream, uint longValue)
    {
      byte[] bytes = BitConverter.GetBytes(longValue);
      byte[] Buffer = new byte[4]
      {
        bytes[3],
        bytes[2],
        bytes[1],
        bytes[0]
      };
      pStream.Append(Buffer);
    }

    private void AddGlyphIDToList(int nGlyph)
    {
      int index = 0;
      foreach (int num in this.m_arrGlyphIDs)
      {
        if (nGlyph == num)
          return;
        if (nGlyph < num)
        {
          this.m_arrGlyphIDs.Insert(index, nGlyph);
          return;
        }
        else
          ++index;
      }
      this.m_arrGlyphIDs.Add(nGlyph);
    }

    internal void ParseToDetermineBoldItalic(string Path)
    {
      PdfInput Input = (PdfInput) null;
      try
      {
        Input = new PdfInput(Path, this.m_pDoc, (string) null);
        this.ParseFontDirectory(Input);
        this.ParseHead(Input, this.m_mapTableOffset["head"], this.m_mapTableLength["head"]);
        this.ParseName(Input, this.m_mapTableOffset["name"], this.m_mapTableLength["name"]);
        this.m_bParsed = true;
      }
      catch (Exception ex)
      {
      }
      if (Input == null)
        return;
      Input.Close();
    }
  }
}
