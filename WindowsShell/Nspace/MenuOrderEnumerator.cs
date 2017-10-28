using System;
using System.Collections;

namespace WindowsShell.Nspace
{
	// An enumerator over shell menu items; filters and reorders the items
	// based on the context menu options
	internal class MenuOrderEnumerator : IEnumerator
	{		
         private readonly bool bNormal;
         private readonly bool bDefaultOnly;
         private readonly bool bVerbsOnly;
         private readonly bool bExplore;
         private readonly bool bNoVerbs;
         private readonly bool bCanRename;
         private readonly bool bNoDefault;
         private readonly bool bIncludeStatic;
         private readonly bool bExtendedVerbs;
         private readonly bool bReserved;

		private int i = -1;
		private readonly ShellMenuItem[] menuItems;

		internal MenuOrderEnumerator(ShellMenuItem[] menuItems, ContextMenuOptions opts)
		{
			if (menuItems == null)
			{
				throw new ArgumentNullException("menuItems");
			}

			this.menuItems = menuItems;

            bNormal = (opts & ContextMenuOptions.Normal) == ContextMenuOptions.Normal;
            bDefaultOnly = (opts & ContextMenuOptions.DefaultOnly) == ContextMenuOptions.DefaultOnly;
            bVerbsOnly = (opts & ContextMenuOptions.VerbsOnly) == ContextMenuOptions.VerbsOnly;
            bExplore = (opts & ContextMenuOptions.Explore) == ContextMenuOptions.Explore;
            bNoVerbs = (opts & ContextMenuOptions.NoVerbs) == ContextMenuOptions.NoVerbs;
            bCanRename = (opts & ContextMenuOptions.CanRename) == ContextMenuOptions.CanRename;
            bNoDefault = (opts & ContextMenuOptions.NoDefault) == ContextMenuOptions.NoDefault;
            bIncludeStatic = (opts & ContextMenuOptions.IncludeStatic) == ContextMenuOptions.IncludeStatic;
            bExtendedVerbs = (opts & ContextMenuOptions.ExtendedVerbs) == ContextMenuOptions.ExtendedVerbs;
            bReserved = (opts & ContextMenuOptions.Reserved) == ContextMenuOptions.Reserved;
		}

		#region IEnumerator Members

		public void Reset()
		{
			i = -1;
		}

		public object Current
		{
			get
			{
				// if the explore and open items should be swapped because the
				// explorer tree view is showing
				if (!bDefaultOnly &&
					HasExploreItem && HasOpenItem &&
					((bExplore && !ExploreBeforeOpen) ||
                     (!bExplore && ExploreBeforeOpen)))
				{
					// swap them

					if (i == ExploreItemIndex)
					{
						return menuItems[OpenItemIndex];
					}
					else if (i == OpenItemIndex)
					{
						return menuItems[ExploreItemIndex];
					}
				}

			    if (!bCanRename && IsRenameItem(menuItems[i]))
			    {
			        menuItems[i].Enabled = false;
			    }
			    if (IsNewFolderItem(menuItems[i]))
			    {
			        menuItems[i].Enabled = !bCanRename;
			    }

                if (bNoDefault && (ItemIsDefault(menuItems[i]) || IsOpenItem(menuItems[i]) || IsCopyItem(menuItems[i]) || IsDeleteItem(menuItems[i])))
			    {
                    menuItems[i].Enabled = false;
			    }
                //link
                if (bVerbsOnly && (IsCopyItem(menuItems[i]) || IsDeleteItem(menuItems[i]) || IsRenameItem(menuItems[i]) || IsPasteItem(menuItems[i])))
			    {
                    menuItems[i].Enabled = false;
			    }
				
                return menuItems[i];
			}
		}

		public bool MoveNext()
		{
			while (++i < menuItems.Length)
			{
				if (Included(menuItems[i]))
				{
					break;
				}
			}

			return i < menuItems.Length;
		}

