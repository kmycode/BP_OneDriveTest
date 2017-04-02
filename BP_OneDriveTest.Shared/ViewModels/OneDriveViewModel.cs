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
		private OneDriveFileManagementModel fileManagementModel = new OneDriveFileManagementModel();

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

		public ICollection<OneDriveObject> CurrentDirectoryObjects
		{
			get
			{
				return this.fileManagementModel.CurrentDirectoryObjects;
			}
		}

		/// <summary>
		/// 現在選択されているOneDriveのオブジェクト
		/// </summary>
		public OneDriveObject SelectedOneDriveObject
		{
			get
			{
				return this._selectedOneDriveObject;
			}
			set
			{
				if (this._selectedOneDriveObject != value)
				{
					this._selectedOneDriveObject = value;
					this.OnPropertyChanged();
				}
			}
		}
		private OneDriveObject _selectedOneDriveObject;

		/// <summary>
		/// ダウンロードしたテキスト
		/// </summary>
		public string DownloadedText
		{
			get
			{
				return this.fileManagementModel.DownloadedText;
			}
		}
		
		#endregion

		#region メソッド

		public OneDriveViewModel()
		{
			// OnPropertyChangedをつなげる
			this.model.PropertyChanged += this.RaisePropertyChanged;
			this.fileManagementModel.PropertyChanged += this.RaisePropertyChanged;

			// イベントをつなげる
			this.authModel.OneDriveAuthFinished += this.model.OnAuthenticationFinished;
			this.model.OneDriveConnectionEstablished += this.fileManagementModel.OnOneDriveConnectionEstablished;

			// 認証完了時、ブラウザを操作する
			this.authModel.OneDriveAuthFinished += this.OneDriveAuthFinished;
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

		/// <summary>
		/// 指定したディレクトリへ移動する
		/// </summary>
		public RelayCommand MoveDirectoryCommand
		{
			get
			{
				return this._moveDirectoryCommand = this._moveDirectoryCommand ?? new RelayCommand(async () =>
				{
					await this.fileManagementModel.MoveDirectoryAsync(this.SelectedOneDriveObject);
				});
			}
		}
		private RelayCommand _moveDirectoryCommand;

		/// <summary>
		/// 1つ前のディレクトリに戻る
		/// </summary>
		public RelayCommand BackDirectoryCommand
		{
			get
			{
				return this._backDirectoryCommand = this._backDirectoryCommand ?? new RelayCommand(async () =>
				{
					await this.fileManagementModel.BackDirectoryAsync();
				});
			}
		}
		private RelayCommand _backDirectoryCommand;

		/// <summary>
		/// テストファイルをアップロードする
		/// </summary>
		public RelayCommand UploadTestFileCommand
		{
			get
			{
				return this._uploadTestFileCommand = this._uploadTestFileCommand ?? new RelayCommand(async () =>
				{
					await this.fileManagementModel.UploadTestFileAsync();
				});
			}
		}
		private RelayCommand _uploadTestFileCommand;

		/// <summary>
		/// テストファイルをダウンロードする
		/// </summary>
		public RelayCommand DownloadTestFileCommand
		{
			get
			{
				return this._downloadTestFileCommand = this._downloadTestFileCommand ?? new RelayCommand(async () =>
				{
					await this.fileManagementModel.DownloadTestFileAsync();
				});
			}
		}
		private RelayCommand _downloadTestFileCommand;

		/// <summary>
		/// テストファイルを削除する
		/// </summary>
		public RelayCommand DeleteTestFileCommand
		{
			get
			{
				return this._deleteTestFileCommand = this._deleteTestFileCommand ?? new RelayCommand(async () =>
				{
					await this.fileManagementModel.DeleteTestFileAsync();
				});
			}
		}
		private RelayCommand _deleteTestFileCommand;
		
		#endregion

		#region メソッド

		/// <summary>
		/// 認証が完了した時、ブラウザを閉じるようにする（画面を操作する）
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OneDriveAuthFinished(object sender, OneDriveAuthFinishedEventArgs e)
		{
			// ブラウザを閉じて、循環参照を抜く
			this.browser.Navigated -= this.authModel.OnAuthBrowserNavigated;
			DependencyInjection.AuthBrowserProvider.CloseBrowser();
			this.browser = null;
		}

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
