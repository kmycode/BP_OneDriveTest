using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using BP_OneDriveTest.Shared.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 を参照してください

namespace BP_OneDriveTest.UWP
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page, IAuthBrowserProvider, IAuthBrowser
    {
        public MainPage()
        {
            this.InitializeComponent();

			// 自身をIAuthBrowserProviderに登録
			DependencyInjection.AuthBrowserProvider = this;

			// イベント登録
			this.Browser.NavigationCompleted +=
				(sender, e) => this.Navigated?.Invoke(this, new AuthBrowserNavigatedEventArgs(e.Uri.AbsoluteUri));
        }

		#region IAuthBrowserProvider

		public IAuthBrowser OpenBrowser()
		{
			// ブラウザはもう表示されてる
			return this;
		}

		public void CloseBrowser()
		{
			// 何もしない
		}

		#endregion

		#region IAuthBrowser

		public void GoToUrl(string url)
		{
			this.Browser.Navigate(new Uri(url));
		}

		public event AuthBrowserNavigatedEventHandler Navigated;

		#endregion
	}
}
