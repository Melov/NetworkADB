using System;
using System.Reflection;

namespace WindowsShell.Nspace
{
	[AttributeUsage(AttributeTargets.Field)]
	internal class TargetNameAttribute : Attribute
	{
		internal static string GetName(NsTarget t)
		{
			FieldInfo f = typeof(NsTarget).GetField(Enum.GetName(typeof(NsTarget), t));
			TargetNameAttribute n = (TargetNameAttribute) Attribute.GetCustomAttribute(f, typeof(TargetNameAttribute));
			return n == null ? null : n.Value;
		}

		private readonly string name;

		internal TargetNameAttribute(string name)
		{
			this.name = name;
		}

		internal string Value
		{
			get
			{
				return name;
			}
		}
	}
}
