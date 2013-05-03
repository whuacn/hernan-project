using System;
namespace Persits.PDF
{
	internal class PSStack
	{
		private const int psStackSize = 100;
		private PSObject[] stack = new PSObject[100];
		private int sp;
		internal PSStack()
		{
			this.sp = 100;
		}
		internal void pushBool(bool booln)
		{
			if (this.checkOverflow())
			{
				this.stack[--this.sp].type = PSObjectType.psBool;
				this.stack[this.sp].booln = booln;
			}
		}
		internal void pushInt(int intg)
		{
			if (this.checkOverflow())
			{
				this.stack[--this.sp].type = PSObjectType.psInt;
				this.stack[this.sp].intg = intg;
			}
		}
		internal void pushReal(double real)
		{
			if (this.checkOverflow())
			{
				this.stack[--this.sp].type = PSObjectType.psReal;
				this.stack[this.sp].real = real;
			}
		}
		internal bool popBool()
		{
			return this.checkUnderflow() && this.checkType(PSObjectType.psBool, PSObjectType.psBool) && this.stack[this.sp++].booln;
		}
		internal int popInt()
		{
			if (this.checkUnderflow() && this.checkType(PSObjectType.psInt, PSObjectType.psInt))
			{
				return this.stack[this.sp++].intg;
			}
			return 0;
		}
		internal double popNum()
		{
			if (this.checkUnderflow() && this.checkType(PSObjectType.psInt, PSObjectType.psReal))
			{
				double result = (this.stack[this.sp].type == PSObjectType.psInt) ? ((double)this.stack[this.sp].intg) : this.stack[this.sp].real;
				this.sp++;
				return result;
			}
			return 0.0;
		}
		private bool empty()
		{
			return this.sp == 100;
		}
		internal bool topIsInt()
		{
			return this.sp < 100 && this.stack[this.sp].type == PSObjectType.psInt;
		}
		internal bool topTwoAreInts()
		{
			return this.sp < 99 && this.stack[this.sp].type == PSObjectType.psInt && this.stack[this.sp + 1].type == PSObjectType.psInt;
		}
		internal bool topIsReal()
		{
			return this.sp < 100 && this.stack[this.sp].type == PSObjectType.psReal;
		}
		internal bool topTwoAreNums()
		{
			return this.sp < 99 && (this.stack[this.sp].type == PSObjectType.psInt || this.stack[this.sp].type == PSObjectType.psReal) && (this.stack[this.sp + 1].type == PSObjectType.psInt || this.stack[this.sp + 1].type == PSObjectType.psReal);
		}
		internal void copy(int n)
		{
			if (!this.checkOverflow(n))
			{
				return;
			}
			for (int i = this.sp + n - 1; i <= this.sp; i++)
			{
				this.stack[i - n] = this.stack[i];
			}
			this.sp -= n;
		}
		internal void roll(int n, int j)
		{
			if (j >= 0)
			{
				j %= n;
			}
			else
			{
				j = -j % n;
				if (j != 0)
				{
					j = n - j;
				}
			}
			if (n <= 0 || j == 0)
			{
				return;
			}
			for (int i = 0; i < j; i++)
			{
				PSObject pSObject = this.stack[this.sp];
				for (int k = this.sp; k < this.sp + n - 1; k++)
				{
					this.stack[k] = this.stack[k + 1];
				}
				this.stack[this.sp + n - 1] = pSObject;
			}
		}
		internal void index(int i)
		{
			if (!this.checkOverflow())
			{
				return;
			}
			this.sp--;
			this.stack[this.sp] = this.stack[this.sp + 1 + i];
		}
		internal void pop()
		{
			if (!this.checkUnderflow())
			{
				return;
			}
			this.sp++;
		}
		private bool checkOverflow()
		{
			return this.checkOverflow(1);
		}
		private bool checkOverflow(int n)
		{
			if (this.sp - n < 0)
			{
				AuxException.Throw("Stack overflow in PostScript function.", PdfErrors._ERROR_PREVIEW_PARSE);
				return false;
			}
			return true;
		}
		private bool checkUnderflow()
		{
			if (this.sp == 100)
			{
				AuxException.Throw("Stack underflow in PostScript function.", PdfErrors._ERROR_PREVIEW_PARSE);
				return false;
			}
			return true;
		}
		private bool checkType(PSObjectType t1, PSObjectType t2)
		{
			if (this.stack[this.sp].type != t1 && this.stack[this.sp].type != t2)
			{
				AuxException.Throw("Type mismatch in PostScript function.", PdfErrors._ERROR_PREVIEW_PARSE);
				return false;
			}
			return true;
		}
	}
}
