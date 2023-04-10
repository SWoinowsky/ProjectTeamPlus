using System;
using System.Collections.Generic;
using SteamProject.Models.DTO;

namespace SteamProject.Models;

public partial class UserAchievement
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public int AchievementId { get; set; }

    public bool Achieved { get; set; }

    public DateTime? UnlockTime { get; set; }

    public virtual GameAchievement Achievement { get; set; } = null!;

    public virtual User Owner { get; set; } = null!;

    public UserAchievement(){}

    public UserAchievement( GameAchievement gameAchIn, Achievement achPOCO )
    {
        AchievementId = gameAchIn.Id;
        Achieved = ( achPOCO.achieved == 1 );
        if( Achieved )
            UnlockTime = getDateFromUnix( achPOCO.unlocktime );
        Achievement = gameAchIn;
    }

    public UserAchievement( List<GameAchievement> gameAchIn, List<Achievement> achPOCO )
    {
        foreach( var poco in achPOCO )
        {
            
        }
    }

    DateTime getDateFromUnix( int? unixTime )
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds( unixTime ?? 0 ).ToLocalTime();
        return dateTime;
    }
}
