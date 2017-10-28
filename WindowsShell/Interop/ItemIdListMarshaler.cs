using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[ComVisible(false)]
	public sealed class ItemIdListMarshaler : ICustomMarshaler
	{
		private static ICustomMarshaler instance = null;

		public static ICustomMarshaler GetInstance(string cookie)
		{
			if (instance == null)
			{
				instance = new ItemIdListMarshaler();
			}

			return instance;
		}

		private ItemIdListMarshaler() {}

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
			if (!(managedObj is ItemIdList))
			{
				throw new ArgumentOutOfRangeException("managedObj", managedObj, "expected an ItemIdList");
			}

			return managedObj == null
				? IntPtr.Zero
				: (managedObj as ItemIdList).Ptr;
		}

		object ICustomMarshaler.MarshalNativeToManaged(IntPtr pNativeData)
		{
			return ItemIdList.Create(pNativeData);
		}
	}
}
