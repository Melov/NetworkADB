using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[ComVisible(false)]
	internal sealed class Win32WindowMarshaler : ICustomMarshaler
	{
		private static ICustomMarshaler instance = null;

		public static ICustomMarshaler GetInstance(string cookie)
		{
			if (instance == null)
			{
				instance = new Win32WindowMarshaler();
			}

			return instance;
		}

		private Win32WindowMarshaler() {}

		void ICustomMarshaler.CleanUpManagedData(object managedObj)
		{
		}

		void ICustomMarshaler.CleanUpNativeData(IntPtr pNativeData)
		{
		}

		int ICustomMarshaler.GetNativeDataSize()
		{
			return IntPtr.Size;
		}

		IntPtr ICustomMarshaler.MarshalManagedToNative(object managedObj)
		{
			if (!(managedObj is IWin32Window))
			{
				throw new ArgumentOutOfRangeException("managedObj", managedObj, "expected an IWin32Window");
			}

			return managedObj == null
				? IntPtr.Zero
				: (managedObj as IWin32Window).Handle;
		}

		object ICustomMarshaler.MarshalNativeToManaged(IntPtr pNativeData)
		{
			return Win32Window.Create(pNativeData);
		}
	}
}