		#endregion

		// Gets the ID of the current menu item
		internal int CurrentId
		{
			get
			{
				for (int i = 0; i < menuItems.Length; i++)
				{
					if (menuItems[i] == Current)
					{
						return i;
					}
				}

				return -1;
			}
		}

		// Gets whether the menu item in Current should be the default menu
		// item; this overrides the Current.Default property because the
		// default menu item can change depending on whether the 'Explore'
		// option was set by the shell
		internal bool CurrentIsDefault
		{
			get
			{
				return ItemIsDefault((ShellMenuItem) Current);
			}
		}

		// Gets whether the Explore menu item precedes the Open menu item
		// in the natural order of the menu items
		private bool ExploreBeforeOpen
		{
			get
			{
				return ExploreItemIndex < OpenItemIndex;
			}
		}

		// The index of the Explore menu item, or -1 if there is no explore
		// menu item
		private int ExploreItemIndex
		{
			get
			{
				for (int i = 0; i < menuItems.Length; i++)
				{
					if (IsExploreItem(menuItems[i]))
					{
						return i;
					}
				}

				return -1;
			}
		}

		// Gets whether there is an explore menu item in the enumeration
		private bool HasExploreItem
		{
			get
			{
				return ExploreItemIndex != -1;
			}
		}

		// Gets whether there is an open menu item in the enumeration
		private bool HasOpenItem
		{
			get
			{
				return OpenItemIndex != -1;
			}
		}

		// Gets whether the item at the specified index is the default
		// item; the default item can change between the 'Explore' and 'Open'
		// menu items depending on whether the shell set the Explore context
		// menu option
		private bool ItemIsDefault(ShellMenuItem item)
		{
            if (!bExplore && IsOpenItem(item))
			{
				return true;
			}
            else if (bExplore && IsExploreItem(item))
			{
				return true;
			}
			else if (
                (!bExplore && !HasOpenItem) ||
                (bExplore && !HasExploreItem))
			{
				return item.Default;
			}
			else
			{
				return false;
			}
		}

		// Gets whether the menu item at the specified index is included
		// in the context menu
		private bool Included(ShellMenuItem item)
		{
			if (bDefaultOnly)
			{
				return ItemIsDefault(item);
			}

			return true;
		}

		// Gets whether the specified menu item is the 'explore' menu item
		private bool IsExploreItem(ShellMenuItem item)
		{
			return item.Verb == ExploreMenuItem.ExploreVerb;
		}

		// Gets whether the specified menu item is the 'open' menu item
		private bool IsOpenItem(ShellMenuItem item)
		{
			return item.Verb == ExploreMenuItem.OpenVerb;
		}

        private bool IsRenameItem(ShellMenuItem item)
        {
            return item.Verb == ExploreMenuItem.RenameVerb;
        }

        // Gets whether the specified menu item is the 'open' menu item
        private bool IsCopyItem(ShellMenuItem item)
        {
            return item.Verb == ExploreMenuItem.CopyVerb;
        }

        // Gets whether the specified menu item is the 'open' menu item
        private bool IsPasteItem(ShellMenuItem item)
        {
            return item.Verb == ExploreMenuItem.PasteVerb;
        }

        // Gets whether the specified menu item is the 'open' menu item
        private bool IsDeleteItem(ShellMenuItem item)
        {
            return item.Verb == ExploreMenuItem.DeleteVerb;
        }

        private bool IsNewFolderItem(ShellMenuItem item)
        {
            return item.Verb == ExploreMenuItem.NewFolderVerb;
        }

		// The index of the open menu item, or -1 if there is no open menu
		// item
		private int OpenItemIndex
		{
			get
			{
				for (int i = 0; i < menuItems.Length; i++)
				{
					if (IsOpenItem(menuItems[i]))
					{
						return i;
					}
				}

				return -1;
			}
		}
	}
}
