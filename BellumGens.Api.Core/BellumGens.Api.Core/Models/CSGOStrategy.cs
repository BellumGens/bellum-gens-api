using BellumGens.Api.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BellumGens.Api.Core.Models
{
	public class CSGOStrategy
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid? TeamId { get; set; }

		public string UserId { get; set; }

		public Side Side { get; set; }

		public string Title { get; set; }

		public CSGOMap Map { get; set; }

		public string Description { get; set; }

		public string Url { get; set; }

		[JsonProperty("Image")]
		[Column("Image")]
		public string StratImage { get; set; }

		public string EditorMetadata { get; set; }

		public bool Visible { get; set; }

		[JsonIgnore]
		public string PrivateShareLink { get; set; }

		public DateTimeOffset LastUpdated { get; set; } = DateTimeOffset.Now;

		[MaxLength(64)]
		public string CustomUrl { get; set; }

		[NotMapped]
		public int Rating
		{
			get
			{
				return Votes == null ? 0 : Votes.Where(v => v.Vote == VoteDirection.Up).Count() - Votes.Where(v => v.Vote == VoteDirection.Down).Count();
			}
			private set { }
		}

		[NotMapped]
		public string Owner
		{
			get
			{
				return User?.UserName;
			}
		}

		public virtual ICollection<StrategyVote> Votes { get; set; }

		public virtual ICollection<StrategyComment> Comments { get; set; }

		[JsonIgnore]
		[ForeignKey("TeamId")]
		public virtual CSGOTeam Team { get; set; }

		[JsonIgnore]
		[ForeignKey("UserId")]
		public virtual ApplicationUser User { get; set; }

		public void UniqueCustomUrl(BellumGensDbContext context)
		{
			if (string.IsNullOrEmpty(CustomUrl))
			{
				var parts = Title.Split(' ');
				string url = string.Join("-", parts);
				while (context.Strategies.Where(s => s.CustomUrl == url).SingleOrDefault() != null)
				{
					if (url.Length > 58)
						url = url.Substring(0, 58);
					url += '-' + Util.GenerateHashString(6);
				}
				CustomUrl = url;
			}
		}

		//public void SaveStrategyImage()
		//{
		//	if (!string.IsNullOrEmpty(StratImage) && !Uri.IsWellFormedUriString(StratImage, UriKind.Absolute))
		//	{
		//		string base64 = StratImage.Substring(StratImage.IndexOf(',') + 1);
		//		byte[] bytes = Convert.FromBase64String(base64);

		//		Image image;
		//		using (MemoryStream ms = new MemoryStream(bytes))
		//		{
		//			image = Image.FromStream(ms);
		//			string path = Path.Combine(_hostEnvironment.WebRootPath, "/Content/Strats");
		//			if (!Directory.Exists(path))
		//			{
		//				Directory.CreateDirectory(path);
		//			}
		//			path = Path.Combine(path, $"{CustomUrl}.png");
		//			image.Save(path);
		//			StratImage = CORSConfig.apiDomain + $"/Content/Strats/{CustomUrl}.png";
		//		}
		//	}
		//}
	}
}