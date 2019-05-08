using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace BellumGens.Api.Models
{
	public class BGUserInfoViewModel : ApplicationUser
	{
		[JsonIgnore]
		public override string PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }

		[JsonIgnore]
		public override int AccessFailedCount { get => base.AccessFailedCount; set => base.AccessFailedCount = value; }

		[JsonIgnore]
		public override ICollection<IdentityUserClaim> Claims => base.Claims;

		[JsonIgnore]
		public override bool LockoutEnabled { get => base.LockoutEnabled; set => base.LockoutEnabled = value; }

		[JsonIgnore]
		public override DateTime? LockoutEndDateUtc { get => base.LockoutEndDateUtc; set => base.LockoutEndDateUtc = value; }

		[JsonIgnore]
		public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

		[JsonIgnore]
		public override ICollection<IdentityUserLogin> Logins => base.Logins;

		[JsonIgnore]
		public override bool PhoneNumberConfirmed { get => base.PhoneNumberConfirmed; set => base.PhoneNumberConfirmed = value; }

		[JsonIgnore]
		public override string SecurityStamp { get => base.SecurityStamp; set => base.SecurityStamp = value; }

		[JsonIgnore]
		public override bool TwoFactorEnabled { get => base.TwoFactorEnabled; set => base.TwoFactorEnabled = value; }
	}
}