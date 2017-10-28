using System;

namespace WindowsShell.Interop
{
	[Flags]
	public enum EnumOptions
	{
		Folders             = 0x0020,   // only want folders enumerated (SFGAO_FOLDER)
		NonFolders          = 0x0040,   // include non folders
		IncludeHidden       = 0x0080,   // show items normally hidden
		InitOnFirstNext     = 0x0100,   // allow EnumObject() to return before validating enum
		NetPrinterSrch      = 0x0200,   // hint that client is looking for printers
		Shareable           = 0x0400,   // hint that client is looking sharable resources (remote shares)
		Storage             = 0x0800    // include all items with accessible storage and their ancestors
	}

    public enum SHCONT : ushort
    {
        SHCONTF_CHECKING_FOR_CHILDREN = 0x0010,
        SHCONTF_FOLDERS = 0x0020,
        SHCONTF_NONFOLDERS = 0x0040,
        SHCONTF_INCLUDEHIDDEN = 0x0080,
        SHCONTF_INIT_ON_FIRST_NEXT = 0x0100,
        SHCONTF_NETPRINTERSRCH = 0x0200,
        SHCONTF_SHAREABLE = 0x0400,
        SHCONTF_STORAGE = 0x0800,
        SHCONTF_NAVIGATION_ENUM = 0x1000,
        SHCONTF_FASTITEMS = 0x2000,
        SHCONTF_FLATLIST = 0x4000,
        SHCONTF_ENABLE_ASYNC = 0x8000
    }
}
