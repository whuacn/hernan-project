using System;
using System.IO;
using System.Net;
namespace Persits.PDF
{
	internal class HtmlXmlUrl
	{
		public string m_bstrPath;
		public string m_bstrHost;
		public string m_bstrFilename;
		public string m_bstrUrl;
		public string m_bstrUsername;
		public string m_bstrPassword;
		public string m_bstrAuth;
		public bool m_bFileUrl;
		public bool m_bDirectHTML;
		public string m_bstrCookie;
		private HtmlManager m_pHtmlManager;
		public HtmlXmlUrl(HtmlManager pHtmlManager)
		{
			this.m_pHtmlManager = pHtmlManager;
			this.m_bDirectHTML = false;
			this.m_bFileUrl = false;
		}
		public HtmlXmlUrl(string Url, string Username, string Password, HtmlManager pHtmlManager)
		{
			this.m_pHtmlManager = pHtmlManager;
			this.m_bDirectHTML = false;
			this.m_bFileUrl = false;
			if (this.m_pHtmlManager.m_bDirectHTML)
			{
				this.m_bDirectHTML = true;
				return;
			}
			this.m_bstrUrl = Url;
			this.m_bstrUsername = Username;
			this.m_bstrPassword = Password;
			this.SplitUrl();
			if (this.m_bstrUsername != null && this.m_bstrUsername.Length > 7 && string.Compare(this.m_bstrUsername, 0, "Cookie:", 0, 7, true) == 0)
			{
				this.m_bstrCookie = this.m_bstrUsername.Substring(7) + "=" + Password;
				this.m_bstrUsername = null;
				this.m_bstrPassword = null;
			}
		}
		public bool SplitUrl()
		{
			if (this.m_bstrUrl == null || (!this.m_bDirectHTML && this.m_bstrUrl.Length <= 3))
			{
				AuxException.Throw("Invalid URL.", PdfErrors._ERROR_HTML_URL);
			}
			if ((this.m_bstrUrl[1] == ':' && (this.m_bstrUrl[2] == '\\' || this.m_bstrUrl[2] == '/')) || (this.m_bstrUrl[0] == '\\' && this.m_bstrUrl[1] == '\\'))
			{
				this.m_bFileUrl = true;
				this.m_bstrPath = this.m_bstrUrl;
				string directoryName = Path.GetDirectoryName(this.m_bstrPath);
				this.m_bstrHost = directoryName;
				if (this.m_bstrHost[this.m_bstrHost.Length - 1] != '\\')
				{
					this.m_bstrHost += '\\';
				}
				return true;
			}
			if (this.m_bDirectHTML)
			{
				return true;
			}
			this.m_bstrHost = null;
			this.m_bstrPath = null;
			int num = 0;
			if (string.Compare(this.m_bstrUrl, 0, "http://", 0, 7, true) == 0)
			{
				num += 7;
			}
			if (string.Compare(this.m_bstrUrl, 0, "https://", 0, 8, true) == 0)
			{
				num += 8;
			}
			int num2 = this.m_bstrUrl.IndexOf('/', num);
			if (num2 == -1)
			{
				this.m_bstrHost = this.m_bstrUrl;
				this.m_bstrHost += "/";
				this.m_bstrPath = this.m_bstrHost;
				return true;
			}
			if (num2 == this.m_bstrUrl.Length - 1)
			{
				this.m_bstrHost = this.m_bstrUrl;
				this.m_bstrPath = this.m_bstrHost;
				return true;
			}
			this.m_bstrHost = this.m_bstrUrl.Substring(0, num2 + 1);
			num = num2 + 1;
			this.m_bstrFilename = this.m_bstrUrl.Substring(num);
			bool flag = false;
			while ((num2 = this.m_bstrUrl.IndexOf('/', num)) != -1)
			{
				flag = true;
				num = num2 + 1;
			}
			if (!flag)
			{
				this.m_bstrHost = this.m_bstrUrl.Substring(0, num);
				this.m_bstrPath = this.m_bstrHost;
				return true;
			}
			this.m_bstrPath = this.m_bstrUrl.Substring(0, num);
			return true;
		}
		public bool MakeUrl(HtmlXmlUrl pBaseUrl, string strPath)
		{
			int length = strPath.Length;
			if (length == 0)
			{
				return false;
			}
			this.m_bFileUrl = pBaseUrl.m_bFileUrl;
			this.m_bstrHost = pBaseUrl.m_bstrHost;
			this.m_bstrUsername = pBaseUrl.m_bstrUsername;
			this.m_bstrPassword = pBaseUrl.m_bstrPassword;
			if (length >= 8 && (string.Compare(strPath, 0, "http://", 0, 7, true) == 0 || string.Compare(strPath, 0, "https://", 0, 8, true) == 0 || string.Compare(strPath, 0, "mailto:", 0, 7, true) == 0))
			{
				this.m_bstrPath = strPath;
				this.m_bstrUrl = strPath;
				this.m_bFileUrl = false;
				return true;
			}
			if (length > 3 && strPath[1] == ':' && strPath[2] == '\\')
			{
				this.m_bstrPath = strPath;
				this.m_bstrUrl = strPath;
				this.m_bFileUrl = true;
				return true;
			}
			if (length > 5 && string.Compare(strPath, 0, "data:", 0, 5) == 0)
			{
				this.m_bstrPath = strPath;
				this.m_bstrUrl = strPath;
				this.m_bFileUrl = false;
				return true;
			}
			if (length > 2 && strPath[0] == '\\' && strPath[1] == '\\')
			{
				this.m_bstrPath = strPath;
				this.m_bstrUrl = strPath;
				this.m_bFileUrl = true;
				return true;
			}
			if (length > 1 && strPath[0] == '#')
			{
				this.m_bstrPath = pBaseUrl.m_bstrHost + pBaseUrl.m_bstrFilename + strPath;
				this.m_bstrUrl = this.m_bstrPath;
				this.m_bFileUrl = false;
				return true;
			}
			if (this.m_bFileUrl)
			{
				int num = strPath.IndexOf("?");
				if (num > -1)
				{
					strPath = strPath.Substring(0, num);
				}
				this.m_bstrPath = pBaseUrl.m_bstrHost + strPath;
				try
				{
					this.m_bstrUrl = Path.GetFullPath(this.m_bstrPath);
				}
				catch (Exception)
				{
					this.m_bstrUrl = this.m_bstrPath;
				}
				return true;
			}
			if (strPath[0] == '/')
			{
				this.m_bstrUrl = pBaseUrl.m_bstrHost + strPath.Substring(1);
			}
			else
			{
				if (strPath[0] == '.' && strPath[1] == '.' && strPath[2] == '/' && pBaseUrl.m_bstrHost != null && pBaseUrl.m_bstrPath != null && string.Compare(pBaseUrl.m_bstrHost, pBaseUrl.m_bstrPath) == 0)
				{
					this.m_bstrUrl = pBaseUrl.m_bstrPath + strPath.Substring(3);
				}
				else
				{
					this.m_bstrUrl = pBaseUrl.m_bstrPath + strPath;
				}
			}
			if (this.m_pHtmlManager.m_bDirectHTML && this.m_bstrUrl.Length > 2 && this.m_bstrUrl[1] == ':' && this.m_bstrUrl[2] == '\\')
			{
				this.m_bFileUrl = true;
			}
			return true;
		}
		public bool MakeUrl(string BasePath, string Path)
		{
			HtmlXmlUrl htmlXmlUrl = new HtmlXmlUrl(this.m_pHtmlManager);
			htmlXmlUrl.m_bstrUrl = BasePath;
			htmlXmlUrl.SplitUrl();
			return this.MakeUrl(htmlXmlUrl, Path);
		}
		private void ReadFileUrl(ref byte[] pStream)
		{
			AuxFile auxFile = new AuxFile();
			auxFile.Open(this.m_bstrUrl);
			pStream = new byte[auxFile.Size + 1];
			auxFile.Read(pStream, auxFile.Size);
			pStream[auxFile.Size] = 32;
			auxFile.Close();
		}
		private bool EmbeddedImage(ref byte[] pStream)
		{
			if (this.m_bstrUrl == null || this.m_bstrUrl.Length == 0)
			{
				return false;
			}
			if (this.m_bstrUrl.IndexOf("data:") == -1)
			{
				return false;
			}
			int num;
			if ((num = this.m_bstrUrl.IndexOf("base64,")) == -1)
			{
				return false;
			}
			num += 7;
			pStream = Convert.FromBase64String(this.m_bstrUrl.Substring(num));
			return true;
		}
		public void ReadUrl(ref byte[] pStream)
		{
			if (this.EmbeddedImage(ref pStream))
			{
				return;
			}
			if (this.m_bFileUrl)
			{
				this.ReadFileUrl(ref pStream);
				return;
			}
			if (this.m_bDirectHTML)
			{
				return;
			}
			this.SplitUrl();
			PdfStream pdfStream = new PdfStream();
			byte[] array = new byte[10000];
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(this.m_bstrUrl);
				if (this.m_bstrUsername != null)
				{
					httpWebRequest.Credentials = new NetworkCredential(this.m_bstrUsername, this.m_bstrPassword);
				}
				else
				{
					httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
				}
				if (this.m_pHtmlManager.m_nTimeout > 0)
				{
					httpWebRequest.Timeout = this.m_pHtmlManager.m_nTimeout;
				}
				if (this.m_bstrCookie != null && this.m_bstrCookie.Length > 0)
				{
					httpWebRequest.Headers.Add(HttpRequestHeader.Cookie, this.m_bstrCookie);
				}
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				HttpStatusCode statusCode = httpWebResponse.StatusCode;
				if (statusCode != HttpStatusCode.OK)
				{
					AuxException.Throw(string.Format("Error opening URL '(1}'. HTTP Status Code: {0}.", (int)statusCode, this.m_bstrUrl), PdfErrors._ERROR_HTML_XMLHTTP2);
				}
				Stream responseStream = httpWebResponse.GetResponseStream();
				int num;
				do
				{
					num = responseStream.Read(array, 0, array.Length);
					if (num != 0)
					{
						pdfStream.Append(array, 0, num);
					}
				}
				while (num > 0);
			}
			catch (Exception ex)
			{
				AuxException.Throw("Error connecting to URL '" + this.m_bstrUrl + "': " + ex.Message, PdfErrors._ERROR_HTML_XMLHTTP3);
			}
			pdfStream.Append(new byte[]
			{
				32
			});
			pStream = pdfStream.ToBytes();
		}
	}
}
