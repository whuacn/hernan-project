using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
namespace Persits.PDF
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
	internal class AspPDF
	{
		private static ResourceManager resourceMan;
		private static CultureInfo resourceCulture;
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(AspPDF.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("AspPDF.AspPDF", typeof(AspPDF).Assembly);
					AspPDF.resourceMan = resourceManager;
				}
				return AspPDF.resourceMan;
			}
		}
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return AspPDF.resourceCulture;
			}
			set
			{
				AspPDF.resourceCulture = value;
			}
		}
		internal static byte[] barcodef
		{
			get
			{
				object @object = AspPDF.ResourceManager.GetObject("barcodef", AspPDF.resourceCulture);
				return (byte[])@object;
			}
		}
		internal static byte[] USWebUncoated_icc
		{
			get
			{
				object @object = AspPDF.ResourceManager.GetObject("USWebUncoated_icc", AspPDF.resourceCulture);
				return (byte[])@object;
			}
		}
		internal AspPDF()
		{
		}
	}
}
