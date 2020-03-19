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
using System.Threading.Tasks;

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

		public static async Task<CSGOPlayerStats> GetStatsForCSGOUser(string username)
		{
			CSGOPlayerStats statsForUser;
			using (HttpClient client = new HttpClient())
			{
				Uri endpoint = new Uri(string.Format(_statsForGameUrl, AppInfo.Config.csgoGameId, AppInfo.Config.steamApiKey, username));
				var statsForGameResponse = await client.GetStringAsync(endpoint).ConfigureAwait(false);
				statsForUser = JsonConvert.DeserializeObject<CSGOPlayerStats>(statsForGameResponse);

			}
			return statsForUser;
		}

        public static async Task<SteamUser> GetSteamUser(string name)
        {
			if (_cache.Get(name) is UserStatsViewModel)
			{
				UserStatsViewModel viewModel = _cache.Get(name) as UserStatsViewModel;
				return viewModel.steamUser;
			}

			SteamUser user;
			using (HttpClient client = new HttpClient())
			{
				var playerDetailsResponse = await client.GetStreamAsync(NormalizeUsername(name)).ConfigureAwait(false);
				XmlSerializer serializer = new XmlSerializer(typeof(SteamUser));
				user = (SteamUser)serializer.Deserialize(playerDetailsResponse);
			}
			return user;
		}

		public static async Task<List<SteamUserSummary>> GetSteamUsersSummary(string users)
		{
			SteamUsersSummary result;
			using (HttpClient client = new HttpClient())
			{
				var playerDetailsResponse = await client.GetStringAsync(string.Format(_steamUserUrl, AppInfo.Config.steamApiKey, users)).ConfigureAwait(false);
				result = JsonConvert.DeserializeObject<SteamUsersSummary>(playerDetailsResponse);
			}
			return result.response.players;
		}

		public static async Task<UserStatsViewModel> GetSteamUserDetails(string name)
		{
			lock (_cache)
			{
				if (_cache.Get(name) is UserStatsViewModel)
				{
					return _cache.Get(name) as UserStatsViewModel;
				}
			}

			UserStatsViewModel model = new UserStatsViewModel();
			using (HttpClient client = new HttpClient())
			{
				var playerDetailsResponse = await client.GetAsync(NormalizeUsername(name)).ConfigureAwait(false);

				if (playerDetailsResponse.IsSuccessStatusCode)
				{
					XmlSerializer serializer = new XmlSerializer(typeof(SteamUser));

					try
					{
						model.steamUser = (SteamUser)serializer.Deserialize(await playerDetailsResponse.Content.ReadAsStreamAsync().ConfigureAwait(false));
					}
					catch
					{
						model.steamUserException = true;
						return model;
					}
				}
				else
				{
					model.steamUserException = true;
					return model;
				}

				Uri endpoint = new Uri(string.Format(_statsForGameUrl, AppInfo.Config.csgoGameId, AppInfo.Config.steamApiKey, model.steamUser.steamID64));
				var statsForGameResponse = await client.GetAsync(endpoint).ConfigureAwait(false);
				if (statsForGameResponse.IsSuccessStatusCode)
				{
					try
					{
						model.userStats = JsonConvert.DeserializeObject<CSGOPlayerStats>(await statsForGameResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
						lock (_cache)
						{
							_cache.Add(name, model, null, DateTime.Now.AddDays(2), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
						}
						return model;
					}
					catch
					{
						model.userStatsException = true;
						return model;
					}
				}
				model.userStatsException = true;
			}
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
			SteamGroup group = GetSteamGroup(groupid);
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

		public static Uri NormalizeUsername(string name)
		{
			string pattern = "^[0-9]{17}$",
				   url = "^http(s)?://steamcommunity.com";
			return Regex.IsMatch(name, url) ? new Uri(name + "/?xml=1") : Regex.IsMatch(name, pattern) ? new Uri(string.Format(_playerDetailsById, name)) : new Uri(string.Format(_playerDetailsByUrl, name));
		}

		public static string SteamUserId(string userUri)
		{
			var parts = userUri.Split('/');
			return parts.Length >= 6 ? parts[5] : null;
		}
	}
}