using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DocumentManager
{
    public class Utils
    {

        public static string HtmlEmbedImages(string htmlSource)
        {
            string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
            MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match m in matchesImgSrc)
            {
                string src = m.Groups[1].Value;
                string img = MakeImageSrcData(Path.Combine(Path.GetTempPath(), src));
                htmlSource = htmlSource.Replace(src, img);
            }
            return htmlSource;

        }
        public static string MakeImageSrcData(string filename)
        {
            string result = filename;
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    byte[] filebytes = new byte[fs.Length];
                    fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    result = "data:image/png;base64," +
                      Convert.ToBase64String(filebytes, Base64FormattingOptions.None);
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
    }
}
