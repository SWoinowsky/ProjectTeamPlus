using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class Status
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Competition> Competitions { get; } = new List<Competition>();
}
