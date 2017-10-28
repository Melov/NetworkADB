using System;
using System.Collections;

namespace WindowsShell.Nspace
{
    [Serializable]
	public class Column
	{
		private IComparer comparer;
		private bool defaultVisible;
		private ColumnFormat fmt;
		private Guid fmtid;
		private string name;
		private int pid;
		private bool slow;
		private Type type;
		private int width;

		public Column(string name, ColumnFormat fmt, int width)
			: this(name, fmt, typeof(string), Guid.Empty, -1, width, true, false)
		{
		}

		public Column(string name, ColumnFormat fmt, Type type, Guid fmtid, int pid, int width, bool defaultVisible, bool slow)
		{
			this.name = name;
			this.fmt = fmt;
			this.type = type;
			this.fmtid = fmtid;
			this.pid = pid;
			this.width = width;
			this.defaultVisible = defaultVisible;
			this.slow = slow;
		}

		public IComparer Comparer
		{
			get
			{
				return comparer;
			}

			set
			{
				comparer = value;
			}
		}

		public ColumnFormat Format
		{
			get
			{
				return fmt;
			}
		}

		public Guid FormatIdentifier
		{
			get
			{
				return fmtid;
			}
		}

		public int PropertyIdentifier
		{
			get
			{
				return pid;
			}
		}
        
		public string Name
		{
			get
			{
				return name;
			}
		}

		public bool DefaultVisible
		{
			get
			{
				return defaultVisible;
			}
		}

		public bool Slow
		{
			get
			{
				return slow;
			}
		}

		public Type Type
		{
			get
			{
				return type;
			}
		}

		public int Width
		{
			get
			{
				return width;
			}
		}
	}
}
