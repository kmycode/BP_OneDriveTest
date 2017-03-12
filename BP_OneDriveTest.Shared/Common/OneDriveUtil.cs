using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BP_OneDriveTest.Shared.Common
{
    static class OneDriveUtil
    {
		/// <summary>
		/// URLから、GETパラメータを取得する
		/// </summary>
		/// <param name="url">調べるURL</param>
		/// <returns>GETパラメータのキー名と値の組み合わせ</returns>
		public static IDictionary<string, string> ParseUrlParameters(string url)
		{
			var parameters = new Dictionary<string, string>();

			var query = url.Split('#').ElementAtOrDefault(1);
			if (string.IsNullOrEmpty(query)) return parameters;

			var rawParameters = query.Split('&');
			foreach (var rawParameter in rawParameters)
			{
				var tmp = rawParameter.Split('=');
				if (tmp.Length == 2)
				{
					parameters.Add(tmp[0], Uri.UnescapeDataString(tmp[1]));
				}
			}
			return parameters;
		}
    }
}
