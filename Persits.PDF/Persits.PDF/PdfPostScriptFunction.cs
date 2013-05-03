using System;
namespace Persits.PDF
{
	internal class PdfPostScriptFunction : PdfFunctionTop
	{
		private PSObject[] code;
		private int codeSize;
		private bool ok;
		private static string[] psOpNames = new string[]
		{
			"abs",
			"add",
			"and",
			"atan",
			"bitshift",
			"ceiling",
			"copy",
			"cos",
			"cvi",
			"cvr",
			"div",
			"dup",
			"eq",
			"exch",
			"exp",
			"false",
			"floor",
			"ge",
			"gt",
			"idiv",
			"index",
			"le",
			"ln",
			"log",
			"lt",
			"mod",
			"mul",
			"ne",
			"neg",
			"not",
			"or",
			"pop",
			"roll",
			"round",
			"sin",
			"sqrt",
			"sub",
			"true",
			"truncate",
			"xor"
		};
		internal PdfPostScriptFunction(PdfObject funcObj, PdfDict dict)
		{
			this.code = null;
			this.codeSize = 0;
			this.ok = false;
			if (!base.init(dict))
			{
				return;
			}
			if (!this.hasRange)
			{
				AuxException.Throw("Type 4 function is missing range.", PdfErrors._ERROR_PREVIEW_PARSE);
				return;
			}
			if (funcObj.m_nType != enumType.pdfDictWithStream)
			{
				AuxException.Throw("Type 4 function isn't a stream.", PdfErrors._ERROR_PREVIEW_PARSE);
				return;
			}
			PdfDictWithStream pdfDictWithStream = (PdfDictWithStream)funcObj;
			pdfDictWithStream.reset();
			PdfString token;
			if ((token = this.getToken(pdfDictWithStream)) == null || token[0] != 123)
			{
				AuxException.Throw("Expected '{' at start of PostScript function.", PdfErrors._ERROR_PREVIEW_PARSE);
				return;
			}
			int num = 0;
			if (!this.parseCode(pdfDictWithStream, ref num))
			{
				return;
			}
			this.ok = true;
		}
		private bool parseCode(PdfDictWithStream str, ref int codePtr)
		{
			int num = 0;
			PdfString token;
			while ((token = this.getToken(str)) != null)
			{
				string text = token.ToString();
				int i = 0;
				if (char.IsDigit(text[i]) || text[i] == '.' || text[i] == '-')
				{
					bool flag = false;
					for (i++; i < text.Length; i++)
					{
						if (text[i] == '.')
						{
							flag = true;
							break;
						}
					}
					this.resizeCode(codePtr);
					if (flag)
					{
						this.code[codePtr].type = PSObjectType.psReal;
						double real = 0.0;
						double.TryParse(text, out real);
						this.code[codePtr].real = real;
					}
					else
					{
						this.code[codePtr].type = PSObjectType.psInt;
						int intg = 0;
						int.TryParse(text, out intg);
						this.code[codePtr].intg = intg;
					}
					codePtr++;
				}
				else
				{
					if (string.Compare(text, "{") == 0)
					{
						int num2 = codePtr;
						codePtr += 3;
						this.resizeCode(num2 + 2);
						if (!this.parseCode(str, ref codePtr))
						{
							return false;
						}
						if ((token = this.getToken(str)) == null)
						{
							AuxException.Throw("Unexpected end of PostScript function stream.", PdfErrors._ERROR_PREVIEW_PARSE);
							return false;
						}
						text = token.ToString();
						int num3;
						if (string.Compare(text, "{") == 0)
						{
							num3 = codePtr;
							if (!this.parseCode(str, ref codePtr))
							{
								return false;
							}
							if ((token = this.getToken(str)) == null)
							{
								AuxException.Throw("Unexpected end of PostScript function stream.", PdfErrors._ERROR_PREVIEW_PARSE);
								return false;
							}
						}
						else
						{
							num3 = -1;
						}
						text = token.ToString();
						if (string.Compare(text, "if") == 0)
						{
							if (num3 >= 0)
							{
								AuxException.Throw("Got 'if' operator with two blocks in PostScript function.", PdfErrors._ERROR_PREVIEW_PARSE);
								return false;
							}
							this.code[num2].type = PSObjectType.psOperator;
							this.code[num2].op = PSOp.psOpIf;
							this.code[num2 + 2].type = PSObjectType.psBlock;
							this.code[num2 + 2].blk = codePtr;
						}
						else
						{
							if (string.Compare(text, "ifelse") != 0)
							{
								AuxException.Throw("Expected if/ifelse operator in PostScript function.", PdfErrors._ERROR_PREVIEW_PARSE);
								return false;
							}
							if (num3 < 0)
							{
								AuxException.Throw("Got 'ifelse' operator with one blocks in PostScript function.", PdfErrors._ERROR_PREVIEW_PARSE);
								return false;
							}
							this.code[num2].type = PSObjectType.psOperator;
							this.code[num2].op = PSOp.psOpIfelse;
							this.code[num2 + 1].type = PSObjectType.psBlock;
							this.code[num2 + 1].blk = num3;
							this.code[num2 + 2].type = PSObjectType.psBlock;
							this.code[num2 + 2].blk = codePtr;
						}
					}
					else
					{
						if (string.Compare(text, "}") == 0)
						{
							this.resizeCode(codePtr);
							this.code[codePtr].type = PSObjectType.psOperator;
							this.code[codePtr].op = PSOp.psOpReturn;
							codePtr++;
							return true;
						}
						int num4 = -1;
						int num5 = PdfPostScriptFunction.psOpNames.Length;
						while (num5 - num4 > 1)
						{
							int num6 = (num4 + num5) / 2;
							num = string.Compare(text, PdfPostScriptFunction.psOpNames[num6]);
							if (num > 0)
							{
								num4 = num6;
							}
							else
							{
								if (num < 0)
								{
									num5 = num6;
								}
								else
								{
									num5 = (num4 = num6);
								}
							}
						}
						if (num != 0)
						{
							AuxException.Throw("Unknown operator in PostScript function.", PdfErrors._ERROR_PREVIEW_PARSE);
							return false;
						}
						this.resizeCode(codePtr);
						this.code[codePtr].type = PSObjectType.psOperator;
						this.code[codePtr].op = (PSOp)num4;
						codePtr++;
					}
				}
			}
			AuxException.Throw("Unexpected end of PostScript function stream", PdfErrors._ERROR_PREVIEW_PARSE);
			return false;
		}
		private PdfString getToken(PdfDictWithStream str)
		{
			PdfString pdfString = new PdfString();
			int num;
			do
			{
				num = str.getChar();
			}
			while (num != -1 && char.IsWhiteSpace((char)num));
			if (num == 123 || num == 125)
			{
				pdfString.AppendChar((byte)num);
			}
			else
			{
				if (char.IsDigit((char)num) || num == 46 || num == 45)
				{
					while (true)
					{
						pdfString.AppendChar((byte)num);
						num = str.lookChar();
						if (num == -1 || (!char.IsDigit((char)num) && num != 46 && num != 45))
						{
							break;
						}
						str.getChar();
					}
				}
				else
				{
					while (true)
					{
						pdfString.AppendChar((byte)num);
						num = str.lookChar();
						if (num == -1 || !char.IsLetterOrDigit((char)num))
						{
							break;
						}
						str.getChar();
					}
				}
			}
			pdfString.TestForAnsi();
			return pdfString;
		}
		private void resizeCode(int newSize)
		{
			if (newSize >= this.codeSize)
			{
				this.codeSize += 64;
				PSObject[] destinationArray = new PSObject[this.codeSize];
				if (this.code != null)
				{
					Array.Copy(this.code, 0, destinationArray, 0, this.code.Length);
				}
				this.code = destinationArray;
			}
		}
		internal PdfPostScriptFunction(PdfPostScriptFunction func) : base(func)
		{
			this.codeSize = func.codeSize;
			this.code = new PSObject[this.codeSize];
			Array.Copy(func.code, this.code, this.codeSize);
		}
		internal override PdfFunctionTop copy()
		{
			return new PdfPostScriptFunction(this);
		}
		internal override void transform(double[] inv, double[] outv)
		{
			PSStack pSStack = new PSStack();
			for (int i = 0; i < this.m; i++)
			{
				pSStack.pushReal(inv[i]);
			}
			this.exec(pSStack, 0);
			for (int i = this.n - 1; i >= 0; i--)
			{
				outv[i] = pSStack.popNum();
				if (outv[i] < this.range[i, 0])
				{
					outv[i] = this.range[i, 0];
				}
				else
				{
					if (outv[i] > this.range[i, 1])
					{
						outv[i] = this.range[i, 1];
					}
				}
			}
		}
		private void exec(PSStack stack, int codePtr)
		{
			while (true)
			{
				switch (this.code[codePtr].type)
				{
				case PSObjectType.psInt:
					stack.pushInt(this.code[codePtr++].intg);
					break;
				case PSObjectType.psReal:
					stack.pushReal(this.code[codePtr++].real);
					break;
				case PSObjectType.psOperator:
					switch (this.code[codePtr++].op)
					{
					case PSOp.psOpAbs:
						if (stack.topIsInt())
						{
							stack.pushInt(Math.Abs(stack.popInt()));
						}
						else
						{
							stack.pushReal(Math.Abs(stack.popNum()));
						}
						break;
					case PSOp.psOpAdd:
						if (stack.topTwoAreInts())
						{
							int num = stack.popInt();
							int num2 = stack.popInt();
							stack.pushInt(num2 + num);
						}
						else
						{
							double num3 = stack.popNum();
							double num4 = stack.popNum();
							stack.pushReal(num4 + num3);
						}
						break;
					case PSOp.psOpAnd:
						if (stack.topTwoAreInts())
						{
							int num = stack.popInt();
							int num2 = stack.popInt();
							stack.pushInt(num2 & num);
						}
						else
						{
							bool flag = stack.popBool();
							bool flag2 = stack.popBool();
							stack.pushBool(flag2 && flag);
						}
						break;
					case PSOp.psOpAtan:
					{
						double num3 = stack.popNum();
						double num4 = stack.popNum();
						stack.pushReal(Math.Atan2(num4, num3));
						break;
					}
					case PSOp.psOpBitshift:
					{
						int num = stack.popInt();
						int num2 = stack.popInt();
						if (num > 0)
						{
							stack.pushInt(num2 << num);
						}
						else
						{
							if (num < 0)
							{
								stack.pushInt((int)((uint)num2 >> num));
							}
							else
							{
								stack.pushInt(num2);
							}
						}
						break;
					}
					case PSOp.psOpCeiling:
						if (!stack.topIsInt())
						{
							stack.pushReal(Math.Ceiling(stack.popNum()));
						}
						break;
					case PSOp.psOpCopy:
						stack.copy(stack.popInt());
						break;
					case PSOp.psOpCos:
						stack.pushReal(Math.Cos(stack.popNum()));
						break;
					case PSOp.psOpCvi:
						if (!stack.topIsInt())
						{
							stack.pushInt((int)stack.popNum());
						}
						break;
					case PSOp.psOpCvr:
						if (!stack.topIsReal())
						{
							stack.pushReal(stack.popNum());
						}
						break;
					case PSOp.psOpDiv:
					{
						double num3 = stack.popNum();
						double num4 = stack.popNum();
						stack.pushReal(num4 / num3);
						break;
					}
					case PSOp.psOpDup:
						stack.copy(1);
						break;
					case PSOp.psOpEq:
						if (stack.topTwoAreInts())
						{
							int num = stack.popInt();
							int num2 = stack.popInt();
							stack.pushBool(num2 == num);
						}
						else
						{
							if (stack.topTwoAreNums())
							{
								double num3 = stack.popNum();
								double num4 = stack.popNum();
								stack.pushBool(num4 == num3);
							}
							else
							{
								bool flag = stack.popBool();
								bool flag2 = stack.popBool();
								stack.pushBool(flag2 == flag);
							}
						}
						break;
					case PSOp.psOpExch:
						stack.roll(2, 1);
						break;
					case PSOp.psOpExp:
					{
						double num3 = stack.popNum();
						double num4 = stack.popNum();
						stack.pushReal(Math.Pow(num4, num3));
						break;
					}
					case PSOp.psOpFalse:
						stack.pushBool(false);
						break;
					case PSOp.psOpFloor:
						if (!stack.topIsInt())
						{
							stack.pushReal(Math.Floor(stack.popNum()));
						}
						break;
					case PSOp.psOpGe:
						if (stack.topTwoAreInts())
						{
							int num = stack.popInt();
							int num2 = stack.popInt();
							stack.pushBool(num2 >= num);
						}
						else
						{
							double num3 = stack.popNum();
							double num4 = stack.popNum();
							stack.pushBool(num4 >= num3);
						}
						break;
					case PSOp.psOpGt:
						if (stack.topTwoAreInts())
						{
							int num = stack.popInt();
							int num2 = stack.popInt();
							stack.pushBool(num2 > num);
						}
						else
						{
							double num3 = stack.popNum();
							double num4 = stack.popNum();
							stack.pushBool(num4 > num3);
						}
						break;
					case PSOp.psOpIdiv:
					{
						int num = stack.popInt();
						int num2 = stack.popInt();
						stack.pushInt(num2 / num);
						break;
					}
					case PSOp.psOpIndex:
						stack.index(stack.popInt());
						break;
					case PSOp.psOpLe:
						if (stack.topTwoAreInts())
						{
							int num = stack.popInt();
							int num2 = stack.popInt();
							stack.pushBool(num2 <= num);
						}
						else
						{
							double num3 = stack.popNum();
							double num4 = stack.popNum();
							stack.pushBool(num4 <= num3);
						}
						break;
					case PSOp.psOpLn:
						stack.pushReal(Math.Log(stack.popNum()));
						break;
					case PSOp.psOpLog:
						stack.pushReal(Math.Log10(stack.popNum()));
						break;
					case PSOp.psOpLt:
						if (stack.topTwoAreInts())
						{
							int num = stack.popInt();
							int num2 = stack.popInt();
							stack.pushBool(num2 < num);
						}
						else
						{
							double num3 = stack.popNum();
							double num4 = stack.popNum();
							stack.pushBool(num4 < num3);
						}
						break;
					case PSOp.psOpMod:
					{
						int num = stack.popInt();
						int num2 = stack.popInt();
						stack.pushInt(num2 % num);
						break;
					}
					case PSOp.psOpMul:
						if (stack.topTwoAreInts())
						{
							int num = stack.popInt();
							int num2 = stack.popInt();
							stack.pushInt(num2 * num);
						}
						else
						{
							double num3 = stack.popNum();
							double num4 = stack.popNum();
							stack.pushReal(num4 * num3);
						}
						break;
					case PSOp.psOpNe:
						if (stack.topTwoAreInts())
						{
							int num = stack.popInt();
							int num2 = stack.popInt();
							stack.pushBool(num2 != num);
						}
						else
						{
							if (stack.topTwoAreNums())
							{
								double num3 = stack.popNum();
								double num4 = stack.popNum();
								stack.pushBool(num4 != num3);
							}
							else
							{
								bool flag = stack.popBool();
								bool flag2 = stack.popBool();
								stack.pushBool(flag2 != flag);
							}
						}
						break;
					case PSOp.psOpNeg:
						if (stack.topIsInt())
						{
							stack.pushInt(-stack.popInt());
						}
						else
						{
							stack.pushReal(-stack.popNum());
						}
						break;
					case PSOp.psOpNot:
						if (stack.topIsInt())
						{
							stack.pushInt(~stack.popInt());
						}
						else
						{
							stack.pushBool(!stack.popBool());
						}
						break;
					case PSOp.psOpOr:
						if (stack.topTwoAreInts())
						{
							int num = stack.popInt();
							int num2 = stack.popInt();
							stack.pushInt(num2 | num);
						}
						else
						{
							bool flag = stack.popBool();
							stack.pushBool(stack.popBool() || flag);
						}
						break;
					case PSOp.psOpPop:
						stack.pop();
						break;
					case PSOp.psOpRoll:
					{
						int num = stack.popInt();
						int num2 = stack.popInt();
						stack.roll(num2, num);
						break;
					}
					case PSOp.psOpRound:
						if (!stack.topIsInt())
						{
							double num4 = stack.popNum();
							stack.pushReal((num4 >= 0.0) ? Math.Floor(num4 + 0.5) : Math.Ceiling(num4 - 0.5));
						}
						break;
					case PSOp.psOpSin:
						stack.pushReal(Math.Sin(stack.popNum()));
						break;
					case PSOp.psOpSqrt:
						stack.pushReal(Math.Sqrt(stack.popNum()));
						break;
					case PSOp.psOpSub:
						if (stack.topTwoAreInts())
						{
							int num = stack.popInt();
							int num2 = stack.popInt();
							stack.pushInt(num2 - num);
						}
						else
						{
							double num3 = stack.popNum();
							double num4 = stack.popNum();
							stack.pushReal(num4 - num3);
						}
						break;
					case PSOp.psOpTrue:
						stack.pushBool(true);
						break;
					case PSOp.psOpTruncate:
						if (!stack.topIsInt())
						{
							double num4 = stack.popNum();
							stack.pushReal((num4 >= 0.0) ? Math.Floor(num4) : Math.Ceiling(num4));
						}
						break;
					case PSOp.psOpXor:
						if (stack.topTwoAreInts())
						{
							int num = stack.popInt();
							int num2 = stack.popInt();
							stack.pushInt(num2 ^ num);
						}
						else
						{
							bool flag = stack.popBool();
							bool flag2 = stack.popBool();
							stack.pushBool(flag2 ^ flag);
						}
						break;
					case PSOp.psOpIf:
					{
						bool flag2 = stack.popBool();
						if (flag2)
						{
							this.exec(stack, codePtr + 2);
						}
						codePtr = this.code[codePtr + 1].blk;
						break;
					}
					case PSOp.psOpIfelse:
					{
						bool flag2 = stack.popBool();
						if (flag2)
						{
							this.exec(stack, codePtr + 2);
						}
						else
						{
							this.exec(stack, this.code[codePtr].blk);
						}
						codePtr = this.code[codePtr + 1].blk;
						break;
					}
					case PSOp.psOpReturn:
						return;
					}
					break;
				default:
					AuxException.Throw("Internal: bad object in PostScript function code.", PdfErrors._ERROR_PREVIEW_PARSE);
					break;
				}
			}
		}
		internal override bool isOk()
		{
			return this.ok;
		}
	}
}
