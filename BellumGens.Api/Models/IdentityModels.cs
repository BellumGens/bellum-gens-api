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
		public ApplicationUser() : base()
		{
			Availability = new HashSet<UserAvailability>();
			MapPool = new HashSet<UserMapPool>();
			MemberOf = new HashSet<TeamMember>();
		}

		public void InitializeDefaults()
		{
			Availability = new HashSet<UserAvailability>() {
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
			MapPool = new HashSet<UserMapPool>()
			{
				new UserMapPool
				{
					Map = CSGOMap.Cache
				},
				new UserMapPool
				{
					Map = CSGOMap.Dust2
				},
				new UserMapPool
				{
					Map = CSGOMap.Inferno
				},
				new UserMapPool
				{
					Map = CSGOMap.Mirage
				},
				new UserMapPool
				{
					Map = CSGOMap.Nuke
				},
				new UserMapPool
				{
					Map = CSGOMap.Overpass
				},
				new UserMapPool
				{
					Map = CSGOMap.Train
				},
				new UserMapPool
				{
					Map = CSGOMap.Vertigo
				},
				new UserMapPool
				{
					Map = CSGOMap.Cobblestone
				}
			};
		}

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType).ConfigureAwait(false);
            // Add custom user claims here
            return userIdentity;
        }

		public string ESEA { get; set; }

        public bool SearchVisible { get; set; } = true;

        public string AvatarFull { get; set; }

        public string AvatarMedium { get; set; }

        public string AvatarIcon { get; set; }

        public string RealName { get; set; }

        public string CustomUrl { get; set; }

        public string Country { get; set; }

        public decimal HeadshotPercentage { get; set; }

        public decimal KillDeathRatio { get; set; }

        public decimal Accuracy { get; set; }

        public bool SteamPrivate { get; set; } = false;

        public DateTimeOffset RegisteredOn { get; set; } = DateTimeOffset.Now;

		public DateTimeOffset LastSeen { get; set; } = DateTimeOffset.Now;

		public PlaystyleRole PreferredPrimaryRole { get; set; }

		public PlaystyleRole PreferredSecondaryRole { get; set; }

		public virtual ICollection<Languages> LanguagesSpoken { get; set; }

		public virtual ICollection<UserAvailability> Availability { get; set; }

		public virtual ICollection<UserMapPool> MapPool { get; set; }

		public virtual ICollection<TeamInvite> Notifications { get; set; }

        [JsonIgnore]
		public ICollection<TeamMember> MemberOf { get; set; }

		public virtual ICollection<TeamApplication> TeamApplications { get; set; }
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

		public DbSet<CSGOStrategy> Strategies { get; set; }

		public DbSet<TeamMapPool> TeamMapPool { get; set; }

		public DbSet<TeamAvailability> TeamPracticeSchedule { get; set; }

		public DbSet<UserMessage> Messages { get; set; }

		public DbSet<BellumGensPushSubscription> PushSubscriptions { get; set; }

		public DbSet<StrategyComment> StrategyComments { get; set; }

		public DbSet<Subscriber> Subscribers { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<TournamentApplication> TournamentApplications { get; set; }

        public DbSet<Company> Companies { get; set; }

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

			modelBuilder.Entity<CSGOStrategy>()
						.HasOptional(s => s.Team)
						.WithMany(e => e.Strategies);

			modelBuilder.Entity<CSGOTeam>()
						.HasMany(e => e.Strategies)
						.WithOptional(e => e.Team);
		}
	}
}