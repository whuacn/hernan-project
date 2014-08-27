using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Globalization;
using System.IO;
using System.Text;

namespace Microsoft.Tools.TestClient
{
	internal class StringFormatter
	{
		public StringFormatter()
		{
		}

		internal static string FromEscapeCode(string input)
		{
			char[] charArray = input.ToCharArray();
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			int num1 = 0;
			while (num < (int)charArray.Length)
			{
				char chr = charArray[num];
				if (num1 != 0)
				{
					if (num1 != 1)
					{
						return null;
					}
					if (chr == 'r')
					{
						num1 = 0;
						stringBuilder.Append('\r');
					}
					else if (chr == 'n')
					{
						num1 = 0;
						stringBuilder.Append('\n');
					}
					else if (chr != 't')
					{
						if (chr != '\\')
						{
							return null;
						}
						num1 = 0;
						stringBuilder.Append('\\');
					}
					else
					{
						num1 = 0;
						stringBuilder.Append('\t');
					}
				}
				else if (charArray[num] != '\\')
				{
					num1 = 0;
					stringBuilder.Append(chr);
				}
				else
				{
					num1 = 1;
				}
				num++;
			}
			if (num1 != 0)
			{
				return null;
			}
			return stringBuilder.ToString();
		}

		internal static string ToEscapeCode(string input)
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture);
			(new CSharpCodeProvider()).GenerateCodeFromExpression(new CodePrimitiveExpression(input), stringWriter, new CodeGeneratorOptions());
			return stringWriter.ToString();
		}
	}
}