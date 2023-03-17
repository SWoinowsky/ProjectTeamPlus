using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class Competition
{
    public int Id { get; set; }

    public int GameId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual Game Game { get; set; } = null!;

    public bool Equals( Competition compIn )
    {
        if( Id == compIn.Id )
            if( GameId == compIn.GameId )
                if( StartDate == compIn.StartDate )
                    if( EndDate == compIn.EndDate )
                        return true;

        return false;
    }
}
