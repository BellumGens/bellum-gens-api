using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Utils
{
	public static class Util
	{
		public static string GenerateHashString(int length = 0)
		{
			string text = "";
			string possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			Random random = new Random();

			for (int i = 0; i < length; i++)
			{
				text += possible[(int)Math.Floor(random.NextDouble() * possible.Length)];
			}

			return text;
		}
	}
}