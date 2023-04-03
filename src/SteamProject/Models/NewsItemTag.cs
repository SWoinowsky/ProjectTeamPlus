using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class NewsItemTag
{
    public int Id { get; set; }

    public string? Gid { get; set; }

    public string? Tag { get; set; }

    public virtual NewsItem? GidNavigation { get; set; }
}
