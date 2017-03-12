using System;
using System.Collections.Generic;
using System.Text;

namespace BP_OneDriveTest.Shared.Common
{
	/// <summary>
	/// ブラウザの開閉を行うオブジェクト
	/// </summary>
    public interface IAuthBrowserProvider
    {
		/// <summary>
		/// ブラウザを開く
		/// </summary>
		/// <returns>開いたブラウザを操作するオブジェクト</returns>
		IAuthBrowser OpenBrowser();

		/// <summary>
		/// ブラウザを閉じる
		/// </summary>
		void CloseBrowser();
    }
}
