using Newtonsoft.Json;
using SteamModels;
using System.Net.Http;
using System.Xml.Serialization;
using System.Collections.Generic;
using SteamModels.CSGO;
using BellumGens.Api.Models;
using System;

namespace BellumGens.Api.Providers
{
	public static class SteamServiceProvider
	{
		private static readonly string _statsForGameUrl =
				"http://api.steampowered.com/ISteamUserStats/GetUserStatsForGame/v0002/?appid={0}&key={1}&steamid={2}&format=json";

		private static readonly string _playerDetailsUrl =
			"http://steamcommunity.com/profiles/{0}/?xml=1";

		private static readonly string _steamAppNewsUrl = "http://api.steampowered.com/ISteamNews/GetNewsForApp/v0002/?appid={0}&maxlength=300&format=json";

		private static Dictionary<string, UserStatsViewModel> _cachedUsers = new Dictionary<string, UserStatsViewModel>();

		public static CSGOPlayerStats GetStatsForGame(string username)
		{
			HttpClient client = new HttpClient();
			var statsForGameResponse = client.GetStringAsync(string.Format(_statsForGameUrl, SteamInfo.Config.gameId, SteamInfo.Config.steamApiKey, username));
			CSGOPlayerStats statsForUser = JsonConvert.DeserializeObject<CSGOPlayerStats>(statsForGameResponse.Result);
			return statsForUser;
		}

        public static SteamUser GetSteamUser(string name)
        {
            if (_cachedUsers.ContainsKey(name))
            {
                return _cachedUsers[name].steamUser;
            }
            HttpClient client = new HttpClient();
            var playerDetailsResponse = client.GetStreamAsync(NormalizeUsername(name));
            SteamUser steamUser = null;
            XmlSerializer serializer = new XmlSerializer(typeof(SteamUser));
            steamUser = (SteamUser)serializer.Deserialize(playerDetailsResponse.Result);
            return steamUser;
        }

		public static UserStatsViewModel GetSteamUserDetails(string name)
		{
			if (_cachedUsers.ContainsKey(name))
			{
				return _cachedUsers[name];
			}
			HttpClient client = new HttpClient();
			var playerDetailsResponse = client.GetStreamAsync(NormalizeUsername(name));
			SteamUser steamUser = null;
			XmlSerializer serializer = new XmlSerializer(typeof(SteamUser));
			try
			{
				steamUser = (SteamUser)serializer.Deserialize(playerDetailsResponse.Result);
			}
			catch (Exception e)
			{
				return new UserStatsViewModel()
				{
					steamUser = steamUser,
					steamUserException = e.Message
				};
			}

			var statsForGameResponse = client.GetStringAsync(string.Format(_statsForGameUrl, SteamInfo.Config.gameId, SteamInfo.Config.steamApiKey, steamUser.steamID64));
			CSGOPlayerStats statsForUser = null;
			try
			{
				statsForUser = JsonConvert.DeserializeObject<CSGOPlayerStats>(statsForGameResponse.Result);
			}
			catch (Exception e)
			{
				return new UserStatsViewModel()
				{
					steamUser = steamUser,
					userStatsException = e.Message
				};
			}

			UserStatsViewModel user = new UserStatsViewModel()
			{
				steamUser = steamUser,
				userStats = statsForUser
			};
			_cachedUsers[name] = user;
			return user;
		}

		public static bool VerifyUserIsGroupAdmin(string userid, string groupid)
		{
			return true;
		}

		public static void InvalidateUserCache(string name)
		{
			if (_cachedUsers.ContainsKey(name))
			{
				_cachedUsers.Remove(name);
			}
		}

		//public static SteamAppNews GetSteamAppNews(int appid)
		//{
		//	HttpClient client = new HttpClient();
		//	var playerDetailsResponse = client.GetStreamAsync(_steamAppNewsUrl);
		//	XmlSerializer serializer = new XmlSerializer(typeof(SteamAppNews));
		//	SteamAppNews news = (SteamAppNews)serializer.Deserialize(playerDetailsResponse.Result);
		//	return news;
		//}

		//public static SteamNews GetSteamAppNewsJSON(int appid)
		//{
		//	HttpClient client = new HttpClient();
		//	var steamnews = client.GetStringAsync(_steamAppNewsUrl);
		//	SteamNews news = JsonConvert.DeserializeObject<SteamNews>(steamnews.Result);
		//	return news;
		//}

		public static string NormalizeUsername(string name)
		{
			return name.Contains("http://") ? name : string.Format(_playerDetailsUrl, name);
		}

		public static string SteamUserId(string userUri)
		{
			return userUri.Split('/')[5];
		}
	}
}