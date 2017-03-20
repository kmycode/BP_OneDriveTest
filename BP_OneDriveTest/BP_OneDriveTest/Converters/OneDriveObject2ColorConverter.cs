using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BP_OneDriveTest.Converters
{
	class OneDriveObject2ColorConverter : IValueConverter
	{
		private static readonly Color folderColor = Color.Blue;
		private static readonly Color fileColor = Color.Black;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool isDir && targetType == typeof(Color))
			{
				return isDir ? folderColor : fileColor;
			}
			throw new NotSupportedException();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
