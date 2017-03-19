using BP_OneDriveTest.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BP_OneDriveTest
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AuthPage : ContentPage, IAuthBrowser
	{
		public AuthPage()
		{
			InitializeComponent();
			this.Browser.Navigated += (sender, e) =>
			{
				this.Navigated?.Invoke(this, new AuthBrowserNavigatedEventArgs(e.Url));
			};
		}

		#region IAuthBrowser

		public event AuthBrowserNavigatedEventHandler Navigated;

		public void GoToUrl(string url)
		{
			this.Browser.Source = url;
		}

		#endregion
	}
}
