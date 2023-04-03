using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class AppNews
{
    public int AppId { get; set; }

    public int? Count { get; set; }

    public virtual ICollection<NewsItem> NewsItems { get; } = new List<NewsItem>();
}
