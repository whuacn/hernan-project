/***************************************************************************

Copyright (c) Hernán Javier Hegykozi. All rights reserved.

***************************************************************************/

using System;
using System.Globalization;
using MsVsShell = Microsoft.VisualStudio.Shell;

namespace Sipro.SourceControlProvider.SiproSCP
{
	/// <summary>
	/// This attribute registers the source control provider.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class ProvideSourceControlProvider : MsVsShell.RegistrationAttribute
	{
        private string _regName = null;
        private string _uiName = null;
        
        /// <summary>
		/// </summary>
        public ProvideSourceControlProvider(string regName, string uiName)
		{
            _regName = regName;
            _uiName = uiName;
    	}

        /// <summary>
        /// Get the friendly name of the provider (written in registry)
        /// </summary>
        public string RegName
        {
            get { return _regName; }
        }

        /// <summary>
        /// Get the unique guid identifying the provider
        /// </summary>
        public Guid RegGuid
        {
            get { return GuidList.guidSiproSCP; }
        }

        /// <summary>
        /// Get the UI name of the provider (string resource ID)
        /// </summary>
        public string UIName
        {
            get { return _uiName; }
        }

        /// <summary>
        /// Get the package containing the UI name of the provider
        /// </summary>
        public Guid UINamePkg
        {
            get { return GuidList.guidSiproSCPPkg; }
        }

        /// <summary>
        /// Get the guid of the provider's service
        /// </summary>
        public Guid SiproSCPService
        {
            get { return GuidList.guidSiproSCPService; }
        }

		/// <summary>
		///     Called to register this attribute with the given context.  The context
		///     contains the location where the registration inforomation should be placed.
		///     It also contains other information such as the type being registered and path information.
		/// </summary>
        public override void Register(RegistrationContext context)
		{
            // Write to the context's log what we are about to do
            context.Log.WriteLine(String.Format(CultureInfo.CurrentCulture, "SiproSCP:\t\t{0}\n", RegName));

            // Declare the source control provider, its name, the provider's service 
            // and aditionally the packages implementing this provider
            using (Key SiproSCPs = context.CreateKey("SourceControlProviders"))
            {
                using (Key SiproSCPKey = SiproSCPs.CreateSubkey(RegGuid.ToString("B")))
                {
                    SiproSCPKey.SetValue("", RegName);
                    SiproSCPKey.SetValue("Service", SiproSCPService.ToString("B"));

                    using (Key SiproSCPNameKey = SiproSCPKey.CreateSubkey("Name"))
                    {
                        SiproSCPNameKey.SetValue("", UIName);
                        SiproSCPNameKey.SetValue("Package", UINamePkg.ToString("B"));

                        SiproSCPNameKey.Close();
                    }

                    // Additionally, you can create a "Packages" subkey where you can enumerate the dll
                    // that are used by the source control provider, something like "Package1"="SiproSCP.dll"
                    // but this is not a requirement.
                    SiproSCPKey.Close();
                }

                SiproSCPs.Close();
            }
		}

		/// <summary>
		/// Unregister the source control provider
		/// </summary>
		/// <param name="context"></param>
        public override void Unregister(RegistrationContext context)
		{
            context.RemoveKey("SourceControlProviders\\" + GuidList.guidSiproSCPPkg.ToString("B"));
		}
	}
}
