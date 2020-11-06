namespace BellumGens.Api.Core.Models
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

    public enum CSGOMap
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

    public enum SC2Map
    {
        TritonLE,
        EphemeronLE,
        WorldofSleepersLE,
        ZenLE,
        SimulacrumLE,
        NightshadeLE,
        EternalEmpireLE,
        GoldenWallLE,
        PurityAndIndustryLE,
        EverDreamLE,
        SubmarineLE,
        DeathauraLE,
        PillarsofGoldLE,
        OxideLE,
        LightshadeLE,
        RomanticideLE,
        JagannathaLE,
        IceAndChromeLE
    }

    public enum TournamentApplicationState
    {
        Pending,
        Confirmed
    }

    public enum JerseyCut
    {
        Male,
        Female
    }

    public enum JerseySize
    {
        XS,
        S,
        M,
        L,
        XL,
        XXL,
        XXXL
    }
}