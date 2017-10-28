using System;

namespace WindowsShell.Nspace
{
	[Flags]
	public enum FolderAttributes
	{
		None =				0x00000000,
		CanCopy =			0x00000001,	// Objects can be copied    (0x1,)
		CanMove =			0x00000002,	// Objects can be moved     (0x2,)
		CanLink =			0x00000004,	// Objects can be linked    (0x4,)
		Storage =			0x00000008, // supports BindToObject(IID_IStorage)
		CanRename =			0x00000010, // Objects can be renamed
		CanDelete =			0x00000020, // Objects can be deleted
		HasPropSheet =		0x00000040, // Objects have property sheets
		DropTarget =		0x00000100, // Objects are drop target
		CapabilityMask =	0x00000177,
		Encrypted =			0x00002000, // object is encrypted (use alt color)
		IsSlow =			0x00004000, // 'slow' object
		Ghosted =			0x00008000, // ghosted icon
		Link =				0x00010000, // Shortcut (link)
		Share =				0x00020000, // shared
		ReadOnly =			0x00040000, // read-only
		Hidden =			0x00080000, // hidden object
		DisplayAttrMask =	0x000FC000,
		FileSysAncestor =	0x10000000, // may contain children with SFGAO_FILESYSTEM
		Folder =			0x20000000, // support BindToObject(IID_IShellFolder)
		FileSystem =		0x40000000, // is a win32 file system object (file/folder/root)
		HasSubFolder =		unchecked ((int) 0x80000000), // may contain children with SFGAO_FOLDER
		ContentsMask =		unchecked ((int) 0x80000000),
		Validate =			0x01000000, // invalidate cached information
		Removable =			0x02000000, // is this removeable media?
		Compressed =		0x04000000, // Object is compressed (use alt color)
		Browsable =			0x08000000, // supports IShellFolder, but only implements CreateViewObject() (non-folder view)
		NonEnumerated =		0x00100000, // is a non-enumerated object
		NewContent =		0x00200000, // should show bold in explorer tree
		CanMoniker =		0x00400000, // defunct
		HasStorage =		0x00400000, // defunct
		Stream =			0x00400000, // supports BindToObject(IID_IStream)
		StorageAncestor =	0x00800000, // may contain children with SFGAO_STORAGE or SFGAO_STREAM
		StorageCapMask =	0x70C50008	// for determining storage capabilities, ie for open/save semantics
	}
}
