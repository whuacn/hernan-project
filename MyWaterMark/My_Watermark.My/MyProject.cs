using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace My_Watermark.My
{
	[StandardModule, HideModuleName, GeneratedCode("MyTemplate", "8.0.0.0")]
	internal sealed class MyProject
	{
		[MyGroupCollection("System.Windows.Forms.Form", "Create__Instance__", "Dispose__Instance__", "My.MyProject.Forms"), EditorBrowsable(EditorBrowsableState.Never)]
		internal sealed class MyForms
		{
			public Form_About m_Form_About;
			public Form_Bundle_Q m_Form_Bundle_Q;
			public Form_Donate m_Form_Donate;
			public Form_Main m_Form_Main;
			public Form_VerDwl m_Form_VerDwl;
			[ThreadStatic]
			private static Hashtable m_FormBeingCreated;
			public Form_About Form_About
			{
				[DebuggerNonUserCode]
				get
				{
					this.m_Form_About = MyProject.MyForms.Create__Instance__<Form_About>(this.m_Form_About);
					return this.m_Form_About;
				}
				[DebuggerNonUserCode]
				set
				{
					if (value == this.m_Form_About)
					{
						return;
					}
					if (value != null)
					{
						throw new ArgumentException("Property can only be set to Nothing");
					}
					this.Dispose__Instance__<Form_About>(this.m_Form_About);
				}
			}
			public Form_Bundle_Q Form_Bundle_Q
			{
				[DebuggerNonUserCode]
				get
				{
					this.m_Form_Bundle_Q = MyProject.MyForms.Create__Instance__<Form_Bundle_Q>(this.m_Form_Bundle_Q);
					return this.m_Form_Bundle_Q;
				}
				[DebuggerNonUserCode]
				set
				{
					if (value == this.m_Form_Bundle_Q)
					{
						return;
					}
					if (value != null)
					{
						throw new ArgumentException("Property can only be set to Nothing");
					}
					this.Dispose__Instance__<Form_Bundle_Q>(this.m_Form_Bundle_Q);
				}
			}
			public Form_Donate Form_Donate
			{
				[DebuggerNonUserCode]
				get
				{
					this.m_Form_Donate = MyProject.MyForms.Create__Instance__<Form_Donate>(this.m_Form_Donate);
					return this.m_Form_Donate;
				}
				[DebuggerNonUserCode]
				set
				{
					if (value == this.m_Form_Donate)
					{
						return;
					}
					if (value != null)
					{
						throw new ArgumentException("Property can only be set to Nothing");
					}
					this.Dispose__Instance__<Form_Donate>(this.m_Form_Donate);
				}
			}
			public Form_Main Form_Main
			{
				[DebuggerNonUserCode]
				get
				{
					this.m_Form_Main = MyProject.MyForms.Create__Instance__<Form_Main>(this.m_Form_Main);
					return this.m_Form_Main;
				}
				[DebuggerNonUserCode]
				set
				{
					if (value == this.m_Form_Main)
					{
						return;
					}
					if (value != null)
					{
						throw new ArgumentException("Property can only be set to Nothing");
					}
					this.Dispose__Instance__<Form_Main>(this.m_Form_Main);
				}
			}
			public Form_VerDwl Form_VerDwl
			{
				[DebuggerNonUserCode]
				get
				{
					this.m_Form_VerDwl = MyProject.MyForms.Create__Instance__<Form_VerDwl>(this.m_Form_VerDwl);
					return this.m_Form_VerDwl;
				}
				[DebuggerNonUserCode]
				set
				{
					if (value == this.m_Form_VerDwl)
					{
						return;
					}
					if (value != null)
					{
						throw new ArgumentException("Property can only be set to Nothing");
					}
					this.Dispose__Instance__<Form_VerDwl>(this.m_Form_VerDwl);
				}
			}
			[DebuggerHidden]
			private static T Create__Instance__<T>(T Instance) where T : Form, new()
			{
				if (Instance == null || Instance.IsDisposed)
				{
					if (MyProject.MyForms.m_FormBeingCreated != null)
					{
						if (MyProject.MyForms.m_FormBeingCreated.ContainsKey(typeof(T)))
						{
							throw new InvalidOperationException(Utils.GetResourceString("WinForms_RecursiveFormCreate", new string[0]));
						}
					}
					else
					{
						MyProject.MyForms.m_FormBeingCreated = new Hashtable();
					}
					MyProject.MyForms.m_FormBeingCreated.Add(typeof(T), null);
					try
					{
						try
						{
							return Activator.CreateInstance<T>();
						}
                        catch (Exception) { }
						object arg_74_0 = null;
						TargetInvocationException expr_79 = arg_74_0 as TargetInvocationException;
						int arg_96_0;
						if (expr_79 == null)
						{
							arg_96_0 = 0;
						}
						else
						{
							TargetInvocationException ex = expr_79;
							ProjectData.SetProjectError(expr_79);
							arg_96_0 = (((ex.InnerException != null) == false) ? 1 : 0);
						}
						//endfilter(arg_96_0);
					}
                    catch (Exception) { }
					finally
					{
						MyProject.MyForms.m_FormBeingCreated.Remove(typeof(T));
					}
					return Instance;
				}
				return Instance;
			}
			[DebuggerHidden]
			private void Dispose__Instance__<T>(T instance) where T : Form
			{
				instance.Dispose();
				instance = default(T);
			}
			[EditorBrowsable(EditorBrowsableState.Never), DebuggerHidden]
			public MyForms()
			{
			}
			[EditorBrowsable(EditorBrowsableState.Never)]
			public override bool Equals(object o)
			{
				return base.Equals(RuntimeHelpers.GetObjectValue(o));
			}
			[EditorBrowsable(EditorBrowsableState.Never)]
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
			[EditorBrowsable(EditorBrowsableState.Never)]
			internal new Type GetType()
			{
				return typeof(MyProject.MyForms);
			}
			[EditorBrowsable(EditorBrowsableState.Never)]
			public override string ToString()
			{
				return base.ToString();
			}
		}
		[MyGroupCollection("System.Web.Services.Protocols.SoapHttpClientProtocol", "Create__Instance__", "Dispose__Instance__", ""), EditorBrowsable(EditorBrowsableState.Never)]
		internal sealed class MyWebServices
		{
			[EditorBrowsable(EditorBrowsableState.Never), DebuggerHidden]
			public override bool Equals(object o)
			{
				return base.Equals(RuntimeHelpers.GetObjectValue(o));
			}
			[EditorBrowsable(EditorBrowsableState.Never), DebuggerHidden]
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
			[EditorBrowsable(EditorBrowsableState.Never), DebuggerHidden]
			internal new Type GetType()
			{
				return typeof(MyProject.MyWebServices);
			}
			[EditorBrowsable(EditorBrowsableState.Never), DebuggerHidden]
			public override string ToString()
			{
				return base.ToString();
			}
			[DebuggerHidden]
			private static T Create__Instance__<T>(T instance) where T : new()
			{
				if (instance == null)
				{
					return Activator.CreateInstance<T>();
				}
				return instance;
			}
			[DebuggerHidden]
			private void Dispose__Instance__<T>(T instance)
			{
				instance = default(T);
			}
			[EditorBrowsable(EditorBrowsableState.Never), DebuggerHidden]
			public MyWebServices()
			{
			}
		}
		[EditorBrowsable(EditorBrowsableState.Never), ComVisible(false)]
		internal sealed class ThreadSafeObjectProvider<T> where T : new()
		{
			[CompilerGenerated, ThreadStatic]
			private static T m_ThreadStaticValue;
			internal T GetInstance
			{
				[DebuggerHidden]
				get
				{
					if (MyProject.ThreadSafeObjectProvider<T>.m_ThreadStaticValue == null)
					{
						MyProject.ThreadSafeObjectProvider<T>.m_ThreadStaticValue = Activator.CreateInstance<T>();
					}
					return MyProject.ThreadSafeObjectProvider<T>.m_ThreadStaticValue;
				}
			}
			[EditorBrowsable(EditorBrowsableState.Never), DebuggerHidden]
			public ThreadSafeObjectProvider()
			{
			}
		}
		private static readonly MyProject.ThreadSafeObjectProvider<MyComputer> m_ComputerObjectProvider = new MyProject.ThreadSafeObjectProvider<MyComputer>();
		private static readonly MyProject.ThreadSafeObjectProvider<MyApplication> m_AppObjectProvider = new MyProject.ThreadSafeObjectProvider<MyApplication>();
		private static readonly MyProject.ThreadSafeObjectProvider<User> m_UserObjectProvider = new MyProject.ThreadSafeObjectProvider<User>();
		private static MyProject.ThreadSafeObjectProvider<MyProject.MyForms> m_MyFormsObjectProvider = new MyProject.ThreadSafeObjectProvider<MyProject.MyForms>();
		private static readonly MyProject.ThreadSafeObjectProvider<MyProject.MyWebServices> m_MyWebServicesObjectProvider = new MyProject.ThreadSafeObjectProvider<MyProject.MyWebServices>();
		[HelpKeyword("My.Computer")]
		internal static MyComputer Computer
		{
			[DebuggerHidden]
			get
			{
				return MyProject.m_ComputerObjectProvider.GetInstance;
			}
		}
		[HelpKeyword("My.Application")]
		internal static MyApplication Application
		{
			[DebuggerHidden]
			get
			{
				return MyProject.m_AppObjectProvider.GetInstance;
			}
		}
		[HelpKeyword("My.User")]
		internal static User User
		{
			[DebuggerHidden]
			get
			{
				return MyProject.m_UserObjectProvider.GetInstance;
			}
		}
		[HelpKeyword("My.Forms")]
		internal static MyProject.MyForms Forms
		{
			[DebuggerHidden]
			get
			{
				return MyProject.m_MyFormsObjectProvider.GetInstance;
			}
		}
		[HelpKeyword("My.WebServices")]
		internal static MyProject.MyWebServices WebServices
		{
			[DebuggerHidden]
			get
			{
				return MyProject.m_MyWebServicesObjectProvider.GetInstance;
			}
		}
	}
}
