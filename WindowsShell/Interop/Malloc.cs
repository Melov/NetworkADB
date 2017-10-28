using System;
using System.Runtime.InteropServices;

namespace WindowsShell.Interop
{
	[ComVisible(false),
	 CLSCompliant(false)]
	internal class Malloc : IDisposable, IMalloc
	{
		private bool disposed = false;
		private readonly IMalloc m;

		internal Malloc(IntPtr pMalloc)
		{
			m = (IMalloc) Marshal.GetObjectForIUnknown(pMalloc);
		}

		internal Malloc(IMalloc m)
		{
			if (m == null)
			{
				throw new ArgumentNullException();
			}

			this.m = m;
		}

		~Malloc()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (!disposed)
			{
				disposed = true;
				GC.SuppressFinalize(this);

				IntPtr pUnk = Marshal.GetIUnknownForObject(m);
				while (Marshal.ReleaseComObject(m) != 0);
				Marshal.Release(pUnk);
			}
		}

		public IntPtr Alloc(int cb)
		{
			return m.Alloc(cb);
		}

		public IntPtr Realloc(IntPtr pv, int cb)
		{
			return m.Realloc(pv, cb);
		}

		public void Free(IntPtr pv)
		{
			m.Free(pv);
		}

		public int GetSize(IntPtr pv)
		{
			return m.GetSize(pv);
		}

		public int DidAlloc(IntPtr pv)
		{
			return m.DidAlloc(pv);
		}

		public void HeapMinimize()
		{
			m.HeapMinimize();
		}
	}
}
