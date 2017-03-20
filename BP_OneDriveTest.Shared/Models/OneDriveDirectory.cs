using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace BP_OneDriveTest.Shared.Models
{
	/// <summary>
	/// OneDrive上のディレクトリ
	/// </summary>
    public class OneDriveDirectory : OneDriveObject
    {
		public OneDriveDirectory(Item item) : base(item)
		{
		}
    }
}
