using System;
using System.Collections;

namespace WindowsShell.Nspace
{
    [Serializable]
	public class ColumnCollection : IEnumerable
	{
		private IList items;
		private Column defaultDisplayColumn;
		private Column defaultSearchColumn;

		public ColumnCollection()
		{
			items = new ArrayList();
		}

		public void Add(Column column)
		{
			if (column == null)
			{
				throw new ArgumentNullException("column");
			}
			else if (items.Contains(column))
			{
				throw new InvalidOperationException("Column already in collection");
			}

			items.Add(column);
		}

		public int Count
		{
			get
			{
				return items.Count;
			}
		}

		public Column DefaultDisplayColumn
		{
			get
			{
				return defaultDisplayColumn;
			}

			set
			{
				defaultDisplayColumn = value;
			}
		}

		public Column DefaultSortColumn
		{
			get
			{
				return defaultSearchColumn;
			}

			set
			{
				defaultSearchColumn = value;
			}
		}

		public Column this[int index]
		{
			get
			{
				return (Column) items[index];
			}
		}

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return items.GetEnumerator();
		}

		#endregion
	}
}
