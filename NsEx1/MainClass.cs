using System;
using System.Reflection;
using System.Runtime.InteropServices;
using WindowsShell.Interop;
using WindowsShell.Nspace;

namespace NsEx1
{
    //{00021490-0000-0000-C000-000000000046}
    //4de81366-c7e6-44e8-84f7-372e8bc81a91
	[Guid("4de81366-c7e6-44e8-84f7-372e8bc81a79"),
     NsExtension(NsTarget.MyComputer, "Registry", FolderAttributes.Folder | FolderAttributes.HasSubFolder | FolderAttributes.Browsable | FolderAttributes.CanLink, InfoTip = "Browse the Windows registry.", IconString = @"C:\Windows\Regedit.exe,0")]
    public class MainClass3 : NsExtension
	{
		internal static Assembly CurrentDomain_Resolve(object sender, ResolveEventArgs args)
		{
			return Assembly.GetExecutingAssembly();
		}
        
		public MainClass3() : base(new Root())
		{
		}

	}
}
