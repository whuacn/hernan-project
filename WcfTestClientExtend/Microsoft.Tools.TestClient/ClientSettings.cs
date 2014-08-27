using System;
using System.Reflection;

namespace Microsoft.Tools.TestClient
{
	internal static class ClientSettings
	{
		private static Assembly clientAssembly;

		internal static Assembly ClientAssembly
		{
			get
			{
				if (ClientSettings.clientAssembly == null)
				{
					string data = (string)AppDomain.CurrentDomain.GetData("clientAssemblyPath");
					AssemblyName assemblyName = new AssemblyName()
					{
						CodeBase = data
					};
					ClientSettings.clientAssembly = Assembly.Load(assemblyName);
				}
				return ClientSettings.clientAssembly;
			}
		}

		internal static Type GetType(string typeName)
		{
			Type type = ClientSettings.ClientAssembly.GetType(typeName);
			if (type == null)
			{
				type = Type.GetType(typeName);
				if (type == null)
				{
					AssemblyName[] referencedAssemblies = ClientSettings.ClientAssembly.GetReferencedAssemblies();
					for (int i = 0; i < (int)referencedAssemblies.Length; i++)
					{
						Assembly assembly = Assembly.Load(referencedAssemblies[i]);
						if (assembly != null)
						{
							type = assembly.GetType(typeName);
							if (type != null)
							{
								break;
							}
						}
					}
				}
			}
			return type;
		}
	}
}