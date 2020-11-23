using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BellumGens.Api.Core.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "AspNetRoles",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUsers",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        ESEA = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SearchVisible = table.Column<bool>(type: "bit", nullable: false),
            //        AvatarFull = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AvatarMedium = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AvatarIcon = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RealName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CustomUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        HeadshotPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
            //        KillDeathRatio = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
            //        Accuracy = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
            //        BattleNetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SteamID = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SteamPrivate = table.Column<bool>(type: "bit", nullable: false),
            //        RegisteredOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //        LastSeen = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //        PreferredPrimaryRole = table.Column<int>(type: "int", nullable: false),
            //        PreferredSecondaryRole = table.Column<int>(type: "int", nullable: false),
            //        UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
            //        PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
            //        TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
            //        LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
            //        AccessFailedCount = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Companies",
            //    columns: table => new
            //    {
            //        Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Companies", x => x.Name);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PromoCodes",
            //    columns: table => new
            //    {
            //        Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Discount = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
            //        Expiration = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PromoCodes", x => x.Code);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Subscribers",
            //    columns: table => new
            //    {
            //        Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Subscribed = table.Column<bool>(type: "bit", nullable: false),
            //        SubKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Subscribers", x => x.Email);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Teams",
            //    columns: table => new
            //    {
            //        TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        SteamGroupId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
            //        TeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        TeamAvatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Discord = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        RegisteredOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //        Visible = table.Column<bool>(type: "bit", nullable: false),
            //        CustomUrl = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Teams", x => x.TeamId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Tournaments",
            //    columns: table => new
            //    {
            //        ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //        EndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //        Active = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Tournaments", x => x.ID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetRoleClaims",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
            //            column: x => x.RoleId,
            //            principalTable: "AspNetRoles",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserClaims",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetUserClaims_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserLogins",
            //    columns: table => new
            //    {
            //        LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
            //        table.ForeignKey(
            //            name: "FK_AspNetUserLogins_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserRoles",
            //    columns: table => new
            //    {
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
            //        table.ForeignKey(
            //            name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
            //            column: x => x.RoleId,
            //            principalTable: "AspNetRoles",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_AspNetUserRoles_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserTokens",
            //    columns: table => new
            //    {
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
            //        table.ForeignKey(
            //            name: "FK_AspNetUserTokens_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Messages",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        From = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        To = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        TimeStamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //        Message = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Messages", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Messages_AspNetUsers_From",
            //            column: x => x.From,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Messages_AspNetUsers_To",
            //            column: x => x.To,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PushSubscriptions",
            //    columns: table => new
            //    {
            //        p256dh = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        auth = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        userId = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        endpoint = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        expirationTime = table.Column<TimeSpan>(type: "time", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PushSubscriptions", x => new { x.p256dh, x.auth });
            //        table.ForeignKey(
            //            name: "FK_PushSubscriptions_AspNetUsers_userId",
            //            column: x => x.userId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "UserAvailabilities",
            //    columns: table => new
            //    {
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Day = table.Column<int>(type: "int", nullable: false),
            //        Available = table.Column<bool>(type: "bit", nullable: false),
            //        From = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //        To = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_UserAvailabilities", x => new { x.UserId, x.Day });
            //        table.ForeignKey(
            //            name: "FK_UserAvailabilities_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "UserMapPool",
            //    columns: table => new
            //    {
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Map = table.Column<int>(type: "int", nullable: false),
            //        IsPlayed = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_UserMapPool", x => new { x.UserId, x.Map });
            //        table.ForeignKey(
            //            name: "FK_UserMapPool_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "JerseyOrders",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        City = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        PromoCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        Confirmed = table.Column<bool>(type: "bit", nullable: false),
            //        Shipped = table.Column<bool>(type: "bit", nullable: false),
            //        OrderDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_JerseyOrders", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_JerseyOrders_PromoCodes_PromoCode",
            //            column: x => x.PromoCode,
            //            principalTable: "PromoCodes",
            //            principalColumn: "Code",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Strategies",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        Side = table.Column<int>(type: "int", nullable: false),
            //        Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Map = table.Column<int>(type: "int", nullable: false),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        EditorMetadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Visible = table.Column<bool>(type: "bit", nullable: false),
            //        PrivateShareLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //        CustomUrl = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Strategies", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Strategies_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Strategies_Teams_TeamId",
            //            column: x => x.TeamId,
            //            principalTable: "Teams",
            //            principalColumn: "TeamId",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TeamApplications",
            //    columns: table => new
            //    {
            //        ApplicantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        State = table.Column<int>(type: "int", nullable: false),
            //        Sent = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TeamApplications", x => new { x.ApplicantId, x.TeamId });
            //        table.ForeignKey(
            //            name: "FK_TeamApplications_AspNetUsers_ApplicantId",
            //            column: x => x.ApplicantId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_TeamApplications_Teams_TeamId",
            //            column: x => x.TeamId,
            //            principalTable: "Teams",
            //            principalColumn: "TeamId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TeamInvites",
            //    columns: table => new
            //    {
            //        InvitingUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        InvitedUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        State = table.Column<int>(type: "int", nullable: false),
            //        Sent = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TeamInvites", x => new { x.InvitingUserId, x.InvitedUserId, x.TeamId });
            //        table.ForeignKey(
            //            name: "FK_TeamInvites_AspNetUsers_InvitedUserId",
            //            column: x => x.InvitedUserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_TeamInvites_AspNetUsers_InvitingUserId",
            //            column: x => x.InvitingUserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_TeamInvites_Teams_TeamId",
            //            column: x => x.TeamId,
            //            principalTable: "Teams",
            //            principalColumn: "TeamId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TeamMapPool",
            //    columns: table => new
            //    {
            //        TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Map = table.Column<int>(type: "int", nullable: false),
            //        IsPlayed = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TeamMapPool", x => new { x.TeamId, x.Map });
            //        table.ForeignKey(
            //            name: "FK_TeamMapPool_Teams_TeamId",
            //            column: x => x.TeamId,
            //            principalTable: "Teams",
            //            principalColumn: "TeamId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TeamMembers",
            //    columns: table => new
            //    {
            //        TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        IsAdmin = table.Column<bool>(type: "bit", nullable: false),
            //        IsEditor = table.Column<bool>(type: "bit", nullable: false),
            //        Role = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TeamMembers", x => new { x.TeamId, x.UserId });
            //        table.ForeignKey(
            //            name: "FK_TeamMembers_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_TeamMembers_Teams_TeamId",
            //            column: x => x.TeamId,
            //            principalTable: "Teams",
            //            principalColumn: "TeamId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TeamPracticeSchedule",
            //    columns: table => new
            //    {
            //        TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Day = table.Column<int>(type: "int", nullable: false),
            //        Available = table.Column<bool>(type: "bit", nullable: false),
            //        From = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //        To = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TeamPracticeSchedule", x => new { x.TeamId, x.Day });
            //        table.ForeignKey(
            //            name: "FK_TeamPracticeSchedule_Teams_TeamId",
            //            column: x => x.TeamId,
            //            principalTable: "Teams",
            //            principalColumn: "TeamId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TournamentCSGOGroups",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        TournamentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TournamentCSGOGroups", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_TournamentCSGOGroups_Tournaments_TournamentId",
            //            column: x => x.TournamentId,
            //            principalTable: "Tournaments",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TournamentSC2Groups",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        TournamentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TournamentSC2Groups", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_TournamentSC2Groups_Tournaments_TournamentId",
            //            column: x => x.TournamentId,
            //            principalTable: "Tournaments",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "JerseyDetails",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Cut = table.Column<int>(type: "int", nullable: false),
            //        Size = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_JerseyDetails", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_JerseyDetails_JerseyOrders_OrderId",
            //            column: x => x.OrderId,
            //            principalTable: "JerseyOrders",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "StrategyComments",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        StratId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        Published = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //        Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_StrategyComments", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_StrategyComments_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_StrategyComments_Strategies_StratId",
            //            column: x => x.StratId,
            //            principalTable: "Strategies",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "StrategyVote",
            //    columns: table => new
            //    {
            //        StratId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Vote = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_StrategyVote", x => new { x.StratId, x.UserId });
            //        table.ForeignKey(
            //            name: "FK_StrategyVote_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_StrategyVote_Strategies_StratId",
            //            column: x => x.StratId,
            //            principalTable: "Strategies",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TournamentCSGOMatches",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Team1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Team2Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        Team1Points = table.Column<int>(type: "int", nullable: false),
            //        Team2Points = table.Column<int>(type: "int", nullable: false),
            //        DemoLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        VideoLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NoShow = table.Column<bool>(type: "bit", nullable: false),
            //        StartTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //        TournamentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TournamentCSGOMatches", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_TournamentCSGOMatches_Teams_Team1Id",
            //            column: x => x.Team1Id,
            //            principalTable: "Teams",
            //            principalColumn: "TeamId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_TournamentCSGOMatches_Teams_Team2Id",
            //            column: x => x.Team2Id,
            //            principalTable: "Teams",
            //            principalColumn: "TeamId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_TournamentCSGOMatches_TournamentCSGOGroups_GroupId",
            //            column: x => x.GroupId,
            //            principalTable: "TournamentCSGOGroups",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_TournamentCSGOMatches_Tournaments_TournamentId",
            //            column: x => x.TournamentId,
            //            principalTable: "Tournaments",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TournamentApplications",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TournamentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        DateSubmitted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //        Game = table.Column<int>(type: "int", nullable: false),
            //        Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Hash = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        BattleNetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        State = table.Column<int>(type: "int", nullable: false),
            //        TournamentCSGOGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        TournamentSC2GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TournamentApplications", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_TournamentApplications_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_TournamentApplications_Companies_CompanyId",
            //            column: x => x.CompanyId,
            //            principalTable: "Companies",
            //            principalColumn: "Name",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_TournamentApplications_Teams_TeamId",
            //            column: x => x.TeamId,
            //            principalTable: "Teams",
            //            principalColumn: "TeamId",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_TournamentApplications_TournamentCSGOGroups_TournamentCSGOGroupId",
            //            column: x => x.TournamentCSGOGroupId,
            //            principalTable: "TournamentCSGOGroups",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_TournamentApplications_Tournaments_TournamentId",
            //            column: x => x.TournamentId,
            //            principalTable: "Tournaments",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_TournamentApplications_TournamentSC2Groups_TournamentSC2GroupId",
            //            column: x => x.TournamentSC2GroupId,
            //            principalTable: "TournamentSC2Groups",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TournamentSC2Matches",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Player1Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        Player2Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        Player1Points = table.Column<int>(type: "int", nullable: false),
            //        Player2Points = table.Column<int>(type: "int", nullable: false),
            //        DemoLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        VideoLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NoShow = table.Column<bool>(type: "bit", nullable: false),
            //        StartTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //        TournamentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TournamentSC2Matches", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_TournamentSC2Matches_AspNetUsers_Player1Id",
            //            column: x => x.Player1Id,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_TournamentSC2Matches_AspNetUsers_Player2Id",
            //            column: x => x.Player2Id,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_TournamentSC2Matches_Tournaments_TournamentId",
            //            column: x => x.TournamentId,
            //            principalTable: "Tournaments",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_TournamentSC2Matches_TournamentSC2Groups_GroupId",
            //            column: x => x.GroupId,
            //            principalTable: "TournamentSC2Groups",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TournamentCSGOMatchMaps",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Map = table.Column<int>(type: "int", nullable: false),
            //        CSGOMatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        TeamPickId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        TeamBanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        Team1Score = table.Column<int>(type: "int", nullable: false),
            //        Team2Score = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TournamentCSGOMatchMaps", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_TournamentCSGOMatchMaps_Teams_TeamBanId",
            //            column: x => x.TeamBanId,
            //            principalTable: "Teams",
            //            principalColumn: "TeamId",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_TournamentCSGOMatchMaps_Teams_TeamPickId",
            //            column: x => x.TeamPickId,
            //            principalTable: "Teams",
            //            principalColumn: "TeamId",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_TournamentCSGOMatchMaps_TournamentCSGOMatches_CSGOMatchId",
            //            column: x => x.CSGOMatchId,
            //            principalTable: "TournamentCSGOMatches",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TournamentSC2MatchMaps",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        Map = table.Column<int>(type: "int", nullable: false),
            //        SC2MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        PlayerPickId = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        PlayerBanId = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        WinnerId = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TournamentSC2MatchMaps", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_TournamentSC2MatchMaps_AspNetUsers_PlayerBanId",
            //            column: x => x.PlayerBanId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_TournamentSC2MatchMaps_AspNetUsers_PlayerPickId",
            //            column: x => x.PlayerPickId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_TournamentSC2MatchMaps_TournamentSC2Matches_SC2MatchId",
            //            column: x => x.SC2MatchId,
            //            principalTable: "TournamentSC2Matches",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetRoleClaims_RoleId",
            //    table: "AspNetRoleClaims",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "RoleNameIndex",
            //    table: "AspNetRoles",
            //    column: "NormalizedName",
            //    unique: true,
            //    filter: "[NormalizedName] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserClaims_UserId",
            //    table: "AspNetUserClaims",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserLogins_UserId",
            //    table: "AspNetUserLogins",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserRoles_RoleId",
            //    table: "AspNetUserRoles",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "EmailIndex",
            //    table: "AspNetUsers",
            //    column: "NormalizedEmail");

            //migrationBuilder.CreateIndex(
            //    name: "UserNameIndex",
            //    table: "AspNetUsers",
            //    column: "NormalizedUserName",
            //    unique: true,
            //    filter: "[NormalizedUserName] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Companies_Name",
            //    table: "Companies",
            //    column: "Name",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_JerseyDetails_OrderId",
            //    table: "JerseyDetails",
            //    column: "OrderId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_JerseyOrders_PromoCode",
            //    table: "JerseyOrders",
            //    column: "PromoCode");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Messages_From",
            //    table: "Messages",
            //    column: "From");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Messages_To",
            //    table: "Messages",
            //    column: "To");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PromoCodes_Code",
            //    table: "PromoCodes",
            //    column: "Code",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_PushSubscriptions_userId",
            //    table: "PushSubscriptions",
            //    column: "userId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Strategies_CustomUrl",
            //    table: "Strategies",
            //    column: "CustomUrl",
            //    unique: true,
            //    filter: "[CustomUrl] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Strategies_TeamId",
            //    table: "Strategies",
            //    column: "TeamId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Strategies_UserId",
            //    table: "Strategies",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_StrategyComments_StratId",
            //    table: "StrategyComments",
            //    column: "StratId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_StrategyComments_UserId",
            //    table: "StrategyComments",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_StrategyVote_UserId",
            //    table: "StrategyVote",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TeamApplications_TeamId",
            //    table: "TeamApplications",
            //    column: "TeamId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TeamInvites_InvitedUserId",
            //    table: "TeamInvites",
            //    column: "InvitedUserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TeamInvites_TeamId",
            //    table: "TeamInvites",
            //    column: "TeamId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TeamMembers_UserId",
            //    table: "TeamMembers",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Teams_CustomUrl",
            //    table: "Teams",
            //    column: "CustomUrl",
            //    unique: true,
            //    filter: "[CustomUrl] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentApplications_CompanyId",
            //    table: "TournamentApplications",
            //    column: "CompanyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentApplications_TeamId",
            //    table: "TournamentApplications",
            //    column: "TeamId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentApplications_TournamentCSGOGroupId",
            //    table: "TournamentApplications",
            //    column: "TournamentCSGOGroupId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentApplications_TournamentId",
            //    table: "TournamentApplications",
            //    column: "TournamentId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentApplications_TournamentSC2GroupId",
            //    table: "TournamentApplications",
            //    column: "TournamentSC2GroupId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentApplications_UserId",
            //    table: "TournamentApplications",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentCSGOGroups_TournamentId",
            //    table: "TournamentCSGOGroups",
            //    column: "TournamentId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentCSGOMatches_GroupId",
            //    table: "TournamentCSGOMatches",
            //    column: "GroupId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentCSGOMatches_Team1Id",
            //    table: "TournamentCSGOMatches",
            //    column: "Team1Id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentCSGOMatches_Team2Id",
            //    table: "TournamentCSGOMatches",
            //    column: "Team2Id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentCSGOMatches_TournamentId",
            //    table: "TournamentCSGOMatches",
            //    column: "TournamentId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentCSGOMatchMaps_CSGOMatchId",
            //    table: "TournamentCSGOMatchMaps",
            //    column: "CSGOMatchId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentCSGOMatchMaps_TeamBanId",
            //    table: "TournamentCSGOMatchMaps",
            //    column: "TeamBanId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentCSGOMatchMaps_TeamPickId",
            //    table: "TournamentCSGOMatchMaps",
            //    column: "TeamPickId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentSC2Groups_TournamentId",
            //    table: "TournamentSC2Groups",
            //    column: "TournamentId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentSC2Matches_GroupId",
            //    table: "TournamentSC2Matches",
            //    column: "GroupId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentSC2Matches_Player1Id",
            //    table: "TournamentSC2Matches",
            //    column: "Player1Id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentSC2Matches_Player2Id",
            //    table: "TournamentSC2Matches",
            //    column: "Player2Id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentSC2Matches_TournamentId",
            //    table: "TournamentSC2Matches",
            //    column: "TournamentId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentSC2MatchMaps_PlayerBanId",
            //    table: "TournamentSC2MatchMaps",
            //    column: "PlayerBanId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentSC2MatchMaps_PlayerPickId",
            //    table: "TournamentSC2MatchMaps",
            //    column: "PlayerPickId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TournamentSC2MatchMaps_SC2MatchId",
            //    table: "TournamentSC2MatchMaps",
            //    column: "SC2MatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "AspNetRoleClaims");

            //migrationBuilder.DropTable(
            //    name: "AspNetUserClaims");

            //migrationBuilder.DropTable(
            //    name: "AspNetUserLogins");

            //migrationBuilder.DropTable(
            //    name: "AspNetUserRoles");

            //migrationBuilder.DropTable(
            //    name: "AspNetUserTokens");

            //migrationBuilder.DropTable(
            //    name: "JerseyDetails");

            //migrationBuilder.DropTable(
            //    name: "Messages");

            //migrationBuilder.DropTable(
            //    name: "PushSubscriptions");

            //migrationBuilder.DropTable(
            //    name: "StrategyComments");

            //migrationBuilder.DropTable(
            //    name: "StrategyVote");

            //migrationBuilder.DropTable(
            //    name: "Subscribers");

            //migrationBuilder.DropTable(
            //    name: "TeamApplications");

            //migrationBuilder.DropTable(
            //    name: "TeamInvites");

            //migrationBuilder.DropTable(
            //    name: "TeamMapPool");

            //migrationBuilder.DropTable(
            //    name: "TeamMembers");

            //migrationBuilder.DropTable(
            //    name: "TeamPracticeSchedule");

            //migrationBuilder.DropTable(
            //    name: "TournamentApplications");

            //migrationBuilder.DropTable(
            //    name: "TournamentCSGOMatchMaps");

            //migrationBuilder.DropTable(
            //    name: "TournamentSC2MatchMaps");

            //migrationBuilder.DropTable(
            //    name: "UserAvailabilities");

            //migrationBuilder.DropTable(
            //    name: "UserMapPool");

            //migrationBuilder.DropTable(
            //    name: "AspNetRoles");

            //migrationBuilder.DropTable(
            //    name: "JerseyOrders");

            //migrationBuilder.DropTable(
            //    name: "Strategies");

            //migrationBuilder.DropTable(
            //    name: "Companies");

            //migrationBuilder.DropTable(
            //    name: "TournamentCSGOMatches");

            //migrationBuilder.DropTable(
            //    name: "TournamentSC2Matches");

            //migrationBuilder.DropTable(
            //    name: "PromoCodes");

            //migrationBuilder.DropTable(
            //    name: "Teams");

            //migrationBuilder.DropTable(
            //    name: "TournamentCSGOGroups");

            //migrationBuilder.DropTable(
            //    name: "AspNetUsers");

            //migrationBuilder.DropTable(
            //    name: "TournamentSC2Groups");

            //migrationBuilder.DropTable(
            //    name: "Tournaments");
        }
    }
}
