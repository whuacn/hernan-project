using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PDFLive.ConvertHTML
{
    public class HTMLFunctions
    {

        #region HTMLparser

        internal static Object getTagHeader(string sHTML, string baseurl)
        {
            PDFLive.ConvertHTML.Structures.TagHeader obj = new PDFLive.ConvertHTML.Structures.TagHeader();
            obj.Height = 0;
            obj.auth = "";
            obj.baseurl = "";

            try
            {
                DOL.DHtml.DHtmlParser.DHtmlDocument mDocument = new DOL.DHtml.DHtmlParser.DHtmlDocument(sHTML);

                //Obtengo el tag header y sus atributos si existe

                if (mDocument.Nodes.Count > 0)
                {

                    DOL.DHtml.DHtmlParser.Node.DHtmlElement node;

                    node = (DOL.DHtml.DHtmlParser.Node.DHtmlElement)mDocument.Nodes[mDocument.Nodes.Count - 1];

                    DOL.DHtml.DHtmlParser.DHtmlNodeCollection nodes = node.Nodes;

                    foreach (object objnodo in nodes)
                    {

                        if (objnodo.GetType().ToString() == "DOL.DHtml.DHtmlParser.Node.DHtmlElement")
                        {
                            DOL.DHtml.DHtmlParser.Node.DHtmlElement Elemento = (DOL.DHtml.DHtmlParser.Node.DHtmlElement)objnodo;

                            if (Elemento.NodeName == "header")
                            {

                                if (Elemento.Attributes.IndexOf("height") != -1)
                                    obj.Height = (float)Convert.ToDecimal(Elemento.Attributes["height"].Value);

                                obj.baseurl = baseurl;
                                obj.texto = Elemento.HTML;

                            }
                        }

                    }
                }


            }

            catch (Exception ex)
            {
                //Errorhandle("Error al traer el header",ex);

            }
            return obj;
        }

        internal static Object getTagFooter(string sHTML, string baseurl)
        {
            PDFLive.ConvertHTML.Structures.TagFooter obj = new PDFLive.ConvertHTML.Structures.TagFooter();
            obj.Height = 0;
            obj.auth = "";
            obj.baseurl = "";

            try
            {
                DOL.DHtml.DHtmlParser.DHtmlDocument mDocument = new DOL.DHtml.DHtmlParser.DHtmlDocument(sHTML);

                //Obtengo el tag header y sus atributos si existe

                if (mDocument.Nodes.Count > 0)
                {

                    DOL.DHtml.DHtmlParser.Node.DHtmlElement node;

                    node = (DOL.DHtml.DHtmlParser.Node.DHtmlElement)mDocument.Nodes[mDocument.Nodes.Count - 1];

                    DOL.DHtml.DHtmlParser.DHtmlNodeCollection nodes = node.Nodes;

                    foreach (object objnodo in nodes)
                    {

                        if (objnodo.GetType().ToString() == "DOL.DHtml.DHtmlParser.Node.DHtmlElement")
                        {
                            DOL.DHtml.DHtmlParser.Node.DHtmlElement Elemento = (DOL.DHtml.DHtmlParser.Node.DHtmlElement)objnodo;

                            if (Elemento.NodeName == "footer")
                            {

                                if (Elemento.Attributes.IndexOf("height") != -1)
                                    obj.Height = (float)Convert.ToDecimal(Elemento.Attributes["height"].Value);

                                obj.baseurl = baseurl;
                                obj.texto = Elemento.HTML;

                            }
                        }

                    }
                }


            }

            catch (Exception ex) { }


            return obj;
        }

        #endregion

        internal static void ClearCode(ref String _html)
        {

            if (_html.IndexOf("<header", StringComparison.OrdinalIgnoreCase) != -1)
                _html = _html.Remove(_html.IndexOf("<header", StringComparison.OrdinalIgnoreCase), _html.IndexOf("</header>", StringComparison.OrdinalIgnoreCase) + 9 - _html.IndexOf("<header", StringComparison.OrdinalIgnoreCase));

            if (_html.IndexOf("<footer", StringComparison.OrdinalIgnoreCase) != -1)
                _html = _html.Remove(_html.IndexOf("<footer", StringComparison.OrdinalIgnoreCase), _html.IndexOf("</footer>", StringComparison.OrdinalIgnoreCase) + 9 - _html.IndexOf("<footer", StringComparison.OrdinalIgnoreCase));

            //BORRO EL TITLE
            if (_html.IndexOf("<title", StringComparison.OrdinalIgnoreCase) != -1)
            {
                _html = _html.Remove(_html.IndexOf("<title", StringComparison.OrdinalIgnoreCase), _html.IndexOf("</title>", StringComparison.OrdinalIgnoreCase) + 8 - _html.IndexOf("<title", StringComparison.OrdinalIgnoreCase));
            }
            //-------------------------

            //BORRO EL CODIGO SCRIPT
            while (_html.IndexOf("<script", StringComparison.OrdinalIgnoreCase) != -1)
            {
                _html = _html.Remove(_html.IndexOf("<script", StringComparison.OrdinalIgnoreCase), _html.IndexOf("</script>", StringComparison.OrdinalIgnoreCase) + 9 - _html.IndexOf("<script", StringComparison.OrdinalIgnoreCase));
            }
            //-------------------------

            _html = _html.Replace("</p>", "</p><br />");

        }

        internal static iTextSharp.text.html.simpleparser.StyleSheet extractStyles(ref String _html)
        {
            DOL.DHtml.DCssResolver.DCssResolver m_cssResolver = new DOL.DHtml.DCssResolver.DCssResolver();
            List<DOL.DHtml.DCssResolver.DCssSelector> m_selectorList = new List<DOL.DHtml.DCssResolver.DCssSelector>();

            iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
            Regex expr = new Regex("<style[^>]*>[^<]*</style>", RegexOptions.IgnoreCase);
            MatchCollection mc = expr.Matches(_html);
            if (mc.Count > 0)
            {
                foreach (Match m in mc)
                {
                    String tagStyle = m.Result("$0");
                    _html = _html.Replace(tagStyle, "");


                    //---PARSEO DEL STYLE---//
                    String Class = "";

                    m_cssResolver.Resolve(tagStyle, m_selectorList);

                    for (int selectorIndex = 0, selectorCount = m_selectorList.Count; selectorIndex < selectorCount; ++selectorIndex)
                        m_selectorList[selectorIndex].Priority = selectorIndex;

                    m_selectorList.Sort();
                    Dictionary<string, string> h = new Dictionary<string, string>();
                    //foreach(DOL.DHtml.DCssResolver.DCssSelector selector in m_selectorList)
                    for (int i = 0; i < m_selectorList.Count; i++)
                    {

                        DOL.DHtml.DCssResolver.DCssSelector selector = (DOL.DHtml.DCssResolver.DCssSelector)m_selectorList[i];

                        foreach (DOL.DHtml.DCssResolver.DCssProperty property in selector.Properties)
                        {
                            String[] Attrib;
                            Attrib = property.CSS.Split(':');
                            if (Attrib.Length > 1)
                            {
                                String Key = Attrib[0];
                                String Value = Attrib[1].Replace(";", "");
                                h[Key] = Value;

                            }
                        }

                        if ((i + 1) < m_selectorList.Count)
                        {
                            DOL.DHtml.DCssResolver.DCssSelector selector_post = (DOL.DHtml.DCssResolver.DCssSelector)m_selectorList[i + 1];

                            if (selector_post.Selector.ToLower().Replace("<style>", "").Trim() != selector.Selector.ToLower().Replace("<style>", "").Trim())
                            {

                                Class = selector.Selector;
                                Class = Class.ToLower().Replace("<style>", "");

                                if (Class.IndexOf(".") != -1)
                                {
                                    Class = Class.Replace(".", "");
                                    Class = Class.Trim();
                                    styles.LoadStyle(Class, h);
                                }
                                else
                                {
                                    Class = Class.Trim();
                                    styles.LoadTagStyle(Class, h);
                                }
                                h = new Dictionary<string, string>();

                            }
                        }
                        else
                        {

                            Class = selector.Selector;
                            Class = Class.ToLower().Replace("<style>", "");

                            if (Class.IndexOf(".") != -1)
                            {
                                Class = Class.Replace(".", "");
                                Class = Class.Trim();
                                styles.LoadStyle(Class, h);
                            }
                            else
                            {
                                Class = Class.Trim();
                                styles.LoadTagStyle(Class, h);
                            }
                            h = new Dictionary<string, string>();

                        }

                    }
                    //----------------------------------------------

                }
            }
            return styles;

        }

        //Busca las hojas de estilo y junta todas las clases--
        internal static String getCss(string sHTML, string baseurl, string user, string psw)
        {

            string urlCss = "";
            string codigo = "";
            try
            {
                DOL.DHtml.DHtmlParser.DHtmlDocument mDocument = new DOL.DHtml.DHtmlParser.DHtmlDocument(sHTML);

                //Obtengo el tag link y sus atributos si existe

                if (mDocument.Nodes.Count > 0)
                {

                    DOL.DHtml.DHtmlParser.Node.DHtmlElement node;

                    node = (DOL.DHtml.DHtmlParser.Node.DHtmlElement)mDocument.Nodes[mDocument.Nodes.Count - 1];

                    DOL.DHtml.DHtmlParser.DHtmlNodeCollection nodes = node.Nodes;

                    foreach (object objnodo in nodes)
                    {

                        if (objnodo.GetType().ToString() == "DOL.DHtml.DHtmlParser.Node.DHtmlElement")
                        {
                            DOL.DHtml.DHtmlParser.Node.DHtmlElement Elemento = (DOL.DHtml.DHtmlParser.Node.DHtmlElement)objnodo;

                            if (Elemento.NodeName == "head")
                            {
                                if (Elemento.Nodes.Count > 0)
                                {
                                    Int16 css = -1;
                                    for (Int16 i = 0; i < Elemento.Nodes.Count; i++)
                                    {
                                        if (Elemento.Nodes[i].NodeName == "link")
                                        {
                                            DOL.DHtml.DHtmlParser.Node.DHtmlElement nodelink;
                                            nodelink = (DOL.DHtml.DHtmlParser.Node.DHtmlElement)Elemento.Nodes[i];

                                            if (nodelink.Attributes.IndexOf("href") != -1)
                                            {
                                                if (nodelink.Attributes.IndexOf("rel") != -1)
                                                {
                                                    if (nodelink.Attributes["rel"].Value == "stylesheet")
                                                    {
                                                        urlCss = "";
                                                        urlCss = nodelink.Attributes["href"].Value;

                                                        if (urlCss != "")
                                                        {
                                                            if (!urlCss.StartsWith("http"))// referencia relativa solamente
                                                            {
                                                                if (baseurl != null && baseurl != "")
                                                                {
                                                                    urlCss = baseurl + urlCss;
                                                                }
                                                            }

                                                            if (user != null)
                                                                codigo = codigo + System.Text.Encoding.Default.GetString(PDFLive.Functions.SourceDataBin(urlCss, user, psw));
                                                            else
                                                                codigo = codigo + System.Text.Encoding.Default.GetString(PDFLive.Functions.SourceDataBin(urlCss));

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

                return "<style>" + codigo + "</style>";


            }

            catch { return ""; }

        }

        internal static IElement ExtendElement(IElement elem)
        {
            if (elem.GetType().ToString() == "iTextSharp.text.pdf.PdfPTable")
            {
                PdfPTable t = (PdfPTable)elem;
                List<PdfPRow> rl = t.Rows;


                for (int i = 0; i < rl.Count; i++)
                {
                    PdfPRow r = (PdfPRow)rl[i];
                    int isheader = 0;

                    float[] widths = new float[r.GetCells().Length];
                    for (int j = 0; j < r.GetCells().Length; j++)
                    {

                        if (r.GetCells()[j] != null)
                        {

                            //busca si dentro de las celdas hay otra tabla (recursivo)
                            try
                            {
                                if (r.GetCells()[j].CompositeElements.Count > 0)
                                {
                                    for (int h = 0; h < r.GetCells()[j].CompositeElements.Count; h++)
                                    {
                                        r.GetCells()[j].CompositeElements[h] = ExtendElement((IElement)r.GetCells()[j].CompositeElements[h]);
                                    }

                                }
                            }
                            catch (Exception e) { }

                            if (r.GetCells()[j].isTH)
                                isheader = 1;

                            if (r.GetCells()[j].CssWidth != 0 && t.widths_seteados != 2)
                            {
                                widths[j] = r.GetCells()[j].CssWidth;
                                t.widths_seteados = 1;
                            }

                           
                            if (r.GetCells()[j].Colspan > 1)
                                j = j + r.GetCells()[j].Colspan - 1;

                        }

                    }
                    if (isheader == 1)
                    {
                        t.HeaderRows = t.HeaderRows + 1;
                    }
                    if (t.widths_seteados == 1)
                    {
                        t.SetWidths(widths);
                        t.widths_seteados = 2;
                    }
                }
            }

            return elem;

        }

    }
}
