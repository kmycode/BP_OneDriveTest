using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace BP_OneDriveTest.Shared.Models
{
	/// <summary>
	/// OneDriveのディレクトリやファイルを管理するモデル
	/// </summary>
    class OneDriveFileManagementModel
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
	}
}
