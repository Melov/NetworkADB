using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[ComVisible(false)]
	internal sealed class IconMarshaler : ICustomMarshaler
	{
		private static ICustomMarshaler instance = null;

		public static ICustomMarshaler GetInstance(string cookie)
		{
			if (instance == null)
			{
				instance = new IconMarshaler();
			}

			return instance;
		}

		private IconMarshaler() {}

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
			if (!(managedObj is Icon))
			{
				throw new ArgumentOutOfRangeException("managedObj", managedObj, "expected an Icon");
			}

			return managedObj == null
				? IntPtr.Zero
				: (managedObj as Icon).Handle;
		}

		object ICustomMarshaler.MarshalNativeToManaged(IntPtr pNativeData)
		{
			return Icon.FromHandle(pNativeData);
		}
	}
}
