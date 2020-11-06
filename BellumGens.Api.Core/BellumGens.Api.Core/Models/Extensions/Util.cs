using BellumGens.Api.Core.Models;
using System;
using System.Collections.Generic;

namespace BellumGens.Api.Common
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

		public static Dictionary<JerseyCut, string> JerseyCutNames = new Dictionary<JerseyCut, string>()
		{
			{ JerseyCut.Male, "Мъжка" },
			{ JerseyCut.Female, "Дамска" }
		};

		public static Dictionary<JerseySize, string> JerseySizeNames = new Dictionary<JerseySize, string>()
		{
			{ JerseySize.XS, "XS" },
			{ JerseySize.S, "S" },
			{ JerseySize.M, "M" },
			{ JerseySize.L, "L" },
			{ JerseySize.XL, "XL" },
			{ JerseySize.XXL, "XXL" },
			{ JerseySize.XXXL, "XXXL" }
		};
	}
}