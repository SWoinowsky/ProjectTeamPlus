using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class UserBadge
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int BadgeId { get; set; }

    public virtual Badge Badge { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
