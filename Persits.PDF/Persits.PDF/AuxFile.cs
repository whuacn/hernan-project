using System;
using System.IO;
using System.Text;
namespace Persits.PDF
{
	internal class AuxFile
	{
		public string m_bstrOriginalPath;
		public string m_bstrFileName;
		public string m_bstrPath;
		private FileStream m_objFileStm;
		public int Size
		{
			get
			{
				if (this.m_objFileStm == null)
				{
					AuxException.Throw("File not opened.", PdfErrors._ERROR_READFILE);
				}
				return (int)this.m_objFileStm.Length;
			}
		}
		public AuxFile(string strPath)
		{
			this.m_bstrOriginalPath = strPath;
			this.m_bstrPath = strPath;
			this.m_bstrFileName = Path.GetFileName(strPath);
		}
		public AuxFile()
		{
		}
		~AuxFile()
		{
			this.Close();
		}
		public string ExtractFileName()
		{
			return Path.GetFileName(this.m_bstrOriginalPath);
		}
		public void CreateUniqueFileName()
		{
			string text = this.m_bstrOriginalPath;
			int num = 1;
			while (File.Exists(text))
			{
				string directoryName = Path.GetDirectoryName(this.m_bstrOriginalPath);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.m_bstrOriginalPath);
				string extension = Path.GetExtension(this.m_bstrOriginalPath);
				text = string.Concat(new string[]
				{
					directoryName,
					"\\",
					fileNameWithoutExtension,
					string.Format("({0})", num),
					extension
				});
				num++;
			}
			this.m_bstrFileName = Path.GetFileName(text);
			this.m_bstrPath = text;
		}
		public void Open(string Path)
		{
			try
			{
				this.m_objFileStm = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);
				this.m_bstrOriginalPath = Path;
			}
			catch (Exception ex)
			{
				AuxException.Throw(ex.Message, PdfErrors._ERROR_OPEN_EXISTING);
			}
		}
		public void OpenForWriting(string Path)
		{
			try
			{
				this.m_objFileStm = new FileStream(Path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
				this.m_bstrOriginalPath = Path;
			}
			catch (Exception ex)
			{
				AuxException.Throw(ex.Message, PdfErrors._ERROR_OPEN_EXISTING);
			}
		}
		public int SetPointer(int Distance)
		{
			this.m_objFileStm.Position = (long)Distance;
			return Distance;
		}
		public void Write(byte[] data)
		{
			this.m_objFileStm.Write(data, 0, data.Length);
		}
		public int Read(byte[] Buffer)
		{
			return this.Read(Buffer, Buffer.Length);
		}
		public int Read(byte[] Buffer, int nOffset, int nLen)
		{
			int result = 0;
			try
			{
				result = this.m_objFileStm.Read(Buffer, nOffset, nLen);
			}
			catch (Exception ex)
			{
				AuxException.Throw(ex.Message, PdfErrors._ERROR_READFILE);
			}
			return result;
		}
		public int Read(byte[] Buffer, int nLen)
		{
			return this.Read(Buffer, 0, nLen);
		}
		public void Close()
		{
			if (this.m_objFileStm != null)
			{
				this.m_objFileStm.Dispose();
			}
		}
		public void GetFileDates(out DateTime CreationDate, out DateTime ModifiedDate)
		{
			CreationDate = File.GetCreationTime(this.m_bstrOriginalPath);
			ModifiedDate = File.GetLastWriteTime(this.m_bstrOriginalPath);
		}
		public static string ConvertToAnsi(string Filename)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < Filename.Length; i++)
			{
				char c = Filename[i];
				if (c < ' ' || c > '\u007f')
				{
					string value;
					if (c < 'Ā')
					{
						value = string.Format("%{0:x02}", c);
					}
					else
					{
						value = string.Format("%{0:x02}%{1:x02}", (int)(c >> 8), (int)(c & 'ÿ'));
					}
					stringBuilder.Append(value);
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
