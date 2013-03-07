using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Srt.WebControlExtend
{
    public enum ValidationType
    {
        [StringValue("none")]
        None = 0,
        [StringValue("numeric")]
        Numeric = 1,
        [StringValue("email")]
        Email = 2,
        [StringValue("alpha")]
        AlphaNumeric = 3,
        [StringValue("date")]
        Date = 4
    }

    public class StringValue : System.Attribute
    {
        private string _value;

        public StringValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }

    public static class Converter
    {
        public static string EnumToString(Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            System.Reflection.FieldInfo fieldInfo = type.GetField(value.ToString());
            // Get the stringvalue attributes
            StringValue[] attribs = fieldInfo.GetCustomAttributes(typeof(StringValue), false) as StringValue[];
            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].Value : null;
        }

    }
}
