using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using WindowsShell.Nspace;

namespace NsEx3
{
    //{00021490-0000-0000-C000-000000000046}
	[Guid("57852db1-0d17-4e7f-b381-35627bed7eac"),
    NsExtension(NsTarget.MyComputer, "Network ADB", FolderAttributes.Folder | FolderAttributes.HasSubFolder | FolderAttributes.Browsable | FolderAttributes.CanLink | FolderAttributes.DropTarget,
        InfoTip = "Network Android Debug Bridge", IconIndex = 0)]
	public class MainClass : NsExtension
	{
		public MainClass() : base(new Root())
		{

		}
	}
    /*
    [ComVisible(true)]
    [DisplayName("Copy Path Data Handler")]
    [COMServerAssociation(AssociationType.Directory)]
    [COMServerAssociation(AssociationType.AllFiles)]
    public class CopyPathDataHandler
    {
        
    }
     */
}
