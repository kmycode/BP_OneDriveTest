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
	public partial class MainPage : ContentPage, IAuthBrowserProvider
	{
		public MainPage()
		{
			InitializeComponent();
		}

		#region IAuthBrowserProvider

		private AuthPage authPage
		{
			get
			{
				if (this._authPage == null)
				{
					this._authPage = new AuthPage();
				}
				return this._authPage;
			}
		}
		private AuthPage _authPage;

		public IAuthBrowser OpenBrowser()
		{
			this.Navigation.PushModalAsync(this.authPage);
			return this.authPage;
		}

		public async void CloseBrowser()
		{
			await this.Navigation.PopModalAsync();
		}

		#endregion
	}
}
