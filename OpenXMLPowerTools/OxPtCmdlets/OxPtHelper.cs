using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Validation;
using OpenXmlPowerTools;

public class HtmlConverterHelper
{
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
                        DirectoryInfo localDirInfo = new DirectoryInfo(imageDirectoryName);
                        if (!localDirInfo.Exists)
                            localDirInfo.Create();
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
                            return null;

                        string imageFileName = imageDirectoryName + "/image" +
                            imageCounter.ToString() + "." + extension;
                        try
                        {
                            imageInfo.Bitmap.Save(imageFileName, imageFormat);
                        }
                        catch (System.Runtime.InteropServices.ExternalException)
                        {
                            return null;
                        }
                        XElement img = new XElement(Xhtml.img,
                            new XAttribute(NoNamespace.src, imageFileName),
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
}

public class OpenXmlValidationHelper
{
    public static bool IsValid(string fileName, string officeVersion)
    {
        FileFormatVersions fileFormatVersion;
        if (!Enum.TryParse(officeVersion, out fileFormatVersion))
            fileFormatVersion = FileFormatVersions.Office2013;

        FileInfo fi = new FileInfo(fileName);
        if (FlatOpc.IsWordprocessingML(fi.Extension))
        {
            using (WordprocessingDocument wDoc = WordprocessingDocument.Open(fileName, false))
            {
                OpenXmlValidator validator = new OpenXmlValidator(fileFormatVersion);
                var errors = validator.Validate(wDoc);
                bool valid = errors.Count() == 0;
                return valid;
            }
        }
        else if (FlatOpc.IsSpreadsheetML(fi.Extension))
        {
            using (SpreadsheetDocument sDoc = SpreadsheetDocument.Open(fileName, false))
            {
                OpenXmlValidator validator = new OpenXmlValidator(fileFormatVersion);
                var errors = validator.Validate(sDoc);
                bool valid = errors.Count() == 0;
                return valid;
            }
        }
        else if (FlatOpc.IsPresentationML(fi.Extension))
        {
            using (PresentationDocument pDoc = PresentationDocument.Open(fileName, false))
            {
                OpenXmlValidator validator = new OpenXmlValidator(fileFormatVersion);
                var errors = validator.Validate(pDoc);
                bool valid = errors.Count() == 0;
                return valid;
            }
        }
        return false;
    }

    public static IEnumerable<ValidationErrorInfo> GetOpenXmlValidationErrors(string fileName,
        string officeVersion)
    {
        FileFormatVersions fileFormatVersion;
        if (!Enum.TryParse(officeVersion, out fileFormatVersion))
            fileFormatVersion = FileFormatVersions.Office2013;

        FileInfo fi = new FileInfo(fileName);
        if (FlatOpc.IsWordprocessingML(fi.Extension))
        {
            WmlDocument wml = new WmlDocument(fileName);
            using (OpenXmlMemoryStreamDocument streamDoc = new OpenXmlMemoryStreamDocument(wml))
            using (WordprocessingDocument wDoc = streamDoc.GetWordprocessingDocument())
            {
                OpenXmlValidator validator = new OpenXmlValidator(fileFormatVersion);
                var errors = validator.Validate(wDoc);
                return errors.ToList();
            }
        }
        else if (FlatOpc.IsSpreadsheetML(fi.Extension))
        {
            SmlDocument Sml = new SmlDocument(fileName);
            using (OpenXmlMemoryStreamDocument streamDoc = new OpenXmlMemoryStreamDocument(Sml))
            using (SpreadsheetDocument wDoc = streamDoc.GetSpreadsheetDocument())
            {
                OpenXmlValidator validator = new OpenXmlValidator(fileFormatVersion);
                var errors = validator.Validate(wDoc);
                return errors.ToList();
            }
        }
        else if (FlatOpc.IsPresentationML(fi.Extension))
        {
            PmlDocument Pml = new PmlDocument(fileName);
            using (OpenXmlMemoryStreamDocument streamDoc = new OpenXmlMemoryStreamDocument(Pml))
            using (PresentationDocument wDoc = streamDoc.GetPresentationDocument())
            {
                OpenXmlValidator validator = new OpenXmlValidator(fileFormatVersion);
                var errors = validator.Validate(wDoc);
                return errors.ToList();
            }
        }
        return Enumerable.Empty<ValidationErrorInfo>();
    }
}

