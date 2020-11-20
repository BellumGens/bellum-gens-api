using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace BellumGens.Api.Core.Models.Extensions
{
	public static class ModelExtensions
	{
		public static string GetUserId(this ClaimsPrincipal principal)
		{
			if (principal == null)
				throw new ArgumentNullException(nameof(principal));

			return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}

		public static string GetSteamUserId(this ClaimsPrincipal identity)
        {
            var parts = identity.GetUserId().Split('/');
            return parts.Length >= 6 ? parts[5] : null;
        }

        public static string GetResolvedUserId(this ClaimsPrincipal identity)
        {
            string userId = identity.GetSteamUserId();
            if (userId == null)
                userId = identity.GetUserId();
            return userId;
        }

        public static double GetTotalAvailability(this ApplicationUser user)
		{
			double total = 0;
			foreach (Availability availability in user.Availability.Where(a => a.Available))
			{
				total += (availability.To - availability.From).TotalHours;
			}
			return total;
		}

		public static double GetTotalAvailability(this CSGOTeam team)
		{
			double total = 0;
			foreach (Availability availability in team.PracticeSchedule.Where(a => a.Available))
			{
				total += (availability.To - availability.From).TotalHours;
			}
			return total;
		}

		public static double GetTotalOverlap(this CSGOTeam team, ApplicationUser user)
		{
			double total = 0;
			foreach (TeamAvailability practice in team.PracticeSchedule.Where(d => d.Available))
			{
				UserAvailability availability = user.Availability.SingleOrDefault(a => a.Day == practice.Day && a.Available);
				if (availability != null)
				{
					/* CASE 0
					 * user     |------------|
					 * team	                     |----------------------|
					 * OR
					 * user                           |------------|
					 * team	   |----------------------|
					 */
					if (practice.To <= availability.From || practice.From >= availability.To)
						continue;

					/* CASE 1
					 * user     |------------|
					 * team	|----------------------|	
					 */
					if (practice.From <= availability.From && practice.To >= availability.To)
						total += (availability.To - availability.From).TotalHours;
					/* CASE 2
					 * user |----------------------| 
					 * team    |------------|
					 */
					else if (practice.From >= availability.From && practice.To <= availability.To)
						total += (practice.To - practice.From).TotalHours;
					/* CASE 3
					 * user	            |---------| 
					 * team   |------------|
					 */
					else if (practice.To > availability.From)
						total += (practice.To - availability.From).TotalHours;
					/* CASE 4
					 * user	  |---------| 
					 * team         |------------|
					 */
					else if (practice.From < availability.To)
						total += (availability.To - practice.From).TotalHours;
				}
			}
			return total;
		}

		public static double GetTotalOverlap(this ApplicationUser player, ApplicationUser user)
		{
			double total = 0;
			foreach (UserAvailability practice in player.Availability.Where(d => d.Available))
			{
				UserAvailability availability = user.Availability.SingleOrDefault(a => a.Day == practice.Day && a.Available);
				if (availability != null)
				{
					/* CASE 0
					 * user     |------------|
					 * player	                     |----------------------|
					 * OR
					 * user                           |------------|
					 * player  |----------------------|
					 */
					if (practice.To <= availability.From || practice.From >= availability.To)
						continue;

					/* CASE 1
					 * user     |------------|
					 * player |----------------------|	
					 */
					if (practice.From <= availability.From && practice.To >= availability.To)
						total += (availability.To - availability.From).TotalHours;
					/* CASE 2
					 * user    |----------------------| 
					 * player    |------------|
					 */
					else if (practice.From >= availability.From && practice.To <= availability.To)
						total += (practice.To - practice.From).TotalHours;
					/* CASE 3
					 * user	            |---------| 
					 * player   |------------|
					 */
					else if (practice.To > availability.From)
						total += (practice.To - availability.From).TotalHours;
					/* CASE 4
					 * user	  |---------| 
					 * player      |------------|
					 */
					else if (practice.From < availability.To)
						total += (availability.To - practice.From).TotalHours;
				}
			}
			return total;
		}
	}
}