using System;
namespace Persits.PDF
{
	internal class RavenColor
	{
		internal static int ravenMaxColorComps = 3;
		private byte[] components = new byte[RavenColor.ravenMaxColorComps];
		internal byte this[int i]
		{
			get
			{
				return this.components[i];
			}
			set
			{
				this.components[i] = value;
			}
		}
		internal byte[] getPtr()
		{
			return this.components;
		}
	}
}
