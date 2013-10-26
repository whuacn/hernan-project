using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Sipro.Providers
{
    public class AuthenticationProvider : ConfigurationElement, IConfigurationSectionHandler
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
        }

        [ConfigurationProperty("test", DefaultValue = "123", IsRequired = false)]
        public string Test
        {
            get
            {
                return this["test"] as string;
            }
        }

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            return this;
        }
    }
}