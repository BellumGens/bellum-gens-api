using BellumGens.Api.Core.Models;
using SteamModels;
using SteamModels.CSGO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BellumGens.Api.Core.Providers
{
    public interface ISteamService
    {
        public Task<CSGOPlayerStats> GetStatsForCSGOUser(string username);
        public Task<SteamUser> GetSteamUser(string name);
        public Task<List<SteamUserSummary>> GetSteamUsersSummary(string users);
        public Task<UserStatsViewModel> GetSteamUserDetails(string name);
        public Task<SteamGroup> GetSteamGroup(string groupid);
        public Task<bool> VerifyUserIsGroupAdmin(string userid, string groupid);
        public Uri NormalizeUsername(string name);
        public string SteamUserId(string userUri);
    }
}
