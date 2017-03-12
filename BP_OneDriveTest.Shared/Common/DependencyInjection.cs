using System;
using System.Collections.Generic;
using System.Text;

namespace BP_OneDriveTest.Shared.Common
{
    public static class DependencyInjection
    {
		/// <summary>
		/// 認証を行うためのブラウザを操作するオブジェクト
		/// </summary>
		public static IAuthBrowserProvider AuthBrowserProvider { get; set; }
    }
}
