using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[ComVisible(false)]
	public sealed class ItemIdMarshaler : ICustomMarshaler
	{
		private static ICustomMarshaler instance = null;

		public static ICustomMarshaler GetInstance(string cookie)
		{
			if (instance == null)
			{
				instance = new ItemIdMarshaler();
			}

			return instance;
		}

		private ItemIdMarshaler() {}

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
			if (!(managedObj is ItemId))
			{
				throw new ArgumentOutOfRangeException("managedObj", managedObj, "expected an ItemId");
			}

			return managedObj == null
				? IntPtr.Zero
				: (managedObj as ItemId).Ptr;
		}

		object ICustomMarshaler.MarshalNativeToManaged(IntPtr pNativeData)
		{
			return ItemId.Create(pNativeData);
		}
	}
}
