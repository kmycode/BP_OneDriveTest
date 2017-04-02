using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace BP_OneDriveTest.Shared.Models
{
    class OneDriveFileOperator
    {
		private OneDriveClient client;

		public string FileName { get; set; }

		public string ParentFolderId { get; set; }

		public OneDriveFileOperator(OneDriveClient client)
		{
			this.client = client;
		}

		/// <summary>
		/// ファイルをアップロードする
		/// </summary>
		/// <param name="text">アップロードする文字列</param>
		/// <returns></returns>
		public async Task UploadFileAsync(string text)
		{
			await this.UploadFileAsync(Encoding.UTF8.GetBytes(text));
		}

		/// <summary>
		/// ファイルをアップロードする
		/// </summary>
		/// <param name="data">アップロードするバイナリ配列のデータ</param>
		/// <returns></returns>
		public async Task UploadFileAsync(byte[] data)
		{
			using (var contentStream = new MemoryStream(data))
			{
				await this.UploadFileAsync(contentStream);
			}
		}

		/// <summary>
		/// ファイルをアップロードする
		/// </summary>
		/// <param name="stream">アップロードするストリーム</param>
		/// <returns></returns>
		public async Task UploadFileAsync(Stream stream)
		{
			var requestPath = this.ParentFolderId == null ? this.client.Drive.Root :
															this.client.Drive.Items[this.ParentFolderId];

			await requestPath.ItemWithPath(this.FileName)
							 .Content
							 .Request()
							 .PutAsync<Item>(stream);
		}

		/// <summary>
		/// ファイルのストリームを入手する
		/// </summary>
		/// <returns>ストリーム</returns>
		public async Task<Stream> GetDownloadStreamAsync()
		{
			if (!(await this.CheckExistsAsync()))
			{
				// 本来ならここで例外を投げたほうがいい
				return new MemoryStream();
			}

			var requestPath = this.ParentFolderId == null ? this.client.Drive.Root :
													 this.client.Drive.Items[this.ParentFolderId];

			return await requestPath.ItemWithPath(this.FileName)
													.Content
													.Request()
													.GetAsync();
		}

		/// <summary>
		/// ファイルを文字列としてダウンロードする
		/// </summary>
		/// <returns>ファイルの中身</returns>
		public async Task<string> DownloadTextAsync()
		{
			string text = null;

			using (var fileStream = await this.GetDownloadStreamAsync())
			{
				text = new StreamReader(fileStream).ReadToEnd();
			}

			return text;
		}

		/// <summary>
		/// ファイルを削除する
		/// </summary>
		/// <returns></returns>
		public async Task DeleteFileAsync()
		{
			if (!(await this.CheckExistsAsync()))
			{
				return;
			}

			var requestPath = this.ParentFolderId == null ? this.client.Drive.Root :
													 this.client.Drive.Items[this.ParentFolderId];

			await requestPath.ItemWithPath(this.FileName)
								.Request()
								.DeleteAsync();
		}

		/// <summary>
		/// 指定したファイルが存在するか確認する
		/// </summary>
		/// <param name="fileName">ファイル名</param>
		/// <param name="folderId">確認したいファイルが有るフォルダ名</param>
		/// <returns></returns>
		public async Task<bool> CheckExistsAsync()
		{
			var requestPath = this.ParentFolderId == null ? this.client.Drive.Root :
															this.client.Drive.Items[this.ParentFolderId];

			var children = await requestPath.Children
											.Request()
											.GetAsync();
			return children.Any(item => item.Name == this.FileName);
		}
	}
}
