// Type: Persits.PDF.HtmlCSS
// Assembly: Persits.PDF, Version=2.5.0.19989, Culture=neutral, PublicKeyToken=b8d8c63f1ff142a8
// Assembly location: C:\Users\hernan\Desktop\asppdf_net\Samples\Bin\Persits.PDF.dll

using System;
using System.Collections.Generic;
using System.Text;

namespace Persits.PDF
{
  internal class HtmlCSS
  {
    private const int MAX_SYMBOL_LENGTH = 80;
    public HtmlTreeItem m_pRoot;
    public HtmlTreeItem m_pCurrentLeaf;
    public HtmlTreeItem m_pBackupLeaf;
    public HtmlManager m_pHtmlManager;
    public bool m_bStateChanged;
    public List<HtmlCSSRule> m_arrRules;
    public HtmlCSSRule m_pCurrentRule;
    public int m_nHyperLinkColorRuleNumber;
    public uint m_dwFlags;
    public float m_fSize;
    public string m_bstrFont;
    public string m_bstrHref;
    public AuxRGB m_rgbColor;
    public AuxRGB m_rgbBackgroundColor;
    public CssProperties m_nCapitalization;
    public float m_fLineHeight;
    public bool m_bLineHeightPercent;
    public bool m_bDisplayNone;
    private List<string> m_arrURLs;
    private char[] m_pCSS;
    private int m_nSize;
    private int m_nPtr;
    private char m_c;
    private CssSymbol m_nNextSymbol;
    private StringBuilder m_szSymbol;
    public string m_bstrCurrentRelativePath;
    private List<string> m_arrRelativePathStack;
    private int m_nSavePtr;
    private char m_cSaveChar;
    internal static CSSNames[] m_arrPropNames;
    internal static CSSNames[] m_arrPropValues;
    internal static CSSRegistry[] m_arrPropRegistry;
    internal static HtmlSymbolMap[] SymbolMap;

