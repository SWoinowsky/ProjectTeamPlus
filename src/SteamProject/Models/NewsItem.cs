using System;
using System.Collections.Generic;

namespace SteamProject.Models;

public partial class NewsItem
{
    public string Gid { get; set; } = null!;

    public string? Title { get; set; }

    public string? Url { get; set; }

    public bool? IsExternalUrl { get; set; }

    public string? Author { get; set; }

    public string? Contents { get; set; }

    public string? FeedLabel { get; set; }

    public int? Date { get; set; }

    public DateTime? DateTime { get; set; }

    public string? FeedName { get; set; }

    public int? FeedType { get; set; }

    public int? AppId { get; set; }

    public virtual AppNews? App { get; set; }

    public virtual ICollection<NewsItemTag> NewsItemTags { get; } = new List<NewsItemTag>();
}
