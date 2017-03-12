using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using BP_OneDriveTest.Shared.Common;
using BP_OneDriveTest.Shared.Models;

namespace BP_OneDriveTest.Shared.ViewModels
{
	/// <summary>
	/// OneDrive画面のビューモデル
	/// </summary>
	public class OneDriveViewModel : INotifyPropertyChanged
	{
		#region モデル

		private OneDriveAuthModel authModel = new OneDriveAuthModel();
		private OneDriveModel model = new OneDriveModel();

		#endregion

		#region 変数

		private IAuthBrowser browser;

		#endregion

		#region モデルのプロパティ

		public string Status
		{
			get
			{
				return this.model.Status;
			}
		}

		#endregion

		#region メソッド

		public OneDriveViewModel()
		{
			// OnPropertyChangedをつなげる
			this.model.PropertyChanged += this.RaisePropertyChanged;

			// イベントをつなげる
			this.authModel.OneDriveAuthFinished += this.model.OnAuthenticationFinished;

			// 認証完了時、ブラウザからAuthModelへの参照を抜く
			this.authModel.OneDriveAuthFinished += (sender, e) =>
			{
				this.browser.Navigated -= this.authModel.OnAuthBrowserNavigated;
				this.browser = null;
			};
		}

		#endregion

		#region コマンド

		/// <summary>
		/// 認証を行う
		/// </summary>
		public RelayCommand AuthenticateCommand
		{
			get
			{
				return this._authenticateCommand = this._authenticateCommand ?? new RelayCommand(() =>
				{
					// ブラウザを操作する
					var url = this.authModel.BuildAuthUrl();
					this.browser = DependencyInjection.AuthBrowserProvider.OpenBrowser();
					this.browser.Navigated += this.authModel.OnAuthBrowserNavigated;
					this.browser.GoToUrl(url);
				});
			}
		}
		private RelayCommand _authenticateCommand;

		#endregion

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		protected void RaisePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnPropertyChanged(e.PropertyName);
		}

		#endregion
	}
}
