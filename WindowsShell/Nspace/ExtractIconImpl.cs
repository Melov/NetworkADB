using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using WindowsShell.Interop;
using WindowsShell.Nspace.Icon;

namespace WindowsShell.Nspace
{
    internal class ExtractIconImpl : StandardOleMarshalObject, IExtractIcon
	{
		private readonly IFolderObject folderObj;
		private ShellIcon icon;

        /*
        public  IImageList list_SHIL_SMALL;
        public  IImageList list_SHIL_LARGE;
        public  IImageList list_SHIL_EXTRALARGE;
        public  IImageList list_SHIL_JUMBO;

        public System.Drawing.Icon icon_s;
        public System.Drawing.Icon icon_l;
        public System.Drawing.Icon icon_e;
        public System.Drawing.Icon icon_j;
        */
		internal ExtractIconImpl(IFolderObject folderObj)
		{
			if (folderObj == null)
			{
				throw new ArgumentNullException("folderObj");
			}

			this.folderObj = folderObj;            
		}

		int IExtractIcon.GetIconLocation(ExtractIconOptions uFlags, IntPtr szIconFile, uint cchMax, out int piIndex, out ExtractIconFlags pwFlags)
		{
			piIndex = -1;
			pwFlags = ExtractIconFlags.None;

			icon = folderObj.GetIcon((uFlags & ExtractIconOptions.OpenIcon) == ExtractIconOptions.OpenIcon);

			if (icon is ShellIcon.FromFile)
			{
				string filename = (icon as ShellIcon.FromFile).Filename;
				byte[] data = new byte[cchMax * 2];
				Encoding.Unicode.GetBytes(filename, 0, filename.Length, data, 0);
				Marshal.Copy(data, 0, szIconFile, data.Length);

				piIndex = (icon as ShellIcon.FromFile).Index;
			    pwFlags = icon.Flags;
				return 0;
			}
			else if (icon is ShellIcon.FromIcon)
			{
                pwFlags = ExtractIconFlags.NotFilename | ExtractIconFlags.DontCache;//icon.Flags;
				return 0;
			}
			else if (icon == null)
			{
				return 1;	// use default icon
			}

			Trace.Fail("Unreachable code");
			return 0;
		}

        int IExtractIcon.Extract(string pszFile, uint nIconIndex, out System.Drawing.Icon phiconLarge, out System.Drawing.Icon phiconSmall, uint nIconSize)
		{
			phiconLarge = null;
			phiconSmall = null;

			if (icon is ShellIcon.FromFile)
			{
				return 1;	// caller extracts icon from file
			}
			else if (icon is ShellIcon.FromIcon)
			{
			   // int index = IconHelper.GetIconIndex(((ShellIcon.FromIcon) icon).Extension);

				int largeSize = (int) (nIconSize & 0x0000FFFF);
				int smallSize = (int) ((nIconSize & 0xFFFF0000) >> 16);

                Debug.WriteLine(string.Format("ICON: {0} {1}", smallSize, largeSize));

			    if (folderObj.Icons != null)
			    {
                    bool bLink = ((folderObj.Attributes & FolderAttributes.Link) == FolderAttributes.Link);

			        if (!bLink)
			        {
                        phiconSmall = (System.Drawing.Icon)folderObj.Icons[smallSize].Clone();
                        phiconLarge = (System.Drawing.Icon)folderObj.Icons[largeSize].Clone();
                        
			        }else
                    {
                        phiconSmall = IconHelper.AddIconOverlay((System.Drawing.Icon)folderObj.Icons[smallSize], (System.Drawing.Icon)folderObj.Icons[1000 + smallSize].Clone());
                        phiconLarge = IconHelper.AddIconOverlay((System.Drawing.Icon)folderObj.Icons[largeSize], (System.Drawing.Icon)folderObj.Icons[1000 + largeSize].Clone());
                    }
			    }

                /*
			    if (smallSize == 16)
			        phiconSmall = icon_s;
                if (smallSize == 32)
                    phiconSmall = icon_l;
                if (smallSize == 48)
                    phiconSmall = icon_e;
                if (smallSize == 256)
                    phiconSmall = icon_j;

                if (largeSize == 16)
                    phiconLarge = icon_s;
                if (largeSize == 32)
                    phiconLarge = icon_l;
                if (largeSize == 48)
                    phiconLarge = icon_e;
                if (largeSize == 256)
                    phiconLarge = icon_j;
                */
				return 0;
			}
			
			Trace.Fail("Unreachable code");
			return 0;
		}
	}
}
