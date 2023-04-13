using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class BlackList
{
    public int Id { get; set; }

    public string? SteamId { get; set; }
}
