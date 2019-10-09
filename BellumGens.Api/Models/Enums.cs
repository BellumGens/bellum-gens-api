using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Models
{
    public enum Game
    {
        CSGO,
        StarCraft2
    }

    public enum NotificationState
    {
        NotSeen,
        Seen,
        Rejected,
        Accepted
    }

    public enum Side
    {
        TSide,
        CTSide
    }

    public enum PlaystyleRole
    {
        NotSet,
        IGL,
        Awper,
        EntryFragger,
        Support,
        Lurker
    }

    public enum VoteDirection
    {
        Up,
        Down
    }

    public enum CSGOMaps
    {
        Cache,
        Dust2,
        Inferno,
        Mirage,
        Nuke,
        Overpass,
        Train,
        Vertigo,
        Cobblestone
    }

    public enum TShirtSize
    {
        XS,
        S,
        M,
        L,
        XL,
        XXL
    }
}