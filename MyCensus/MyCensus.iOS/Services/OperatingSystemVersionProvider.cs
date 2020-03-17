using System;
using MyCensus;
using UIKit;

namespace MyCensus.iOS
{
	public class OperatingSystemVersionProvider : IOperatingSystemVersionProvider
	{
		public string GetOperatingSystemVersionString()
		{
			return UIDevice.CurrentDevice.SystemVersion;
		}
	}
}
