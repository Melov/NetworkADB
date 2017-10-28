using System;
using System.Runtime.InteropServices;
using WindowsShell.Nspace;

namespace NsEx2
{
	[Guid("6a125063-a28d-4690-a433-6a88ebe1bded"),
    NsExtension(NsTarget.MyComputer, "Shell Icons", FolderAttributes.Folder | FolderAttributes.HasSubFolder | FolderAttributes.CanLink | FolderAttributes.DropTarget)]
	public class MainClass : NsExtension
	{
		public MainClass() : base(new Root())
		{
		}
	}
}
