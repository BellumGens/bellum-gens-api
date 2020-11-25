using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BellumGens.Api.Core.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
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

		public string BattleNetId { get; set; }

		public string SteamID { get; set; }

		public string TwitchId { get; set; }

		public bool SteamPrivate { get; set; } = false;

        public DateTimeOffset RegisteredOn { get; set; } = DateTimeOffset.Now;

		public DateTimeOffset LastSeen { get; set; } = DateTimeOffset.Now;

		public PlaystyleRole PreferredPrimaryRole { get; set; }

		public PlaystyleRole PreferredSecondaryRole { get; set; }

		// public virtual ICollection<Languages> LanguagesSpoken { get; set; }

		public virtual ICollection<UserAvailability> Availability { get; set; } = new HashSet<UserAvailability>();

		public virtual ICollection<UserMapPool> MapPool { get; set; } = new HashSet<UserMapPool>();

		public virtual ICollection<TeamInvite> Notifications { get; set; } = new HashSet<TeamInvite>();

		[JsonIgnore]
		public virtual ICollection<TeamMember> MemberOf { get; set; } = new HashSet<TeamMember>();

		public virtual ICollection<TeamApplication> TeamApplications { get; set; }
    }

	public class BellumGensDbContext : IdentityDbContext<ApplicationUser>
	{
		public DbSet<UserAvailability> UserAvailabilities { get; set; }

		// public DbSet<Languages> Languages { get; set; }

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

		public DbSet<TournamentCSGOGroup> TournamentCSGOGroups { get; set; }

		public DbSet<TournamentSC2Group> TournamentSC2Groups { get; set; }

		public DbSet<TournamentCSGOMatch> TournamentCSGOMatches { get; set; }

		public DbSet<CSGOMatchMap> TournamentCSGOMatchMaps { get; set; }

		public DbSet<TournamentSC2Match> TournamentSC2Matches { get; set; }

		public DbSet<SC2MatchMap> TournamentSC2MatchMaps { get; set; }

		public DbSet<Company> Companies { get; set; }

		public DbSet<JerseyOrder> JerseyOrders { get; set; }

		public DbSet<Promo> PromoCodes { get; set; }

        public BellumGensDbContext(DbContextOptions<BellumGensDbContext> options)
            : base(options)
        {
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			//modelBuilder.Entity<ApplicationUser>()
			//            .HasMany(e => e.LanguagesSpoken)
			//            .WithMany(e => e.Users)
			//            .Map(e =>
			//            {
			//                e.MapLeftKey("UserId");
			//                e.MapRightKey("LanguageId");
			//                e.ToTable("UserLanguages");
			//            });

			modelBuilder.Entity<ApplicationUser>()
						.Property(p => p.Accuracy)
						.HasPrecision(3, 2);

			modelBuilder.Entity<ApplicationUser>()
						.Property(p => p.HeadshotPercentage)
						.HasPrecision(5, 2);

			modelBuilder.Entity<ApplicationUser>()
						.Property(p => p.KillDeathRatio)
						.HasPrecision(4, 2);

			modelBuilder.Entity<Promo>()
						.Property(p => p.Discount)
						.HasPrecision(3, 2);

			modelBuilder.Entity<ApplicationUser>()
						.HasMany(e => e.Notifications)
						.WithOne(e => e.InvitedUser);

			modelBuilder.Entity<ApplicationUser>()
						.HasMany(e => e.TeamApplications)
						.WithOne(e => e.User);

			modelBuilder.Entity<CSGOStrategy>()
						.HasOne(s => s.Team)
						.WithMany(e => e.Strategies);

			modelBuilder.Entity<CSGOTeam>()
						.HasMany(e => e.Strategies)
						.WithOne(e => e.Team);

			modelBuilder.Entity<Company>()
						.HasIndex(c => c.Name)
						.IsUnique();

			modelBuilder.Entity<CSGOStrategy>()
						.HasIndex(c => c.CustomUrl)
						.IsUnique();

			modelBuilder.Entity<CSGOTeam>()
						.HasIndex(c => c.CustomUrl)
						.IsUnique();

			modelBuilder.Entity<Promo>()
						.HasIndex(c => c.Code)
						.IsUnique();

			modelBuilder.Entity<BellumGensPushSubscription>()
						.HasKey(c => new { c.p256dh, c.auth });

			modelBuilder.Entity<StrategyVote>()
						.HasKey(c => new { c.StratId, c.UserId });

			modelBuilder.Entity<TeamApplication>()
						.HasKey(c => new { c.ApplicantId, c.TeamId });

			modelBuilder.Entity<TeamAvailability>()
						.HasKey(c => new { c.TeamId, c.Day });

			modelBuilder.Entity<UserAvailability>()
						.HasKey(c => new { c.UserId, c.Day });

			modelBuilder.Entity<TeamMapPool>()
						.HasKey(c => new { c.TeamId, c.Map });

			modelBuilder.Entity<UserMapPool>()
						.HasKey(c => new { c.UserId, c.Map });

			modelBuilder.Entity<TeamMember>()
						.HasKey(c => new { c.TeamId, c.UserId });

			modelBuilder.Entity<TeamInvite>()
						.HasKey(c => new { c.InvitingUserId, c.InvitedUserId, c.TeamId });
		}
	}
}