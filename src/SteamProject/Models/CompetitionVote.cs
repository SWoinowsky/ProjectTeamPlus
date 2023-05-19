using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class CompetitionVote
{
    public int Id { get; set; }

    public int CompetitionId { get; set; }

    public int UserId { get; set; }

    public bool WantsToPlayAgain { get; set; }

    public virtual Competition Competition { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
