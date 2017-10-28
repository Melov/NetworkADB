using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WindowsShell.Interop;

namespace WindowsShell.Nspace
{
	// FIXME: This type should notify the system of changes to the menu
	public class ShellMenuItem
	{
		private const int ID_NOT_ASSIGNED = -1;

		public static ShellMenuItem CreateSeparator()
		{
			return new ShellMenuItem(null, false, false, true, null, false, true, null, null);
		}

		private Bitmap bitmap;
		private bool @checked;
		private bool @default;
		private bool enabled;
		private string helpText;
		private bool radioCheck;
		private bool separator;
		private string text;
		private string verb;

		public ShellMenuItem() : this(null, false, false, true, null, false, false, null, null) {}

		public ShellMenuItem(string text, string verb) : this(null, false, false, true, null, false, false, text, verb) {}

		public ShellMenuItem(string text, string verb, EventHandler onClick) : this(null, false, false, true, null, false, false, text, verb)
		{
			Click += onClick;
		}

		protected ShellMenuItem(Bitmap bitmap, bool @checked, bool @default, bool enabled, string helpText, bool radioCheck, bool separator, string text, string verb)
		{
			this.bitmap = bitmap;
			this.@checked = @checked;
			this.@default = @default;
			this.enabled = enabled;
			this.helpText = helpText;
			this.radioCheck = radioCheck;
			this.separator = separator;
			this.text = text;
			this.verb = verb;
		}

		public Bitmap Bitmap
		{
			get
			{
				return bitmap;
			}

			set
			{
				bitmap = value;
			}
		}

		public bool Checked
		{
			get
			{
				return @checked;
			}

			set
			{
				@checked = value;
			}
		}

		public event EventHandler Click;

		public bool Default
		{
			get
			{
				return @default;
			}

			set
			{
				@default = value;
			}
		}

		public bool Enabled
		{
			get
			{
				return enabled;
			}

			set
			{
				enabled = value;
			}
		}

		public string HelpText
		{
			get
			{
				return helpText;
			}

			set
			{
				helpText = value;
			}
		}

		public bool RadioCheck
		{
			get
			{
				return radioCheck;
			}

			set
			{
				radioCheck = value;
			}
		}

		public bool Separator
		{
			get
			{
				return separator;
			}

			set
			{
				separator = value;
			}
		}

		public string Text
		{
			get
			{
				return text;
			}

			set
			{
				text = value;
			}
		}

		public string Verb
		{
			get
			{
				return verb;
			}

			set
			{
				verb = value;
			}
		}

		internal void CreateMenuItem(IntPtr hMenu, uint index, int id, bool @isDefault, bool isInternal)
		{
            /*
		    if (!isInternal)
		    {
                int items = User32.GetMenuItemCount(hMenu);
                if (items == 8)
                {
                    for (uint i = 5; i < items; i++)
                    {
                        User32.RemoveMenu(hMenu, 5, User32.MF_BYPOSITION);
                        
                        uint itmId = User32.GetMenuItemID(hMenu, i);
                        MenuItemInfo mCurr = new MenuItemInfo();
                        //User32.GetMenuItemInfo(hMenu, (uint)i, true, ref mCurr);
                        uint MIIM_STRING = 0x00000040;
                        uint MFT_STRING = 0x00000000;
                        mCurr.cbSize = (uint)Marshal.SizeOf(typeof(MenuItemInfo));
                        mCurr.fMask = MenuItemInfoOptions.String | MenuItemInfoOptions.Id | MenuItemInfoOptions.State;
                        mCurr.fType = MenuItemTypes.String;
                        mCurr.dwTypeData = null;

                        bool res = User32.GetMenuItemInfo(hMenu, (uint)i, true, ref mCurr); //To load cch into memory
                        if (res)
                        {
                            mCurr.cch += 1;
                            //Set the length of the buffer to cch + 1
                            mCurr.dwTypeData = new string(' ', (int)(mCurr.cch));
                            res = User32.GetMenuItemInfo(hMenu, (uint)i, true, ref mCurr); //To fill dwTypeData

                        }

                        User32.EnableMenuItem(hMenu, (uint)i, User32.MF_BYPOSITION );
                        
                    }
                }   
		    }		    
		    */

			// create a MenuItemInfo describing this item
			
			MenuItemInfo mii = new MenuItemInfo();
			
			

			mii.fMask =
				(Bitmap != null ? MenuItemInfoOptions.CheckMarks : MenuItemInfoOptions.None) |
				(RadioCheck || Separator ? MenuItemInfoOptions.FType : MenuItemInfoOptions.None) |
				MenuItemInfoOptions.Id |
				MenuItemInfoOptions.State |
				MenuItemInfoOptions.String;

			if (RadioCheck)
			{
				mii.fType = MenuItemTypes.RadioCheck;
			}
			else if (Separator)
			{
				mii.fType = MenuItemTypes.Separator;
			}
            else if (Bitmap != null)
            {
                mii.fType = MenuItemTypes.String;
            }

			mii.fState =                 
				(Checked ? MenuItemStates.Checked : MenuItemStates.Unchecked) |
				(Enabled ? MenuItemStates.Enabled : MenuItemStates.Disabled) |
				(isDefault ? MenuItemStates.Default : MenuItemStates.None);

		    

			mii.wID = checked ((uint) id);

			mii.dwTypeData = text;

			mii.cch = (uint) text.Length;

			mii.hbmpChecked = Bitmap == null ? IntPtr.Zero : Bitmap.GetHbitmap();
            mii.hbmpUnchecked = Bitmap == null ? IntPtr.Zero : Bitmap.GetHbitmap();            

            mii.cbSize = checked((uint)Marshal.SizeOf(mii));

			// add the menu item

            if (Enabled)
			    User32.InsertMenuItem(hMenu, index, true, ref mii);
			
			// FIXME: Invoke DrawMenuItem?
		}

		internal void PerformClick(IWin32Window owner, CommandInfo lpici)
		{
		    if (Click != null) Click(owner, new ContextMenuEventArgs(lpici));
		}
	}
}
