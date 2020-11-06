using System;
using System.Collections.Generic;

namespace BellumGens.Api.Core.Models
{
    public class TournamentCSGOParticipant : TournamentParticipant
    {
        public TournamentCSGOParticipant(TournamentApplication application, List<TournamentCSGOMatch> matches)
            : base(application)
        {
            TeamId = application.TeamId;
            Team = new CSGOTeamSummaryViewModel(application.Team);
            TournamentCSGOGroupId = application.TournamentCSGOGroupId;
            if (matches != null)
            {
                foreach (TournamentCSGOMatch match in matches)
                {
                    if (match.Team1Id == Team.TeamId)
                    {
                        TeamPoints += match.Team1Points;
                        foreach (CSGOMatchMap map in match.Maps)
                        {
                            RoundDifference += map.Team1Score;
                            RoundDifference -= map.Team2Score;
                        }
                        if (match.NoShow && match.Team1Points > match.Team2Points)
                            RoundDifference += 8;

                        if (match.Team1Points + match.Team2Points > 0)
                        {
                            Matches++;
                            if (match.Team1Points > match.Team2Points)
                            {
                                if (match.Team1Points + match.Team2Points > 30)
                                    OTWins++;
                                else 
                                    Wins++;
                            }
                            else
                            {
                                if (match.Team1Points + match.Team2Points > 30)
                                    OTLosses++;
                                else
                                    Losses++;
                            }
                        }
                    }
                    else if (match.Team2Id == Team.TeamId)
                    {
                        TeamPoints += match.Team2Points;
                        foreach (CSGOMatchMap map in match.Maps)
                        {
                            RoundDifference -= map.Team1Score;
                            RoundDifference += map.Team2Score;
                        }
                        if (match.NoShow && match.Team2Points > match.Team1Points)
                            RoundDifference += 8;
                            
                        if (match.Team1Points + match.Team2Points > 0)
                        {
                            Matches++;
                            if (match.Team2Points > match.Team1Points)
                            {

                                if (match.Team1Points + match.Team2Points > 30)
                                    OTWins++;
                                else
                                    Wins++;
                            }
                            else
                            {
                                if (match.Team1Points + match.Team2Points > 30)
                                    OTLosses++;
                                else
                                    Losses++;
                            }
                        }
                    }
                }
            }
        }

        public int RoundDifference { get; set; } = 0;
        public int OTWins { get; set; } = 0;
        public int OTLosses { get; set; } = 0;
        public Guid? TeamId { get; set; }
        public Guid? TournamentCSGOGroupId { get; set; }
        public int TeamPoints { get; set; } = 0;
        public CSGOTeamSummaryViewModel Team { get; set; }
    }
}