using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BP_OneDriveTest.Shared.Common;
using Microsoft.Graph;

namespace BP_OneDriveTest.Shared.Models
{
	/// <summary>
	/// OneDriveの認証を行うモデル
	/// </summary>
	class OneDriveAuthModel : IAuthenticationProvider
	{
		#region 変数

		/// <summary>
		/// One Drive APIのアプリケーションID
		/// </summary>
		private string apiKey => ApiKeys.OneDriveAPIKey;

		/// <summary>
		/// OneDrive APIのアクセストークン
		/// </summary>
		private string accessToken;

		/// <summary>
		/// トークンの種類
		/// </summary>
		private string tokenType;

		#endregion

		#region メソッド

		/// <summary>
		/// 認証のためにブラウザにアクセスするURLを作成する
		/// </summary>
		/// <returns>URL</returns>
		public string BuildAuthUrl()
		{
			return Uri.EscapeUriString(
				"https://login.live.com/oauth20_authorize.srf" +
				"?pretty=false" +
				"&client_id=" + this.apiKey +
				"&scope=" + "wl.basic wl.signin wl.skydrive wl.skydrive_update" +
				"&response_type=token" +
				"&redirect_uri=https://login.live.com/oauth20_desktop.srf");
		}

		/// <summary>
		/// 認証ブラウザのURLが変更された時
		/// </summary>
		/// <param name="sender">送信者</param>
		/// <param name="e">イベントパラメータ</param>
		public void OnAuthBrowserNavigated(object sender, AuthBrowserNavigatedEventArgs e)
		{
			var parameters = OneDriveUtil.ParseUrlParameters(e.Url);
			if (parameters.ContainsKey("access_token"))
			{
				this.accessToken = parameters["access_token"];
				this.tokenType = parameters["token_type"];
				e.IsExitAuth = true;
				this.OneDriveAuthFinished?.Invoke(this, new OneDriveAuthFinishedEventArgs(this));
			}
		}

		/// <summary>
		/// HTTPリクエストヘッダに、認証情報を追加する
		/// </summary>
		/// <param name="request">HTTPリクエスト</param>
		/// <returns>async</returns>
		public async Task AuthenticateRequestAsync(HttpRequestMessage request)
		{
			request.Headers.Authorization = new AuthenticationHeaderValue(this.tokenType, this.accessToken);
		}

		#endregion

		#region イベント

		/// <summary>
		/// OneDriveの認証が完了した時に発行される
		/// </summary>
		public event OneDriveAuthFinishedEventHandler OneDriveAuthFinished;

		#endregion
	}

	delegate void OneDriveAuthFinishedEventHandler(object sender, OneDriveAuthFinishedEventArgs e);

	class OneDriveAuthFinishedEventArgs : EventArgs
	{
		public IAuthenticationProvider Provider { get; }
		public OneDriveAuthFinishedEventArgs(IAuthenticationProvider provider)
		{
			this.Provider = provider;
		}
	}
}
