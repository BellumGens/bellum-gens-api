﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace BellumGens.Api.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        private List<CSGOTeam> _teams;

		public ApplicationUser() : base()
		{
			this.Availability = new HashSet<UserAvailability>();
			this.MapPool = new HashSet<UserMapPool>();
			this.MemberOf = new HashSet<TeamMember>();
		}

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

		public PlaystyleRole PreferredPrimaryRole { get; set; }

		public PlaystyleRole PreferredSecondaryRole { get; set; }

		public virtual ICollection<Languages> LanguagesSpoken { get; set; }

		public virtual ICollection<UserAvailability> Availability { get; set; }

		public virtual ICollection<UserMapPool> MapPool { get; set; }

        [JsonIgnore]
		public virtual ICollection<TeamMember> MemberOf { get; set; }

        [NotMapped]
        public List<CSGOTeam> Teams
        {
            get
            {
                if (_teams == null)
                {
                    _teams = new List<CSGOTeam>();
                    foreach (TeamMember memberof in MemberOf)
                    {
                        _teams.Add(memberof.Team);
                    }
                }
                return _teams;
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
		}
	}
}