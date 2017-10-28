using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[ComVisible(false)]
	public sealed class ItemId
	{
		public static unsafe ItemId Create(IntPtr p)
		{
			return p == IntPtr.Zero ? null : new ItemId(p);
		}

		internal static int HeaderSize = Marshal.SizeOf(typeof(ushort));

		private readonly IntPtr p;

		private ItemId(IntPtr p)
		{
			if (p == IntPtr.Zero)
			{
				throw new ArgumentNullException("p");
			}

			this.p = p;
		}

		public unsafe int DataLength
		{
			get
			{
				if (Ptr == IntPtr.Zero)
				{
					throw new NullReferenceException();
				}

				return *((ushort*)Ptr) - ItemId.HeaderSize;
			}
		}

		public byte[] GetData()
		{
			byte[] buf = new byte[DataLength];
			Marshal.Copy(DataPtr, buf, 0, DataLength);
			return buf;
		}

		public IntPtr Ptr
		{
			get
			{
				return p;
			}
		}

		public unsafe int Size
		{
			get
			{
				int len = *(ushort*)p;

				return len == 0
					? ItemId.HeaderSize	// null terminator; shouldn't happen
					: len;
			}
		}

		// pointer to the raw data after the "count bytes" header
		private IntPtr DataPtr
		{
			get
			{
				return (IntPtr) ((uint) p + Marshal.SizeOf(typeof(ushort)));
			}
		}
	}
}
