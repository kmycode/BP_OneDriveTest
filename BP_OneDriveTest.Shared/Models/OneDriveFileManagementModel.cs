using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BP_OneDriveTest.Shared.Models
{
	/// <summary>
	/// OneDriveのディレクトリやファイルを管理するモデル
	/// </summary>
    class OneDriveFileManagementModel : INotifyPropertyChanged
    {
		#region 変数

		private OneDriveClient client;
		private Stack<OneDriveDirectory> directoryStack = new Stack<OneDriveDirectory>();

		#endregion

		#region プロパティ

		/// <summary>
		/// 現在のディレクトリのオブジェクト
		/// </summary>
		public ICollection<OneDriveObject> CurrentDirectoryObjects { get; } = new ObservableCollection<OneDriveObject>();

		/// <summary>
		/// ダウンロードしたテキスト
		/// </summary>
		public string DownloadedText
		{
			get
			{
				return this._downloadedText;
			}
			set
			{
				if (this._downloadedText != value)
				{
					this._downloadedText = value;
					this.OnPropertyChanged();
				}
			}
		}
		private string _downloadedText;

		#endregion

		#region メソッド

		public OneDriveFileManagementModel()
		{
			// ディレクトリのスタックにRootを入れる
			this.directoryStack.Push(new OneDriveRootDirectory());
		}

		/// <summary>
		/// 指定したディレクトリへ移動する
		/// </summary>
		/// <param name="toDir">移動先のディレクトリ</param>
		public async Task MoveDirectoryAsync(OneDriveObject toDir)
		{
			if (toDir is OneDriveDirectory dir)
			{
				await this.UpdateDirectoryAsync(dir.Id);
				this.directoryStack.Push(dir);
			}
		}

		/// <summary>
		/// 1つ前のディレクトリに戻る。1つ前のディレクトリがない場合は何もしない
		/// </summary>
		public async Task BackDirectoryAsync()
		{
			if (this.directoryStack.Count > 1)
			{
				this.directoryStack.Pop();				// 現在のディレクトリ
				var dir = this.directoryStack.Peek();	// 現在のディレクトリのひとつ上
				await this.UpdateDirectoryAsync(dir.Id);
			}
		}

		/// <summary>
		/// 指定したIDのディレクトリの内容を更新する
		/// </summary>
		/// <param name="dirId">ディレクトリのID</param>
		private async Task UpdateDirectoryAsync(string dirId = null)
		{
			this.CurrentDirectoryObjects.Clear();

			var dirRequest = dirId == null ? this.client.Drive.Root :
											 this.client.Drive.Items[dirId];
			var objs = await dirRequest.Children.Request().GetAsync();
			foreach (var obj in objs)
			{
				if (obj.Folder != null)
				{
					this.CurrentDirectoryObjects.Add(new OneDriveDirectory(obj));
				}
				else
				{
					this.CurrentDirectoryObjects.Add(new OneDriveFile(obj));
				}
			}
		}

		/// <summary>
		/// テストファイルをアップロードする
		/// </summary>
		/// <returns></returns>
		public async Task UploadTestFileAsync()
		{
			var folderId = this.directoryStack.Peek().Id;

			await new OneDriveFileOperator(this.client)
			{
				FileName = "test.txt",
				ParentFolderId = folderId,
			}.UploadFileAsync("こんにちは");
			await this.UpdateDirectoryAsync(folderId);
		}

		/// <summary>
		/// テストファイルをダウンロードする
		/// </summary>
		/// <returns></returns>
		public async Task DownloadTestFileAsync()
		{
			var folderId = this.directoryStack.Peek().Id;

			this.DownloadedText = await new OneDriveFileOperator(this.client)
			{
				FileName = "test.txt",
				ParentFolderId = folderId,
			}.DownloadTextAsync();
		}

		/// <summary>
		/// テストファイルを削除する
		/// </summary>
		/// <returns></returns>
		public async Task DeleteTestFileAsync()
		{
			var folderId = this.directoryStack.Peek().Id;

			await new OneDriveFileOperator(this.client)
			{
				FileName = "test.txt",
				ParentFolderId = folderId,
			}.DeleteFileAsync();
			await this.UpdateDirectoryAsync(folderId);
		}

		/// <summary>
		/// 指定したファイルが存在するか確認する
		/// </summary>
		/// <param name="fileName">ファイル名</param>
		/// <param name="folderId">確認したいファイルが有るフォルダ名</param>
		/// <returns></returns>
		private async Task<bool> CheckExists(string fileName, string folderId = null)
		{
			var requestPath = folderId == null ? this.client.Drive.Root :
												 this.client.Drive.Items[folderId];

			var children = await requestPath.Children
											.Request()
											.GetAsync();
			return children.Any(item => item.Name == fileName);
		}

		/// <summary>
		/// OneDriveとの接続が確立された時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public async void OnOneDriveConnectionEstablished(object sender, OneDriveConnectionEstablishedEventArgs e)
		{
			this.client = e.Client;

			// ルートディレクトリのファイル・フォルダリストを作成する
			await this.UpdateDirectoryAsync();
		}

		#endregion

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
