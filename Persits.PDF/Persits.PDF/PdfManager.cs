using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Web;
namespace Persits.PDF
{
	public class PdfManager
	{
		internal const int ENCRYPT_AES = 1;
		internal DateTime m_dtExpires;
		internal bool m_bExpirationChecked;
		internal bool m_bFontTableBuilt;
		internal Dictionary<string, string> m_mapFontPaths = new Dictionary<string, string>();
		public string RegKey
		{
			set
			{
				this.m_bExpirationChecked = true;
				this.CheckExpiration(value);
			}
		}
		public DateTime Expires
		{
			get
			{
				this.CheckExpiration();
				return this.m_dtExpires;
			}
		}
		public string Version
		{
			get
			{
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				AssemblyName name = executingAssembly.GetName();
				Version arg_13_0 = name.Version;
				return name.Version.ToString();
			}
		}
		public string ConfigPath
		{
			get
			{
				return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath;
			}
		}
		public PdfDocument CreateDocument(string ID)
		{
			this.CheckExpiration();
			PdfDocument pdfDocument = new PdfDocument();
			pdfDocument.m_pManager = this;
			pdfDocument.Init(ID);
			return pdfDocument;
		}
		public PdfDocument CreateDocument()
		{
			return this.CreateDocument(null);
		}
		public PdfDocument OpenDocument(string Path)
		{
			return this.OpenDocument(Path, null);
		}
		public PdfDocument OpenDocument(string Path, string Password)
		{
			return this.OpenDocumentHelper(Path, null, Password);
		}
		public PdfDocument OpenDocument(byte[] Blob)
		{
			return this.OpenDocument(Blob, null);
		}
		public PdfDocument OpenDocument(byte[] Blob, string Password)
		{
			return this.OpenDocumentHelper(null, Blob, Password);
		}
		internal PdfDocument OpenDocumentHelper(string Path, byte[] Blob, string Password)
		{
			this.CheckExpiration();
			PdfDocument pdfDocument = new PdfDocument();
			pdfDocument.m_pManager = this;
			if (pdfDocument.Open(Path, Blob, Password))
			{
				pdfDocument.m_bExisting = true;
				return pdfDocument;
			}
			return null;
		}
		public PdfParam CreateParam(string ParamString)
		{
			return new PdfParam(ParamString);
		}
		public PdfParam CreateParam()
		{
			return this.CreateParam("");
		}
		internal PdfParam VariantToParam(object obj)
		{
			if (obj == null)
			{
				return new PdfParam();
			}
			if (obj.GetType().FullName == "System.String")
			{
				return new PdfParam((string)obj);
			}
			if (obj.GetType().FullName == "Persits.PDF.PdfParam")
			{
				return (PdfParam)obj;
			}
			AuxException.Throw("Invalid argument. Must be either a valid parameter string or PdfParam object.", PdfErrors._ERROR_VARTOPARAM);
			return null;
		}
		public string LoadTextFromFile(string Path)
		{
			PdfString pdfString = new PdfString();
			pdfString.LoadFromFile(Path);
			if (pdfString.Length >= 3 && pdfString[0] == 239 && pdfString[1] == 187 && pdfString[2] == 191)
			{
				return new string(Encoding.UTF8.GetChars(pdfString.ToBytes(), 3, pdfString.Length - 3));
			}
			pdfString.TestForAnsi();
			return pdfString.ToString();
		}
		internal void CheckExpiration()
		{
			this.CheckExpiration(null);
		}
		internal void CheckExpiration(string Key)
		{

            this.m_bExpirationChecked = true;
            this.m_dtExpires = DateTime.MaxValue;
			if (this.m_bExpirationChecked)
			{
				return;
			}
			byte[] bytes;
			if (Key == null)
			{
				string text = ConfigurationManager.AppSettings["AspPDF_RegKey"];
				if (text == null)
				{
					AuxException.Throw("The registration key could not be found under AppSettings in the .config file. The entry's key name should be \"AspPDF_RegKey\".", PdfErrors._ERROR_OPENREGKEY);
				}
				text = text.Trim();
				if (text.Length < 70 || text.Length > 80)
				{
					AuxException.Throw("Invalid registration key length.", PdfErrors._ERROR_OPENREGKEY);
				}
				bytes = Encoding.UTF8.GetBytes(text);
			}
			else
			{
				Key = Key.Trim();
				if (Key.Length != 76)
				{
					AuxException.Throw("Invalid registration key length, should be 76.", PdfErrors._ERROR_OPENREGKEY);
				}
				bytes = Encoding.UTF8.GetBytes(Key);
			}
			PdfStream pdfStream = new PdfStream();
			pdfStream.Append(bytes);
			if (pdfStream.DecodeBase64() != 0L)
			{
				AuxException.Throw("Base64 decoding of the registration key failed.", PdfErrors._ERROR_OPENREGKEY);
			}
			if (pdfStream.Length != 57)
			{
				AuxException.Throw("Decoded registration key length is invalid, should be 57.", PdfErrors._ERROR_OPENREGKEY);
			}
			PdfStream pdfStream2 = new PdfStream();
			pdfStream2.Append(new byte[]
			{
				81,
				138,
				78,
				79,
				7,
				13,
				63,
				30,
				189,
				41,
				174,
				135,
				190,
				99,
				235,
				41
			});
			PdfHash pdfHash = new PdfHash();
			byte[] array = new byte[41];
			Array.Copy(pdfStream.ToBytes(), 0, array, 0, 41);
			pdfHash.Compute(array);
			pdfHash.Encrypt(pdfStream2.ToBytes());
			PdfStream pdfStream3 = new PdfStream();
			pdfStream3.Append(pdfStream.ToBytes(), 41, 16);
			if (!pdfHash.Equals(ref pdfStream3))
			{
				AuxException.Throw("Invalid registration key.", PdfErrors._ERROR_OPENREGKEY);
			}
			pdfStream.Encrypt(pdfStream2.ToBytes());
			if (pdfStream[0] != 80 || pdfStream[1] != 68 || pdfStream[2] != 78)
			{
				AuxException.Throw("Registration key is invalid or corrupt.", PdfErrors._ERROR_OPENREGKEY);
			}
			double d = BitConverter.ToDouble(pdfStream.ToBytes(), 11);
			this.m_dtExpires = DateTime.FromOADate(d);
			if (DateTime.Now > this.m_dtExpires)
			{
				AuxException.Throw("AspPDF.NET has expired. Please visit www.asppdf.net to purchase a key that will not expire.", PdfErrors._ERROR_EXPIRED);
			}
			uint num = BitConverter.ToUInt32(pdfStream.ToBytes(), 19);
			uint[] array2 = new uint[0];
			for (int i = 0; i < array2.Length; i++)
			{
				if (num == array2[i])
				{
					AuxException.Throw("Registration key has been revoked.", PdfErrors._ERROR_REVOKED);
				}
			}
			this.m_bExpirationChecked = true;
		}
		public void SendBinary(string Path)
		{
			this.SendBinary(Path, null, null);
		}
		public void SendBinary(string Path, string ContentType, string DispHeader)
		{
			if (HttpContext.Current == null)
			{
				AuxException.Throw("SaveHttp can only be called in an ASP.NET environment.", PdfErrors._ERROR_HTTP_CONTEXT);
			}
			HttpResponse response = HttpContext.Current.Response;
			AuxFile auxFile = new AuxFile();
			auxFile.Open(Path);
			string contentType = (ContentType == null) ? "application/pdf" : ContentType;
			string text = DispHeader;
			if (text == null)
			{
				text = "attachment;filename=\"" + auxFile.m_bstrFileName + "\"";
			}
			response.Clear();
			response.Buffer = false;
			response.ContentType = contentType;
			response.AddHeader("Content-Disposition", text);
			response.AddHeader("Content-Length", auxFile.Size.ToString());
			byte[] array = new byte[10000];
			while (true)
			{
				int num = auxFile.Read(array);
				if (num == 0)
				{
					break;
				}
				if (num == 10000)
				{
					response.BinaryWrite(array);
				}
				else
				{
					byte[] array2 = new byte[num];
					Array.Copy(array, array2, num);
					response.BinaryWrite(array2);
				}
			}
			response.Flush();
			response.Close();
			auxFile.Close();
		}
		public void LogonUser(string Domain, string Username, string Password)
		{
			UserManager userManager = new UserManager();
			userManager.LogonUser(Domain, Username, Password);
		}
		public void RevertToSelf()
		{
			UserManager userManager = new UserManager();
			userManager.Revert();
		}
	}
}
