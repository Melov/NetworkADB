using System;

namespace WindowsShell.Interop
{
    //https://www.winehq.org/pipermail/wine-patches/2004-August/012323.html
	// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/shellcc/platform/shell/reference/callbackfunctions/lpfnviewcallback.asp
	public enum ShellFolderViewMessage : int
	{
		AddPropertyPages			= 47,
		BackgroundEnum				= 32,
		BackgroundEnumDone			= 48,
		ColumnClick					= 24,
		DefItemCount				= 26,
		DefViewMode					= 27,
		DidDragDrop					= 36,
		FsNotify					= 14, // From http://groups.google.com/groups?hl=en&lr=&ie=UTF-8&selm=%23DIBurBkCHA.3244%40tkmsftngp10
		GetAnimation				= 68, // From http://groups.google.com/groups?hl=en&lr=&ie=UTF-8&selm=%23DIBurBkCHA.3244%40tkmsftngp10
		GetButtonInfo				=  5,
		GetButtons					=  6,
		GetDetailsOf				= 23,
		GetHelpText					=  3,
		GetHelpTopic				= 63,
		GetNotify					= 49,
		GetPane						= 59,
		GetSortDefaults				= 53,
		GetToolTipText				=  4,
		GetZone						= 58,
		InitMenuPopup				=  7,
		InvokeCommand				=  2,
		MergeMenu					=  1,
		QueryFsNotify				= 25,
		SetIsfv						= 39,
		Size						= 57,
		ThisIdList					= 41,
		UnmergeMenu					= 28,
		UpdateStatusBar				= 31,
		WindowCreated				= 15,
        SFVM_VIEWRELEASE            =  12,
        SFVM_WINDOWCLOSING          =  16

	}
}
