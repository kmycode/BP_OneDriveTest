using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace BP_OneDriveTest.Shared.Models
{
	/// <summary>
	/// OneDriveファイルシステム上にあるオブジェクトの概念（ファイル、ディレクトリ）
	/// </summary>
    public abstract class OneDriveObject
	{
		#region プロパティ

		/// <summary>
		/// OneDriveを操作するためのオブジェクト
		/// </summary>
		protected Item Item { get; }

		/// <summary>
		/// ID
		/// </summary>
		public string Id { get; protected set; }

		/// <summary>
		/// 名前
		/// </summary>
		public string Name { get; protected set; }

		/// <summary>
		/// 自分はディレクトリであるか
		/// </summary>
		public bool IsDirectory { get; private set; }

		#endregion

		#region メソッド

		protected OneDriveObject(Item item)
		{
			this.Item = item;
			if (this.Item != null)
			{
				this.Id = this.Item.Id;
				this.Name = this.Item.Name;
			}

			this.IsDirectory = this is OneDriveDirectory;
		}

		#endregion
	}
}
