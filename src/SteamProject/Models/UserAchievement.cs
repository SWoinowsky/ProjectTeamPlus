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

    public UserAchievement GetUserAchievementFromAPICall( GameAchievement gameAchIn, List<Achievement> achPOCO )
    {
        var pocoNeeded = new Achievement();
        pocoNeeded = achPOCO.Where( a => a.apiname == gameAchIn.ApiName ).FirstOrDefault();

        var achievementNeeded = new UserAchievement( gameAchIn, pocoNeeded );
        
        return achievementNeeded;
    }

    DateTime getDateFromUnix( int? unixTime )
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds( unixTime ?? 0 ).ToLocalTime();
        return dateTime;
    }

    public bool AchievedWithinWindow( Competition comp )
    {
        if( UnlockTime != null )
        {
            var startTime = new DateTime();
            startTime = comp.StartDate;

            var endTime = new DateTime();
            endTime = comp.EndDate;

            var startToUnix = new long();
            startToUnix = ((DateTimeOffset)startTime).ToUnixTimeSeconds();

            var endToUnix = new long();
            endToUnix = ((DateTimeOffset)endTime).ToUnixTimeSeconds();

            var thisTimeToUnix = new long();
            thisTimeToUnix = ((DateTimeOffset)UnlockTime).ToUnixTimeSeconds();

            if( startToUnix < thisTimeToUnix )
                if( thisTimeToUnix < endToUnix )
                    return true;
        }
        return false;
    }


}
