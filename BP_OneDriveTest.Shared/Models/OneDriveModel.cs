using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;

namespace BP_OneDriveTest.Shared.Models
{
    class OneDriveModel : INotifyPropertyChanged
    {
		#region 変数

		private OneDriveClient client;

		#endregion

		#region プロパティ

		public string Status
		{
			get
			{
				return this._status;
			}
			set
			{
				if (this._status != value)
				{
					this._status = value;
					this.OnPropertyChanged();
				}
			}
		}
		private string _status;

		#endregion

		#region メソッド

		/// <summary>
		/// 認証が完了した時
		/// </summary>
		/// <param name="sender">送信者</param>
		/// <param name="e">イベント</param>
		public async void OnAuthenticationFinished(object sender, OneDriveAuthFinishedEventArgs e)
		{
			this.client = new OneDriveClient(e.Provider);
			this.OneDriveConnectionEstablished?.Invoke(this, new OneDriveConnectionEstablishedEventArgs(this.client));

			var sp1 = System.Diagnostics.Stopwatch.StartNew();
			for (int i = 0; i < 50; i++)
			{
				await this.client.Drive.Root.ItemWithPath("StoryCanvasCloud/workspace1").Children.Request().GetAsync();
				await this.client.Drive.Root.ItemWithPath("StoryCanvasCloud/workspace2").Children.Request().GetAsync();
				await this.client.Drive.Root.ItemWithPath("StoryCanvasCloud/workspace3").Children.Request().GetAsync();
				await this.client.Drive.Root.ItemWithPath("StoryCanvasCloud/workspace4").Children.Request().GetAsync();
			}
			sp1.Stop();

			var sp2 = System.Diagnostics.Stopwatch.StartNew();
			var scc = await this.client.Drive.Root.ItemWithPath("StoryCanvasCloud").Request().GetAsync();
			var id = scc.Id;
			var children = await this.client.Drive.Items[id].Children.Request().GetAsync();
			for (int i = 0; i < 50; i++)
			{
				foreach (var child in children)
				{
					await this.client.Drive.Items[child.Id].Children.Request().GetAsync();
				}
			}
			sp2.Stop();

			System.Diagnostics.Debug.WriteLine("ItemWithPathのみ:" + sp1.ElapsedMilliseconds);
			System.Diagnostics.Debug.WriteLine("IDと併用        :" + sp2.ElapsedMilliseconds);

			//var status = "ログインに成功しました\nRoot直下:\n";
			//var rootChildren = await this.client.Drive.Root.Children.Request().GetAsync();
			//foreach (var c in rootChildren)
			//{
			//	status += "    " + c.Name + "\n";
			//}
			//this.Status = status;
		}

		#endregion

		#region イベント

		public event OneDriveConnectionEstablishedEventHandler OneDriveConnectionEstablished;

		#endregion

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}

	delegate void OneDriveConnectionEstablishedEventHandler(object sender, OneDriveConnectionEstablishedEventArgs e);

	class OneDriveConnectionEstablishedEventArgs : EventArgs
	{
		public OneDriveClient Client { get; }
		public OneDriveConnectionEstablishedEventArgs(OneDriveClient client)
		{
			this.Client = client;
		}
	}
}
