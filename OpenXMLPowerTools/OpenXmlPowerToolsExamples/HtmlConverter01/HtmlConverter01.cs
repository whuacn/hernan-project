/***************************************************************************

Copyright (c) Microsoft Corporation 2010.

This code is licensed using the Microsoft Public License (Ms-PL).  The text of the license
can be found here:

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx

***************************************************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;

class HtmlConverterHelper
{
    static void Main(string[] args)
    {
        /*
         * This example loads each document into a byte array, then into a memory stream, so that the document can be opened for writing without
         * modifying the source document.
         */
        foreach (var file in Directory.GetFiles("../../", "Especificacion_Documentos Digitales v1.4.docx"))
        {
            ConvertToHtml(file, "../../");
        }
        Console.WriteLine("Press Enter");
        Console.ReadKey();
    }

    public static void ConvertToHtml(string file, string outputDirectory)
    {
        var fi = new FileInfo(file);
        Console.WriteLine(fi.Name);
        byte[] byteArray = File.ReadAllBytes(fi.FullName);
        using (MemoryStream memoryStream = new MemoryStream())
        {
            memoryStream.Write(byteArray, 0, byteArray.Length);
            using (WordprocessingDocument wDoc = WordprocessingDocument.Open(memoryStream, true))
            {
                var destFileName = new FileInfo(fi.Name.Replace(".docx", ".html"));
                if (outputDirectory != null && outputDirectory != string.Empty)
                {
                    DirectoryInfo di = new DirectoryInfo(outputDirectory);
                    if (!di.Exists)
                    {
                        throw new OpenXmlPowerToolsException("Output directory does not exist");
                    }
                    destFileName = new FileInfo(Path.Combine(di.FullName, destFileName.Name));
                }
                var imageDirectoryName = destFileName.FullName.Substring(0, destFileName.FullName.Length - 5) + "_files";
                int imageCounter = 0;
                var pageTitle = (string)wDoc.CoreFilePropertiesPart.GetXDocument().Descendants(DC.title).FirstOrDefault();
                if (pageTitle == null)
                    pageTitle = fi.FullName;

                HtmlConverterSettings settings = new HtmlConverterSettings()
                {
                    PageTitle = pageTitle,
                    FabricateCssClasses = true,
                    CssClassPrefix = "pt-",
                    RestrictToSupportedLanguages = false,
                    RestrictToSupportedNumberingFormats = false,
                    ListItemImplementations = new Dictionary<string, Func<string, int, string, string>>()
                        {
                            {"fr-FR", ListItemTextGetter_fr_FR.GetListItemText},
                            {"tr-TR", ListItemTextGetter_tr_TR.GetListItemText},
                            {"ru-RU", ListItemTextGetter_ru_RU.GetListItemText},
                            {"sv-SE", ListItemTextGetter_sv_SE.GetListItemText},
                        },
                    ImageHandler = imageInfo =>
                    {
                        ++imageCounter;
                        string extension = imageInfo.ContentType.Split('/')[1].ToLower();
                        ImageFormat imageFormat = null;
                        if (extension == "png")
                        {
                            // Convert png to jpeg.
                            extension = "gif";
                            imageFormat = ImageFormat.Gif;
                        }
                        else if (extension == "gif")
                            imageFormat = ImageFormat.Gif;
                        else if (extension == "bmp")
                            imageFormat = ImageFormat.Bmp;
                        else if (extension == "jpeg")
                            imageFormat = ImageFormat.Jpeg;
                        else if (extension == "x-emf")
                            imageFormat = ImageFormat.Emf;
                        else if (extension == "tiff")
                        {
                            // Convert tiff to gif.
                            extension = "gif";
                            imageFormat = ImageFormat.Gif;
                        }
                        else if (extension == "x-wmf")
                        {
                            extension = "wmf";
                            imageFormat = ImageFormat.Wmf;
                        }

                        // If the image format isn't one that we expect, ignore it,
                        // and don't return markup for the link.
                        if (imageFormat == null)
                        {
                            extension = "jpeg";
                            imageFormat = ImageFormat.Jpeg;
                            //return null;
                        }
                        string ImgB64 = null;
                        if (extension != "x-emf")
                        {
                            ImgB64 = ImageToBase64(imageInfo.Bitmap, imageFormat);
                        }
                        else
                        {
                            Metafile mf = new Metafile(imageInfo.ImagePart.GetStream());
   
                            
                            ImgB64 = MetaFileToBase64(mf, ImageFormat.Png);
                        }

                        XElement img = new XElement(Xhtml.img,
                            new XAttribute(NoNamespace.src, "data:image/" + imageFormat.ToString() + ";base64," + ImgB64),
                            imageInfo.ImgStyleAttribute,
                            imageInfo.AltText != null ?
                                new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
                        return img;
                    }
                };
                XElement html = HtmlConverter.ConvertToHtml(wDoc, settings);

                // Note: the xhtml returned by ConvertToHtmlTransform contains objects of type
                // XEntity.  PtOpenXmlUtil.cs define the XEntity class.  See
                // http://blogs.msdn.com/ericwhite/archive/2010/01/21/writing-entity-references-using-linq-to-xml.aspx
                // for detailed explanation.
                //
                // If you further transform the XML tree returned by ConvertToHtmlTransform, you
                // must do it correctly, or entities will not be serialized properly.

                var htmlString = html.ToString(SaveOptions.DisableFormatting);
                File.WriteAllText(destFileName.FullName, htmlString, Encoding.UTF8);
            }
        }
    }
    public static string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
    {        
        using (MemoryStream ms = new MemoryStream())
        {
            // Convert Image to byte[]
            image.Save(ms, format);
            byte[] imageBytes = ms.ToArray();

            // Convert byte[] to Base64 String
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }
    }

    public static string MetaFileToBase64(Metafile metafile, System.Drawing.Imaging.ImageFormat format)
    {
        try
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                metafile.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public static string StreamToBase64(Stream stream)
    {
        try
        {
            using (BinaryReader binaryReader = new BinaryReader(stream))
            {
                byte[] binaryByteArray = binaryReader.ReadBytes((int)stream.Length);
                return Convert.ToBase64String(binaryByteArray);
            }
        }
        catch (Exception ex)
        {
            throw ex; 
        }
    }
}
