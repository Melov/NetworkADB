using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WindowsShell.Nspace
{
	internal class NsExtensionRegistrar
	{
		private readonly NsExtensionAttribute config;
		private readonly Type type;

		internal NsExtensionRegistrar(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			else if (!NsExtensionAttribute.IsDefined(type))
			{
				throw new ArgumentOutOfRangeException("type", type, "must have NsExtensionAttribute");
			}

			this.type = type;
			this.config = NsExtensionAttribute.Get(type);
		}

		internal void Register()
		{           
			AddClsidAnnotations();            
			AddToTarget();            
			AddToApprovedShellExtensions();

		    AddContextMenuExts();
		    AddDragDropExts();
		}

		internal void Unregister()
		{
			RemoveClsidAnnotations();
			RemoveFromTarget();
			RemoveFromApprovedShellExtensions();

            RemoveContextMenuExts();
            RemoveDragDropExts();
		}

	    private void AddContextMenuExts()
	    {
            AddContextMenuExt("*");
            AddContextMenuExt("Directory");
            AddContextMenuExt("Drive");
            AddContextMenuExt("Folder");
	    }

        private void RemoveContextMenuExts()
        {
            RemoveContextMenuExt("*");
            RemoveContextMenuExt("Directory");
            RemoveContextMenuExt("Drive");
            RemoveContextMenuExt("Folder");
        }

        private void AddDragDropExts()
        {
            AddDragDropExt("*");
            AddDragDropExt("Directory");
            AddDragDropExt("Drive");
            AddDragDropExt("Folder");
        }

        private void RemoveDragDropExts()
        {
            RemoveDragDropExt("*");
            RemoveDragDropExt("Directory");
            RemoveDragDropExt("Drive");
            RemoveDragDropExt("Folder");
        }

        private void AddContextMenuExt(string sShellObject)
	    {
            if (sShellObject.Equals("Directory"))
            {
                RegistryKey k = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32).OpenSubKey(string.Format(CultureInfo.InvariantCulture, @"{0}\Background\shellex\ContextMenuHandlers\", sShellObject), true);
                if (k != null)
                {
                    using (RegistryKey subkey = k.CreateSubKey(TypeGuid))
                    {
                        subkey.SetValue(null, TypeGuid);
                    }
                }
            }

            RegistryKey k1 = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32).OpenSubKey(string.Format(CultureInfo.InvariantCulture, @"{0}\shellex\ContextMenuHandlers\", sShellObject), true);
            if (k1 != null)
            {
                using (RegistryKey subkey = k1.CreateSubKey(TypeGuid))
                {
                    subkey.SetValue(null, TypeGuid);
                }
            }
	    }

        private void RemoveContextMenuExt(string sShellObject)
        {            
            if (sShellObject.Equals("Directory"))
            {
                RegistryKey k = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32).OpenSubKey(string.Format(CultureInfo.InvariantCulture, @"{0}\Background\shellex\ContextMenuHandlers\", sShellObject), true);
                if (k != null)
                {
                    k.DeleteSubKey(TypeGuid, false);
                }   
            }            

            RegistryKey k1 = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32).OpenSubKey(string.Format(CultureInfo.InvariantCulture, @"{0}\shellex\ContextMenuHandlers\", sShellObject), true);
            if (k1 != null)
            {
                k1.DeleteSubKey(TypeGuid, false);
            }
        }

        private void AddDragDropExt(string sShellObject)
	    {
            RegistryKey k = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32).OpenSubKey(string.Format(CultureInfo.InvariantCulture, @"{0}\shellex\DragDropHandlers\",sShellObject), true);
            if (k != null)
            {
                using (RegistryKey subkey = k.CreateSubKey(TypeGuid))
                {
                    subkey.SetValue(null, TypeGuid);
                }
            }
	    }

        private void RemoveDragDropExt(string sShellObject)
	    {
            RegistryKey k = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32).OpenSubKey(string.Format(CultureInfo.InvariantCulture, @"{0}\shellex\DragDropHandlers\", sShellObject), true);
            if (k != null)
            {
                k.DeleteSubKey(TypeGuid, false);
            }
	    }

		private void AddClsidAnnotations()
		{
			using (RegistryKey key = OpenClsid())
			{
                //
			    try
			    {
                    key.SetValue(null, Config.Name);
			    }
			    catch (Exception e)
			    {
                    MessageBox.Show(e.Message);    
			    }
				
                
				if (Config.InfoTip != null)
				{
                    
					key.SetValue("InfoTip", Config.InfoTip);
                    
				}
                
				using (RegistryKey subkey = key.CreateSubKey("ShellFolder"))
				{
					subkey.SetValue("Attributes", BitConverter.GetBytes((int) Config.Attributes));
				}

				if (HasIcon)
				{
					using (RegistryKey subkey = key.CreateSubKey("DefaultIcon"))
					{
						subkey.SetValue(null, IconString);
					}
				}
               
			}
		}

		private void AddToApprovedShellExtensions()
		{
			using (RegistryKey key = OpenApprovedShellExtensions())
			{
				key.SetValue(TypeGuid, string.Format(CultureInfo.InvariantCulture, "{0}", Config.Name));
			}
		}

		private void AddToTarget()
		{
			if (!HasTarget)
			{
				return;
			}

			using (RegistryKey key = OpenTarget())
			{
				using (RegistryKey subkey = key.CreateSubKey(TypeGuid))
				{
					subkey.SetValue(null, Config.Name);
				}
			}
		}

		private NsExtensionAttribute Config
		{
			get
			{
				return config;
			}
		}

		private bool HasIcon
		{
			get
			{
				return Config.IconIndex >= 0 || Config.IconString != null;
			}
		}

		private bool HasTarget
		{
			get
			{
				return Config.Target != NsTarget.None;
			}
		}

		private string IconString
		{
			get
			{
			    string sret = Config.IconString;
                if(Config.IconIndex >= 0)
                {
                    sret = type.Assembly.CodeBase.Replace("file:///", "").Replace("/", "\\");
                    sret = string.Format("{0},{1}", sret, Config.IconIndex);
                }					
			    return sret;
			}
		}

        private RegistryKey OpenApprovedShellExtensions()
		{
            return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32).OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", true);
		}

		private RegistryKey OpenClsid()
		{
            
		    try
		    {                
                RegistryKey k = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot,
                Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32).OpenSubKey(string.Format(CultureInfo.InvariantCulture, @"CLSID\{0}", TypeGuid), true);
                if (k!=null)
		        return k;                
		    }
		    catch (Exception)
		    {
               
		    }
            return RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot,
                Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32).CreateSubKey(string.Format(CultureInfo.InvariantCulture, @"CLSID\{0}", TypeGuid));
		}

		private RegistryKey OpenTarget()
		{
			if (Config.Target == NsTarget.None)
			{
				throw new InvalidOperationException("No target namespace");
			}

            return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32).OpenSubKey(string.Format(CultureInfo.InvariantCulture, @"Software\Microsoft\Windows\CurrentVersion\Explorer\{0}\Namespace", TargetNameAttribute.GetName(Config.Target)), true);		    
		}

		private void RemoveClsidAnnotations()
		{
			using (RegistryKey key = OpenClsid())
			{
				key.DeleteValue("InfoTip", false);
				key.DeleteSubKey("ShellFolder", false);
				key.DeleteSubKey("DefaultIcon", false);
			}
		}

		private void RemoveFromApprovedShellExtensions()
		{
			using (RegistryKey key = OpenApprovedShellExtensions())
			{
				key.DeleteValue(TypeGuid, false);
			}
		}

		private void RemoveFromTarget()
		{
			if (!HasTarget)
			{
				return;
			}

			using (RegistryKey key = OpenTarget())
			{
				key.DeleteSubKey(TypeGuid);
			}
		}

		private string TypeGuid
		{
			get
			{
				GuidAttribute g = (GuidAttribute) Attribute.GetCustomAttribute(type, typeof(GuidAttribute));
				return string.Format("{{{0}}}", g.Value);
			}
		}
	}
}
