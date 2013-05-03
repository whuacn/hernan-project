using System;
namespace Persits.PDF
{
	internal class TreeNode
	{
		public int m_nValue;
		public int[] m_nChild = new int[2];
		public TreeNode(int Value, int Child1, int Child2)
		{
			this.m_nValue = Value;
			this.m_nChild[0] = Child1;
			this.m_nChild[1] = Child2;
		}
	}
}
