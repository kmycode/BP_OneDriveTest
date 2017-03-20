using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace BP_OneDriveTest.Shared.Models
{
    public class OneDriveFile : OneDriveObject
    {
		public OneDriveFile(Item item) : base(item)
		{
		}
    }
}
