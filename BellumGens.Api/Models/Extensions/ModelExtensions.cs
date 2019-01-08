using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Models.Extensions
{
	public static class ModelExtensions
	{
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
					 * user				|---------| 
					 * team    |------------|
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
	}
}