using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace Microsoft.Tools.TestClient
{
	internal static class ResourceHelper
	{
		private static Bitmap theAboutBoxImage;

		private static Icon theAppIcon;

		private static Bitmap theArrowDownImage;

		private static Bitmap theArrowUpImage;

		private static Bitmap theContractImage;

		private static Bitmap theEndpointImage;

		private static Bitmap theErrorImage;

		private static Bitmap theFileImage;

		private static Bitmap theOperationImage;

		internal static Bitmap AboutBoxImage
		{
			get
			{
				if (ResourceHelper.theAboutBoxImage == null)
				{
					ResourceHelper.theAboutBoxImage = new Bitmap(ResourceHelper.LoadResourceStream("WcfTestClient.Resources.AboutBox.bmp"));
				}
				return ResourceHelper.theAboutBoxImage;
			}
		}

		internal static Icon ApplicationIcon
		{
			get
			{
				if (ResourceHelper.theAppIcon == null)
				{
					ResourceHelper.theAppIcon = new Icon(ResourceHelper.LoadResourceStream("WcfTestClient.Resources.Service.ico"));
				}
				return ResourceHelper.theAppIcon;
			}
		}

		internal static Bitmap ArrowDownImage
		{
			get
			{
				if (ResourceHelper.theArrowDownImage == null)
				{
					ResourceHelper.theArrowDownImage = new Bitmap(ResourceHelper.LoadResourceStream("WcfTestClient.Resources.ArrowDown.bmp"));
					ResourceHelper.theArrowDownImage.MakeTransparent();
				}
				return ResourceHelper.theArrowDownImage;
			}
		}

		internal static Bitmap ArrowUpImage
		{
			get
			{
				if (ResourceHelper.theArrowUpImage == null)
				{
					ResourceHelper.theArrowUpImage = new Bitmap(ResourceHelper.LoadResourceStream("WcfTestClient.Resources.ArrowUp.bmp"));
					ResourceHelper.theArrowUpImage.MakeTransparent();
				}
				return ResourceHelper.theArrowUpImage;
			}
		}

		internal static Bitmap ContractImage
		{
			get
			{
				if (ResourceHelper.theContractImage == null)
				{
					ResourceHelper.theContractImage = new Bitmap(ResourceHelper.LoadResourceStream("WcfTestClient.Resources.Contract.bmp"));
					ResourceHelper.theContractImage.MakeTransparent();
				}
				return ResourceHelper.theContractImage;
			}
		}

		internal static Bitmap EndpointImage
		{
			get
			{
				if (ResourceHelper.theEndpointImage == null)
				{
					ResourceHelper.theEndpointImage = new Bitmap(ResourceHelper.LoadResourceStream("WcfTestClient.Resources.Endpoint.bmp"));
					ResourceHelper.theEndpointImage.MakeTransparent();
				}
				return ResourceHelper.theEndpointImage;
			}
		}

		internal static Bitmap ErrorImage
		{
			get
			{
				if (ResourceHelper.theErrorImage == null)
				{
					ResourceHelper.theErrorImage = new Bitmap(ResourceHelper.LoadResourceStream("WcfTestClient.Resources.Error.bmp"));
					ResourceHelper.theErrorImage.MakeTransparent();
				}
				return ResourceHelper.theErrorImage;
			}
		}

		internal static Bitmap FileImage
		{
			get
			{
				if (ResourceHelper.theFileImage == null)
				{
					ResourceHelper.theFileImage = new Bitmap(ResourceHelper.LoadResourceStream("WcfTestClient.Resources.File.bmp"));
					ResourceHelper.theFileImage.MakeTransparent();
				}
				return ResourceHelper.theFileImage;
			}
		}

		internal static Bitmap OperationImage
		{
			get
			{
				if (ResourceHelper.theOperationImage == null)
				{
					ResourceHelper.theOperationImage = new Bitmap(ResourceHelper.LoadResourceStream("WcfTestClient.Resources.Operation.bmp"));
					ResourceHelper.theOperationImage.MakeTransparent();
				}
				return ResourceHelper.theOperationImage;
			}
		}

		private static Stream LoadResourceStream(string resourceName)
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
		}
	}
}