/***************************************************************************

Copyright (c) Hernán Javier Hegykozi. All rights reserved.

***************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VsSDK.UnitTestLibrary;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Samples.VisualStudio.SourceControlIntegration.SccProvider;

namespace Microsoft.Samples.VisualStudio.SourceControlIntegration.SccProvider.UnitTests
{
    class MockRegisterScciProvider
    {
		private static GenericMockFactory registerScciProviderFactory = null;

        #region RegisterScciProvider Getters

        /// <summary>
        /// Return a IVsRegisterScciProvider without any special implementation
		/// </summary>
		/// <returns></returns>
        internal static IVsRegisterScciProvider GetBaseRegisterScciProvider()
		{
            if (registerScciProviderFactory == null)
                registerScciProviderFactory = new GenericMockFactory("RegisterScciProvider", new Type[] { typeof(IVsRegisterScciProvider) });
            IVsRegisterScciProvider registerProvider = (IVsRegisterScciProvider)registerScciProviderFactory.GetInstance();
            return registerProvider;
		}

		#endregion

		#region Callbacks
		#endregion
	}
}
