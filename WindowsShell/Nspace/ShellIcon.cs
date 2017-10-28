using System;
using System.Drawing;
using WindowsShell.Interop;

namespace WindowsShell.Nspace
{
	public abstract class ShellIcon
	{
		public static ShellIcon CreateFromFile(string filename, int index, bool cache, bool simulateDoc)
		{
			ShellIcon icon = new FromFile(filename, index);
			icon.dontCache = !cache;
			icon.simulateDoc = simulateDoc;
			return icon;
		}

        public static ShellIcon CreateFromIcon(System.Drawing.Icon icon, bool cache, bool simulateDoc, string extension = null)
		{
            ShellIcon si = new FromIcon(icon, extension);
			si.dontCache = !cache;
			si.simulateDoc = simulateDoc;
			return si;
		}

		private bool dontCache;
		private bool simulateDoc;

		internal virtual ExtractIconFlags Flags
		{
			get
			{
				return
					(dontCache
						? ExtractIconFlags.DontCache
						: ExtractIconFlags.None) |
					(simulateDoc
						? ExtractIconFlags.SimulateDoc
						: ExtractIconFlags.None);
			}
		}

		internal sealed class FromFile : ShellIcon
		{
			private readonly string filename;
			private readonly int index;

			internal FromFile(string filename, int index)
			{
				if (filename == null)
				{
					throw new ArgumentNullException("filename");
				}

				this.filename = filename;
				this.index = index;
			}

			internal string Filename
			{
				get
				{
					return filename;
				}
			}
			
			internal int Index
			{
				get
				{
					return index;
				}
			}
		}

		internal sealed class FromIcon : ShellIcon
		{
            private readonly System.Drawing.Icon icon;
		    private readonly string extension;

            internal FromIcon(System.Drawing.Icon icon, string extension = null)
			{
				this.icon = icon;
                this.extension = extension;
			}

            internal System.Drawing.Icon Icon
			{
				get
				{
					return icon;
				}
			}

            internal string Extension
            {
                get
                {
                    return extension;
                }
            }

			internal override ExtractIconFlags Flags
			{
				get
				{
					return base.Flags | ExtractIconFlags.NotFilename;
				}
			}
		}
	}
}
