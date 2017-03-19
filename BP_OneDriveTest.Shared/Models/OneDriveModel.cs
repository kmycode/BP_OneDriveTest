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

			var status = "ログインに成功しました\nRoot直下:\n";
			var rootChildren = await this.client.Drive.Root.Children.Request().GetAsync();
			foreach (var c in rootChildren)
			{
				status += "    " + c.Name + "\n";
			}
			this.Status = status;
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
