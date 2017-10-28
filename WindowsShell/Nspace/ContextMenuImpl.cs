using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WindowsShell.Interop;

namespace WindowsShell.Nspace
{
    public class ContextMenuImpl : IContextMenu
	{
        public void SendCommand(string command)
        {
            
        }

		private readonly ShellMenuItem[] menuItems;	    
		internal ContextMenuImpl(IFolderObject root, params IFolderObject[] fos)
		{
            if (root!=null)
		        root.ContextMenu = this;

			if (fos == null)
			{
				throw new ArgumentNullException("fos");
			}
			else if (fos.Length == 0)
			{
				throw new ArgumentOutOfRangeException("fos.Length", fos.Length, "must be >= 1");
			}
			else if (root == null && fos.Length > 1)
			{
				throw new InvalidOperationException();
			}

			ShellMenuItem[] mis;

			if (fos.Length == 1)
			{
				mis = fos[0].MenuItems;
                fos[0].ContextMenu = this;
			}
			else
			{
				mis = root.GetMenuItems(fos);
			}

			menuItems = (mis == null)
				? new ShellMenuItem[] {}
				: mis;
		}

		int IContextMenu.QueryContextMenu(IntPtr hmenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, ContextMenuOptions uFlags)
		{		    			
            bool bNormal = (uFlags & ContextMenuOptions.Normal) == ContextMenuOptions.Normal;
            bool bDefaultOnly = (uFlags & ContextMenuOptions.DefaultOnly) == ContextMenuOptions.DefaultOnly;
            bool bVerbsOnly = (uFlags & ContextMenuOptions.VerbsOnly) == ContextMenuOptions.VerbsOnly;
            bool bExplore = (uFlags & ContextMenuOptions.Explore) == ContextMenuOptions.Explore;
            bool bNoVerbs = (uFlags & ContextMenuOptions.NoVerbs) == ContextMenuOptions.NoVerbs;
            bool bCanRename = (uFlags & ContextMenuOptions.CanRename) == ContextMenuOptions.CanRename;
            bool bNoDefault = (uFlags & ContextMenuOptions.NoDefault) == ContextMenuOptions.NoDefault;
            bool bIncludeStatic = (uFlags & ContextMenuOptions.IncludeStatic) == ContextMenuOptions.IncludeStatic;
            bool bExtendedVerbs = (uFlags & ContextMenuOptions.ExtendedVerbs) == ContextMenuOptions.ExtendedVerbs;
            bool bReserved = (uFlags & ContextMenuOptions.Reserved) == ContextMenuOptions.Reserved;

            Debug.WriteLine(string.Format("QueryContextMenu: \r\nNormal:{0}\r\nDefaultOnly:{1}\r\nbVerbsOnly:{2}\r\nExplore:{3}\r\nNoVerbs:{4}\r\nCanRename:{5}\r\nNoDefault:{6}\r\nIncludeStatic:{7}\r\nExtendedVerbs:{8}\r\nReserved:{9}", bNormal, bDefaultOnly, bVerbsOnly, bExplore, bNoVerbs, bCanRename, bNoDefault, bIncludeStatic, bExtendedVerbs, bReserved));

			MenuOrderEnumerator m = new MenuOrderEnumerator(menuItems, uFlags);

            
                int items = User32.GetMenuItemCount(hmenu);
                if (items == 8 || items == 9)
                {
                    for (uint i = 5; i < items; i++)
                    {
                        User32.RemoveMenu(hmenu, 5, User32.MF_BYPOSITION);                       
                    }
                }
            		

			while (m.MoveNext())
			{
                ((ShellMenuItem)m.Current).CreateMenuItem(hmenu, indexMenu++, (int)idCmdFirst + m.CurrentId, m.CurrentIsDefault & !bNoDefault, false);
			}

			return menuItems.Length;
		}

		void IContextMenu.InvokeCommand(ref CommandInfo lpici)
		{
		    int id = GetCommandId(lpici);
		    if (id < menuItems.Length)
		    {
                menuItems[GetCommandId(lpici)].PerformClick(Win32Window.Create(lpici.hwnd), lpici);   
		    }			
		}

		void IContextMenu.GetCommandString(int idCmd, CommandStringOptions uFlags, IntPtr pwReserved, byte[] pszName, uint cchMax)
		{
		    if (idCmd > menuItems.Length - 1)
		    {
		        idCmd = 0;
		        //return;
		    }
		        

			ShellMenuItem menuItem = menuItems[idCmd];
			string text;

			switch (uFlags)
			{
				case CommandStringOptions.HelpTextA:
				case CommandStringOptions.HelpTextW:
					text = menuItem.HelpText;
					break;

				case CommandStringOptions.VerbA:
				case CommandStringOptions.VerbW:
					text = menuItem.Verb;
					break;

				case CommandStringOptions.ValidateA:
				case CommandStringOptions.ValidateW:
					if (idCmd < 0 || idCmd >= menuItems.Length)
					{
						Marshal.ThrowExceptionForHR(1); // S_FALSE
					}
					throw new Exception(); // unreachable

				default:
					throw new ArgumentOutOfRangeException("uFlags", uFlags.ToString());
			}

			if (text == null)
			{
				text = string.Empty;
			}

			byte[] buf;

			if ((uFlags & CommandStringOptions.Unicode) == CommandStringOptions.Unicode)
			{
				buf = Encoding.Unicode.GetBytes(text);
			}
			else
			{
				buf = Encoding.ASCII.GetBytes(text);
			}

			int cch = Math.Min(buf.Length, pszName.Length - 1);

			if (cch > 0)
			{
				Array.Copy(buf, 0, pszName, 0, cch);
			}
			else
			{
				// null terminate the buffer
				pszName[0] = 0;
			}
		}

		private int GetCommandId(CommandInfo ci)
		{
			if (User32.IsIntResource(ci.lpVerb))
			{
				int id = (int) ci.lpVerb;

				if (id < 0 || id > menuItems.Length)
				{
					throw new InvalidOperationException("ID " + id + "out of range");
				}
				else
				{
					return id;
				}
			}
			else
			{
				string verb = Marshal.PtrToStringAnsi(ci.lpVerb);

				for (int id = 0; id < menuItems.Length; id++)
				{
					if (menuItems[id].Verb == verb)
					{
						return id;
					}
				}
			}

			throw new InvalidOperationException("command ID not found");
		}
	}
}
