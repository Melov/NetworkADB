using System;

namespace WindowsShell.Nspace
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class NsExtensionAttribute : Attribute
	{
		public static NsExtensionAttribute Get(Type type)
		{
			return (NsExtensionAttribute) Attribute.GetCustomAttribute(type, typeof(NsExtensionAttribute));
		}

		public static bool IsDefined(Type type)
		{
			return NsExtensionAttribute.Get(type) != null;
		}

		private FolderAttributes attributes;
		private bool currentUser;
		private int iconIndex = -1;
		private string iconString;
		private string infoTip;
		private string name;
		private NsTarget target;

		public NsExtensionAttribute(NsTarget target, string name, FolderAttributes attributes)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}

			this.target = target;
			this.name = name;
			this.attributes = attributes;
		}

		public FolderAttributes Attributes
		{
			get
			{
				return attributes;
			}
		}

		public bool CurrentUser
		{
			get
			{
				return currentUser;
			}

			set
			{
				currentUser = value;
			}
		}

		public int IconIndex
		{
			get
			{
				return iconIndex;
			}

			set
			{
				iconIndex = value;
			}
		}

		public string IconString
		{
			get
			{
				return iconString;
			}

			set
			{
				iconString = value;
			}
		}

		public string InfoTip
		{
			get
			{
				return infoTip;
			}

			set
			{
				infoTip = value;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
		}

		public NsTarget Target
		{
			get
			{
				return target;
			}
		}
	}
}
