using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class Badge
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public byte[] Image { get; set; } = null!;

    public virtual ICollection<UserBadge> UserBadges { get; } = new List<UserBadge>();
}
