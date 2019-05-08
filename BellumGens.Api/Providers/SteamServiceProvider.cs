using Newtonsoft.Json;
using SteamModels;
using System.Net.Http;
using System.Xml.Serialization;
using System.Collections.Generic;
using SteamModels.CSGO;
using BellumGens.Api.Models;
using System;
using System.Web.Caching;
using System.Web;
using System.Text.RegularExpressions;

namespace BellumGens.Api.Providers
{
	public static class SteamServiceProvider
	{
		private static Cache _cache = HttpContext.Current.Cache;
		private static readonly string _statsForGameUrl =
				"https://api.steampowered.com/ISteamUserStats/GetUserStatsForGame/v0002/?appid={0}&key={1}&steamid={2}&format=json";

		private static readonly string _steamUserUrl = "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={0}&steamids={1}";

		private static readonly string _playerDetailsById =
			"https://steamcommunity.com/profiles/{0}/?xml=1";

		private static readonly string _playerDetailsByUrl =
			"https://steamcommunity.com/id/{0}/?xml=1";

		private static readonly string _groupMembersUrl = "https://steamcommunity.com/gid/{0}/memberslistxml/?xml=1";

		//private static readonly string _steamAppNewsUrl = "https://api.steampowered.com/ISteamNews/GetNewsForApp/v0002/?appid={0}&maxlength=300&format=json";

		public static CSGOPlayerStats GetStatsForGame(string username)
		{
			HttpClient client = new HttpClient();
			var statsForGameResponse = client.GetStringAsync(string.Format(_statsForGameUrl, SteamInfo.Config.gameId, SteamInfo.Config.steamApiKey, username));
			CSGOPlayerStats statsForUser = JsonConvert.DeserializeObject<CSGOPlayerStats>(statsForGameResponse.Result);
			return statsForUser;
		}

        public static SteamUser GetSteamUser(string name)
        {
            if (_cache.Get(name) is UserStatsViewModel)
            {
				UserStatsViewModel viewModel = _cache.Get(name) as UserStatsViewModel;
				return viewModel.steamUser;
            }
            HttpClient client = new HttpClient();
            var playerDetailsResponse = client.GetStreamAsync(NormalizeUsername(name));
            SteamUser steamUser = null;
            XmlSerializer serializer = new XmlSerializer(typeof(SteamUser));
            steamUser = (SteamUser)serializer.Deserialize(playerDetailsResponse.Result);
            return steamUser;
        }

		public static List<SteamUserSummary> GetSteamUsersSummary(string users)
		{
			HttpClient client = new HttpClient();
			var playerDetailsResponse = client.GetStringAsync(string.Format(_steamUserUrl, SteamInfo.Config.steamApiKey, users));
			SteamUsersSummary result = JsonConvert.DeserializeObject<SteamUsersSummary>(playerDetailsResponse.Result);
			return result.response.players;
		}

		public static UserStatsViewModel GetSteamUserDetails(string name)
		{
			if (_cache.Get(name) is UserStatsViewModel)
			{
				return _cache.Get(name) as UserStatsViewModel;
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
			_cache.Add(name, user, null, DateTime.Now.AddDays(2), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
			return user;
		}

        public static UserStatsViewModel GetSteamUserDetails(UserInfoViewModel user)
        {
            UserStatsViewModel model = GetSteamUserDetails(user.id);
            model.availability = user.availability;
            model.primaryRole = user.primaryRole;
            model.secondaryRole = user.secondaryRole;
            model.mapPool = user.mapPool;
            model.teams = user.teams;
			model.registered = true;
            return model;
        }

		public static SteamGroup GetSteamGroup(string groupid)
		{
			if (_cache.Get(groupid) is SteamGroup)
			{
				return _cache.Get(groupid) as SteamGroup;
			}

			HttpClient client = new HttpClient();
			var playerDetailsResponse = client.GetStreamAsync(string.Format(_groupMembersUrl, groupid));
			XmlSerializer serializer = new XmlSerializer(typeof(SteamGroup));
			SteamGroup group = (SteamGroup)serializer.Deserialize(playerDetailsResponse.Result);

			_cache.Add(groupid, group, null, DateTime.Now.AddDays(7), Cache.NoSlidingExpiration, CacheItemPriority.BelowNormal, null);

			return group;
		}

		public static bool VerifyUserIsGroupAdmin(string userid, string groupid)
		{
			SteamGroup group = SteamServiceProvider.GetSteamGroup(groupid);
			return group.members[0] == userid;
		}

		public static void InvalidateUserCache(string name)
		{
			if (_cache.Get(name) is UserStatsViewModel)
			{
				_cache.Remove(name);
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
			string pattern = "^[0-9]{17}$",
				   url = "^http(s)?://steamcommunity.com";
			return Regex.IsMatch(name, url) ? name + "/?xml=1" : Regex.IsMatch(name, pattern) ? string.Format(_playerDetailsById, name) : string.Format(_playerDetailsByUrl, name);
		}

		public static string SteamUserId(string userUri)
		{
			var parts = userUri.Split('/');
			return parts.Length >= 6 ? parts[5] : null;
		}
	}
}