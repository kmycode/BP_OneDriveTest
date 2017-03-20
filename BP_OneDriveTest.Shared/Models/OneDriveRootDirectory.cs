using System;
using System.Collections.Generic;
using System.Text;

namespace BP_OneDriveTest.Shared.Models
{
	/// <summary>
	/// OneDriveのルートディレクトリ
	/// </summary>
    public class OneDriveRootDirectory : OneDriveDirectory
    {
		public OneDriveRootDirectory() : base(null)
		{
			this.Id = null;
			this.Name = "Root";
		}
    }
}
