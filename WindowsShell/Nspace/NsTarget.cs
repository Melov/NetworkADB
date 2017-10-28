using System;

namespace WindowsShell.Nspace
{
	public enum NsTarget
	{
		None,

		[TargetName("MyComputer")]
		MyComputer,

		[TargetName("Desktop")]
		Desktop,

		[TargetName("ControlPanel")]
		ControlPanel,

		[TargetName("NetworkNeighborhood")]
		NetworkNeighborhood,

		[TargetName(@"NetworkNeighborhood\EntireNetwork")]
		NetworkNeighborhoodEntireNetwork,

		[TargetName("RemoteComputer")]
		RemoteComputer
	}
}