    static HtmlCSS()
    {
      HtmlCSS.m_arrPropNames = new CSSNames[68]
      {
        new CSSNames(CssProperties.BACKGROUND, "background"),
        new CSSNames(CssProperties.BACKGROUND_ATTACHMENT, "background-attachment"),
        new CSSNames(CssProperties.BACKGROUND_COLOR, "background-color"),
        new CSSNames(CssProperties.BACKGROUND_IMAGE, "background-image"),
        new CSSNames(CssProperties.BACKGROUND_POSITION, "background-position"),
        new CSSNames(CssProperties.BACKGROUND_REPEAT, "background-repeat"),
        new CSSNames(CssProperties.BORDER, "border"),
        new CSSNames(CssProperties.BORDER_BOTTOM, "border-bottom"),
        new CSSNames(CssProperties.BORDER_BOTTOM_COLOR, "border-bottom-color"),
        new CSSNames(CssProperties.BORDER_BOTTOM_STYLE, "border-bottom-style"),
        new CSSNames(CssProperties.BORDER_BOTTOM_WIDTH, "border-bottom-width"),
        new CSSNames(CssProperties.BORDER_COLLAPSE, "border-collapse"),
        new CSSNames(CssProperties.BORDER_COLOR, "border-color"),
        new CSSNames(CssProperties.BORDER_LEFT, "border-left"),
        new CSSNames(CssProperties.BORDER_LEFT_COLOR, "border-left-color"),
        new CSSNames(CssProperties.BORDER_LEFT_STYLE, "border-left-style"),
        new CSSNames(CssProperties.BORDER_LEFT_WIDTH, "border-left-width"),
        new CSSNames(CssProperties.BORDER_RIGHT, "border-right"),
        new CSSNames(CssProperties.BORDER_RIGHT_COLOR, "border-right-color"),
        new CSSNames(CssProperties.BORDER_RIGHT_STYLE, "border-right-style"),
        new CSSNames(CssProperties.BORDER_RIGHT_WIDTH, "border-right-width"),
        new CSSNames(CssProperties.BORDER_SPACING, "border-spacing"),
        new CSSNames(CssProperties.BORDER_STYLE, "border-style"),
        new CSSNames(CssProperties.BORDER_TOP, "border-top"),
        new CSSNames(CssProperties.BORDER_TOP_COLOR, "border-top-color"),
        new CSSNames(CssProperties.BORDER_TOP_STYLE, "border-top-style"),
        new CSSNames(CssProperties.BORDER_TOP_WIDTH, "border-top-width"),
        new CSSNames(CssProperties.BORDER_WIDTH, "border-width"),
        new CSSNames(CssProperties.CLEAR, "clear"),
        new CSSNames(CssProperties.COLOR, "color"),
        new CSSNames(CssProperties.DISPLAY, "display"),
        new CSSNames(CssProperties.FLOAT, "float"),
        new CSSNames(CssProperties.FONT, "font"),
        new CSSNames(CssProperties.FONT_FAMILY, "font-family"),
        new CSSNames(CssProperties.FONT_SIZE, "font-size"),
        new CSSNames(CssProperties.FONT_STYLE, "font-style"),
        new CSSNames(CssProperties.FONT_VARIANT, "font-variant"),
        new CSSNames(CssProperties.FONT_WEIGHT, "font-weight"),
        new CSSNames(CssProperties.HEIGHT, "height"),
        new CSSNames(CssProperties.LEFT, "left"),
        new CSSNames(CssProperties.LETTER_SPACING, "letter-spacing"),
        new CSSNames(CssProperties.LINE_HEIGHT, "line-height"),
        new CSSNames(CssProperties.LIST_STYLE, "list-style"),
        new CSSNames(CssProperties.LIST_STYLE_IMAGE, "list-style-image"),
        new CSSNames(CssProperties.LIST_STYLE_POSITION, "list-style-position"),
        new CSSNames(CssProperties.LIST_STYLE_TYPE, "list-style-type"),
        new CSSNames(CssProperties.MARGIN, "margin"),
        new CSSNames(CssProperties.MARGIN_BOTTOM, "margin-bottom"),
        new CSSNames(CssProperties.MARGIN_LEFT, "margin-left"),
        new CSSNames(CssProperties.MARGIN_RIGHT, "margin-right"),
        new CSSNames(CssProperties.MARGIN_TOP, "margin-top"),
        new CSSNames(CssProperties.PADDING, "padding"),
        new CSSNames(CssProperties.PADDING_BOTTOM, "padding-bottom"),
        new CSSNames(CssProperties.PADDING_LEFT, "padding-left"),
        new CSSNames(CssProperties.PADDING_RIGHT, "padding-right"),
        new CSSNames(CssProperties.PADDING_TOP, "padding-top"),
        new CSSNames(CssProperties.PAGE_BREAK_AFTER, "page-break-after"),
        new CSSNames(CssProperties.PAGE_BREAK_BEFORE, "page-break-before"),
        new CSSNames(CssProperties.POSITION, "position"),
        new CSSNames(CssProperties.TEXT_ALIGN, "text-align"),
        new CSSNames(CssProperties.TEXT_DECORATION, "text-decoration"),
        new CSSNames(CssProperties.TEXT_INDENT, "text-indent"),
        new CSSNames(CssProperties.TEXT_TRANSFORM, "text-transform"),
        new CSSNames(CssProperties.TOP, "top"),
        new CSSNames(CssProperties.VERTICAL_ALIGN, "vertical-align"),
        new CSSNames(CssProperties.WHITE_SPACE, "white-space"),
        new CSSNames(CssProperties.WIDTH, "width"),
        new CSSNames(CssProperties.WORD_SPACING, "word-spacing")
      };
      HtmlCSS.m_arrPropValues = new CSSNames[78]
      {
        new CSSNames(CssProperties.V_ABSOLUTE, "absolute"),
        new CSSNames(CssProperties.V_ALWAYS, "always"),
        new CSSNames(CssProperties.V_AUTO, "auto"),
        new CSSNames(CssProperties.V_BASELINE, "baseline"),
        new CSSNames(CssProperties.V_BLOCK, "block"),
        new CSSNames(CssProperties.V_BOLD, "bold"),
        new CSSNames(CssProperties.V_BOLDER, "bolder"),
        new CSSNames(CssProperties.V_BOTH, "both"),
        new CSSNames(CssProperties.V_BOTTOM, "bottom"),
        new CSSNames(CssProperties.V_CAPITALIZE, "capitalize"),
        new CSSNames(CssProperties.V_CENTER, "center"),
        new CSSNames(CssProperties.V_CIRCLE, "circle"),
        new CSSNames(CssProperties.V_COLLAPSE, "collapse"),
        new CSSNames(CssProperties.V_DASHED, "dashed"),
        new CSSNames(CssProperties.V_DECIMAL, "decimal"),
        new CSSNames(CssProperties.V_DEMI_BOLD, "demi-bold"),
        new CSSNames(CssProperties.V_DEMI_LIGHT, "demi-light"),
        new CSSNames(CssProperties.V_DISC, "disc"),
        new CSSNames(CssProperties.V_DOTTED, "dotted"),
        new CSSNames(CssProperties.V_DOUBLE, "double"),
        new CSSNames(CssProperties.V_EXTRA_BOLD, "extra-bold"),
        new CSSNames(CssProperties.V_EXTRA_LIGHT, "extra-light"),
        new CSSNames(CssProperties.V_FIXED, "fixed"),
        new CSSNames(CssProperties.V_GROOVE, "groove"),
        new CSSNames(CssProperties.V_INLINE, "inline"),
        new CSSNames(CssProperties.V_INSET, "inset"),
        new CSSNames(CssProperties.V_INSIDE, "inside"),
        new CSSNames(CssProperties.V_ITALIC, "italic"),
        new CSSNames(CssProperties.V_JUSTIFY, "justify"),
        new CSSNames(CssProperties.V_LARGE, "large"),
        new CSSNames(CssProperties.V_LARGER, "larger"),
        new CSSNames(CssProperties.V_LEFT, "left"),
        new CSSNames(CssProperties.V_LIGHT, "light"),
        new CSSNames(CssProperties.V_LIGHTER, "lighter"),
        new CSSNames(CssProperties.V_LINE_THROUGH, "line-through"),
        new CSSNames(CssProperties.V_LOWER, "lower"),
        new CSSNames(CssProperties.V_LOWER_ALPHA, "lower-alpha"),
        new CSSNames(CssProperties.V_LOWER_ROMAN, "lower-roman"),
        new CSSNames(CssProperties.V_LOWERCASE, "lowercase"),
        new CSSNames(CssProperties.V_MEDIUM, "medium"),
        new CSSNames(CssProperties.V_MIDDLE, "middle"),
        new CSSNames(CssProperties.V_NO_REPEAT, "no-repeat"),
        new CSSNames(CssProperties.V_NONE, "none"),
        new CSSNames(CssProperties.V_NORMAL, "normal"),
        new CSSNames(CssProperties.V_NOWRAP, "nowrap"),
        new CSSNames(CssProperties.V_OBLIQUE, "oblique"),
        new CSSNames(CssProperties.V_OUTSET, "outset"),
        new CSSNames(CssProperties.V_OUTSIDE, "outside"),
        new CSSNames(CssProperties.V_OVERLINE, "overline"),
        new CSSNames(CssProperties.V_PRE, "pre"),
        new CSSNames(CssProperties.V_REPEAT, "repeat"),
        new CSSNames(CssProperties.V_REPEAT_X, "repeat-x"),
        new CSSNames(CssProperties.V_REPEAT_Y, "repeat-y"),
        new CSSNames(CssProperties.V_RIDGE, "ridge"),
        new CSSNames(CssProperties.V_RIGHT, "right"),
        new CSSNames(CssProperties.V_SCROLL, "scroll"),
        new CSSNames(CssProperties.V_SEPARATE, "separate"),
        new CSSNames(CssProperties.V_SMALL, "small"),
        new CSSNames(CssProperties.V_SMALL_CAPS, "small-caps"),
        new CSSNames(CssProperties.V_SMALLER, "smaller"),
        new CSSNames(CssProperties.V_SOLID, "solid"),
        new CSSNames(CssProperties.V_SQUARE, "square"),
        new CSSNames(CssProperties.V_STATIC, "static"),
        new CSSNames(CssProperties.V_SUB, "sub"),
        new CSSNames(CssProperties.V_SUPER, "super"),
        new CSSNames(CssProperties.V_TEXT_BOTTOM, "text-bottom"),
        new CSSNames(CssProperties.V_TEXT_TOP, "text-top"),
        new CSSNames(CssProperties.V_THICK, "thick"),
        new CSSNames(CssProperties.V_THIN, "thin"),
        new CSSNames(CssProperties.V_TOP, "top"),
        new CSSNames(CssProperties.V_UNDERLINE, "underline"),
        new CSSNames(CssProperties.V_UPPER_ALPHA, "upper-alpha"),
        new CSSNames(CssProperties.V_UPPER_ROMAN, "upper-roman"),
        new CSSNames(CssProperties.V_UPPERCASE, "uppercase"),
        new CSSNames(CssProperties.V_X_LARGE, "x-large"),
        new CSSNames(CssProperties.V_X_SMALL, "x-small"),
        new CSSNames(CssProperties.V_XX_LARGE, "xx-large"),
        new CSSNames(CssProperties.V_XX_SMALL, "xx-small")
      };
      HtmlCSS.m_arrPropRegistry = new CSSRegistry[69]
      {
        new CSSRegistry(CssProperties.FONT_FAMILY, CssPropertyType.PROP_STRING, new CssProperties[1]
        {
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.FONT_SIZE, CssPropertyType.PROP_NUMBER, new CssProperties[10]
        {
          CssProperties.V_XX_SMALL,
          CssProperties.V_X_SMALL,
          CssProperties.V_SMALL,
          CssProperties.V_MEDIUM,
          CssProperties.V_LARGE,
          CssProperties.V_X_LARGE,
          CssProperties.V_XX_LARGE,
          CssProperties.V_SMALLER,
          CssProperties.V_LARGER,
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.FONT_STYLE, CssPropertyType.PROP_ENUM, new CssProperties[4]
        {
          CssProperties.V_NORMAL,
          CssProperties.V_ITALIC,
          CssProperties.V_OBLIQUE,
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.FONT_WEIGHT, CssPropertyType.PROP_NUMBER, new CssProperties[12]
        {
          CssProperties.V_NORMAL,
          CssProperties.V_BOLD,
          CssProperties.V_BOLDER,
          CssProperties.V_LIGHTER,
          CssProperties.V_EXTRA_LIGHT,
          CssProperties.V_LIGHT,
          CssProperties.V_DEMI_LIGHT,
          CssProperties.V_MEDIUM,
          CssProperties.V_DEMI_BOLD,
          CssProperties.V_BOLD,
          CssProperties.V_EXTRA_BOLD,
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.FONT_VARIANT, CssPropertyType.PROP_ENUM, new CssProperties[3]
        {
          CssProperties.V_NORMAL,
          CssProperties.V_SMALL_CAPS,
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.FONT, CssPropertyType.PROP_SHORTCUT, new CssProperties[7]
        {
          CssProperties.FONT_STYLE,
          CssProperties.FONT_VARIANT,
          CssProperties.FONT_WEIGHT,
          CssProperties.FONT_SIZE,
          CssProperties.LINE_HEIGHT,
          CssProperties.FONT_FAMILY,
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.TEXT_TRANSFORM, CssPropertyType.PROP_ENUM, new CssProperties[5]
        {
          CssProperties.V_CAPITALIZE,
          CssProperties.V_UPPERCASE,
          CssProperties.V_LOWERCASE,
          CssProperties.V_NONE,
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.TEXT_DECORATION, CssPropertyType.PROP_ENUM, new CssProperties[5]
        {
          CssProperties.V_LINE_THROUGH,
          CssProperties.V_OVERLINE,
          CssProperties.V_UNDERLINE,
          CssProperties.V_NONE,
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.WORD_SPACING, CssPropertyType.PROP_NUMBER, new CssProperties[1]
        {
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.LETTER_SPACING, CssPropertyType.PROP_NUMBER, new CssProperties[2]
        {
          CssProperties.V_NORMAL,
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.VERTICAL_ALIGN, CssPropertyType.PROP_ENUM, new CssProperties[9]
        {
          CssProperties.V_BASELINE,
          CssProperties.V_SUB,
          CssProperties.V_SUPER,
          CssProperties.V_TOP,
          CssProperties.V_TEXT_TOP,
          CssProperties.V_MIDDLE,
          CssProperties.V_BOTTOM,
          CssProperties.V_TEXT_BOTTOM,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.TEXT_ALIGN, CssPropertyType.PROP_ENUM, new CssProperties[5]
        {
          CssProperties.V_LEFT,
          CssProperties.V_RIGHT,
          CssProperties.V_CENTER,
          CssProperties.V_JUSTIFY,
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.TEXT_INDENT, CssPropertyType.PROP_NUMBER, new CssProperties[1]
        {
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.LEFT, CssPropertyType.PROP_NUMBER, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.TOP, CssPropertyType.PROP_NUMBER, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.LINE_HEIGHT, CssPropertyType.PROP_NUMBER, new CssProperties[1]
        {
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.WHITE_SPACE, CssPropertyType.PROP_ENUM, new CssProperties[4]
        {
          CssProperties.V_NORMAL,
          CssProperties.V_PRE,
          CssProperties.V_NOWRAP,
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.LIST_STYLE_TYPE, CssPropertyType.PROP_ENUM, new CssProperties[10]
        {
          CssProperties.V_DECIMAL,
          CssProperties.V_LOWER_ROMAN,
          CssProperties.V_UPPER_ROMAN,
          CssProperties.V_LOWER_ALPHA,
          CssProperties.V_UPPER_ALPHA,
          CssProperties.V_NONE,
          CssProperties.V_CIRCLE,
          CssProperties.V_SQUARE,
          CssProperties.V_DISC,
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.LIST_STYLE_IMAGE, CssPropertyType.PROP_IMAGE, new CssProperties[1]
        {
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.LIST_STYLE_POSITION, CssPropertyType.PROP_ENUM, new CssProperties[3]
        {
          CssProperties.V_INSIDE,
          CssProperties.V_OUTSIDE,
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.LIST_STYLE, CssPropertyType.PROP_SHORTCUT, new CssProperties[4]
        {
          CssProperties.LIST_STYLE_TYPE,
          CssProperties.LIST_STYLE_IMAGE,
          CssProperties.LIST_STYLE_POSITION,
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.COLOR, CssPropertyType.PROP_COLOR, new CssProperties[1]
        {
          CssProperties.V_0
        }, 1 != 0),
        new CSSRegistry(CssProperties.BACKGROUND_COLOR, CssPropertyType.PROP_COLOR, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BACKGROUND_IMAGE, CssPropertyType.PROP_IMAGE, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BACKGROUND_REPEAT, CssPropertyType.PROP_ENUM, new CssProperties[5]
        {
          CssProperties.V_REPEAT,
          CssProperties.V_REPEAT_X,
          CssProperties.V_REPEAT_Y,
          CssProperties.V_NO_REPEAT,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BACKGROUND_ATTACHMENT, CssPropertyType.PROP_ENUM, new CssProperties[3]
        {
          CssProperties.V_FIXED,
          CssProperties.V_SCROLL,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BACKGROUND_POSITION, CssPropertyType.PROP_2NUMBERS, new CssProperties[6]
        {
          CssProperties.V_LEFT,
          CssProperties.V_CENTER,
          CssProperties.V_RIGHT,
          CssProperties.V_TOP,
          CssProperties.V_BOTTOM,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BACKGROUND, CssPropertyType.PROP_SHORTCUT, new CssProperties[6]
        {
          CssProperties.BACKGROUND_COLOR,
          CssProperties.BACKGROUND_IMAGE,
          CssProperties.BACKGROUND_REPEAT,
          CssProperties.BACKGROUND_REPEAT,
          CssProperties.BACKGROUND_POSITION,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.MARGIN_TOP, CssPropertyType.PROP_NUMBER, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.MARGIN_LEFT, CssPropertyType.PROP_NUMBER, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.MARGIN_RIGHT, CssPropertyType.PROP_NUMBER, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.MARGIN_BOTTOM, CssPropertyType.PROP_NUMBER, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.MARGIN, CssPropertyType.PROP_SHORTCUT, new CssProperties[6]
        {
          CssProperties.V_MULTI,
          CssProperties.MARGIN_TOP,
          CssProperties.MARGIN_RIGHT,
          CssProperties.MARGIN_BOTTOM,
          CssProperties.MARGIN_LEFT,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_TOP_STYLE, CssPropertyType.PROP_ENUM, new CssProperties[10]
        {
          CssProperties.V_NONE,
          CssProperties.V_DOTTED,
          CssProperties.V_DASHED,
          CssProperties.V_SOLID,
          CssProperties.V_DOUBLE,
          CssProperties.V_GROOVE,
          CssProperties.V_RIDGE,
          CssProperties.V_INSET,
          CssProperties.V_OUTSET,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_RIGHT_STYLE, CssPropertyType.PROP_ENUM, new CssProperties[10]
        {
          CssProperties.V_NONE,
          CssProperties.V_DOTTED,
          CssProperties.V_DASHED,
          CssProperties.V_SOLID,
          CssProperties.V_DOUBLE,
          CssProperties.V_GROOVE,
          CssProperties.V_RIDGE,
          CssProperties.V_INSET,
          CssProperties.V_OUTSET,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_BOTTOM_STYLE, CssPropertyType.PROP_ENUM, new CssProperties[10]
        {
          CssProperties.V_NONE,
          CssProperties.V_DOTTED,
          CssProperties.V_DASHED,
          CssProperties.V_SOLID,
          CssProperties.V_DOUBLE,
          CssProperties.V_GROOVE,
          CssProperties.V_RIDGE,
          CssProperties.V_INSET,
          CssProperties.V_OUTSET,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_LEFT_STYLE, CssPropertyType.PROP_ENUM, new CssProperties[10]
        {
          CssProperties.V_NONE,
          CssProperties.V_DOTTED,
          CssProperties.V_DASHED,
          CssProperties.V_SOLID,
          CssProperties.V_DOUBLE,
          CssProperties.V_GROOVE,
          CssProperties.V_RIDGE,
          CssProperties.V_INSET,
          CssProperties.V_OUTSET,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_STYLE, CssPropertyType.PROP_SHORTCUT, new CssProperties[6]
        {
          CssProperties.V_MULTI,
          CssProperties.BORDER_TOP_STYLE,
          CssProperties.BORDER_RIGHT_STYLE,
          CssProperties.BORDER_BOTTOM_STYLE,
          CssProperties.BORDER_LEFT_STYLE,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_SPACING, CssPropertyType.PROP_NUMBER, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_COLLAPSE, CssPropertyType.PROP_ENUM, new CssProperties[3]
        {
          CssProperties.V_COLLAPSE,
          CssProperties.V_SEPARATE,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_TOP_WIDTH, CssPropertyType.PROP_NUMBER, new CssProperties[4]
        {
          CssProperties.V_THIN,
          CssProperties.V_MEDIUM,
          CssProperties.V_THICK,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_RIGHT_WIDTH, CssPropertyType.PROP_NUMBER, new CssProperties[4]
        {
          CssProperties.V_THIN,
          CssProperties.V_MEDIUM,
          CssProperties.V_THICK,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_BOTTOM_WIDTH, CssPropertyType.PROP_NUMBER, new CssProperties[4]
        {
          CssProperties.V_THIN,
          CssProperties.V_MEDIUM,
          CssProperties.V_THICK,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_LEFT_WIDTH, CssPropertyType.PROP_NUMBER, new CssProperties[4]
        {
          CssProperties.V_THIN,
          CssProperties.V_MEDIUM,
          CssProperties.V_THICK,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_WIDTH, CssPropertyType.PROP_SHORTCUT, new CssProperties[6]
        {
          CssProperties.V_MULTI,
          CssProperties.BORDER_TOP_WIDTH,
          CssProperties.BORDER_RIGHT_WIDTH,
          CssProperties.BORDER_BOTTOM_WIDTH,
          CssProperties.BORDER_LEFT_WIDTH,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_TOP_COLOR, CssPropertyType.PROP_COLOR, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_RIGHT_COLOR, CssPropertyType.PROP_COLOR, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_BOTTOM_COLOR, CssPropertyType.PROP_COLOR, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_LEFT_COLOR, CssPropertyType.PROP_COLOR, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_COLOR, CssPropertyType.PROP_SHORTCUT, new CssProperties[6]
        {
          CssProperties.V_MULTI,
          CssProperties.BORDER_TOP_COLOR,
          CssProperties.BORDER_RIGHT_COLOR,
          CssProperties.BORDER_BOTTOM_COLOR,
          CssProperties.BORDER_LEFT_COLOR,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_TOP, CssPropertyType.PROP_SHORTCUT, new CssProperties[4]
        {
          CssProperties.BORDER_STYLE,
          CssProperties.BORDER_WIDTH,
          CssProperties.BORDER_COLOR,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_RIGHT, CssPropertyType.PROP_SHORTCUT, new CssProperties[4]
        {
          CssProperties.BORDER_STYLE,
          CssProperties.BORDER_WIDTH,
          CssProperties.BORDER_COLOR,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_BOTTOM, CssPropertyType.PROP_SHORTCUT, new CssProperties[4]
        {
          CssProperties.BORDER_STYLE,
          CssProperties.BORDER_WIDTH,
          CssProperties.BORDER_COLOR,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER_LEFT, CssPropertyType.PROP_SHORTCUT, new CssProperties[4]
        {
          CssProperties.BORDER_STYLE,
          CssProperties.BORDER_WIDTH,
          CssProperties.BORDER_COLOR,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.BORDER, CssPropertyType.PROP_SHORTCUT, new CssProperties[4]
        {
          CssProperties.BORDER_STYLE,
          CssProperties.BORDER_WIDTH,
          CssProperties.BORDER_COLOR,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.PADDING_TOP, CssPropertyType.PROP_NUMBER, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.PADDING_RIGHT, CssPropertyType.PROP_NUMBER, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.PADDING_BOTTOM, CssPropertyType.PROP_NUMBER, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.PADDING_LEFT, CssPropertyType.PROP_NUMBER, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.PADDING, CssPropertyType.PROP_SHORTCUT, new CssProperties[6]
        {
          CssProperties.V_MULTI,
          CssProperties.PADDING_TOP,
          CssProperties.PADDING_RIGHT,
          CssProperties.PADDING_BOTTOM,
          CssProperties.PADDING_LEFT,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.POSITION, CssPropertyType.PROP_ENUM, new CssProperties[3]
        {
          CssProperties.V_STATIC,
          CssProperties.V_ABSOLUTE,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.WIDTH, CssPropertyType.PROP_NUMBER, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.HEIGHT, CssPropertyType.PROP_NUMBER, new CssProperties[1]
        {
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.FLOAT, CssPropertyType.PROP_ENUM, new CssProperties[4]
        {
          CssProperties.V_LEFT,
          CssProperties.V_RIGHT,
          CssProperties.V_NONE,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.DISPLAY, CssPropertyType.PROP_ENUM, new CssProperties[4]
        {
          CssProperties.V_NONE,
          CssProperties.V_BLOCK,
          CssProperties.V_INLINE,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.CLEAR, CssPropertyType.PROP_ENUM, new CssProperties[5]
        {
          CssProperties.V_NONE,
          CssProperties.V_LEFT,
          CssProperties.V_RIGHT,
          CssProperties.V_BOTH,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.PAGE_BREAK_AFTER, CssPropertyType.PROP_ENUM, new CssProperties[3]
        {
          CssProperties.V_ALWAYS,
          CssProperties.V_AUTO,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.PAGE_BREAK_BEFORE, CssPropertyType.PROP_ENUM, new CssProperties[3]
        {
          CssProperties.V_ALWAYS,
          CssProperties.V_AUTO,
          CssProperties.V_0
        }, 0 != 0),
        new CSSRegistry(CssProperties.HREF, CssPropertyType.PROP_STRING, new CssProperties[1]
        {
          CssProperties.V_0
        }, 1 != 0)
      };
      HtmlCSS.SymbolMap = new HtmlSymbolMap[227]
      {
        new HtmlSymbolMap("&AElig;", 198),
        new HtmlSymbolMap("&Aacute;", 193),
        new HtmlSymbolMap("&Acirc;", 194),
        new HtmlSymbolMap("&Agrave;", 192),
        new HtmlSymbolMap("&Aring;", 197),
        new HtmlSymbolMap("&Atilde;", 195),
        new HtmlSymbolMap("&Auml;", 196),
        new HtmlSymbolMap("&Ccedil;", 199),
        new HtmlSymbolMap("&Dagger;", 8225),
        new HtmlSymbolMap("&Eacute;", 201),
        new HtmlSymbolMap("&Ecirc;", 202),
        new HtmlSymbolMap("&Egrave;", 200),
        new HtmlSymbolMap("&Euml;", 203),
        new HtmlSymbolMap("&ETH;", 208),
        new HtmlSymbolMap("&Iacute;", 205),
        new HtmlSymbolMap("&Icirc;", 206),
        new HtmlSymbolMap("&Igrave;", 204),
        new HtmlSymbolMap("&Iuml;", 207),
        new HtmlSymbolMap("&Ntilde;", 209),
        new HtmlSymbolMap("&Oacute;", 211),
        new HtmlSymbolMap("&Ocirc;", 212),
        new HtmlSymbolMap("&Oelig;", 338),
        new HtmlSymbolMap("&Ograve;", 210),
        new HtmlSymbolMap("&Oslash;", 216),
        new HtmlSymbolMap("&Otilde;", 213),
        new HtmlSymbolMap("&Ouml;", 214),
        new HtmlSymbolMap("&Prime;", 8243),
        new HtmlSymbolMap("&Scaron;", 352),
        new HtmlSymbolMap("&THORN;", 222),
        new HtmlSymbolMap("&Uacute;", 218),
        new HtmlSymbolMap("&Ucirc;", 219),
        new HtmlSymbolMap("&Ugrave;", 217),
        new HtmlSymbolMap("&Uuml;", 220),
        new HtmlSymbolMap("&Yacute;", 221),
        new HtmlSymbolMap("&Yuml;", 376),
        new HtmlSymbolMap("&acute;", 180),
        new HtmlSymbolMap("&aacute;", 225),
        new HtmlSymbolMap("&aelig;", 230),
        new HtmlSymbolMap("&acirc;", 226),
        new HtmlSymbolMap("&agrave;", 224),
        new HtmlSymbolMap("&amp;", 38),
        new HtmlSymbolMap("&aring;", 229),
        new HtmlSymbolMap("&atilde;", 227),
        new HtmlSymbolMap("&auml;", 228),
        new HtmlSymbolMap("&bdquo;", 8222),
        new HtmlSymbolMap("&brvbar;", 166),
        new HtmlSymbolMap("&bull;", 8226),
        new HtmlSymbolMap("&ccedil;", 231),
        new HtmlSymbolMap("&cedil;", 184),
        new HtmlSymbolMap("&cent;", 162),
        new HtmlSymbolMap("&circ;", 710),
        new HtmlSymbolMap("&copy;", 169),
        new HtmlSymbolMap("&curren;", 164),
        new HtmlSymbolMap("&dagger;", 8224),
        new HtmlSymbolMap("&deg;", 176),
        new HtmlSymbolMap("&divide;", 247),
        new HtmlSymbolMap("&eacute;", 233),
        new HtmlSymbolMap("&ecirc;", 234),
        new HtmlSymbolMap("&egrave;", 232),
        new HtmlSymbolMap("&eth;", 240),
        new HtmlSymbolMap("&euml;", 235),
        new HtmlSymbolMap("&euro;", 8364),
        new HtmlSymbolMap("&fnof;", 402),
        new HtmlSymbolMap("&frac12;", 189),
        new HtmlSymbolMap("&frac14;", 188),
        new HtmlSymbolMap("&frac34;", 190),
        new HtmlSymbolMap("&frasl;", 8260),
        new HtmlSymbolMap("&ge;", 8805),
        new HtmlSymbolMap("&gt;", 62),
        new HtmlSymbolMap("&hellip;", 8230),
        new HtmlSymbolMap("&iexcl;", 161),
        new HtmlSymbolMap("&iacute;", 237),
        new HtmlSymbolMap("&icirc;", 238),
        new HtmlSymbolMap("&igrave;", 236),
        new HtmlSymbolMap("&iquest;", 191),
        new HtmlSymbolMap("&iuml;", 239),
        new HtmlSymbolMap("&laquo;", 171),
        new HtmlSymbolMap("&ldquo;", 8220),
        new HtmlSymbolMap("&le;", 8804),
        new HtmlSymbolMap("&lsaquo;", 8249),
        new HtmlSymbolMap("&lsquo;", 8216),
        new HtmlSymbolMap("&lt;", 60),
        new HtmlSymbolMap("&macr;", 175),
        new HtmlSymbolMap("&mdash;", 8212),
        new HtmlSymbolMap("&micro;", 181),
        new HtmlSymbolMap("&middot;", 183),
        new HtmlSymbolMap("&nbsp;", 160),
        new HtmlSymbolMap("&ndash;", 8211),
        new HtmlSymbolMap("&ne;", 8800),
        new HtmlSymbolMap("&not;", 172),
        new HtmlSymbolMap("&ntilde;", 241),
        new HtmlSymbolMap("&oacute;", 243),
        new HtmlSymbolMap("&ocirc;", 244),
        new HtmlSymbolMap("&oelig;", 339),
        new HtmlSymbolMap("&ograve;", 242),
        new HtmlSymbolMap("&oline;", 8254),
        new HtmlSymbolMap("&ordf;", 170),
        new HtmlSymbolMap("&ordm;", 186),
        new HtmlSymbolMap("&oslash;", 248),
        new HtmlSymbolMap("&otilde;", 245),
        new HtmlSymbolMap("&ouml;", 246),
        new HtmlSymbolMap("&para;", 182),
        new HtmlSymbolMap("&permil;", 8240),
        new HtmlSymbolMap("&plusmn;", 177),
        new HtmlSymbolMap("&pound;", 163),
        new HtmlSymbolMap("&prime;", 8242),
        new HtmlSymbolMap("&rsaquo;", 8250),
        new HtmlSymbolMap("&quot;", 34),
        new HtmlSymbolMap("&raquo;", 187),
        new HtmlSymbolMap("&rdquo;", 8221),
        new HtmlSymbolMap("&reg;", 174),
        new HtmlSymbolMap("&rsquo;", 8217),
        new HtmlSymbolMap("&sbquo;", 8218),
        new HtmlSymbolMap("&scaron;", 353),
        new HtmlSymbolMap("&sect;", 167),
        new HtmlSymbolMap("&shy;", 173),
        new HtmlSymbolMap("&sup1;", 185),
        new HtmlSymbolMap("&sup2;", 178),
        new HtmlSymbolMap("&sup3;", 179),
        new HtmlSymbolMap("&szlig;", 223),
        new HtmlSymbolMap("&thorn;", 254),
        new HtmlSymbolMap("&tilde;", 732),
        new HtmlSymbolMap("&times;", 215),
        new HtmlSymbolMap("&trade;", 8482),
        new HtmlSymbolMap("&uacute;", 250),
        new HtmlSymbolMap("&ucirc;", 251),
        new HtmlSymbolMap("&ugrave;", 249),
        new HtmlSymbolMap("&uuml;", 252),
        new HtmlSymbolMap("&uml;", 168),
        new HtmlSymbolMap("&yacute;", 253),
        new HtmlSymbolMap("&yen;", 165),
        new HtmlSymbolMap("&yuml;", (int) byte.MaxValue),
        new HtmlSymbolMap("&forall;", 8704),
        new HtmlSymbolMap("&part;", 8706),
        new HtmlSymbolMap("&exist;", 8707),
        new HtmlSymbolMap("&empty;", 8709),
        new HtmlSymbolMap("&nabla;", 8711),
        new HtmlSymbolMap("&isin;", 8712),
        new HtmlSymbolMap("&notin;", 8713),
        new HtmlSymbolMap("&ni;", 8715),
        new HtmlSymbolMap("&prod;", 8719),
        new HtmlSymbolMap("&sum;", 8721),
        new HtmlSymbolMap("&minus;", 8722),
        new HtmlSymbolMap("&lowast;", 8727),
        new HtmlSymbolMap("&radic;", 8730),
        new HtmlSymbolMap("&prop;", 8733),
        new HtmlSymbolMap("&infin;", 8734),
        new HtmlSymbolMap("&ang;", 8736),
        new HtmlSymbolMap("&and;", 8743),
        new HtmlSymbolMap("&or;", 8744),
        new HtmlSymbolMap("&cap;", 8745),
        new HtmlSymbolMap("&cup;", 8746),
        new HtmlSymbolMap("&int;", 8747),
        new HtmlSymbolMap("&there4;", 8756),
        new HtmlSymbolMap("&sim;", 8764),
        new HtmlSymbolMap("&cong;", 8773),
        new HtmlSymbolMap("&asymp;", 8776),
        new HtmlSymbolMap("&ne;", 8800),
        new HtmlSymbolMap("&equiv;", 8801),
        new HtmlSymbolMap("&le;", 8804),
        new HtmlSymbolMap("&ge;", 8805),
        new HtmlSymbolMap("&sub;", 8834),
        new HtmlSymbolMap("&sup;", 8835),
        new HtmlSymbolMap("&nsub;", 8836),
        new HtmlSymbolMap("&sube;", 8838),
        new HtmlSymbolMap("&supe;", 8839),
        new HtmlSymbolMap("&oplus;", 8853),
        new HtmlSymbolMap("&otimes;", 8855),
        new HtmlSymbolMap("&perp;", 8869),
        new HtmlSymbolMap("&sdot;", 8901),
        new HtmlSymbolMap("&Alpha;", 913),
        new HtmlSymbolMap("&Beta;", 914),
        new HtmlSymbolMap("&Gamma;", 915),
        new HtmlSymbolMap("&Delta;", 916),
        new HtmlSymbolMap("&Epsilon;", 917),
        new HtmlSymbolMap("&Zeta;", 918),
        new HtmlSymbolMap("&Eta;", 919),
        new HtmlSymbolMap("&Theta;", 920),
        new HtmlSymbolMap("&Iota;", 921),
        new HtmlSymbolMap("&Kappa;", 922),
        new HtmlSymbolMap("&Lambda;", 923),
        new HtmlSymbolMap("&Mu;", 924),
        new HtmlSymbolMap("&Nu;", 925),
        new HtmlSymbolMap("&Xi;", 926),
        new HtmlSymbolMap("&Omicron;", 927),
        new HtmlSymbolMap("&Pi;", 928),
        new HtmlSymbolMap("&Rho;", 929),
        new HtmlSymbolMap("&Sigma;", 931),
        new HtmlSymbolMap("&Tau;", 932),
        new HtmlSymbolMap("&Upsilon;", 933),
        new HtmlSymbolMap("&Phi;", 934),
        new HtmlSymbolMap("&Chi;", 935),
        new HtmlSymbolMap("&Psi;", 936),
        new HtmlSymbolMap("&Omega;", 937),
        new HtmlSymbolMap("&alpha;", 945),
        new HtmlSymbolMap("&beta;", 946),
        new HtmlSymbolMap("&gamma;", 947),
        new HtmlSymbolMap("&delta;", 948),
        new HtmlSymbolMap("&epsilon;", 949),
        new HtmlSymbolMap("&zeta;", 950),
        new HtmlSymbolMap("&eta;", 951),
        new HtmlSymbolMap("&theta;", 952),
        new HtmlSymbolMap("&iota;", 953),
        new HtmlSymbolMap("&kappa;", 954),
        new HtmlSymbolMap("&lambda;", 955),
        new HtmlSymbolMap("&mu;", 956),
        new HtmlSymbolMap("&nu;", 957),
        new HtmlSymbolMap("&xi;", 958),
        new HtmlSymbolMap("&omicron;", 959),
        new HtmlSymbolMap("&pi;", 960),
        new HtmlSymbolMap("&rho;", 961),
        new HtmlSymbolMap("&sigmaf;", 962),
        new HtmlSymbolMap("&sigma;", 963),
        new HtmlSymbolMap("&tau;", 964),
        new HtmlSymbolMap("&upsilon;", 965),
        new HtmlSymbolMap("&phi;", 966),
        new HtmlSymbolMap("&chi;", 967),
        new HtmlSymbolMap("&psi;", 968),
        new HtmlSymbolMap("&omega;", 969),
        new HtmlSymbolMap("&thetasym;", 977),
        new HtmlSymbolMap("&upsih;", 978),
        new HtmlSymbolMap("&piv;", 982),
        new HtmlSymbolMap("&larr;", 8592),
        new HtmlSymbolMap("&uarr;", 8593),
        new HtmlSymbolMap("&rarr;", 8594),
        new HtmlSymbolMap("&darr;", 8595),
        new HtmlSymbolMap("&harr;", 8596)
      };
    }

    public HtmlCSS()
    {
      this.m_bStateChanged = true;
      this.m_arrRules = new List<HtmlCSSRule>();
      this.m_rgbColor = new AuxRGB();
      this.m_rgbBackgroundColor = new AuxRGB();
      this.m_arrURLs = new List<string>();
      this.m_arrRelativePathStack = new List<string>();      
      this.m_pCSS = (char[]) null;
      this.m_nPtr = 0;
      this.m_pRoot = new HtmlTreeItem();
      this.m_pRoot.m_nType = TagType.TAG_BODY;
      this.m_pRoot.m_dwTagFlag = 8192U;
      this.m_pCurrentLeaf = this.m_pRoot;
      this.m_bStateChanged = true;
      this.m_szSymbol = new StringBuilder();
    }

    public void InitializeWithDefaultRules()
    {
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_B, CssProperties.FONT_WEIGHT, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 900f));
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_I, CssProperties.FONT_STYLE, CssPropertyType.PROP_ENUM, CssProperties.V_ITALIC, 0.0f));
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_U, CssProperties.TEXT_DECORATION, CssPropertyType.PROP_ENUM, CssProperties.V_UNDERLINE, 0.0f));
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_S, CssProperties.TEXT_DECORATION, CssPropertyType.PROP_ENUM, CssProperties.V_LINE_THROUGH, 0.0f));
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_INS, CssProperties.TEXT_DECORATION, CssPropertyType.PROP_ENUM, CssProperties.V_UNDERLINE, 0.0f));
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_DEL, CssProperties.TEXT_DECORATION, CssPropertyType.PROP_ENUM, CssProperties.V_LINE_THROUGH, 0.0f));
      float[] numArray = new float[6]
      {
        24f,
        18f,
        13.5f,
        12f,
        10f,
        7.5f
      };
      for (int index = 40; index <= 45; ++index)
      {
        this.m_arrRules.Add(new HtmlCSSRule((TagType) index, CssProperties.FONT_SIZE, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, numArray[index - 40]));
        this.m_arrRules.Add(new HtmlCSSRule((TagType) index, CssProperties.FONT_WEIGHT, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 900f));
        HtmlCSSRule htmlCssRule = new HtmlCSSRule((TagType) index, CssProperties.WIDTH, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 100f);
        htmlCssRule.m_arrProperties[0].m_bPercent = true;
        this.m_arrRules.Add(htmlCssRule);
      }
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_CENTER, CssProperties.TEXT_ALIGN, CssPropertyType.PROP_ENUM, CssProperties.V_CENTER, 0.0f));
      HtmlCSSRule htmlCssRule1 = new HtmlCSSRule(TagType.TAG_CENTER, CssProperties.WIDTH, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 100f);
      htmlCssRule1.m_arrProperties[0].m_bPercent = true;
      this.m_arrRules.Add(htmlCssRule1);
      HtmlCSSRule htmlCssRule2 = new HtmlCSSRule(TagType.TAG_DIV, CssProperties.WIDTH, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 100f);
      htmlCssRule2.m_arrProperties[0].m_bPercent = true;
      this.m_arrRules.Add(htmlCssRule2);
      HtmlCSSRule htmlCssRule3 = new HtmlCSSRule(TagType.TAG_P, CssProperties.WIDTH, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 100f);
      htmlCssRule3.m_arrProperties[0].m_bPercent = true;
      this.m_arrRules.Add(htmlCssRule3);
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_ADDRESS, CssProperties.FONT_STYLE, CssPropertyType.PROP_ENUM, CssProperties.V_ITALIC, 0.0f));
      HtmlCSSRule htmlCssRule4 = new HtmlCSSRule(TagType.TAG_ADDRESS, CssProperties.WIDTH, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 100f);
      htmlCssRule4.m_arrProperties[0].m_bPercent = true;
      this.m_arrRules.Add(htmlCssRule4);
      HtmlCSSRule htmlCssRule5 = new HtmlCSSRule(TagType.TAG_FORM, CssProperties.WIDTH, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 100f);
      htmlCssRule5.m_arrProperties[0].m_bPercent = true;
      this.m_arrRules.Add(htmlCssRule5);
      TagType[] tagTypeArray = new TagType[9]
      {
        TagType.TAG_CODE,
        TagType.TAG_KBD,
        TagType.TAG_SAMP,
        TagType.TAG_TT,
        TagType.TAG_CITE,
        TagType.TAG_VAR,
        TagType.TAG_EM,
        TagType.TAG_DFN,
        TagType.TAG_STRONG
      };
      for (int index = 0; index < 4; ++index)
      {
        HtmlCSSRule htmlCssRule6 = new HtmlCSSRule(tagTypeArray[index], CssProperties.FONT_FAMILY, CssPropertyType.PROP_STRING, CssProperties.NOTFOUND, 0.0f);
        htmlCssRule6.m_arrProperties[0].m_bstrValue = "Courier New";
        this.m_arrRules.Add(htmlCssRule6);
      }
      for (int index = 4; index < 8; ++index)
        this.m_arrRules.Add(new HtmlCSSRule(tagTypeArray[index], CssProperties.FONT_STYLE, CssPropertyType.PROP_ENUM, CssProperties.V_ITALIC, 0.0f));
      this.m_arrRules.Add(new HtmlCSSRule(tagTypeArray[8], CssProperties.FONT_WEIGHT, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 900f));
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_SUB, CssProperties.VERTICAL_ALIGN, CssPropertyType.PROP_ENUM, CssProperties.V_SUB, 0.0f));
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_SUB, CssProperties.FONT_SIZE, CssPropertyType.PROP_ENUM, CssProperties.V_SMALLER, 0.0f));
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_SUP, CssProperties.VERTICAL_ALIGN, CssPropertyType.PROP_ENUM, CssProperties.V_SUPER, 0.0f));
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_SUP, CssProperties.FONT_SIZE, CssPropertyType.PROP_ENUM, CssProperties.V_SMALLER, 0.0f));
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_BIG, CssProperties.FONT_SIZE, CssPropertyType.PROP_ENUM, CssProperties.V_LARGER, 0.0f));
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_SMALL, CssProperties.FONT_SIZE, CssPropertyType.PROP_ENUM, CssProperties.V_SMALLER, 0.0f));
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_TH, CssProperties.FONT_WEIGHT, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 900f));
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_TH, CssProperties.TEXT_ALIGN, CssPropertyType.PROP_ENUM, CssProperties.V_CENTER, 0.0f));
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_UL, CssProperties.LIST_STYLE_TYPE, CssPropertyType.PROP_ENUM, CssProperties.V_DISC, 0.0f));
      HtmlCSSRule htmlCssRule7 = new HtmlCSSRule(TagType.TAG_UL, CssProperties.WIDTH, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 100f);
      htmlCssRule7.m_arrProperties[0].m_bPercent = true;
      this.m_arrRules.Add(htmlCssRule7);
      HtmlCSSRule htmlCssRule8 = new HtmlCSSRule(TagType.TAG_UL, CssProperties.LIST_STYLE_TYPE, CssPropertyType.PROP_ENUM, CssProperties.V_CIRCLE, 0.0f);
      HtmlCSSSelector htmlCssSelector1 = new HtmlCSSSelector();
      htmlCssSelector1.m_nTag = TagType.TAG_UL;
      htmlCssRule8.m_arrSelectors.Add(htmlCssSelector1);
      this.m_arrRules.Add(htmlCssRule8);
      HtmlCSSRule htmlCssRule9 = new HtmlCSSRule(TagType.TAG_UL, CssProperties.LIST_STYLE_TYPE, CssPropertyType.PROP_ENUM, CssProperties.V_SQUARE, 0.0f);
      HtmlCSSSelector htmlCssSelector2 = new HtmlCSSSelector();
      htmlCssSelector2.m_nTag = TagType.TAG_UL;
      htmlCssRule9.m_arrSelectors.Add(htmlCssSelector2);
      HtmlCSSSelector htmlCssSelector3 = new HtmlCSSSelector();
      htmlCssSelector3.m_nTag = TagType.TAG_UL;
      htmlCssRule9.m_arrSelectors.Add(htmlCssSelector3);
      this.m_arrRules.Add(htmlCssRule9);
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_OL, CssProperties.LIST_STYLE_TYPE, CssPropertyType.PROP_ENUM, CssProperties.V_DECIMAL, 0.0f));
      HtmlCSSRule htmlCssRule10 = new HtmlCSSRule(TagType.TAG_OL, CssProperties.WIDTH, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 100f);
      htmlCssRule10.m_arrProperties[0].m_bPercent = true;
      this.m_arrRules.Add(htmlCssRule10);
      HtmlCSSRule htmlCssRule11 = new HtmlCSSRule(TagType.TAG_LI, CssProperties.WIDTH, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 100f);
      htmlCssRule11.m_arrProperties[0].m_bPercent = true;
      this.m_arrRules.Add(htmlCssRule11);
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_UL, CssProperties.LIST_STYLE_POSITION, CssPropertyType.PROP_ENUM, CssProperties.V_OUTSIDE, 0.0f));
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_OL, CssProperties.LIST_STYLE_POSITION, CssPropertyType.PROP_ENUM, CssProperties.V_OUTSIDE, 0.0f));
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_CAPTION, CssProperties.TEXT_ALIGN, CssPropertyType.PROP_ENUM, CssProperties.V_CENTER, 0.0f));
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_HR, CssProperties.MARGIN_TOP, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 8f));
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_HR, CssProperties.MARGIN_BOTTOM, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 8f));
      HtmlCSSRule htmlCssRule12 = new HtmlCSSRule(TagType.TAG_BQ, CssProperties.WIDTH, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 100f);
      htmlCssRule12.m_arrProperties[0].m_bPercent = true;
      this.m_arrRules.Add(htmlCssRule12);
      HtmlCSSRule htmlCssRule13 = new HtmlCSSRule(TagType.TAG_DD, CssProperties.WIDTH, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 100f);
      htmlCssRule13.m_arrProperties[0].m_bPercent = true;
      this.m_arrRules.Add(htmlCssRule13);
      HtmlCSSRule htmlCssRule14 = new HtmlCSSRule(TagType.TAG_DL, CssProperties.WIDTH, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 100f);
      htmlCssRule14.m_arrProperties[0].m_bPercent = true;
      this.m_arrRules.Add(htmlCssRule14);
      HtmlCSSRule htmlCssRule15 = new HtmlCSSRule(TagType.TAG_DT, CssProperties.WIDTH, CssPropertyType.PROP_NUMBER, CssProperties.NOTFOUND, 100f);
      htmlCssRule15.m_arrProperties[0].m_bPercent = true;
      this.m_arrRules.Add(htmlCssRule15);
      this.m_arrRules.Add(new HtmlCSSRule(TagType.TAG_A, CssProperties.TEXT_DECORATION, CssPropertyType.PROP_ENUM, CssProperties.V_UNDERLINE, 0.0f));
      HtmlCSSRule htmlCssRule16 = new HtmlCSSRule(TagType.TAG_A, CssProperties.COLOR, CssPropertyType.PROP_COLOR, CssProperties.NOTFOUND, 0.0f);
      AuxRGB rgb = new AuxRGB(0.0f, 0.0f, 1f);
      htmlCssRule16.m_arrProperties[0].m_rgbColor.Set(ref rgb);
      this.m_arrRules.Add(htmlCssRule16);
      this.m_nHyperLinkColorRuleNumber = this.m_arrRules.Count - 1;
    }

    public void AddToTree(HtmlTag pTag)
    {
      if (pTag.m_nType == TagType.TAG_META || pTag.m_nType == TagType.TAG_LINK || (pTag.m_nType == TagType.TAG_HTML || pTag.m_nType == TagType.TAG_STYLE))
        return;
      this.m_bStateChanged = true;
      if (!pTag.m_bClosing)
      {
        if (pTag.IsOneOf(17407U) && pTag.m_nType != TagType.TAG_TABLE && this.IsUnder(TagType.TAG_P, 17407U))
          this.AddThisTagToTree(TagType.TAG_P);
        if (pTag.IsOneOf(7168U) && this.IsUnder(TagType.TAG_P, 32U))
          this.AddThisTagToTree(TagType.TAG_P);
        switch (pTag.m_nType)
        {
          case TagType.TAG_A:
            if (this.IsUnder(TagType.TAG_A, 8224U))
            {
              this.AddThisTagToTree(TagType.TAG_A);
              break;
            }
            else
              break;
          case TagType.TAG_LI:
            if (this.IsUnder(TagType.TAG_LI, 3U))
            {
              this.AddThisTagToTree(TagType.TAG_LI);
              break;
            }
            else
              break;
          case TagType.TAG_P:
            if (this.IsUnder(TagType.TAG_P, 32U))
            {
              this.AddThisTagToTree(TagType.TAG_P);
              break;
            }
            else
              break;
          case TagType.TAG_TD:
          case TagType.TAG_TH:
            if (!this.IsUnder(TagType.TAG_TR, 32U))
              this.AddThisTagToTree(TagType.TAG_TR, false);
            if (this.IsUnder(pTag.m_nType, 32U))
            {
              this.AddThisTagToTree(pTag.m_nType);
              break;
            }
            else
              break;
          case TagType.TAG_TR:
            if (this.IsUnder(pTag.m_nType, 32U))
            {
              this.AddThisTagToTree(pTag.m_nType);
              break;
            }
            else
              break;
        }
        if (pTag.m_nType == TagType.TAG_OPTION && this.IsUnder(TagType.TAG_OPTION, 56U))
          this.AddThisTagToTree(TagType.TAG_OPTION);
      }
      if (pTag.m_bClosing)
      {
        if (pTag.IsOneOf(20223U) && this.IsUnder(TagType.TAG_P, pTag.m_dwTagFlag))
          this.AddThisTagToTree(TagType.TAG_P);
        switch (pTag.m_nType)
        {
          case TagType.TAG_UL:
            if (this.IsUnder(TagType.TAG_LI, 1U))
            {
              this.AddThisTagToTree(TagType.TAG_LI);
              break;
            }
            else
              break;
          case TagType.TAG_OL:
            if (this.IsUnder(TagType.TAG_LI, 2U))
            {
              this.AddThisTagToTree(TagType.TAG_LI);
              break;
            }
            else
              break;
          case TagType.TAG_TABLE:
            if (this.IsUnder(TagType.TAG_TD, 32U))
            {
              this.AddThisTagToTree(TagType.TAG_TD);
              break;
            }
            else if (this.IsUnder(TagType.TAG_TH, 32U))
            {
              this.AddThisTagToTree(TagType.TAG_TH);
              break;
            }
            else
              break;
          case TagType.TAG_TR:
            if (this.IsUnder(TagType.TAG_TD, 32U))
            {
              this.AddThisTagToTree(TagType.TAG_TD);
              break;
            }
            else
              break;
        }
        if (pTag.m_nType == TagType.TAG_SELECT && this.IsUnder(TagType.TAG_OPTION, 56U))
          this.AddThisTagToTree(TagType.TAG_OPTION);
      }
      if (pTag.m_bClosing)
      {
        if (!this.IsUnder(pTag.m_nType, 8192U))
          return;
        this.m_pBackupLeaf = this.m_pCurrentLeaf;
        HtmlTreeItem ppTree = (HtmlTreeItem) null;
        HtmlTreeItem ppSkippedTagTreeBottom = (HtmlTreeItem) null;
        for (; this.m_pCurrentLeaf.m_pParent != null && this.m_pCurrentLeaf.m_nType != pTag.m_nType; this.m_pCurrentLeaf = this.m_pCurrentLeaf.m_pParent)
          this.BuildTreeFromSkippedTags(ref ppTree, ref ppSkippedTagTreeBottom, this.m_pCurrentLeaf);
        if (ppTree != null && (pTag.m_nType == TagType.TAG_TD || pTag.m_nType == TagType.TAG_TR || pTag.m_nType == TagType.TAG_TABLE))
          ppTree = (HtmlTreeItem) null;
        if (this.m_pCurrentLeaf.m_pParent != null)
          this.m_pCurrentLeaf = this.m_pCurrentLeaf.m_pParent;
        if (ppTree == null)
          return;
        this.m_pCurrentLeaf.m_arrChildren.Add(ppTree);
        ppTree.m_pParent = this.m_pCurrentLeaf;
        this.m_pCurrentLeaf = ppSkippedTagTreeBottom;
      }
      else
      {
        if (pTag.m_nType == TagType.TAG_BODY)
        {
          this.m_pCurrentLeaf = this.m_pRoot;
        }
        else
        {
          HtmlTreeItem htmlTreeItem = new HtmlTreeItem(this.m_pCurrentLeaf);
          htmlTreeItem.m_nType = pTag.m_nType;
          htmlTreeItem.m_dwTagFlag = pTag.m_dwTagFlag;
          htmlTreeItem.m_pParent = this.m_pCurrentLeaf;
          this.m_pCurrentLeaf.m_arrChildren.Add(htmlTreeItem);
          this.m_pCurrentLeaf = htmlTreeItem;
        }
        pTag.GetStringAttribute("Class", ref this.m_pCurrentLeaf.m_bstrClass);
        pTag.GetStringAttribute("ID", ref this.m_pCurrentLeaf.m_bstrID);
        if (this.m_pCurrentLeaf.m_bstrClass != null)
          this.m_pCurrentLeaf.m_bstrClass.ToLower();
        this.m_pCurrentLeaf.AddApplicableProperties(this, pTag);
      }
    }

    private void BuildTreeFromSkippedTags(ref HtmlTreeItem ppTree, ref HtmlTreeItem ppSkippedTagTreeBottom, HtmlTreeItem pNode)
    {
      if (ppTree == null)
      {
        ppTree = new HtmlTreeItem(pNode, true);
        ppSkippedTagTreeBottom = ppTree;
      }
      else
      {
        HtmlTreeItem htmlTreeItem = new HtmlTreeItem(pNode, true);
        ppTree.m_pParent = htmlTreeItem;
        htmlTreeItem.m_arrChildren.Add(ppTree);
        ppTree = htmlTreeItem;
      }
    }

    public void RemoveLastLeaf()
    {
      if (this.m_pCurrentLeaf.m_pParent == null)
        return;
      HtmlTreeItem htmlTreeItem1 = this.m_pCurrentLeaf;
      this.m_pCurrentLeaf = htmlTreeItem1.m_pParent;
      foreach (HtmlTreeItem htmlTreeItem2 in this.m_pCurrentLeaf.m_arrChildren)
      {
        if (htmlTreeItem2 == htmlTreeItem1)
        {
          this.m_pCurrentLeaf.m_arrChildren.Remove(htmlTreeItem2);
          break;
        }
      }
    }

    public void ReverseLastClosure()
    {
      this.m_pCurrentLeaf = this.m_pBackupLeaf;
    }

    private bool IsUnder(TagType nType, uint LookNoFurtherThanFlag)
    {
      for (HtmlTreeItem htmlTreeItem = this.m_pCurrentLeaf; htmlTreeItem != null && ((int) htmlTreeItem.m_dwTagFlag & (int) LookNoFurtherThanFlag) == 0; htmlTreeItem = htmlTreeItem.m_pParent)
      {
        if (htmlTreeItem.m_nType == nType)
          return true;
      }
      return false;
    }

    public void AddThisTagToTree(TagType nType)
    {
      this.AddThisTagToTree(nType, true);
    }

    public void AddThisTagToTree(TagType nType, bool bClosing)
    {
      HtmlTag pTag = new HtmlTag(this.m_pHtmlManager);
      pTag.m_nType = nType;
      pTag.m_bClosing = bClosing;
      pTag.LookupFlag();
      this.AddToTree(pTag);
    }

    public void LoadStyle(HtmlTag pTag)
    {
      string pVal1 = (string) null;
      if (pTag.GetStringAttribute("type", ref pVal1) && string.Compare(pVal1, "text/css", true) != 0)
        return;
      string pVal2 = "all";
      pTag.GetStringAttribute("media", ref pVal2);
      ushort num = (ushort) 128;
      if (string.Compare(pVal2, "all", true) == 0 || pVal2.Length == 0)
        num = (ushort) 1;
      else if (string.Compare(pVal2, "screen", true) == 0)
        num = (ushort) 2;
      else if (string.Compare(pVal2, "print", true) == 0)
        num = (ushort) 4;
      else if (string.Compare(pVal2, "asppdf", true) == 0)
        num = (ushort) 8;
      if (pTag.m_nType == TagType.TAG_LINK)
      {
        if (((int) num & (int) this.m_pHtmlManager.m_nMediaFlags) == 0)
          return;
        string pVal3 = (string) null;
        if (!pTag.GetStringAttribute("href", ref pVal3))
          return;
        string pVal4 = (string) null;
        if (!pTag.GetStringAttribute("rel", ref pVal4) || !(pVal4.ToLower() == "stylesheet"))
          return;
        HtmlXmlUrl htmlXmlUrl = new HtmlXmlUrl(this.m_pHtmlManager);
        if (!htmlXmlUrl.MakeUrl(this.m_pHtmlManager.m_pBaseUrl, pVal3))
          return;
        foreach (string strA in this.m_pHtmlManager.m_objCSS.m_arrURLs)
        {
          if (string.Compare(strA, htmlXmlUrl.m_bstrUrl, true) == 0)
            return;
        }
        this.m_pHtmlManager.m_objCSS.m_arrURLs.Add(htmlXmlUrl.m_bstrUrl);
        this.m_pHtmlManager.m_objCSS.PushRelativePath(pVal3);
        try
        {
          byte[] pStream = (byte[]) null;
          htmlXmlUrl.ReadUrl(ref pStream);
          this.LoadBufferAndParse(pStream);
        }
        catch (Exception ex)
        {
          this.m_pHtmlManager.LogError("CSS", htmlXmlUrl.m_bstrUrl, ex.Message);
        }
        this.m_pHtmlManager.m_objCSS.PopRelativePath();
      }
      else
      {
        int @string = PdfStream.FindString(this.m_pHtmlManager.m_pBuffer, this.m_pHtmlManager.m_nPtr, "</STYLE>".ToCharArray());
        if (@string == -1)
          return;
        this.AddThisTagToTree(TagType.TAG_STYLE);
        if (((int) num & (int) this.m_pHtmlManager.m_nMediaFlags) != 0)
          this.LoadBufferAndParse(this.m_pHtmlManager.m_pBuffer, this.m_pHtmlManager.m_nPtr - 1, @string - this.m_pHtmlManager.m_nPtr + 1);
        this.m_pHtmlManager.m_nPtr = @string + 8;
        this.m_pHtmlManager.m_pMasterTable.m_arrCells[0].Next();
      }
    }

    public void Next()
    {
      if (this.m_nPtr >= this.m_nSize)
        throw new PdfException("Reached EOF.", 55);
      this.m_c = this.m_pCSS[this.m_nPtr++];
      if ((int) this.m_c == 47 && this.m_nPtr < this.m_nSize && (int) this.m_pCSS[this.m_nPtr] == 42)
      {
        int @string = PdfStream.FindString(this.m_pCSS, this.m_nPtr + 1, "*/".ToCharArray());
        if (@string != -1)
          this.m_nPtr = @string + 2;
        else
          this.Next();
        this.Next();
      }
      if ((int) this.m_c != 60 || this.m_nPtr + 3 >= this.m_nSize || ((int) this.m_pCSS[this.m_nPtr] != 33 || (int) this.m_pCSS[this.m_nPtr + 1] != 45) || (int) this.m_pCSS[this.m_nPtr + 2] != 45)
        return;
      this.m_nPtr += 3;
      this.Next();
    }

    private void NextSymbol()
    {
      this.m_nNextSymbol = CssSymbol.cssUnknown;
      if (this.IsNameChar())
      {
        this.m_szSymbol = new StringBuilder();
        int num = 0;
        while (this.IsNameChar() && num < 80)
        {
          this.m_szSymbol.Append(this.m_c);
          this.Next();
        }
        this.m_nNextSymbol = CssSymbol.cssName;
      }
      else
      {
        if ((int) this.m_c == 42)
          this.m_nNextSymbol = CssSymbol.cssAsterisk;
        else if ((int) this.m_c == 35)
          this.m_nNextSymbol = CssSymbol.cssPound;
        else if ((int) this.m_c == 46)
          this.m_nNextSymbol = CssSymbol.cssDot;
        else if ((int) this.m_c == 58)
          this.m_nNextSymbol = CssSymbol.cssColon;
        else if ((int) this.m_c == 59)
          this.m_nNextSymbol = CssSymbol.cssSemicolon;
        else if ((int) this.m_c == 123)
          this.m_nNextSymbol = CssSymbol.cssOpenBrace;
        else if ((int) this.m_c == 125)
          this.m_nNextSymbol = CssSymbol.cssCloseBrace;
        else if ((int) this.m_c == 44)
          this.m_nNextSymbol = CssSymbol.cssComma;
        else if ((int) this.m_c == 60)
          this.m_nNextSymbol = CssSymbol.cssOpenAngle;
        else if ((int) this.m_c == 43)
          this.m_nNextSymbol = CssSymbol.cssPlus;
        else if ((int) this.m_c == 64)
          this.m_nNextSymbol = CssSymbol.cssAt;
        else if ((int) this.m_c == 40)
          this.m_nNextSymbol = CssSymbol.cssOpenParent;
        else if ((int) this.m_c == 33)
          this.m_nNextSymbol = CssSymbol.cssExclaim;
        else if ((int) this.m_c == 32 || (int) this.m_c == 13 || ((int) this.m_c == 10 || (int) this.m_c == 9) || (int) this.m_c == 12)
          this.m_nNextSymbol = CssSymbol.cssSpace;
        this.Next();
      }
    }

    private bool IsNameChar()
    {
      if (((int) this.m_c < 97 || (int) this.m_c > 122) && ((int) this.m_c < 65 || (int) this.m_c > 90) && (((int) this.m_c < 48 || (int) this.m_c > 57) && (int) this.m_c != 45))
        return (int) this.m_c == 95;
      else
        return true;
    }

    private void SkipSpaces()
    {
      while ((int) this.m_c == 32 || (int) this.m_c == 13 || ((int) this.m_c == 10 || (int) this.m_c == 9) || (int) this.m_c == 12)
        this.Next();
    }

    private bool ParseSelectors(HtmlCSSRule pRule)
    {
      if (this.m_nNextSymbol == CssSymbol.cssAt)
      {
        StringBuilder stringBuilder = new StringBuilder();
        this.NextSymbol();
        if (this.m_nNextSymbol != CssSymbol.cssName || string.Compare(((object) this.m_szSymbol).ToString(), "import", true) != 0)
          return false;
        this.SkipSpaces();
        this.NextSymbol();
        if (this.m_nNextSymbol == CssSymbol.cssName && string.Compare(((object) this.m_szSymbol).ToString(), "url", true) == 0)
        {
          this.SkipSpaces();
          this.NextSymbol();
          if (this.m_nNextSymbol != CssSymbol.cssOpenParent)
            return false;
        }
        this.SkipSpaces();
        if ((int) this.m_c == 34 || (int) this.m_c == 39)
        {
          int @string = PdfStream.FindString(this.m_pCSS, this.m_nPtr + 1, (int) this.m_c == 34 ? "\"".ToCharArray() : "'".ToCharArray());
          if (@string == -1)
            return false;
          stringBuilder.Append(this.m_pCSS, this.m_nPtr, @string - this.m_nPtr);
          this.m_nPtr = @string + 1;
        }
        else
        {
          int @string = PdfStream.FindString(this.m_pCSS, this.m_nPtr, ";".ToCharArray());
          if (@string == -1)
            return false;
          stringBuilder.Append(this.m_pCSS, this.m_nPtr - 1, @string - this.m_nPtr);
          this.m_nPtr = @string;
        }
        while ((int) this.m_c != 59)
          this.Next();
        this.Next();
        string s = ((object) stringBuilder).ToString();
        HtmlObject.Trim(ref s);
        HtmlCSS htmlCss = new HtmlCSS();
        htmlCss.m_pHtmlManager = this.m_pHtmlManager;
        htmlCss.m_bstrCurrentRelativePath = this.m_bstrCurrentRelativePath;
        htmlCss.PushRelativePath(s);
        HtmlTag pTag = new HtmlTag(this.m_pHtmlManager);
        pTag.m_nType = TagType.TAG_LINK;
        pTag.m_arrNames.Add("href");
        pTag.m_arrValues.Add(htmlCss.m_bstrCurrentRelativePath);
        pTag.m_arrNames.Add("rel");
        pTag.m_arrValues.Add("stylesheet");
        htmlCss.LoadStyle(pTag);
        foreach (HtmlCSSRule pRule1 in htmlCss.m_arrRules)
        {
          HtmlCSSRule htmlCssRule = new HtmlCSSRule();
          htmlCssRule.CopyFrom(pRule1, true);
          this.m_arrRules.Add(htmlCssRule);
        }
        this.SkipSpaces();
        return true;
      }
      else
      {
        while (true)
        {
          int num;
          do
          {
            num = this.m_nPtr;
            HtmlCSSSelector htmlCssSelector = new HtmlCSSSelector();
            pRule.m_arrSelectors.Add(htmlCssSelector);
            if (this.m_nNextSymbol == CssSymbol.cssAsterisk || this.m_nNextSymbol == CssSymbol.cssDot || (this.m_nNextSymbol == CssSymbol.cssPound || this.m_nNextSymbol == CssSymbol.cssColon))
            {
              htmlCssSelector.m_nTag = TagType.TAG_NOTATAG;
              if (this.m_nNextSymbol == CssSymbol.cssAsterisk)
                this.NextSymbol();
            }
            else if (this.m_nNextSymbol == CssSymbol.cssName)
            {
              htmlCssSelector.m_nTag = this.IdentifyTag();
              this.NextSymbol();
            }
            if (this.m_nNextSymbol == CssSymbol.cssDot)
            {
              while (this.m_nNextSymbol == CssSymbol.cssDot)
              {
                this.NextSymbol();
                if (this.m_nNextSymbol != CssSymbol.cssName)
                  return this.SkipRule();
                string str = ((object) this.m_szSymbol).ToString().ToLower();
                htmlCssSelector.m_arrClasses.Add(str);
                this.NextSymbol();
              }
            }
            if (this.m_nNextSymbol == CssSymbol.cssPound)
            {
              this.NextSymbol();
              if (this.m_nNextSymbol != CssSymbol.cssName)
                return this.SkipRule();
              htmlCssSelector.m_bstrID = ((object) this.m_szSymbol).ToString();
              this.NextSymbol();
            }
            if (this.m_nNextSymbol == CssSymbol.cssColon)
            {
              this.NextSymbol();
              if (this.m_nNextSymbol != CssSymbol.cssName)
                return this.SkipRule();
              htmlCssSelector.m_bstrPseudo = ((object) this.m_szSymbol).ToString();
              htmlCssSelector.m_bstrPseudo.ToLower();
              if (htmlCssSelector.m_nTag == TagType.TAG_NOTATAG && htmlCssSelector.m_bstrPseudo == "link")
                htmlCssSelector.m_nTag = TagType.TAG_A;
              this.NextSymbol();
            }
            if (this.m_nNextSymbol == CssSymbol.cssSpace || this.m_nNextSymbol == CssSymbol.cssAsterisk || this.m_nNextSymbol == CssSymbol.cssUnknown)
            {
              this.SkipSpaces();
              this.NextSymbol();
            }
            if (this.m_nNextSymbol != CssSymbol.cssOpenBrace && this.m_nNextSymbol != CssSymbol.cssComma)
            {
              if (this.m_nNextSymbol == CssSymbol.cssOpenAngle || this.m_nNextSymbol == CssSymbol.cssPlus)
              {
                htmlCssSelector.m_nDelimiter = this.m_nNextSymbol;
                this.SkipSpaces();
                this.NextSymbol();
              }
            }
            else
              goto label_48;
          }
          while (num != this.m_nPtr);
          this.SkipSpaces();
          this.NextSymbol();
        }
label_48:
        return true;
      }
    }

    private bool ParseRule()
    {
      do
      {
        this.SkipSpaces();
        this.NextSymbol();
        this.m_pCurrentRule = new HtmlCSSRule();
        this.m_arrRules.Add(this.m_pCurrentRule);
        this.ParseSelectors(this.m_pCurrentRule);
        this.SkipSpaces();
      }
      while (this.m_nNextSymbol == CssSymbol.cssComma);
      this.m_pCurrentRule.m_bNeedsCopying = false;
      if (this.m_nNextSymbol == CssSymbol.cssOpenBrace)
      {
        int num1 = this.m_nPtr;
        this.SkipRule();
        int num2 = this.m_nPtr;
        this.ParseProperties(this.m_pCSS, num1 - 1, num2 - num1 - 1);
        this.m_nNextSymbol = CssSymbol.cssUnknown;
      }
      if (this.m_pCurrentRule.m_arrProperties.Count > 0)
        this.m_pCurrentRule.m_bValid = true;
      if (this.m_pCurrentRule.m_arrSelectors.Count > 0 && this.m_pCurrentRule.m_arrSelectors[this.m_pCurrentRule.m_arrSelectors.Count - 1].m_bstrPseudo != null && string.Compare(this.m_pCurrentRule.m_arrSelectors[this.m_pCurrentRule.m_arrSelectors.Count - 1].m_bstrPseudo, "link", true) != 0)
        this.m_pCurrentRule.m_bValid = false;
      foreach (HtmlCSSRule htmlCssRule in this.m_arrRules)
      {
        if (htmlCssRule.m_bNeedsCopying && htmlCssRule != this.m_pCurrentRule)
        {
          htmlCssRule.CopyFrom(this.m_pCurrentRule, false);
          htmlCssRule.m_bNeedsCopying = false;
          htmlCssRule.m_bValid = true;
        }
      }
      return true;
    }

    private bool SkipRule()
    {
      while ((int) this.m_c != 125)
        this.Next();
      this.Next();
      return false;
    }

    private void Parse()
    {
      try
      {
        this.Next();
        while (true)
          this.ParseRule();
      }
      catch (PdfException ex)
      {
      }
    }

    private void LoadBufferAndParse(byte[] pString)
    {
      HtmlManager htmlManager = new HtmlManager();
      htmlManager.m_pParam = new PdfParam();
      htmlManager.m_objContent = pString;
      htmlManager.ConvertToUnicode();
      this.LoadBufferAndParse(htmlManager.m_pBuffer, 0, htmlManager.m_nBufferSize);
    }

    private void LoadBufferAndParse(char[] pString, int nStart, int nLen)
    {
      this.m_pCSS = new char[nLen + 2];
      Array.Copy((Array) pString, nStart, (Array) this.m_pCSS, 0, nLen);
      this.m_pCSS[nLen] = ' ';
      this.m_pCSS[nLen + 1] = char.MinValue;
      this.m_nSize = nLen + 1;
      this.m_nPtr = 0;
      this.Parse();
    }

    private TagType IdentifyTag()
    {
      for (int index = 0; index < HtmlTag.m_arrTags.Length; ++index)
      {
        if (string.Compare(((object) this.m_szSymbol).ToString(), HtmlTag.m_arrTags[index].m_szName, true) == 0)
          return HtmlTag.m_arrTags[index].m_nType;
      }
      return TagType.TAG_UNKNOWN;
    }

    private void PushRelativePath(string Path)
    {
      HtmlXmlUrl htmlXmlUrl = new HtmlXmlUrl(this.m_pHtmlManager);
      if (this.m_bstrCurrentRelativePath == null || this.m_bstrCurrentRelativePath.Length == 0)
        htmlXmlUrl.MakeUrl(this.m_pHtmlManager.m_pBaseUrl, Path);
      else
        htmlXmlUrl.MakeUrl(this.m_bstrCurrentRelativePath, Path);
      this.m_arrRelativePathStack.Add(htmlXmlUrl.m_bstrUrl);
      this.m_bstrCurrentRelativePath = htmlXmlUrl.m_bstrUrl;
    }

    private void PopRelativePath()
    {
      this.m_bstrCurrentRelativePath = (string) null;
      if (this.m_arrRelativePathStack.Count <= 0)
        return;
      this.m_arrRelativePathStack.RemoveAt(this.m_arrRelativePathStack.Count - 1);
      if (this.m_arrRelativePathStack.Count <= 0)
        return;
      this.m_bstrCurrentRelativePath = this.m_arrRelativePathStack[this.m_arrRelativePathStack.Count - 1];
    }

    public bool ParseProperties(char[] sz, int nFrom, int nLen)
    {
      HtmlCSS pCSS = new HtmlCSS();
      pCSS.m_pCSS = new char[nLen + 2];
      Array.Copy((Array) sz, nFrom, (Array) pCSS.m_pCSS, 0, nLen);
      pCSS.m_pCSS[nLen] = ';';
      pCSS.m_pCSS[nLen + 1] = char.MinValue;
      pCSS.m_nSize = nLen + 1;
      try
      {
        pCSS.Next();
        pCSS.SkipSpaces();
        pCSS.NextSymbol();
        while (true)
          this.ParseProperty(pCSS);
      }
      catch (PdfException ex)
      {
      }
      return true;
    }

    private bool SkipProperty()
    {
      while ((int) this.m_c != 59)
        this.Next();
      this.Next();
      this.SkipSpaces();
      this.NextSymbol();
      return false;
    }

    private CssProperties LookupName(string szName)
    {
      if (szName == null)
        return CssProperties.NOTFOUND;
      int num1 = 0;
      int num2 = HtmlCSS.m_arrPropNames.Length - 1;
      while (num1 <= num2)
      {
        int index = (num1 + num2) / 2;
        int num3 = string.Compare(szName, HtmlCSS.m_arrPropNames[index].m_szName, StringComparison.OrdinalIgnoreCase);
        if (num3 == 0)
          return HtmlCSS.m_arrPropNames[index].m_nProp;
        if (num3 > 0)
          num1 = index + 1;
        else
          num2 = index - 1;
      }
      return CssProperties.NOTFOUND;
    }

    private CSSRegistry LookupPropertyName(string szName)
    {
      CssProperties cssProperties = this.LookupName(szName);
      if (cssProperties == CssProperties.NOTFOUND)
        return (CSSRegistry) null;
      for (int index = 0; index < HtmlCSS.m_arrPropRegistry.Length; ++index)
      {
        if (cssProperties == HtmlCSS.m_arrPropRegistry[index].m_nProp)
          return HtmlCSS.m_arrPropRegistry[index];
      }
      return (CSSRegistry) null;
    }

    private CssProperties LookupValue(string szName)
    {
      if (szName == null)
        return CssProperties.NOTFOUND;
      int num1 = 0;
      int num2 = HtmlCSS.m_arrPropValues.Length - 1;
      while (num1 <= num2)
      {
        int index = (num1 + num2) / 2;
        int num3 = string.Compare(szName, HtmlCSS.m_arrPropValues[index].m_szName, StringComparison.OrdinalIgnoreCase);
        if (num3 == 0)
          return HtmlCSS.m_arrPropValues[index].m_nProp;
        if (num3 > 0)
          num1 = index + 1;
        else
          num2 = index - 1;
      }
      return CssProperties.NOTFOUND;
    }

    public bool ParseProperty(HtmlCSS pCSS)
    {
      CSSRegistry pProp = this.LookupPropertyName(((object) pCSS.m_szSymbol).ToString());
      if (pProp == null)
      {
        pCSS.SkipProperty();
        return true;
      }
      else
      {
        HtmlCSSProperty pProperty = new HtmlCSSProperty(pProp.m_nProp, pProp.m_nType);
        pProperty.m_nName = pProp.m_nProp;
        pProperty.m_nType = pProp.m_nType;
        pProperty.m_bInheritable = pProp.m_bInheritable;
        pCSS.SkipSpaces();
        pCSS.NextSymbol();
        if (pCSS.m_nNextSymbol != CssSymbol.cssColon)
        {
          pCSS.SkipProperty();
          return true;
        }
        else
        {
          pCSS.SkipSpaces();
          int pPropertyCount = 0;
          if (pProp.m_nType == CssPropertyType.PROP_NUMBER)
          {
            if (!this.ParseNumericValue(pCSS, pProp, pProperty))
            {
              pCSS.SkipProperty();
              return true;
            }
          }
          else if (pProp.m_nType == CssPropertyType.PROP_2NUMBERS)
          {
            if (!this.ParseTwoNumericValues(pCSS, pProp, pProperty))
            {
              pCSS.SkipProperty();
              return true;
            }
          }
          else if (pProp.m_nType == CssPropertyType.PROP_ENUM)
          {
            if (!this.ParseEnumValue(pCSS, pProp, pProperty))
            {
              pCSS.SkipProperty();
              return true;
            }
          }
          else if (pProp.m_nType == CssPropertyType.PROP_STRING)
          {
            if (!this.ParseStringValue(pCSS, pProp, pProperty))
            {
              pCSS.SkipProperty();
              return true;
            }
          }
          else if (pProp.m_nType == CssPropertyType.PROP_COLOR)
          {
            if (!this.ParseColorValue(pCSS, pProp, pProperty))
            {
              pCSS.SkipProperty();
              return true;
            }
          }
          else if (pProp.m_nType == CssPropertyType.PROP_IMAGE)
          {
            if (!this.ParseImageValue(pCSS, pProp, pProperty))
            {
              pCSS.SkipProperty();
              return true;
            }
          }
          else if (pProp.m_nType == CssPropertyType.PROP_SHORTCUT && !this.ParseShortcutValue(pCSS, pProp, pProperty, ref pPropertyCount))
          {
            pCSS.SkipProperty();
            return true;
          }
          if (pPropertyCount == 0)
            this.m_pCurrentRule.m_arrProperties.Add(pProperty);
          pCSS.SkipSpaces();
          pCSS.NextSymbol();
          if (pCSS.m_nNextSymbol == CssSymbol.cssExclaim)
          {
            pCSS.SkipSpaces();
            pCSS.NextSymbol();
            if (pCSS.m_nNextSymbol == CssSymbol.cssName && string.Compare(((object) pCSS.m_szSymbol).ToString(), "important", true) == 0)
            {
              pProperty.m_bImportant = true;
              if (pPropertyCount > 0)
              {
                for (int index = this.m_pCurrentRule.m_arrProperties.Count - 1; index >= 0 && pPropertyCount > 0; --index)
                {
                  this.m_pCurrentRule.m_arrProperties[index].m_bImportant = true;
                  --pPropertyCount;
                }
              }
              pCSS.SkipSpaces();
              pCSS.NextSymbol();
            }
          }
          if (pCSS.m_nNextSymbol == CssSymbol.cssSemicolon)
          {
            pCSS.SkipSpaces();
            pCSS.NextSymbol();
          }
          return true;
        }
      }
    }

    private bool ParseNumericValue(HtmlCSS pCSS, CSSRegistry pProp, HtmlCSSProperty pProperty)
    {
      pProperty.m_nValue = CssProperties.NOTFOUND;
      pProperty.m_bPercent = false;
      if (((int) pCSS.m_c < 48 || (int) pCSS.m_c > 57) && ((int) pCSS.m_c != 46 && (int) pCSS.m_c != 45))
        return this.ParseEnumValue(pCSS, pProp, pProperty);
      int num1 = 1;
      if ((int) pCSS.m_c == 45)
      {
        num1 = -1;
        pCSS.Next();
      }
      int num2 = 0;
      int num3 = 0;
      int num4 = 1;
      while ((int) pCSS.m_c >= 48 && (int) pCSS.m_c <= 57)
      {
        num2 = num2 * 10 + ((int) pCSS.m_c - 48);
        pCSS.Next();
      }
      if ((int) pCSS.m_c == 46)
      {
        pCSS.Next();
        while ((int) pCSS.m_c >= 48 && (int) pCSS.m_c <= 57)
        {
          num3 = num3 * 10 + ((int) pCSS.m_c - 48);
          num4 *= 10;
          pCSS.Next();
        }
      }
      float num5 = (float) num1 * ((float) num2 + (float) num3 / (float) num4);
      if ((int) pCSS.m_c == 37)
      {
        pProperty.m_bPercent = true;
        pProperty.m_bNoMeasurementUnit = true;
        pCSS.Next();
      }
      else
      {
        string strA = "";
        int num6 = 0;
        while (HtmlObject.IsLetter(pCSS.m_c))
        {
          strA = strA + (object) pCSS.m_c;
          ++num6;
          pCSS.Next();
          if (num6 > 1)
            break;
        }
        if (num6 == 2)
        {
          if (string.Compare(strA, "in", true) == 0)
            num5 *= 72f;
          else if (string.Compare(strA, "cm", true) == 0)
            num5 *= 28.3465f;
          else if (string.Compare(strA, "mm", true) == 0)
            num5 *= 2.83465f;
          else if (string.Compare(strA, "pc", true) == 0)
            num5 *= 12f;
          else if (string.Compare(strA, "px", true) == 0)
            num5 *= HtmlManager.fEmpiricalScaleFactor;
          else if (string.Compare(strA, "em", true) == 0)
          {
            num5 *= 100f;
            pProperty.m_bPercent = true;
            pProperty.m_bNoMeasurementUnit = true;
          }
        }
        else if (num6 == 0)
        {
          num5 *= HtmlManager.fEmpiricalScaleFactor;
          pProperty.m_bNoMeasurementUnit = true;
        }
      }
      pProperty.m_fValue = num5;
      return true;
    }

    private bool ParseTwoNumericValues(HtmlCSS pCSS, CSSRegistry pProp, HtmlCSSProperty pProperty)
    {
      if (!this.ParseNumericValue(pCSS, pProp, pProperty))
        return false;
      pProperty.m_nValue2 = CssProperties.NOTFOUND;
      float num = pProperty.m_fValue;
      CssProperties cssProperties = pProperty.m_nValue;
      pCSS.SkipSpaces();
      if ((int) pCSS.m_c == 59 || pCSS.m_nPtr >= pCSS.m_nSize)
        return true;
      if (!this.ParseNumericValue(pCSS, pProp, pProperty))
        return false;
      pProperty.m_fValue2 = pProperty.m_fValue;
      pProperty.m_nValue2 = pProperty.m_nValue;
      pProperty.m_fValue = num;
      pProperty.m_nValue = cssProperties;
      return true;
    }

    private bool ParseEnumValue(HtmlCSS pCSS, CSSRegistry pProp, HtmlCSSProperty pProperty)
    {
      if (pProp.m_arrValues[0] == CssProperties.V_0)
        return false;
      pCSS.NextSymbol();
      if (pCSS.m_nNextSymbol != CssSymbol.cssName)
        return false;
      CssProperties cssProperties = this.LookupValue(((object) pCSS.m_szSymbol).ToString());
      if (cssProperties == CssProperties.NOTFOUND)
        return false;
      int index = 0;
      bool flag = false;
      for (; pProp.m_arrValues[index] != CssProperties.V_0; ++index)
      {
        if (pProp.m_arrValues[index] == cssProperties)
        {
          pProperty.m_nValue = cssProperties;
          flag = true;
          break;
        }
      }
      return flag;
    }

    private bool ParseStringValue(HtmlCSS pCSS, CSSRegistry pProp, HtmlCSSProperty pProperty)
    {
      char ch = ';';
      if ((int) pCSS.m_c == 34 || (int) pCSS.m_c == 39)
      {
        ch = pCSS.m_c;
        pCSS.Next();
        pCSS.SkipSpaces();
      }
      int startIndex = pCSS.m_nPtr - 1;
      while ((int) pCSS.m_c != (int) ch && (int) pCSS.m_c != 33 && pCSS.m_nPtr < pCSS.m_nSize)
        pCSS.Next();
      int num = pCSS.m_nPtr - 2;
      if (startIndex > num)
        return false;
      pProperty.m_bstrValue = new string(pCSS.m_pCSS, startIndex, num - startIndex + 1);
      if (((int) ch == 34 || (int) ch == 39) && (int) pCSS.m_c == (int) ch)
        pCSS.Next();
      HtmlObject.Trim(ref pProperty.m_bstrValue);
      return true;
    }

    private bool ParseColorValue(HtmlCSS pCSS, CSSRegistry pProp, HtmlCSSProperty pProperty)
    {
      float num1 = 0.0f;
      if ((int) pCSS.m_c == 35)
      {
        pCSS.Next();
        int num2 = 0;
        float num3 = 0.0f;
        for (; num2 < 6 && pCSS.m_nPtr < pCSS.m_nSize && ((int) pCSS.m_c != 59 && !HtmlObject.IsSpace(pCSS.m_c)); ++num2)
        {
          if ((int) pCSS.m_c >= 48 && (int) pCSS.m_c <= 57)
          {
            num1 = 16f * num1 + (float) ((int) pCSS.m_c - 48);
            num3 = 256f * num3 + (float) (16 * ((int) pCSS.m_c - 48)) + (float) ((int) pCSS.m_c - 48);
          }
          if ((int) pCSS.m_c >= 97 && (int) pCSS.m_c <= 102)
          {
            num1 = 16f * num1 + (float) ((int) pCSS.m_c - 97 + 10);
            num3 = 256f * num3 + (float) (16 * ((int) pCSS.m_c - 97 + 10)) + (float) ((int) pCSS.m_c - 97 + 10);
          }
          if ((int) pCSS.m_c >= 65 && (int) pCSS.m_c <= 70)
          {
            num1 = 16f * num1 + (float) ((int) pCSS.m_c - 65 + 10);
            num3 = 256f * num3 + (float) (16 * ((int) pCSS.m_c - 65 + 10)) + (float) ((int) pCSS.m_c - 65 + 10);
          }
          pCSS.Next();
        }
        if (num2 == 3)
          num1 = num3;
      }
      else
      {
        pCSS.NextSymbol();
        if (pCSS.m_nNextSymbol != CssSymbol.cssName)
          return false;
        if (string.Compare(((object) pCSS.m_szSymbol).ToString(), "rgb", true) == 0)
        {
          pCSS.SkipSpaces();
          pCSS.NextSymbol();
          if (pCSS.m_nNextSymbol != CssSymbol.cssOpenParent)
            return false;
          int[] numArray = new int[3];
          for (int index = 0; index < 3; ++index)
          {
            pCSS.SkipSpaces();
            this.ParseNumericValue(pCSS, pProp, pProperty);
            numArray[index] = (int) ((double) pProperty.m_fValue / (double) HtmlManager.fEmpiricalScaleFactor);
            if (numArray[index] < 0 || numArray[index] > (int) byte.MaxValue)
              return false;
            if (index < 2)
            {
              pCSS.SkipSpaces();
              pCSS.NextSymbol();
              if (pCSS.m_nNextSymbol != CssSymbol.cssComma)
                return false;
            }
          }
          num1 = (float) (65536 * numArray[0] + 256 * numArray[1] + numArray[2]);
          pCSS.SkipSpaces();
          if ((int) pCSS.m_c == 41)
            pCSS.Next();
        }
        else if (!PdfParam.ColorNameToValue(((object) pCSS.m_szSymbol).ToString(), ref num1))
          return false;
      }
      pProperty.m_rgbColor.Set((uint) num1);
      return true;
    }

    private bool ParseImageValue(HtmlCSS pCSS, CSSRegistry pProp, HtmlCSSProperty pProperty)
    {
      if ((int) pCSS.m_c != 117 && (int) pCSS.m_c != 85)
        return false;
      pCSS.NextSymbol();
      if (string.Compare(((object) pCSS.m_szSymbol).ToString(), "url", true) != 0)
        return false;
      pCSS.SkipSpaces();
      pCSS.NextSymbol();
      if (pCSS.m_nNextSymbol != CssSymbol.cssOpenParent)
        return false;
      char ch1 = char.MinValue;
      pCSS.SkipSpaces();
      if ((int) pCSS.m_c == 34 || (int) pCSS.m_c == 39)
      {
        ch1 = pCSS.m_c;
        pCSS.Next();
        pCSS.SkipSpaces();
      }
      int startIndex = pCSS.m_nPtr - 1;
      char ch2 = (int) ch1 == 0 ? ')' : ch1;
      while ((int) pCSS.m_c != (int) ch2)
        pCSS.Next();
      pCSS.SkipSpaces();
      pCSS.NextSymbol();
      int num = pCSS.m_nPtr;
      if ((int) ch1 != 0)
      {
        pCSS.SkipSpaces();
        pCSS.NextSymbol();
      }
      pProperty.m_bstrValue = new string(pCSS.m_pCSS, startIndex, num - startIndex - 2);
      HtmlObject.Trim(ref pProperty.m_bstrValue);
      if (this.m_bstrCurrentRelativePath != null && this.m_bstrCurrentRelativePath.Length > 0 && pProperty.m_bstrValue.Length > 0)
      {
        HtmlXmlUrl htmlXmlUrl = new HtmlXmlUrl(this.m_pHtmlManager);
        htmlXmlUrl.MakeUrl(this.m_bstrCurrentRelativePath, pProperty.m_bstrValue);
        pProperty.m_bstrValue = htmlXmlUrl.m_bstrUrl;
      }
      return true;
    }

    private bool ParseMultipleValuesInAnyOrder(HtmlCSS pCSS, ref CSSRegistry[] pProps, int nPropCount, HtmlCSSProperty pProperty, ref int pPropertyCount)
    {
      bool flag1 = false;
      List<bool> list = new List<bool>();
      for (int index = 0; index < nPropCount; ++index)
        list.Add(false);
      bool flag2 = true;
      while (flag2)
      {
        bool flag3 = false;
        for (int index = 0; index < nPropCount; ++index)
        {
          if (!list[index])
          {
            pCSS.SaveState();
            switch (pProps[index].m_nType)
            {
              case CssPropertyType.PROP_NUMBER:
                flag1 = this.ParseNumericValue(pCSS, pProps[index], pProperty);
                break;
              case CssPropertyType.PROP_ENUM:
                flag1 = this.ParseEnumValue(pCSS, pProps[index], pProperty);
                break;
              case CssPropertyType.PROP_IMAGE:
                flag1 = this.ParseImageValue(pCSS, pProps[index], pProperty);
                break;
              case CssPropertyType.PROP_COLOR:
                flag1 = this.ParseColorValue(pCSS, pProps[index], pProperty);
                break;
              case CssPropertyType.PROP_2NUMBERS:
                flag1 = this.ParseTwoNumericValues(pCSS, pProps[index], pProperty);
                break;
            }
            if (flag1)
            {
              list[index] = true;
              flag3 = true;
              HtmlCSSProperty htmlCssProperty = new HtmlCSSProperty(pProps[index].m_nProp, pProps[index].m_nType);
              htmlCssProperty.CopyFrom(pProperty);
              this.m_pCurrentRule.m_arrProperties.Add(htmlCssProperty);
              ++pPropertyCount;
              pCSS.SkipSpaces();
              if ((int) pCSS.m_c == 59 || (int) pCSS.m_c == 33 || pCSS.m_nPtr >= pCSS.m_nSize)
              {
                flag2 = false;
                break;
              }
              else
                break;
            }
            else
              pCSS.RestoreState();
          }
        }
        if (!flag3)
        {
          pCSS.NextSymbol();
          pCSS.SkipSpaces();
          if ((int) pCSS.m_c == 59 || (int) pCSS.m_c == 33 || pCSS.m_nPtr >= pCSS.m_nSize)
            flag2 = false;
        }
      }
      return true;
    }

    private bool ParseShortcutValue(HtmlCSS pCSS, CSSRegistry pProp, HtmlCSSProperty pProperty, ref int pPropertyCount)
    {
      pPropertyCount = 0;
      if (pProp.m_nProp == CssProperties.FONT)
      {
        CSSRegistry pProp1 = this.LookupPropertyName("font-style");
        pCSS.SaveState();
        if (this.ParseEnumValue(pCSS, pProp1, pProperty))
        {
          HtmlCSSProperty htmlCssProperty = new HtmlCSSProperty(pProp1.m_nProp, pProp1.m_nType);
          htmlCssProperty.m_nValue = pProperty.m_nValue;
          this.m_pCurrentRule.m_arrProperties.Add(htmlCssProperty);
          ++pPropertyCount;
        }
        else
          pCSS.RestoreState();
        pCSS.SkipSpaces();
        if ((int) pCSS.m_c == 33)
          return true;
        CSSRegistry pProp2 = this.LookupPropertyName("font-variant");
        pCSS.SaveState();
        if (this.ParseEnumValue(pCSS, pProp2, pProperty))
        {
          HtmlCSSProperty htmlCssProperty = new HtmlCSSProperty(pProp2.m_nProp, pProp2.m_nType);
          htmlCssProperty.m_nValue = pProperty.m_nValue;
          this.m_pCurrentRule.m_arrProperties.Add(htmlCssProperty);
          ++pPropertyCount;
        }
        else
          pCSS.RestoreState();
        pCSS.SkipSpaces();
        if ((int) pCSS.m_c == 33)
          return true;
        CSSRegistry pProp3 = this.LookupPropertyName("font-weight");
        pCSS.SaveState();
        pProperty.m_nValue = CssProperties.NOTFOUND;
        if (this.ParseNumericValue(pCSS, pProp3, pProperty))
        {
          if (pProperty.m_nValue == CssProperties.NOTFOUND && (double) pProperty.m_fValue != 100.0 && ((double) pProperty.m_fValue != 200.0 && (double) pProperty.m_fValue != 300.0) && ((double) pProperty.m_fValue != 400.0 && (double) pProperty.m_fValue != 500.0 && ((double) pProperty.m_fValue != 600.0 && (double) pProperty.m_fValue != 700.0)) && ((double) pProperty.m_fValue != 800.0 && (double) pProperty.m_fValue != 900.0))
          {
            pCSS.RestoreState();
          }
          else
          {
            HtmlCSSProperty htmlCssProperty = new HtmlCSSProperty(pProp3.m_nProp, pProp3.m_nType);
            htmlCssProperty.CopyFrom(pProperty);
            this.m_pCurrentRule.m_arrProperties.Add(htmlCssProperty);
            ++pPropertyCount;
          }
        }
        else
          pCSS.RestoreState();
        pCSS.SkipSpaces();
        if ((int) pCSS.m_c == 33)
          return true;
        CSSRegistry pProp4 = this.LookupPropertyName("font-size");
        pCSS.SaveState();
        if (this.ParseNumericValue(pCSS, pProp4, pProperty))
        {
          HtmlCSSProperty htmlCssProperty = new HtmlCSSProperty(pProp4.m_nProp, pProp4.m_nType);
          htmlCssProperty.CopyFrom(pProperty);
          this.m_pCurrentRule.m_arrProperties.Add(htmlCssProperty);
          ++pPropertyCount;
        }
        else
          pCSS.RestoreState();
        if ((int) pCSS.m_c == 47)
        {
          pCSS.Next();
          CSSRegistry pProp5 = this.LookupPropertyName("line-height");
          pCSS.SaveState();
          if (this.ParseNumericValue(pCSS, pProp5, pProperty))
          {
            HtmlCSSProperty htmlCssProperty = new HtmlCSSProperty(pProp5.m_nProp, pProp5.m_nType);
            htmlCssProperty.CopyFrom(pProperty);
            this.m_pCurrentRule.m_arrProperties.Add(htmlCssProperty);
            ++pPropertyCount;
          }
          else
            pCSS.RestoreState();
        }
        pCSS.SkipSpaces();
        if ((int) pCSS.m_c == 33)
          return true;
        CSSRegistry pProp6 = this.LookupPropertyName("font-family");
        if (this.ParseStringValue(pCSS, pProp6, pProperty))
        {
          HtmlCSSProperty htmlCssProperty = new HtmlCSSProperty(pProp6.m_nProp, pProp6.m_nType);
          htmlCssProperty.m_bstrValue = pProperty.m_bstrValue;
          this.m_pCurrentRule.m_arrProperties.Add(htmlCssProperty);
          ++pPropertyCount;
        }
      }
      else if (pProp.m_nProp == CssProperties.LIST_STYLE)
      {
        CSSRegistry[] pProps = new CSSRegistry[3]
        {
          this.LookupPropertyName("list-style-type"),
          this.LookupPropertyName("list-style-image"),
          this.LookupPropertyName("list-style-position")
        };
        this.ParseMultipleValuesInAnyOrder(pCSS, ref pProps, 3, pProperty, ref pPropertyCount);
      }
      else if (pProp.m_nProp == CssProperties.BACKGROUND)
      {
        CSSRegistry[] pProps = new CSSRegistry[5]
        {
          this.LookupPropertyName("background-image"),
          this.LookupPropertyName("background-color"),
          this.LookupPropertyName("background-repeat"),
          this.LookupPropertyName("background-attachment"),
          this.LookupPropertyName("background-position")
        };
        this.ParseMultipleValuesInAnyOrder(pCSS, ref pProps, 5, pProperty, ref pPropertyCount);
      }
      else if (pProp.m_nProp == CssProperties.MARGIN || pProp.m_nProp == CssProperties.BORDER_WIDTH || (pProp.m_nProp == CssProperties.BORDER_COLOR || pProp.m_nProp == CssProperties.PADDING))
      {
        CSSRegistry[] cssRegistryArray = new CSSRegistry[4];
        switch (pProp.m_nProp)
        {
          case CssProperties.MARGIN:
            cssRegistryArray[0] = this.LookupPropertyName("margin-top");
            cssRegistryArray[1] = this.LookupPropertyName("margin-right");
            cssRegistryArray[2] = this.LookupPropertyName("margin-bottom");
            cssRegistryArray[3] = this.LookupPropertyName("margin-left");
            break;
          case CssProperties.PADDING:
            cssRegistryArray[0] = this.LookupPropertyName("padding-top");
            cssRegistryArray[1] = this.LookupPropertyName("padding-right");
            cssRegistryArray[2] = this.LookupPropertyName("padding-bottom");
            cssRegistryArray[3] = this.LookupPropertyName("padding-left");
            break;
          case CssProperties.BORDER_COLOR:
            cssRegistryArray[0] = this.LookupPropertyName("border-top-color");
            cssRegistryArray[1] = this.LookupPropertyName("border-right-color");
            cssRegistryArray[2] = this.LookupPropertyName("border-bottom-color");
            cssRegistryArray[3] = this.LookupPropertyName("border-left-color");
            break;
          case CssProperties.BORDER_WIDTH:
            cssRegistryArray[0] = this.LookupPropertyName("border-top-width");
            cssRegistryArray[1] = this.LookupPropertyName("border-right-width");
            cssRegistryArray[2] = this.LookupPropertyName("border-bottom-width");
            cssRegistryArray[3] = this.LookupPropertyName("border-left-width");
            break;
        }
        float[] numArray = new float[4];
        CssProperties[] cssPropertiesArray = new CssProperties[4];
        AuxRGB[] auxRgbArray = new AuxRGB[4];
        for (int index = 0; index < 4; ++index)
          auxRgbArray[index] = new AuxRGB();
        int index1 = 0;
        for (int index2 = 0; index2 < 4; ++index2)
        {
          if (pProp.m_nProp == CssProperties.BORDER_COLOR)
          {
            if (!this.ParseColorValue(pCSS, cssRegistryArray[index2], pProperty))
              continue;
          }
          else if (!this.ParseNumericValue(pCSS, cssRegistryArray[index2], pProperty))
            continue;
          numArray[index1] = pProperty.m_fValue;
          cssPropertiesArray[index1] = pProperty.m_nValue;
          if (pProp.m_nProp == CssProperties.BORDER_COLOR)
            auxRgbArray[index1].Set(ref pProperty.m_rgbColor);
          ++index1;
          pCSS.SkipSpaces();
          if ((int) pCSS.m_c == 59 || pCSS.m_nPtr >= pCSS.m_nSize)
            break;
        }
        if (index1 == 0)
          return false;
        if (index1 == 1)
        {
          numArray[1] = numArray[2] = numArray[3] = numArray[0];
          cssPropertiesArray[1] = cssPropertiesArray[2] = cssPropertiesArray[3] = cssPropertiesArray[0];
          if (pProp.m_nProp == CssProperties.BORDER_COLOR)
          {
            auxRgbArray[1].Set(ref auxRgbArray[0]);
            auxRgbArray[2].Set(ref auxRgbArray[0]);
            auxRgbArray[3].Set(ref auxRgbArray[0]);
          }
        }
        else if (index1 == 2)
        {
          numArray[2] = numArray[0];
          numArray[3] = numArray[1];
          cssPropertiesArray[2] = cssPropertiesArray[0];
          cssPropertiesArray[3] = cssPropertiesArray[1];
          if (pProp.m_nProp == CssProperties.BORDER_COLOR)
          {
            auxRgbArray[2].Set(ref auxRgbArray[0]);
            auxRgbArray[3].Set(ref auxRgbArray[1]);
          }
        }
        else if (index1 == 3)
        {
          numArray[3] = numArray[1];
          cssPropertiesArray[3] = cssPropertiesArray[1];
          if (pProp.m_nProp == CssProperties.BORDER_COLOR)
            auxRgbArray[3].Set(ref auxRgbArray[1]);
        }
        for (int index2 = 0; index2 < 4; ++index2)
        {
          HtmlCSSProperty htmlCssProperty = new HtmlCSSProperty(cssRegistryArray[index2].m_nProp, cssRegistryArray[index2].m_nType);
          htmlCssProperty.m_fValue = numArray[index2];
          htmlCssProperty.m_nValue = cssPropertiesArray[index2];
          if (pProp.m_nProp == CssProperties.BORDER_COLOR)
            htmlCssProperty.m_rgbColor.Set(ref auxRgbArray[index2]);
          this.m_pCurrentRule.m_arrProperties.Add(htmlCssProperty);
          ++pPropertyCount;
        }
      }
      else if (pProp.m_nProp == CssProperties.BORDER_STYLE)
      {
        CSSRegistry[] cssRegistryArray = new CSSRegistry[4]
        {
          this.LookupPropertyName("border-top-style"),
          this.LookupPropertyName("border-right-style"),
          this.LookupPropertyName("border-bottom-style"),
          this.LookupPropertyName("border-left-style")
        };
        CssProperties[] cssPropertiesArray = new CssProperties[4];
        int num = 0;
        for (int index = 0; index < 4; ++index)
        {
          if (this.ParseEnumValue(pCSS, cssRegistryArray[index], pProperty))
          {
            cssPropertiesArray[num++] = pProperty.m_nValue;
            pCSS.SkipSpaces();
            if ((int) pCSS.m_c == 59 || pCSS.m_nPtr >= pCSS.m_nSize)
              break;
          }
        }
        if (num == 0)
          return false;
        if (num == 1)
          cssPropertiesArray[1] = cssPropertiesArray[2] = cssPropertiesArray[3] = cssPropertiesArray[0];
        else if (num == 2)
        {
          cssPropertiesArray[2] = cssPropertiesArray[0];
          cssPropertiesArray[3] = cssPropertiesArray[1];
        }
        else if (num == 3)
          cssPropertiesArray[3] = cssPropertiesArray[1];
        for (int index = 0; index < 4; ++index)
        {
          HtmlCSSProperty htmlCssProperty = new HtmlCSSProperty(cssRegistryArray[index].m_nProp, cssRegistryArray[index].m_nType);
          htmlCssProperty.m_nValue = cssPropertiesArray[index];
          this.m_pCurrentRule.m_arrProperties.Add(htmlCssProperty);
          ++pPropertyCount;
        }
      }
      else if (pProp.m_nProp == CssProperties.BORDER_TOP || pProp.m_nProp == CssProperties.BORDER_RIGHT || (pProp.m_nProp == CssProperties.BORDER_BOTTOM || pProp.m_nProp == CssProperties.BORDER_LEFT))
      {
        string szName1 = (string) null;
        string szName2 = (string) null;
        string szName3 = (string) null;
        switch (pProp.m_nProp)
        {
          case CssProperties.BORDER_BOTTOM:
            szName1 = "border-bottom-color";
            szName2 = "border-bottom-style";
            szName3 = "border-bottom-width";
            break;
          case CssProperties.BORDER_LEFT:
            szName1 = "border-left-color";
            szName2 = "border-left-style";
            szName3 = "border-left-width";
            break;
          case CssProperties.BORDER_RIGHT:
            szName1 = "border-right-color";
            szName2 = "border-right-style";
            szName3 = "border-right-width";
            break;
          case CssProperties.BORDER_TOP:
            szName1 = "border-top-color";
            szName2 = "border-top-style";
            szName3 = "border-top-width";
            break;
        }
        CSSRegistry[] pProps = new CSSRegistry[3]
        {
          this.LookupPropertyName(szName1),
          this.LookupPropertyName(szName2),
          this.LookupPropertyName(szName3)
        };
        this.ParseMultipleValuesInAnyOrder(pCSS, ref pProps, 3, pProperty, ref pPropertyCount);
      }
      else if (pProp.m_nProp == CssProperties.BORDER)
      {
        CSSRegistry[] pProps = new CSSRegistry[3]
        {
          this.LookupPropertyName("border-top-color"),
          this.LookupPropertyName("border-top-style"),
          this.LookupPropertyName("border-top-width")
        };
        CssProperties[][] cssPropertiesArray = new CssProperties[3][]
        {
          new CssProperties[3]
          {
            CssProperties.BORDER_RIGHT_COLOR,
            CssProperties.BORDER_BOTTOM_COLOR,
            CssProperties.BORDER_LEFT_COLOR
          },
          new CssProperties[3]
          {
            CssProperties.BORDER_RIGHT_STYLE,
            CssProperties.BORDER_BOTTOM_STYLE,
            CssProperties.BORDER_LEFT_STYLE
          },
          new CssProperties[3]
          {
            CssProperties.BORDER_RIGHT_WIDTH,
            CssProperties.BORDER_BOTTOM_WIDTH,
            CssProperties.BORDER_LEFT_WIDTH
          }
        };
        int num1 = pPropertyCount;
        this.ParseMultipleValuesInAnyOrder(pCSS, ref pProps, 3, pProperty, ref pPropertyCount);
        int num2 = pPropertyCount - num1;
        int count = this.m_pCurrentRule.m_arrProperties.Count;
        for (int index1 = 0; index1 < num2; ++index1)
        {
          HtmlCSSProperty htmlCssProperty1 = this.m_pCurrentRule.m_arrProperties[count - index1 - 1];
          int index2 = 0;
          switch (htmlCssProperty1.m_nName)
          {
            case CssProperties.BORDER_TOP_COLOR:
              index2 = 0;
              break;
            case CssProperties.BORDER_TOP_STYLE:
              index2 = 1;
              break;
            case CssProperties.BORDER_TOP_WIDTH:
              index2 = 2;
              break;
          }
          for (int index3 = 0; index3 < 3; ++index3)
          {
            HtmlCSSProperty htmlCssProperty2 = new HtmlCSSProperty(htmlCssProperty1);
            htmlCssProperty2.m_nName = cssPropertiesArray[index2][index3];
            this.m_pCurrentRule.m_arrProperties.Add(htmlCssProperty2);
          }
        }
      }
      return true;
    }

    public void SaveState()
    {
      this.m_nSavePtr = this.m_nPtr;
      this.m_cSaveChar = this.m_c;
    }

    public void RestoreState()
    {
      this.m_nPtr = this.m_nSavePtr;
      this.m_c = this.m_cSaveChar;
    }
  }
}
