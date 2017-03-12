using System;
using System.Collections.Generic;
using System.Text;

namespace BP_OneDriveTest.Shared.Common
{
	/// <summary>
	/// 認証を行うブラウザ
	/// </summary>
    public interface IAuthBrowser
    {
		/// <summary>
		/// 指定されたURLへ移動する
		/// </summary>
		/// <param name="url">URL</param>
		void GoToUrl(string url);

		/// <summary>
		/// ブラウザのURLが変更された時に発行されるイベント
		/// </summary>
		event AuthBrowserNavigatedEventHandler Navigated;
    }

	/// <summary>
	/// ブラウザのURLが変更された時のイベントデリゲート
	/// </summary>
	/// <param name="sender">送信者</param>
	/// <param name="e">イベント</param>
	public delegate void AuthBrowserNavigatedEventHandler(object sender, AuthBrowserNavigatedEventArgs e);

	/// <summary>
	/// ブラウザのURLが変更された時のイベントパラメータ
	/// </summary>
	public class AuthBrowserNavigatedEventArgs : EventArgs
	{
		/// <summary>
		/// 移動先のURL
		/// </summary>
		public string Url { get; }

		/// <summary>
		/// ここで認証を終了してもいいかどうか
		/// </summary>
		public bool IsExitAuth { get; set; }

		public AuthBrowserNavigatedEventArgs(string url)
		{
			this.Url = url;
		}
	}
}
