using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices;
// FIXME- strong name app block and re add, see also commend below: using Microsoft.ApplicationBlocks.ExceptionManagement;
using WindowsShell.Interop;

namespace WindowsShell.Nspace
{
	internal class EnumIdListImpl : IEnumIDList
	{
		private bool done;
		private EnumOptions opts;
		private IEnumerator ienum;

		internal EnumIdListImpl(EnumOptions opts, IEnumerable ie)
		{
			if (ie == null)
			{
				ienum = new ArrayList(0).GetEnumerator();
			}
			else
			{
				IEnumerator ienum = ie.GetEnumerator();
				this.ienum = ienum == null
					? new ArrayList(0).GetEnumerator()
					: ienum;
			}

			this.opts = opts;
			done = !MoveNext();
		}

		unsafe int IEnumIDList.Next(uint celt, IntPtr[] rgelt, IntPtr pceltFetched)
		{
			Debug.Assert(celt > 0);

			int i = 0;

			/* try
			{ */

			//using (Malloc m = Shell32.GetMalloc())
			{
				if (!done)
				{
					do
					{
						rgelt[i++] = ItemIdList.Create(null, ((IFolderObject) ienum.Current).Persist()).Ptr;
						celt--;
					}
					while (celt > 0 & !(done = !MoveNext()));
				}

				if (pceltFetched != IntPtr.Zero)
				{
					int* fetched = (int*) pceltFetched;
					*fetched = i;
				}

				return done && i == 0 ? 1 : 0;
			}
			
			/* }
			catch (Exception e)
			{
				NameValueCollection info = new NameValueCollection();
				info.Add("done", done.ToString());
				info.Add("opts", Enum.Format(typeof(EnumOptions), opts, "g"));
				ExceptionManager.Publish(e, info);
				throw;
			} */
		}

		void IEnumIDList.Skip(uint celt)
		{
			throw new NotSupportedException();
		}

		void IEnumIDList.Reset()
		{
			throw new NotSupportedException();
		}

		void IEnumIDList.Clone(out IEnumIDList ppenum)
		{
			throw new NotSupportedException();
		}

		private bool Matches(IFolderObject folderObj)
		{
			FolderAttributes attrs = folderObj.Attributes;

			bool hidden = (attrs & FolderAttributes.Hidden) != FolderAttributes.None;
			bool includeHidden = (opts & EnumOptions.IncludeHidden) == EnumOptions.IncludeHidden;

			bool folder = (attrs & FolderAttributes.Folder) != FolderAttributes.None;
			bool includeFolders = (opts & EnumOptions.Folders) == EnumOptions.Folders;
			bool includeNonFolders = (opts & EnumOptions.NonFolders) == EnumOptions.NonFolders;

			bool storage = (attrs & (FolderAttributes.StorageAncestor | FolderAttributes.StorageAncestor)) != FolderAttributes.None;
			bool includeStorage = (opts & EnumOptions.Storage) == EnumOptions.Storage;

			bool share = (attrs & FolderAttributes.Share) != FolderAttributes.None;
			bool includeShares = (opts & EnumOptions.Shareable) == EnumOptions.Shareable;

			return
				(hidden ? includeHidden : true) &&
				(folder ? includeFolders : includeNonFolders) &&
				(storage ? includeStorage : true) &&
				(share ? includeShares : true);
		}

		private bool MoveNext()
		{
			bool hasNext = false;
			
			while (hasNext = ienum.MoveNext())
			{
				if (Matches(ienum.Current as IFolderObject))
				{
					break;
				}
			}

			return hasNext;
		}
	}
}
