using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[ComVisible(false)]
	public sealed class ItemIdList
	{
		public static ItemIdList Create(IntPtr p)
		{
			return p == IntPtr.Zero ? null : new ItemIdList(p);
		}

		public static unsafe ItemIdList Create(IMalloc m, params byte[][] data)
		{
			//if (m == null)
			{
			//	throw new ArgumentNullException("m");
			}

			int len = ItemId.HeaderSize; // terminator

			foreach (byte[] item in data)
			{
				len += item.Length + ItemId.HeaderSize;
			}

            IntPtr p = (m == null) ? Marshal.AllocCoTaskMem(len) : m.Alloc(len);

			if (p == IntPtr.Zero)
			{
				throw new OutOfMemoryException("Shell failed to allocate ITEMIDLIST");
			}

			IntPtr workingP = p;

			foreach (byte[] item in data)
			{
				*((ushort*) workingP) = checked ((ushort) (item.Length + ItemId.HeaderSize));
				workingP = (IntPtr) ((int) workingP + ItemId.HeaderSize);
				Marshal.Copy(item, 0, workingP, item.Length);
				workingP = (IntPtr) ((int) workingP + item.Length);
			}

			*((ushort*) workingP) = 0;

			return new ItemIdList(p);
		}

		// gets whether the p points to a null-terminator
		private static unsafe bool IsTerminator(IntPtr p)
		{
			return *((ushort*)p) == 0;
		}

		private readonly IntPtr p;

		private ItemIdList(IntPtr p)
		{
			if (p == IntPtr.Zero)
			{
				throw new ArgumentNullException("p");
			}

			this.p = p;
		}

		public byte[][] GetItemData()
		{
			ArrayList data = new ArrayList();

			foreach (ItemId item in GetItems())
			{
				data.Add(item.GetData());
			}

			return (byte[][]) data.ToArray(typeof(byte[]));
		}

		public ItemId[] GetItems()
		{
			ArrayList items = new ArrayList();
			IntPtr itemP = p;

			while (!ItemIdList.IsTerminator(itemP))
			{
				ItemId item = ItemId.Create(itemP);
				items.Add(item);
				itemP = (IntPtr) ((int) itemP + ItemId.HeaderSize + item.DataLength);
			}

			return (ItemId[]) items.ToArray(typeof(ItemId));
		}

		public IntPtr Ptr
		{
			get
			{
				return p;
			}
		}

		public int Size
		{
			get
			{
				int size = ItemId.HeaderSize; // terminator
				
				foreach (ItemId item in GetItems())
				{
					size += item.Size;
				}
				
				return size;
			}
		}
	}
}
