using BP_OneDriveTest.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace BP_OneDriveTest.UWP.Converters
{
	/// <summary>
	/// ディレクトリかファイルかを、色に変換するコンバータ
	/// </summary>
	class OneDriveObject2ColorConverter : IValueConverter
	{
		private static readonly Brush folderBrush = new SolidColorBrush(Colors.Blue);
		private static readonly Brush fileBrush = new SolidColorBrush(Colors.Black);

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is bool isDir && targetType == typeof(Brush))
			{
				return isDir ? folderBrush : fileBrush;
			}
			throw new NotSupportedException();
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
