using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using BellumGens.Api.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using SteamModels;

namespace BellumGens.Api.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        private List<CSGOTeamSummaryViewModel> _teams;
		private List<CSGOTeam> _teamAdmin;
		private SteamUser _user;

		public ApplicationUser() : base()
		{
			this.Availability = new HashSet<UserAvailability>();
			this.MapPool = new HashSet<UserMapPool>();
			this.MemberOf = new HashSet<TeamMember>();
		}

		public void InitializeDefaults()
		{

			this.Availability = new HashSet<UserAvailability>() {
				new UserAvailability
				{
					Day = DayOfWeek.Monday
				},
				new UserAvailability
				{
					Day = DayOfWeek.Tuesday
				},
				new UserAvailability
				{
					Day = DayOfWeek.Wednesday
				},
				new UserAvailability
				{
					Day = DayOfWeek.Thursday
				},
				new UserAvailability
				{
					Day = DayOfWeek.Friday
				},
				new UserAvailability
				{
					Day = DayOfWeek.Saturday
				},
				new UserAvailability
				{
					Day = DayOfWeek.Sunday
				}
			};
			this.MapPool = new HashSet<UserMapPool>()
			{
				new UserMapPool
				{
					Map = CSGOMaps.Cache
				},
				new UserMapPool
				{
					Map = CSGOMaps.Dust2
				},
				new UserMapPool
				{
					Map = CSGOMaps.Inferno
				},
				new UserMapPool
				{
					Map = CSGOMaps.Mirage
				},
				new UserMapPool
				{
					Map = CSGOMaps.Nuke
				},
				new UserMapPool
				{
					Map = CSGOMaps.Overpass
				},
				new UserMapPool
				{
					Map = CSGOMaps.Train
				}
			};
		}

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

		public string ESEA { get; set; }

		public bool SearchVisibile { get; set; }

		public PlaystyleRole PreferredPrimaryRole { get; set; }

		public PlaystyleRole PreferredSecondaryRole { get; set; }

		public virtual ICollection<Languages> LanguagesSpoken { get; set; }

		public virtual ICollection<UserAvailability> Availability { get; set; }

		public virtual ICollection<UserMapPool> MapPool { get; set; }

		public virtual ICollection<TeamInvite> Notifications { get; set; }

        [JsonIgnore]
		public virtual ICollection<TeamMember> MemberOf { get; set; }

		public virtual ICollection<TeamApplication> TeamApplications { get; set; }

		[NotMapped]
		public SteamUser SteamUser
		{
			get
			{
				if (_user == null)
				{
					_user = SteamServiceProvider.GetSteamUser(this.Id);
				}
				return _user;
			}
		}

		[NotMapped]
        public List<CSGOTeamSummaryViewModel> Teams
        {
            get
            {
                if (_teams == null)
                {
                    _teams = new List<CSGOTeamSummaryViewModel>();
                    foreach (TeamMember memberof in MemberOf)
                    {
                        _teams.Add(new CSGOTeamSummaryViewModel(memberof.Team));
                    }
                }
                return _teams;
            }
        }

		[NotMapped]
		public List<CSGOTeam> TeamAdmin
		{
			get
			{
				if (_teamAdmin == null)
				{
					_teamAdmin = new List<CSGOTeam>();
					foreach (TeamMember memberof in MemberOf)
					{
						if (memberof.IsAdmin)
							_teamAdmin.Add(memberof.Team);
					}
				}
				return _teamAdmin;
			}
		}
    }

	public class BellumGensDbContext : IdentityDbContext<ApplicationUser>
	{
		public DbSet<UserAvailability> UserAvailabilities { get; set; }

		public DbSet<Languages> Languages { get; set; }

		public DbSet<UserMapPool> UserMapPool { get; set; }

		public DbSet<CSGOTeam> Teams { get; set; }

		public DbSet<TeamMember> TeamMembers { get; set; }

		public DbSet<TeamInvite> TeamInvites { get; set; }

		public DbSet<TeamApplication> TeamApplications { get; set; }

		public DbSet<TeamStrategy> Strategies { get; set; }

		public DbSet<TeamMapPool> TeamMapPool { get; set; }

		public DbSet<TeamAvailability> TeamPracticeSchedule { get; set; }

		public DbSet<UserMessage> Messages { get; set; }

		public DbSet<BellumGensPushSubscription> PushSubscriptions { get; set; }

        public BellumGensDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static BellumGensDbContext Create()
        {
            return new BellumGensDbContext();
        }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ApplicationUser>()
						.HasMany(e => e.LanguagesSpoken)
						.WithMany(e => e.Users)
						.Map(e =>
						{
							e.MapLeftKey("UserId");
							e.MapRightKey("LanguageId");
							e.ToTable("UserLanguages");
						});

			modelBuilder.Entity<ApplicationUser>()
						.HasMany(e => e.Notifications)
						.WithRequired(e => e.InvitedUser);

			modelBuilder.Entity<ApplicationUser>()
						.HasMany(e => e.TeamApplications)
						.WithRequired(e => e.User);

			modelBuilder.Entity<CSGOTeam>()
						.HasMany(e => e.Strategies)
						.WithRequired(e => e.Team);
		}
	}
}